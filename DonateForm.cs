using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace frm_vietnamese_calendar
{
    /// <summary>
    /// Donation prompt shown to the user on exit (unless they checked "Don't show this again")
    /// or opened directly from the right-click context menu.
    /// </summary>
    public partial class DonateForm : Form
    {
        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [DllImport("dwmapi.dll", PreserveSig = false)]
        private static extern void DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int value, int size);

        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;

        public const string DonateUrl = "https://nguyenthanhdev.com/donate";

        protected override void OnHandleCreated(EventArgs e) // L-12: Win11 rounded corners
        {
            base.OnHandleCreated(e);
            try { int p = 2; DwmSetWindowAttribute(Handle, 33, ref p, sizeof(int)); } catch { }
        }

        private static readonly Size CompactSize  = new Size(460, 320);
        private static readonly Size ExpandedSize = new Size(460, 464);

        private readonly bool _showDontShowCheckbox;

        /// <summary>Whether the user checked "Don't show this again".</summary>
        public bool SkipNextTime { get; private set; }

        public DonateForm(bool showDontShowCheckbox = true, bool startExpanded = false)
        {
            _showDontShowCheckbox = showDontShowCheckbox;
            InitializeComponent();
            ClientSize = CompactSize;

            // V11-08: apply bilingual text from AppStrings
            Text               = AppStrings.DonateTitle;
            lblTitleText.Text  = AppStrings.DonateTitle;
            lblHeading.Text    = AppStrings.DonateHeading;
            lblMessage.Text    = AppStrings.DonateMessage;
            lblEncourage.Text  = AppStrings.DonateEncourage;
            lblHint.Text       = AppStrings.DonateHint;
            lnkWebsite.Text    = AppStrings.DonateWebLink;
            chkDontShow.Text   = AppStrings.DonateDontShow;
            btnYes.Text        = AppStrings.DonateYes;
            btnNo.Text         = AppStrings.DonateNo;
            lblThanks.Text     = AppStrings.DonateThanks;

            chkDontShow.Visible = _showDontShowCheckbox;

            pnlTitleBar.MouseDown  += TitleBar_MouseDown;
            lblTitleText.MouseDown += TitleBar_MouseDown;
            lblTitleClose.Click    += LblTitleClose_Click;
            lblTitleClose.MouseEnter += (s, e) => lblTitleClose.BackColor = Color.FromArgb(220, 50, 50);
            lblTitleClose.MouseLeave += (s, e) => lblTitleClose.BackColor = Color.Transparent;

            KeyDown += DonateForm_KeyDown;

            LoadDonationImage(picTcb, "tcb.jpg");
            LoadDonationImage(picMomo, "momo.jpg");

            if (startExpanded)
                ExpandToQrView();
        }

        private void DonateForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                CloseDialog();
            }
        }

        private void TitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, (IntPtr)HT_CAPTION, IntPtr.Zero);
            }
        }

        private void LblTitleClose_Click(object sender, EventArgs e)
        {
            CloseDialog();
        }

        private void lnkWebsite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try { Process.Start(DonateUrl); } catch { }
        }

        private void LoadDonationImage(PictureBox target, string fileName)
        {
            try
            {
                string resourceName = typeof(DonateForm).Namespace + "." + fileName;
                using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
                {
                    if (stream != null)
                    {
                        using (Image temp = Image.FromStream(stream))
                        {
                            target.Image = new Bitmap(temp);
                        }
                    }
                }
            }
            catch
            {
            }
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            ExpandToQrView();
        }

        private void ExpandToQrView()
        {
            lblMessage.Visible   = false;
            lblEncourage.Visible = false;
            lblHint.Visible      = false;
            btnYes.Visible       = false;

            pnlQrTcb.Visible  = true;
            pnlQrMomo.Visible = true;
            lblThanks.Visible = true;

            // Website link centered below thanks note
            lnkWebsite.Location  = new Point(22, 378);
            lnkWebsite.TextAlign = ContentAlignment.MiddleCenter;

            if (_showDontShowCheckbox)
            {
                chkDontShow.Location = new Point(24, 416);
                chkDontShow.Visible  = true;
                btnNo.Text           = AppStrings.DonateClose;
                btnNo.Size           = new Size(140, 34);
                btnNo.Location       = new Point(298, 410);
            }
            else
            {
                chkDontShow.Visible = false;
                btnNo.Text          = AppStrings.DonateClose;
                btnNo.Size          = new Size(160, 34);
                btnNo.Location      = new Point(150, 410);
            }

            ClientSize = ExpandedSize;
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            CloseDialog();
        }

        private void CloseDialog()
        {
            SkipNextTime = chkDontShow.Checked;
            Close();
        }
    }
}
