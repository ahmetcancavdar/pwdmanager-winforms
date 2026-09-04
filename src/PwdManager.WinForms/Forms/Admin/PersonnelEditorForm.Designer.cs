namespace PwdManager.WinForms.Forms.Admin
{
    partial class PersonnelEditorForm
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
            this._userLabel = new System.Windows.Forms.Label();
            this._username = new Guna.UI2.WinForms.Guna2TextBox();
            this._nameLabel = new System.Windows.Forms.Label();
            this._fullName = new Guna.UI2.WinForms.Guna2TextBox();
            this._passLabel = new System.Windows.Forms.Label();
            this._password = new Guna.UI2.WinForms.Guna2TextBox();
            this._reveal = new Guna.UI2.WinForms.Guna2Button();
            this._gen = new Guna.UI2.WinForms.Guna2Button();
            this._note = new System.Windows.Forms.Label();
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
            this._card.Size = new System.Drawing.Size(412, 436);
            this._card.Tag = "card";
            this._card.Controls.Add(this._heading);
            this._card.Controls.Add(this._userLabel);
            this._card.Controls.Add(this._username);
            this._card.Controls.Add(this._nameLabel);
            this._card.Controls.Add(this._fullName);
            this._card.Controls.Add(this._passLabel);
            this._card.Controls.Add(this._password);
            this._card.Controls.Add(this._reveal);
            this._card.Controls.Add(this._gen);
            this._card.Controls.Add(this._note);
            this._card.Controls.Add(this._status);
            this._card.Controls.Add(this._save);
            this._card.Controls.Add(this._cancel);
            //
            // _heading
            //
            this._heading.AutoSize = true;
            this._heading.BackColor = System.Drawing.Color.Transparent;
            this._heading.Font = new System.Drawing.Font("Segoe UI", 15.5F, System.Drawing.FontStyle.Bold);
            this._heading.Location = new System.Drawing.Point(28, 24);
            this._heading.Name = "_heading";
            this._heading.Size = new System.Drawing.Size(140, 28);
            this._heading.TabIndex = 0;
            this._heading.Text = "Yeni personel";
            //
            // _userLabel
            //
            this._userLabel.AutoSize = true;
            this._userLabel.BackColor = System.Drawing.Color.Transparent;
            this._userLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this._userLabel.Location = new System.Drawing.Point(29, 64);
            this._userLabel.Name = "_userLabel";
            this._userLabel.Tag = "overline";
            this._userLabel.Text = "KULLANICI ADI";
            //
            // _username
            //
            this._username.BorderRadius = 10;
            this._username.Location = new System.Drawing.Point(28, 86);
            this._username.Name = "_username";
            this._username.PlaceholderText = "Kullanıcı adı";
            this._username.Size = new System.Drawing.Size(356, 44);
            this._username.TabIndex = 1;
            //
            // _nameLabel
            //
            this._nameLabel.AutoSize = true;
            this._nameLabel.BackColor = System.Drawing.Color.Transparent;
            this._nameLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this._nameLabel.Location = new System.Drawing.Point(29, 140);
            this._nameLabel.Name = "_nameLabel";
            this._nameLabel.Tag = "overline";
            this._nameLabel.Text = "AD SOYAD";
            //
            // _fullName
            //
            this._fullName.BorderRadius = 10;
            this._fullName.Location = new System.Drawing.Point(28, 162);
            this._fullName.Name = "_fullName";
            this._fullName.PlaceholderText = "Ad soyad";
            this._fullName.Size = new System.Drawing.Size(356, 44);
            this._fullName.TabIndex = 2;
            //
            // _passLabel
            //
            this._passLabel.AutoSize = true;
            this._passLabel.BackColor = System.Drawing.Color.Transparent;
            this._passLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this._passLabel.Location = new System.Drawing.Point(29, 216);
            this._passLabel.Name = "_passLabel";
            this._passLabel.Tag = "overline";
            this._passLabel.Text = "GEÇİCİ PAROLA";
            //
            // _password
            //
            this._password.BorderRadius = 10;
            this._password.Location = new System.Drawing.Point(28, 238);
            this._password.Name = "_password";
            this._password.PasswordChar = '●';
            this._password.PlaceholderText = "Geçici parola";
            this._password.Size = new System.Drawing.Size(184, 44);
            this._password.TabIndex = 3;
            //
            // _reveal
            //
            this._reveal.BorderRadius = 10;
            this._reveal.Location = new System.Drawing.Point(222, 238);
            this._reveal.Name = "_reveal";
            this._reveal.Size = new System.Drawing.Size(90, 44);
            this._reveal.TabIndex = 4;
            this._reveal.Tag = "secondary";
            this._reveal.Text = "Göster";
            //
            // _gen
            //
            this._gen.BorderRadius = 10;
            this._gen.Location = new System.Drawing.Point(322, 238);
            this._gen.Name = "_gen";
            this._gen.Size = new System.Drawing.Size(62, 44);
            this._gen.TabIndex = 5;
            this._gen.Tag = "secondary";
            this._gen.Text = "Üret";
            //
            // _note
            //
            this._note.BackColor = System.Drawing.Color.Transparent;
            this._note.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this._note.Location = new System.Drawing.Point(29, 292);
            this._note.MaximumSize = new System.Drawing.Size(356, 0);
            this._note.Name = "_note";
            this._note.Size = new System.Drawing.Size(356, 32);
            this._note.TabIndex = 6;
            this._note.Tag = "muted";
            this._note.Text = "Personel ilk girişte bu parolayı değiştirmek zorunda kalır.";
            //
            // _status
            //
            this._status.BackColor = System.Drawing.Color.Transparent;
            this._status.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this._status.Location = new System.Drawing.Point(29, 336);
            this._status.MaximumSize = new System.Drawing.Size(356, 0);
            this._status.Name = "_status";
            this._status.Size = new System.Drawing.Size(0, 0);
            this._status.TabIndex = 7;
            //
            // _save
            //
            this._save.BorderRadius = 10;
            this._save.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this._save.Location = new System.Drawing.Point(28, 376);
            this._save.Name = "_save";
            this._save.Size = new System.Drawing.Size(236, 44);
            this._save.TabIndex = 8;
            this._save.Text = "Kaydet";
            //
            // _cancel
            //
            this._cancel.BorderRadius = 10;
            this._cancel.Location = new System.Drawing.Point(276, 376);
            this._cancel.Name = "_cancel";
            this._cancel.Size = new System.Drawing.Size(108, 44);
            this._cancel.TabIndex = 9;
            this._cancel.Tag = "secondary";
            this._cancel.Text = "İptal";
            //
            // PersonnelEditorForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(468, 492);
            this.Controls.Add(this._card);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PersonnelEditorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Yeni personel";
            this._card.ResumeLayout(false);
            this._card.PerformLayout();
            this.ResumeLayout(false);
        }

        private Guna.UI2.WinForms.Guna2Panel _card;
        private System.Windows.Forms.Label _heading;
        private System.Windows.Forms.Label _userLabel;
        private Guna.UI2.WinForms.Guna2TextBox _username;
        private System.Windows.Forms.Label _nameLabel;
        private Guna.UI2.WinForms.Guna2TextBox _fullName;
        private System.Windows.Forms.Label _passLabel;
        private Guna.UI2.WinForms.Guna2TextBox _password;
        private Guna.UI2.WinForms.Guna2Button _reveal;
        private Guna.UI2.WinForms.Guna2Button _gen;
        private System.Windows.Forms.Label _note;
        private System.Windows.Forms.Label _status;
        private Guna.UI2.WinForms.Guna2Button _save;
        private Guna.UI2.WinForms.Guna2Button _cancel;
    }
}
