using System.Reflection;
using Guna.UI2.WinForms;

namespace PwdManager.App.Theme;

/// <summary>
/// Applies the dark <see cref="AppPalette"/> to a form and every control it contains
/// at runtime. Forms lay out their controls in the Visual Studio designer
/// (<c>*.Designer.cs</c>) with neutral styling; this repaints them on load so the
/// palette stays defined in exactly one place.
/// Call <see cref="Apply(Control)"/> once from a form's constructor after InitializeComponent().
/// </summary>
public static class ThemeManager
{
    public static void Apply(Control root)
    {
        if (root is Form form)
        {
            form.BackColor = AppPalette.Background;
            form.ForeColor = AppPalette.TextPrimary;
            if (form.Font.Name != AppFonts.Body.Name)
                form.Font = AppFonts.Body;
        }

        ApplyToTree(root.Controls);
        root.ControlAdded -= OnControlAdded;
        root.ControlAdded += OnControlAdded;
    }

    private static void OnControlAdded(object? sender, ControlEventArgs e)
    {
        if (e.Control is not null) ApplyToControl(e.Control);
    }

    public static void ApplyToTree(Control.ControlCollection controls)
    {
        foreach (Control c in controls)
            ApplyToControl(c);
    }

    public static void ApplyToControl(Control c)
    {
        switch (c)
        {
            case Guna2GradientButton gb:
                gb.FillColor = AppPalette.Primary;
                gb.FillColor2 = AppPalette.PrimaryHover;
                gb.ForeColor = AppPalette.TextOnPrimary;
                gb.Font = AppFonts.BodyStrong;
                gb.BorderRadius = 8;
                break;

            case Guna2Button b:
                StyleButton(b);
                break;

            case Guna2TextBox tb:
                tb.FillColor = AppPalette.SurfaceAlt;
                tb.ForeColor = AppPalette.TextPrimary;
                tb.BorderColor = AppPalette.Border;
                tb.FocusedState.BorderColor = AppPalette.Primary;
                tb.HoverState.BorderColor = AppPalette.Border;
                tb.PlaceholderForeColor = AppPalette.TextSecondary;
                if (tb.BorderRadius == 0) tb.BorderRadius = 8;
                if (tb.Font.Size < 6) tb.Font = AppFonts.Body;
                break;

            case Guna2ComboBox cb:
                cb.FillColor = AppPalette.SurfaceAlt;
                cb.ForeColor = AppPalette.TextPrimary;
                cb.BorderColor = AppPalette.Border;
                cb.FocusedColor = AppPalette.Primary;
                if (cb.BorderRadius == 0) cb.BorderRadius = 8;
                break;

            case Guna2CheckBox chk:
                chk.CheckedState.FillColor = AppPalette.Primary;
                chk.CheckedState.BorderColor = AppPalette.Primary;
                chk.UncheckedState.BorderColor = AppPalette.Border;
                chk.ForeColor = AppPalette.TextPrimary;
                break;

            case Guna2DataGridView grid:
                StyleGrid(grid);
                break;

            case Guna2GradientPanel gp:
                gp.FillColor = AppPalette.Surface;
                gp.FillColor2 = AppPalette.Surface;
                gp.ForeColor = AppPalette.TextPrimary;
                break;

            case Guna2Panel p:
                // A panel tinted brighter than the form background reads as a "card".
                p.FillColor = p.Tag as string == "alt" ? AppPalette.SurfaceAlt : AppPalette.Surface;
                p.ForeColor = AppPalette.TextPrimary;
                break;

            case Label lbl:
                if (lbl.ForeColor == SystemColors.ControlText || lbl.ForeColor == Color.Empty)
                    lbl.ForeColor = lbl.Font.Bold ? AppPalette.TextPrimary : AppPalette.TextSecondary;
                lbl.BackColor = Color.Transparent;
                break;

            case TreeView tv:
                tv.BackColor = AppPalette.SurfaceAlt;
                tv.ForeColor = AppPalette.TextPrimary;
                tv.BorderStyle = BorderStyle.None;
                tv.LineColor = AppPalette.Border;
                break;

            case ListBox lb:
                lb.BackColor = AppPalette.SurfaceAlt;
                lb.ForeColor = AppPalette.TextPrimary;
                lb.BorderStyle = BorderStyle.None;
                break;

            case FlowLayoutPanel _:
            case TableLayoutPanel _:
            case Panel _:
                c.BackColor = c.Tag as string == "surface" ? AppPalette.Surface : AppPalette.Background;
                c.ForeColor = AppPalette.TextPrimary;
                break;

            case GroupBox _:
                c.BackColor = AppPalette.Surface;
                c.ForeColor = AppPalette.TextPrimary;
                break;

            default:
                TrySet(c, "FillColor", AppPalette.SurfaceAlt);
                TrySet(c, "BorderColor", AppPalette.Border);
                c.ForeColor = AppPalette.TextPrimary;
                break;
        }

        if (c.HasChildren)
            ApplyToTree(c.Controls);
    }

