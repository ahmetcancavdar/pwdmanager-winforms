using Microsoft.Extensions.DependencyInjection;
using PwdManager.Application.Configuration;
using PwdManager.Application.Services;
using PwdManager.WinForms.Theme;
using PwdManager.Domain.Security;

namespace PwdManager.WinForms.Forms.Personnel;

/// <summary>
/// One row in the personnel list. Collapsed it shows the title and a masked marker.
/// Double-clicking (or "Göster") expands the row in place: the user re-enters their
/// login password right here — no popup — and on success the credential is shown for
/// a few seconds, with a per-second access re-check that hides it instantly if the
/// admin revokes access. (Layout is bespoke — see <see cref="LayoutRow"/>.)
/// </summary>
public sealed partial class SecretRowControl : UserControl
{
    private const int CollapsedHeight = 48;
    private const int ExpandedHeight = 116;

    private readonly SecretService _secrets = null!;
    private readonly AuthService _auth = null!;
    private readonly SessionContext _session = null!;
    private readonly long _secretId;

    private int _maxAttempts = 3;
    private int _visibleSeconds = 20;
    private int _remaining;
    private int _attempts;
    private bool _locked;
    private bool _busy;

    private const string MaskedUser = "kullanıcı: ••••";
    private const string MaskedPass = "••••••••••";

    /// <summary>Designer-only constructor.</summary>
    public SecretRowControl()
    {
        InitializeComponent();
    }

    public SecretRowControl(IServiceProvider provider, SessionContext session, long secretId, string title) : this()
    {
        _secrets = provider.GetRequiredService<SecretService>();
        _auth = provider.GetRequiredService<AuthService>();
        _session = session;
        var security = provider.GetRequiredService<SecurityConfig>();
        _maxAttempts = Math.Max(1, security.RevealMaxAttempts);
        _visibleSeconds = Math.Max(3, security.RevealVisibleSeconds);
        _secretId = secretId;

        ThemeManager.Apply(this);
        _title.ForeColor = AppPalette.TextPrimary;
        _user.ForeColor = _mask.ForeColor = _status.ForeColor = AppPalette.TextSecondary;
        _title.Text = title;
        BackColor = AppPalette.Background;
        Margin = new Padding(0, 0, 0, 6);

        _action.Click += (_, _) => TogglePrompt();
        _ok.Click += (_, _) => _ = SubmitAsync();
        _cancel.Click += (_, _) => Collapse();
        _hide.Click += (_, _) => Collapse();
        _pass.KeyDown += (_, e) =>
        {
            if (e.KeyCode == Keys.Enter && _ok.Visible) { e.SuppressKeyPress = true; _ = SubmitAsync(); }
        };
        _timer.Tick += async (_, _) => await OnTickAsync();

        foreach (Control c in new Control[] { this, _card, _top, _title, _user, _mask })
            c.DoubleClick += (_, _) => TogglePrompt();

        Size = new Size(900, CollapsedHeight);
        LayoutRow();
    }

    protected override void OnSizeChanged(EventArgs e)
    {
        base.OnSizeChanged(e);
        LayoutRow();
    }

    private void LayoutRow()
    {
        if (_title is null) return; // size event can fire before fields are built
        int w = ClientSize.Width;
        _title.Location = new Point(16, 15);
        _user.Location = new Point(Math.Min(340, w / 3), 17);
        _mask.Location = new Point(Math.Max(200, w - 210), 16);
        _action.Location = new Point(w - _action.Width - 14, 9);

        _ok.Location = new Point(w - _ok.Width - 14, 8);
        _cancel.Location = new Point(w - _cancel.Width - 14, 8);
        _hide.Location = new Point(w - _hide.Width - 14, 8);
        _pass.Location = new Point(16, 8);
        _pass.Width = Math.Max(180, w - 16 - _ok.Width - 24 - 14);
        _status.Location = new Point(16, 46);
    }

    private void TogglePrompt()
    {
        if (_locked) return;
        if (_bottom.Visible) { Collapse(); return; }

        _bottom.Visible = true;
        Height = ExpandedHeight;
        _pass.Visible = true;
        _pass.ReadOnly = false;
        _pass.Enabled = true;
        UiFactory.SetPasswordVisible(_pass, false);
        _pass.Text = "";
        _pass.PlaceholderText = "Giriş parolan";
        _ok.Visible = true;
        _cancel.Visible = true;
        _hide.Visible = false;
        _status.Text = _attempts > 0
            ? $"Kalan deneme: {_maxAttempts - _attempts}"
            : "Şifreyi görmek için giriş parolanı gir.";
        _status.ForeColor = AppPalette.TextSecondary;
        _action.Text = "Kapat";
        LayoutRow();
        _pass.Focus();
    }

