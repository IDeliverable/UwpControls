using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace IDeliverable.Controls.Uwp.TimeSpanPicker
{
    public sealed class TimeSpanEditor : Control, ITimeSpanEditor
    {
        public static DependencyProperty MinValueProperty { get; } =
            DependencyProperty.Register(
                nameof(MinValue),
                typeof(TimeSpan),
                typeof(TimeSpanEditor),
                new PropertyMetadata(
                    TimeSpan.Zero,
                    (d, e) =>
                    {
                        var editor = (TimeSpanEditor)d;
                        var newValue = (TimeSpan)e.NewValue;
                        editor.AdjustMaxValue(newValue);
                        editor.AdjustValue();
                        editor.ConfigureSelectors();
                    }));

        public static DependencyProperty MaxValueProperty { get; } =
            DependencyProperty.Register(
                nameof(MaxValue),
                typeof(TimeSpan),
                typeof(TimeSpanEditor),
                new PropertyMetadata(
                    TimeSpan.FromDays(90), // TODO: Should be TimeSpan.MaxValue perhaps?
                    (d, e) =>
                    {
                        var editor = (TimeSpanEditor)d;
                        var newValue = (TimeSpan)e.NewValue;
                        editor.AdjustMinValue(newValue);
                        editor.AdjustPrecision(newValue);
                        editor.AdjustValue();
                        editor.ConfigureSelectors();
                    }));

        public static DependencyProperty ValueProperty { get; } =
            DependencyProperty.Register(
                nameof(Value),
                typeof(TimeSpan),
                typeof(TimeSpanEditor),
                new PropertyMetadata(
                    TimeSpan.Zero,
                    (d, e) =>
                    {
                        var editor = (TimeSpanEditor)d;
                        var newValue = (TimeSpan)e.NewValue;
                        editor.AdjustValue();
                        editor.UpdateSelectors();
                    }));

        public static DependencyProperty MinuteIncrementProperty { get; } =
            DependencyProperty.Register(
                nameof(MinuteIncrement),
                typeof(TimeIncrement),
                typeof(TimeSpanEditor),
                new PropertyMetadata(
                    TimeIncrement.One,
                    (d, e) =>
                    {
                        var editor = (TimeSpanEditor)d;
                        var newValue = (TimeIncrement)e.NewValue;
                        editor.ConfigureSelectors();
                    }));

        public static DependencyProperty SecondIncrementProperty { get; } =
            DependencyProperty.Register(
                nameof(SecondIncrement),
                typeof(TimeIncrement),
                typeof(TimeSpanEditor),
                new PropertyMetadata(
                    TimeIncrement.One,
                    (d, e) =>
                    {
                        var editor = (TimeSpanEditor)d;
                        var newValue = (TimeIncrement)e.NewValue;
                        editor.ConfigureSelectors();
                    }));

        public static DependencyProperty PrecisionProperty { get; } =
            DependencyProperty.Register(
                nameof(Precision),
                typeof(TimePrecision),
                typeof(TimeSpanEditor),
                new PropertyMetadata(
                    TimePrecision.Seconds,
                    (d, e) =>
                    {
                        var editor = (TimeSpanEditor)d;
                        var newValue = (TimePrecision)e.NewValue;
                        editor.AdjustMaxValue(newValue);
                        editor.ConfigureSelectors();
                    }));

        public static DependencyProperty DaysLabelProperty { get; } =
            DependencyProperty.Register(
                nameof(DaysLabel),
                typeof(string),
                typeof(TimeSpanEditor),
                new PropertyMetadata("d"));

        public static DependencyProperty HoursLabelProperty { get; } =
            DependencyProperty.Register(
                nameof(HoursLabel),
                typeof(string),
                typeof(TimeSpanEditor),
                new PropertyMetadata("h"));

        public static DependencyProperty MinutesLabelProperty { get; } =
            DependencyProperty.Register(
                nameof(MinutesLabel),
                typeof(string),
                typeof(TimeSpanEditor),
                new PropertyMetadata("min"));

        public static DependencyProperty SecondsLabelProperty { get; } =
            DependencyProperty.Register(
                nameof(SecondsLabel),
                typeof(string),
                typeof(TimeSpanEditor),
                new PropertyMetadata("s"));

        public TimeSpanEditor()
        {
            DefaultStyleKey = typeof(TimeSpanEditor);

        }

        private Grid mSelectorGrid;
        private ScrollSelector.ScrollSelector mDaysSelector;
        private ScrollSelector.ScrollSelector mHoursSelector;
        private ScrollSelector.ScrollSelector mMinutesSelector;
        private ScrollSelector.ScrollSelector mSecondsSelector;

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
            set => SetValue(ValueProperty, value);
        }

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

            mSelectorGrid = GetTemplateChild("SelectorGrid") as Grid;
            mDaysSelector = GetTemplateChild("DaysSelector") as ScrollSelector.ScrollSelector;
            mHoursSelector = GetTemplateChild("HoursSelector") as ScrollSelector.ScrollSelector;
            mMinutesSelector = GetTemplateChild("MinutesSelector") as ScrollSelector.ScrollSelector;
            mSecondsSelector = GetTemplateChild("SecondsSelector") as ScrollSelector.ScrollSelector;

            mTemplateIsApplied = true;

            ConfigureSelectors();

            mDaysSelector.SelectionChanged += Selector_SelectionChanged;
            mHoursSelector.SelectionChanged += Selector_SelectionChanged;
            mMinutesSelector.SelectionChanged += Selector_SelectionChanged;
            mSecondsSelector.SelectionChanged += Selector_SelectionChanged;
        }

        protected override void OnKeyDown(KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Left)
            {
                FocusManager.TryMoveFocus(FocusNavigationDirection.Left);
                e.Handled = true;
            }
            else if (e.Key == Windows.System.VirtualKey.Right)
            {
                FocusManager.TryMoveFocus(FocusNavigationDirection.Right);
                e.Handled = true;
            }
        }

        private void ConfigureSelectors()
        {
            if (!mTemplateIsApplied)
                return;

            // Determine days selector visibility.
            if (MaxValue >= TimeSpan.FromDays(1))
            {
                mSelectorGrid.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
                mDaysSelector.Visibility = Visibility.Visible;
            }
            else
            {
                mSelectorGrid.ColumnDefinitions[0].Width = new GridLength(0, GridUnitType.Star);
                mDaysSelector.Visibility = Visibility.Collapsed;
            }

            // Determine hours selector visibility.
            if (MaxValue >= TimeSpan.FromHours(1) && Precision >= TimePrecision.Hours)
            {
                mSelectorGrid.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);
                mHoursSelector.Visibility = Visibility.Visible;
            }
            else
            {
                mSelectorGrid.ColumnDefinitions[1].Width = new GridLength(0, GridUnitType.Star);
                mHoursSelector.Visibility = Visibility.Collapsed;
            }

            // Determine minutes selector visibility.
            if (MaxValue >= TimeSpan.FromMinutes(1) && Precision >= TimePrecision.Minutes)
            {
                mSelectorGrid.ColumnDefinitions[2].Width = new GridLength(1, GridUnitType.Star);
                mMinutesSelector.Visibility = Visibility.Visible;
            }
            else
            {
                mSelectorGrid.ColumnDefinitions[2].Width = new GridLength(0, GridUnitType.Star);
                mMinutesSelector.Visibility = Visibility.Collapsed;
            }

            // Determine seconds selector visibility.
            if (Precision >= TimePrecision.Seconds)
            {
                mSelectorGrid.ColumnDefinitions[3].Width = new GridLength(1, GridUnitType.Star);
                mSecondsSelector.Visibility = Visibility.Visible;
            }
            else
            {
                mSelectorGrid.ColumnDefinitions[3].Width = new GridLength(0, GridUnitType.Star);
                mSecondsSelector.Visibility = Visibility.Collapsed;
            }

            // Determine the set of day values to show.
            mDaysSelector.ItemsSource = CreateRange(MinValue.Days, MaxValue.Days, TimeIncrement.One);

            // Determine the set of hour values to show.
            if ((MaxValue - MinValue).TotalDays >= 1 || MaxValue.Days > MinValue.Days)
                mHoursSelector.ItemsSource = CreateRange(0, 23, TimeIncrement.One);
            else
                mHoursSelector.ItemsSource = CreateRange(MinValue.Hours, MaxValue.Hours, TimeIncrement.One);

            // Determine the set of minute values to show.
            if ((MaxValue - MinValue).TotalHours >= 1 || MaxValue.Hours > MinValue.Hours)
                mMinutesSelector.ItemsSource = CreateRange(0, 59, MinuteIncrement);
            else
                mMinutesSelector.ItemsSource = CreateRange(MinValue.Minutes, MaxValue.Minutes, MinuteIncrement);

            // Determine the set of second values to show.
            if ((MaxValue - MinValue).TotalMinutes >= 1 || MaxValue.Minutes > MinValue.Minutes)
                mSecondsSelector.ItemsSource = CreateRange(0, 59, SecondIncrement);
            else
                mSecondsSelector.ItemsSource = CreateRange(MinValue.Seconds, MaxValue.Seconds, SecondIncrement);

            UpdateSelectors();
        }

        private void UpdateSelectors()
        {
            if (!mTemplateIsApplied)
                return;

            mDaysSelector.SelectedItem = Value.Days;
            mHoursSelector.SelectedItem = Value.Hours;
            mMinutesSelector.SelectedItem = Value.Minutes;
            mSecondsSelector.SelectedItem = Value.Seconds;
        }

        private void Selector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If any selector has a null SelectedItem it is most likely in being reconfigured; don't
            // update the value in this case.
            if (mDaysSelector.SelectedItem == null || mHoursSelector.SelectedItem == null || mMinutesSelector.SelectedItem == null || mSecondsSelector.SelectedItem == null)
                return;

            var days = mDaysSelector.Visibility == Visibility.Visible ? (int)mDaysSelector.SelectedItem : 0;
            var hours = mHoursSelector.Visibility == Visibility.Visible ? (int)mHoursSelector.SelectedItem : 0;
            var minutes = mMinutesSelector.Visibility == Visibility.Visible ? (int)mMinutesSelector.SelectedItem : 0;
            var seconds = mSecondsSelector.Visibility == Visibility.Visible ? (int)mSecondsSelector.SelectedItem : 0;

            Value = new TimeSpan(days, hours, minutes, seconds);
        }

        private IEnumerable<int> CreateRange(int start, int end, TimeIncrement increment)
        {
            var rangeQuery =
                from i in Enumerable.Range(start, end - start + 1)
                where i == start || i == end || i % (int)increment == 0
                select i;

            return rangeQuery.ToArray();
        }
    }
}
