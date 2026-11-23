namespace frm_vietnamese_calendar
{
    partial class ShortcutsHelpForm
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
            this.pnlTitleBar = new System.Windows.Forms.Panel();
            this.lblTitleClose = new System.Windows.Forms.Label();
            this.lblTitleText = new System.Windows.Forms.Label();
            this.lblContent = new System.Windows.Forms.Label();
            this.pnlTitleBar.SuspendLayout();
            this.SuspendLayout();
            //
            // pnlTitleBar
            //
            this.pnlTitleBar.BackColor = System.Drawing.Color.FromArgb(24, 144, 220);
            this.pnlTitleBar.Controls.Add(this.lblTitleClose);
            this.pnlTitleBar.Controls.Add(this.lblTitleText);
            this.pnlTitleBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTitleBar.Location = new System.Drawing.Point(0, 0);
            this.pnlTitleBar.Name = "pnlTitleBar";
            this.pnlTitleBar.Size = new System.Drawing.Size(400, 34);
            this.pnlTitleBar.TabIndex = 0;
            //
            // lblTitleText
            //
            this.lblTitleText.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTitleText.ForeColor = System.Drawing.Color.White;
            this.lblTitleText.Location = new System.Drawing.Point(12, 6);
            this.lblTitleText.Name = "lblTitleText";
            this.lblTitleText.Size = new System.Drawing.Size(280, 22);
            this.lblTitleText.TabIndex = 0;
            this.lblTitleText.Text = "Basic shortcuts";
            this.lblTitleText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // lblTitleClose
            //
            this.lblTitleClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblTitleClose.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblTitleClose.ForeColor = System.Drawing.Color.White;
            this.lblTitleClose.Location = new System.Drawing.Point(368, 4);
            this.lblTitleClose.Name = "lblTitleClose";
            this.lblTitleClose.Size = new System.Drawing.Size(26, 26);
            this.lblTitleClose.TabIndex = 1;
            this.lblTitleClose.Text = "×";
            this.lblTitleClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // lblContent
            //
            this.lblContent.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblContent.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            this.lblContent.Location = new System.Drawing.Point(20, 46);
            this.lblContent.Name = "lblContent";
            this.lblContent.Size = new System.Drawing.Size(360, 310);
            this.lblContent.TabIndex = 1;
            //
            // ShortcutsHelpForm
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(24, 24, 24);
            this.ClientSize = new System.Drawing.Size(400, 370);
            this.Controls.Add(this.lblContent);
            this.Controls.Add(this.pnlTitleBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "ShortcutsHelpForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Basic shortcuts";
            this.TopMost = true;
            this.pnlTitleBar.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel pnlTitleBar;
        private System.Windows.Forms.Label lblTitleText;
        private System.Windows.Forms.Label lblTitleClose;
        private System.Windows.Forms.Label lblContent;
    }
}
