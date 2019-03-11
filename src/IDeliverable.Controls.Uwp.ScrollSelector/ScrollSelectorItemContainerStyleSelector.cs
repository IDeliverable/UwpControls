using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace IDeliverable.Controls.Uwp.ScrollSelector
{
	/// <summary>
	/// Dynamically selects an ItemContainerStyle resource to use for a <see cref="ScrollSelector"/> depending on
	/// the value of its <c>HorizontalContentAlignment</c> property.
	/// </summary>
	class ScrollSelectorItemContainerStyleSelector : StyleSelector
	{
		// TODO: There should be a cleaner way to accomplish this, directly in the XAML, but I have not found one.
		// ListViewItems are styled by assigning a style resource to the ItemContainerStyle property of a ListView
		// derived class, and it's not possible to use TemplateBinding markup directive in styles (only in control
		// templates).

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
