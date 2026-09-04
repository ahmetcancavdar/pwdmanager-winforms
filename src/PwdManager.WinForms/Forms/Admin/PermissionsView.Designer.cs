namespace PwdManager.WinForms.Forms.Admin
{
    partial class PermissionsView
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this._pageHeader = new Guna.UI2.WinForms.Guna2Panel();
            this._title = new System.Windows.Forms.Label();
            this._subtitle = new System.Windows.Forms.Label();
            this._headerDivider = new Guna.UI2.WinForms.Guna2Panel();
            this._split = new System.Windows.Forms.Panel();
            this._peopleCard = new Guna.UI2.WinForms.Guna2Panel();
            this._peopleLabel = new System.Windows.Forms.Label();
            this._people = new System.Windows.Forms.ListBox();
            this._bodyGap = new System.Windows.Forms.Panel();
            this._treeCard = new Guna.UI2.WinForms.Guna2Panel();
            this._tree = new System.Windows.Forms.TreeView();
            this._status = new System.Windows.Forms.Label();
            this._pageHeader.SuspendLayout();
            this._split.SuspendLayout();
            this._peopleCard.SuspendLayout();
            this._treeCard.SuspendLayout();
            this.SuspendLayout();
            //
            // _pageHeader
            //
            this._pageHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this._pageHeader.Height = 84;
            this._pageHeader.Name = "_pageHeader";
            this._pageHeader.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._pageHeader.Controls.Add(this._subtitle);
            this._pageHeader.Controls.Add(this._title);
            //
            // _title
            //
            this._title.AutoSize = true;
            this._title.BackColor = System.Drawing.Color.Transparent;
            this._title.Font = new System.Drawing.Font("Segoe UI", 15.5F, System.Drawing.FontStyle.Bold);
            this._title.Location = new System.Drawing.Point(4, 14);
            this._title.Name = "_title";
            this._title.Size = new System.Drawing.Size(90, 28);
            this._title.TabIndex = 0;
            this._title.Text = "Yetkiler";
            //
            // _subtitle
            //
            this._subtitle.AutoSize = true;
            this._subtitle.BackColor = System.Drawing.Color.Transparent;
            this._subtitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._subtitle.Location = new System.Drawing.Point(6, 48);
            this._subtitle.Name = "_subtitle";
            this._subtitle.Size = new System.Drawing.Size(420, 15);
            this._subtitle.TabIndex = 1;
            this._subtitle.Tag = "muted";
            this._subtitle.Text = "Soldan personel seç, sağdan kategori/parola erişimini işaretle. Anında yazılır.";
            //
            // _headerDivider
            //
            this._headerDivider.Dock = System.Windows.Forms.DockStyle.Top;
            this._headerDivider.Height = 1;
            this._headerDivider.Name = "_headerDivider";
            this._headerDivider.Tag = "divider";
            //
            // _split
            //
            this._split.Dock = System.Windows.Forms.DockStyle.Fill;
            this._split.Name = "_split";
            this._split.Padding = new System.Windows.Forms.Padding(0, 12, 0, 4);
            this._split.Controls.Add(this._treeCard);
            this._split.Controls.Add(this._bodyGap);
            this._split.Controls.Add(this._peopleCard);
            //
            // _peopleCard
            //
            this._peopleCard.Dock = System.Windows.Forms.DockStyle.Left;
            this._peopleCard.Width = 288;
            this._peopleCard.Name = "_peopleCard";
            this._peopleCard.Padding = new System.Windows.Forms.Padding(6, 8, 6, 6);
            this._peopleCard.Tag = "card";
            this._peopleCard.Controls.Add(this._people);
            this._peopleCard.Controls.Add(this._peopleLabel);
            //
            // _peopleLabel
            //
            this._peopleLabel.BackColor = System.Drawing.Color.Transparent;
            this._peopleLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this._peopleLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this._peopleLabel.Height = 22;
            this._peopleLabel.Name = "_peopleLabel";
            this._peopleLabel.Padding = new System.Windows.Forms.Padding(6, 4, 0, 0);
            this._peopleLabel.Tag = "overline";
            this._peopleLabel.Text = "PERSONEL";
            //
            // _people
            //
            this._people.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._people.Dock = System.Windows.Forms.DockStyle.Fill;
            this._people.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this._people.IntegralHeight = false;
            this._people.ItemHeight = 28;
            this._people.Name = "_people";
            this._people.TabIndex = 0;
            //
            // _bodyGap
            //
            this._bodyGap.Dock = System.Windows.Forms.DockStyle.Left;
            this._bodyGap.Width = 16;
            this._bodyGap.Name = "_bodyGap";
            //
            // _treeCard
            //
            this._treeCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this._treeCard.Name = "_treeCard";
            this._treeCard.Padding = new System.Windows.Forms.Padding(12, 12, 8, 12);
            this._treeCard.Tag = "card";
            this._treeCard.Controls.Add(this._tree);
            //
            // _tree
            //
            this._tree.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._tree.CheckBoxes = true;
            this._tree.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tree.Font = new System.Drawing.Font("Segoe UI", 10F);
            this._tree.FullRowSelect = true;
            this._tree.ItemHeight = 32;
            this._tree.Name = "_tree";
            this._tree.ShowLines = false;
            this._tree.ShowRootLines = false;
            this._tree.TabIndex = 0;
            //
            // _status
            //
            this._status.BackColor = System.Drawing.Color.Transparent;
            this._status.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._status.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this._status.Height = 30;
            this._status.Name = "_status";
            this._status.Padding = new System.Windows.Forms.Padding(4, 8, 4, 4);
            this._status.TabIndex = 2;
            //
            // PermissionsView
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._split);
            this.Controls.Add(this._headerDivider);
            this.Controls.Add(this._pageHeader);
            this.Controls.Add(this._status);
            this.Name = "PermissionsView";
            this.Size = new System.Drawing.Size(937, 693);
            this._pageHeader.ResumeLayout(false);
            this._pageHeader.PerformLayout();
            this._split.ResumeLayout(false);
            this._peopleCard.ResumeLayout(false);
            this._treeCard.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private Guna.UI2.WinForms.Guna2Panel _pageHeader;
        private System.Windows.Forms.Label _title;
        private System.Windows.Forms.Label _subtitle;
        private Guna.UI2.WinForms.Guna2Panel _headerDivider;
        private System.Windows.Forms.Panel _split;
        private Guna.UI2.WinForms.Guna2Panel _peopleCard;
        private System.Windows.Forms.Label _peopleLabel;
        private System.Windows.Forms.ListBox _people;
        private System.Windows.Forms.Panel _bodyGap;
        private Guna.UI2.WinForms.Guna2Panel _treeCard;
        private System.Windows.Forms.TreeView _tree;
        private System.Windows.Forms.Label _status;
    }
}
