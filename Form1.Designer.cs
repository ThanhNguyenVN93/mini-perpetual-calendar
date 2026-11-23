namespace frm_vietnamese_calendar
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timerRefresh = new System.Windows.Forms.Timer(this.components);
            this.timerBackground = new System.Windows.Forms.Timer(this.components);
            this.contextMenuMain = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuItemMonthView = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemStartWithWindows = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemChangeHotkeys = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemFeedback = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.lblWeekday = new System.Windows.Forms.Label();
            this.lblSolarDate = new System.Windows.Forms.Label();
            this.lblLunarDate = new System.Windows.Forms.Label();
            this.lblClose = new System.Windows.Forms.Label();
            this.lblFeedback = new System.Windows.Forms.Label();
            this.lblHelp = new System.Windows.Forms.Label();
            this.chkStartWithWindows = new System.Windows.Forms.CheckBox();
            this.notifyIconMain = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuMain.SuspendLayout();
            this.SuspendLayout();
            //
            // timerRefresh
            //
            this.timerRefresh.Interval = 60000;
            this.timerRefresh.Tick += new System.EventHandler(this.timerRefresh_Tick);
            //
            // timerBackground
            //
            this.timerBackground.Interval = 45000;
            this.timerBackground.Tick += new System.EventHandler(this.timerBackground_Tick);
            //
            // contextMenuMain
            //
            this.contextMenuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemMonthView,
            this.menuItemStartWithWindows,
            this.menuItemChangeHotkeys,
            this.menuSeparator,
            this.menuItemFeedback,
            this.menuSeparator2,
            this.menuItemExit});
            this.contextMenuMain.Name = "contextMenuMain";
            this.contextMenuMain.Size = new System.Drawing.Size(200, 120);
            //
            // menuItemMonthView
            //
            this.menuItemMonthView.Name = "menuItemMonthView";
            this.menuItemMonthView.Size = new System.Drawing.Size(199, 22);
            this.menuItemMonthView.Text = "View month calendar";
            this.menuItemMonthView.Click += new System.EventHandler(this.menuItemMonthView_Click);
            //
            // menuItemStartWithWindows
            //
            this.menuItemStartWithWindows.CheckOnClick = true;
            this.menuItemStartWithWindows.Name = "menuItemStartWithWindows";
            this.menuItemStartWithWindows.Size = new System.Drawing.Size(199, 22);
            this.menuItemStartWithWindows.Text = "Start with Windows";
            //
            // menuItemChangeHotkeys
            //
            this.menuItemChangeHotkeys.Name = "menuItemChangeHotkeys";
            this.menuItemChangeHotkeys.Size = new System.Drawing.Size(199, 22);
            this.menuItemChangeHotkeys.Text = "Change hotkeys...";
            this.menuItemChangeHotkeys.Click += new System.EventHandler(this.menuItemChangeHotkeys_Click);
            //
            // menuSeparator
            //
            this.menuSeparator.Name = "menuSeparator";
            this.menuSeparator.Size = new System.Drawing.Size(176, 6);
            //
            // menuItemFeedback
            //
            this.menuItemFeedback.Name = "menuItemFeedback";
            this.menuItemFeedback.Size = new System.Drawing.Size(179, 22);
            this.menuItemFeedback.Text = "Feedback";
            this.menuItemFeedback.Click += new System.EventHandler(this.menuItemFeedback_Click);
            //
            // menuSeparator2
            //
            this.menuSeparator2.Name = "menuSeparator2";
            this.menuSeparator2.Size = new System.Drawing.Size(176, 6);
            //
            // menuItemExit
            //
            this.menuItemExit.Name = "menuItemExit";
            this.menuItemExit.Size = new System.Drawing.Size(179, 22);
            this.menuItemExit.Text = "Exit";
            this.menuItemExit.Click += new System.EventHandler(this.menuItemExit_Click);
            //
            // lblWeekday
            //
            this.lblWeekday.BackColor = System.Drawing.Color.Transparent;
            this.lblWeekday.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular);
            this.lblWeekday.ForeColor = System.Drawing.Color.White;
            this.lblWeekday.Location = new System.Drawing.Point(0, 18);
            this.lblWeekday.Name = "lblWeekday";
            this.lblWeekday.Size = new System.Drawing.Size(360, 25);
            this.lblWeekday.TabIndex = 0;
            this.lblWeekday.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // lblSolarDate
            //
            this.lblSolarDate.BackColor = System.Drawing.Color.Transparent;
            this.lblSolarDate.Font = new System.Drawing.Font("Segoe UI", 26F, System.Drawing.FontStyle.Bold);
            this.lblSolarDate.ForeColor = System.Drawing.Color.White;
            this.lblSolarDate.Location = new System.Drawing.Point(0, 50);
            this.lblSolarDate.Name = "lblSolarDate";
            this.lblSolarDate.Size = new System.Drawing.Size(360, 55);
            this.lblSolarDate.TabIndex = 1;
            this.lblSolarDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // lblLunarDate
            //
            this.lblLunarDate.BackColor = System.Drawing.Color.Transparent;
            this.lblLunarDate.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular);
            this.lblLunarDate.ForeColor = System.Drawing.Color.FromArgb(255, 210, 130);
            this.lblLunarDate.Location = new System.Drawing.Point(0, 118); // L-06: moved up to give room for countdown badge
            this.lblLunarDate.Name = "lblLunarDate";
            this.lblLunarDate.Size = new System.Drawing.Size(360, 30);
            this.lblLunarDate.TabIndex = 2;
            this.lblLunarDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // lblClose
            //
            this.lblClose.BackColor = System.Drawing.Color.Transparent;
            this.lblClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblClose.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblClose.Location = new System.Drawing.Point(330, 4);
            this.lblClose.Name = "lblClose";
            this.lblClose.Size = new System.Drawing.Size(26, 24);
            this.lblClose.TabIndex = 3;
            this.lblClose.Text = "×";
            this.lblClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // lblFeedback
            //
            this.lblFeedback.BackColor = System.Drawing.Color.Transparent;
            this.lblFeedback.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblFeedback.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Underline);
            this.lblFeedback.ForeColor = System.Drawing.Color.FromArgb(180, 180, 180);
            this.lblFeedback.Location = new System.Drawing.Point(263, 178);
            this.lblFeedback.Name = "lblFeedback";
            this.lblFeedback.Size = new System.Drawing.Size(90, 18);
            this.lblFeedback.TabIndex = 4;
            this.lblFeedback.Text = "Feedback";
            this.lblFeedback.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // lblHelp
            //
            this.lblHelp.BackColor = System.Drawing.Color.Transparent;
            this.lblHelp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblHelp.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblHelp.ForeColor = System.Drawing.Color.FromArgb(180, 180, 180);
            this.lblHelp.Location = new System.Drawing.Point(220, 178);
            this.lblHelp.Name = "lblHelp";
            this.lblHelp.Size = new System.Drawing.Size(28, 18);
            this.lblHelp.TabIndex = 6;
            this.lblHelp.Text = "?";
            this.lblHelp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // chkStartWithWindows
            //
            this.chkStartWithWindows.AutoSize = false;
            this.chkStartWithWindows.BackColor = System.Drawing.Color.Transparent;
            this.chkStartWithWindows.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkStartWithWindows.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular);
            this.chkStartWithWindows.ForeColor = System.Drawing.Color.FromArgb(180, 180, 180);
            this.chkStartWithWindows.Location = new System.Drawing.Point(6, 178);
            this.chkStartWithWindows.Name = "chkStartWithWindows";
            this.chkStartWithWindows.Size = new System.Drawing.Size(130, 18);
            this.chkStartWithWindows.TabIndex = 5;
            this.chkStartWithWindows.Text = "Start with Windows";
            this.chkStartWithWindows.UseVisualStyleBackColor = false;
            //
            // notifyIconMain
            //
            this.notifyIconMain.ContextMenuStrip = this.contextMenuMain;
            this.notifyIconMain.Text = "Mini Perpetual Calendar";
            this.notifyIconMain.Visible = true;
            //
            // Form1
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(45, 45, 45);
            this.ClientSize = new System.Drawing.Size(360, 200);
            this.Controls.Add(this.lblClose);
            this.Controls.Add(this.lblFeedback);
            this.Controls.Add(this.lblHelp);
            this.Controls.Add(this.chkStartWithWindows);
            this.Controls.Add(this.lblWeekday);
            this.Controls.Add(this.lblSolarDate);
            this.Controls.Add(this.lblLunarDate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "Form1";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Mini Perpetual Calendar";
            this.TopMost = true;
            this.contextMenuMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timerRefresh;
        private System.Windows.Forms.Timer timerBackground;
        private System.Windows.Forms.ContextMenuStrip contextMenuMain;
        private System.Windows.Forms.ToolStripMenuItem menuItemMonthView;
        private System.Windows.Forms.ToolStripMenuItem menuItemStartWithWindows;
        private System.Windows.Forms.ToolStripMenuItem menuItemChangeHotkeys;
        private System.Windows.Forms.ToolStripSeparator menuSeparator;
        private System.Windows.Forms.ToolStripMenuItem menuItemFeedback;
        private System.Windows.Forms.ToolStripSeparator menuSeparator2;
        private System.Windows.Forms.ToolStripMenuItem menuItemExit;
        private System.Windows.Forms.Label lblWeekday;
        private System.Windows.Forms.Label lblSolarDate;
        private System.Windows.Forms.Label lblLunarDate;
        private System.Windows.Forms.Label lblClose;
        private System.Windows.Forms.Label lblFeedback;
        private System.Windows.Forms.Label lblHelp;
        private System.Windows.Forms.CheckBox chkStartWithWindows;
        private System.Windows.Forms.NotifyIcon notifyIconMain;
    }
}
