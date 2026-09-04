using System.Drawing;
using System.Drawing.Text;
using System.Linq;

namespace PwdManager.WinForms.Theme;

/// <summary>
/// Renders a single glyph from Windows' built-in icon font ("Segoe Fluent Icons" on
/// Win11, "Segoe MDL2 Assets" on Win10) to a transparent, tinted bitmap. No image
/// files, no resources — the icon always matches the current theme colour.
/// The constants are private-use codepoints from that font.
/// </summary>
public static class IconFont
{
    // Navigation
    public const string Tag         = ""; // Kategoriler
    public const string Lock        = ""; // Parolalar
    public const string People      = ""; // Personel
    public const string Permissions = ""; // Yetkiler
    public const string History     = ""; // Denetim
    public const string Trash       = ""; // Silinenler

    // Toolbar
    public const string Add     = "";
    public const string Edit    = "";
    public const string Delete  = "";
    public const string Refresh = "";
    public const string Toggle  = "";
    public const string Restore = "";

    private static readonly string Family = ResolveFamily();

    private static string ResolveFamily()
    {
        try
        {
            using var installed = new InstalledFontCollection();
            var names = installed.Families.Select(f => f.Name).ToArray();
            if (names.Contains("Segoe Fluent Icons")) return "Segoe Fluent Icons";
            if (names.Contains("Segoe MDL2 Assets")) return "Segoe MDL2 Assets";
        }
        catch { /* fall through */ }
        return "Segoe MDL2 Assets";
    }

    /// <summary>A <paramref name="size"/>×<paramref name="size"/> px bitmap of <paramref name="glyph"/> in <paramref name="color"/>.</summary>
    public static Bitmap Render(string glyph, int size, Color color)
    {
        var bmp = new Bitmap(size, size, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        using var g = Graphics.FromImage(bmp);
        g.Clear(Color.Transparent);
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
        using var font = new Font(Family, size * 0.68f, FontStyle.Regular, GraphicsUnit.Pixel);
        using var brush = new SolidBrush(color);
        using var fmt = new StringFormat
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center
        };
        g.DrawString(glyph, font, brush, new RectangleF(0, 0, size, size), fmt);
        return bmp;
    }
}
