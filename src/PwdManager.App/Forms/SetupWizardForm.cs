using Microsoft.Extensions.DependencyInjection;
using PwdManager.App.Composition;
using PwdManager.App.Services;
using PwdManager.App.Theme;
using PwdManager.Data.Infrastructure;

namespace PwdManager.App.Forms;

/// <summary>
/// First-run wizard: (1) test the MySQL/MariaDB server and apply the schema,
/// (2) create the first admin and show the one-time recovery code.
/// The three steps are panels laid out in the designer; only one is visible at a time.
/// </summary>
public sealed partial class SetupWizardForm : Form
{
    private readonly AppConfig _config;

    private Label _activeStatus = null!;
    private Guna.UI2.WinForms.Guna2Button _activePrimary = null!;
    private string _recoveryCode = "";

    /// <summary>Designer-only constructor.</summary>
    public SetupWizardForm() : this(new AppConfig())
    {
    }

    public SetupWizardForm(AppConfig config)
    {
        InitializeComponent();
        ThemeManager.Apply(this);

        _config = config;

        _host_.Text = _config.Database.Host;
        _port.Text = _config.Database.Port.ToString();
        _dbName.Text = _config.Database.Name;
        _dbUser.Text = string.IsNullOrEmpty(_config.Database.User) ? "root" : _config.Database.User;

        _s1Primary.Click += async (_, _) => await RunDatabaseStepAsync();
        _s1Cancel.Click += (_, _) => { DialogResult = DialogResult.Cancel; Close(); };
        _s2Primary.Click += async (_, _) => await RunAdminStepAsync();

        _copy.Click += (_, _) => { Clipboard.SetText(_recoveryCode); SetStatus("Kopyalandı."); };
        _confirm.CheckedChanged += (_, _) => _finish.Enabled = _confirm.Checked;
        _finish.Click += (_, _) => { DialogResult = DialogResult.OK; Close(); };

        ShowStep(1);
    }

    private void ShowStep(int step)
    {
        _step1.Visible = step == 1;
        _step2.Visible = step == 2;
        _step3.Visible = step == 3;
        (step == 1 ? _step1 : step == 2 ? _step2 : _step3).BringToFront();

        _activeStatus = step == 1 ? _s1Status : step == 2 ? _s2Status : _s3Status;
        _activePrimary = step == 1 ? _s1Primary : step == 2 ? _s2Primary : _finish;
    }

    private async Task RunDatabaseStepAsync()
    {
        if (!int.TryParse(_port.Text.Trim(), out int port) || port is <= 0 or > 65535)
        {
            SetStatus("Port geçersiz.", error: true);
            return;
        }
        if (string.IsNullOrWhiteSpace(_host_.Text) || string.IsNullOrWhiteSpace(_dbName.Text) || string.IsNullOrWhiteSpace(_dbUser.Text))
        {
            SetStatus("Sunucu, veritabanı adı ve kullanıcı zorunlu.", error: true);
            return;
        }

        var db = new DatabaseConfig
        {
            Host = _host_.Text.Trim(),
            Port = port,
            Name = _dbName.Text.Trim(),
            User = _dbUser.Text.Trim(),
            Password = _dbPass.Text
        };

        Busy(true);
        try
        {
            var options = new DbOptions { Host = db.Host, Port = db.Port, Name = db.Name, User = db.User, Password = db.Password };
            var installer = new DatabaseInstaller(options);

            SetStatus("Sunucuya bağlanılıyor…");
            await installer.TestServerConnectionAsync();

            SetStatus("Şema uygulanıyor…");
            await installer.ApplySchemaAsync();

            ConfigStore.SaveDatabase(db);
            _config.Database = db;

            if (await installer.HasAdminAsync())
            {
                DialogResult = DialogResult.OK;
                Close();
                return;
            }

            ShowStep(2);
        }
        catch (Exception ex)
        {
            SetStatus("Bağlantı/şema hatası: " + ex.Message, error: true);
        }
        finally
        {
            Busy(false);
        }
    }

    private async Task RunAdminStepAsync()
    {
        string user = _adminUser.Text.Trim();
        string name = _adminName.Text.Trim();
        string pass = _adminPass.Text;

        if (user.Length < 3) { SetStatus("Kullanıcı adı en az 3 karakter olmalı.", error: true); return; }
        if (pass.Length < 10) { SetStatus("Parola en az 10 karakter olmalı.", error: true); return; }
        if (pass != _adminPass2.Text) { SetStatus("Parolalar eşleşmiyor.", error: true); return; }

        Busy(true);
        try
        {
            using var provider = AppServices.Build(_config);
            var setup = provider.GetRequiredService<SetupService>();
            SetupService.Result result = await setup.CreateFirstAdminAsync(user, pass, name);

            _recoveryCode = result.RecoveryCode;
            _codeBox.Text = _recoveryCode;
            ShowStep(3);
        }
        catch (Exception ex)
        {
            SetStatus("Oluşturma hatası: " + ex.Message, error: true);
        }
        finally
        {
            Busy(false);
        }
    }

    private void SetStatus(string message, bool error = false)
    {
        _activeStatus.Text = message;
        _activeStatus.ForeColor = error ? AppPalette.Danger : AppPalette.TextSecondary;
    }

    private void Busy(bool busy)
    {
        _activePrimary.Enabled = !busy;
        Cursor = busy ? Cursors.WaitCursor : Cursors.Default;
    }
}
