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
            this._identityLabel = new System.Windows.Forms.Label();
            this._newWindowButton = new Guna.UI2.WinForms.Guna2Button();
            this._logoutButton = new Guna.UI2.WinForms.Guna2Button();
            this.Content = new System.Windows.Forms.Panel();
            this._topBar.SuspendLayout();
            this.SuspendLayout();
            //
            // _topBar
            //
            this._topBar.Dock = System.Windows.Forms.DockStyle.Top;
            this._topBar.Height = 56;
            this._topBar.Name = "_topBar";
            this._topBar.Controls.Add(this._identityLabel);
            this._topBar.Controls.Add(this._newWindowButton);
            this._topBar.Controls.Add(this._logoutButton);
            //
            // _identityLabel
            //
            this._identityLabel.AutoSize = true;
            this._identityLabel.BackColor = System.Drawing.Color.Transparent;
            this._identityLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this._identityLabel.Location = new System.Drawing.Point(20, 18);
            this._identityLabel.Name = "_identityLabel";
            this._identityLabel.Size = new System.Drawing.Size(40, 19);
            this._identityLabel.TabIndex = 0;
            this._identityLabel.Text = "—";
            //
            // _newWindowButton
            //
            this._newWindowButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._newWindowButton.BorderRadius = 8;
            this._newWindowButton.Location = new System.Drawing.Point(808, 11);
            this._newWindowButton.Name = "_newWindowButton";
            this._newWindowButton.Size = new System.Drawing.Size(130, 34);
            this._newWindowButton.TabIndex = 1;
            this._newWindowButton.Tag = "secondary";
            this._newWindowButton.Text = "Yeni pencere";
            //
            // _logoutButton
            //
            this._logoutButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._logoutButton.BorderRadius = 8;
            this._logoutButton.Location = new System.Drawing.Point(950, 11);
            this._logoutButton.Name = "_logoutButton";
            this._logoutButton.Size = new System.Drawing.Size(110, 34);
            this._logoutButton.TabIndex = 2;
            this._logoutButton.Tag = "secondary";
            this._logoutButton.Text = "Çıkış yap";
            //
            // Content
            //
            this.Content.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Content.Name = "Content";
            this.Content.Padding = new System.Windows.Forms.Padding(16);
            //
            // ShellFormBase
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1080, 680);
            this.Controls.Add(this.Content);
            this.Controls.Add(this._topBar);
            this.MinimumSize = new System.Drawing.Size(900, 560);
            this.Name = "ShellFormBase";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PwdManager";
            this._topBar.ResumeLayout(false);
            this._topBar.PerformLayout();
            this.ResumeLayout(false);
        }

        private Guna.UI2.WinForms.Guna2Panel _topBar;
        private System.Windows.Forms.Label _identityLabel;
        private Guna.UI2.WinForms.Guna2Button _newWindowButton;
        private Guna.UI2.WinForms.Guna2Button _logoutButton;
        protected System.Windows.Forms.Panel Content;
    }
}
