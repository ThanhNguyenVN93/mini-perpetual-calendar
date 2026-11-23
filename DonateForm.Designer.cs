namespace frm_vietnamese_calendar
{
    partial class DonateForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
                picTcb?.Image?.Dispose();
                picMomo?.Image?.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlTitleBar = new System.Windows.Forms.Panel();
            this.lblTitleClose = new System.Windows.Forms.Label();
            this.lblTitleText = new System.Windows.Forms.Label();
            this.pnlBottomBar = new System.Windows.Forms.Panel();
            this.lblHeart = new System.Windows.Forms.Label();
            this.lblHeading = new System.Windows.Forms.Label();
            this.lblMessage = new System.Windows.Forms.Label();
            this.lblEncourage = new System.Windows.Forms.Label();
            this.lblHint = new System.Windows.Forms.Label();
            this.lnkWebsite = new System.Windows.Forms.LinkLabel();
            this.chkDontShow = new System.Windows.Forms.CheckBox();
            this.btnYes = new System.Windows.Forms.Button();
            this.btnNo = new System.Windows.Forms.Button();
            this.pnlQrTcb = new System.Windows.Forms.Panel();
            this.picTcb = new System.Windows.Forms.PictureBox();
            this.lblTcb = new System.Windows.Forms.Label();
            this.pnlQrMomo = new System.Windows.Forms.Panel();
            this.picMomo = new System.Windows.Forms.PictureBox();
            this.lblMomo = new System.Windows.Forms.Label();
            this.lblThanks = new System.Windows.Forms.Label();
            this.pnlTitleBar.SuspendLayout();
            this.pnlQrTcb.SuspendLayout();
            this.pnlQrMomo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picTcb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMomo)).BeginInit();
            this.SuspendLayout();
            //
            // pnlTitleBar
            //
            this.pnlTitleBar.BackColor = System.Drawing.Color.FromArgb(18, 128, 204);
            this.pnlTitleBar.Controls.Add(this.lblTitleClose);
            this.pnlTitleBar.Controls.Add(this.lblTitleText);
            this.pnlTitleBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTitleBar.Location = new System.Drawing.Point(0, 0);
            this.pnlTitleBar.Name = "pnlTitleBar";
            this.pnlTitleBar.Size = new System.Drawing.Size(460, 38);
            this.pnlTitleBar.TabIndex = 0;
            //
            // lblTitleText
            //
            this.lblTitleText.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.lblTitleText.ForeColor = System.Drawing.Color.White;
            this.lblTitleText.Location = new System.Drawing.Point(14, 8);
            this.lblTitleText.Name = "lblTitleText";
            this.lblTitleText.Size = new System.Drawing.Size(340, 22);
            this.lblTitleText.TabIndex = 0;
            this.lblTitleText.Text = "Support the author";
            this.lblTitleText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // lblTitleClose
            //
            this.lblTitleClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblTitleClose.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitleClose.ForeColor = System.Drawing.Color.White;
            this.lblTitleClose.Location = new System.Drawing.Point(424, 5);
            this.lblTitleClose.Name = "lblTitleClose";
            this.lblTitleClose.Size = new System.Drawing.Size(28, 28);
            this.lblTitleClose.TabIndex = 1;
            this.lblTitleClose.Text = "×";
            this.lblTitleClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // pnlBottomBar
            //
            this.pnlBottomBar.BackColor = System.Drawing.Color.FromArgb(18, 128, 204);
            this.pnlBottomBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottomBar.Location = new System.Drawing.Point(0, 316);
            this.pnlBottomBar.Name = "pnlBottomBar";
            this.pnlBottomBar.Size = new System.Drawing.Size(460, 4);
            this.pnlBottomBar.TabIndex = 1;
            //
            // lblHeart
            //
            this.lblHeart.Font = new System.Drawing.Font("Segoe UI", 22F);
            this.lblHeart.ForeColor = System.Drawing.Color.FromArgb(245, 75, 85);
            this.lblHeart.Location = new System.Drawing.Point(18, 50);
            this.lblHeart.Name = "lblHeart";
            this.lblHeart.Size = new System.Drawing.Size(48, 48);
            this.lblHeart.TabIndex = 2;
            this.lblHeart.Text = "❤";
            this.lblHeart.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // lblHeading
            //
            this.lblHeading.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblHeading.ForeColor = System.Drawing.Color.White;
            this.lblHeading.Location = new System.Drawing.Point(72, 50);
            this.lblHeading.Name = "lblHeading";
            this.lblHeading.Size = new System.Drawing.Size(366, 48);
            this.lblHeading.TabIndex = 3;
            this.lblHeading.Text = "Thank you for using Mini Perpetual Calendar!";
            this.lblHeading.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // lblMessage
            //
            this.lblMessage.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblMessage.ForeColor = System.Drawing.Color.FromArgb(215, 215, 215);
            this.lblMessage.Location = new System.Drawing.Point(22, 106);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(416, 46);
            this.lblMessage.TabIndex = 4;
            this.lblMessage.Text = "This app is developed for free, non-profit. If you find it useful, you can buy me a coffee for extra motivation to keep maintaining and improving it.";
            //
            // lblEncourage
            //
            this.lblEncourage.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblEncourage.ForeColor = System.Drawing.Color.FromArgb(255, 215, 110);
            this.lblEncourage.Location = new System.Drawing.Point(22, 156);
            this.lblEncourage.Name = "lblEncourage";
            this.lblEncourage.Size = new System.Drawing.Size(416, 22);
            this.lblEncourage.TabIndex = 5;
            this.lblEncourage.Text = "Every bit of support means a lot!";
            //
            // lblHint
            //
            this.lblHint.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblHint.ForeColor = System.Drawing.Color.FromArgb(175, 175, 175);
            this.lblHint.Location = new System.Drawing.Point(22, 180);
            this.lblHint.Name = "lblHint";
            this.lblHint.Size = new System.Drawing.Size(416, 20);
            this.lblHint.TabIndex = 6;
            this.lblHint.Text = "Click 'Yes, show QR' or open the online donation page.";
            //
            // lnkWebsite
            //
            this.lnkWebsite.ActiveLinkColor = System.Drawing.Color.FromArgb(120, 235, 255);
            this.lnkWebsite.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lnkWebsite.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lnkWebsite.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkWebsite.LinkColor = System.Drawing.Color.FromArgb(0, 209, 236);
            this.lnkWebsite.Location = new System.Drawing.Point(22, 205);
            this.lnkWebsite.Name = "lnkWebsite";
            this.lnkWebsite.Size = new System.Drawing.Size(416, 22);
            this.lnkWebsite.TabIndex = 7;
            this.lnkWebsite.TabStop = true;
            this.lnkWebsite.Text = "🌐  nguyenthanhdev.com/donate";
            this.lnkWebsite.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lnkWebsite.VisitedLinkColor = System.Drawing.Color.FromArgb(0, 209, 236);
            this.lnkWebsite.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkWebsite_LinkClicked);
            //
            // chkDontShow
            //
            this.chkDontShow.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkDontShow.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chkDontShow.ForeColor = System.Drawing.Color.FromArgb(170, 170, 170);
            this.chkDontShow.Location = new System.Drawing.Point(24, 234);
            this.chkDontShow.Name = "chkDontShow";
            this.chkDontShow.Size = new System.Drawing.Size(260, 22);
            this.chkDontShow.TabIndex = 8;
            this.chkDontShow.Text = "Don't show again on exit";
            //
            // btnYes
            //
            this.btnYes.BackColor = System.Drawing.Color.FromArgb(0, 209, 236);
            this.btnYes.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnYes.FlatAppearance.BorderSize = 0;
            this.btnYes.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(40, 225, 250);
            this.btnYes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnYes.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnYes.ForeColor = System.Drawing.Color.FromArgb(18, 18, 18);
            this.btnYes.Location = new System.Drawing.Point(22, 266);
            this.btnYes.Name = "btnYes";
            this.btnYes.Size = new System.Drawing.Size(200, 38);
            this.btnYes.TabIndex = 9;
            this.btnYes.Text = "☕  Yes, show QR";
            this.btnYes.UseVisualStyleBackColor = false;
            this.btnYes.Click += new System.EventHandler(this.btnYes_Click);
            //
            // btnNo
            //
            this.btnNo.BackColor = System.Drawing.Color.FromArgb(52, 54, 60);
            this.btnNo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNo.FlatAppearance.BorderSize = 0;
            this.btnNo.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(72, 75, 82);
            this.btnNo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNo.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnNo.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            this.btnNo.Location = new System.Drawing.Point(238, 266);
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(200, 38);
            this.btnNo.TabIndex = 10;
            this.btnNo.Text = "Maybe later";
            this.btnNo.UseVisualStyleBackColor = false;
            this.btnNo.Click += new System.EventHandler(this.btnNo_Click);
            //
            // pnlQrTcb
            //
            this.pnlQrTcb.BackColor = System.Drawing.Color.FromArgb(36, 38, 44);
            this.pnlQrTcb.Controls.Add(this.picTcb);
            this.pnlQrTcb.Controls.Add(this.lblTcb);
            this.pnlQrTcb.Location = new System.Drawing.Point(22, 106);
            this.pnlQrTcb.Name = "pnlQrTcb";
            this.pnlQrTcb.Size = new System.Drawing.Size(200, 240);
            this.pnlQrTcb.TabIndex = 11;
            this.pnlQrTcb.Visible = false;
            //
            // picTcb
            //
            this.picTcb.BackColor = System.Drawing.Color.White;
            this.picTcb.Location = new System.Drawing.Point(10, 10);
            this.picTcb.Name = "picTcb";
            this.picTcb.Size = new System.Drawing.Size(180, 196);
            this.picTcb.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picTcb.TabIndex = 0;
            this.picTcb.TabStop = false;
            //
            // lblTcb
            //
            this.lblTcb.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblTcb.ForeColor = System.Drawing.Color.FromArgb(255, 110, 110);
            this.lblTcb.Location = new System.Drawing.Point(6, 210);
            this.lblTcb.Name = "lblTcb";
            this.lblTcb.Size = new System.Drawing.Size(188, 24);
            this.lblTcb.TabIndex = 1;
            this.lblTcb.Text = "Techcombank";
            this.lblTcb.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // pnlQrMomo
            //
            this.pnlQrMomo.BackColor = System.Drawing.Color.FromArgb(36, 38, 44);
            this.pnlQrMomo.Controls.Add(this.picMomo);
            this.pnlQrMomo.Controls.Add(this.lblMomo);
            this.pnlQrMomo.Location = new System.Drawing.Point(238, 106);
            this.pnlQrMomo.Name = "pnlQrMomo";
            this.pnlQrMomo.Size = new System.Drawing.Size(200, 240);
            this.pnlQrMomo.TabIndex = 12;
            this.pnlQrMomo.Visible = false;
            //
            // picMomo
            //
            this.picMomo.BackColor = System.Drawing.Color.White;
            this.picMomo.Location = new System.Drawing.Point(10, 10);
            this.picMomo.Name = "picMomo";
            this.picMomo.Size = new System.Drawing.Size(180, 196);
            this.picMomo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picMomo.TabIndex = 0;
            this.picMomo.TabStop = false;
            //
            // lblMomo
            //
            this.lblMomo.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblMomo.ForeColor = System.Drawing.Color.FromArgb(245, 115, 180);
            this.lblMomo.Location = new System.Drawing.Point(6, 210);
            this.lblMomo.Name = "lblMomo";
            this.lblMomo.Size = new System.Drawing.Size(188, 24);
            this.lblMomo.TabIndex = 1;
            this.lblMomo.Text = "MoMo / VietQR";
            this.lblMomo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // lblThanks
            //
            this.lblThanks.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this.lblThanks.ForeColor = System.Drawing.Color.FromArgb(190, 190, 190);
            this.lblThanks.Location = new System.Drawing.Point(22, 354);
            this.lblThanks.Name = "lblThanks";
            this.lblThanks.Size = new System.Drawing.Size(416, 20);
            this.lblThanks.TabIndex = 13;
            this.lblThanks.Text = "Thank you so much! Scan a QR code above or click the website link.";
            this.lblThanks.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblThanks.Visible = false;
            //
            // DonateForm
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(24, 25, 28);
            this.ClientSize = new System.Drawing.Size(460, 320);
            this.Controls.Add(this.lblThanks);
            this.Controls.Add(this.pnlQrMomo);
            this.Controls.Add(this.pnlQrTcb);
            this.Controls.Add(this.btnNo);
            this.Controls.Add(this.btnYes);
            this.Controls.Add(this.chkDontShow);
            this.Controls.Add(this.lnkWebsite);
            this.Controls.Add(this.lblHint);
            this.Controls.Add(this.lblEncourage);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.lblHeading);
            this.Controls.Add(this.lblHeart);
            this.Controls.Add(this.pnlBottomBar);
            this.Controls.Add(this.pnlTitleBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "DonateForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Support the author";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.picTcb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMomo)).EndInit();
            this.pnlQrTcb.ResumeLayout(false);
            this.pnlQrMomo.ResumeLayout(false);
            this.pnlTitleBar.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel pnlTitleBar;
        private System.Windows.Forms.Label lblTitleText;
        private System.Windows.Forms.Label lblTitleClose;
        private System.Windows.Forms.Panel pnlBottomBar;
        private System.Windows.Forms.Label lblHeart;
        private System.Windows.Forms.Label lblHeading;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Label lblEncourage;
        private System.Windows.Forms.Label lblHint;
        private System.Windows.Forms.LinkLabel lnkWebsite;
        private System.Windows.Forms.CheckBox chkDontShow;
        private System.Windows.Forms.Button btnYes;
        private System.Windows.Forms.Button btnNo;
        private System.Windows.Forms.Panel pnlQrTcb;
        private System.Windows.Forms.PictureBox picTcb;
        private System.Windows.Forms.Label lblTcb;
        private System.Windows.Forms.Panel pnlQrMomo;
        private System.Windows.Forms.PictureBox picMomo;
        private System.Windows.Forms.Label lblMomo;
        private System.Windows.Forms.Label lblThanks;
    }
}
