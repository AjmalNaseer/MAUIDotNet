using Microsoft.Maui.Layouts;
namespace PublicAPI.Custom;
public class ResponsiveFlexLayout : Microsoft.Maui.Controls.FlexLayout
{
private int _columns = 7;
private double _columnSpacing = 10;

public ResponsiveFlexLayout()
{
    // Set default properties
    Wrap = FlexWrap.Wrap;
    AlignContent = FlexAlignContent.Center;
    AlignItems = FlexAlignItems.Center;
    JustifyContent = FlexJustify.Center;

    // Subscribe to size changes to recalculate column width
    SizeChanged += OnSizeChanged;
}

private void OnSizeChanged(object sender, EventArgs e)
{
    UpdateColumnWidth();
}

private void UpdateColumnWidth()
{
    double screenWidth = Width;
    if (screenWidth > 0)
    {
        double columnWidth = (screenWidth - (_columnSpacing * (_columns - 1))) / _columns;

        // Apply column width to each child element, casting each to View
        foreach (var child in Children)
        {
            if (child is View viewChild)
            {
                viewChild.WidthRequest = columnWidth;
            }
        }
    }
}

}