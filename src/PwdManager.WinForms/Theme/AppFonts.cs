using System.Drawing;

namespace PwdManager.WinForms.Theme;

/// <summary>Shared typography — a small, deliberate type scale. Segoe UI everywhere.</summary>
public static class AppFonts
{
    private const string Family = "Segoe UI";

    /// <summary>Big page identity (login title, wizard step title).</summary>
    public static Font Display    => new(Family, 19f, FontStyle.Bold);
    /// <summary>View / dialog heading.</summary>
    public static Font Title      => new(Family, 15.5f, FontStyle.Bold);
    /// <summary>Card / section heading.</summary>
    public static Font Subtitle   => new(Family, 11.5f, FontStyle.Bold);
    /// <summary>Small ALL-CAPS section label / column headers.</summary>
    public static Font Overline   => new(Family, 8.25f, FontStyle.Bold);

    public static Font Body       => new(Family, 9.75f, FontStyle.Regular);
    public static Font BodyStrong => new(Family, 9.75f, FontStyle.Bold);
    /// <summary>Captions, hints, status lines.</summary>
    public static Font Small      => new(Family, 8.5f, FontStyle.Regular);

    public static Font Mono       => new("Consolas", 11f, FontStyle.Regular);
    public static Font MonoLarge  => new("Consolas", 13f, FontStyle.Bold);
}
