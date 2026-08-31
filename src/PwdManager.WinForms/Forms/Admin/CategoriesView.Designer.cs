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
            this._heading = new System.Windows.Forms.Label();
            this._toolbar = new System.Windows.Forms.FlowLayoutPanel();
            this._add = new Guna.UI2.WinForms.Guna2Button();
            this._edit = new Guna.UI2.WinForms.Guna2Button();
            this._del = new Guna.UI2.WinForms.Guna2Button();
            this._gridHost = new System.Windows.Forms.Panel();
            this._grid = new Guna.UI2.WinForms.Guna2DataGridView();
            this._status = new System.Windows.Forms.Label();
            this._toolbar.SuspendLayout();
            this._gridHost.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._grid)).BeginInit();
            this.SuspendLayout();
            //
            // _heading
            //
            this._heading.AutoSize = true;
            this._heading.BackColor = System.Drawing.Color.Transparent;
            this._heading.Dock = System.Windows.Forms.DockStyle.Top;
            this._heading.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this._heading.Padding = new System.Windows.Forms.Padding(2, 2, 0, 6);
            this._heading.Name = "_heading";
            this._heading.Size = new System.Drawing.Size(120, 35);
            this._heading.Text = "Kategoriler";
            //
            // _toolbar
            //
            this._toolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this._toolbar.Height = 52;
            this._toolbar.Padding = new System.Windows.Forms.Padding(0, 6, 0, 6);
            this._toolbar.Name = "_toolbar";
            this._toolbar.Controls.Add(this._add);
            this._toolbar.Controls.Add(this._edit);
            this._toolbar.Controls.Add(this._del);
            //
            // _add / _edit / _del
            //
            this._add.BorderRadius = 8;
            this._add.Size = new System.Drawing.Size(90, 38);
            this._add.Name = "_add";
            this._add.Text = "Yeni";
            this._edit.BorderRadius = 8;
            this._edit.Size = new System.Drawing.Size(100, 38);
            this._edit.Name = "_edit";
            this._edit.Tag = "secondary";
            this._edit.Text = "Düzenle";
            this._del.BorderRadius = 8;
            this._del.Size = new System.Drawing.Size(80, 38);
            this._del.Name = "_del";
            this._del.Tag = "secondary";
            this._del.Text = "Sil";
            //
            // _gridHost
            //
            this._gridHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this._gridHost.Padding = new System.Windows.Forms.Padding(0, 8, 0, 8);
            this._gridHost.Name = "_gridHost";
            this._gridHost.Controls.Add(this._grid);
            //
            // _grid
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
            // _status
            //
            this._status.BackColor = System.Drawing.Color.Transparent;
            this._status.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._status.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this._status.Height = 22;
            this._status.Name = "_status";
            //
            // CategoriesView
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._gridHost);
            this.Controls.Add(this._toolbar);
            this.Controls.Add(this._heading);
            this.Controls.Add(this._status);
            this.Name = "CategoriesView";
            this.Size = new System.Drawing.Size(820, 520);
            this._toolbar.ResumeLayout(false);
            this._gridHost.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._grid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label _heading;
        private System.Windows.Forms.FlowLayoutPanel _toolbar;
        private Guna.UI2.WinForms.Guna2Button _add;
        private Guna.UI2.WinForms.Guna2Button _edit;
        private Guna.UI2.WinForms.Guna2Button _del;
        private System.Windows.Forms.Panel _gridHost;
        private Guna.UI2.WinForms.Guna2DataGridView _grid;
        private System.Windows.Forms.Label _status;
    }
}
