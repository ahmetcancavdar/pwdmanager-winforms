namespace PwdManager.App.Forms
{
    partial class AdminShellForm
    {
        private void InitializeComponent()
        {
            this._nav = new Guna.UI2.WinForms.Guna2Panel();
            this._viewHost = new System.Windows.Forms.Panel();
            this._navCategories = new Guna.UI2.WinForms.Guna2Button();
            this._navSecrets = new Guna.UI2.WinForms.Guna2Button();
            this._navPersonnel = new Guna.UI2.WinForms.Guna2Button();
            this._navPermissions = new Guna.UI2.WinForms.Guna2Button();
            this._navAudit = new Guna.UI2.WinForms.Guna2Button();
            this._nav.SuspendLayout();
            this.SuspendLayout();
            //
            // _nav
            //
            this._nav.Dock = System.Windows.Forms.DockStyle.Left;
            this._nav.Width = 210;
            this._nav.Padding = new System.Windows.Forms.Padding(12, 16, 12, 16);
            this._nav.Name = "_nav";
            this._nav.Controls.Add(this._navCategories);
            this._nav.Controls.Add(this._navSecrets);
            this._nav.Controls.Add(this._navPersonnel);
            this._nav.Controls.Add(this._navPermissions);
            this._nav.Controls.Add(this._navAudit);
            //
            // _viewHost
            //
            this._viewHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this._viewHost.Padding = new System.Windows.Forms.Padding(16);
            this._viewHost.Name = "_viewHost";
            //
            // nav buttons
            //
            this._navCategories.Location = new System.Drawing.Point(12, 16);
            this._navCategories.Size = new System.Drawing.Size(186, 44);
            this._navCategories.BorderRadius = 8;
            this._navCategories.Name = "_navCategories";
            this._navCategories.TabIndex = 0;
            this._navCategories.Tag = "secondary";
            this._navCategories.Text = "   Kategoriler";
            this._navCategories.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            //
            this._navSecrets.Location = new System.Drawing.Point(12, 66);
            this._navSecrets.Size = new System.Drawing.Size(186, 44);
            this._navSecrets.BorderRadius = 8;
            this._navSecrets.Name = "_navSecrets";
            this._navSecrets.TabIndex = 1;
            this._navSecrets.Tag = "secondary";
            this._navSecrets.Text = "   Parolalar";
            this._navSecrets.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            //
            this._navPersonnel.Location = new System.Drawing.Point(12, 116);
            this._navPersonnel.Size = new System.Drawing.Size(186, 44);
            this._navPersonnel.BorderRadius = 8;
            this._navPersonnel.Name = "_navPersonnel";
            this._navPersonnel.TabIndex = 2;
            this._navPersonnel.Tag = "secondary";
            this._navPersonnel.Text = "   Personel";
            this._navPersonnel.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            //
            this._navPermissions.Location = new System.Drawing.Point(12, 166);
            this._navPermissions.Size = new System.Drawing.Size(186, 44);
            this._navPermissions.BorderRadius = 8;
            this._navPermissions.Name = "_navPermissions";
            this._navPermissions.TabIndex = 3;
            this._navPermissions.Tag = "secondary";
            this._navPermissions.Text = "   Yetkiler";
            this._navPermissions.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            //
            this._navAudit.Location = new System.Drawing.Point(12, 216);
            this._navAudit.Size = new System.Drawing.Size(186, 44);
            this._navAudit.BorderRadius = 8;
            this._navAudit.Name = "_navAudit";
            this._navAudit.TabIndex = 4;
            this._navAudit.Tag = "secondary";
            this._navAudit.Text = "   Denetim";
            this._navAudit.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            //
            // AdminShellForm
            //
            this.Content.Controls.Add(this._viewHost);
            this.Content.Controls.Add(this._nav);
            this.Content.Padding = new System.Windows.Forms.Padding(0);
            this.Name = "AdminShellForm";
            this._nav.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private Guna.UI2.WinForms.Guna2Panel _nav;
        private System.Windows.Forms.Panel _viewHost;
        private Guna.UI2.WinForms.Guna2Button _navCategories;
        private Guna.UI2.WinForms.Guna2Button _navSecrets;
        private Guna.UI2.WinForms.Guna2Button _navPersonnel;
        private Guna.UI2.WinForms.Guna2Button _navPermissions;
        private Guna.UI2.WinForms.Guna2Button _navAudit;
    }
}
