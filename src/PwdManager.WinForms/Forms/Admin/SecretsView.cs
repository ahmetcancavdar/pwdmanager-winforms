using Microsoft.Extensions.DependencyInjection;
using PwdManager.Application.Services;
using PwdManager.WinForms.Theme;
using PwdManager.Domain.Security;
using PwdManager.Application.Interfaces;
using PwdManager.Application.Models;

namespace PwdManager.WinForms.Forms.Admin;

public sealed partial class SecretsView : UserControl, IAdminView
{
    private readonly SecretService _secrets = null!;
    private readonly CategoryService _categories = null!;
    private readonly ISecretRepository _secretRepo = null!;
    private readonly SessionContext _session = null!;

    private List<CategoryRecord> _categoryList = new();
    private List<SecretSummary> _rows = new();

    private sealed record FilterItem(long? Id, string Name)
    {
        public override string ToString() => Name;
    }

    /// <summary>Designer-only constructor.</summary>
    public SecretsView()
    {
        InitializeComponent();
    }

    public SecretsView(IServiceProvider provider, SessionContext session) : this()
    {
        _secrets = provider.GetRequiredService<SecretService>();
        _categories = provider.GetRequiredService<CategoryService>();
        _secretRepo = provider.GetRequiredService<ISecretRepository>();
        _session = session;

        ThemeManager.Apply(this);
        Dock = DockStyle.Fill;

        _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Category", HeaderText = "Kategori", FillWeight = 22 });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Title", HeaderText = "Başlık", FillWeight = 30 });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Notes", HeaderText = "Not", FillWeight = 33 });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Updated", HeaderText = "Güncellenme", FillWeight = 15 });

        _add.Click += async (_, _) => await AddAsync();
        _edit.Click += async (_, _) => await EditAsync();
        _del.Click += async (_, _) => await DeleteAsync();
        _filter.SelectedIndexChanged += async (_, _) => await ReloadGridAsync();
        _grid.CellDoubleClick += async (_, e) => { if (e.RowIndex >= 0) await EditAsync(); };
    }

    public async Task LoadAsync()
    {
        try
        {
            _categoryList = (await _categories.ListAsync()).ToList();
            _filter.Items.Clear();
            _filter.Items.Add(new FilterItem(null, "Tüm kategoriler"));
            foreach (var c in _categoryList) _filter.Items.Add(new FilterItem(c.Id, c.Name));
            _filter.SelectedIndex = 0; // triggers ReloadGridAsync
        }
        catch (Exception ex) { Fail(ex.Message); }
    }

    private async Task ReloadGridAsync()
    {
        try
        {
            long? categoryId = (_filter.SelectedItem as FilterItem)?.Id;
            _rows = (categoryId is { } id
                ? await _secretRepo.ListSummariesByCategoryAsync(id)
                : await _secretRepo.ListAllSummariesAsync()).ToList();

            _grid.Rows.Clear();
            foreach (var s in _rows)
                _grid.Rows.Add(s.CategoryName, s.Title, s.Notes, s.UpdatedAt.ToString("yyyy-MM-dd HH:mm"));
            Info($"{_rows.Count} parola.");
        }
        catch (Exception ex) { Fail(ex.Message); }
    }

    private SecretSummary? Selected =>
        _grid.CurrentRow is { Index: >= 0 } row && row.Index < _rows.Count ? _rows[row.Index] : null;

    private async Task AddAsync()
    {
        if (_categoryList.Count == 0) { Fail("Önce en az bir kategori oluşturun."); return; }
        using var dlg = new SecretEditorForm("Yeni parola", _categoryList);
        if (dlg.ShowDialog(this) != DialogResult.OK) return;
        try
        {
            await _secrets.AddAsync(_session, dlg.CategoryId, dlg.SecretTitle, dlg.Username, dlg.Password, dlg.Notes);
            await ReloadGridAsync();
        }
        catch (Exception ex) { Fail(ex.Message); }
    }

    private async Task EditAsync()
    {
        if (Selected is not { } summary) { Fail("Önce bir parola seçin."); return; }
        try
        {
            var revealed = await _secrets.RevealAsync(_session, summary.Id);
            if (revealed is null) { Fail("Kayıt okunamadı."); return; }

            using var dlg = new SecretEditorForm("Parolayı düzenle", _categoryList, summary.CategoryId,
                revealed.Title, revealed.Username, revealed.Password, revealed.Notes);
            if (dlg.ShowDialog(this) != DialogResult.OK) return;

            await _secrets.UpdateAsync(_session, summary.Id, dlg.CategoryId, dlg.SecretTitle, dlg.Username, dlg.Password, dlg.Notes);
            await ReloadGridAsync();
        }
        catch (Exception ex) { Fail(ex.Message); }
    }

    private async Task DeleteAsync()
    {
        if (Selected is not { } summary) { Fail("Önce bir parola seçin."); return; }
        if (MessageBox.Show($"'{summary.Title}' parolası 'Silinenler'e taşınsın mı? Oradan geri yükleyebilirsin.",
                "Parolayı sil", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
            return;
        try
        {
            await _secrets.DeleteAsync(_session, summary.Id, summary.Title);
            await ReloadGridAsync();
        }
        catch (Exception ex) { Fail(ex.Message); }
    }

    private void Info(string m) { _status.Text = m; _status.ForeColor = AppPalette.TextSecondary; }
    private void Fail(string m) { _status.Text = m; _status.ForeColor = AppPalette.Danger; }
}
