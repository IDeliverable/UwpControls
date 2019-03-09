using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace IDeliverable.Controls.Uwp.TimeSpanPicker
{
    public class TimeSpanPickerFlyout : PickerFlyoutBase
    {
        protected override Control CreatePresenter()
        {
            return new TimeSpanEditor();
        }
    }
}
