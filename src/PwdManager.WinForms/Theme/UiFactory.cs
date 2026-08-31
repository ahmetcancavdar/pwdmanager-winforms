using Guna.UI2.WinForms;

namespace PwdManager.WinForms.Theme;

/// <summary>Factory helpers for consistently styled Guna2 controls (dark theme).</summary>
public static class UiFactory
{
    public static Guna2Button PrimaryButton(string text, int width = 120, int height = 40)
    {
        var b = new Guna2Button
        {
            Text = text,
            Size = new Size(width, height),
            FillColor = AppPalette.Primary,
            ForeColor = AppPalette.TextOnPrimary,
            Font = AppFonts.BodyStrong,
            BorderRadius = 8,
            Cursor = Cursors.Hand
        };
        b.HoverState.FillColor = AppPalette.PrimaryHover;
        b.PressedColor = AppPalette.PrimaryPressed;
        b.DisabledState.FillColor = AppPalette.SurfaceAlt;
        b.DisabledState.ForeColor = AppPalette.TextDisabled;
        return b;
    }

    public static Guna2Button SecondaryButton(string text, int width = 120, int height = 40)
    {
        var b = new Guna2Button
        {
            Text = text,
            Size = new Size(width, height),
            FillColor = Color.Transparent,
            ForeColor = AppPalette.TextPrimary,
            Font = AppFonts.Body,
            BorderRadius = 8,
            BorderThickness = 1,
            BorderColor = AppPalette.Border,
            Cursor = Cursors.Hand
        };
        b.HoverState.FillColor = AppPalette.SurfaceAlt;
        b.HoverState.BorderColor = AppPalette.Primary;
        return b;
    }

    public static Guna2TextBox Input(string placeholder, bool isPassword = false, int width = 320)
    {
        var t = new Guna2TextBox
        {
            PlaceholderText = placeholder,
            Size = new Size(width, 42),
            FillColor = AppPalette.SurfaceAlt,
            ForeColor = AppPalette.TextPrimary,
            BorderColor = AppPalette.Border,
            PlaceholderForeColor = AppPalette.TextSecondary,
            BorderRadius = 8,
            Font = AppFonts.Body
        };
        t.FocusedState.BorderColor = AppPalette.Primary;
        if (isPassword)
            SetPasswordVisible(t, false);
        return t;
    }

    /// <summary>
    /// Single, reliable way to mask/unmask a text box. Guna2TextBox keeps masking if
    /// either <c>PasswordChar</c> or <c>UseSystemPasswordChar</c> is set, so a real
    /// "show" must clear both.
    /// </summary>
    public static void SetPasswordVisible(Guna2TextBox box, bool visible)
    {
        box.UseSystemPasswordChar = false;
        box.PasswordChar = visible ? '\0' : '●';
    }

    /// <summary>Wires a button as a mask/unmask toggle for the given field.</summary>
    public static void AttachRevealToggle(Guna2Button toggle, Guna2TextBox field,
        string showText = "Göster", string hideText = "Gizle")
    {
        toggle.Text = showText;
        toggle.Click += (_, _) =>
        {
            bool nowVisible = field.PasswordChar == '\0' && !field.UseSystemPasswordChar;
            SetPasswordVisible(field, !nowVisible);
            toggle.Text = nowVisible ? showText : hideText;
        };
    }

    public static Label Heading(string text) => new()
    {
        Text = text,
        Font = AppFonts.Title,
        ForeColor = AppPalette.TextPrimary,
        AutoSize = true,
        BackColor = Color.Transparent
    };

    public static Label Body(string text, bool secondary = true) => new()
    {
        Text = text,
        Font = AppFonts.Body,
        ForeColor = secondary ? AppPalette.TextSecondary : AppPalette.TextPrimary,
        AutoSize = true,
        BackColor = Color.Transparent
    };

    public static Guna2Panel Card() => new()
    {
        FillColor = AppPalette.Surface,
        BorderRadius = 12,
        ShadowDecoration = { Enabled = true, Depth = 6, Color = Color.Black }
    };

    public static Guna2Button NavButton(string text) => new()
    {
        Text = "   " + text,
        Dock = DockStyle.Top,
        Height = 44,
        TextAlign = HorizontalAlignment.Left,
        FillColor = Color.Transparent,
        ForeColor = AppPalette.TextSecondary,
        Font = AppFonts.Body,
        BorderRadius = 8,
        Cursor = Cursors.Hand,
        Margin = new Padding(0),
        HoverState = { FillColor = AppPalette.SurfaceAlt, ForeColor = AppPalette.TextPrimary }
    };

    public static Guna2DataGridView Grid()
    {
        var grid = new Guna2DataGridView
        {
            Dock = DockStyle.Fill,
            BackgroundColor = AppPalette.Surface,
            GridColor = AppPalette.Border,
            BorderStyle = BorderStyle.None,
            Font = AppFonts.Body,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            AllowUserToResizeRows = false,
            ReadOnly = true,
            RowHeadersVisible = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            EnableHeadersVisualStyles = false
        };
        grid.ColumnHeadersDefaultCellStyle.BackColor = AppPalette.GridHeader;
        grid.ColumnHeadersDefaultCellStyle.ForeColor = AppPalette.TextPrimary;
        grid.ColumnHeadersDefaultCellStyle.Font = AppFonts.BodyStrong;
        grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        grid.ColumnHeadersHeight = 40;
        grid.DefaultCellStyle.BackColor = AppPalette.Surface;
        grid.DefaultCellStyle.ForeColor = AppPalette.TextPrimary;
        grid.DefaultCellStyle.SelectionBackColor = AppPalette.GridSelection;
        grid.DefaultCellStyle.SelectionForeColor = AppPalette.TextPrimary;
        grid.DefaultCellStyle.Padding = new Padding(8, 0, 8, 0);
        grid.RowTemplate.Height = 36;
        grid.AlternatingRowsDefaultCellStyle.BackColor = AppPalette.SurfaceAlt;
        return grid;
    }
}
