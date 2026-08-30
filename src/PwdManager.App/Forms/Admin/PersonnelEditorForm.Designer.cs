namespace PwdManager.App.Forms.Admin
{
    partial class PersonnelEditorForm
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
            this._username = new Guna.UI2.WinForms.Guna2TextBox();
            this._fullName = new Guna.UI2.WinForms.Guna2TextBox();
            this._password = new Guna.UI2.WinForms.Guna2TextBox();
            this._reveal = new Guna.UI2.WinForms.Guna2Button();
            this._gen = new Guna.UI2.WinForms.Guna2Button();
            this._note = new System.Windows.Forms.Label();
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
            this._heading.Size = new System.Drawing.Size(140, 28);
            this._heading.TabIndex = 0;
            this._heading.Text = "Yeni personel";
            //
            // _username
            //
            this._username.BorderRadius = 8;
            this._username.Location = new System.Drawing.Point(32, 78);
            this._username.Name = "_username";
            this._username.PlaceholderText = "Kullanıcı adı";
            this._username.Size = new System.Drawing.Size(376, 42);
            this._username.TabIndex = 1;
            //
            // _fullName
            //
            this._fullName.BorderRadius = 8;
            this._fullName.Location = new System.Drawing.Point(32, 130);
            this._fullName.Name = "_fullName";
            this._fullName.PlaceholderText = "Ad soyad";
            this._fullName.Size = new System.Drawing.Size(376, 42);
            this._fullName.TabIndex = 2;
            //
            // _password
            //
            this._password.BorderRadius = 8;
            this._password.Location = new System.Drawing.Point(32, 182);
            this._password.Name = "_password";
            this._password.PasswordChar = '●';
            this._password.PlaceholderText = "Geçici parola";
            this._password.Size = new System.Drawing.Size(232, 42);
            this._password.TabIndex = 3;
            //
            // _reveal
            //
            this._reveal.BorderRadius = 8;
            this._reveal.Location = new System.Drawing.Point(272, 182);
            this._reveal.Name = "_reveal";
            this._reveal.Size = new System.Drawing.Size(70, 42);
            this._reveal.TabIndex = 4;
            this._reveal.Tag = "secondary";
            this._reveal.Text = "Göster";
            //
            // _gen
            //
            this._gen.BorderRadius = 8;
            this._gen.Location = new System.Drawing.Point(348, 182);
            this._gen.Name = "_gen";
            this._gen.Size = new System.Drawing.Size(60, 42);
            this._gen.TabIndex = 5;
            this._gen.Tag = "secondary";
            this._gen.Text = "Üret";
            //
            // _note
            //
            this._note.BackColor = System.Drawing.Color.Transparent;
            this._note.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this._note.Location = new System.Drawing.Point(32, 232);
            this._note.MaximumSize = new System.Drawing.Size(376, 0);
            this._note.Name = "_note";
            this._note.Size = new System.Drawing.Size(376, 19);
            this._note.TabIndex = 6;
            this._note.Text = "Personel ilk girişte bu parolayı değiştirmek zorunda kalır.";
            //
            // _status
            //
            this._status.BackColor = System.Drawing.Color.Transparent;
            this._status.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this._status.Location = new System.Drawing.Point(32, 270);
            this._status.MaximumSize = new System.Drawing.Size(376, 0);
            this._status.Name = "_status";
            this._status.Size = new System.Drawing.Size(0, 0);
            this._status.TabIndex = 7;
            //
            // _save
            //
            this._save.BorderRadius = 8;
            this._save.Location = new System.Drawing.Point(32, 306);
            this._save.Name = "_save";
            this._save.Size = new System.Drawing.Size(190, 42);
            this._save.TabIndex = 8;
            this._save.Text = "Kaydet";
            //
            // _cancel
            //
            this._cancel.BorderRadius = 8;
            this._cancel.Location = new System.Drawing.Point(238, 306);
            this._cancel.Name = "_cancel";
            this._cancel.Size = new System.Drawing.Size(120, 42);
            this._cancel.TabIndex = 9;
            this._cancel.Tag = "secondary";
            this._cancel.Text = "İptal";
            //
            // PersonnelEditorForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(440, 380);
            this.Controls.Add(this._heading);
            this.Controls.Add(this._username);
            this.Controls.Add(this._fullName);
            this.Controls.Add(this._password);
            this.Controls.Add(this._reveal);
            this.Controls.Add(this._gen);
            this.Controls.Add(this._note);
            this.Controls.Add(this._status);
            this.Controls.Add(this._save);
            this.Controls.Add(this._cancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PersonnelEditorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Yeni personel";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label _heading;
        private Guna.UI2.WinForms.Guna2TextBox _username;
        private Guna.UI2.WinForms.Guna2TextBox _fullName;
        private Guna.UI2.WinForms.Guna2TextBox _password;
        private Guna.UI2.WinForms.Guna2Button _reveal;
        private Guna.UI2.WinForms.Guna2Button _gen;
        private System.Windows.Forms.Label _note;
        private System.Windows.Forms.Label _status;
        private Guna.UI2.WinForms.Guna2Button _save;
        private Guna.UI2.WinForms.Guna2Button _cancel;
    }
}
