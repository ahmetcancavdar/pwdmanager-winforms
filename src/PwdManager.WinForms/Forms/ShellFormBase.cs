using PwdManager.WinForms.Theme;
using PwdManager.Domain.Security;
using WinFormsApp = System.Windows.Forms.Application;

namespace PwdManager.WinForms.Forms;

/// <summary>
/// Shared chrome for the role shells (top bar with identity + logout + "new window",
/// and a <see cref="Content"/> host). Disposes the session on close and locks back to
/// login after a period with no user input. The chrome layout lives in the designer;
/// this file wires behaviour.
/// </summary>
// Not abstract: the WinForms designer cannot open a form whose base type is abstract.
// It is still never instantiated directly — only via AdminShellForm / PersonnelShellForm.
public partial class ShellFormBase : Form, IShellForm, IMessageFilter
{
    private const int WM_KEYDOWN = 0x0100;
    private const int WM_SYSKEYDOWN = 0x0104;
    private const int WM_LBUTTONDOWN = 0x0201;
    private const int WM_RBUTTONDOWN = 0x0204;
    private const int WM_MBUTTONDOWN = 0x0207;
    private const int WM_MOUSEWHEEL = 0x020A;
    private const int WM_NCLBUTTONDOWN = 0x00A1;
    private const int WM_MOUSEMOVE = 0x0200;

    protected SessionContext Session { get; private set; } = null!;

    /// <summary>Set when the shell closed itself (idle timeout, account deactivated, …).</summary>
    public string? ExitNotice { get; private set; }

    private readonly TimeSpan _idleTimeout;
    private readonly System.Windows.Forms.Timer _idleTimer;
    private DateTime _lastActivityUtc = DateTime.UtcNow;
    private Point _lastMousePos;

    /// <summary>Designer-only constructor.</summary>
    protected ShellFormBase() : this("PwdManager", 5)
    {
    }

    protected ShellFormBase(string roleCaption, int idleLockMinutes)
    {
        InitializeComponent();
        ThemeManager.Apply(this);

        _idleTimeout = TimeSpan.FromMinutes(Math.Max(1, idleLockMinutes));
        Text = $"PwdManager — {roleCaption}";

        _logoutButton.Click += (_, _) => Close();
        _newWindowButton.Click += (_, _) => LaunchNewInstance();

        _idleTimer = new System.Windows.Forms.Timer { Interval = 15_000 };
        _idleTimer.Tick += (_, _) =>
        {
            if (DateTime.UtcNow - _lastActivityUtc >= _idleTimeout)
                CloseWithNotice("Hareketsizlik nedeniyle oturum kilitlendi. Lütfen tekrar giriş yapın.");
        };
    }

    public void Attach(SessionContext session)
    {
        Session = session;
        _identityLabel.Text = $"{session.User.FullName}  ·  {session.User.Username}  ·  {session.User.Role}";
        OnSessionAttached();
    }

    /// <summary>Called after the session is available; build role-specific content here.</summary>
    protected virtual void OnSessionAttached() { }

    /// <summary>Close the shell and hand a reason back to the login screen.</summary>
    public void CloseWithNotice(string notice)
    {
        ExitNotice = notice;
        if (IsHandleCreated && !IsDisposed)
            BeginInvoke(new Action(Close));
        else
            Close();
    }

    /// <summary>Starts a second, fully independent copy of the app (for a parallel account).</summary>
    public static void LaunchNewInstance()
    {
        try
        {
            string exe = WinFormsApp.ExecutablePath;
            if (!string.IsNullOrEmpty(exe))
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(exe) { UseShellExecute = true });
        }
        catch
        {
            // best effort; nothing to recover
        }
    }

    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);
        WinFormsApp.AddMessageFilter(this);
        _lastActivityUtc = DateTime.UtcNow;
        _idleTimer.Start();
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        _idleTimer.Stop();
        WinFormsApp.RemoveMessageFilter(this);
        Session?.Dispose();
        base.OnFormClosed(e);
    }

    bool IMessageFilter.PreFilterMessage(ref Message m)
    {
        switch (m.Msg)
        {
            case WM_KEYDOWN:
            case WM_SYSKEYDOWN:
            case WM_LBUTTONDOWN:
            case WM_RBUTTONDOWN:
            case WM_MBUTTONDOWN:
            case WM_MOUSEWHEEL:
            case WM_NCLBUTTONDOWN:
                _lastActivityUtc = DateTime.UtcNow;
                break;
            case WM_MOUSEMOVE:
                if (Cursor.Position != _lastMousePos)
                {
                    _lastMousePos = Cursor.Position;
                    _lastActivityUtc = DateTime.UtcNow;
                }
                break;
        }
        return false;
    }
}
