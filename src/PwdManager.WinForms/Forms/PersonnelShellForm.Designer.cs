namespace PwdManager.WinForms.Forms
{
    partial class PersonnelShellForm
    {
        private void InitializeComponent()
        {
            this._pageHeader = new Guna.UI2.WinForms.Guna2Panel();
            this._title = new System.Windows.Forms.Label();
            this._hint = new System.Windows.Forms.Label();
            this._headerDivider = new Guna.UI2.WinForms.Guna2Panel();
            this._toolbar = new System.Windows.Forms.FlowLayoutPanel();
            this._refresh = new Guna.UI2.WinForms.Guna2Button();
            this._card = new Guna.UI2.WinForms.Guna2Panel();
            this._viewHost = new System.Windows.Forms.Panel();
            this._status = new System.Windows.Forms.Label();
            this._pageHeader.SuspendLayout();
            this._toolbar.SuspendLayout();
            this._card.SuspendLayout();
            this.SuspendLayout();
            //
            // _pageHeader
            //
            this._pageHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this._pageHeader.Height = 84;
            this._pageHeader.Name = "_pageHeader";
            this._pageHeader.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._pageHeader.Controls.Add(this._hint);
            this._pageHeader.Controls.Add(this._title);
            //
            // _title
            //
            this._title.AutoSize = true;
            this._title.BackColor = System.Drawing.Color.Transparent;
            this._title.Font = new System.Drawing.Font("Segoe UI", 15.5F, System.Drawing.FontStyle.Bold);
            this._title.Location = new System.Drawing.Point(4, 14);
            this._title.Name = "_title";
            this._title.Size = new System.Drawing.Size(260, 28);
            this._title.TabIndex = 0;
            this._title.Text = "Erişebildiğiniz parolalar";
            //
            // _hint
            //
            this._hint.AutoSize = true;
            this._hint.BackColor = System.Drawing.Color.Transparent;
            this._hint.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._hint.Location = new System.Drawing.Point(6, 48);
            this._hint.Name = "_hint";
            this._hint.Size = new System.Drawing.Size(420, 15);
            this._hint.TabIndex = 1;
            this._hint.Tag = "muted";
            this._hint.Text = "Liste canlı güncellenir. Şifreyi görmek için satıra çift tıklayın.";
            //
            // _headerDivider
            //
            this._headerDivider.Dock = System.Windows.Forms.DockStyle.Top;
            this._headerDivider.Height = 1;
            this._headerDivider.Name = "_headerDivider";
            this._headerDivider.Tag = "divider";
            //
            // _toolbar
            //
            this._toolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this._toolbar.Height = 58;
            this._toolbar.Name = "_toolbar";
            this._toolbar.Padding = new System.Windows.Forms.Padding(0, 12, 0, 8);
            this._toolbar.WrapContents = false;
            this._toolbar.Controls.Add(this._refresh);
            //
            // _refresh
            //
            this._refresh.BorderRadius = 10;
            this._refresh.Margin = new System.Windows.Forms.Padding(0);
            this._refresh.Name = "_refresh";
            this._refresh.Size = new System.Drawing.Size(104, 38);
            this._refresh.TabIndex = 0;
            this._refresh.Tag = "secondary";
            this._refresh.Text = "Yenile";
            //
            // _card
            //
            this._card.Dock = System.Windows.Forms.DockStyle.Fill;
            this._card.Name = "_card";
            this._card.Padding = new System.Windows.Forms.Padding(4);
            this._card.Tag = "card";
            this._card.Controls.Add(this._viewHost);
            //
            // _viewHost
            //
            this._viewHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this._viewHost.Name = "_viewHost";
            //
            // _status
            //
            this._status.BackColor = System.Drawing.Color.Transparent;
            this._status.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._status.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this._status.Height = 30;
            this._status.Name = "_status";
            this._status.Padding = new System.Windows.Forms.Padding(4, 8, 4, 4);
            this._status.TabIndex = 3;
            //
            // PersonnelShellForm
            //
            this.Content.Controls.Add(this._card);
            this.Content.Controls.Add(this._toolbar);
            this.Content.Controls.Add(this._headerDivider);
            this.Content.Controls.Add(this._pageHeader);
            this.Content.Controls.Add(this._status);
            this.Name = "PersonnelShellForm";
            this._pageHeader.ResumeLayout(false);
            this._pageHeader.PerformLayout();
            this._toolbar.ResumeLayout(false);
            this._card.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private Guna.UI2.WinForms.Guna2Panel _pageHeader;
        private System.Windows.Forms.Label _title;
        private System.Windows.Forms.Label _hint;
        private Guna.UI2.WinForms.Guna2Panel _headerDivider;
        private System.Windows.Forms.FlowLayoutPanel _toolbar;
        private Guna.UI2.WinForms.Guna2Button _refresh;
        private Guna.UI2.WinForms.Guna2Panel _card;
        private System.Windows.Forms.Panel _viewHost;
        private System.Windows.Forms.Label _status;
    }
}
