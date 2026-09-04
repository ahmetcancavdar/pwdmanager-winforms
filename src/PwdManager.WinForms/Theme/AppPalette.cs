using System.Drawing;

namespace PwdManager.WinForms.Theme;

/// <summary>
/// Central dark-theme colour palette ("slate + violet"). Every form/control colour
/// comes from here so the whole application stays visually consistent and can be
/// retuned in one place. Layout lives in the *.Designer.cs files; these colours are
/// painted on at runtime by <see cref="ThemeManager"/>.
/// </summary>
public static class AppPalette
{
    // ── Surfaces: an elevation ramp from the app background upward ──────────────
    public static readonly Color Background   = FromHex("#141419"); // form / app background
    public static readonly Color Surface      = FromHex("#1C1C24"); // cards, panels, grid body
    public static readonly Color SurfaceAlt   = FromHex("#23232E"); // inputs, alternating rows
    public static readonly Color SurfaceHover = FromHex("#2C2C3A"); // hover fill
    public static readonly Color Elevated     = FromHex("#26262F"); // menus / popovers

    // ── Lines ─────────────────────────────────────────────────────────────────
    public static readonly Color Border       = FromHex("#31313D"); // hairline dividers, input borders
    public static readonly Color BorderStrong = FromHex("#43434F"); // emphasised borders

    // ── Accent ───────────────────────────────────────────────────────────────
    public static readonly Color Primary        = FromHex("#7C5CFF");
    public static readonly Color PrimaryHover    = FromHex("#8E73FF");
    public static readonly Color PrimaryPressed  = FromHex("#6A49E6");
    public static readonly Color PrimarySoft     = FromHex("#2A2442"); // tint: selected nav, grid selection
    public static readonly Color PrimarySoftLine = FromHex("#4A3D82"); // border on tinted surfaces

    // ── Text ─────────────────────────────────────────────────────────────────
    public static readonly Color TextPrimary   = FromHex("#ECECF1");
    public static readonly Color TextSecondary = FromHex("#9797A8");
    public static readonly Color TextMuted     = FromHex("#6C6C7D");
    public static readonly Color TextDisabled  = FromHex("#5A5A69");
    public static readonly Color TextOnPrimary = FromHex("#FFFFFF");

    // ── Semantic ─────────────────────────────────────────────────────────────
    public static readonly Color Danger      = FromHex("#F0555B");
    public static readonly Color DangerSoft  = FromHex("#3A222A");
    public static readonly Color Success     = FromHex("#46C862");
    public static readonly Color Warning     = FromHex("#E8A423");
    public static readonly Color Info        = FromHex("#4CA0F0");

    // ── DataGridView helpers ─────────────────────────────────────────────────
    public static readonly Color GridHeader    = FromHex("#1C1C24");
    public static readonly Color GridLine      = FromHex("#26262F");
    public static readonly Color GridRowAlt    = FromHex("#1F1F27");
    public static readonly Color GridSelection = FromHex("#2A2442");

    // ── Effects ──────────────────────────────────────────────────────────────
    public static readonly Color Shadow = Color.FromArgb(100, 0, 0, 0);

    public static Color FromHex(string hex)
    {
        hex = hex.TrimStart('#');
        return Color.FromArgb(
            int.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber),
            int.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber),
            int.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber));
    }

    /// <summary>Linear blend between two colours (0 = a, 1 = b).</summary>
    public static Color Mix(Color a, Color b, double t)
    {
        t = t < 0 ? 0 : t > 1 ? 1 : t;
        return Color.FromArgb(
            (int)(a.R + (b.R - a.R) * t),
            (int)(a.G + (b.G - a.G) * t),
            (int)(a.B + (b.B - a.B) * t));
    }
}
