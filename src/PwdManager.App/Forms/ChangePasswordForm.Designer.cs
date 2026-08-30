namespace PwdManager.App.Forms
{
    partial class ChangePasswordForm
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
            this._title = new System.Windows.Forms.Label();
            this._forcedNote = new System.Windows.Forms.Label();
            this._current = new Guna.UI2.WinForms.Guna2TextBox();
            this._new = new Guna.UI2.WinForms.Guna2TextBox();
            this._confirm = new Guna.UI2.WinForms.Guna2TextBox();
            this._reveal = new Guna.UI2.WinForms.Guna2Button();
            this._status = new System.Windows.Forms.Label();
            this._save = new Guna.UI2.WinForms.Guna2Button();
            this._cancel = new Guna.UI2.WinForms.Guna2Button();
            this.SuspendLayout();
            //
            // _title
            //
            this._title.AutoSize = true;
            this._title.BackColor = System.Drawing.Color.Transparent;
            this._title.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this._title.Location = new System.Drawing.Point(40, 28);
            this._title.Name = "_title";
            this._title.Size = new System.Drawing.Size(214, 28);
            this._title.TabIndex = 0;
            this._title.Text = "Yeni parola belirle";
            //
            // _forcedNote
            //
            this._forcedNote.BackColor = System.Drawing.Color.Transparent;
            this._forcedNote.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this._forcedNote.Location = new System.Drawing.Point(40, 60);
            this._forcedNote.MaximumSize = new System.Drawing.Size(340, 0);
            this._forcedNote.Name = "_forcedNote";
            this._forcedNote.Size = new System.Drawing.Size(340, 20);
            this._forcedNote.TabIndex = 1;
            this._forcedNote.Text = "İlk giriş: devam etmek için parolanızı değiştirmelisiniz.";
            this._forcedNote.Visible = false;
            //
            // _current
            //
            this._current.BorderRadius = 8;
            this._current.DefaultText = "";
            this._current.Location = new System.Drawing.Point(40, 88);
            this._current.Name = "_current";
            this._current.PasswordChar = '●';
            this._current.PlaceholderText = "Mevcut parola";
            this._current.Size = new System.Drawing.Size(260, 42);
            this._current.TabIndex = 2;
            //
            // _new
            //
            this._new.BorderRadius = 8;
            this._new.DefaultText = "";
            this._new.Location = new System.Drawing.Point(40, 140);
            this._new.Name = "_new";
            this._new.PasswordChar = '●';
            this._new.PlaceholderText = "Yeni parola (en az 10 karakter)";
            this._new.Size = new System.Drawing.Size(260, 42);
            this._new.TabIndex = 3;
            //
            // _confirm
            //
            this._confirm.BorderRadius = 8;
            this._confirm.DefaultText = "";
            this._confirm.Location = new System.Drawing.Point(40, 192);
            this._confirm.Name = "_confirm";
            this._confirm.PasswordChar = '●';
            this._confirm.PlaceholderText = "Yeni parola (tekrar)";
            this._confirm.Size = new System.Drawing.Size(260, 42);
            this._confirm.TabIndex = 4;
            //
            // _reveal
            //
            this._reveal.BorderRadius = 8;
            this._reveal.Location = new System.Drawing.Point(312, 140);
            this._reveal.Name = "_reveal";
            this._reveal.Size = new System.Drawing.Size(76, 42);
            this._reveal.TabIndex = 5;
            this._reveal.Tag = "secondary";
            this._reveal.Text = "Göster";
            //
            // _status
            //
            this._status.BackColor = System.Drawing.Color.Transparent;
            this._status.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this._status.Location = new System.Drawing.Point(40, 244);
            this._status.MaximumSize = new System.Drawing.Size(340, 0);
            this._status.Name = "_status";
            this._status.Size = new System.Drawing.Size(0, 0);
            this._status.TabIndex = 6;
            //
            // _save
            //
            this._save.BorderRadius = 8;
            this._save.Location = new System.Drawing.Point(40, 292);
            this._save.Name = "_save";
            this._save.Size = new System.Drawing.Size(200, 44);
            this._save.TabIndex = 7;
            this._save.Text = "Kaydet";
            //
            // _cancel
            //
            this._cancel.BorderRadius = 8;
            this._cancel.Location = new System.Drawing.Point(260, 292);
            this._cancel.Name = "_cancel";
            this._cancel.Size = new System.Drawing.Size(120, 44);
            this._cancel.TabIndex = 8;
            this._cancel.Tag = "secondary";
            this._cancel.Text = "İptal";
            //
            // ChangePasswordForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(420, 380);
            this.Controls.Add(this._title);
            this.Controls.Add(this._forcedNote);
            this.Controls.Add(this._current);
            this.Controls.Add(this._new);
            this.Controls.Add(this._confirm);
            this.Controls.Add(this._reveal);
            this.Controls.Add(this._status);
            this.Controls.Add(this._save);
            this.Controls.Add(this._cancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ChangePasswordForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Parola değiştir";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

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
