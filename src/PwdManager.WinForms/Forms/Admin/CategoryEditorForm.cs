using PwdManager.WinForms.Theme;

namespace PwdManager.WinForms.Forms.Admin;

public sealed partial class CategoryEditorForm : Form
{
    public string CategoryName => _name.Text.Trim();
    public string Description => _description.Text.Trim();

    /// <summary>Designer-only constructor.</summary>
    public CategoryEditorForm() : this("Kategori")
    {
    }

    public CategoryEditorForm(string title, string name = "", string description = "")
    {
        InitializeComponent();
        ThemeManager.Apply(this);

        Text = title;
        _heading.Text = title;
        _name.Text = name;
        _description.Text = description;
        AcceptButton = _save;

        _save.Click += (_, _) =>
        {
            if (CategoryName.Length == 0) { Fail("Kategori adı zorunlu."); return; }
            DialogResult = DialogResult.OK;
            Close();
        };
        _cancel.Click += (_, _) => { DialogResult = DialogResult.Cancel; Close(); };
    }

    public void Fail(string message)
    {
        _status.Text = message;
        _status.ForeColor = AppPalette.Danger;
    }
}
