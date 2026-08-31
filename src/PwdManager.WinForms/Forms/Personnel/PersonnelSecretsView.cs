using Guna.UI2.WinForms;
using PwdManager.WinForms.Theme;
using PwdManager.WinForms.Controls;
using PwdManager.Application.Security;
using PwdManager.Application.DTOs;

namespace PwdManager.WinForms.Forms.Personnel;

/// <summary>
/// Vertical, grouped table of the secrets a personnel may see. One section per
/// category (categories with no accessible secret are simply never added), a column
/// header, then a <see cref="SecretRowControl"/> per secret.
/// </summary>
public sealed partial class PersonnelSecretsView : UserControl
{
    private readonly IServiceProvider _provider = null!;
    private readonly SessionContext _session = null!;

    /// <summary>Designer-only constructor.</summary>
    public PersonnelSecretsView()
    {
        InitializeComponent();
    }

    public PersonnelSecretsView(IServiceProvider provider, SessionContext session) : this()
    {
        _provider = provider;
        _session = session;

        ThemeManager.Apply(this);
        Dock = DockStyle.Fill;
        _list.Resize += (_, _) => ResizeChildren();
    }

    public void Render(IReadOnlyList<SecretSummary> secrets)
    {
        _list.SuspendLayout();
        foreach (Control c in _list.Controls.Cast<Control>().ToArray())
            c.Dispose();
        _list.Controls.Clear();

        if (secrets.Count == 0)
        {
            _list.Controls.Add(new Label
            {
                Text = "Henüz size bir parola erişimi verilmemiş.",
                Font = AppFonts.Body,
                ForeColor = AppPalette.TextSecondary,
                AutoSize = true,
                Margin = new Padding(8, 16, 8, 8)
            });
            _list.ResumeLayout();
            return;
        }

        foreach (var group in secrets.GroupBy(s => s.CategoryName).OrderBy(g => g.Key, StringComparer.CurrentCultureIgnoreCase))
        {
            _list.Controls.Add(CategoryHeader(group.Key, group.Count()));
            _list.Controls.Add(ColumnHeader());
            foreach (var secret in group.OrderBy(s => s.Title, StringComparer.CurrentCultureIgnoreCase))
                _list.Controls.Add(new SecretRowControl(_provider, _session, secret.Id, secret.Title));
        }

        ResizeChildren();
        _list.ResumeLayout();
    }

    private int RowWidth => Math.Max(320,
        _list.ClientSize.Width - _list.Padding.Horizontal - SystemInformation.VerticalScrollBarWidth - 2);

    private void ResizeChildren()
    {
        int w = RowWidth;
        foreach (Control c in _list.Controls)
            c.Width = w;
    }

    private static Guna2Panel CategoryHeader(string name, int count)
    {
        var panel = new Guna2Panel
        {
            Height = 38,
            Margin = new Padding(0, 10, 0, 4),
            FillColor = AppPalette.SurfaceAlt,
            BorderRadius = 6
        };
        panel.Controls.Add(new Label
        {
            Text = $"{name}   ·   {count} parola",
            Font = AppFonts.Subtitle,
            ForeColor = AppPalette.TextPrimary,
            AutoSize = true,
            Location = new Point(12, 9),
            BackColor = Color.Transparent
        });
        return panel;
    }

    private static Panel ColumnHeader()
    {
        var panel = new Panel { Height = 24, Margin = new Padding(4, 0, 0, 2), BackColor = Color.Transparent };
        panel.Controls.Add(HeaderLabel("Başlık", 16));
        panel.Controls.Add(HeaderLabel("Kullanıcı adı", 300));
        var right = HeaderLabel("Parola", 0);
        right.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        right.Location = new Point(panel.Width - 220, 4);
        panel.Controls.Add(right);
        return panel;

        static Label HeaderLabel(string text, int x) => new()
        {
            Text = text,
            Font = AppFonts.Small,
            ForeColor = AppPalette.TextSecondary,
            AutoSize = true,
            Location = new Point(x, 4),
            BackColor = Color.Transparent
        };
    }
}
