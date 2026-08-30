namespace PwdManager.App.Forms.Admin
{
    partial class SecretEditorForm
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
            this._category = new Guna.UI2.WinForms.Guna2ComboBox();
            this._title = new Guna.UI2.WinForms.Guna2TextBox();
            this._username = new Guna.UI2.WinForms.Guna2TextBox();
            this._password = new Guna.UI2.WinForms.Guna2TextBox();
            this._toggle = new Guna.UI2.WinForms.Guna2Button();
            this._gen = new Guna.UI2.WinForms.Guna2Button();
            this._notes = new Guna.UI2.WinForms.Guna2TextBox();
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
            this._heading.Location = new System.Drawing.Point(32, 24);
            this._heading.Name = "_heading";
            this._heading.Size = new System.Drawing.Size(120, 28);
            this._heading.TabIndex = 0;
            this._heading.Text = "Parola";
            //
            // _category
            //
            this._category.BorderRadius = 8;
            this._category.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this._category.ItemHeight = 30;
            this._category.Location = new System.Drawing.Point(32, 76);
            this._category.Name = "_category";
            this._category.Size = new System.Drawing.Size(396, 40);
            this._category.TabIndex = 1;
            //
            // _title
            //
            this._title.BorderRadius = 8;
            this._title.Location = new System.Drawing.Point(32, 128);
            this._title.Name = "_title";
            this._title.PlaceholderText = "Başlık (ör. \'Sunucu kök hesabı\')";
            this._title.Size = new System.Drawing.Size(396, 42);
            this._title.TabIndex = 2;
            //
            // _username
            //
            this._username.BorderRadius = 8;
            this._username.Location = new System.Drawing.Point(32, 180);
            this._username.Name = "_username";
            this._username.PlaceholderText = "Kullanıcı adı";
            this._username.Size = new System.Drawing.Size(396, 42);
            this._username.TabIndex = 3;
            //
            // _password
            //
            this._password.BorderRadius = 8;
            this._password.Location = new System.Drawing.Point(32, 232);
            this._password.Name = "_password";
            this._password.PasswordChar = '●';
            this._password.PlaceholderText = "Parola";
            this._password.Size = new System.Drawing.Size(300, 42);
            this._password.TabIndex = 4;
            //
            // _toggle
            //
            this._toggle.BorderRadius = 8;
            this._toggle.Location = new System.Drawing.Point(348, 236);
            this._toggle.Name = "_toggle";
            this._toggle.Size = new System.Drawing.Size(80, 34);
            this._toggle.TabIndex = 5;
            this._toggle.Tag = "secondary";
            this._toggle.Text = "Göster";
            //
            // _gen
            //
            this._gen.BorderRadius = 8;
            this._gen.Location = new System.Drawing.Point(348, 276);
            this._gen.Name = "_gen";
            this._gen.Size = new System.Drawing.Size(80, 34);
            this._gen.TabIndex = 6;
            this._gen.Tag = "secondary";
            this._gen.Text = "Üret";
            //
            // _notes
            //
            this._notes.BorderRadius = 8;
            this._notes.Location = new System.Drawing.Point(32, 316);
            this._notes.Name = "_notes";
            this._notes.PlaceholderText = "Not (isteğe bağlı)";
            this._notes.Size = new System.Drawing.Size(396, 42);
            this._notes.TabIndex = 7;
            //
            // _status
            //
            this._status.BackColor = System.Drawing.Color.Transparent;
            this._status.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this._status.Location = new System.Drawing.Point(32, 366);
            this._status.MaximumSize = new System.Drawing.Size(396, 0);
            this._status.Name = "_status";
            this._status.Size = new System.Drawing.Size(0, 0);
            this._status.TabIndex = 8;
            //
            // _save
            //
            this._save.BorderRadius = 8;
            this._save.Location = new System.Drawing.Point(32, 400);
            this._save.Name = "_save";
            this._save.Size = new System.Drawing.Size(200, 42);
            this._save.TabIndex = 9;
            this._save.Text = "Kaydet";
            //
            // _cancel
            //
            this._cancel.BorderRadius = 8;
            this._cancel.Location = new System.Drawing.Point(244, 400);
            this._cancel.Name = "_cancel";
            this._cancel.Size = new System.Drawing.Size(120, 42);
            this._cancel.TabIndex = 10;
            this._cancel.Tag = "secondary";
            this._cancel.Text = "İptal";
            //
            // SecretEditorForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(460, 470);
            this.Controls.Add(this._heading);
            this.Controls.Add(this._category);
            this.Controls.Add(this._title);
            this.Controls.Add(this._username);
            this.Controls.Add(this._password);
            this.Controls.Add(this._toggle);
            this.Controls.Add(this._gen);
            this.Controls.Add(this._notes);
            this.Controls.Add(this._status);
            this.Controls.Add(this._save);
            this.Controls.Add(this._cancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SecretEditorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Parola";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label _heading;
        private Guna.UI2.WinForms.Guna2ComboBox _category;
        private Guna.UI2.WinForms.Guna2TextBox _title;
        private Guna.UI2.WinForms.Guna2TextBox _username;
        private Guna.UI2.WinForms.Guna2TextBox _password;
        private Guna.UI2.WinForms.Guna2Button _toggle;
        private Guna.UI2.WinForms.Guna2Button _gen;
        private Guna.UI2.WinForms.Guna2TextBox _notes;
        private System.Windows.Forms.Label _status;
        private Guna.UI2.WinForms.Guna2Button _save;
        private Guna.UI2.WinForms.Guna2Button _cancel;
    }
}
