using PwdManager.Domain.Security;
using PwdManager.WinForms.Theme;

namespace PwdManager.WinForms.Forms.Admin;

/// <summary>Create a personnel account (admin only) or edit an existing one's display name.</summary>
public sealed partial class PersonnelEditorForm : Form
{
    private readonly bool _isNew;

    public string Username => _username.Text.Trim();
    public string FullName => _fullName.Text.Trim();
    public string InitialPassword => _isNew ? _password.Text : "";

    /// <summary>Designer-only constructor.</summary>
    public PersonnelEditorForm() : this(true)
    {
    }

    public PersonnelEditorForm(bool isNew, string username = "", string fullName = "")
    {
        InitializeComponent();
        ThemeManager.Apply(this);

        _isNew = isNew;
        Text = isNew ? "Yeni personel" : "Personeli düzenle";
        _heading.Text = Text;
        _username.Text = username;
        _username.ReadOnly = !isNew;
        _fullName.Text = fullName;
        AcceptButton = _save;

        if (isNew)
        {
            _password.PlaceholderText = $"Geçici parola ({PasswordPolicy.Hint})";
            _note.Text = $"{PasswordPolicy.RequirementMessage}  Personel ilk girişte bu parolayı değiştirmek zorunda kalır.";
            _note.MaximumSize = new Size(376, 0);
            _note.AutoSize = true;

            _gen.Click += (_, _) =>
            {
                _password.Text = Generate(16);
                UiFactory.SetPasswordVisible(_password, true);
                _reveal.Text = "Gizle";
            };
            _reveal.Click += (_, _) =>
            {
                bool visible = _password.PasswordChar == '\0' && !_password.UseSystemPasswordChar;
                UiFactory.SetPasswordVisible(_password, !visible);
                _reveal.Text = visible ? "Göster" : "Gizle";
            };
        }
        else
        {
            // Editing an existing account: only the display name is editable.
            _password.Visible = _reveal.Visible = _gen.Visible = _passLabel.Visible = _note.Visible = false;
            _status.Location = new Point(29, 224);
            _save.Location = new Point(28, 256);
            _cancel.Location = new Point(276, 256);
            _card.Size = new Size(412, 316);
            ClientSize = new Size(468, 372);
        }

        _save.Click += (_, _) => Submit();
        _cancel.Click += (_, _) => { DialogResult = DialogResult.Cancel; Close(); };
    }

    private void Submit()
    {
        if (Username.Length < 3) { Fail("Kullanıcı adı en az 3 karakter olmalı."); return; }
        if (FullName.Length == 0) { Fail("Ad soyad zorunlu."); return; }
        if (_isNew && !PasswordPolicy.IsValid(_password.Text, out string pwError)) { Fail(pwError); return; }
        DialogResult = DialogResult.OK;
        Close();
    }

    private void Fail(string message)
    {
        _status.AutoSize = true;
        _status.MaximumSize = new Size(376, 0);
        _status.Text = message;
        _status.ForeColor = AppPalette.Danger;
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
