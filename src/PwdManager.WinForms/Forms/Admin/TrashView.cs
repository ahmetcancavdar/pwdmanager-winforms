using Microsoft.Extensions.DependencyInjection;
using PwdManager.Application.Services;
using PwdManager.WinForms.Theme;
using PwdManager.Domain.Security;

namespace PwdManager.WinForms.Forms.Admin;

/// <summary>
/// Admin "Silinenler" — soft-deleted kategori ve parolalar. Buradan geri yüklenebilir
/// veya kalıcı olarak silinebilir. Personel bu sayfayı hiç görmez.
/// </summary>
public sealed partial class TrashView : UserControl, IAdminView
{
    private readonly TrashService _trash = null!;
    private readonly SessionContext _session = null!;
    private List<TrashService.TrashItem> _rows = new();

    /// <summary>Designer-only constructor.</summary>
    public TrashView()
    {
        InitializeComponent();
    }

    public TrashView(IServiceProvider provider, SessionContext session) : this()
    {
        _trash = provider.GetRequiredService<TrashService>();
        _session = session;

        ThemeManager.Apply(this);
        Dock = DockStyle.Fill;

        _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Kind", HeaderText = "Tür", FillWeight = 14 });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Name", HeaderText = "Ad / Başlık", FillWeight = 34 });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Category", HeaderText = "Kategori", FillWeight = 26 });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "DeletedAt", HeaderText = "Silinme", FillWeight = 26 });

        _restore.Click += async (_, _) => await RestoreAsync();
        _purge.Click += async (_, _) => await PurgeAsync();
        _refresh.Click += async (_, _) => await LoadAsync();
    }

    public async Task LoadAsync()
    {
        try
        {
            _rows = (await _trash.ListAsync(_session)).ToList();
            _grid.Rows.Clear();
            foreach (var i in _rows)
            {
                _grid.Rows.Add(
                    i.Kind == TrashService.ItemKind.Category ? "Kategori" : "Parola",
                    i.Name,
                    i.Kind == TrashService.ItemKind.Category ? "—" : i.CategoryName,
                    i.DeletedAt == DateTime.MinValue ? "—" : i.DeletedAt.ToLocalTime().ToString("yyyy-MM-dd HH:mm"));
            }
            Info(_rows.Count == 0 ? "Çöp kutusu boş." : $"{_rows.Count} silinmiş kayıt.");
        }
        catch (Exception ex) { Fail(ex.Message); }
    }

    private TrashService.TrashItem? Selected =>
        _grid.CurrentRow is { Index: >= 0 } row && row.Index < _rows.Count ? _rows[row.Index] : null;

    private async Task RestoreAsync()
    {
        if (Selected is not { } item) { Fail("Önce bir kayıt seçin."); return; }
        try
        {
            if (item.Kind == TrashService.ItemKind.Category)
                await _trash.RestoreCategoryAsync(_session, item.Id);
            else
                await _trash.RestoreSecretAsync(_session, item.Id);

            await LoadAsync();
            Info($"'{item.Name}' geri yüklendi.");
        }
        catch (Exception ex) { Fail(ex.Message); }
    }

    private async Task PurgeAsync()
    {
        if (Selected is not { } item) { Fail("Önce bir kayıt seçin."); return; }

        string kindText = item.Kind == TrashService.ItemKind.Category ? "kategori" : "parola";
        string extra = item.Kind == TrashService.ItemKind.Category
            ? " Bu kategorinin içindeki (silinmiş olsun olmasın) tüm parolalar ve erişim izinleri de kalıcı olarak silinir."
            : " İlgili erişim izinleri de kalıcı olarak silinir.";
        if (MessageBox.Show(
                $"'{item.Name}' {kindText}sı KALICI olarak silinsin mi? Bu işlem GERİ ALINAMAZ.{extra}",
                "Kalıcı sil", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
            return;

        try
        {
            if (item.Kind == TrashService.ItemKind.Category)
                await _trash.PurgeCategoryAsync(_session, item.Id);
            else
                await _trash.PurgeSecretAsync(_session, item.Id);

            await LoadAsync();
            Info($"'{item.Name}' kalıcı olarak silindi.");
        }
        catch (Exception ex) { Fail(ex.Message); }
    }

    private void Info(string m) { _status.Text = m; _status.ForeColor = AppPalette.TextSecondary; }
    private void Fail(string m) { _status.Text = m; _status.ForeColor = AppPalette.Danger; }
}
