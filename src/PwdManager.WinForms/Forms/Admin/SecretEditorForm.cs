using PwdManager.WinForms.Theme;
using PwdManager.Domain.Entities;

namespace PwdManager.WinForms.Forms.Admin;

public sealed partial class SecretEditorForm : Form
{
    public long CategoryId => ((CategoryItem)_category.SelectedItem!).Id;
    public string SecretTitle => _title.Text.Trim();
    public string Username => _username.Text;
    public string Password => _password.Text;
    public string Notes => _notes.Text.Trim();

    private sealed record CategoryItem(long Id, string Name)
    {
        public override string ToString() => Name;
    }

    /// <summary>Designer-only constructor.</summary>
    public SecretEditorForm() : this("Parola", System.Array.Empty<CategoryRecord>())
    {
    }

    public SecretEditorForm(
        string title,
        IReadOnlyList<CategoryRecord> categories,
        long? categoryId = null,
        string titleValue = "", string usernameValue = "", string passwordValue = "", string notesValue = "")
    {
        InitializeComponent();
        ThemeManager.Apply(this);

        Text = title;
        _heading.Text = title;
        _title.Text = titleValue;
        _username.Text = usernameValue;
        _password.Text = passwordValue;
        _notes.Text = notesValue;
        AcceptButton = _save;

        foreach (var c in categories)
            _category.Items.Add(new CategoryItem(c.Id, c.Name));

        if (categoryId is { } id)
        {
            for (int i = 0; i < _category.Items.Count; i++)
                if (_category.Items[i] is CategoryItem item && item.Id == id) { _category.SelectedIndex = i; break; }
        }
        else if (_category.Items.Count > 0)
        {
            _category.SelectedIndex = 0;
        }

        _toggle.Click += (_, _) =>
        {
            bool visible = _password.PasswordChar == '\0' && !_password.UseSystemPasswordChar;
            UiFactory.SetPasswordVisible(_password, !visible);
            _toggle.Text = visible ? "Göster" : "Gizle";
        };
        _gen.Click += (_, _) =>
        {
            _password.Text = GeneratePassword(20);
            UiFactory.SetPasswordVisible(_password, true);
            _toggle.Text = "Gizle";
        };
        _save.Click += (_, _) =>
        {
            if (_category.SelectedItem is null) { Fail("Kategori seçin."); return; }
            if (SecretTitle.Length == 0) { Fail("Başlık zorunlu."); return; }
            if (Password.Length == 0) { Fail("Parola zorunlu."); return; }
            DialogResult = DialogResult.OK;
            Close();
        };
        _cancel.Click += (_, _) => { DialogResult = DialogResult.Cancel; Close(); };
    }

    private void Fail(string message)
    {
        _status.Text = message;
        _status.ForeColor = AppPalette.Danger;
    }

    private static string GeneratePassword(int length)
    {
        const string alphabet = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnpqrstuvwxyz23456789!@#$%^&*-_";
        Span<byte> bytes = stackalloc byte[length];
        System.Security.Cryptography.RandomNumberGenerator.Fill(bytes);
        var chars = new char[length];
        for (int i = 0; i < length; i++) chars[i] = alphabet[bytes[i] % alphabet.Length];
        return new string(chars);
    }
}
