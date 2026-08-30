namespace PwdManager.App.Forms.Admin
{
    partial class ResetPasswordForm
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
            this._who = new System.Windows.Forms.Label();
            this._password = new Guna.UI2.WinForms.Guna2TextBox();
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
            this._heading.Size = new System.Drawing.Size(150, 28);
            this._heading.TabIndex = 0;
            this._heading.Text = "Parola sıfırla";
            //
            // _who
            //
            this._who.AutoSize = true;
            this._who.BackColor = System.Drawing.Color.Transparent;
            this._who.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this._who.Location = new System.Drawing.Point(32, 66);
            this._who.Name = "_who";
            this._who.Size = new System.Drawing.Size(70, 19);
            this._who.TabIndex = 1;
            this._who.Text = "Kullanıcı:";
            //
            // _password
            //
            this._password.BorderRadius = 8;
            this._password.Location = new System.Drawing.Point(32, 104);
            this._password.Name = "_password";
            this._password.PlaceholderText = "Yeni geçici parola";
            this._password.Size = new System.Drawing.Size(288, 42);
            this._password.TabIndex = 2;
            //
            // _gen
            //
            this._gen.BorderRadius = 8;
            this._gen.Location = new System.Drawing.Point(328, 104);
            this._gen.Name = "_gen";
            this._gen.Size = new System.Drawing.Size(80, 40);
            this._gen.TabIndex = 3;
            this._gen.Tag = "secondary";
            this._gen.Text = "Üret";
            //
            // _note
            //
            this._note.BackColor = System.Drawing.Color.Transparent;
            this._note.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this._note.Location = new System.Drawing.Point(32, 150);
            this._note.MaximumSize = new System.Drawing.Size(376, 0);
            this._note.Name = "_note";
            this._note.Size = new System.Drawing.Size(376, 38);
            this._note.TabIndex = 4;
            this._note.Text = "Personel ilk girişte bu parolayı değiştirecek. Parolayı kendisine güvenli bir kanaldan iletin.";
            //
            // _status
            //
            this._status.BackColor = System.Drawing.Color.Transparent;
            this._status.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this._status.Location = new System.Drawing.Point(32, 198);
            this._status.MaximumSize = new System.Drawing.Size(376, 0);
            this._status.Name = "_status";
            this._status.Size = new System.Drawing.Size(0, 0);
            this._status.TabIndex = 5;
            //
            // _save
            //
            this._save.BorderRadius = 8;
            this._save.Location = new System.Drawing.Point(32, 224);
            this._save.Name = "_save";
            this._save.Size = new System.Drawing.Size(180, 42);
            this._save.TabIndex = 6;
            this._save.Text = "Sıfırla";
            //
            // _cancel
            //
            this._cancel.BorderRadius = 8;
            this._cancel.Location = new System.Drawing.Point(228, 224);
            this._cancel.Name = "_cancel";
            this._cancel.Size = new System.Drawing.Size(120, 42);
            this._cancel.TabIndex = 7;
            this._cancel.Tag = "secondary";
            this._cancel.Text = "İptal";
            //
            // ResetPasswordForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(440, 280);
            this.Controls.Add(this._heading);
            this.Controls.Add(this._who);
            this.Controls.Add(this._password);
            this.Controls.Add(this._gen);
            this.Controls.Add(this._note);
            this.Controls.Add(this._status);
            this.Controls.Add(this._save);
            this.Controls.Add(this._cancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ResetPasswordForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Parola sıfırla";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label _heading;
        private System.Windows.Forms.Label _who;
        private Guna.UI2.WinForms.Guna2TextBox _password;
        private Guna.UI2.WinForms.Guna2Button _gen;
        private System.Windows.Forms.Label _note;
        private System.Windows.Forms.Label _status;
        private Guna.UI2.WinForms.Guna2Button _save;
        private Guna.UI2.WinForms.Guna2Button _cancel;
    }
}
