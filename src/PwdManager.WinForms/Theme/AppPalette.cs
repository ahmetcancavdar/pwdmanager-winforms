using System.Drawing;

namespace PwdManager.WinForms.Theme;

/// <summary>
/// Central dark-theme colour palette. Every form/control colour comes from here so
/// the whole application stays visually consistent and can be retuned in one place.
/// </summary>
public static class AppPalette
{
    // Surfaces (darkest -> lightest)
    public static readonly Color Background = FromHex("#1E1E2E"); // form background
    public static readonly Color Surface    = FromHex("#252537"); // cards / panels
    public static readonly Color SurfaceAlt = FromHex("#2D2D44"); // inputs, grid rows
    public static readonly Color Border      = FromHex("#3B3B58");

    // Accent
    public static readonly Color Primary      = FromHex("#7C5CFF");
    public static readonly Color PrimaryHover = FromHex("#6B4CE6");
    public static readonly Color PrimaryPressed = FromHex("#5A3FD1");

    // Text
    public static readonly Color TextPrimary   = FromHex("#E6E6EC");
    public static readonly Color TextSecondary = FromHex("#9A9AB2");
    public static readonly Color TextOnPrimary = FromHex("#FFFFFF");
    public static readonly Color TextDisabled  = FromHex("#6A6A80");

    // Semantic
    public static readonly Color Danger  = FromHex("#E5484D");
    public static readonly Color Success = FromHex("#3FB950");
    public static readonly Color Warning = FromHex("#E3A008");

    // Grid helpers
    public static readonly Color GridHeader     = FromHex("#2A2A40");
    public static readonly Color GridSelection  = FromHex("#3A3358");

    public static Color FromHex(string hex)
    {
        hex = hex.TrimStart('#');
        return Color.FromArgb(
            int.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber),
            int.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber),
            int.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber));
    }
}
