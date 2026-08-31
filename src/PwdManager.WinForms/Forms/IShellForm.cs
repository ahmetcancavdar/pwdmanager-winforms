using PwdManager.Application.Security;

namespace PwdManager.WinForms.Forms;

/// <summary>Implemented by the role shells so the login form can hand over the session.</summary>
public interface IShellForm
{
    void Attach(SessionContext session);
}
