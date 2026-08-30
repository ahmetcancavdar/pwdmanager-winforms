using Microsoft.Extensions.DependencyInjection;
using PwdManager.App.Composition;
using PwdManager.App.Forms.Personnel;
using PwdManager.App.Services;
using PwdManager.App.Theme;
using PwdManager.Data.Repositories;

namespace PwdManager.App.Forms;

/// <summary>
/// Read-only. Shows the secrets the admin has granted, grouped by category in a table.
/// Re-polls on an interval and re-renders only when the visible set actually changes;
/// each row does its own per-second access re-check while a password is shown.
/// </summary>
public sealed partial class PersonnelShellForm : ShellFormBase
{
    private readonly IServiceProvider _provider = null!;
    private readonly IPermissionRepository _permissions = null!;
    private readonly AuthService _auth = null!;
    private readonly SecurityConfig _security = null!;

    private PersonnelSecretsView _view = null!;
    private System.Windows.Forms.Timer _poll = null!;
    private string _signature = "";
    private bool _pollBusy;
    private bool _closing;

    /// <summary>Designer-only constructor.</summary>
    public PersonnelShellForm() : base()
    {
        InitializeComponent();
    }

    [ActivatorUtilitiesConstructor]
    public PersonnelShellForm(IServiceProvider provider)
        : base("Personel", provider.GetRequiredService<SecurityConfig>().IdleLockMinutes)
    {
        InitializeComponent();
        ThemeManager.Apply(this);

        _provider = provider;
        _permissions = provider.GetRequiredService<IPermissionRepository>();
        _auth = provider.GetRequiredService<AuthService>();
        _security = provider.GetRequiredService<SecurityConfig>();

        _hint.Text = $"Liste canlı güncellenir ({_security.PermissionPollSeconds} sn). Şifreyi görmek için satıra çift tıklayın.";
        _refresh.Click += async (_, _) => await ReloadAsync(force: true);
    }

    protected override void OnSessionAttached()
    {
        _view = new PersonnelSecretsView(_provider, Session) { Dock = DockStyle.Fill };
        _viewHost.Controls.Add(_view);

        _poll = new System.Windows.Forms.Timer { Interval = Math.Max(1, _security.PermissionPollSeconds) * 1000 };
        _poll.Tick += async (_, _) => await ReloadAsync(force: false);
        _poll.Start();

        _ = ReloadAsync(force: true);
    }

    private async Task ReloadAsync(bool force)
    {
        if (_closing || _pollBusy) return;
        _pollBusy = true;
        try
        {
            if (!await _auth.IsAccountActiveAsync(Session.User.Id))
            {
                _closing = true;
                _poll?.Stop();
                CloseWithNotice("Hesabınız devre dışı bırakıldı. Yönetici ile görüşün.");
                return;
            }

            var visible = await _permissions.ListVisibleSecretsAsync(Session.User.Id);
            string signature = string.Join("|", visible.Select(v => $"{v.Id}:{v.CategoryName}:{v.Title}:{v.UpdatedAt.Ticks}"));

            if (!force && signature == _signature)
                return;

            _signature = signature;
            _view.Render(visible);

            int categoryCount = visible.Select(v => v.CategoryName).Distinct().Count();
            _status.Text = visible.Count == 0
                ? "Henüz size bir parola erişimi verilmemiş."
                : $"{visible.Count} parola · {categoryCount} kategori.";
            _status.ForeColor = AppPalette.TextSecondary;
        }
        catch (Exception ex)
        {
            _status.Text = "Güncelleme hatası: " + ex.Message;
            _status.ForeColor = AppPalette.Danger;
        }
        finally
        {
            _pollBusy = false;
        }
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        _poll?.Stop();
        _poll?.Dispose();
        base.OnFormClosed(e);
    }
}
