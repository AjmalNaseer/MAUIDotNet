using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Compatibility;
using System.Collections.Generic;

public class WrapPanel : Layout<View>
{
    protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
    {
        double totalWidth = 0;
        double totalHeight = 0;
        double rowHeight = 0;

        foreach (var child in Children)
        {
            var sizeRequest = child.Measure(widthConstraint, heightConstraint);
            if (totalWidth + sizeRequest.Request.Width > widthConstraint)
            {
                totalWidth = 0;
                totalHeight += rowHeight;
                rowHeight = 0;
            }

            totalWidth += sizeRequest.Request.Width;
            rowHeight = Math.Max(rowHeight, sizeRequest.Request.Height);
        }

        totalHeight += rowHeight; // Add last row height
        return new SizeRequest(new Size(widthConstraint, totalHeight));
    }

    protected override void LayoutChildren(double x, double y, double width, double height)
    {
        double rowHeight = 0;
        double totalWidth = 0;

        foreach (var child in Children)
        {
            var sizeRequest = child.Measure(width, height);
            if (totalWidth + sizeRequest.Request.Width > width)
            {
                totalWidth = 0;
                y += rowHeight;
                rowHeight = 0;
            }

            // Use Rect here
            LayoutChildIntoBoundingRegion(child, new Rect(x + totalWidth, y, sizeRequest.Request.Width, sizeRequest.Request.Height));
            totalWidth += sizeRequest.Request.Width;
            rowHeight = Math.Max(rowHeight, sizeRequest.Request.Height);
        }
    }
}
