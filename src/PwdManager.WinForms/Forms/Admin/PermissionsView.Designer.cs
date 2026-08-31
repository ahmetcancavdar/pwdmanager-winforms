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
            _heading = new Label();
            _split = new Panel();
            _treeHost = new Panel();
            _tree = new TreeView();
            _people = new ListBox();
            _status = new Label();
            _split.SuspendLayout();
            _treeHost.SuspendLayout();
            SuspendLayout();
            // 
            // _heading
            // 
            _heading.AutoSize = true;
            _heading.BackColor = Color.Transparent;
            _heading.Dock = DockStyle.Top;
            _heading.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            _heading.Location = new Point(0, 0);
            _heading.Name = "_heading";
            _heading.Padding = new Padding(2, 3, 0, 11);
            _heading.Size = new Size(106, 49);
            _heading.TabIndex = 1;
            _heading.Text = "Yetkiler";
            // 
            // _split
            // 
            _split.Controls.Add(_treeHost);
            _split.Controls.Add(_people);
            _split.Dock = DockStyle.Fill;
            _split.Location = new Point(0, 49);
            _split.Margin = new Padding(3, 4, 3, 4);
            _split.Name = "_split";
            _split.Size = new Size(937, 615);
            _split.TabIndex = 0;
            // 
            // _treeHost
            // 
            _treeHost.Controls.Add(_tree);
            _treeHost.Dock = DockStyle.Fill;
            _treeHost.Location = new Point(274, 0);
            _treeHost.Margin = new Padding(3, 4, 3, 4);
            _treeHost.Name = "_treeHost";
            _treeHost.Padding = new Padding(14, 0, 0, 0);
            _treeHost.Size = new Size(663, 615);
            _treeHost.TabIndex = 0;
            // 
            // _tree
            // 
            _tree.BorderStyle = BorderStyle.None;
            _tree.CheckBoxes = true;
            _tree.Dock = DockStyle.Fill;
            _tree.Font = new Font("Segoe UI", 9.75F);
            _tree.ItemHeight = 28;
            _tree.Location = new Point(14, 0);
            _tree.Margin = new Padding(3, 4, 3, 4);
            _tree.Name = "_tree";
            _tree.Size = new Size(649, 615);
            _tree.TabIndex = 0;
            // 
            // _people
            // 
            _people.BorderStyle = BorderStyle.None;
            _people.Dock = DockStyle.Left;
            _people.Font = new Font("Segoe UI", 9.75F);
            _people.IntegralHeight = false;
            _people.ItemHeight = 21;
            _people.Location = new Point(0, 0);
            _people.Margin = new Padding(3, 4, 3, 4);
            _people.Name = "_people";
            _people.Size = new Size(274, 615);
            _people.TabIndex = 1;
            // 
            // _status
            // 
            _status.BackColor = Color.Transparent;
            _status.Dock = DockStyle.Bottom;
            _status.Font = new Font("Segoe UI", 9.75F);
            _status.Location = new Point(0, 664);
            _status.Name = "_status";
            _status.Size = new Size(937, 29);
            _status.TabIndex = 2;
            // 
            // PermissionsView
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(_split);
            Controls.Add(_heading);
            Controls.Add(_status);
            Margin = new Padding(3, 4, 3, 4);
            Name = "PermissionsView";
            Size = new Size(937, 693);
            _split.ResumeLayout(false);
            _treeHost.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.Label _heading;
        private System.Windows.Forms.Panel _split;
        private System.Windows.Forms.Panel _treeHost;
        private System.Windows.Forms.TreeView _tree;
        private System.Windows.Forms.ListBox _people;
        private System.Windows.Forms.Label _status;
    }
}
