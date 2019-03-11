using System;

namespace IDeliverable.Controls.Uwp.TimeSpanPicker
{
	interface ITimeSpanEditor
	{
		TimePrecision Precision { get; set; }

		TimeSpan MinValue { get; set; }

		TimeSpan MaxValue { get; set; }

		TimeSpan Value { get; set; }

		event EventHandler<TimeSpanChangedEventArgs> ValueChanged;

		TimeIncrement MinuteIncrement { get; set; }

		TimeIncrement SecondIncrement { get; set; }

		string DaysLabel { get; set; }

		string HoursLabel { get; set; }

		string MinutesLabel { get; set; }

		string SecondsLabel { get; set; }
	}

	internal static class ITimeSpanEditorExtensions
	{
		public static void AdjustPrecision(this ITimeSpanEditor target, TimeSpan newMaxValue)
		{
			if (newMaxValue < TimeSpan.FromMinutes(1) && target.Precision == TimePrecision.Minutes)
				target.Precision = TimePrecision.Seconds;
			else if (newMaxValue < TimeSpan.FromHours(1) && target.Precision == TimePrecision.Hours)
				target.Precision = TimePrecision.Minutes;
			else if (newMaxValue < TimeSpan.FromDays(1) && target.Precision == TimePrecision.Days)
				target.Precision = TimePrecision.Hours;
		}

		public static void AdjustMinValue(this ITimeSpanEditor target, TimeSpan newMaxValue)
		{
			if (target.MinValue > newMaxValue)
				target.MinValue = newMaxValue;
		}

		public static void AdjustMaxValue(this ITimeSpanEditor target, TimeSpan newMinValue)
		{
			if (target.MaxValue < newMinValue)
				target.MaxValue = newMinValue;
		}

		public static void AdjustMaxValue(this ITimeSpanEditor target, TimePrecision newPrecision)
		{
			if (newPrecision == TimePrecision.Days && target.MaxValue < TimeSpan.FromDays(1))
				target.MaxValue = TimeSpan.FromDays(1);
			else if (newPrecision == TimePrecision.Hours && target.MaxValue < TimeSpan.FromHours(1))
				target.MaxValue = TimeSpan.FromHours(1);
			else if (newPrecision == TimePrecision.Minutes && target.MaxValue < TimeSpan.FromMinutes(1))
				target.MaxValue = TimeSpan.FromMinutes(1);
		}

		public static void AdjustValue(this ITimeSpanEditor target)
		{
			// Truncate to nearest increment for minutes and seconds.
			var excessMinutes = target.Value.Minutes % (int)target.MinuteIncrement;
			var excessSeconds = target.Value.Seconds % (int)target.SecondIncrement;
			target.Value = new TimeSpan(target.Value.Days, target.Value.Hours, target.Value.Minutes - excessMinutes, target.Value.Seconds - excessSeconds);
				
			// Truncate to min and max values.
			if (target.Value < target.MinValue)
				target.Value = target.MinValue;
			if (target.Value > target.MaxValue)
				target.Value = target.MaxValue;
		}
	}
}
