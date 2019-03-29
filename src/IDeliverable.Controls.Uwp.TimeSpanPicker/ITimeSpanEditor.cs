using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace IDeliverable.Controls.Uwp.TimeSpanPicker
{
	public interface ITimeSpanEditor
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

	static class ITimeSpanEditorExtensions
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

		public static void ConfigureComponents<TElement>(this ITimeSpanEditor target, (ColumnDefinition Column, TElement Element)[] components, Action<TElement, bool> setBorderFunc) where TElement : UIElement
		{
			if (components.Length != 4)
				throw new ArgumentException("Compenent array must contain exactly 4 items.", nameof(components));

			// Determine visibility of day, hour, minute and second components, based on the
			// configured MaxValue and Precision properties of the editor.

			var daysColumn = components[0].Column;
			var daysElement = components[0].Element;
			var hoursColumn = components[1].Column;
			var hoursElement = components[1].Element;
			var minutesColumn = components[2].Column;
			var minutesElement = components[2].Element;
			var secondsColumn = components[3].Column;
			var secondsElement = components[3].Element;

			if (target.MaxValue >= TimeSpan.FromDays(1))
			{
				daysColumn.Width = new GridLength(1, GridUnitType.Star);
				daysElement.Visibility = Visibility.Visible;
			}
			else
			{
				daysColumn.Width = new GridLength(0, GridUnitType.Star);
				daysElement.Visibility = Visibility.Collapsed;
			}

			if (target.MaxValue >= TimeSpan.FromHours(1) && target.Precision >= TimePrecision.Hours)
			{
				hoursColumn.Width = new GridLength(1, GridUnitType.Star);
				hoursElement.Visibility = Visibility.Visible;
			}
			else
			{
				hoursColumn.Width = new GridLength(0, GridUnitType.Star);
				hoursElement.Visibility = Visibility.Collapsed;
			}

			if (target.MaxValue >= TimeSpan.FromMinutes(1) && target.Precision >= TimePrecision.Minutes)
			{
				minutesColumn.Width = new GridLength(1, GridUnitType.Star);
				minutesElement.Visibility = Visibility.Visible;
			}
			else
			{
				minutesColumn.Width = new GridLength(0, GridUnitType.Star);
				minutesElement.Visibility = Visibility.Collapsed;
			}

			if (target.Precision >= TimePrecision.Seconds)
			{
				secondsColumn.Width = new GridLength(1, GridUnitType.Star);
				secondsElement.Visibility = Visibility.Visible;
			}
			else
			{
				secondsColumn.Width = new GridLength(0, GridUnitType.Star);
				secondsElement.Visibility = Visibility.Collapsed;
			}

			// Show a left border on all elements except the left-most visible one.
			var firstVisibleElementFound = false;
			foreach (var (_, element) in components)
			{
				setBorderFunc(element, true);
				if (!firstVisibleElementFound && element.Visibility == Visibility.Visible)
				{
					setBorderFunc(element, false);
					firstVisibleElementFound = true;
				}
			}
		}
	}
}
