using System;
using System.Threading;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

// Foreground brush should affect rendered ListViewItems - but how?

// When clicking a non-selected item, the resulting ScrollToSelectedItem() works as
// expected, but when touching the same item, there is no scroll, instead the
// ScrollViewer just jumps to the item. Probably this is because the touch engages
// the DirectManipulation mechanism of the ScrollViewer and this somehow interferes
// with the programmatically invoked ChangeView(). A possible workaround might be to
// somehow catch the touch events on the ListViewItem elements and not let them through
// to the underlying ScrollViewer.

namespace IDeliverable.Controls.Uwp.ScrollSelector
{
	[TemplateVisualState(Name = mPointerUiHiddenStateName, GroupName = mPointerUiStatesGroupName)]
	[TemplateVisualState(Name = mPointerUiVisibleStateName, GroupName = mPointerUiStatesGroupName)]
	[TemplatePart(Name = "HeaderContentPresenter", Type = typeof(ContentPresenter))]
	[TemplatePart(Name = "ItemsScrollViewer", Type = typeof(ScrollViewer))]
	[TemplatePart(Name = "Presenter", Type = typeof(ItemsPresenter))]
	[TemplatePart(Name = "UpButton", Type = typeof(Button))]
	[TemplatePart(Name = "DownButton", Type = typeof(Button))]
	[TemplatePart(Name = "SelectionHighlight", Type = typeof(Rectangle))]
	public sealed class ScrollSelector : ListView
	{
		private const string mPointerUiStatesGroupName = "PointerUiStates";
		private const string mPointerUiHiddenStateName = "PointerUiHidden";
		private const string mPointerUiVisibleStateName = "PointerUiVisible";

		public static DependencyProperty SelectionHighlightVisibilityProperty { get; } =
			DependencyProperty.Register(
				nameof(SelectionHighlightVisibility),
				typeof(Visibility),
				typeof(ScrollSelector),
				new PropertyMetadata(
					Visibility.Visible,
					(d, e) =>
					{
						var selector = (ScrollSelector)d;
						selector.ScrollToSelectedItem(); // This method will also show/hide selection highlight.
					}));

		public ScrollSelector()
		{
			DefaultStyleKey = typeof(ScrollSelector);
			ItemContainerStyleSelector = new ScrollSelectorItemContainerStyleSelector(this);

			SelectionChanged += ScrollSelector_SelectionChanged;    
			SizeChanged += ScrollSelector_SizeChanged;
			Loaded += ScrollSelector_Loaded;

			mScrollDelayTimer = new Timer(ScrollDelayTimerTick, null, Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
		}

		private readonly Timer mScrollDelayTimer;

		private ContentPresenter mHeaderContentPresenter;
		private ScrollViewer mItemsScrollViewer;
		private ItemsPresenter mPresenter;
		private Button mUpButton;
		private Button mDownButton;
		private Rectangle mSelectionHighlight;

		private bool mTemplateIsApplied = false;
		private bool mIsScrolling = false;

		public Visibility SelectionHighlightVisibility
		{
			get => (Visibility)GetValue(SelectionHighlightVisibilityProperty);
			set => SetValue(SelectionHighlightVisibilityProperty, value);
		}

		protected override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			mHeaderContentPresenter = GetTemplateChild("HeaderContentPresenter") as ContentPresenter;
			mItemsScrollViewer = GetTemplateChild("ItemsScrollViewer") as ScrollViewer;
			mPresenter = GetTemplateChild("Presenter") as ItemsPresenter;
			mUpButton = GetTemplateChild("UpButton") as Button;
			mDownButton = GetTemplateChild("DownButton") as Button;
			mSelectionHighlight = GetTemplateChild("SelectionHighlight") as Rectangle;

			mTemplateIsApplied = true;

			mItemsScrollViewer.DirectManipulationStarted += ItemsScrollViewer_DirectManipulationStarted;
			mItemsScrollViewer.DirectManipulationCompleted += ItemsScrollViewer_DirectManipulationCompleted;
			mItemsScrollViewer.ViewChanged += ItemsScrollViewer_ViewChanged;
			mUpButton.Click += (sender, e) => SelectPreviousItem();
			mDownButton.Click += (sender, e) => SelectNextItem();

			RegisterPropertyChangedCallback(HeaderProperty, (sender, e) => ConfigureHeaderVisibility());

			//var scrollPadding = Height / 2;
			//mPresenter.Padding = new Thickness(0, scrollPadding, 0, scrollPadding);

			// Unless we are in the designer where everything should be visible, always
			// start the control assuming pointer UI should not be visible; if mouse or
			// pen presence is detected the visual state will change.
			if (!DesignMode.DesignModeEnabled)
				VisualStateManager.GoToState(this, mPointerUiHiddenStateName, useTransitions: false);
		}

		protected override void OnItemsChanged(object e)
		{
			ConfigureScrollPadding();
			ScrollToSelectedItem();
		}