    private static void StyleButton(Guna2Button b)
    {
        bool primary = b.Tag as string != "secondary";
        if (primary)
        {
            b.FillColor = AppPalette.Primary;
            b.ForeColor = AppPalette.TextOnPrimary;
            b.Font = AppFonts.BodyStrong;
            b.HoverState.FillColor = AppPalette.PrimaryHover;
            b.PressedColor = AppPalette.PrimaryPressed;
        }
        else
        {
            b.FillColor = Color.Transparent;
            b.ForeColor = AppPalette.TextPrimary;
            b.Font = AppFonts.Body;
            b.BorderThickness = 1;
            b.BorderColor = AppPalette.Border;
            b.HoverState.FillColor = AppPalette.SurfaceAlt;
            b.HoverState.BorderColor = AppPalette.Primary;
        }
        if (b.BorderRadius == 0) b.BorderRadius = 8;
        b.DisabledState.FillColor = AppPalette.SurfaceAlt;
        b.DisabledState.ForeColor = AppPalette.TextDisabled;
        b.Cursor = Cursors.Hand;
    }

    private static void StyleGrid(Guna2DataGridView grid)
    {
        grid.BackgroundColor = AppPalette.Surface;
        grid.GridColor = AppPalette.Border;
        grid.BorderStyle = BorderStyle.None;
        grid.EnableHeadersVisualStyles = false;
        grid.ColumnHeadersDefaultCellStyle.BackColor = AppPalette.GridHeader;
        grid.ColumnHeadersDefaultCellStyle.ForeColor = AppPalette.TextPrimary;
        grid.ColumnHeadersDefaultCellStyle.Font = AppFonts.BodyStrong;
        grid.DefaultCellStyle.BackColor = AppPalette.Surface;
        grid.DefaultCellStyle.ForeColor = AppPalette.TextPrimary;
        grid.DefaultCellStyle.SelectionBackColor = AppPalette.GridSelection;
        grid.DefaultCellStyle.SelectionForeColor = AppPalette.TextPrimary;
        grid.RowsDefaultCellStyle.BackColor = AppPalette.Surface;
        grid.AlternatingRowsDefaultCellStyle.BackColor = AppPalette.SurfaceAlt;
        grid.RowHeadersVisible = false;
        grid.AllowUserToResizeRows = false;
    }

    private static void TrySet(object target, string propertyName, object value)
    {
        PropertyInfo? prop = target.GetType().GetProperty(propertyName,
            BindingFlags.Public | BindingFlags.Instance);
        if (prop is { CanWrite: true } && prop.PropertyType.IsInstanceOfType(value))
        {
            try { prop.SetValue(target, value); }
            catch { /* control doesn't support it; ignore */ }
        }
    }
}
