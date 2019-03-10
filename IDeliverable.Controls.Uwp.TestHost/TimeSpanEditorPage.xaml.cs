using IDeliverable.Controls.Uwp.TimeSpanPicker;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace IDeliverable.Controls.Uwp.TestHost
{
	public sealed partial class TimeSpanEditorPage : Page
	{
		public TimeSpanEditorPage()
		{
			InitializeComponent();
		}

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			PrecisionComboBox.ItemsSource = Enum.GetNames(typeof(TimePrecision));
			PrecisionComboBox.SelectedItem = Editor.Precision.ToString();
			MinValueTextBox.Text = Editor.MinValue.ToString();
			MaxValueTextBox.Text = Editor.MaxValue.ToString();
			ValueTextBox.Text = Editor.Value.ToString();
			MinuteIncrementComboBox.ItemsSource = Enum.GetNames(typeof(TimeIncrement));
			MinuteIncrementComboBox.SelectedItem = Editor.MinuteIncrement.ToString();
			SecondIncrementComboBox.ItemsSource = Enum.GetNames(typeof(TimeIncrement));
			SecondIncrementComboBox.SelectedItem = Editor.SecondIncrement.ToString();
			DaysLabelTextBox.Text = Editor.DaysLabel;
			HoursLabelTextBox.Text = Editor.HoursLabel;
			MinutesLabelTextBox.Text = Editor.MinutesLabel;
			SecondsLabelTextBox.Text = Editor.SecondsLabel;

			Editor.RegisterPropertyChangedCallback(TimeSpanEditor.PrecisionProperty, (_, dp) => PrecisionComboBox.SelectedItem = Editor.Precision.ToString());
			Editor.RegisterPropertyChangedCallback(TimeSpanEditor.MinValueProperty, (_, dp) => MinValueTextBox.Text = Editor.MinValue.ToString());
			Editor.RegisterPropertyChangedCallback(TimeSpanEditor.MaxValueProperty, (_, dp) => MaxValueTextBox.Text = Editor.MaxValue.ToString());
			Editor.RegisterPropertyChangedCallback(TimeSpanEditor.ValueProperty, (_, dp) => ValueTextBox.Text = Editor.Value.ToString());
		}

		private void PrecisionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Editor.Precision = (TimePrecision)Enum.Parse(typeof(TimePrecision), (string)PrecisionComboBox.SelectedItem);
		}

		private void MinValueTextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			ParseTimeSpanEntry(MinValueTextBox, Editor.MinValue, newValue => Editor.MinValue = newValue);
		}

		private void MaxValueTextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			ParseTimeSpanEntry(MaxValueTextBox, Editor.MaxValue, newValue => Editor.MaxValue = newValue);
		}

		private void ValueTextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			ParseTimeSpanEntry(ValueTextBox, Editor.Value, newValue => Editor.Value = newValue);
		}

		private void MinuteIncrementComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Editor.MinuteIncrement = (TimeIncrement)Enum.Parse(typeof(TimeIncrement), (string)MinuteIncrementComboBox.SelectedItem);
		}

		private void SecondIncrementComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Editor.SecondIncrement = (TimeIncrement)Enum.Parse(typeof(TimeIncrement), (string)SecondIncrementComboBox.SelectedItem);
		}

		private void DaysLabelTextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			Editor.DaysLabel = DaysLabelTextBox.Text;
		}

		private void HoursLabelTextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			Editor.HoursLabel = HoursLabelTextBox.Text;
		}

		private void MinutesLabelTextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			Editor.MinutesLabel = MinutesLabelTextBox.Text;
		}

		private void SecondsLabelTextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			Editor.SecondsLabel = SecondsLabelTextBox.Text;
		}

		private void ParseTimeSpanEntry(TextBox textBox, TimeSpan currentValue, Action<TimeSpan> setResultAction)
		{
			if (TimeSpan.TryParse(textBox.Text, out var newValue))
			{
				setResultAction(newValue);
				textBox.Text = newValue.ToString();
			}
			else
				textBox.Text = currentValue.ToString();
		}
	}
}
