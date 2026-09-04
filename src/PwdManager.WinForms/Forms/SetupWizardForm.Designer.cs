namespace PwdManager.WinForms.Forms
{
    partial class SetupWizardForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this._step1 = new System.Windows.Forms.Panel();
            this._step2 = new System.Windows.Forms.Panel();
            this._step3 = new System.Windows.Forms.Panel();

            this._s1Title = new System.Windows.Forms.Label();
            this._s1Sub = new System.Windows.Forms.Label();
            this._host_ = new Guna.UI2.WinForms.Guna2TextBox();
            this._port = new Guna.UI2.WinForms.Guna2TextBox();
            this._dbName = new Guna.UI2.WinForms.Guna2TextBox();
            this._dbUser = new Guna.UI2.WinForms.Guna2TextBox();
            this._dbPass = new Guna.UI2.WinForms.Guna2TextBox();
            this._s1Status = new System.Windows.Forms.Label();
            this._s1Primary = new Guna.UI2.WinForms.Guna2Button();
            this._s1Cancel = new Guna.UI2.WinForms.Guna2Button();

            this._s2Title = new System.Windows.Forms.Label();
            this._s2Sub = new System.Windows.Forms.Label();
            this._adminUser = new Guna.UI2.WinForms.Guna2TextBox();
            this._adminName = new Guna.UI2.WinForms.Guna2TextBox();
            this._adminPass = new Guna.UI2.WinForms.Guna2TextBox();
            this._adminPass2 = new Guna.UI2.WinForms.Guna2TextBox();
            this._s2Status = new System.Windows.Forms.Label();
            this._s2Primary = new Guna.UI2.WinForms.Guna2Button();

            this._s3Title = new System.Windows.Forms.Label();
            this._s3Sub = new System.Windows.Forms.Label();
            this._codeBox = new Guna.UI2.WinForms.Guna2TextBox();
            this._copy = new Guna.UI2.WinForms.Guna2Button();
            this._confirm = new Guna.UI2.WinForms.Guna2CheckBox();
            this._s3Status = new System.Windows.Forms.Label();
            this._finish = new Guna.UI2.WinForms.Guna2Button();

            this._step1.SuspendLayout();
            this._step2.SuspendLayout();
            this._step3.SuspendLayout();
            this.SuspendLayout();
            //
            // ── step 1 ──────────────────────────────────────────────
            //
            this._step1.Dock = System.Windows.Forms.DockStyle.Fill;
            this._step1.Padding = new System.Windows.Forms.Padding(48, 44, 48, 28);
            this._step1.Name = "_step1";
            this._step1.Controls.Add(this._s1Title);
            this._step1.Controls.Add(this._s1Sub);
            this._step1.Controls.Add(this._host_);
            this._step1.Controls.Add(this._port);
            this._step1.Controls.Add(this._dbName);
            this._step1.Controls.Add(this._dbUser);
            this._step1.Controls.Add(this._dbPass);
            this._step1.Controls.Add(this._s1Status);
            this._step1.Controls.Add(this._s1Primary);
            this._step1.Controls.Add(this._s1Cancel);
            //
            this._s1Title.AutoSize = true;
            this._s1Title.BackColor = System.Drawing.Color.Transparent;
            this._s1Title.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this._s1Title.Location = new System.Drawing.Point(46, 42);
            this._s1Title.Name = "_s1Title";
            this._s1Title.Text = "Veritabanı bağlantısı";
            //
            this._s1Sub.AutoSize = true;
            this._s1Sub.BackColor = System.Drawing.Color.Transparent;
            this._s1Sub.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this._s1Sub.Location = new System.Drawing.Point(48, 82);
            this._s1Sub.Name = "_s1Sub";
            this._s1Sub.Tag = "muted";
            this._s1Sub.Text = "Adım 1 / 2  ·  MySQL/MariaDB sunucusu test edilip şema oluşturulur.";
            //
            this._host_.BorderRadius = 10;
            this._host_.Location = new System.Drawing.Point(48, 128);
            this._host_.Name = "_host_";
            this._host_.PlaceholderText = "Sunucu (ör. 127.0.0.1)";
            this._host_.Size = new System.Drawing.Size(330, 44);
            this._host_.TabIndex = 0;
            //
            this._port.BorderRadius = 10;
            this._port.Location = new System.Drawing.Point(390, 128);
            this._port.Name = "_port";
            this._port.PlaceholderText = "Port";
            this._port.Size = new System.Drawing.Size(114, 44);
            this._port.TabIndex = 1;
            //
            this._dbName.BorderRadius = 10;
            this._dbName.Location = new System.Drawing.Point(48, 188);
            this._dbName.Name = "_dbName";
            this._dbName.PlaceholderText = "Veritabanı adı";
            this._dbName.Size = new System.Drawing.Size(456, 44);
            this._dbName.TabIndex = 2;
            //
            this._dbUser.BorderRadius = 10;
            this._dbUser.Location = new System.Drawing.Point(48, 248);
            this._dbUser.Name = "_dbUser";
            this._dbUser.PlaceholderText = "Kullanıcı";
            this._dbUser.Size = new System.Drawing.Size(222, 44);
            this._dbUser.TabIndex = 3;
            //
            this._dbPass.BorderRadius = 10;
            this._dbPass.Location = new System.Drawing.Point(282, 248);
            this._dbPass.Name = "_dbPass";
            this._dbPass.PasswordChar = '●';
            this._dbPass.PlaceholderText = "Parola (boş bırakılabilir)";
            this._dbPass.Size = new System.Drawing.Size(222, 44);
            this._dbPass.TabIndex = 4;
            //
            this._s1Status.BackColor = System.Drawing.Color.Transparent;
            this._s1Status.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._s1Status.Location = new System.Drawing.Point(48, 312);
            this._s1Status.MaximumSize = new System.Drawing.Size(456, 0);
            this._s1Status.Name = "_s1Status";
            this._s1Status.Size = new System.Drawing.Size(0, 0);
            //
            this._s1Primary.BorderRadius = 10;
            this._s1Primary.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this._s1Primary.Location = new System.Drawing.Point(48, 408);
            this._s1Primary.Name = "_s1Primary";
            this._s1Primary.Size = new System.Drawing.Size(220, 46);
            this._s1Primary.TabIndex = 5;
            this._s1Primary.Text = "Test et ve devam";
            //
            this._s1Cancel.BorderRadius = 10;
            this._s1Cancel.Location = new System.Drawing.Point(282, 408);
            this._s1Cancel.Name = "_s1Cancel";
            this._s1Cancel.Size = new System.Drawing.Size(120, 46);
            this._s1Cancel.TabIndex = 6;
            this._s1Cancel.Tag = "secondary";
            this._s1Cancel.Text = "İptal";
            //
            // ── step 2 ──────────────────────────────────────────────
            //
            this._step2.Dock = System.Windows.Forms.DockStyle.Fill;
            this._step2.Padding = new System.Windows.Forms.Padding(48, 44, 48, 28);
            this._step2.Name = "_step2";
            this._step2.Visible = false;
            this._step2.Controls.Add(this._s2Title);
            this._step2.Controls.Add(this._s2Sub);
            this._step2.Controls.Add(this._adminUser);
            this._step2.Controls.Add(this._adminName);
            this._step2.Controls.Add(this._adminPass);
            this._step2.Controls.Add(this._adminPass2);
            this._step2.Controls.Add(this._s2Status);
            this._step2.Controls.Add(this._s2Primary);
            //
            this._s2Title.AutoSize = true;
            this._s2Title.BackColor = System.Drawing.Color.Transparent;
            this._s2Title.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this._s2Title.Location = new System.Drawing.Point(46, 42);
            this._s2Title.Name = "_s2Title";
            this._s2Title.Text = "İlk yönetici hesabı";
            //
            this._s2Sub.BackColor = System.Drawing.Color.Transparent;
            this._s2Sub.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this._s2Sub.Location = new System.Drawing.Point(48, 82);
            this._s2Sub.MaximumSize = new System.Drawing.Size(456, 0);
            this._s2Sub.Name = "_s2Sub";
            this._s2Sub.Size = new System.Drawing.Size(456, 34);
            this._s2Sub.Tag = "muted";
            this._s2Sub.Text = "Adım 2 / 2  ·  Bu hesap sistemin şifreleme anahtarını taşır; parolasını güvende tut.";
            //
            this._adminUser.BorderRadius = 10;
            this._adminUser.Location = new System.Drawing.Point(48, 134);
            this._adminUser.Name = "_adminUser";
            this._adminUser.PlaceholderText = "Kullanıcı adı";
            this._adminUser.Size = new System.Drawing.Size(456, 44);
            this._adminUser.TabIndex = 0;
            //
            this._adminName.BorderRadius = 10;
            this._adminName.Location = new System.Drawing.Point(48, 194);
            this._adminName.Name = "_adminName";
            this._adminName.PlaceholderText = "Ad soyad";
            this._adminName.Size = new System.Drawing.Size(456, 44);
            this._adminName.TabIndex = 1;
            //
            this._adminPass.BorderRadius = 10;
            this._adminPass.Location = new System.Drawing.Point(48, 254);
            this._adminPass.Name = "_adminPass";
            this._adminPass.PasswordChar = '●';
            this._adminPass.PlaceholderText = "Parola (en az 10 karakter)";
            this._adminPass.Size = new System.Drawing.Size(222, 44);
            this._adminPass.TabIndex = 2;
            //
            this._adminPass2.BorderRadius = 10;
            this._adminPass2.Location = new System.Drawing.Point(282, 254);
            this._adminPass2.Name = "_adminPass2";
            this._adminPass2.PasswordChar = '●';
            this._adminPass2.PlaceholderText = "Parola (tekrar)";
            this._adminPass2.Size = new System.Drawing.Size(222, 44);
            this._adminPass2.TabIndex = 3;
            //
            this._s2Status.BackColor = System.Drawing.Color.Transparent;
            this._s2Status.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._s2Status.Location = new System.Drawing.Point(48, 316);
            this._s2Status.MaximumSize = new System.Drawing.Size(456, 0);
            this._s2Status.Name = "_s2Status";
            this._s2Status.Size = new System.Drawing.Size(0, 0);
            //
            this._s2Primary.BorderRadius = 10;
            this._s2Primary.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this._s2Primary.Location = new System.Drawing.Point(48, 408);
            this._s2Primary.Name = "_s2Primary";
            this._s2Primary.Size = new System.Drawing.Size(240, 46);
            this._s2Primary.TabIndex = 4;
            this._s2Primary.Text = "Yöneticiyi oluştur";
            //
            // ── step 3 ──────────────────────────────────────────────
            //
            this._step3.Dock = System.Windows.Forms.DockStyle.Fill;
            this._step3.Padding = new System.Windows.Forms.Padding(48, 44, 48, 28);
            this._step3.Name = "_step3";
            this._step3.Visible = false;
            this._step3.Controls.Add(this._s3Title);
            this._step3.Controls.Add(this._s3Sub);
            this._step3.Controls.Add(this._codeBox);
            this._step3.Controls.Add(this._copy);
            this._step3.Controls.Add(this._confirm);
            this._step3.Controls.Add(this._s3Status);
            this._step3.Controls.Add(this._finish);
            //
            this._s3Title.AutoSize = true;
            this._s3Title.BackColor = System.Drawing.Color.Transparent;
            this._s3Title.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this._s3Title.Location = new System.Drawing.Point(46, 42);
            this._s3Title.Name = "_s3Title";
            this._s3Title.Text = "Kurtarma kodu";
            //
            this._s3Sub.BackColor = System.Drawing.Color.Transparent;
            this._s3Sub.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this._s3Sub.Location = new System.Drawing.Point(48, 82);
            this._s3Sub.MaximumSize = new System.Drawing.Size(456, 0);
            this._s3Sub.Name = "_s3Sub";
            this._s3Sub.Size = new System.Drawing.Size(456, 42);
            this._s3Sub.Tag = "muted";
            this._s3Sub.Text = "Bu kod yalnızca bir kez gösterilir. Tüm yönetici parolaları kaybolursa sistemi yalnızca bu kod kurtarır. Yazdır veya güvenli bir yere kaydet.";
            //
            this._codeBox.BorderRadius = 10;
            this._codeBox.Font = new System.Drawing.Font("Consolas", 13F, System.Drawing.FontStyle.Bold);
            this._codeBox.Location = new System.Drawing.Point(48, 150);
            this._codeBox.Name = "_codeBox";
            this._codeBox.ReadOnly = true;
            this._codeBox.Size = new System.Drawing.Size(456, 54);
            //
            this._copy.BorderRadius = 10;
            this._copy.Location = new System.Drawing.Point(48, 218);
            this._copy.Name = "_copy";
            this._copy.Size = new System.Drawing.Size(170, 42);
            this._copy.Tag = "secondary";
            this._copy.Text = "Panoya kopyala";
            //
            this._confirm.Location = new System.Drawing.Point(48, 282);
            this._confirm.Name = "_confirm";
            this._confirm.Size = new System.Drawing.Size(456, 28);
            this._confirm.Text = "  Kurtarma kodunu güvenli bir yere kaydettim";
            //
            this._s3Status.BackColor = System.Drawing.Color.Transparent;
            this._s3Status.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._s3Status.Location = new System.Drawing.Point(48, 324);
            this._s3Status.MaximumSize = new System.Drawing.Size(456, 0);
            this._s3Status.Name = "_s3Status";
            this._s3Status.Size = new System.Drawing.Size(0, 0);
            //
            this._finish.BorderRadius = 10;
            this._finish.Enabled = false;
            this._finish.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this._finish.Location = new System.Drawing.Point(48, 400);
            this._finish.Name = "_finish";
            this._finish.Size = new System.Drawing.Size(240, 46);
            this._finish.Text = "Bitir ve girişe geç";
            //
            // ── SetupWizardForm ────────────────────────────────────
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 560);
            this.Controls.Add(this._step1);
            this.Controls.Add(this._step2);
            this.Controls.Add(this._step3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "SetupWizardForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PwdManager — Kurulum";
            this._step1.ResumeLayout(false);
            this._step1.PerformLayout();
            this._step2.ResumeLayout(false);
            this._step2.PerformLayout();
            this._step3.ResumeLayout(false);
            this._step3.PerformLayout();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel _step1;
        private System.Windows.Forms.Panel _step2;
        private System.Windows.Forms.Panel _step3;
        private System.Windows.Forms.Label _s1Title;
        private System.Windows.Forms.Label _s1Sub;
        private Guna.UI2.WinForms.Guna2TextBox _host_;
        private Guna.UI2.WinForms.Guna2TextBox _port;
        private Guna.UI2.WinForms.Guna2TextBox _dbName;
        private Guna.UI2.WinForms.Guna2TextBox _dbUser;
        private Guna.UI2.WinForms.Guna2TextBox _dbPass;
        private System.Windows.Forms.Label _s1Status;
        private Guna.UI2.WinForms.Guna2Button _s1Primary;
        private Guna.UI2.WinForms.Guna2Button _s1Cancel;
        private System.Windows.Forms.Label _s2Title;
        private System.Windows.Forms.Label _s2Sub;
        private Guna.UI2.WinForms.Guna2TextBox _adminUser;
        private Guna.UI2.WinForms.Guna2TextBox _adminName;
        private Guna.UI2.WinForms.Guna2TextBox _adminPass;
        private Guna.UI2.WinForms.Guna2TextBox _adminPass2;
        private System.Windows.Forms.Label _s2Status;
        private Guna.UI2.WinForms.Guna2Button _s2Primary;
        private System.Windows.Forms.Label _s3Title;
        private System.Windows.Forms.Label _s3Sub;
        private Guna.UI2.WinForms.Guna2TextBox _codeBox;
        private Guna.UI2.WinForms.Guna2Button _copy;
        private Guna.UI2.WinForms.Guna2CheckBox _confirm;
        private System.Windows.Forms.Label _s3Status;
        private Guna.UI2.WinForms.Guna2Button _finish;
    }
}
