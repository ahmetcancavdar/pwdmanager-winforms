namespace PwdManager.WinForms.Forms
{
    partial class LoginForm
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
            this._brandDot = new Guna.UI2.WinForms.Guna2Panel();
            this._title = new System.Windows.Forms.Label();
            this._subtitle = new System.Windows.Forms.Label();
            this._userLabel = new System.Windows.Forms.Label();
            this._username = new Guna.UI2.WinForms.Guna2TextBox();
            this._passLabel = new System.Windows.Forms.Label();
            this._password = new Guna.UI2.WinForms.Guna2TextBox();
            this._reveal = new Guna.UI2.WinForms.Guna2Button();
            this._status = new System.Windows.Forms.Label();
            this._loginButton = new Guna.UI2.WinForms.Guna2Button();
            this._card.SuspendLayout();
            this.SuspendLayout();
            //
            // _card
            //
            this._card.Location = new System.Drawing.Point(44, 52);
            this._card.Name = "_card";
            this._card.Size = new System.Drawing.Size(404, 400);
            this._card.Tag = "card";
            this._card.Controls.Add(this._brandDot);
            this._card.Controls.Add(this._title);
            this._card.Controls.Add(this._subtitle);
            this._card.Controls.Add(this._userLabel);
            this._card.Controls.Add(this._username);
            this._card.Controls.Add(this._passLabel);
            this._card.Controls.Add(this._password);
            this._card.Controls.Add(this._reveal);
            this._card.Controls.Add(this._status);
            this._card.Controls.Add(this._loginButton);
            //
            // _brandDot
            //
            this._brandDot.BorderRadius = 9;
            this._brandDot.FillColor = System.Drawing.Color.FromArgb(124, 92, 255);
            this._brandDot.Location = new System.Drawing.Point(34, 24);
            this._brandDot.Name = "_brandDot";
            this._brandDot.Size = new System.Drawing.Size(56, 56);
            this._brandDot.Tag = "accent";
            //
            // _title
            //
            this._title.AutoSize = true;
            this._title.BackColor = System.Drawing.Color.Transparent;
            this._title.Font = new System.Drawing.Font("Segoe UI", 17F, System.Drawing.FontStyle.Bold);
            this._title.Location = new System.Drawing.Point(102, 38);
            this._title.Name = "_title";
            this._title.Size = new System.Drawing.Size(150, 32);
            this._title.TabIndex = 0;
            this._title.Text = "PwdManager";
            //
            // _subtitle
            //
            this._subtitle.AutoSize = true;
            this._subtitle.BackColor = System.Drawing.Color.Transparent;
            this._subtitle.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this._subtitle.Location = new System.Drawing.Point(37, 82);
            this._subtitle.Name = "_subtitle";
            this._subtitle.Size = new System.Drawing.Size(180, 19);
            this._subtitle.TabIndex = 1;
            this._subtitle.Tag = "muted";
            this._subtitle.Text = "Devam etmek için giriş yapın.";
            //
            // _userLabel
            //
            this._userLabel.AutoSize = true;
            this._userLabel.BackColor = System.Drawing.Color.Transparent;
            this._userLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this._userLabel.Location = new System.Drawing.Point(37, 122);
            this._userLabel.Name = "_userLabel";
            this._userLabel.Tag = "overline";
            this._userLabel.Text = "KULLANICI ADI";
            //
            // _username
            //
            this._username.BorderRadius = 10;
            this._username.Location = new System.Drawing.Point(37, 144);
            this._username.Name = "_username";
            this._username.PlaceholderText = "Kullanıcı adı";
            this._username.Size = new System.Drawing.Size(330, 44);
            this._username.TabIndex = 2;
            //
            // _passLabel
            //
            this._passLabel.AutoSize = true;
            this._passLabel.BackColor = System.Drawing.Color.Transparent;
            this._passLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this._passLabel.Location = new System.Drawing.Point(37, 204);
            this._passLabel.Name = "_passLabel";
            this._passLabel.Tag = "overline";
            this._passLabel.Text = "PAROLA";
            //
            // _password
            //
            this._password.BorderRadius = 10;
            this._password.Location = new System.Drawing.Point(37, 226);
            this._password.Name = "_password";
            this._password.PasswordChar = '●';
            this._password.PlaceholderText = "Parola";
            this._password.Size = new System.Drawing.Size(234, 44);
            this._password.TabIndex = 3;
            //
            // _reveal
            //
            this._reveal.BorderRadius = 10;
            this._reveal.Location = new System.Drawing.Point(279, 226);
            this._reveal.Name = "_reveal";
            this._reveal.Size = new System.Drawing.Size(88, 44);
            this._reveal.TabIndex = 4;
            this._reveal.Tag = "secondary";
            this._reveal.Text = "Göster";
            //
            // _status
            //
            this._status.BackColor = System.Drawing.Color.Transparent;
            this._status.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this._status.Location = new System.Drawing.Point(37, 282);
            this._status.MaximumSize = new System.Drawing.Size(330, 0);
            this._status.Name = "_status";
            this._status.Size = new System.Drawing.Size(0, 0);
            this._status.TabIndex = 5;
            //
            // _loginButton
            //
            this._loginButton.BorderRadius = 10;
            this._loginButton.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this._loginButton.Location = new System.Drawing.Point(37, 318);
            this._loginButton.Name = "_loginButton";
            this._loginButton.Size = new System.Drawing.Size(330, 48);
            this._loginButton.TabIndex = 6;
            this._loginButton.Text = "Giriş yap";
            //
            // LoginForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(492, 504);
            this.Controls.Add(this._card);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PwdManager — Giriş";
            this._card.ResumeLayout(false);
            this._card.PerformLayout();
            this.ResumeLayout(false);
        }

        private Guna.UI2.WinForms.Guna2Panel _card;
        private Guna.UI2.WinForms.Guna2Panel _brandDot;
        private System.Windows.Forms.Label _title;
        private System.Windows.Forms.Label _subtitle;
        private System.Windows.Forms.Label _userLabel;
        private Guna.UI2.WinForms.Guna2TextBox _username;
        private System.Windows.Forms.Label _passLabel;
        private Guna.UI2.WinForms.Guna2TextBox _password;
        private Guna.UI2.WinForms.Guna2Button _reveal;
        private System.Windows.Forms.Label _status;
        private Guna.UI2.WinForms.Guna2Button _loginButton;
    }
}
