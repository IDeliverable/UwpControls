using IDeliverable.Controls.Uwp.TimeSpanPicker;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace IDeliverable.Controls.TestHost
{
	partial class TimeSpanEditorViewModel : INotifyPropertyChanged
	{
		private TimePrecision mPrecision = TimePrecision.Seconds;
		private TimeSpan mMinValue = TimeSpan.Zero;
		private TimeSpan mMaxValue = TimeSpan.FromDays(7);
		private TimeSpan mValue = new TimeSpan(12, 15, 30);
		private TimeIncrement mMinuteIncrement = TimeIncrement.One;
		private TimeIncrement mSecondIncrement = TimeIncrement.Five;
		private string mDaysLabel = "D";
		private string mHoursLabel = "H";
		private string mMinutesLabel = "M";
		private string mSecondsLabel = "S";
		private bool mIsEnabled = true;

		public EnumItem<TimePrecision>[] TimePrecisionEnumItems { get; } = EnumItem<TimePrecision>.GetAll();
		public EnumItem<TimeIncrement>[] TimeIncrementEnumItems { get; } = EnumItem<TimeIncrement>.GetAll();

		public TimePrecision Precision
		{
			get => mPrecision;
			set
			{
				if (SetValue(ref mPrecision, value))
				{
					OnPropertyChanged();
					OnPropertyChanged(nameof(PrecisionInt32));
				}
			}
		}

		public int PrecisionInt32
		{
			get => (int)mPrecision;
			set => Precision = (TimePrecision)value;
		}

		public TimeSpan MinValue
		{
			get => mMinValue;
			set
			{
				if (SetValue(ref mMinValue, value))
					OnPropertyChanged();
			}
		}

		public TimeSpan MaxValue
		{
			get => mMaxValue;
			set
			{
				if (SetValue(ref mMaxValue, value))
					OnPropertyChanged();
			}
		}

		public TimeSpan Value
		{
			get => mValue;
			set
			{
				var oldValue = mValue;
				if (SetValue(ref mValue, value))
				{
					ValueChanged?.Invoke(this, new TimeSpanChangedEventArgs(oldValue, value));
					OnPropertyChanged();
				}
			}
		}

		public event EventHandler<TimeSpanChangedEventArgs> ValueChanged;

		public TimeIncrement MinuteIncrement
		{
			get => mMinuteIncrement;
			set
			{
				if (SetValue(ref mMinuteIncrement, value))
				{
					OnPropertyChanged();
					OnPropertyChanged(nameof(MinuteIncrementInt32));
				}
			}
		}

		public int MinuteIncrementInt32
		{
			get => (int)mMinuteIncrement;
			set => MinuteIncrement = (TimeIncrement)value;
		}

		public TimeIncrement SecondIncrement
		{
			get => mSecondIncrement;
			set
			{
				if (SetValue(ref mSecondIncrement, value))
				{
					OnPropertyChanged();
					OnPropertyChanged(nameof(SecondIncrementInt32));
				}
			}
		}

		public int SecondIncrementInt32
		{
			get => (int)mSecondIncrement;
			set => SecondIncrement = (TimeIncrement)value;
		}

		public string DaysLabel
		{
			get => mDaysLabel;
			set
			{
				if (SetValue(ref mDaysLabel, value))
					OnPropertyChanged();
			}
		}

		public string HoursLabel
		{
			get => mHoursLabel;
			set
			{
				if (SetValue(ref mHoursLabel, value))
					OnPropertyChanged();
			}
		}

		public string MinutesLabel
		{
			get => mMinutesLabel;
			set
			{
				if (SetValue(ref mMinutesLabel, value))
					OnPropertyChanged();
			}
		}

		public string SecondsLabel
		{
			get => mSecondsLabel;
			set
			{
				if (SetValue(ref mSecondsLabel, value))
					OnPropertyChanged();
			}
		}

		public bool IsEnabled
		{
			get => mIsEnabled;
			set
			{
				if (SetValue(ref mIsEnabled, value))
					OnPropertyChanged();
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private bool SetValue<T>(ref T target, T newValue)
		{
			if (!newValue.Equals(target))
			{
				target = newValue;
				return true;
			}

			return false;
		}
	}
}
