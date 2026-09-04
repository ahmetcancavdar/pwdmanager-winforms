using System.Reflection;
using Guna.UI2.WinForms;

namespace PwdManager.WinForms.Theme;

/// <summary>
/// Applies the dark <see cref="AppPalette"/> to a form and every control it contains
/// at runtime. Forms lay out their controls in the Visual Studio designer
/// (<c>*.Designer.cs</c>) with neutral styling; this repaints them on load so the
/// palette stays defined in exactly one place.
/// Call <see cref="Apply(Control)"/> once from a form's constructor after InitializeComponent().
///
/// <para><b>Tag conventions</b> (set in the designer):</para>
/// <list type="bullet">
///   <item><c>"secondary"</c> on a Guna2Button → ghost/outline button.</item>
///   <item><c>"nav"</c> on a Guna2Button → sidebar item (ghost, left aligned, hover tint).</item>
///   <item><c>"card"</c> on a Guna2Panel → elevated rounded card with a soft shadow.</item>
///   <item><c>"alt"</c> on a Guna2Panel → uses the brighter <see cref="AppPalette.SurfaceAlt"/>.</item>
///   <item><c>"divider"</c> on a Panel/Guna2Panel → 1px hairline in <see cref="AppPalette.Border"/>.</item>
///   <item><c>"surface"</c> on a plain Panel → uses <see cref="AppPalette.Surface"/> instead of the form background.</item>
///   <item><c>"muted"</c> on a Label → uses <see cref="AppPalette.TextMuted"/>.</item>
/// </list>
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
            AppBranding.ApplyWindowIcon(form);
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
        string tag = c.Tag as string ?? "";

        switch (c)
        {
            case Guna2GradientButton gb:
                gb.FillColor = AppPalette.Primary;
                gb.FillColor2 = AppPalette.PrimaryHover;
                gb.ForeColor = AppPalette.TextOnPrimary;
                gb.Font = AppFonts.BodyStrong;
                gb.BorderRadius = 10;
                gb.Cursor = Cursors.Hand;
                break;

            case Guna2Button b when tag == "nav":
                StyleNavButton(b);
                break;

            case Guna2Button b:
                StyleButton(b, tag);
                break;

            case Guna2TextBox tb:
                tb.FillColor = AppPalette.SurfaceAlt;
                tb.ForeColor = AppPalette.TextPrimary;
                tb.BorderColor = AppPalette.Border;
                tb.BorderThickness = 1;
                tb.FocusedState.BorderColor = AppPalette.Primary;
                tb.HoverState.BorderColor = AppPalette.BorderStrong;
                tb.PlaceholderForeColor = AppPalette.TextMuted;
                tb.BorderRadius = 10;
                if (tb.Font.Size < 6) tb.Font = AppFonts.Body;
                break;

            case Guna2ComboBox cb:
                cb.FillColor = AppPalette.SurfaceAlt;
                cb.ForeColor = AppPalette.TextPrimary;
                cb.BorderColor = AppPalette.Border;
                cb.FocusedColor = AppPalette.Primary;
                cb.ItemsAppearance.BackColor = AppPalette.Elevated;
                cb.ItemsAppearance.ForeColor = AppPalette.TextPrimary;
                cb.ItemsAppearance.SelectedForeColor = AppPalette.TextOnPrimary;
                cb.ItemsAppearance.SelectedBackColor = AppPalette.Primary;
                cb.BorderRadius = 10;
                break;

            case Guna2CheckBox chk:
                chk.CheckedState.FillColor = AppPalette.Primary;
                chk.CheckedState.BorderColor = AppPalette.Primary;
                chk.UncheckedState.BorderColor = AppPalette.BorderStrong;
                chk.UncheckedState.BorderThickness = 1;
                chk.ForeColor = AppPalette.TextPrimary;
                break;

            case Guna2DataGridView grid:
                StyleGrid(grid);
                break;

            case Guna2Separator sep:
                sep.FillColor = AppPalette.Border;
                break;

            case Guna2GradientPanel gp:
                gp.FillColor = AppPalette.Surface;
                gp.FillColor2 = AppPalette.Surface;
                gp.ForeColor = AppPalette.TextPrimary;
                break;

            case Guna2Panel p:
                StylePanel(p, tag);
                break;

            case Label lbl:
                StyleLabel(lbl, tag);
                break;

            case TreeView tv:
                tv.BackColor = AppPalette.Surface;
                tv.ForeColor = AppPalette.TextPrimary;
                tv.BorderStyle = BorderStyle.None;
                tv.LineColor = AppPalette.Border;
                tv.ItemHeight = Math.Max(tv.ItemHeight, 30);
                break;

            case ListBox lb:
                lb.BackColor = AppPalette.Surface;
                lb.ForeColor = AppPalette.TextPrimary;
                lb.BorderStyle = BorderStyle.None;
                if (lb.ItemHeight < 26 && lb.DrawMode == DrawMode.Normal) lb.ItemHeight = 28;
                break;

            case FlowLayoutPanel:
            case TableLayoutPanel:
            case Panel:
                c.BackColor = tag switch
                {
                    "divider" => AppPalette.Border,
                    "surface" => AppPalette.Surface,
                    "alt"     => AppPalette.SurfaceAlt,
                    _         => AppPalette.Background
                };
                c.ForeColor = AppPalette.TextPrimary;
                break;

            case GroupBox:
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

    // ── Buttons ──────────────────────────────────────────────────────────────
    private static void StyleButton(Guna2Button b, string tag)
    {
        bool secondary = tag == "secondary";
        b.BorderRadius = b.BorderRadius == 0 ? 10 : b.BorderRadius;
        b.Cursor = Cursors.Hand;
        b.DisabledState.FillColor = AppPalette.SurfaceAlt;
        b.DisabledState.ForeColor = AppPalette.TextDisabled;
        b.DisabledState.BorderColor = AppPalette.Border;

        if (secondary)
        {
            b.FillColor = System.Drawing.Color.Transparent;
            b.ForeColor = AppPalette.TextPrimary;
            b.Font = AppFonts.Body;
            b.BorderThickness = 1;
            b.BorderColor = AppPalette.BorderStrong;
            b.HoverState.FillColor = AppPalette.SurfaceHover;
            b.HoverState.BorderColor = AppPalette.Primary;
            b.HoverState.ForeColor = AppPalette.TextPrimary;
            b.PressedColor = AppPalette.SurfaceAlt;
        }
        else
        {
            b.FillColor = AppPalette.Primary;
            b.ForeColor = AppPalette.TextOnPrimary;
            b.Font = AppFonts.BodyStrong;
            b.BorderThickness = 0;
            b.HoverState.FillColor = AppPalette.PrimaryHover;
            b.PressedColor = AppPalette.PrimaryPressed;
        }
    }

    private static void StyleNavButton(Guna2Button b)
    {
        b.BorderRadius = 10;
        b.BorderThickness = 0;
        b.BorderColor = System.Drawing.Color.Transparent;
        b.FillColor = System.Drawing.Color.Transparent;
        b.ForeColor = AppPalette.TextSecondary;
        b.Font = AppFonts.Body;
        b.TextAlign = HorizontalAlignment.Left;
        b.Cursor = Cursors.Hand;
        b.HoverState.FillColor = AppPalette.SurfaceHover;
        b.HoverState.ForeColor = AppPalette.TextPrimary;
        b.HoverState.BorderColor = System.Drawing.Color.Transparent;
        b.PressedColor = AppPalette.SurfaceAlt;
        b.PressedDepth = 0;
        b.FocusedColor = System.Drawing.Color.Transparent;
        b.DisabledState.FillColor = System.Drawing.Color.Transparent;
        b.DisabledState.ForeColor = AppPalette.TextDisabled;
        b.DisabledState.BorderColor = System.Drawing.Color.Transparent;
    }

    // ── Panels ───────────────────────────────────────────────────────────────
    private static void StylePanel(Guna2Panel p, string tag)
    {
        p.ForeColor = AppPalette.TextPrimary;

        switch (tag)
        {
            case "card":
                p.FillColor = AppPalette.Surface;
                p.BorderColor = AppPalette.Border;
                p.BorderThickness = 1;
                if (p.BorderRadius == 0) p.BorderRadius = 14;
                p.ShadowDecoration.Enabled = true;
                p.ShadowDecoration.Depth = 8;
                p.ShadowDecoration.Shadow = new Padding(6);
                p.ShadowDecoration.Color = AppPalette.Shadow;
                break;

            case "divider":
                p.FillColor = AppPalette.Border;
                break;

            case "accent":
                p.FillColor = AppPalette.Primary;
                break;

            case "row":
                p.FillColor = AppPalette.Surface;
                p.BorderColor = AppPalette.Border;
                p.BorderThickness = 1;
                if (p.BorderRadius == 0) p.BorderRadius = 10;
                break;

            case "alt":
                p.FillColor = AppPalette.SurfaceAlt;
                break;

            case "nav":
            case "topbar":
                p.FillColor = AppPalette.Surface;
                break;

            default:
                // Untagged panels are invisible layout containers — blend with the form.
                p.FillColor = AppPalette.Background;
                break;
        }
    }

    // ── Labels ───────────────────────────────────────────────────────────────
    private static void StyleLabel(Label lbl, string tag)
    {
        lbl.BackColor = System.Drawing.Color.Transparent;

        if (tag == "overline")
        {
            lbl.Font = AppFonts.Overline;
            lbl.ForeColor = AppPalette.TextMuted;
            return;
        }
        if (tag == "muted")
        {
            lbl.ForeColor = AppPalette.TextMuted;
            return;
        }
        if (tag == "danger") { lbl.ForeColor = AppPalette.Danger; return; }

        // Untouched (designer-default) colours get a sensible default by role.
        if (lbl.ForeColor == SystemColors.ControlText || lbl.ForeColor == System.Drawing.Color.Empty)
        {
            bool heading = lbl.Font.Bold || lbl.Font.Size >= 13f;
            lbl.ForeColor = heading ? AppPalette.TextPrimary : AppPalette.TextSecondary;
        }
    }

    // ── Grid ─────────────────────────────────────────────────────────────────
    private static void StyleGrid(Guna2DataGridView grid)
    {
        grid.BackgroundColor = AppPalette.Surface;
        grid.GridColor = AppPalette.GridLine;
        grid.BorderStyle = BorderStyle.None;
        grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        grid.EnableHeadersVisualStyles = false;

        grid.ColumnHeadersDefaultCellStyle.BackColor = AppPalette.GridHeader;
        grid.ColumnHeadersDefaultCellStyle.ForeColor = AppPalette.TextMuted;
        grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = AppPalette.GridHeader;
        grid.ColumnHeadersDefaultCellStyle.SelectionForeColor = AppPalette.TextMuted;
        grid.ColumnHeadersDefaultCellStyle.Font = AppFonts.Overline;
        grid.ColumnHeadersDefaultCellStyle.Padding = new Padding(10, 0, 10, 0);
        grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        grid.ColumnHeadersHeight = 40;

        grid.DefaultCellStyle.BackColor = AppPalette.Surface;
        grid.DefaultCellStyle.ForeColor = AppPalette.TextPrimary;
        grid.DefaultCellStyle.SelectionBackColor = AppPalette.GridSelection;
        grid.DefaultCellStyle.SelectionForeColor = AppPalette.TextPrimary;
        grid.DefaultCellStyle.Padding = new Padding(10, 0, 10, 0);
        grid.DefaultCellStyle.Font = AppFonts.Body;

        grid.RowsDefaultCellStyle.BackColor = AppPalette.Surface;
        grid.AlternatingRowsDefaultCellStyle.BackColor = AppPalette.GridRowAlt;
        grid.RowTemplate.Height = 42;
        grid.RowHeadersVisible = false;
        grid.AllowUserToResizeRows = false;
        grid.AllowUserToResizeColumns = false;
        grid.ScrollBars = ScrollBars.Vertical;
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
