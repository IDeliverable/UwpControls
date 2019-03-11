using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace IDeliverable.Controls.Uwp.Containers
{
	/// <summary>
	/// Arranges child elements into a single line that can be oriented horizontally or vertically,
	/// where the full available space along the orientation axis is distributed equally among the child
	/// elements, with an optional spacing between them.
	/// </summary>
	public sealed class ArrayPanel : Panel
	{
		public static DependencyProperty OrientationProperty { get; } =
			DependencyProperty.Register(
				nameof(Orientation), 
				typeof(Orientation), 
				typeof(ArrayPanel), 
				new PropertyMetadata(
					Orientation.Horizontal, 
					(d, e) => (d as ArrayPanel)?.InvalidateArrange()));

		public static DependencyProperty SpacingProperty { get; } = 
			DependencyProperty.Register(
				nameof(Spacing), 
				typeof(double), 
				typeof(ArrayPanel), 
				new PropertyMetadata(
					null, 
					(d, e) => (d as ArrayPanel)?.InvalidateArrange()));

		public Orientation Orientation
		{
			get => (Orientation)GetValue(OrientationProperty);
			set => SetValue(OrientationProperty, value);
		}

		public double Spacing
		{
			get => (double)GetValue(SpacingProperty);
			set => SetValue(SpacingProperty, value);
		}

		protected override Size MeasureOverride(Size availableSize)
		{
			var desiredSize = availableSize;
			var maxCellCrossSize = 0.0d;
			
			switch (Orientation)
			{
				case Orientation.Horizontal:
					var cellWidth = (availableSize.Width - Spacing * (Children.Count - 1)) / Children.Count;
					foreach (var child in Children)
					{
						child.Measure(new Size(cellWidth, availableSize.Height));
						maxCellCrossSize = Math.Max(maxCellCrossSize, child.DesiredSize.Height);
					}
					desiredSize = new Size(availableSize.Width, Math.Min(availableSize.Height, maxCellCrossSize));
					break;

				case Orientation.Vertical:
					var cellHeight = (availableSize.Height - Spacing * (Children.Count - 1)) / Children.Count;
					foreach (var child in Children)
					{
						child.Measure(new Size(availableSize.Width, cellHeight));
						maxCellCrossSize = Math.Max(maxCellCrossSize, child.DesiredSize.Width);
					}
					desiredSize = new Size(Math.Min(availableSize.Width, maxCellCrossSize), availableSize.Height);
					break;
			}

			return desiredSize;
		}

		protected override Size ArrangeOverride(Size finalSize)
		{
			switch (Orientation)
			{
				case Orientation.Horizontal:
					var cellWidth = (finalSize.Width - Spacing * (Children.Count - 1)) / Children.Count;
					for (var i = 0; i < Children.Count; i++)
						Children[i].Arrange(new Rect((cellWidth + Spacing) * i, 0, cellWidth, finalSize.Height));
					break;

				case Orientation.Vertical:
					var cellHeight = (finalSize.Height - Spacing * (Children.Count - 1)) / Children.Count;
					for (var i = 0; i < Children.Count; i++)
						Children[i].Arrange(new Rect(0, (cellHeight + Spacing) * i, finalSize.Width, cellHeight));
					break;
			}

			return finalSize;
		}
	}
}
