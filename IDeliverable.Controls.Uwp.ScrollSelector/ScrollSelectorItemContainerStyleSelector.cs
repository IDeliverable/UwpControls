using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace IDeliverable.Controls.Uwp.ScrollSelector
{
	public class ScrollSelectorItemContainerStyleSelector : StyleSelector
	{
		public ScrollSelectorItemContainerStyleSelector(ScrollSelector scrollSelector)
		{
			mScrollSelector = scrollSelector;
			mResources = new ResourceDictionary() { Source = new System.Uri("ms-appx:///IDeliverable.Controls.Uwp.ScrollSelector/Themes/Generic.xaml") };
		}

		private readonly ScrollSelector mScrollSelector;
		private readonly ResourceDictionary mResources;

		protected override Style SelectStyleCore(object item, DependencyObject container)
		{
			switch (mScrollSelector.HorizontalContentAlignment)
			{
				case HorizontalAlignment.Left:
					return (Style)mResources["LeftScrollSelectorItemContainerStyle"];
				case HorizontalAlignment.Center:
					return (Style)mResources["CenterScrollSelectorItemContainerStyle"];
				case HorizontalAlignment.Right:
					return (Style)mResources["RightScrollSelectorItemContainerStyle"];
				default:
					return (Style)mResources["ScrollSelectorItemContainerStyle"];
			}
		}
	}
}
