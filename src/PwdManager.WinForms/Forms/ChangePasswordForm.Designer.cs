namespace PwdManager.WinForms.Forms
{
    partial class ChangePasswordForm
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
            this._title = new System.Windows.Forms.Label();
            this._forcedNote = new System.Windows.Forms.Label();
            this._current = new Guna.UI2.WinForms.Guna2TextBox();
            this._new = new Guna.UI2.WinForms.Guna2TextBox();
            this._confirm = new Guna.UI2.WinForms.Guna2TextBox();
            this._reveal = new Guna.UI2.WinForms.Guna2Button();
            this._status = new System.Windows.Forms.Label();
            this._save = new Guna.UI2.WinForms.Guna2Button();
            this._cancel = new Guna.UI2.WinForms.Guna2Button();
            this._card.SuspendLayout();
            this.SuspendLayout();
            //
            // _card
            //
            this._card.Location = new System.Drawing.Point(36, 36);
            this._card.Name = "_card";
            this._card.Size = new System.Drawing.Size(408, 440);
            this._card.Tag = "card";
            this._card.Controls.Add(this._title);
            this._card.Controls.Add(this._forcedNote);
            this._card.Controls.Add(this._current);
            this._card.Controls.Add(this._new);
            this._card.Controls.Add(this._confirm);
            this._card.Controls.Add(this._reveal);
            this._card.Controls.Add(this._status);
            this._card.Controls.Add(this._save);
            this._card.Controls.Add(this._cancel);
            //
            // _title
            //
            this._title.AutoSize = true;
            this._title.BackColor = System.Drawing.Color.Transparent;
            this._title.Font = new System.Drawing.Font("Segoe UI", 15.5F, System.Drawing.FontStyle.Bold);
            this._title.Location = new System.Drawing.Point(32, 30);
            this._title.Name = "_title";
            this._title.Size = new System.Drawing.Size(200, 28);
            this._title.TabIndex = 0;
            this._title.Text = "Yeni parola belirle";
            //
            // _forcedNote
            //
            this._forcedNote.BackColor = System.Drawing.Color.Transparent;
            this._forcedNote.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._forcedNote.Location = new System.Drawing.Point(33, 64);
            this._forcedNote.MaximumSize = new System.Drawing.Size(340, 0);
            this._forcedNote.Name = "_forcedNote";
            this._forcedNote.Size = new System.Drawing.Size(340, 30);
            this._forcedNote.TabIndex = 1;
            this._forcedNote.Tag = "muted";
            this._forcedNote.Text = "İlk giriş: devam etmek için parolanızı değiştirmelisiniz.";
            this._forcedNote.Visible = false;
            //
            // _current
            //
            this._current.BorderRadius = 10;
            this._current.DefaultText = "";
            this._current.Location = new System.Drawing.Point(32, 104);
            this._current.Name = "_current";
            this._current.PasswordChar = '●';
            this._current.PlaceholderText = "Mevcut parola";
            this._current.Size = new System.Drawing.Size(252, 44);
            this._current.TabIndex = 2;
            //
            // _new
            //
            this._new.BorderRadius = 10;
            this._new.DefaultText = "";
            this._new.Location = new System.Drawing.Point(32, 160);
            this._new.Name = "_new";
            this._new.PasswordChar = '●';
            this._new.PlaceholderText = "Yeni parola (en az 10 karakter)";
            this._new.Size = new System.Drawing.Size(252, 44);
            this._new.TabIndex = 3;
            //
            // _confirm
            //
            this._confirm.BorderRadius = 10;
            this._confirm.DefaultText = "";
            this._confirm.Location = new System.Drawing.Point(32, 216);
            this._confirm.Name = "_confirm";
            this._confirm.PasswordChar = '●';
            this._confirm.PlaceholderText = "Yeni parola (tekrar)";
            this._confirm.Size = new System.Drawing.Size(252, 44);
            this._confirm.TabIndex = 4;
            //
            // _reveal
            //
            this._reveal.BorderRadius = 10;
            this._reveal.Location = new System.Drawing.Point(292, 160);
            this._reveal.Name = "_reveal";
            this._reveal.Size = new System.Drawing.Size(84, 44);
            this._reveal.TabIndex = 5;
            this._reveal.Tag = "secondary";
            this._reveal.Text = "Göster";
            //
            // _status
            //
            this._status.BackColor = System.Drawing.Color.Transparent;
            this._status.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this._status.Location = new System.Drawing.Point(33, 274);
            this._status.MaximumSize = new System.Drawing.Size(344, 0);
            this._status.Name = "_status";
            this._status.Size = new System.Drawing.Size(0, 0);
            this._status.TabIndex = 6;
            //
            // _save
            //
            this._save.BorderRadius = 10;
            this._save.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this._save.Location = new System.Drawing.Point(32, 312);
            this._save.Name = "_save";
            this._save.Size = new System.Drawing.Size(224, 46);
            this._save.TabIndex = 7;
            this._save.Text = "Kaydet";
            //
            // _cancel
            //
            this._cancel.BorderRadius = 10;
            this._cancel.Location = new System.Drawing.Point(268, 312);
            this._cancel.Name = "_cancel";
            this._cancel.Size = new System.Drawing.Size(108, 46);
            this._cancel.TabIndex = 8;
            this._cancel.Tag = "secondary";
            this._cancel.Text = "İptal";
            //
            // ChangePasswordForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(480, 512);
            this.Controls.Add(this._card);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ChangePasswordForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Parola değiştir";
            this._card.ResumeLayout(false);
            this._card.PerformLayout();
            this.ResumeLayout(false);
        }

        private Guna.UI2.WinForms.Guna2Panel _card;
        private System.Windows.Forms.Label _title;
        private System.Windows.Forms.Label _forcedNote;
        private Guna.UI2.WinForms.Guna2TextBox _current;
        private Guna.UI2.WinForms.Guna2TextBox _new;
        private Guna.UI2.WinForms.Guna2TextBox _confirm;
        private Guna.UI2.WinForms.Guna2Button _reveal;
        private System.Windows.Forms.Label _status;
        private Guna.UI2.WinForms.Guna2Button _save;
        private Guna.UI2.WinForms.Guna2Button _cancel;
    }
}
