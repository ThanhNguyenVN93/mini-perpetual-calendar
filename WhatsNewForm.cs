using System;
using System.Drawing;
using System.Windows.Forms;

namespace frm_vietnamese_calendar
{
    internal class WhatsNewForm : Form
    {
        public bool WillInstall  { get; private set; }
        public bool SkipVersion  { get; private set; }

        public WhatsNewForm(string tag, string notes)
        {
            bool vi = AppStrings.IsVietnamese;

            Text            = "MiniPerpetualCalendar";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            ClientSize      = new Size(460, 275);
            StartPosition   = FormStartPosition.CenterScreen;
            MaximizeBox     = false;
            MinimizeBox     = false;
            TopMost         = true;

            // ---- header panel ----
            var pnlHeader = new Panel
            {
                Dock      = DockStyle.Top,
                Height    = 58,
                BackColor = Color.FromArgb(0, 102, 204),
            };

            var lblTitle = new Label
            {
                Text      = (vi ? "Phiên bản mới: " : "New version: ") + tag,
                Font      = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = Color.White,
                Location  = new Point(12, 8),
                Size      = new Size(436, 26),
                BackColor = Color.Transparent,
            };

            var lblSub = new Label
            {
                Text      = (vi ? "Hiện tại: v" : "Current: v") + UpdateChecker.CurrentVersion,
                Font      = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(200, 230, 255),
                Location  = new Point(14, 36),
                Size      = new Size(300, 18),
                BackColor = Color.Transparent,
            };

            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblSub);

            // ---- notes label ----
            var lblWhat = new Label
            {
                Text     = vi ? "Có gì mới:" : "What's new:",
                Font     = new Font("Segoe UI", 9F, FontStyle.Bold),
                Location = new Point(12, 68),
                Size     = new Size(436, 18),
            };

            // ---- notes text ----
            var txtNotes = new RichTextBox
            {
                Text        = string.IsNullOrWhiteSpace(notes)
                                  ? (vi ? "(Không có thông tin)" : "(No release notes)")
                                  : notes,
                ReadOnly    = true,
                Location    = new Point(12, 88),
                Size        = new Size(436, 128),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor   = Color.FromArgb(248, 248, 248),
                ScrollBars  = RichTextBoxScrollBars.Vertical,
                Font        = new Font("Segoe UI", 9F),
            };

            // ---- buttons ----
            var btnUpdate = new Button
            {
                Text      = vi ? "Cập nhật ngay" : "Update Now",
                Location  = new Point(12, 231),
                Size      = new Size(130, 32),
                BackColor = Color.FromArgb(0, 102, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
            };
            btnUpdate.FlatAppearance.BorderSize = 0;
            btnUpdate.Click += (s, e) => { WillInstall = true; Close(); };

            var btnLater = new Button
            {
                Text     = vi ? "Nhắc tôi sau" : "Remind Later",
                Location = new Point(155, 231),
                Size     = new Size(130, 32),
            };
            btnLater.Click += (s, e) => Close();

            var btnSkip = new Button
            {
                Text      = vi ? "Bỏ phiên bản này" : "Skip This Version",
                Location  = new Point(298, 231),
                Size      = new Size(150, 32),
                ForeColor = Color.Gray,
            };
            btnSkip.Click += (s, e) => { SkipVersion = true; Close(); };

            AcceptButton = btnUpdate;
            CancelButton = btnLater;

            Controls.Add(pnlHeader);
            Controls.Add(lblWhat);
            Controls.Add(txtNotes);
            Controls.Add(btnUpdate);
            Controls.Add(btnLater);
            Controls.Add(btnSkip);
        }
    }
}
