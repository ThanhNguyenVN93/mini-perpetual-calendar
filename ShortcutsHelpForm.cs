using System.Runtime.InteropServices;
using System.Windows.Forms;
using System;

namespace frm_vietnamese_calendar
{
    // Dialog that quickly lists the tool's basic actions / shortcuts
    public partial class ShortcutsHelpForm : Form
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

        public ShortcutsHelpForm(uint toggleModifiers, uint toggleKey, uint monthModifiers, uint monthKey)
        {
            InitializeComponent();

            pnlTitleBar.MouseDown += TitleBar_MouseDown;
            lblTitleText.MouseDown += TitleBar_MouseDown;
            lblTitleClose.Click += (sender, e) => Close();

            KeyDown += (sender, e) =>
            {
                if (e.KeyCode == Keys.Escape)
                {
                    Close();
                }
            };

            string toggleHotkey = GlobalHotkeyHelper.Format(toggleModifiers, toggleKey);
            string monthHotkey  = GlobalHotkeyHelper.Format(monthModifiers, monthKey);

            Text              = AppStrings.ShortcutsHelpTitle; // L-07: bilingual window/title-bar title
            lblTitleText.Text = AppStrings.ShortcutsHelpTitle;
            lblContent.Text   = AppStrings.BuildShortcutsContent(toggleHotkey, monthHotkey);
        }

        private void TitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, (IntPtr)HT_CAPTION, IntPtr.Zero);
            }
        }
    }
}
