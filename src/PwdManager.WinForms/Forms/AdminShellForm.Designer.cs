namespace PwdManager.WinForms.Forms
{
    partial class AdminShellForm
    {
        private void InitializeComponent()
        {
            this._nav = new Guna.UI2.WinForms.Guna2Panel();
            this._navLabel = new System.Windows.Forms.Label();
            this._navCategories = new Guna.UI2.WinForms.Guna2Button();
            this._navSecrets = new Guna.UI2.WinForms.Guna2Button();
            this._navPersonnel = new Guna.UI2.WinForms.Guna2Button();
            this._navPermissions = new Guna.UI2.WinForms.Guna2Button();
            this._navAudit = new Guna.UI2.WinForms.Guna2Button();
            this._navTrash = new Guna.UI2.WinForms.Guna2Button();
            this._navDivider = new Guna.UI2.WinForms.Guna2Panel();
            this._viewHost = new System.Windows.Forms.Panel();
            this._nav.SuspendLayout();
            this.SuspendLayout();
            //
            // _nav
            //
            this._nav.Dock = System.Windows.Forms.DockStyle.Left;
            this._nav.Width = 224;
            this._nav.Padding = new System.Windows.Forms.Padding(14, 12, 14, 16);
            this._nav.Name = "_nav";
            this._nav.Tag = "nav";
            this._nav.Controls.Add(this._navCategories);
            this._nav.Controls.Add(this._navSecrets);
            this._nav.Controls.Add(this._navPersonnel);
            this._nav.Controls.Add(this._navPermissions);
            this._nav.Controls.Add(this._navAudit);
            this._nav.Controls.Add(this._navTrash);
            this._nav.Controls.Add(this._navLabel);
            //
            // _navLabel
            //
            this._navLabel.AutoSize = true;
            this._navLabel.BackColor = System.Drawing.Color.Transparent;
            this._navLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this._navLabel.Location = new System.Drawing.Point(18, 14);
            this._navLabel.Name = "_navLabel";
            this._navLabel.Size = new System.Drawing.Size(40, 13);
            this._navLabel.TabIndex = 0;
            this._navLabel.Tag = "overline";
            this._navLabel.Text = "YÖNETİM";
            //
            // _navCategories
            //
            this._navCategories.Location = new System.Drawing.Point(14, 40);
            this._navCategories.Size = new System.Drawing.Size(196, 42);
            this._navCategories.BorderRadius = 10;
            this._navCategories.Name = "_navCategories";
            this._navCategories.TabIndex = 1;
            this._navCategories.Tag = "nav";
            this._navCategories.Text = "    Kategoriler";
            this._navCategories.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            //
            // _navSecrets
            //
            this._navSecrets.Location = new System.Drawing.Point(14, 88);
            this._navSecrets.Size = new System.Drawing.Size(196, 42);
            this._navSecrets.BorderRadius = 10;
            this._navSecrets.Name = "_navSecrets";
            this._navSecrets.TabIndex = 2;
            this._navSecrets.Tag = "nav";
            this._navSecrets.Text = "    Parolalar";
            this._navSecrets.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            //
            // _navPersonnel
            //
            this._navPersonnel.Location = new System.Drawing.Point(14, 136);
            this._navPersonnel.Size = new System.Drawing.Size(196, 42);
            this._navPersonnel.BorderRadius = 10;
            this._navPersonnel.Name = "_navPersonnel";
            this._navPersonnel.TabIndex = 3;
            this._navPersonnel.Tag = "nav";
            this._navPersonnel.Text = "    Personel";
            this._navPersonnel.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            //
            // _navPermissions
            //
            this._navPermissions.Location = new System.Drawing.Point(14, 184);
            this._navPermissions.Size = new System.Drawing.Size(196, 42);
            this._navPermissions.BorderRadius = 10;
            this._navPermissions.Name = "_navPermissions";
            this._navPermissions.TabIndex = 4;
            this._navPermissions.Tag = "nav";
            this._navPermissions.Text = "    Yetkiler";
            this._navPermissions.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            //
            // _navAudit
            //
            this._navAudit.Location = new System.Drawing.Point(14, 232);
            this._navAudit.Size = new System.Drawing.Size(196, 42);
            this._navAudit.BorderRadius = 10;
            this._navAudit.Name = "_navAudit";
            this._navAudit.TabIndex = 5;
            this._navAudit.Tag = "nav";
            this._navAudit.Text = "    Denetim";
            this._navAudit.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            //
            // _navTrash
            //
            this._navTrash.Location = new System.Drawing.Point(14, 280);
            this._navTrash.Size = new System.Drawing.Size(196, 42);
            this._navTrash.BorderRadius = 10;
            this._navTrash.Name = "_navTrash";
            this._navTrash.TabIndex = 6;
            this._navTrash.Tag = "nav";
            this._navTrash.Text = "    Silinenler";
            this._navTrash.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            //
            // _navDivider
            //
            this._navDivider.Dock = System.Windows.Forms.DockStyle.Left;
            this._navDivider.Width = 1;
            this._navDivider.Name = "_navDivider";
            this._navDivider.Tag = "divider";
            //
            // _viewHost
            //
            this._viewHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this._viewHost.Padding = new System.Windows.Forms.Padding(24, 8, 8, 8);
            this._viewHost.Name = "_viewHost";
            //
            // AdminShellForm
            //
            this.Content.Controls.Add(this._viewHost);
            this.Content.Controls.Add(this._navDivider);
            this.Content.Controls.Add(this._nav);
            this.Content.Padding = new System.Windows.Forms.Padding(0);
            this.Name = "AdminShellForm";
            this._nav.ResumeLayout(false);
            this._nav.PerformLayout();
            this.ResumeLayout(false);
        }

        private Guna.UI2.WinForms.Guna2Panel _nav;
        private System.Windows.Forms.Label _navLabel;
        private Guna.UI2.WinForms.Guna2Button _navCategories;
        private Guna.UI2.WinForms.Guna2Button _navSecrets;
        private Guna.UI2.WinForms.Guna2Button _navPersonnel;
        private Guna.UI2.WinForms.Guna2Button _navPermissions;
        private Guna.UI2.WinForms.Guna2Button _navAudit;
        private Guna.UI2.WinForms.Guna2Button _navTrash;
        private Guna.UI2.WinForms.Guna2Panel _navDivider;
        private System.Windows.Forms.Panel _viewHost;
    }
}