		protected override void OnKeyDown(KeyRoutedEventArgs e)
		{
			if (e.Key == Windows.System.VirtualKey.Up)
			{
				SelectPreviousItem();
				e.Handled = true;
			}
			else if (e.Key == Windows.System.VirtualKey.Down)
			{
				SelectNextItem();
				e.Handled = true;
			}
		}

		protected override void OnPointerEntered(PointerRoutedEventArgs e)
		{
			base.OnPointerEntered(e);
			VisualStateManager.GoToState(this, mPointerUiVisibleStateName, useTransitions: true);
		}

		protected override void OnPointerExited(PointerRoutedEventArgs e)
		{
			base.OnPointerExited(e);
			VisualStateManager.GoToState(this, mPointerUiHiddenStateName, useTransitions: true);
		}

		private void ScrollSelector_Loaded(object sender, RoutedEventArgs e)
		{
			ConfigureHeaderVisibility();
			ConfigureScrollPadding();
			ScrollToSelectedItem();
		}

		private void ScrollSelector_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			ConfigureScrollPadding();
			ScrollToSelectedItem();
		}

		private void ScrollSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ScrollToSelectedItem();
		}

		private void ItemsScrollViewer_DirectManipulationStarted(object sender, object e)
		{
			mIsScrolling = true;
		}

		private void ItemsScrollViewer_DirectManipulationCompleted(object sender, object e)
		{
			mIsScrolling = false;
			SelectScrolledItem();
		}

		private void ItemsScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
		{
			// If we are currently scrolling (i.e. direct manipulation is currently
			// happening on ItemsScrollViewer) then we'll hold off on syncing scroll
			// position to selection until after the scrolling has completed.
			if (!mIsScrolling)
				mScrollDelayTimer.Change(TimeSpan.FromMilliseconds(100), Timeout.InfiniteTimeSpan);
		}

		private void ScrollDelayTimerTick(object state)
		{
			_ = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, SelectScrolledItem);
		}

		private void ConfigureHeaderVisibility()
		{
			if (!mTemplateIsApplied)
				return;

			if (Header == null || (Header is string && (string)Header == ""))
				mHeaderContentPresenter.Visibility = Visibility.Collapsed;
			else
				mHeaderContentPresenter.Visibility = Visibility.Visible;
		}

		private void ConfigureScrollPadding()
		{
			if (!mTemplateIsApplied)
				return;

			var itemsPanel = GetFirstChildOfType<ScrollStackPanel>(this);
			if (itemsPanel != null)
			{
				var verticalPadding = ActualHeight / 2;
				itemsPanel.Padding = new Thickness(0, verticalPadding, 0, verticalPadding);
				itemsPanel.UpdateLayout();

			}
		}

		private void ScrollToSelectedItem()
		{
			if (!mTemplateIsApplied)
				return;

			if (SelectedItem != null)
			{
				var itemContainer = ContainerFromItem(SelectedItem) as ListViewItem;
				if (itemContainer == null)
					return;
				var offsetToTop = itemContainer.TransformToVisual(mPresenter).TransformPoint(new Point(0, 0)).Y;
				var offsetToCenter = offsetToTop + itemContainer.ActualHeight / 2;
				var scrollViewerCenter = mItemsScrollViewer.ActualHeight / 2;
				mItemsScrollViewer.ChangeView(null, offsetToCenter - scrollViewerCenter, null);

				mSelectionHighlight.Height = itemContainer.ActualHeight;
			}

			if (DesignMode.DesignModeEnabled || SelectedItem != null)
				mSelectionHighlight.Visibility = SelectionHighlightVisibility;
			else
				mSelectionHighlight.Visibility = Visibility.Collapsed;
		}

		private void SelectScrolledItem()
		{
			if (!mTemplateIsApplied)
				return;

			foreach (var item in Items)
			{
				var itemContainer = ContainerFromItem(item) as ListViewItem;
				if (itemContainer == null)
					continue;
				var offsetToTop = itemContainer.TransformToVisual(mItemsScrollViewer).TransformPoint(new Point(0, 0)).Y;
				var offsetToBottom = offsetToTop + itemContainer.ActualHeight;
				var scrollViewerCenter = mItemsScrollViewer.ActualHeight / 2;
				if (offsetToTop < scrollViewerCenter && offsetToBottom > scrollViewerCenter)
				{
					SelectedItem = item;
					break;
				}
			}
		}

		private void SelectPreviousItem()
		{
			if (SelectedIndex > 0)
				SelectedIndex--;
		}

		private void SelectNextItem()
		{
			if (SelectedIndex < Items.Count - 1)
				SelectedIndex++;
		}

		private T GetFirstChildOfType<T>(DependencyObject parent) where T : DependencyObject
		{
			if (parent == null)
				return null;

			for (var i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
			{
				var child = VisualTreeHelper.GetChild(parent, i);
				var result = (child as T) ?? GetFirstChildOfType<T>(child);
				if (result != null)
					return result;
			}

			return null;
		}
	}
}