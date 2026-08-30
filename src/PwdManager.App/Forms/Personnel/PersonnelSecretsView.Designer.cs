namespace PwdManager.App.Forms.Personnel
{
    partial class PersonnelSecretsView
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this._list = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            //
            // _list
            //
            this._list.AutoScroll = true;
            this._list.Dock = System.Windows.Forms.DockStyle.Fill;
            this._list.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this._list.Name = "_list";
            this._list.Padding = new System.Windows.Forms.Padding(4, 4, 4, 12);
            this._list.WrapContents = false;
            //
            // PersonnelSecretsView
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._list);
            this.Name = "PersonnelSecretsView";
            this.Size = new System.Drawing.Size(900, 520);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.FlowLayoutPanel _list;
    }
}
