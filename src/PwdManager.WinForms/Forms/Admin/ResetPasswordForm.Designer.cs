namespace PwdManager.WinForms.Forms.Admin
{
    partial class ResetPasswordForm
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
            this._who = new System.Windows.Forms.Label();
            this._passLabel = new System.Windows.Forms.Label();
            this._password = new Guna.UI2.WinForms.Guna2TextBox();
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
            this._card.Size = new System.Drawing.Size(420, 340);
            this._card.Tag = "card";
            this._card.Controls.Add(this._heading);
            this._card.Controls.Add(this._who);
            this._card.Controls.Add(this._passLabel);
            this._card.Controls.Add(this._password);
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
            this._heading.Size = new System.Drawing.Size(150, 28);
            this._heading.TabIndex = 0;
            this._heading.Text = "Parola sıfırla";
            //
            // _who
            //
            this._who.AutoSize = true;
            this._who.BackColor = System.Drawing.Color.Transparent;
            this._who.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this._who.Location = new System.Drawing.Point(29, 62);
            this._who.Name = "_who";
            this._who.Size = new System.Drawing.Size(70, 19);
            this._who.TabIndex = 1;
            this._who.Text = "Kullanıcı:";
            //
            // _passLabel
            //
            this._passLabel.AutoSize = true;
            this._passLabel.BackColor = System.Drawing.Color.Transparent;
            this._passLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this._passLabel.Location = new System.Drawing.Point(29, 96);
            this._passLabel.Name = "_passLabel";
            this._passLabel.Tag = "overline";
            this._passLabel.Text = "YENİ GEÇİCİ PAROLA";
            //
            // _password
            //
            this._password.BorderRadius = 10;
            this._password.Location = new System.Drawing.Point(28, 118);
            this._password.Name = "_password";
            this._password.PlaceholderText = "Yeni geçici parola";
            this._password.Size = new System.Drawing.Size(284, 44);
            this._password.TabIndex = 2;
            //
            // _gen
            //
            this._gen.BorderRadius = 10;
            this._gen.Location = new System.Drawing.Point(320, 118);
            this._gen.Name = "_gen";
            this._gen.Size = new System.Drawing.Size(72, 44);
            this._gen.TabIndex = 3;
            this._gen.Tag = "secondary";
            this._gen.Text = "Üret";
            //
            // _note
            //
            this._note.BackColor = System.Drawing.Color.Transparent;
            this._note.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this._note.Location = new System.Drawing.Point(29, 174);
            this._note.MaximumSize = new System.Drawing.Size(364, 0);
            this._note.Name = "_note";
            this._note.Size = new System.Drawing.Size(364, 34);
            this._note.TabIndex = 4;
            this._note.Tag = "muted";
            this._note.Text = "Personel ilk girişte bu parolayı değiştirecek. Parolayı kendisine güvenli bir kanaldan iletin.";
            //
            // _status
            //
            this._status.BackColor = System.Drawing.Color.Transparent;
            this._status.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this._status.Location = new System.Drawing.Point(29, 224);
            this._status.MaximumSize = new System.Drawing.Size(364, 0);
            this._status.Name = "_status";
            this._status.Size = new System.Drawing.Size(0, 0);
            this._status.TabIndex = 5;
            //
            // _save
            //
            this._save.BorderRadius = 10;
            this._save.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this._save.Location = new System.Drawing.Point(28, 260);
            this._save.Name = "_save";
            this._save.Size = new System.Drawing.Size(244, 44);
            this._save.TabIndex = 6;
            this._save.Text = "Sıfırla";
            //
            // _cancel
            //
            this._cancel.BorderRadius = 10;
            this._cancel.Location = new System.Drawing.Point(284, 260);
            this._cancel.Name = "_cancel";
            this._cancel.Size = new System.Drawing.Size(108, 44);
            this._cancel.TabIndex = 7;
            this._cancel.Tag = "secondary";
            this._cancel.Text = "İptal";
            //
            // ResetPasswordForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(476, 396);
            this.Controls.Add(this._card);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ResetPasswordForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Parola sıfırla";
            this._card.ResumeLayout(false);
            this._card.PerformLayout();
            this.ResumeLayout(false);
        }

        private Guna.UI2.WinForms.Guna2Panel _card;
        private System.Windows.Forms.Label _heading;
        private System.Windows.Forms.Label _who;
        private System.Windows.Forms.Label _passLabel;
        private Guna.UI2.WinForms.Guna2TextBox _password;
        private Guna.UI2.WinForms.Guna2Button _gen;
        private System.Windows.Forms.Label _note;
        private System.Windows.Forms.Label _status;
        private Guna.UI2.WinForms.Guna2Button _save;
        private Guna.UI2.WinForms.Guna2Button _cancel;
    }
}
