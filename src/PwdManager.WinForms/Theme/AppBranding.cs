using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using Guna.UI2.WinForms;

namespace PwdManager.WinForms.Theme;

/// <summary>
/// Application logo / window icon. Looks for a logo file in the <c>Assets\</c> folder
/// next to the exe — <c>logo.png</c> / <c>logo.jpg</c> / <c>logo.ico</c>, or, failing
/// that, the first image file it finds there. If nothing is present the app falls back
/// to the plain accent-coloured brand square and the default window icon.
///
/// A transparent PNG is shown directly on the bar. Any other format (JPG/BMP) keeps its
/// own background, so it is shown on a small white rounded "app tile" — this keeps the
/// artwork sharp instead of trying to cut the background out.
/// </summary>
public static class AppBranding
{
    private static readonly object _gate = new();
    private static bool _loaded;
    private static Image? _logo;
    private static bool _logoHasAlpha;
    private static Icon? _windowIcon;

    private static string AssetsDir => System.IO.Path.Combine(AppContext.BaseDirectory, "Assets");

    public static Image? Logo { get { EnsureLoaded(); return _logo; } }
    public static bool LogoHasAlpha { get { EnsureLoaded(); return _logoHasAlpha; } }
    public static Icon? WindowIcon { get { EnsureLoaded(); return _windowIcon; } }

    private static void EnsureLoaded()
    {
        if (_loaded) return;
        lock (_gate)
        {
            if (_loaded) return;
            try
            {
                string? imgPath = FindImage();
                if (imgPath is not null)
                {
                    using var fs = System.IO.File.OpenRead(imgPath);
                    _logo = Image.FromStream(fs, false, true);

                    bool isPng = string.Equals(System.IO.Path.GetExtension(imgPath), ".png",
                        StringComparison.OrdinalIgnoreCase);
                    _logoHasAlpha = isPng || System.Drawing.Image.IsAlphaPixelFormat(_logo.PixelFormat);
                }

                string ico = System.IO.Path.Combine(AssetsDir, "logo.ico");
                if (System.IO.File.Exists(ico))
                    _windowIcon = new Icon(ico);
                else if (_logo is not null)
                    _windowIcon = BuildIcon(_logo);
            }
            catch { /* keep fallbacks */ }
            _loaded = true;
        }
    }

    private static string? FindImage()
    {
        if (!System.IO.Directory.Exists(AssetsDir)) return null;

        string[] preferred = { "logo.png", "logo.jpg", "logo.jpeg", "logo.bmp", "logo.gif" };
        foreach (string name in preferred)
        {
            string p = System.IO.Path.Combine(AssetsDir, name);
            if (System.IO.File.Exists(p)) return p;
        }

        string[] exts = { ".png", ".jpg", ".jpeg", ".bmp", ".gif" };
        return System.IO.Directory.EnumerateFiles(AssetsDir)
            .Where(f => exts.Contains(System.IO.Path.GetExtension(f).ToLowerInvariant()))
            .OrderBy(f => f)
            .FirstOrDefault();
    }

    /// <summary>High-quality downscale of <paramref name="src"/> to fit <paramref name="box"/>×<paramref name="box"/> px.</summary>
    public static Bitmap Scaled(Image src, int box)
    {
        double s = Math.Min((double)box / src.Width, (double)box / src.Height);
        int w = Math.Max(1, (int)Math.Round(src.Width * s));
        int h = Math.Max(1, (int)Math.Round(src.Height * s));

        var bmp = new Bitmap(box, box, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        using var g = Graphics.FromImage(bmp);
        g.Clear(Color.Transparent);
        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
        g.SmoothingMode = SmoothingMode.HighQuality;
        g.PixelOffsetMode = PixelOffsetMode.HighQuality;
        g.CompositingQuality = CompositingQuality.HighQuality;
        g.DrawImage(src, new Rectangle((box - w) / 2, (box - h) / 2, w, h));
        return bmp;
    }

    private static Icon BuildIcon(Image source)
    {
        using Bitmap square = Scaled(source, 64);
        IntPtr h = square.GetHicon();
        try { return (Icon)Icon.FromHandle(h).Clone(); }
        finally { NativeMethods.DestroyIcon(h); }
    }

    /// <summary>Sets a form's window icon to the app logo when one is configured.</summary>
    public static void ApplyWindowIcon(System.Windows.Forms.Form form)
    {
        if (WindowIcon is { } icon)
        {
            form.Icon = icon;
            form.ShowIcon = true;
        }
    }

    /// <summary>
    /// Turns the accent-coloured brand square into the logo, rendered crisply at the
    /// panel size. Transparent PNGs sit directly on the bar; opaque images get a white
    /// rounded tile. Call after <see cref="ThemeManager.Apply"/>.
    /// </summary>
    public static void ApplyBrand(Guna2Panel brandDot)
    {
        if (Logo is not { } logo) return;

        brandDot.BorderThickness = 0;
        brandDot.BackgroundImage = null;
        brandDot.FillColor = _logoHasAlpha ? Color.Transparent : Color.White;
        brandDot.BorderRadius = _logoHasAlpha ? 0 : Math.Max(8, brandDot.BorderRadius);

        int inset = _logoHasAlpha ? 0 : 5;
        int box = Math.Max(8, Math.Min(brandDot.Width, brandDot.Height) - inset * 2);

        System.Windows.Forms.PictureBox? pb = brandDot.Controls
            .OfType<System.Windows.Forms.PictureBox>()
            .FirstOrDefault(p => p.Name == "_brandLogoImg");

        if (pb is null)
        {
            pb = new System.Windows.Forms.PictureBox
            {
                Name = "_brandLogoImg",
                Dock = System.Windows.Forms.DockStyle.Fill,
                SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage,
                Margin = new System.Windows.Forms.Padding(0)
            };
            brandDot.Controls.Add(pb);
        }

        pb.BackColor = Color.Transparent;
        var old = pb.Image;
        pb.Image = Scaled(logo, box); // pre-scaled, then CenterImage → no extra resampling
        old?.Dispose();
    }

    private static class NativeMethods
    {
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        public static extern bool DestroyIcon(IntPtr handle);
    }
}
