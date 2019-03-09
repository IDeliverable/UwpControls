using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace IDeliverable.Controls.Uwp.ScrollSelector
{
    public sealed class ScrollStackPanel : StackPanel, IScrollSnapPointsInfo
    {
        bool IScrollSnapPointsInfo.AreHorizontalSnapPointsRegular => false;
        bool IScrollSnapPointsInfo.AreVerticalSnapPointsRegular => false;

        IReadOnlyList<float> IScrollSnapPointsInfo.GetIrregularSnapPoints(Orientation orientation, SnapPointsAlignment alignment)
        {
            if (Orientation != Orientation.Vertical || orientation != Orientation.Vertical || alignment != SnapPointsAlignment.Center)
                return GetIrregularSnapPoints(orientation, alignment);

            var result = new List<float>();

            foreach (var child in Children)
            {
                var childOffset = child.TransformToVisual(this).TransformPoint(new Point(0, 0)).Y;
                result.Add((float)(childOffset + (child as FrameworkElement).ActualHeight / 2));
            }

            return result.AsReadOnly();
        }
    }
}
