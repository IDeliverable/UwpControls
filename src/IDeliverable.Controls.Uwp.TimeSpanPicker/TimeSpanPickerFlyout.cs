using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace IDeliverable.Controls.Uwp.TimeSpanPicker
{
	public class TimeSpanPickerFlyout : FlyoutBase
	{
		public double Width { get; set; }
		public double Height { get; set; }

		protected override Control CreatePresenter()
		{
			var presenter = new TimeSpanEditor()
			{
				MinWidth = Width,
				MaxHeight = Height
			};
			
			return presenter;
		}
	}
}
