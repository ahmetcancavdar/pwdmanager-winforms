namespace PwdManager.WinForms.Forms.Admin
{
    partial class TrashView
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this._heading = new System.Windows.Forms.Label();
            this._toolbar = new System.Windows.Forms.FlowLayoutPanel();
            this._restore = new Guna.UI2.WinForms.Guna2Button();
            this._purge = new Guna.UI2.WinForms.Guna2Button();
            this._refresh = new Guna.UI2.WinForms.Guna2Button();
            this._gridHost = new System.Windows.Forms.Panel();
            this._grid = new Guna.UI2.WinForms.Guna2DataGridView();
            this._status = new System.Windows.Forms.Label();
            this._toolbar.SuspendLayout();
            this._gridHost.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._grid)).BeginInit();
            this.SuspendLayout();
            //
            this._heading.AutoSize = true;
            this._heading.BackColor = System.Drawing.Color.Transparent;
            this._heading.Dock = System.Windows.Forms.DockStyle.Top;
            this._heading.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this._heading.Padding = new System.Windows.Forms.Padding(2, 2, 0, 6);
            this._heading.Name = "_heading";
            this._heading.Size = new System.Drawing.Size(110, 35);
            this._heading.Text = "Silinenler";
            //
            this._toolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this._toolbar.Height = 52;
            this._toolbar.Padding = new System.Windows.Forms.Padding(0, 6, 0, 6);
            this._toolbar.Name = "_toolbar";
            this._toolbar.Controls.Add(this._restore);
            this._toolbar.Controls.Add(this._purge);
            this._toolbar.Controls.Add(this._refresh);
            //
            this._restore.BorderRadius = 8;
            this._restore.Size = new System.Drawing.Size(120, 38);
            this._restore.Name = "_restore";
            this._restore.Text = "Geri yükle";
            //
            this._purge.BorderRadius = 8;
            this._purge.Size = new System.Drawing.Size(120, 38);
            this._purge.Name = "_purge";
            this._purge.Tag = "secondary";
            this._purge.Text = "Kalıcı sil";
            //
            this._refresh.BorderRadius = 8;
            this._refresh.Size = new System.Drawing.Size(90, 38);
            this._refresh.Name = "_refresh";
            this._refresh.Tag = "secondary";
            this._refresh.Text = "Yenile";
            //
            this._gridHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this._gridHost.Padding = new System.Windows.Forms.Padding(0, 8, 0, 8);
            this._gridHost.Name = "_gridHost";
            this._gridHost.Controls.Add(this._grid);
            //
            this._grid.AllowUserToAddRows = false;
            this._grid.AllowUserToDeleteRows = false;
            this._grid.AllowUserToResizeRows = false;
            this._grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this._grid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this._grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._grid.MultiSelect = false;
            this._grid.Name = "_grid";
            this._grid.ReadOnly = true;
            this._grid.RowHeadersVisible = false;
            this._grid.RowTemplate.Height = 36;
            this._grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            //
            this._status.BackColor = System.Drawing.Color.Transparent;
            this._status.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._status.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this._status.Height = 22;
            this._status.Name = "_status";
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._gridHost);
            this.Controls.Add(this._toolbar);
            this.Controls.Add(this._heading);
            this.Controls.Add(this._status);
            this.Name = "TrashView";
            this.Size = new System.Drawing.Size(820, 520);
            this._toolbar.ResumeLayout(false);
            this._gridHost.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._grid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label _heading;
        private System.Windows.Forms.FlowLayoutPanel _toolbar;
        private Guna.UI2.WinForms.Guna2Button _restore;
        private Guna.UI2.WinForms.Guna2Button _purge;
        private Guna.UI2.WinForms.Guna2Button _refresh;
        private System.Windows.Forms.Panel _gridHost;
        private Guna.UI2.WinForms.Guna2DataGridView _grid;
        private System.Windows.Forms.Label _status;
    }
}
