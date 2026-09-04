using Microsoft.Extensions.DependencyInjection;
using PwdManager.Application.Configuration;
using PwdManager.WinForms.Forms.Admin;
using PwdManager.WinForms.Theme;

namespace PwdManager.WinForms.Forms;

public sealed partial class AdminShellForm : ShellFormBase
{
    private readonly IServiceProvider _provider = null!;
    private Guna.UI2.WinForms.Guna2Button? _activeButton;

    // Which icon glyph belongs to each nav button (so the active one can be re-tinted).
    private readonly Dictionary<Guna.UI2.WinForms.Guna2Button, string> _navGlyphs = new();
    private const int NavIconSize = 18;

    /// <summary>Designer-only constructor.</summary>
    public AdminShellForm() : base()
    {
        InitializeComponent();
    }

    [ActivatorUtilitiesConstructor]
    public AdminShellForm(IServiceProvider provider)
        : base("Yönetici", provider.GetRequiredService<SecurityConfig>().IdleLockMinutes)
    {
        InitializeComponent();
        ThemeManager.Apply(this);
        _provider = provider;

        SetNavIcon(_navCategories, IconFont.Tag);
        SetNavIcon(_navSecrets, IconFont.Lock);
        SetNavIcon(_navPersonnel, IconFont.People);
        SetNavIcon(_navPermissions, IconFont.Permissions);
        SetNavIcon(_navAudit, IconFont.History);
        SetNavIcon(_navTrash, IconFont.Trash);

        Wire(_navCategories, () => new CategoriesView(_provider, Session));
        Wire(_navSecrets, () => new SecretsView(_provider, Session));
        Wire(_navPersonnel, () => new PersonnelView(_provider, Session));
        Wire(_navPermissions, () => new PermissionsView(_provider, Session));
        Wire(_navAudit, () => new AuditView(_provider));
        Wire(_navTrash, () => new TrashView(_provider, Session));
    }

    private void SetNavIcon(Guna.UI2.WinForms.Guna2Button button, string glyph)
    {
        _navGlyphs[button] = glyph;
        button.Image = IconFont.Render(glyph, NavIconSize, AppPalette.TextSecondary);
        button.ImageAlign = HorizontalAlignment.Left;
        button.ImageOffset = new Point(8, 0);
        button.Text = "       " + button.Text.Trim();
    }

    protected override void OnSessionAttached()
    {
        // Land straight on Yetkiler instead of an empty view host. Calling SetActive +
        // SwapView directly (rather than _navPermissions.PerformClick()) so this doesn't
        // depend on the button's own click plumbing having anything to hook into yet.
        SetActive(_navPermissions);
        SwapView(new PermissionsView(_provider, Session));
    }

    private void Wire(Guna.UI2.WinForms.Guna2Button button, Func<UserControl> factory)
    {
        button.Click += (_, _) =>
        {
            SetActive(button);
            SwapView(factory());
        };
    }

    private void SetActive(Guna.UI2.WinForms.Guna2Button button)
    {
        if (_activeButton is not null)
        {
            _activeButton.FillColor = Color.Transparent;
            _activeButton.ForeColor = AppPalette.TextSecondary;
            _activeButton.Font = AppFonts.Body;
            _activeButton.BorderThickness = 0;
            RetintNavIcon(_activeButton, AppPalette.TextSecondary);
        }
        // Active item = a soft violet-tinted pill (not a loud solid fill).
        button.FillColor = AppPalette.PrimarySoft;
        button.ForeColor = AppPalette.TextPrimary;
        button.Font = AppFonts.BodyStrong;
        button.BorderThickness = 0;
        RetintNavIcon(button, AppPalette.Primary);
        _activeButton = button;
    }

    private void RetintNavIcon(Guna.UI2.WinForms.Guna2Button button, Color color)
    {
        if (!_navGlyphs.TryGetValue(button, out string? glyph)) return;
        Image? old = button.Image;
        button.Image = IconFont.Render(glyph, NavIconSize, color);
        old?.Dispose();
    }

    private void SwapView(UserControl view)
    {
        var previous = _viewHost.Controls.Cast<Control>().ToArray();
        _viewHost.Controls.Clear();
        foreach (Control old in previous)
            old.Dispose();

        view.Dock = DockStyle.Fill;
        _viewHost.Controls.Add(view);

        if (view is IAdminView adminView)
            _ = adminView.LoadAsync();
    }
}
