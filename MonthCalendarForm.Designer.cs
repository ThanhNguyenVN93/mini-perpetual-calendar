namespace frm_vietnamese_calendar
{
    partial class MonthCalendarForm
    {
        private System.ComponentModel.IContainer components = null;

        // Dispose is implemented in MonthCalendarForm.cs to also release shared Font instances

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnToday = new System.Windows.Forms.Button();
            this.lblMonthYear = new System.Windows.Forms.Label();
            this.btnPrev = new System.Windows.Forms.Button();
            this.pnlGrid = new System.Windows.Forms.Panel();
            this.pnlHeader.SuspendLayout();
            this.SuspendLayout();
            //
            // pnlHeader
            //
            this.pnlHeader.Controls.Add(this.btnNext);
            this.pnlHeader.Controls.Add(this.btnToday);
            this.pnlHeader.Controls.Add(this.lblMonthYear);
            this.pnlHeader.Controls.Add(this.btnPrev);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(560, 44);
            this.pnlHeader.TabIndex = 0;
            //
            // btnPrev
            //
            this.btnPrev.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnPrev.Location = new System.Drawing.Point(10, 8);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(30, 28);
            this.btnPrev.TabIndex = 0;
            this.btnPrev.Text = "<";
            this.btnPrev.UseVisualStyleBackColor = true;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            //
            // lblMonthYear
            //
            this.lblMonthYear.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblMonthYear.Location = new System.Drawing.Point(150, 8);
            this.lblMonthYear.Name = "lblMonthYear";
            this.lblMonthYear.Size = new System.Drawing.Size(260, 28);
            this.lblMonthYear.TabIndex = 1;
            this.lblMonthYear.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // btnToday
            //
            this.btnToday.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnToday.Location = new System.Drawing.Point(420, 8);
            this.btnToday.Name = "btnToday";
            this.btnToday.Size = new System.Drawing.Size(70, 28);
            this.btnToday.TabIndex = 2;
            this.btnToday.Text = "Today";
            this.btnToday.UseVisualStyleBackColor = true;
            this.btnToday.Click += new System.EventHandler(this.btnToday_Click);
            //
            // btnNext
            //
            this.btnNext.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnNext.Location = new System.Drawing.Point(500, 8);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(30, 28);
            this.btnNext.TabIndex = 3;
            this.btnNext.Text = ">";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            //
            // pnlGrid
            //
            this.pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGrid.Location = new System.Drawing.Point(0, 44);
            this.pnlGrid.Name = "pnlGrid";
            this.pnlGrid.Size = new System.Drawing.Size(560, 436);
            this.pnlGrid.TabIndex = 1;
            //
            // MonthCalendarForm
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(560, 480);
            this.Controls.Add(this.pnlGrid);
            this.Controls.Add(this.pnlHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MonthCalendarForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Month calendar";
            this.TopMost = true;
            this.pnlHeader.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Label lblMonthYear;
        private System.Windows.Forms.Button btnToday;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Panel pnlGrid;
    }
}
