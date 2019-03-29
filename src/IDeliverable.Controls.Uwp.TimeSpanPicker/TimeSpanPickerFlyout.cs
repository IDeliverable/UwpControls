using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace IDeliverable.Controls.Uwp.TimeSpanPicker
{
	public class TimeSpanPickerFlyout : FlyoutBase
	{
		public TimeSpanPickerFlyout(TimeSpanPicker picker)
		{
			mPicker = picker;
			mEditor = new TimeSpanEditor();

			Opening += Flyout_Opening;
			Closing += Flyout_Closing;
		}

		private void Flyout_Closing(FlyoutBase sender, FlyoutBaseClosingEventArgs args)
		{
			var oldValue = SelectedValue;
			SelectedValue = mEditor.Value;
			ValueSelected?.Invoke(this, new TimeSpanChangedEventArgs(oldValue, mEditor.Value));
		}

		private void Flyout_Opening(object sender, object e)
		{
			mEditor.MinWidth = Width;
			mEditor.MaxHeight = Height;
			mEditor.Precision = mPicker.Precision;
			mEditor.MinValue = mPicker.MinValue;
			mEditor.MaxValue = mPicker.MaxValue;
			mEditor.Value = mPicker.Value;
			mEditor.MinuteIncrement = mPicker.MinuteIncrement;
			mEditor.SecondIncrement = mPicker.SecondIncrement;
			mEditor.DaysLabel = mPicker.DaysLabel;
			mEditor.HoursLabel = mPicker.HoursLabel;
			mEditor.MinutesLabel = mPicker.MinutesLabel;
			mEditor.SecondsLabel = mPicker.SecondsLabel;
		}

		private readonly TimeSpanPicker mPicker;
		private readonly TimeSpanEditor mEditor;

		public TimeSpan SelectedValue { get; private set; }
		public double Width { get; set; }
		public double Height { get; set; }

		public event EventHandler<TimeSpanChangedEventArgs> ValueSelected;

		protected override Control CreatePresenter()
		{
			return mEditor;
		}
	}
}
