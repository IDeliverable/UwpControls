using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace IDeliverable.Controls.Uwp.TimeSpanPicker
{
	public class TimeSpanPickerFlyout : FlyoutBase
	{
		public TimeSpanPicker Picker { get; set; }
		public double Width { get; set; }
		public double Height { get; set; }
		public TimeSpanEditor Editor { get; private set; }

		protected override Control CreatePresenter()
		{
			Editor = new TimeSpanEditor()
			{
				MinWidth = Width,
				MaxHeight = Height,
				Precision = Picker.Precision,
				MinValue = Picker.MinValue,
				MaxValue = Picker.MaxValue,
				Value = Picker.Value,
				MinuteIncrement = Picker.MinuteIncrement,
				SecondIncrement = Picker.SecondIncrement,
				DaysLabel = Picker.DaysLabel,
				HoursLabel = Picker.HoursLabel,
				MinutesLabel = Picker.MinutesLabel,
				SecondsLabel = Picker.SecondsLabel
			};
			
			return Editor;
		}
	}
}
