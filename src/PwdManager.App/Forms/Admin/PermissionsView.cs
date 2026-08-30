using Microsoft.Extensions.DependencyInjection;
using PwdManager.App.Services;
using PwdManager.App.Theme;
using PwdManager.Core.Security;
using PwdManager.Data.Entities;
using PwdManager.Data.Repositories;

namespace PwdManager.App.Forms.Admin;

/// <summary>
/// Left: personnel list. Right: category/secret tree with checkboxes.
/// Checking a category grants the whole category; checking a single secret grants just
/// that secret. Every toggle is written to the database immediately and bumps the
/// personnel's sync counter, so the change reaches them within seconds.
/// </summary>
public sealed partial class PermissionsView : UserControl, IAdminView
{
    private sealed record NodeRef(bool IsCategory, long Id);

    private readonly PermissionService _permissions = null!;
    private readonly CategoryService _categories = null!;
    private readonly PersonnelService _personnel = null!;
    private readonly ISecretRepository _secretRepo = null!;
    private readonly SessionContext _session = null!;

    private List<User> _personnelList = new();
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

    private User? SelectedUser =>
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
                    bool secretGranted = categoryGranted || state.SecretIds.Contains(secret.Id);
                    var secNode = new TreeNode(secret.Title)
                    {
                        Tag = new NodeRef(false, secret.Id),
                        Checked = secretGranted,
                        ForeColor = categoryGranted ? AppPalette.TextDisabled : AppPalette.TextPrimary
                    };
                    catNode.Nodes.Add(secNode);
                }

                _tree.Nodes.Add(catNode);
            }

            _tree.ExpandAll();
            _tree.EndUpdate();
            _suppress = false;

            Info($"{user.FullName}: kategori bazında veya tek tek parola bazında erişim verin.");
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

                _suppress = true;
                foreach (TreeNode child in node.Nodes)
                {
                    child.Checked = node.Checked;
                    child.ForeColor = node.Checked ? AppPalette.TextDisabled : AppPalette.TextPrimary;
                }
                _suppress = false;

                if (!node.Checked)
                    await LoadTreeAsync();

                Info(node.Checked
                    ? $"'{node.Text}' kategorisinin tamamı verildi."
                    : $"'{node.Text}' kategori erişimi kaldırıldı.");
            }
            else
            {
                if (node.Parent is { Checked: true })
                {
                    _suppress = true;
                    node.Checked = true;
                    _suppress = false;
                    Info("Bu parola, kategorisi tümüyle verildiği için zaten erişilebilir.");
                    return;
                }

                await _permissions.SetSecretAsync(_session, user.Id, nodeRef.Id, node.Checked);
                Info(node.Checked ? $"'{node.Text}' parolasına erişim verildi." : $"'{node.Text}' erişimi kaldırıldı.");
            }
        }
        catch (Exception ex) { Fail(ex.Message); }
    }

    private void Info(string m) { _status.Text = m; _status.ForeColor = AppPalette.TextSecondary; }
    private void Fail(string m) { _status.Text = m; _status.ForeColor = AppPalette.Danger; }
}
