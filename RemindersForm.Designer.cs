namespace frm_vietnamese_calendar
{
    partial class RemindersForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.listView  = new System.Windows.Forms.ListView();
            this.btnAdd    = new System.Windows.Forms.Button();
            this.btnEdit   = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnOk     = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // listView
            this.listView.FullRowSelect = true;
            this.listView.GridLines     = true;
            this.listView.Location      = new System.Drawing.Point(12, 12);
            this.listView.MultiSelect   = false;
            this.listView.Name          = "listView";
            this.listView.Size          = new System.Drawing.Size(530, 280);
            this.listView.TabIndex      = 0;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View          = System.Windows.Forms.View.Details;

            // btnAdd
            this.btnAdd.Location = new System.Drawing.Point(12, 302);
            this.btnAdd.Name     = "btnAdd";
            this.btnAdd.Size     = new System.Drawing.Size(80, 28);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Click   += new System.EventHandler(this.btnAdd_Click);

            // btnEdit
            this.btnEdit.Location = new System.Drawing.Point(100, 302);
            this.btnEdit.Name     = "btnEdit";
            this.btnEdit.Size     = new System.Drawing.Size(80, 28);
            this.btnEdit.TabIndex = 2;
            this.btnEdit.Click   += new System.EventHandler(this.btnEdit_Click);

            // btnDelete
            this.btnDelete.Location = new System.Drawing.Point(188, 302);
            this.btnDelete.Name     = "btnDelete";
            this.btnDelete.Size     = new System.Drawing.Size(80, 28);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Click   += new System.EventHandler(this.btnDelete_Click);

            // btnOk
            this.btnOk.Location = new System.Drawing.Point(380, 302);
            this.btnOk.Name     = "btnOk";
            this.btnOk.Size     = new System.Drawing.Size(75, 28);
            this.btnOk.TabIndex = 4;
            this.btnOk.Click   += new System.EventHandler(this.btnOk_Click);

            // btnCancel
            this.btnCancel.Location = new System.Drawing.Point(462, 302);
            this.btnCancel.Name     = "btnCancel";
            this.btnCancel.Size     = new System.Drawing.Size(80, 28);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Click   += new System.EventHandler(this.btnCancel_Click);

            // RemindersForm
            this.AutoScaleMode      = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize         = new System.Drawing.Size(556, 344);
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.listView, this.btnAdd, this.btnEdit, this.btnDelete, this.btnOk, this.btnCancel });
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox     = false;
            this.MinimizeBox     = false;
            this.Name            = "RemindersForm";
            this.StartPosition   = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.ListView listView;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
    }
}
