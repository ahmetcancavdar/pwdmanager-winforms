namespace PwdManager.WinForms.Forms
{
    partial class ShellFormBase
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _idleTimer?.Dispose();
                if (components != null) components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this._topBar = new Guna.UI2.WinForms.Guna2Panel();
            this._brandDot = new Guna.UI2.WinForms.Guna2Panel();
            this._appName = new System.Windows.Forms.Label();
            this._identityLabel = new System.Windows.Forms.Label();
            this._logoutButton = new Guna.UI2.WinForms.Guna2Button();
            this._topDivider = new Guna.UI2.WinForms.Guna2Panel();
            this.Content = new System.Windows.Forms.Panel();
            this._topBar.SuspendLayout();
            this.SuspendLayout();
            //
            // _topBar
            //
            this._topBar.Dock = System.Windows.Forms.DockStyle.Top;
            this._topBar.Height = 64;
            this._topBar.Name = "_topBar";
            this._topBar.Tag = "topbar";
            this._topBar.Controls.Add(this._identityLabel);
            this._topBar.Controls.Add(this._appName);
            this._topBar.Controls.Add(this._brandDot);
            this._topBar.Controls.Add(this._logoutButton);
            //
            // _brandDot
            //
            this._brandDot.BorderRadius = 8;
            this._brandDot.FillColor = System.Drawing.Color.FromArgb(124, 92, 255);
            this._brandDot.Location = new System.Drawing.Point(20, 12);
            this._brandDot.Name = "_brandDot";
            this._brandDot.Size = new System.Drawing.Size(40, 40);
            this._brandDot.TabIndex = 0;
            this._brandDot.Tag = "accent";
            //
            // _appName
            //
            this._appName.AutoSize = true;
            this._appName.BackColor = System.Drawing.Color.Transparent;
            this._appName.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this._appName.Location = new System.Drawing.Point(72, 21);
            this._appName.Name = "_appName";
            this._appName.Size = new System.Drawing.Size(110, 21);
            this._appName.TabIndex = 1;
            this._appName.Text = "PwdManager";
            //
            // _identityLabel
            //
            this._identityLabel.AutoSize = true;
            this._identityLabel.BackColor = System.Drawing.Color.Transparent;
            this._identityLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this._identityLabel.Location = new System.Drawing.Point(196, 23);
            this._identityLabel.Name = "_identityLabel";
            this._identityLabel.Size = new System.Drawing.Size(14, 15);
            this._identityLabel.TabIndex = 2;
            this._identityLabel.Tag = "muted";
            this._identityLabel.Text = "—";
            //
            // _logoutButton
            //
            this._logoutButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._logoutButton.BorderRadius = 10;
            this._logoutButton.Location = new System.Drawing.Point(946, 14);
            this._logoutButton.Name = "_logoutButton";
            this._logoutButton.Size = new System.Drawing.Size(112, 36);
            this._logoutButton.TabIndex = 3;
            this._logoutButton.Tag = "secondary";
            this._logoutButton.Text = "Çıkış yap";
            //
            // _topDivider
            //
            this._topDivider.Dock = System.Windows.Forms.DockStyle.Top;
            this._topDivider.Height = 1;
            this._topDivider.Name = "_topDivider";
            this._topDivider.Tag = "divider";
            //
            // Content
            //
            this.Content.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Content.Name = "Content";
            this.Content.Padding = new System.Windows.Forms.Padding(20);
            //
            // ShellFormBase
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1080, 680);
            this.Controls.Add(this.Content);
            this.Controls.Add(this._topDivider);
            this.Controls.Add(this._topBar);
            this.MinimumSize = new System.Drawing.Size(940, 580);
            this.Name = "ShellFormBase";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PwdManager";
            this._topBar.ResumeLayout(false);
            this._topBar.PerformLayout();
            this.ResumeLayout(false);
        }

        private Guna.UI2.WinForms.Guna2Panel _topBar;
        private Guna.UI2.WinForms.Guna2Panel _brandDot;
        private System.Windows.Forms.Label _appName;
        private System.Windows.Forms.Label _identityLabel;
        private Guna.UI2.WinForms.Guna2Button _logoutButton;
        private Guna.UI2.WinForms.Guna2Panel _topDivider;
        protected System.Windows.Forms.Panel Content;
    }
}
