using PwdManager.App.Theme;

namespace PwdManager.App.Forms.Admin;

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
        AcceptButton = _save;

        _gen.Click += (_, _) => _password.Text = Generate(16);
        _save.Click += (_, _) =>
        {
            if (_password.Text.Length < 8)
            {
                _status.Text = "Parola en az 8 karakter olmalı.";
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
