using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace IDeliverable.Controls.Uwp.TimeSpanPicker
{
	public class TimeSpanPicker : Control, ITimeSpanEditor
	{
		public static DependencyProperty MinValueProperty { get; } =
			DependencyProperty.Register(
				nameof(MinValue),
				typeof(TimeSpan),
				typeof(TimeSpanPicker),
				new PropertyMetadata(
					TimeSpan.Zero,
					(d, e) =>
					{
						var editor = (TimeSpanPicker)d;
						var newValue = (TimeSpan)e.NewValue;
						editor.AdjustMaxValue(newValue);
						editor.AdjustValue();
					}));

		public static DependencyProperty MaxValueProperty { get; } =
			DependencyProperty.Register(
				nameof(MaxValue),
				typeof(TimeSpan),
				typeof(TimeSpanPicker),
				new PropertyMetadata(
					TimeSpan.FromDays(90), // TODO: Should be TimeSpan.MaxValue perhaps?
					(d, e) =>
					{
						var editor = (TimeSpanPicker)d;
						var newValue = (TimeSpan)e.NewValue;
						editor.AdjustMinValue(newValue);
						editor.AdjustPrecision(newValue);
						editor.AdjustValue();
					}));

		public static DependencyProperty ValueProperty { get; } =
			DependencyProperty.Register(
				nameof(Value),
				typeof(TimeSpan),
				typeof(TimeSpanPicker),
				new PropertyMetadata(
					TimeSpan.Zero,
					(d, e) =>
					{
						var editor = (TimeSpanPicker)d;
						var newValue = (TimeSpan)e.NewValue;
						editor.AdjustValue();
						editor.ConfigureLabels();
					}));

		public static DependencyProperty MinuteIncrementProperty { get; } =
			DependencyProperty.Register(
				nameof(MinuteIncrement),
				typeof(TimeIncrement),
				typeof(TimeSpanPicker),
				new PropertyMetadata(
					TimeIncrement.One));

		public static DependencyProperty SecondIncrementProperty { get; } =
			DependencyProperty.Register(
				nameof(SecondIncrement),
				typeof(TimeIncrement),
				typeof(TimeSpanPicker),
				new PropertyMetadata(
					TimeIncrement.One));

		public static DependencyProperty PrecisionProperty { get; } =
			DependencyProperty.Register(
				nameof(Precision),
				typeof(TimePrecision),
				typeof(TimeSpanPicker),
				new PropertyMetadata(
					TimePrecision.Seconds,
					(d, e) =>
					{
						var editor = (TimeSpanPicker)d;
						var newValue = (TimePrecision)e.NewValue;
						editor.AdjustMaxValue(newValue);
						editor.ConfigureLabels();
					}));

		public static DependencyProperty DaysLabelProperty { get; } =
			DependencyProperty.Register(
				nameof(DaysLabel),
				typeof(string),
				typeof(TimeSpanPicker),
				new PropertyMetadata("d"));

		public static DependencyProperty HoursLabelProperty { get; } =
			DependencyProperty.Register(
				nameof(HoursLabel),
				typeof(string),
				typeof(TimeSpanPicker),
				new PropertyMetadata("h"));

		public static DependencyProperty MinutesLabelProperty { get; } =
			DependencyProperty.Register(
				nameof(MinutesLabel),
				typeof(string),
				typeof(TimeSpanPicker),
				new PropertyMetadata("min"));

		public static DependencyProperty SecondsLabelProperty { get; } =
			DependencyProperty.Register(
				nameof(SecondsLabel),
				typeof(string),
				typeof(TimeSpanPicker),
				new PropertyMetadata("s"));

		public TimeSpanPicker()
		{
			Loaded += TimeSpanPicker_Loaded;
		}

		private Button mFlyoutButton;
		private TextBlock mDaysTextBlock;
		private TextBlock mHoursTextBlock;
		private TextBlock mMinutesTextBlock;
		private TextBlock mSecondsTextBlock;

		private bool mTemplateIsApplied = false;

		public TimeSpan MinValue
		{
			get => (TimeSpan)GetValue(MinValueProperty);
			set => SetValue(MinValueProperty, value);
		}

		public TimeSpan MaxValue
		{
			get => (TimeSpan)GetValue(MaxValueProperty);
			set => SetValue(MaxValueProperty, value);
		}

		public TimeSpan Value
		{
			get => (TimeSpan)GetValue(ValueProperty);
			set
			{
				var oldValue = (TimeSpan)GetValue(ValueProperty);
				SetValue(ValueProperty, value);
				if (value != oldValue)
					ValueChanged.Invoke(this, new TimeSpanChangedEventArgs(oldValue, value));
			}
		}

		public event EventHandler<TimeSpanChangedEventArgs> ValueChanged;

		public TimeIncrement MinuteIncrement
		{
			get => (TimeIncrement)GetValue(MinuteIncrementProperty);
			set => SetValue(MinuteIncrementProperty, value);
		}

		public TimeIncrement SecondIncrement
		{
			get => (TimeIncrement)GetValue(SecondIncrementProperty);
			set => SetValue(SecondIncrementProperty, value);
		}

		public TimePrecision Precision
		{
			get => (TimePrecision)GetValue(PrecisionProperty);
			set => SetValue(PrecisionProperty, value);
		}

		public string DaysLabel
		{
			get => (string)GetValue(DaysLabelProperty);
			set => SetValue(DaysLabelProperty, value);
		}

		public string HoursLabel
		{
			get => (string)GetValue(HoursLabelProperty);
			set => SetValue(HoursLabelProperty, value);
		}

		public string MinutesLabel
		{
			get => (string)GetValue(MinutesLabelProperty);
			set => SetValue(MinutesLabelProperty, value);
		}

		public string SecondsLabel
		{
			get => (string)GetValue(SecondsLabelProperty);
			set => SetValue(SecondsLabelProperty, value);
		}

		protected override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			mFlyoutButton = GetTemplateChild("FlyoutButton") as Button;
			mDaysTextBlock = GetTemplateChild("DaysTextBlock") as TextBlock;
			mHoursTextBlock = GetTemplateChild("HoursTextBlock") as TextBlock;
			mMinutesTextBlock = GetTemplateChild("MinutesTextBlock") as TextBlock;
			mSecondsTextBlock = GetTemplateChild("SecondsTextBlock") as TextBlock;

			mTemplateIsApplied = true;

			mFlyoutButton.Click += FlyoutButton_Click;
		}

		private void TimeSpanPicker_Loaded(object sender, RoutedEventArgs e)
		{
			ConfigureLabels();
		}

		private void FlyoutButton_Click(object sender, RoutedEventArgs e)
		{
			var p = new TimeSpanPickerFlyout()
			{
				Placement = Windows.UI.Xaml.Controls.Primitives.FlyoutPlacementMode.Right
			};

			p.ShowAt(this);
		}

		private void ConfigureLabels()
		{
			if (!mTemplateIsApplied)
				return;

			mDaysTextBlock.Text = $"{Value.Days} {DaysLabel}";
			mHoursTextBlock.Text = $"{Value.Hours} {HoursLabel}";
			mMinutesTextBlock.Text = $"{Value.Minutes} {MinutesLabel}";
			mSecondsTextBlock.Text = $"{Value.Seconds} {SecondsLabel}";
		}
	}
}
