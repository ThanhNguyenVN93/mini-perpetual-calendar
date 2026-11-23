namespace frm_vietnamese_calendar
{
    partial class HotkeySettingsForm
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
            this.lblToggleCaption = new System.Windows.Forms.Label();
            this.lblToggleValue = new System.Windows.Forms.Label();
            this.lblMonthCaption = new System.Windows.Forms.Label();
            this.lblMonthValue = new System.Windows.Forms.Label();
            this.lblHint = new System.Windows.Forms.Label();
            this.lblError = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
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
            this.pnlTitleBar.Size = new System.Drawing.Size(360, 34);
            this.pnlTitleBar.TabIndex = 0;
            //
            // lblTitleText
            //
            this.lblTitleText.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTitleText.ForeColor = System.Drawing.Color.White;
            this.lblTitleText.Location = new System.Drawing.Point(12, 6);
            this.lblTitleText.Name = "lblTitleText";
            this.lblTitleText.Size = new System.Drawing.Size(240, 22);
            this.lblTitleText.TabIndex = 0;
            this.lblTitleText.Text = "Change hotkeys";
            this.lblTitleText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // lblTitleClose
            //
            this.lblTitleClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblTitleClose.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblTitleClose.ForeColor = System.Drawing.Color.White;
            this.lblTitleClose.Location = new System.Drawing.Point(328, 4);
            this.lblTitleClose.Name = "lblTitleClose";
            this.lblTitleClose.Size = new System.Drawing.Size(26, 26);
            this.lblTitleClose.TabIndex = 1;
            this.lblTitleClose.Text = "×";
            this.lblTitleClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // lblToggleCaption
            //
            this.lblToggleCaption.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblToggleCaption.ForeColor = System.Drawing.Color.White;
            this.lblToggleCaption.Location = new System.Drawing.Point(20, 50);
            this.lblToggleCaption.Name = "lblToggleCaption";
            this.lblToggleCaption.Size = new System.Drawing.Size(140, 26);
            this.lblToggleCaption.TabIndex = 1;
            this.lblToggleCaption.Text = "Show/hide window:";
            this.lblToggleCaption.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // lblToggleValue
            //
            this.lblToggleValue.BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
            this.lblToggleValue.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblToggleValue.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblToggleValue.ForeColor = System.Drawing.Color.FromArgb(0, 209, 236);
            this.lblToggleValue.Location = new System.Drawing.Point(168, 46);
            this.lblToggleValue.Name = "lblToggleValue";
            this.lblToggleValue.Size = new System.Drawing.Size(172, 30);
            this.lblToggleValue.TabIndex = 2;
            this.lblToggleValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // lblMonthCaption
            //
            this.lblMonthCaption.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblMonthCaption.ForeColor = System.Drawing.Color.White;
            this.lblMonthCaption.Location = new System.Drawing.Point(20, 90);
            this.lblMonthCaption.Name = "lblMonthCaption";
            this.lblMonthCaption.Size = new System.Drawing.Size(140, 26);
            this.lblMonthCaption.TabIndex = 3;
            this.lblMonthCaption.Text = "Open month calendar:";
            this.lblMonthCaption.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // lblMonthValue
            //
            this.lblMonthValue.BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
            this.lblMonthValue.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblMonthValue.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblMonthValue.ForeColor = System.Drawing.Color.FromArgb(0, 209, 236);
            this.lblMonthValue.Location = new System.Drawing.Point(168, 86);
            this.lblMonthValue.Name = "lblMonthValue";
            this.lblMonthValue.Size = new System.Drawing.Size(172, 30);
            this.lblMonthValue.TabIndex = 4;
            this.lblMonthValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // lblHint
            //
            this.lblHint.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Italic);
            this.lblHint.ForeColor = System.Drawing.Color.FromArgb(160, 160, 160);
            this.lblHint.Location = new System.Drawing.Point(20, 126);
            this.lblHint.Name = "lblHint";
            this.lblHint.Size = new System.Drawing.Size(320, 34);
            this.lblHint.TabIndex = 5;
            this.lblHint.Text = "Click a field then press the new key combo. Requires Ctrl/Alt/Shift plus 1 " +
                "other key. Press Esc to cancel.";
            //
            // lblError
            //
            this.lblError.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblError.ForeColor = System.Drawing.Color.FromArgb(255, 90, 90);
            this.lblError.Location = new System.Drawing.Point(20, 162);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(320, 34);
            this.lblError.TabIndex = 6;
            this.lblError.Visible = false;
            //
            // btnSave
            //
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(0, 209, 236);
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.FromArgb(20, 20, 20);
            this.btnSave.Location = new System.Drawing.Point(90, 202);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 34);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            //
            // btnCancel
            //
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(58, 58, 58);
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.btnCancel.ForeColor = System.Drawing.Color.FromArgb(210, 210, 210);
            this.btnCancel.Location = new System.Drawing.Point(190, 202);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 34);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            //
            // HotkeySettingsForm
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(24, 24, 24);
            this.ClientSize = new System.Drawing.Size(360, 250);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.lblHint);
            this.Controls.Add(this.lblMonthValue);
            this.Controls.Add(this.lblMonthCaption);
            this.Controls.Add(this.lblToggleValue);
            this.Controls.Add(this.lblToggleCaption);
            this.Controls.Add(this.pnlTitleBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "HotkeySettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Change hotkeys";
            this.TopMost = true;
            this.pnlTitleBar.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel pnlTitleBar;
        private System.Windows.Forms.Label lblTitleText;
        private System.Windows.Forms.Label lblTitleClose;
        private System.Windows.Forms.Label lblToggleCaption;
        private System.Windows.Forms.Label lblToggleValue;
        private System.Windows.Forms.Label lblMonthCaption;
        private System.Windows.Forms.Label lblMonthValue;
        private System.Windows.Forms.Label lblHint;
        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
    }
}
