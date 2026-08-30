namespace PwdManager.App.Forms
{
    partial class LoginForm
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
            this._subtitle = new System.Windows.Forms.Label();
            this._username = new Guna.UI2.WinForms.Guna2TextBox();
            this._password = new Guna.UI2.WinForms.Guna2TextBox();
            this._reveal = new Guna.UI2.WinForms.Guna2Button();
            this._status = new System.Windows.Forms.Label();
            this._loginButton = new Guna.UI2.WinForms.Guna2Button();
            this._newWindow = new Guna.UI2.WinForms.Guna2Button();
            this.SuspendLayout();
            //
            // _title
            //
            this._title.AutoSize = true;
            this._title.BackColor = System.Drawing.Color.Transparent;
            this._title.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this._title.Location = new System.Drawing.Point(48, 48);
            this._title.Name = "_title";
            this._title.Size = new System.Drawing.Size(160, 28);
            this._title.TabIndex = 0;
            this._title.Text = "Şifre Yöneticisi";
            //
            // _subtitle
            //
            this._subtitle.AutoSize = true;
            this._subtitle.BackColor = System.Drawing.Color.Transparent;
            this._subtitle.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this._subtitle.Location = new System.Drawing.Point(48, 88);
            this._subtitle.Name = "_subtitle";
            this._subtitle.Size = new System.Drawing.Size(180, 19);
            this._subtitle.TabIndex = 1;
            this._subtitle.Text = "Devam etmek için giriş yapın";
            //
            // _username
            //
            this._username.BorderRadius = 8;
            this._username.Location = new System.Drawing.Point(48, 140);
            this._username.Name = "_username";
            this._username.PlaceholderText = "Kullanıcı adı";
            this._username.Size = new System.Drawing.Size(324, 42);
            this._username.TabIndex = 2;
            //
            // _password
            //
            this._password.BorderRadius = 8;
            this._password.Location = new System.Drawing.Point(48, 196);
            this._password.Name = "_password";
            this._password.PasswordChar = '●';
            this._password.PlaceholderText = "Parola";
            this._password.Size = new System.Drawing.Size(244, 42);
            this._password.TabIndex = 3;
            //
            // _reveal
            //
            this._reveal.BorderRadius = 8;
            this._reveal.Location = new System.Drawing.Point(300, 196);
            this._reveal.Name = "_reveal";
            this._reveal.Size = new System.Drawing.Size(72, 42);
            this._reveal.TabIndex = 4;
            this._reveal.Tag = "secondary";
            this._reveal.Text = "Göster";
            //
            // _status
            //
            this._status.BackColor = System.Drawing.Color.Transparent;
            this._status.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this._status.Location = new System.Drawing.Point(48, 250);
            this._status.MaximumSize = new System.Drawing.Size(324, 0);
            this._status.Name = "_status";
            this._status.Size = new System.Drawing.Size(0, 0);
            this._status.TabIndex = 5;
            //
            // _loginButton
            //
            this._loginButton.BorderRadius = 8;
            this._loginButton.Location = new System.Drawing.Point(48, 296);
            this._loginButton.Name = "_loginButton";
            this._loginButton.Size = new System.Drawing.Size(324, 46);
            this._loginButton.TabIndex = 6;
            this._loginButton.Text = "Giriş yap";
            //
            // _newWindow
            //
            this._newWindow.BorderRadius = 8;
            this._newWindow.Location = new System.Drawing.Point(48, 352);
            this._newWindow.Name = "_newWindow";
            this._newWindow.Size = new System.Drawing.Size(324, 34);
            this._newWindow.TabIndex = 7;
            this._newWindow.Tag = "secondary";
            this._newWindow.Text = "Yeni giriş penceresi";
            //
            // LoginForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(420, 420);
            this.Controls.Add(this._title);
            this.Controls.Add(this._subtitle);
            this.Controls.Add(this._username);
            this.Controls.Add(this._password);
            this.Controls.Add(this._reveal);
            this.Controls.Add(this._status);
            this.Controls.Add(this._loginButton);
            this.Controls.Add(this._newWindow);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PwdManager — Giriş";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label _title;
        private System.Windows.Forms.Label _subtitle;
        private Guna.UI2.WinForms.Guna2TextBox _username;
        private Guna.UI2.WinForms.Guna2TextBox _password;
        private Guna.UI2.WinForms.Guna2Button _reveal;
        private System.Windows.Forms.Label _status;
        private Guna.UI2.WinForms.Guna2Button _loginButton;
        private Guna.UI2.WinForms.Guna2Button _newWindow;
    }
}
