using Microsoft.Extensions.DependencyInjection;
using PwdManager.Application.Services;
using PwdManager.WinForms.Theme;
using PwdManager.Domain.Security;
using PwdManager.Application.Models;

namespace PwdManager.WinForms.Forms.Admin;

public sealed partial class PersonnelView : UserControl, IAdminView
{
    private readonly PersonnelService _personnel = null!;
    private readonly SessionContext _session = null!;
    private List<UserRecord> _rows = new();

    /// <summary>Designer-only constructor.</summary>
    public PersonnelView()
    {
        InitializeComponent();
    }

    public PersonnelView(IServiceProvider provider, SessionContext session) : this()
    {
        _personnel = provider.GetRequiredService<PersonnelService>();
        _session = session;

        ThemeManager.Apply(this);
        Dock = DockStyle.Fill;

        UiFactory.SetIcon(_add, IconFont.Add);
        UiFactory.SetIcon(_edit, IconFont.Edit);
        UiFactory.SetIcon(_toggle, IconFont.Toggle);
        UiFactory.SetIcon(_reset, IconFont.Refresh);

        _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Username", HeaderText = "Kullanıcı adı", FillWeight = 22 });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "FullName", HeaderText = "Ad soyad", FillWeight = 30 });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Status", HeaderText = "Durum", FillWeight = 14 });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "FirstLogin", HeaderText = "İlk giriş", FillWeight = 16 });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Created", HeaderText = "Oluşturulma", FillWeight = 18 });

        _add.Click += async (_, _) => await AddAsync();
        _edit.Click += async (_, _) => await EditAsync();
        _toggle.Click += async (_, _) => await ToggleActiveAsync();
        _reset.Click += async (_, _) => await ResetAsync();
        _grid.CellDoubleClick += async (_, e) => { if (e.RowIndex >= 0) await EditAsync(); };
    }

    public async Task LoadAsync()
    {
        try
        {
            string? keep = Selected?.Username;

            _rows = (await _personnel.ListPersonnelAsync()).ToList();
            _grid.Rows.Clear();
            foreach (var u in _rows)
            {
                _grid.Rows.Add(
                    u.Username,
                    u.FullName,
                    u.IsActive == true ? "Aktif" : "Pasif",
                    u.MustChangePassword == true ? "bekliyor" : "tamam",
                    u.CreatedAt.ToString("yyyy-MM-dd"));
            }

            // Keep the previous selection (or select the first row) so the toolbar always acts on a row.
            int target = Math.Max(0, _rows.FindIndex(u => u.Username == keep));
            if (_grid.Rows.Count > 0)
            {
                _grid.ClearSelection();
                _grid.Rows[target].Selected = true;
                _grid.CurrentCell = _grid.Rows[target].Cells[0];
            }

            Info($"{_rows.Count} personel.");
        }
        catch (Exception ex) { Fail(ex.Message); }
    }

    private UserRecord? Selected
    {
        get
        {
            int i = _grid.CurrentRow?.Index ?? (_grid.SelectedRows.Count > 0 ? _grid.SelectedRows[0].Index : -1);
            return i >= 0 && i < _rows.Count ? _rows[i] : null;
        }
    }

    private async Task AddAsync()
    {
        using var dlg = new PersonnelEditorForm(isNew: true);
        if (dlg.ShowDialog(this) != DialogResult.OK) return;
        try
        {
            await _personnel.CreateAsync(_session, dlg.Username, dlg.FullName, dlg.InitialPassword);
            await LoadAsync();
            Info($"'{dlg.Username}' oluşturuldu. Geçici parolayı kendisine iletin.");
        }
        catch (Exception ex) { Fail(ex.Message); }
    }

    private async Task EditAsync()
    {
        if (Selected is not { } user) { Fail("Önce bir personel seçin."); return; }
        using var dlg = new PersonnelEditorForm(isNew: false, user.Username, user.FullName);
        if (dlg.ShowDialog(this) != DialogResult.OK) return;
        try
        {
            await _personnel.UpdateAsync(_session, user.Id, dlg.FullName, user.IsActive == true);
            await LoadAsync();
        }
        catch (Exception ex) { Fail(ex.Message); }
    }

    private async Task ToggleActiveAsync()
    {
        if (Selected is not { } user) { Fail("Önce bir personel seçin."); return; }
        bool newActive = user.IsActive != true;
        try
        {
            await _personnel.UpdateAsync(_session, user.Id, user.FullName, newActive);
            await LoadAsync();
            Info(newActive
                ? $"'{user.Username}' aktifleştirildi — tekrar giriş yapabilir."
                : $"'{user.Username}' pasifleştirildi — açık oturumu birkaç saniye içinde kapanır, yeni giriş yapamaz.");
        }
        catch (Exception ex) { Fail(ex.Message); }
    }

    private async Task ResetAsync()
    {
        if (Selected is not { } user) { Fail("Önce bir personel seçin."); return; }
        using var dlg = new ResetPasswordForm(user.Username);
        if (dlg.ShowDialog(this) != DialogResult.OK) return;
        try
        {
            await _personnel.ResetPasswordAsync(_session, user.Id, dlg.NewPassword);
            await LoadAsync();
            Info($"'{user.Username}' parolası sıfırlandı. Yeni geçici parolayı kendisine iletin.");
        }
        catch (Exception ex) { Fail(ex.Message); }
    }

    private void Info(string m) { _status.Text = m; _status.ForeColor = AppPalette.TextSecondary; }
    private void Fail(string m) { _status.Text = m; _status.ForeColor = AppPalette.Danger; }
}
