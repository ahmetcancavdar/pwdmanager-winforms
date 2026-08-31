namespace PwdManager.WinForms.Forms.Admin
{
    partial class CategoryEditorForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this._heading = new System.Windows.Forms.Label();
            this._name = new Guna.UI2.WinForms.Guna2TextBox();
            this._description = new Guna.UI2.WinForms.Guna2TextBox();
            this._status = new System.Windows.Forms.Label();
            this._save = new Guna.UI2.WinForms.Guna2Button();
            this._cancel = new Guna.UI2.WinForms.Guna2Button();
            this.SuspendLayout();
            //
            // _heading
            //
            this._heading.AutoSize = true;
            this._heading.BackColor = System.Drawing.Color.Transparent;
            this._heading.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this._heading.Location = new System.Drawing.Point(32, 28);
            this._heading.Name = "_heading";
            this._heading.Size = new System.Drawing.Size(140, 28);
            this._heading.TabIndex = 0;
            this._heading.Text = "Kategori";
            //
            // _name
            //
            this._name.BorderRadius = 8;
            this._name.Location = new System.Drawing.Point(32, 84);
            this._name.Name = "_name";
            this._name.PlaceholderText = "Kategori adı";
            this._name.Size = new System.Drawing.Size(356, 42);
            this._name.TabIndex = 1;
            //
            // _description
            //
            this._description.BorderRadius = 8;
            this._description.Location = new System.Drawing.Point(32, 136);
            this._description.Name = "_description";
            this._description.PlaceholderText = "Açıklama (isteğe bağlı)";
            this._description.Size = new System.Drawing.Size(356, 42);
            this._description.TabIndex = 2;
            //
            // _status
            //
            this._status.BackColor = System.Drawing.Color.Transparent;
            this._status.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this._status.Location = new System.Drawing.Point(32, 188);
            this._status.MaximumSize = new System.Drawing.Size(356, 0);
            this._status.Name = "_status";
            this._status.Size = new System.Drawing.Size(0, 0);
            this._status.TabIndex = 3;
            //
            // _save
            //
            this._save.BorderRadius = 8;
            this._save.Location = new System.Drawing.Point(32, 226);
            this._save.Name = "_save";
            this._save.Size = new System.Drawing.Size(180, 42);
            this._save.TabIndex = 4;
            this._save.Text = "Kaydet";
            //
            // _cancel
            //
            this._cancel.BorderRadius = 8;
            this._cancel.Location = new System.Drawing.Point(224, 226);
            this._cancel.Name = "_cancel";
            this._cancel.Size = new System.Drawing.Size(120, 42);
            this._cancel.TabIndex = 5;
            this._cancel.Tag = "secondary";
            this._cancel.Text = "İptal";
            //
            // CategoryEditorForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(420, 300);
            this.Controls.Add(this._heading);
            this.Controls.Add(this._name);
            this.Controls.Add(this._description);
            this.Controls.Add(this._status);
            this.Controls.Add(this._save);
            this.Controls.Add(this._cancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CategoryEditorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Kategori";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label _heading;
        private Guna.UI2.WinForms.Guna2TextBox _name;
        private Guna.UI2.WinForms.Guna2TextBox _description;
        private System.Windows.Forms.Label _status;
        private Guna.UI2.WinForms.Guna2Button _save;
        private Guna.UI2.WinForms.Guna2Button _cancel;
    }
}
