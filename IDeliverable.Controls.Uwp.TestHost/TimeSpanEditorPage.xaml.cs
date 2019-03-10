using System;
using IDeliverable.Controls.Uwp.TimeSpanPicker;
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
			ValueTextBox.Text = Editor.Value.ToString();
		}

		private void PrecisionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Editor.Precision = (TimePrecision)Enum.Parse(typeof(TimePrecision), (string)PrecisionComboBox.SelectedItem);
		}

		private void ValueTextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			if (TimeSpan.TryParse(ValueTextBox.Text, out var newValue))
				Editor.Value = newValue;
			else
				ValueTextBox.Text = Editor.Value.ToString();
		}

		private void Editor_ValueChanged(object sender, TimeSpanChangedEventArgs e)
		{
			ValueTextBox.Text = Editor.Value.ToString();
		}
	}
}
