namespace PwdManager.WinForms.Forms.Personnel
{
    partial class SecretRowControl
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _timer?.Stop();
                if (components != null) components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this._card = new Guna.UI2.WinForms.Guna2Panel();
            this._top = new System.Windows.Forms.Panel();
            this._bottom = new System.Windows.Forms.Panel();
            this._title = new System.Windows.Forms.Label();
            this._user = new System.Windows.Forms.Label();
            this._mask = new System.Windows.Forms.Label();
            this._status = new System.Windows.Forms.Label();
            this._action = new Guna.UI2.WinForms.Guna2Button();
            this._ok = new Guna.UI2.WinForms.Guna2Button();
            this._cancel = new Guna.UI2.WinForms.Guna2Button();
            this._hide = new Guna.UI2.WinForms.Guna2Button();
            this._pass = new Guna.UI2.WinForms.Guna2TextBox();
            this._timer = new System.Windows.Forms.Timer(this.components);
            this._card.SuspendLayout();
            this._top.SuspendLayout();
            this._bottom.SuspendLayout();
            this.SuspendLayout();
            //
            // _card
            //
            this._card.BorderRadius = 10;
            this._card.Dock = System.Windows.Forms.DockStyle.Fill;
            this._card.Name = "_card";
            this._card.Tag = "row";
            this._card.Controls.Add(this._top);
            this._card.Controls.Add(this._bottom);
            //
            // _top
            //
            this._top.BackColor = System.Drawing.Color.Transparent;
            this._top.Dock = System.Windows.Forms.DockStyle.Fill;
            this._top.Name = "_top";
            this._top.Controls.Add(this._title);
            this._top.Controls.Add(this._user);
            this._top.Controls.Add(this._mask);
            this._top.Controls.Add(this._action);
            //
            // _bottom
            //
            this._bottom.BackColor = System.Drawing.Color.Transparent;
            this._bottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._bottom.Height = 68;
            this._bottom.Name = "_bottom";
            this._bottom.Visible = false;
            this._bottom.Controls.Add(this._pass);
            this._bottom.Controls.Add(this._ok);
            this._bottom.Controls.Add(this._cancel);
            this._bottom.Controls.Add(this._hide);
            this._bottom.Controls.Add(this._status);
            //
            // labels
            //
            this._title.AutoSize = true;
            this._title.BackColor = System.Drawing.Color.Transparent;
            this._title.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this._title.Location = new System.Drawing.Point(16, 15);
            this._title.Name = "_title";
            //
            this._user.AutoSize = true;
            this._user.BackColor = System.Drawing.Color.Transparent;
            this._user.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this._user.Location = new System.Drawing.Point(340, 17);
            this._user.Name = "_user";
            this._user.Text = "kullanıcı: ••••";
            //
            this._mask.AutoSize = true;
            this._mask.BackColor = System.Drawing.Color.Transparent;
            this._mask.Font = new System.Drawing.Font("Consolas", 11F);
            this._mask.Location = new System.Drawing.Point(600, 16);
            this._mask.Name = "_mask";
            this._mask.Text = "••••••••••";
            //
            this._status.AutoSize = true;
            this._status.BackColor = System.Drawing.Color.Transparent;
            this._status.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this._status.Location = new System.Drawing.Point(16, 46);
            this._status.Name = "_status";
            //
            // buttons
            //
            this._action.BorderRadius = 8;
            this._action.Size = new System.Drawing.Size(94, 32);
            this._action.Name = "_action";
            this._action.Tag = "secondary";
            this._action.Text = "Göster";
            //
            this._ok.BorderRadius = 8;
            this._ok.Size = new System.Drawing.Size(96, 34);
            this._ok.Name = "_ok";
            this._ok.Text = "Onayla";
            //
            this._cancel.BorderRadius = 8;
            this._cancel.Size = new System.Drawing.Size(78, 34);
            this._cancel.Name = "_cancel";
            this._cancel.Tag = "secondary";
            this._cancel.Text = "İptal";
            //
            this._hide.BorderRadius = 8;
            this._hide.Size = new System.Drawing.Size(78, 34);
            this._hide.Name = "_hide";
            this._hide.Tag = "secondary";
            this._hide.Text = "Gizle";
            this._hide.Visible = false;
            //
            // _pass
            //
            this._pass.BorderRadius = 8;
            this._pass.Location = new System.Drawing.Point(16, 8);
            this._pass.Name = "_pass";
            this._pass.PasswordChar = '●';
            this._pass.PlaceholderText = "Giriş parolan";
            this._pass.Size = new System.Drawing.Size(260, 36);
            //
            // _timer
            //
            this._timer.Interval = 1000;
            //
            // SecretRowControl
            //
            this.Controls.Add(this._card);
            this.Name = "SecretRowControl";
            this.Size = new System.Drawing.Size(900, 48);
            this._card.ResumeLayout(false);
            this._top.ResumeLayout(false);
            this._top.PerformLayout();
            this._bottom.ResumeLayout(false);
            this._bottom.PerformLayout();
            this.ResumeLayout(false);
        }

        private Guna.UI2.WinForms.Guna2Panel _card;
        private System.Windows.Forms.Panel _top;
        private System.Windows.Forms.Panel _bottom;
        private System.Windows.Forms.Label _title;
        private System.Windows.Forms.Label _user;
        private System.Windows.Forms.Label _mask;
        private System.Windows.Forms.Label _status;
        private Guna.UI2.WinForms.Guna2Button _action;
        private Guna.UI2.WinForms.Guna2Button _ok;
        private Guna.UI2.WinForms.Guna2Button _cancel;
        private Guna.UI2.WinForms.Guna2Button _hide;
        private Guna.UI2.WinForms.Guna2TextBox _pass;
        private System.Windows.Forms.Timer _timer;
    }
}
