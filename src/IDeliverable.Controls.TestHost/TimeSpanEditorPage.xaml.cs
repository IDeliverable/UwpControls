using IDeliverable.Controls.TestHost;
using Windows.UI.Xaml.Controls;

namespace IDeliverable.Controls.Uwp.TestHost
{
	public sealed partial class TimeSpanEditorPage : Page
	{
		public TimeSpanEditorPage()
		{
			InitializeComponent();
		}

		internal TimeSpanEditorViewModel ViewModel { get; } = new TimeSpanEditorViewModel();

		//private void ParseTimeSpanEntry(TextBox textBox, TimeSpan currentValue, Action<TimeSpan> setResultAction)
		//{
		//	if (TimeSpan.TryParse(textBox.Text, out var newValue))
		//	{
		//		setResultAction(newValue);
		//		textBox.Text = newValue.ToString();
		//	}
		//	else
		//		textBox.Text = currentValue.ToString();
		//}
	}
}
