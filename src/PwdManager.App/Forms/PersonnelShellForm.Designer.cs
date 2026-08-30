namespace PwdManager.App.Forms
{
    partial class PersonnelShellForm
    {
        private void InitializeComponent()
        {
            this._heading = new System.Windows.Forms.Label();
            this._toolbar = new System.Windows.Forms.FlowLayoutPanel();
            this._refresh = new Guna.UI2.WinForms.Guna2Button();
            this._hint = new System.Windows.Forms.Label();
            this._viewHost = new System.Windows.Forms.Panel();
            this._status = new System.Windows.Forms.Label();
            this._toolbar.SuspendLayout();
            this.SuspendLayout();
            //
            // _heading
            //
            this._heading.AutoSize = true;
            this._heading.BackColor = System.Drawing.Color.Transparent;
            this._heading.Dock = System.Windows.Forms.DockStyle.Top;
            this._heading.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this._heading.Padding = new System.Windows.Forms.Padding(4, 4, 0, 8);
            this._heading.Name = "_heading";
            this._heading.Size = new System.Drawing.Size(240, 40);
            this._heading.TabIndex = 0;
            this._heading.Text = "Erişebildiğiniz parolalar";
            //
            // _toolbar
            //
            this._toolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this._toolbar.Height = 52;
            this._toolbar.Padding = new System.Windows.Forms.Padding(4, 6, 0, 6);
            this._toolbar.Name = "_toolbar";
            this._toolbar.Controls.Add(this._refresh);
            this._toolbar.Controls.Add(this._hint);
            //
            // _refresh
            //
            this._refresh.BorderRadius = 8;
            this._refresh.Size = new System.Drawing.Size(100, 38);
            this._refresh.Name = "_refresh";
            this._refresh.TabIndex = 0;
            this._refresh.Tag = "secondary";
            this._refresh.Text = "Yenile";
            //
            // _hint
            //
            this._hint.AutoSize = true;
            this._hint.BackColor = System.Drawing.Color.Transparent;
            this._hint.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this._hint.Margin = new System.Windows.Forms.Padding(12, 10, 0, 0);
            this._hint.Name = "_hint";
            this._hint.Size = new System.Drawing.Size(420, 19);
            this._hint.Text = "Liste canlı güncellenir. Şifreyi görmek için satıra çift tıklayın.";
            //
            // _viewHost
            //
            this._viewHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this._viewHost.Padding = new System.Windows.Forms.Padding(4, 0, 4, 4);
            this._viewHost.Name = "_viewHost";
            //
            // _status
            //
            this._status.BackColor = System.Drawing.Color.Transparent;
            this._status.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._status.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this._status.Height = 22;
            this._status.Name = "_status";
            this._status.TabIndex = 3;
            //
            // PersonnelShellForm
            //
            this.Content.Controls.Add(this._viewHost);
            this.Content.Controls.Add(this._toolbar);
            this.Content.Controls.Add(this._heading);
            this.Content.Controls.Add(this._status);
            this.Name = "PersonnelShellForm";
            this._toolbar.ResumeLayout(false);
            this._toolbar.PerformLayout();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Label _heading;
        private System.Windows.Forms.FlowLayoutPanel _toolbar;
        private Guna.UI2.WinForms.Guna2Button _refresh;
        private System.Windows.Forms.Label _hint;
        private System.Windows.Forms.Panel _viewHost;
        private System.Windows.Forms.Label _status;
    }
}
