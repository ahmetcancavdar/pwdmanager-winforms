using Microsoft.Extensions.DependencyInjection;
using PwdManager.Application.Services;
using PwdManager.WinForms.Theme;
using PwdManager.Domain.Enums;
using PwdManager.Application.DTOs;
using PwdManager.Application.Security;

namespace PwdManager.WinForms.Forms;

public sealed partial class LoginForm : Form
{
    private readonly AuthService _auth = null!;
    private readonly IServiceProvider _provider = null!;

    /// <summary>Designer-only constructor.</summary>
    public LoginForm()
    {
        InitializeComponent();
    }

    [ActivatorUtilitiesConstructor]
    public LoginForm(AuthService auth, IServiceProvider provider) : this()
    {
        _auth = auth;
        _provider = provider;

        ThemeManager.Apply(this);
        AcceptButton = _loginButton;

        UiFactory.AttachRevealToggle(_reveal, _password);
        _loginButton.Click += async (_, _) => await TryLoginAsync();
        _newWindow.Click += (_, _) => ShellFormBase.LaunchNewInstance();
    }

    private async Task TryLoginAsync()
    {
        if (string.IsNullOrWhiteSpace(_username.Text) || _password.Text.Length == 0)
        {
            SetStatus("Kullanıcı adı ve parola gerekli.", error: true);
            return;
        }

        Busy(true);
        try
        {
            var outcome = await _auth.LoginAsync(_username.Text, _password.Text);
            switch (outcome.Status)
            {
                case LoginStatus.Success:
                    await OnLoggedInAsync(outcome.Session!);
                    break;
                case LoginStatus.LockedOut:
                    SetStatus($"Hesap kilitli. {outcome.LockedUntil:HH:mm}'e kadar deneyemezsiniz.", error: true);
                    break;
                case LoginStatus.Inactive:
                    SetStatus("Bu hesap devre dışı bırakılmış.", error: true);
                    break;
                default:
                    SetStatus("Kullanıcı adı veya parola hatalı.", error: true);
                    break;
            }
        }
        catch (Exception ex)
        {
            SetStatus("Bağlantı hatası: " + ex.Message, error: true);
        }
        finally
        {
            Busy(false);
        }
    }

    private async Task OnLoggedInAsync(SessionContext session)
    {
        if (await _auth.MustChangePasswordAsync(session.User.Id))
        {
            using var change = _provider.GetRequiredService<ChangePasswordForm>();
            change.Attach(session, forced: true);
            if (change.ShowDialog(this) != DialogResult.OK)
            {
                session.Dispose();
                SetStatus("Parola değişikliği zorunlu. Giriş iptal edildi.", error: true);
                return;
            }
        }

        Form shell = session.User.Role == UserRole.Admin
            ? _provider.GetRequiredService<AdminShellForm>()
            : _provider.GetRequiredService<PersonnelShellForm>();

        ((IShellForm)shell).Attach(session);
        shell.FormClosed += (_, _) =>
        {
            _username.Clear();
            _password.Clear();
            SetStatus(shell is ShellFormBase { ExitNotice: { } notice } ? notice : "");
            Show();
            Activate();
            _username.Focus();
        };
        _password.Clear();
        Hide();
        shell.Show();
    }

    private void SetStatus(string message, bool error = false)
    {
        _status.Text = message;
        _status.ForeColor = error ? AppPalette.Danger : AppPalette.TextSecondary;
    }

    private void Busy(bool busy)
    {
        _loginButton.Enabled = !busy;
        _username.Enabled = !busy;
        _password.Enabled = !busy;
        Cursor = busy ? Cursors.WaitCursor : Cursors.Default;
    }
}
