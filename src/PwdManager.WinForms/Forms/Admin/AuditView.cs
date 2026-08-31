using Microsoft.Extensions.DependencyInjection;
using PwdManager.WinForms.Theme;
using PwdManager.Application.Interfaces;

namespace PwdManager.WinForms.Forms.Admin;

public sealed partial class AuditView : UserControl, IAdminView
{
    private readonly IAuditRepository _audit = null!;

    /// <summary>Designer-only constructor.</summary>
    public AuditView()
    {
        InitializeComponent();
    }

    public AuditView(IServiceProvider provider) : this()
    {
        _audit = provider.GetRequiredService<IAuditRepository>();

        ThemeManager.Apply(this);
        Dock = DockStyle.Fill;

        _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "When", HeaderText = "Zaman", FillWeight = 18 });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "User", HeaderText = "Kullanıcı", FillWeight = 16 });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Action", HeaderText = "İşlem", FillWeight = 20 });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Target", HeaderText = "Hedef", FillWeight = 16 });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Detail", HeaderText = "Ayrıntı", FillWeight = 30 });

        _refresh.Click += async (_, _) => await LoadAsync();
    }

    public async Task LoadAsync()
    {
        try
        {
            var rows = await _audit.RecentAsync(300);
            _grid.Rows.Clear();
            foreach (var e in rows)
            {
                string target = e.TargetType.Length > 0
                    ? $"{e.TargetType}#{e.TargetId?.ToString() ?? "-"}"
                    : "";
                _grid.Rows.Add(e.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"), e.Username, e.Action, target, e.Detail);
            }
            _status.Text = $"Son {rows.Count} kayıt.";
            _status.ForeColor = AppPalette.TextSecondary;
        }
        catch (Exception ex)
        {
            _status.Text = ex.Message;
            _status.ForeColor = AppPalette.Danger;
        }
    }
}
