using PwdManager.Domain.Security;
using PwdManager.WinForms.Theme;

namespace PwdManager.WinForms.Forms.Admin;

public sealed partial class ResetPasswordForm : Form
{
    public string NewPassword => _password.Text;

    /// <summary>Designer-only constructor.</summary>
    public ResetPasswordForm() : this("kullanıcı")
    {
    }

    public ResetPasswordForm(string username)
    {
        InitializeComponent();
        ThemeManager.Apply(this);

        _who.Text = $"Kullanıcı: {username}";
        _who.ForeColor = AppPalette.TextPrimary;
        _password.Text = Generate(16);
        _password.PlaceholderText = $"Yeni geçici parola ({PasswordPolicy.Hint})";
        _note.Text = $"{PasswordPolicy.RequirementMessage}  " + _note.Text;
        AcceptButton = _save;

        _gen.Click += (_, _) => _password.Text = Generate(16);
        _save.Click += (_, _) =>
        {
            if (!PasswordPolicy.IsValid(_password.Text, out string error))
            {
                _status.AutoSize = true;
                _status.MaximumSize = new Size(376, 0);
                _status.Text = error;
                _status.ForeColor = AppPalette.Danger;
                return;
            }
            DialogResult = DialogResult.OK;
            Close();
        };
        _cancel.Click += (_, _) => { DialogResult = DialogResult.Cancel; Close(); };
    }

    private static string Generate(int length)
    {
        const string alphabet = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnpqrstuvwxyz23456789";
        Span<byte> bytes = stackalloc byte[length];
        System.Security.Cryptography.RandomNumberGenerator.Fill(bytes);
        var chars = new char[length];
        for (int i = 0; i < length; i++) chars[i] = alphabet[bytes[i] % alphabet.Length];
        return new string(chars);
    }
}