    private async Task SubmitAsync()
    {
        if (_busy || _pass.Text.Length == 0) return;
        _busy = true;
        _ok.Enabled = false;
        _pass.Enabled = false;
        try
        {
            bool valid = await _auth.VerifyPasswordAsync(_session, _pass.Text);
            if (IsDisposed) return;

            if (!valid)
            {
                _attempts++;
                if (_attempts >= _maxAttempts)
                {
                    _locked = true;
                    _pass.Visible = false;
                    _ok.Visible = _cancel.Visible = _hide.Visible = false;
                    _status.Text = "Çok fazla hatalı deneme. Bu kayıt için görüntüleme kilitlendi.";
                    _status.ForeColor = AppPalette.Danger;
                    _action.Enabled = false;
                    return;
                }
                _pass.Enabled = true;
                _pass.Text = "";
                _status.Text = $"Parola hatalı. Kalan deneme: {_maxAttempts - _attempts}";
                _status.ForeColor = AppPalette.Danger;
                _pass.Focus();
                return;
            }

            var revealed = await _secrets.RevealAsync(_session, _secretId);
            if (IsDisposed) return;

            if (revealed is null)
            {
                _pass.Visible = false;
                _ok.Visible = _cancel.Visible = false;
                _hide.Visible = true;
                _status.Text = "Bu parolaya erişiminiz yok veya kaldırılmış.";
                _status.ForeColor = AppPalette.Warning;
                return;
            }

            ShowRevealed(revealed.Username, revealed.Password);
        }
        catch (Exception ex)
        {
            _pass.Enabled = true;
            _status.Text = "Hata: " + ex.Message;
            _status.ForeColor = AppPalette.Danger;
        }
        finally
        {
            _busy = false;
            _ok.Enabled = true;
        }
    }

    private void ShowRevealed(string username, string password)
    {
        _user.Text = string.IsNullOrEmpty(username) ? "kullanıcı: —" : "kullanıcı: " + username;
        _mask.Text = "görünüyor";

        _pass.Visible = true;
        _pass.Enabled = true;
        _pass.ReadOnly = true;
        _pass.PlaceholderText = "";
        _pass.Text = password;
        UiFactory.SetPasswordVisible(_pass, true);

        _ok.Visible = false;
        _cancel.Visible = false;
        _hide.Visible = true;
        _hide.Enabled = true;
        LayoutRow();

        _remaining = _visibleSeconds;
        _status.Text = $"{_remaining} sn sonra gizlenecek";
        _status.ForeColor = AppPalette.TextSecondary;
        _timer.Start();
    }

    private async Task OnTickAsync()
    {
        if (_busy) return;
        _busy = true;
        try
        {
            bool allowed;
            try { allowed = await _secrets.CanRevealAsync(_session, _secretId); }
            catch { allowed = true; }

            if (IsDisposed) return;

            if (!allowed)
            {
                _timer.Stop();
                _pass.Visible = false;
                _ok.Visible = _cancel.Visible = false;
                _hide.Visible = true;
                _user.Text = MaskedUser;
                _mask.Text = MaskedPass;
                _status.Text = "Erişiminiz kaldırıldı — parola gizlendi.";
                _status.ForeColor = AppPalette.Warning;
                return;
            }

            _remaining--;
            if (_remaining <= 0)
                Collapse();
            else
                _status.Text = $"{_remaining} sn sonra gizlenecek";
        }
        finally
        {
            _busy = false;
        }
    }

    private void Collapse()
    {
        _timer.Stop();
        _bottom.Visible = false;
        Height = CollapsedHeight;

        _pass.Visible = true;
        _pass.ReadOnly = false;
        _pass.Enabled = true;
        _pass.Text = "";
        _pass.PlaceholderText = "Giriş parolan";
        UiFactory.SetPasswordVisible(_pass, false);

        _ok.Visible = true;
        _cancel.Visible = true;
        _hide.Visible = false;
        _status.Text = "";

        _user.Text = MaskedUser;
        _mask.Text = MaskedPass;
        _action.Text = "Göster";
    }
}
