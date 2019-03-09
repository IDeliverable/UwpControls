using System;
using IDeliverable.Controls.Uwp.ScrollSelector;
using IDeliverable.Controls.Uwp.TimeSpanPicker;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace IDeliverable.Controls.Uwp.TestHost
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void textBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TimeSpanPicker1.Value = TimeSpan.Parse(textBox.Text);
        }

        private void textBox1_LostFocus(object sender, RoutedEventArgs e)
        {
            TimeSpanPicker1.MinValue = TimeSpan.Parse(textBox1.Text);
        }

        private void textBox2_LostFocus(object sender, RoutedEventArgs e)
        {
            TimeSpanPicker1.MaxValue = TimeSpan.Parse(textBox2.Text);

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            PrecisionComboBox.ItemsSource = Enum.GetNames(typeof(TimePrecision));
            MinuteIncrementComboBox.ItemsSource = Enum.GetNames(typeof(TimeIncrement));
            SecondIncrementComboBox.ItemsSource = Enum.GetNames(typeof(TimeIncrement));

            PrecisionComboBox.SelectedItem = TimeSpanPicker1.Precision.ToString();
            MinuteIncrementComboBox.SelectedItem = TimeSpanPicker1.MinuteIncrement.ToString();
            SecondIncrementComboBox.SelectedItem = TimeSpanPicker1.SecondIncrement.ToString();
        }

        private void PrecisionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TimeSpanPicker1.Precision = (TimePrecision)Enum.Parse(typeof(TimePrecision), (string)PrecisionComboBox.SelectedItem);
        }

        private void MinuteIncrementComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TimeSpanPicker1.MinuteIncrement = (TimeIncrement)Enum.Parse(typeof(TimeIncrement), (string)MinuteIncrementComboBox.SelectedItem);
        }

        private void SecondIncrementComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TimeSpanPicker1.SecondIncrement = (TimeIncrement)Enum.Parse(typeof(TimeIncrement), (string)SecondIncrementComboBox.SelectedItem);
        }
    }
}
