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
                // PictureBox.Dispose() does not free its Image, so we do it manually
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
            this.chkDontShow = new System.Windows.Forms.CheckBox();
            this.btnYes = new System.Windows.Forms.Button();
            this.btnNo = new System.Windows.Forms.Button();
            this.picTcb = new System.Windows.Forms.PictureBox();
            this.picMomo = new System.Windows.Forms.PictureBox();
            this.lblTcb = new System.Windows.Forms.Label();
            this.lblMomo = new System.Windows.Forms.Label();
            this.lblThanks = new System.Windows.Forms.Label();
            this.pnlTitleBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picTcb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMomo)).BeginInit();
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
            this.pnlTitleBar.Size = new System.Drawing.Size(420, 34);
            this.pnlTitleBar.TabIndex = 0;
            //
            // lblTitleText
            //
            this.lblTitleText.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTitleText.ForeColor = System.Drawing.Color.White;
            this.lblTitleText.Location = new System.Drawing.Point(12, 6);
            this.lblTitleText.Name = "lblTitleText";
            this.lblTitleText.Size = new System.Drawing.Size(300, 22);
            this.lblTitleText.TabIndex = 0;
            this.lblTitleText.Text = "Support the author";
            this.lblTitleText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // lblTitleClose
            //
            this.lblTitleClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblTitleClose.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblTitleClose.ForeColor = System.Drawing.Color.White;
            this.lblTitleClose.Location = new System.Drawing.Point(388, 4);
            this.lblTitleClose.Name = "lblTitleClose";
            this.lblTitleClose.Size = new System.Drawing.Size(26, 26);
            this.lblTitleClose.TabIndex = 1;
            this.lblTitleClose.Text = "×";
            this.lblTitleClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // pnlBottomBar
            //
            this.pnlBottomBar.BackColor = System.Drawing.Color.FromArgb(24, 144, 220);
            this.pnlBottomBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottomBar.Location = new System.Drawing.Point(0, 266);
            this.pnlBottomBar.Name = "pnlBottomBar";
            this.pnlBottomBar.Size = new System.Drawing.Size(420, 4);
            this.pnlBottomBar.TabIndex = 1;
            //
            // lblHeart
            //
            this.lblHeart.Font = new System.Drawing.Font("Segoe UI", 20F);
            this.lblHeart.ForeColor = System.Drawing.Color.FromArgb(220, 60, 60);
            this.lblHeart.Location = new System.Drawing.Point(20, 48);
            this.lblHeart.Name = "lblHeart";
            this.lblHeart.Size = new System.Drawing.Size(46, 46);
            this.lblHeart.TabIndex = 2;
            this.lblHeart.Text = "❤";
            this.lblHeart.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // lblHeading
            //
            this.lblHeading.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblHeading.ForeColor = System.Drawing.Color.White;
            this.lblHeading.Location = new System.Drawing.Point(74, 50);
            this.lblHeading.Name = "lblHeading";
            this.lblHeading.Size = new System.Drawing.Size(330, 44);
            this.lblHeading.TabIndex = 3;
            this.lblHeading.Text = "Thank you for using Mini Perpetual Calendar!";
            //
            // lblMessage
            //
            this.lblMessage.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblMessage.ForeColor = System.Drawing.Color.FromArgb(200, 200, 200);
            this.lblMessage.Location = new System.Drawing.Point(20, 100);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(380, 50);
            this.lblMessage.TabIndex = 4;
            this.lblMessage.Text = "This app is developed for free, non-profit. If you find it useful, you can buy" +
    " me a coffee for extra motivation to keep maintaining and improving it.";
            //
            // lblEncourage
            //
            this.lblEncourage.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblEncourage.ForeColor = System.Drawing.Color.White;
            this.lblEncourage.Location = new System.Drawing.Point(20, 156);
            this.lblEncourage.Name = "lblEncourage";
            this.lblEncourage.Size = new System.Drawing.Size(380, 20);
            this.lblEncourage.TabIndex = 5;
            this.lblEncourage.Text = "Every bit of support means a lot!";
            //
            // lblHint
            //
            this.lblHint.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblHint.ForeColor = System.Drawing.Color.FromArgb(180, 180, 180);
            this.lblHint.Location = new System.Drawing.Point(20, 180);
            this.lblHint.Name = "lblHint";
            this.lblHint.Size = new System.Drawing.Size(380, 18);
            this.lblHint.TabIndex = 6;
            this.lblHint.Text = "Click \'Yes\' to see the donation QR codes.";
            //
            // chkDontShow
            //
            this.chkDontShow.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chkDontShow.ForeColor = System.Drawing.Color.FromArgb(160, 160, 160);
            this.chkDontShow.Location = new System.Drawing.Point(20, 204);
            this.chkDontShow.Name = "chkDontShow";
            this.chkDontShow.Size = new System.Drawing.Size(220, 22);
            this.chkDontShow.TabIndex = 7;
            this.chkDontShow.Text = "Don't show this again";
            //
            // btnYes
            //
            this.btnYes.BackColor = System.Drawing.Color.FromArgb(0, 209, 236);
            this.btnYes.FlatAppearance.BorderSize = 0;
            this.btnYes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnYes.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnYes.ForeColor = System.Drawing.Color.FromArgb(20, 20, 20);
            this.btnYes.Location = new System.Drawing.Point(20, 234);
            this.btnYes.Name = "btnYes";
            this.btnYes.Size = new System.Drawing.Size(178, 36);
            this.btnYes.TabIndex = 8;
            this.btnYes.Text = "Yes";
            this.btnYes.UseVisualStyleBackColor = false;
            this.btnYes.Click += new System.EventHandler(this.btnYes_Click);
            //
            // btnNo
            //
            this.btnNo.BackColor = System.Drawing.Color.FromArgb(58, 58, 58);
            this.btnNo.FlatAppearance.BorderSize = 0;
            this.btnNo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNo.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnNo.ForeColor = System.Drawing.Color.FromArgb(210, 210, 210);
            this.btnNo.Location = new System.Drawing.Point(222, 234);
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(178, 36);
            this.btnNo.TabIndex = 9;
            this.btnNo.Text = "No";
            this.btnNo.UseVisualStyleBackColor = false;
            this.btnNo.Click += new System.EventHandler(this.btnNo_Click);
            //
            // picTcb
            //
            this.picTcb.BackColor = System.Drawing.Color.White;
            this.picTcb.Location = new System.Drawing.Point(30, 100);
            this.picTcb.Name = "picTcb";
            this.picTcb.Size = new System.Drawing.Size(150, 150);
            this.picTcb.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picTcb.TabIndex = 10;
            this.picTcb.TabStop = false;
            this.picTcb.Visible = false;
            //
            // picMomo
            //
            this.picMomo.BackColor = System.Drawing.Color.White;
            this.picMomo.Location = new System.Drawing.Point(240, 100);
            this.picMomo.Name = "picMomo";
            this.picMomo.Size = new System.Drawing.Size(150, 150);
            this.picMomo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picMomo.TabIndex = 11;
            this.picMomo.TabStop = false;
            this.picMomo.Visible = false;
            //
            // lblTcb
            //
            this.lblTcb.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblTcb.ForeColor = System.Drawing.Color.White;
            this.lblTcb.Location = new System.Drawing.Point(30, 254);
            this.lblTcb.Name = "lblTcb";
            this.lblTcb.Size = new System.Drawing.Size(150, 20);
            this.lblTcb.TabIndex = 12;
            this.lblTcb.Text = "Techcombank";
            this.lblTcb.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblTcb.Visible = false;
            //
            // lblMomo
            //
            this.lblMomo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblMomo.ForeColor = System.Drawing.Color.White;
            this.lblMomo.Location = new System.Drawing.Point(240, 254);
            this.lblMomo.Name = "lblMomo";
            this.lblMomo.Size = new System.Drawing.Size(150, 20);
            this.lblMomo.TabIndex = 13;
            this.lblMomo.Text = "MoMo";
            this.lblMomo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblMomo.Visible = false;
            //
            // lblThanks
            //
            this.lblThanks.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this.lblThanks.ForeColor = System.Drawing.Color.FromArgb(180, 180, 180);
            this.lblThanks.Location = new System.Drawing.Point(20, 282);
            this.lblThanks.Name = "lblThanks";
            this.lblThanks.Size = new System.Drawing.Size(380, 36);
            this.lblThanks.TabIndex = 14;
            this.lblThanks.Text = "Thank you so much! Close this window to continue.";
            this.lblThanks.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblThanks.Visible = false;
            //
            // DonateForm
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(24, 24, 24);
            this.ClientSize = new System.Drawing.Size(420, 270);
            this.Controls.Add(this.lblThanks);
            this.Controls.Add(this.lblMomo);
            this.Controls.Add(this.lblTcb);
            this.Controls.Add(this.picMomo);
            this.Controls.Add(this.picTcb);
            this.Controls.Add(this.btnNo);
            this.Controls.Add(this.btnYes);
            this.Controls.Add(this.chkDontShow);
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
        private System.Windows.Forms.CheckBox chkDontShow;
        private System.Windows.Forms.Button btnYes;
        private System.Windows.Forms.Button btnNo;
        private System.Windows.Forms.PictureBox picTcb;
        private System.Windows.Forms.PictureBox picMomo;
        private System.Windows.Forms.Label lblTcb;
        private System.Windows.Forms.Label lblMomo;
        private System.Windows.Forms.Label lblThanks;
    }
}
