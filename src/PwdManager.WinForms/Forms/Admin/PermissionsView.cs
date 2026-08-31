using Microsoft.Extensions.DependencyInjection;
using PwdManager.Application.Services;
using PwdManager.WinForms.Theme;
using PwdManager.Domain.Security;
using PwdManager.Application.Models;
using PwdManager.Application.Interfaces;

namespace PwdManager.WinForms.Forms.Admin;

/// <summary>
/// Left: personnel list. Right: category/secret tree with checkboxes.
///  • Kategori kutusu işaretli  → tüm kategori verilir.
///  • Kategori verili + alt şifrenin kutusu kaldırılırsa → o şifre bu personelden
///    "istisna" (deny) olarak gizlenir; kategori verili kalmaya devam eder.
///  • Kategori verili değilse → alt şifre kutuları tek tek verir/kaldırır.
/// Her değişiklik anında DB'ye yazılır ve personelin sync sayacını artırır.
/// </summary>
public sealed partial class PermissionsView : UserControl, IAdminView
{
    private sealed record NodeRef(bool IsCategory, long Id);

    private readonly PermissionService _permissions = null!;
    private readonly CategoryService _categories = null!;
    private readonly PersonnelService _personnel = null!;
    private readonly ISecretRepository _secretRepo = null!;
    private readonly SessionContext _session = null!;

    private List<UserRecord> _personnelList = new();
    private bool _suppress;

    /// <summary>Designer-only constructor.</summary>
    public PermissionsView()
    {
        InitializeComponent();
    }

    public PermissionsView(IServiceProvider provider, SessionContext session) : this()
    {
        _permissions = provider.GetRequiredService<PermissionService>();
        _categories = provider.GetRequiredService<CategoryService>();
        _personnel = provider.GetRequiredService<PersonnelService>();
        _secretRepo = provider.GetRequiredService<ISecretRepository>();
        _session = session;

        ThemeManager.Apply(this);
        Dock = DockStyle.Fill;

        _people.SelectedIndexChanged += async (_, _) => await LoadTreeAsync();
        _tree.AfterCheck += async (_, e) => await OnAfterCheckAsync(e.Node);
    }

    public async Task LoadAsync()
    {
        try
        {
            _personnelList = (await _personnel.ListPersonnelAsync()).ToList();
            _people.Items.Clear();
            foreach (var u in _personnelList)
                _people.Items.Add($"{u.FullName}  ({u.Username})");

            _tree.Nodes.Clear();
            if (_personnelList.Count > 0)
                _people.SelectedIndex = 0; // triggers LoadTreeAsync
            else
                Info("Henüz personel yok. 'Personel' sekmesinden ekleyin.");
        }
        catch (Exception ex) { Fail(ex.Message); }
    }

    private UserRecord? SelectedUser =>
        _people.SelectedIndex >= 0 && _people.SelectedIndex < _personnelList.Count
            ? _personnelList[_people.SelectedIndex]
            : null;

    private async Task LoadTreeAsync()
    {
        if (SelectedUser is not { } user) return;
        try
        {
            var categories = await _categories.ListAsync();
            var secrets = await _secretRepo.ListAllSummariesAsync();
            var state = await _permissions.GetStateAsync(user.Id);

            _suppress = true;
            _tree.BeginUpdate();
            _tree.Nodes.Clear();

            foreach (var category in categories)
            {
                bool categoryGranted = state.CategoryIds.Contains(category.Id);
                var catNode = new TreeNode(category.Name)
                {
                    Tag = new NodeRef(true, category.Id),
                    Checked = categoryGranted
                };

                foreach (var secret in secrets.Where(s => s.CategoryId == category.Id))
                {
                    bool denied = state.DeniedSecretIds.Contains(secret.Id);
                    bool visible = !denied && (categoryGranted || state.SecretIds.Contains(secret.Id));
                    var secNode = new TreeNode(secret.Title)
                    {
                        Tag = new NodeRef(false, secret.Id),
                        Checked = visible,
                        // Carve-out (kategori verili ama bu şifre kısıtlı) uyarı renginde.
                        ForeColor = (categoryGranted && denied) ? AppPalette.Warning : AppPalette.TextPrimary
                    };
                    catNode.Nodes.Add(secNode);
                }

                _tree.Nodes.Add(catNode);
            }

            _tree.ExpandAll();
            _tree.EndUpdate();
            _suppress = false;

            Info($"{user.FullName}: kategori bazında ver; verili kategoride tek tek şifre kutusunu kaldırarak istisna tanımla.");
        }
        catch (Exception ex) { _suppress = false; Fail(ex.Message); }
    }

    private async Task OnAfterCheckAsync(TreeNode? node)
    {
        if (_suppress || node?.Tag is not NodeRef nodeRef || SelectedUser is not { } user)
            return;

        try
        {
            if (nodeRef.IsCategory)
            {
                await _permissions.SetCategoryAsync(_session, user.Id, nodeRef.Id, node.Checked);

                if (node.Checked)
                {
                    // Full grant: every child is now visible (carve-outs were cleared).
                    _suppress = true;
                    foreach (TreeNode child in node.Nodes)
                    {
                        child.Checked = true;
                        child.ForeColor = AppPalette.TextPrimary;
                    }
                    _suppress = false;
                    Info($"'{node.Text}' kategorisinin tamamı verildi.");
                }
                else
                {
                    await LoadTreeAsync(); // restore any individual-secret grants
                    Info($"'{node.Text}' kategori erişimi kaldırıldı.");
                }
            }
            else
            {
                await _permissions.SetSecretAsync(_session, user.Id, nodeRef.Id, node.Checked);

                bool parentGranted = node.Parent is { Checked: true };
                node.ForeColor = (!node.Checked && parentGranted) ? AppPalette.Warning : AppPalette.TextPrimary;

                Info(node.Checked
                    ? $"'{node.Text}' bu personele açıldı."
                    : parentGranted
                        ? $"'{node.Text}' kısıtlandı — kategori verili olsa da bu personel göremez."
                        : $"'{node.Text}' erişimi kaldırıldı.");
            }
        }
        catch (Exception ex) { Fail(ex.Message); }
    }

    private void Info(string m) { _status.Text = m; _status.ForeColor = AppPalette.TextSecondary; }
    private void Fail(string m) { _status.Text = m; _status.ForeColor = AppPalette.Danger; }
}
