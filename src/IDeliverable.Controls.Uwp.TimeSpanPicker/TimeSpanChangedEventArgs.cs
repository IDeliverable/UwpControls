using System;

namespace IDeliverable.Controls.Uwp.TimeSpanPicker
{
	public class TimeSpanChangedEventArgs
	{
		public TimeSpanChangedEventArgs(TimeSpan oldValue, TimeSpan newValue)
		{
			OldValue = oldValue;
			NewValue = newValue;
		}

		public TimeSpan OldValue { get; }
		public TimeSpan NewValue { get; }
	}
}
