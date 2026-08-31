namespace PwdManager.WinForms.Forms.Admin;

/// <summary>An admin content view that loads its data asynchronously when shown.</summary>
public interface IAdminView
{
    Task LoadAsync();
}
