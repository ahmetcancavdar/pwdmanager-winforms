using Microsoft.Extensions.DependencyInjection;
using PwdManager.Application.Services;
using PwdManager.WinForms.Theme;
using PwdManager.Application.Security;

namespace PwdManager.WinForms.Forms;

public sealed partial class ChangePasswordForm : Form
{
    private readonly AuthService _auth = null!;
    private SessionContext _session = null!;

    /// <summary>Designer-only constructor.</summary>
    public ChangePasswordForm()
    {
        InitializeComponent();
    }

    [ActivatorUtilitiesConstructor]
    public ChangePasswordForm(AuthService auth) : this()
    {
        _auth = auth;

        ThemeManager.Apply(this);
        AcceptButton = _save;

        _reveal.Click += (_, _) => ToggleReveal();
        _save.Click += async (_, _) => await SaveAsync();
        _cancel.Click += (_, _) => { DialogResult = DialogResult.Cancel; Close(); };
    }

    public void Attach(SessionContext session, bool forced = false)
    {
        _session = session;
        if (forced)
        {
            _cancel.Visible = false;
            ControlBox = false;
            _forcedNote.Visible = true;
        }
    }

    private void ToggleReveal()
    {
        bool nowVisible = _new.PasswordChar == '\0' && !_new.UseSystemPasswordChar;
        UiFactory.SetPasswordVisible(_current, !nowVisible);
        UiFactory.SetPasswordVisible(_new, !nowVisible);
        UiFactory.SetPasswordVisible(_confirm, !nowVisible);
        _reveal.Text = nowVisible ? "Göster" : "Gizle";
    }

    private async Task SaveAsync()
    {
        if (_new.Text.Length < 10) { SetStatus("Yeni parola en az 10 karakter olmalı.", true); return; }
        if (_new.Text != _confirm.Text) { SetStatus("Parolalar eşleşmiyor.", true); return; }
        if (_new.Text == _current.Text) { SetStatus("Yeni parola eskisiyle aynı olamaz.", true); return; }

        _save.Enabled = false;
        Cursor = Cursors.WaitCursor;
        try
        {
            await _auth.ChangePasswordAsync(_session, _current.Text, _new.Text);
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            SetStatus(ex.Message, true);
        }
        finally
        {
            _save.Enabled = true;
            Cursor = Cursors.Default;
        }
    }

    private void SetStatus(string message, bool error = false)
    {
        _status.Text = message;
        _status.ForeColor = error ? AppPalette.Danger : AppPalette.TextSecondary;
    }
}
