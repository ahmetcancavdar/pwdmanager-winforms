namespace PwdManager.WinForms.Forms.Admin
{
    partial class CategoriesView
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this._pageHeader = new Guna.UI2.WinForms.Guna2Panel();
            this._title = new System.Windows.Forms.Label();
            this._subtitle = new System.Windows.Forms.Label();
            this._headerDivider = new Guna.UI2.WinForms.Guna2Panel();
            this._toolbar = new System.Windows.Forms.FlowLayoutPanel();
            this._add = new Guna.UI2.WinForms.Guna2Button();
            this._edit = new Guna.UI2.WinForms.Guna2Button();
            this._del = new Guna.UI2.WinForms.Guna2Button();
            this._card = new Guna.UI2.WinForms.Guna2Panel();
            this._grid = new Guna.UI2.WinForms.Guna2DataGridView();
            this._status = new System.Windows.Forms.Label();
            this._pageHeader.SuspendLayout();
            this._toolbar.SuspendLayout();
            this._card.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._grid)).BeginInit();
            this.SuspendLayout();
            //
            // _pageHeader
            //
            this._pageHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this._pageHeader.Height = 84;
            this._pageHeader.Name = "_pageHeader";
            this._pageHeader.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._pageHeader.Controls.Add(this._subtitle);
            this._pageHeader.Controls.Add(this._title);
            //
            // _title
            //
            this._title.AutoSize = true;
            this._title.BackColor = System.Drawing.Color.Transparent;
            this._title.Font = new System.Drawing.Font("Segoe UI", 15.5F, System.Drawing.FontStyle.Bold);
            this._title.Location = new System.Drawing.Point(4, 14);
            this._title.Name = "_title";
            this._title.Size = new System.Drawing.Size(120, 28);
            this._title.TabIndex = 0;
            this._title.Text = "Kategoriler";
            //
            // _subtitle
            //
            this._subtitle.AutoSize = true;
            this._subtitle.BackColor = System.Drawing.Color.Transparent;
            this._subtitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._subtitle.Location = new System.Drawing.Point(6, 48);
            this._subtitle.Name = "_subtitle";
            this._subtitle.Size = new System.Drawing.Size(320, 15);
            this._subtitle.TabIndex = 1;
            this._subtitle.Tag = "muted";
            this._subtitle.Text = "Parolaları gruplayan kategoriler. Silinen kategori \"Silinenler\"e taşınır.";
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
            this._toolbar.Controls.Add(this._add);
            this._toolbar.Controls.Add(this._edit);
            this._toolbar.Controls.Add(this._del);
            //
            // _add
            //
            this._add.BorderRadius = 10;
            this._add.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this._add.ForeColor = System.Drawing.Color.White;
            this._add.Margin = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this._add.Name = "_add";
            this._add.Size = new System.Drawing.Size(104, 38);
            this._add.TabIndex = 0;
            this._add.Text = "Yeni";
            //
            // _edit
            //
            this._edit.BorderRadius = 10;
            this._edit.Margin = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this._edit.Name = "_edit";
            this._edit.Size = new System.Drawing.Size(104, 38);
            this._edit.TabIndex = 1;
            this._edit.Tag = "secondary";
            this._edit.Text = "Düzenle";
            //
            // _del
            //
            this._del.BorderRadius = 10;
            this._del.Margin = new System.Windows.Forms.Padding(0);
            this._del.Name = "_del";
            this._del.Size = new System.Drawing.Size(90, 38);
            this._del.TabIndex = 2;
            this._del.Tag = "secondary";
            this._del.Text = "Sil";
            //
            // _card
            //
            this._card.Dock = System.Windows.Forms.DockStyle.Fill;
            this._card.Name = "_card";
            this._card.Padding = new System.Windows.Forms.Padding(2);
            this._card.Tag = "card";
            this._card.Controls.Add(this._grid);
            //
            // _grid
            //
            this._grid.AllowUserToAddRows = false;
            this._grid.AllowUserToDeleteRows = false;
            this._grid.AllowUserToResizeRows = false;
            this._grid.BackgroundColor = System.Drawing.Color.FromArgb(28, 28, 36);
            this._grid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this._grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._grid.MultiSelect = false;
            this._grid.Name = "_grid";
            this._grid.ReadOnly = true;
            this._grid.RowHeadersVisible = false;
            this._grid.RowTemplate.Height = 42;
            this._grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._grid.TabIndex = 0;
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
            // CategoriesView
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._card);
            this.Controls.Add(this._toolbar);
            this.Controls.Add(this._headerDivider);
            this.Controls.Add(this._pageHeader);
            this.Controls.Add(this._status);
            this.Name = "CategoriesView";
            this.Size = new System.Drawing.Size(937, 693);
            this._pageHeader.ResumeLayout(false);
            this._pageHeader.PerformLayout();
            this._toolbar.ResumeLayout(false);
            this._card.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._grid)).EndInit();
            this.ResumeLayout(false);
        }

        private Guna.UI2.WinForms.Guna2Panel _pageHeader;
        private System.Windows.Forms.Label _title;
        private System.Windows.Forms.Label _subtitle;
        private Guna.UI2.WinForms.Guna2Panel _headerDivider;
        private System.Windows.Forms.FlowLayoutPanel _toolbar;
        private Guna.UI2.WinForms.Guna2Button _add;
        private Guna.UI2.WinForms.Guna2Button _edit;
        private Guna.UI2.WinForms.Guna2Button _del;
        private Guna.UI2.WinForms.Guna2Panel _card;
        private Guna.UI2.WinForms.Guna2DataGridView _grid;
        private System.Windows.Forms.Label _status;
    }
}
