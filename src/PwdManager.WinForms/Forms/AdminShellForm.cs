using Microsoft.Extensions.DependencyInjection;
using PwdManager.Application.Configuration;
using PwdManager.WinForms.Forms.Admin;
using PwdManager.WinForms.Theme;

namespace PwdManager.WinForms.Forms;

public sealed partial class AdminShellForm : ShellFormBase
{
    private readonly IServiceProvider _provider = null!;
    private Guna.UI2.WinForms.Guna2Button? _activeButton;

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

        Wire(_navCategories, () => new CategoriesView(_provider, Session));
        Wire(_navSecrets, () => new SecretsView(_provider, Session));
        Wire(_navPersonnel, () => new PersonnelView(_provider, Session));
        Wire(_navPermissions, () => new PermissionsView(_provider, Session));
        Wire(_navAudit, () => new AuditView(_provider));
        Wire(_navTrash, () => new TrashView(_provider, Session));
    }

    protected override void OnSessionAttached()
    {
        _navCategories.PerformClick();
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
        }
        button.FillColor = AppPalette.Primary;
        button.ForeColor = AppPalette.TextOnPrimary;
        _activeButton = button;
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
