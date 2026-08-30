using Microsoft.Extensions.DependencyInjection;
using PwdManager.App.Services;
using PwdManager.App.Theme;
using PwdManager.Core.Security;
using PwdManager.Data.Entities;

namespace PwdManager.App.Forms.Admin;

public sealed partial class CategoriesView : UserControl, IAdminView
{
    private readonly CategoryService _categories = null!;
    private readonly SessionContext _session = null!;
    private List<Category> _rows = new();

    /// <summary>Designer-only constructor.</summary>
    public CategoriesView()
    {
        InitializeComponent();
    }

    public CategoriesView(IServiceProvider provider, SessionContext session) : this()
    {
        _categories = provider.GetRequiredService<CategoryService>();
        _session = session;

        ThemeManager.Apply(this);
        Dock = DockStyle.Fill;

        _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Name", HeaderText = "Ad", FillWeight = 30 });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Description", HeaderText = "Açıklama", FillWeight = 45 });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Count", HeaderText = "Parola", FillWeight = 12 });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Created", HeaderText = "Oluşturulma", FillWeight = 20 });

        _add.Click += async (_, _) => await AddAsync();
        _edit.Click += async (_, _) => await EditAsync();
        _del.Click += async (_, _) => await DeleteAsync();
        _grid.CellDoubleClick += async (_, e) => { if (e.RowIndex >= 0) await EditAsync(); };
    }

    public async Task LoadAsync()
    {
        try
        {
            _rows = (await _categories.ListAsync()).ToList();
            _grid.Rows.Clear();
            foreach (var c in _rows)
            {
                int count = await _categories.CountSecretsAsync(c.Id);
                _grid.Rows.Add(c.Name, c.Description, count, c.CreatedAt.ToString("yyyy-MM-dd"));
            }
            Info($"{_rows.Count} kategori.");
        }
        catch (Exception ex) { Fail(ex.Message); }
    }

    private Category? Selected =>
        _grid.CurrentRow is { Index: >= 0 } row && row.Index < _rows.Count ? _rows[row.Index] : null;

    private async Task AddAsync()
    {
        using var dlg = new CategoryEditorForm("Yeni kategori");
        if (dlg.ShowDialog(this) != DialogResult.OK) return;
        try
        {
            await _categories.CreateAsync(_session, dlg.CategoryName, dlg.Description);
            await LoadAsync();
        }
        catch (Exception ex) { Fail(ex.Message); }
    }

    private async Task EditAsync()
    {
        if (Selected is not { } category) { Fail("Önce bir kategori seçin."); return; }
        using var dlg = new CategoryEditorForm("Kategoriyi düzenle", category.Name, category.Description);
        if (dlg.ShowDialog(this) != DialogResult.OK) return;
        try
        {
            await _categories.UpdateAsync(_session, category.Id, dlg.CategoryName, dlg.Description);
            await LoadAsync();
        }
        catch (Exception ex) { Fail(ex.Message); }
    }

    private async Task DeleteAsync()
    {
        if (Selected is not { } category) { Fail("Önce bir kategori seçin."); return; }
        int count = await _categories.CountSecretsAsync(category.Id);
        string warn = count > 0
            ? $"'{category.Name}' kategorisinde {count} parola var. Kategori silinince bu parolalar ve ilgili tüm erişim izinleri de silinir. Devam edilsin mi?"
            : $"'{category.Name}' kategorisi silinsin mi?";
        if (MessageBox.Show(warn, "Kategori sil", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
            return;
        try
        {
            await _categories.DeleteAsync(_session, category.Id, category.Name);
            await LoadAsync();
        }
        catch (Exception ex) { Fail(ex.Message); }
    }

    private void Info(string m) { _status.Text = m; _status.ForeColor = AppPalette.TextSecondary; }
    private void Fail(string m) { _status.Text = m; _status.ForeColor = AppPalette.Danger; }
}
