using System.Drawing;

namespace PwdManager.WinForms.Theme;

/// <summary>Shared typography. Segoe UI everywhere, a few fixed sizes.</summary>
public static class AppFonts
{
    private const string Family = "Segoe UI";

    public static Font Body       => new(Family, 9.75f, FontStyle.Regular);
    public static Font BodyStrong => new(Family, 9.75f, FontStyle.Bold);
    public static Font Small      => new(Family, 8.25f, FontStyle.Regular);
    public static Font Title      => new(Family, 15f,   FontStyle.Bold);
    public static Font Subtitle   => new(Family, 11f,   FontStyle.Bold);
    public static Font Mono       => new("Consolas", 11f, FontStyle.Regular);
}
