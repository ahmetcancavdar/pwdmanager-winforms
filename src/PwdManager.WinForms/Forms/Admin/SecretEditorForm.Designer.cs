namespace PwdManager.WinForms.Forms.Admin
{
    partial class SecretEditorForm
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
            this._catLabel = new System.Windows.Forms.Label();
            this._category = new Guna.UI2.WinForms.Guna2ComboBox();
            this._titleLabel = new System.Windows.Forms.Label();
            this._title = new Guna.UI2.WinForms.Guna2TextBox();
            this._userLabel = new System.Windows.Forms.Label();
            this._username = new Guna.UI2.WinForms.Guna2TextBox();
            this._passLabel = new System.Windows.Forms.Label();
            this._password = new Guna.UI2.WinForms.Guna2TextBox();
            this._toggle = new Guna.UI2.WinForms.Guna2Button();
            this._gen = new Guna.UI2.WinForms.Guna2Button();
            this._notesLabel = new System.Windows.Forms.Label();
            this._notes = new Guna.UI2.WinForms.Guna2TextBox();
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
            this._card.Size = new System.Drawing.Size(452, 552);
            this._card.Tag = "card";
            this._card.Controls.Add(this._heading);
            this._card.Controls.Add(this._catLabel);
            this._card.Controls.Add(this._category);
            this._card.Controls.Add(this._titleLabel);
            this._card.Controls.Add(this._title);
            this._card.Controls.Add(this._userLabel);
            this._card.Controls.Add(this._username);
            this._card.Controls.Add(this._passLabel);
            this._card.Controls.Add(this._password);
            this._card.Controls.Add(this._toggle);
            this._card.Controls.Add(this._gen);
            this._card.Controls.Add(this._notesLabel);
            this._card.Controls.Add(this._notes);
            this._card.Controls.Add(this._status);
            this._card.Controls.Add(this._save);
            this._card.Controls.Add(this._cancel);
            //
            // _heading
            //
            this._heading.AutoSize = true;
            this._heading.BackColor = System.Drawing.Color.Transparent;
            this._heading.Font = new System.Drawing.Font("Segoe UI", 15.5F, System.Drawing.FontStyle.Bold);
            this._heading.Location = new System.Drawing.Point(28, 22);
            this._heading.Name = "_heading";
            this._heading.Size = new System.Drawing.Size(80, 28);
            this._heading.TabIndex = 0;
            this._heading.Text = "Parola";
            //
            // _catLabel
            //
            this._catLabel.AutoSize = true;
            this._catLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this._catLabel.Location = new System.Drawing.Point(29, 60);
            this._catLabel.Name = "_catLabel";
            this._catLabel.Tag = "overline";
            this._catLabel.Text = "KATEGORİ";
            //
            // _category
            //
            this._category.BorderRadius = 10;
            this._category.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this._category.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this._category.ItemHeight = 30;
            this._category.Location = new System.Drawing.Point(28, 82);
            this._category.Name = "_category";
            this._category.Size = new System.Drawing.Size(396, 42);
            this._category.TabIndex = 1;
            //
            // _titleLabel
            //
            this._titleLabel.AutoSize = true;
            this._titleLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this._titleLabel.Location = new System.Drawing.Point(29, 134);
            this._titleLabel.Name = "_titleLabel";
            this._titleLabel.Tag = "overline";
            this._titleLabel.Text = "BAŞLIK";
            //
            // _title
            //
            this._title.BorderRadius = 10;
            this._title.Location = new System.Drawing.Point(28, 156);
            this._title.Name = "_title";
            this._title.PlaceholderText = "ör. Sunucu kök hesabı";
            this._title.Size = new System.Drawing.Size(396, 44);
            this._title.TabIndex = 2;
            //
            // _userLabel
            //
            this._userLabel.AutoSize = true;
            this._userLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this._userLabel.Location = new System.Drawing.Point(29, 210);
            this._userLabel.Name = "_userLabel";
            this._userLabel.Tag = "overline";
            this._userLabel.Text = "KULLANICI ADI (İSTEĞE BAĞLI)";
            //
            // _username
            //
            this._username.BorderRadius = 10;
            this._username.Location = new System.Drawing.Point(28, 232);
            this._username.Name = "_username";
            this._username.PlaceholderText = "Kullanıcı adı";
            this._username.Size = new System.Drawing.Size(396, 44);
            this._username.TabIndex = 3;
            //
            // _passLabel
            //
            this._passLabel.AutoSize = true;
            this._passLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this._passLabel.Location = new System.Drawing.Point(29, 286);
            this._passLabel.Name = "_passLabel";
            this._passLabel.Tag = "overline";
            this._passLabel.Text = "PAROLA";
            //
            // _password
            //
            this._password.BorderRadius = 10;
            this._password.Location = new System.Drawing.Point(28, 308);
            this._password.Name = "_password";
            this._password.PasswordChar = '●';
            this._password.PlaceholderText = "Parola";
            this._password.Size = new System.Drawing.Size(222, 44);
            this._password.TabIndex = 4;
            //
            // _toggle
            //
            this._toggle.BorderRadius = 10;
            this._toggle.Location = new System.Drawing.Point(258, 308);
            this._toggle.Name = "_toggle";
            this._toggle.Size = new System.Drawing.Size(90, 44);
            this._toggle.TabIndex = 5;
            this._toggle.Tag = "secondary";
            this._toggle.Text = "Göster";
            //
            // _gen
            //
            this._gen.BorderRadius = 10;
            this._gen.Location = new System.Drawing.Point(356, 308);
            this._gen.Name = "_gen";
            this._gen.Size = new System.Drawing.Size(68, 44);
            this._gen.TabIndex = 6;
            this._gen.Tag = "secondary";
            this._gen.Text = "Üret";
            //
            // _notesLabel
            //
            this._notesLabel.AutoSize = true;
            this._notesLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this._notesLabel.Location = new System.Drawing.Point(29, 362);
            this._notesLabel.Name = "_notesLabel";
            this._notesLabel.Tag = "overline";
            this._notesLabel.Text = "NOT (İSTEĞE BAĞLI)";
            //
            // _notes
            //
            this._notes.BorderRadius = 10;
            this._notes.Location = new System.Drawing.Point(28, 384);
            this._notes.Name = "_notes";
            this._notes.PlaceholderText = "Not";
            this._notes.Size = new System.Drawing.Size(396, 44);
            this._notes.TabIndex = 7;
            //
            // _status
            //
            this._status.BackColor = System.Drawing.Color.Transparent;
            this._status.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this._status.Location = new System.Drawing.Point(29, 438);
            this._status.MaximumSize = new System.Drawing.Size(396, 0);
            this._status.Name = "_status";
            this._status.Size = new System.Drawing.Size(0, 0);
            this._status.TabIndex = 8;
            //
            // _save
            //
            this._save.BorderRadius = 10;
            this._save.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this._save.Location = new System.Drawing.Point(28, 476);
            this._save.Name = "_save";
            this._save.Size = new System.Drawing.Size(280, 46);
            this._save.TabIndex = 9;
            this._save.Text = "Kaydet";
            //
            // _cancel
            //
            this._cancel.BorderRadius = 10;
            this._cancel.Location = new System.Drawing.Point(320, 476);
            this._cancel.Name = "_cancel";
            this._cancel.Size = new System.Drawing.Size(104, 46);
            this._cancel.TabIndex = 10;
            this._cancel.Tag = "secondary";
            this._cancel.Text = "İptal";
            //
            // SecretEditorForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(508, 608);
            this.Controls.Add(this._card);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SecretEditorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Parola";
            this._card.ResumeLayout(false);
            this._card.PerformLayout();
            this.ResumeLayout(false);
        }

        private Guna.UI2.WinForms.Guna2Panel _card;
        private System.Windows.Forms.Label _heading;
        private System.Windows.Forms.Label _catLabel;
        private Guna.UI2.WinForms.Guna2ComboBox _category;
        private System.Windows.Forms.Label _titleLabel;
        private Guna.UI2.WinForms.Guna2TextBox _title;
        private System.Windows.Forms.Label _userLabel;
        private Guna.UI2.WinForms.Guna2TextBox _username;
        private System.Windows.Forms.Label _passLabel;
        private Guna.UI2.WinForms.Guna2TextBox _password;
        private Guna.UI2.WinForms.Guna2Button _toggle;
        private Guna.UI2.WinForms.Guna2Button _gen;
        private System.Windows.Forms.Label _notesLabel;
        private Guna.UI2.WinForms.Guna2TextBox _notes;
        private System.Windows.Forms.Label _status;
        private Guna.UI2.WinForms.Guna2Button _save;
        private Guna.UI2.WinForms.Guna2Button _cancel;
    }
}
