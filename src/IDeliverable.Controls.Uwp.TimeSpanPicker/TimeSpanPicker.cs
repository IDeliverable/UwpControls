﻿using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace IDeliverable.Controls.Uwp.TimeSpanPicker
{
	public class TimeSpanPicker : Control, ITimeSpanEditor
	{
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
						editor.ConfigureLabelVisibility();
					}));

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
						editor.ConfigureLabelVisibility();
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
						editor.UpdateLabels();
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

		public static DependencyProperty DaysLabelProperty { get; } =
			DependencyProperty.Register(
				nameof(DaysLabel),
				typeof(string),
				typeof(TimeSpanPicker),
				new PropertyMetadata(
					"d",
					(d, e) =>
					{
						var editor = (TimeSpanPicker)d;
						var newValue = (string)e.NewValue;
						editor.UpdateLabels();
					}));

		public static DependencyProperty HoursLabelProperty { get; } =
			DependencyProperty.Register(
				nameof(HoursLabel),
				typeof(string),
				typeof(TimeSpanPicker),
				new PropertyMetadata(
					"h",
					(d, e) =>
					{
						var editor = (TimeSpanPicker)d;
						var newValue = (string)e.NewValue;
						editor.UpdateLabels();
					}));

		public static DependencyProperty MinutesLabelProperty { get; } =
			DependencyProperty.Register(
				nameof(MinutesLabel),
				typeof(string),
				typeof(TimeSpanPicker),
				new PropertyMetadata(
					"min",
					(d, e) =>
					{
						var editor = (TimeSpanPicker)d;
						var newValue = (string)e.NewValue;
						editor.UpdateLabels();
					}));

		public static DependencyProperty SecondsLabelProperty { get; } =
			DependencyProperty.Register(
				nameof(SecondsLabel),
				typeof(string),
				typeof(TimeSpanPicker),
				new PropertyMetadata(
					"s",
					(d, e) =>
					{
						var editor = (TimeSpanPicker)d;
						var newValue = (string)e.NewValue;
						editor.UpdateLabels();
					}));

		private const int mOptimalWidth = 240;
		private const int mOptimalHeight = 32;
		private const int mOptimalFlyoutHeight = 400;

		public TimeSpanPicker()
		{
			DefaultStyleKey = typeof(TimeSpanPicker);

			mFlyoutOptions = new FlyoutShowOptions()
			{
				ShowMode = FlyoutShowMode.Standard,
				Placement = FlyoutPlacementMode.Right,
				Position = new Point(0, 0) // If we don't specify this, the flyout shifts a few pixels to the right.
			};
		}

		private readonly FlyoutShowOptions mFlyoutOptions;
		private FrameworkElement mFlyoutAnchor;
		private Button mFlyoutButton;
		private Grid mLabelGrid;
		private Border mDaysBorder;
		private Border mHoursBorder;
		private Border mMinutesBorder;
		private Border mSecondsBorder;
		private TextBlock mDaysTextBlock;
		private TextBlock mHoursTextBlock;
		private TextBlock mMinutesTextBlock;
		private TextBlock mSecondsTextBlock;

		private bool mTemplateIsApplied = false;

		public TimePrecision Precision
		{
			get => (TimePrecision)GetValue(PrecisionProperty);
			set => SetValue(PrecisionProperty, value);
		}

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
					ValueChanged?.Invoke(this, new TimeSpanChangedEventArgs(oldValue, value));
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

			mFlyoutAnchor = GetTemplateChild("FlyoutAnchor") as FrameworkElement;
			mFlyoutButton = GetTemplateChild("FlyoutButton") as Button;
			mLabelGrid = GetTemplateChild("LabelGrid") as Grid;
			mDaysBorder = GetTemplateChild("DaysBorder") as Border;
			mHoursBorder = GetTemplateChild("HoursBorder") as Border;
			mMinutesBorder = GetTemplateChild("MinutesBorder") as Border;
			mSecondsBorder = GetTemplateChild("SecondsBorder") as Border;
			mDaysTextBlock = GetTemplateChild("DaysTextBlock") as TextBlock;
			mHoursTextBlock = GetTemplateChild("HoursTextBlock") as TextBlock;
			mMinutesTextBlock = GetTemplateChild("MinutesTextBlock") as TextBlock;
			mSecondsTextBlock = GetTemplateChild("SecondsTextBlock") as TextBlock;

			mTemplateIsApplied = true;

			ConfigureLabelVisibility();
			UpdateLabels();

			mFlyoutButton.Click += FlyoutButton_Click;
		}

		protected override Size MeasureOverride(Size availableSize)
		{
			var desiredWidth = Math.Max(MinWidth, Math.Min(mOptimalWidth, availableSize.Width));
			var desiredHeight = Math.Max(MinHeight, Math.Min(mOptimalHeight, availableSize.Height));
			return new Size(desiredWidth, desiredHeight);
		}

		private void FlyoutButton_Click(object sender, RoutedEventArgs e)
		{
			var flyout = new TimeSpanPickerFlyout(this)
			{
				Height = mOptimalFlyoutHeight,
				Width = ActualWidth,
				AreOpenCloseAnimationsEnabled = false
			};

			flyout.ValueSelected += Flyout_ValueSelected;
			flyout.Closing += Flyout_Closing;

			flyout.ShowAt(mFlyoutAnchor, mFlyoutOptions);
		}

		private void Flyout_ValueSelected(object sender, TimeSpanChangedEventArgs e)
		{
			Value = e.NewValue;
		}

		private void Flyout_Closing(FlyoutBase sender, FlyoutBaseClosingEventArgs args)
		{
			if (sender is TimeSpanPickerFlyout timeSpanPickerFlyout)
			{
				timeSpanPickerFlyout.ValueSelected -= Flyout_ValueSelected;
				timeSpanPickerFlyout.Closing -= Flyout_Closing;
			}
		}

		private void ConfigureLabelVisibility()
		{
			if (!mTemplateIsApplied)
				return;

			// Determine visibility of day, hour, minute and second components.

			this.ConfigureComponents(new[]
			{
				(mLabelGrid.ColumnDefinitions[0], mDaysBorder),
				(mLabelGrid.ColumnDefinitions[1], mHoursBorder),
				(mLabelGrid.ColumnDefinitions[2], mMinutesBorder),
				(mLabelGrid.ColumnDefinitions[3], mSecondsBorder)
			}, (border, showBorder) => border.BorderThickness = new Thickness(showBorder ? 2 : 0, 0, 0, 0));
		}

		private void UpdateLabels()
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
