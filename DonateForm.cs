using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace frm_vietnamese_calendar
{
    /// <summary>
    /// Donation prompt shown to the user every time they exit the app
    /// (unless they've checked "Don't show this again").
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

        protected override void OnHandleCreated(EventArgs e) // L-12: Win11 rounded corners
        {
            base.OnHandleCreated(e);
            try { int p = 2; DwmSetWindowAttribute(Handle, 33, ref p, sizeof(int)); } catch { }
        }

        private static readonly Size CompactSize = new Size(420, 270);
        private static readonly Size ExpandedSize = new Size(420, 355);

        /// <summary>Whether the user checked "Don't show this again".</summary>
        public bool SkipNextTime { get; private set; }

        public DonateForm()
        {
            InitializeComponent();
            ClientSize = CompactSize;

            // V11-08: apply bilingual text from AppStrings
            Text               = AppStrings.DonateTitle;
            lblTitleText.Text  = AppStrings.DonateTitle;
            lblHeading.Text    = AppStrings.DonateHeading;
            lblMessage.Text    = AppStrings.DonateMessage;
            lblEncourage.Text  = AppStrings.DonateEncourage;
            lblHint.Text       = AppStrings.DonateHint;
            chkDontShow.Text   = AppStrings.DonateDontShow;
            btnYes.Text        = AppStrings.DonateYes;
            btnNo.Text         = AppStrings.DonateNo;
            lblThanks.Text     = AppStrings.DonateThanks;

            pnlTitleBar.MouseDown += TitleBar_MouseDown;
            lblTitleText.MouseDown += TitleBar_MouseDown;
            lblTitleClose.Click += LblTitleClose_Click;

            KeyDown += DonateForm_KeyDown;

            LoadDonationImage(picTcb, "tcb.jpg");
            LoadDonationImage(picMomo, "momo.jpg");
        }

        private void DonateForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                CloseDialog();
            }
        }

        // Drag the dialog by "faking" a title bar drag since the form has no border
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

        // Load the QR image embedded in the assembly (so the build produces a single exe file), safely ignoring errors
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
                // A missing or broken QR image shouldn't crash the dialog
            }
        }

        // Click "Yes": hide the invite content, show the 2 QR codes to scan for donations
        // Keep chkDontShow visible so the user can still check it before closing
        private void btnYes_Click(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
            lblEncourage.Visible = false;
            lblHint.Visible = false;
            btnYes.Visible = false;
            btnNo.Visible = false;

            picTcb.Visible = true;
            picMomo.Visible = true;
            lblTcb.Visible = true;
            lblMomo.Visible = true;
            lblThanks.Visible = true;

            // Reposition controls so lblThanks and chkDontShow don't overlap (C-02)
            lblThanks.Location = new System.Drawing.Point(20, 278);
            chkDontShow.Location = new System.Drawing.Point(110, 316);
            chkDontShow.BringToFront();

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
