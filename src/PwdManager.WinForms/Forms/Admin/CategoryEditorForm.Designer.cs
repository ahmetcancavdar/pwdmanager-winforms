namespace PwdManager.WinForms.Forms.Admin
{
    partial class CategoryEditorForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this._card = new Guna.UI2.WinForms.Guna2Panel();
            this._heading = new System.Windows.Forms.Label();
            this._nameLabel = new System.Windows.Forms.Label();
            this._name = new Guna.UI2.WinForms.Guna2TextBox();
            this._descLabel = new System.Windows.Forms.Label();
            this._description = new Guna.UI2.WinForms.Guna2TextBox();
            this._status = new System.Windows.Forms.Label();
            this._save = new Guna.UI2.WinForms.Guna2Button();
            this._cancel = new Guna.UI2.WinForms.Guna2Button();
            this._card.SuspendLayout();
            this.SuspendLayout();
            //
            // _card
            //
            this._card.Location = new System.Drawing.Point(28, 28);
            this._card.Name = "_card";
            this._card.Size = new System.Drawing.Size(408, 300);
            this._card.Tag = "card";
            this._card.Controls.Add(this._heading);
            this._card.Controls.Add(this._nameLabel);
            this._card.Controls.Add(this._name);
            this._card.Controls.Add(this._descLabel);
            this._card.Controls.Add(this._description);
            this._card.Controls.Add(this._status);
            this._card.Controls.Add(this._save);
            this._card.Controls.Add(this._cancel);
            //
            // _heading
            //
            this._heading.AutoSize = true;
            this._heading.BackColor = System.Drawing.Color.Transparent;
            this._heading.Font = new System.Drawing.Font("Segoe UI", 15.5F, System.Drawing.FontStyle.Bold);
            this._heading.Location = new System.Drawing.Point(28, 26);
            this._heading.Name = "_heading";
            this._heading.Size = new System.Drawing.Size(96, 28);
            this._heading.TabIndex = 0;
            this._heading.Text = "Kategori";
            //
            // _nameLabel
            //
            this._nameLabel.AutoSize = true;
            this._nameLabel.BackColor = System.Drawing.Color.Transparent;
            this._nameLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this._nameLabel.Location = new System.Drawing.Point(29, 70);
            this._nameLabel.Name = "_nameLabel";
            this._nameLabel.Tag = "overline";
            this._nameLabel.Text = "KATEGORİ ADI";
            //
            // _name
            //
            this._name.BorderRadius = 10;
            this._name.Location = new System.Drawing.Point(28, 92);
            this._name.Name = "_name";
            this._name.PlaceholderText = "Kategori adı";
            this._name.Size = new System.Drawing.Size(352, 44);
            this._name.TabIndex = 1;
            //
            // _descLabel
            //
            this._descLabel.AutoSize = true;
            this._descLabel.BackColor = System.Drawing.Color.Transparent;
            this._descLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this._descLabel.Location = new System.Drawing.Point(29, 148);
            this._descLabel.Name = "_descLabel";
            this._descLabel.Tag = "overline";
            this._descLabel.Text = "AÇIKLAMA (İSTEĞE BAĞLI)";
            //
            // _description
            //
            this._description.BorderRadius = 10;
            this._description.Location = new System.Drawing.Point(28, 170);
            this._description.Name = "_description";
            this._description.PlaceholderText = "Açıklama";
            this._description.Size = new System.Drawing.Size(352, 44);
            this._description.TabIndex = 2;
            //
            // _status
            //
            this._status.BackColor = System.Drawing.Color.Transparent;
            this._status.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this._status.Location = new System.Drawing.Point(29, 222);
            this._status.MaximumSize = new System.Drawing.Size(352, 0);
            this._status.Name = "_status";
            this._status.Size = new System.Drawing.Size(0, 0);
            this._status.TabIndex = 3;
            //
            // _save
            //
            this._save.BorderRadius = 10;
            this._save.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this._save.Location = new System.Drawing.Point(28, 250);
            this._save.Name = "_save";
            this._save.Size = new System.Drawing.Size(232, 44);
            this._save.TabIndex = 4;
            this._save.Text = "Kaydet";
            //
            // _cancel
            //
            this._cancel.BorderRadius = 10;
            this._cancel.Location = new System.Drawing.Point(272, 250);
            this._cancel.Name = "_cancel";
            this._cancel.Size = new System.Drawing.Size(108, 44);
            this._cancel.TabIndex = 5;
            this._cancel.Tag = "secondary";
            this._cancel.Text = "İptal";
            //
            // CategoryEditorForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 356);
            this.Controls.Add(this._card);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CategoryEditorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Kategori";
            this._card.ResumeLayout(false);
            this._card.PerformLayout();
            this.ResumeLayout(false);
        }

        private Guna.UI2.WinForms.Guna2Panel _card;
        private System.Windows.Forms.Label _heading;
        private System.Windows.Forms.Label _nameLabel;
        private Guna.UI2.WinForms.Guna2TextBox _name;
        private System.Windows.Forms.Label _descLabel;
        private Guna.UI2.WinForms.Guna2TextBox _description;
        private System.Windows.Forms.Label _status;
        private Guna.UI2.WinForms.Guna2Button _save;
        private Guna.UI2.WinForms.Guna2Button _cancel;
    }
}
