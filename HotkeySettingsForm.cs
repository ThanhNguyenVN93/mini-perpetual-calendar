using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace frm_vietnamese_calendar
{
    // Dialog for changing global hotkeys: click a field, then press the new key combo; the actual
    // conflict with the system is checked before saving (by test-registering on the main form's own handle).
    public partial class HotkeySettingsForm : Form
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

        // Key combos that should never be allowed as global hotkeys (would break system/apps)
        private static readonly HashSet<ulong> DangerousKeys = new HashSet<ulong>
        {
            Pack(GlobalHotkeyHelper.MOD_CONTROL, (uint)Keys.C),
            Pack(GlobalHotkeyHelper.MOD_CONTROL, (uint)Keys.V),
            Pack(GlobalHotkeyHelper.MOD_CONTROL, (uint)Keys.X),
            Pack(GlobalHotkeyHelper.MOD_CONTROL, (uint)Keys.Z),
            Pack(GlobalHotkeyHelper.MOD_CONTROL, (uint)Keys.A),
            Pack(GlobalHotkeyHelper.MOD_CONTROL, (uint)Keys.S),
            Pack(GlobalHotkeyHelper.MOD_ALT,     (uint)Keys.F4),
            Pack(GlobalHotkeyHelper.MOD_ALT,     (uint)Keys.Tab),
            Pack(GlobalHotkeyHelper.MOD_ALT,     (uint)Keys.Escape),
        };

        private static ulong Pack(uint mod, uint vk) => ((ulong)mod << 32) | vk;

        // The owning Form1 instance: we use _owner.Handle (always current) instead of a stale IntPtr
        private readonly Form _owner;

        private readonly uint _initialToggleModifiers;
        private readonly uint _initialToggleKey;
        private readonly uint _initialMonthModifiers;
        private readonly uint _initialMonthKey;

        private uint _toggleModifiers;
        private uint _toggleKey;
        private uint _monthModifiers;
        private uint _monthKey;

        private bool _listeningToggle;
        private bool _listeningMonth;

        public uint ToggleModifiers { get { return _toggleModifiers; } }
        public uint ToggleKey { get { return _toggleKey; } }
        public uint MonthModifiers { get { return _monthModifiers; } }
        public uint MonthKey { get { return _monthKey; } }

        public HotkeySettingsForm(Form owner, uint toggleModifiers, uint toggleKey, uint monthModifiers, uint monthKey)
        {
            InitializeComponent();

            _owner = owner;
            _initialToggleModifiers = toggleModifiers;
            _initialToggleKey = toggleKey;
            _initialMonthModifiers = monthModifiers;
            _initialMonthKey = monthKey;

            _toggleModifiers = toggleModifiers;
            _toggleKey = toggleKey;
            _monthModifiers = monthModifiers;
            _monthKey = monthKey;

            pnlTitleBar.MouseDown += TitleBar_MouseDown;
            lblTitleText.MouseDown += TitleBar_MouseDown;
            lblTitleClose.Click += (sender, e) => Close();

            // L-07: bilingual labels
            Text                 = AppStrings.HotkeySettingsTitle;
            lblTitleText.Text    = AppStrings.HotkeySettingsTitle;
            lblToggleCaption.Text = AppStrings.HotkeyToggleCaption;
            lblMonthCaption.Text  = AppStrings.HotkeyMonthCaption;
            lblHint.Text          = AppStrings.HotkeyHint;
            btnSave.Text          = AppStrings.HotkeySave;
            btnCancel.Text        = AppStrings.HotkeyCancel;

            lblToggleValue.Text = GlobalHotkeyHelper.Format(_toggleModifiers, _toggleKey);
            lblMonthValue.Text  = GlobalHotkeyHelper.Format(_monthModifiers, _monthKey);

            lblToggleValue.Click += (sender, e) => StartListening(forToggle: true);
            lblMonthValue.Click += (sender, e) => StartListening(forToggle: false);

            btnSave.Click += BtnSave_Click;
            btnCancel.Click += (sender, e) => Close();
        }

        private void TitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, (IntPtr)HT_CAPTION, IntPtr.Zero);
            }
        }

        private void StartListening(bool forToggle)
        {
            _listeningToggle = forToggle;
            _listeningMonth = !forToggle;
            lblError.Visible = false;

            if (forToggle)
                lblToggleValue.Text = AppStrings.HotkeyPressCombo;
            else
                lblMonthValue.Text  = AppStrings.HotkeyPressCombo;
        }

        // ProcessCmdKey intercepts ALL key messages (including arrow keys, Tab, Space, Enter) before
        // WinForms routing — the only reliable way to capture them in a hotkey-capture dialog
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (!_listeningToggle && !_listeningMonth)
                return base.ProcessCmdKey(ref msg, keyData);

            const int WM_KEYDOWN = 0x0100;
            const int WM_SYSKEYDOWN = 0x0104;
            if (msg.Msg != WM_KEYDOWN && msg.Msg != WM_SYSKEYDOWN)
                return base.ProcessCmdKey(ref msg, keyData);

            Keys keyCode = keyData & Keys.KeyCode;
            Keys modMask = keyData & Keys.Modifiers;

            // Escape cancels capture
            if (keyCode == Keys.Escape)
            {
                if (_listeningToggle)
                    lblToggleValue.Text = GlobalHotkeyHelper.Format(_toggleModifiers, _toggleKey);
                else
                    lblMonthValue.Text  = GlobalHotkeyHelper.Format(_monthModifiers, _monthKey);
                _listeningToggle = false;
                _listeningMonth  = false;
                lblError.Visible = false;
                return true;
            }

            // Ignore standalone modifier-only presses
            if (keyCode == Keys.ControlKey || keyCode == Keys.ShiftKey || keyCode == Keys.Menu
                || keyCode == Keys.LWin || keyCode == Keys.RWin)
                return true; // consumed but not recorded yet

            // Build modifier mask
            uint modifiers = 0;
            if ((modMask & Keys.Control) != 0) modifiers |= GlobalHotkeyHelper.MOD_CONTROL;
            if ((modMask & Keys.Alt) != 0)     modifiers |= GlobalHotkeyHelper.MOD_ALT;
            if ((modMask & Keys.Shift) != 0)   modifiers |= GlobalHotkeyHelper.MOD_SHIFT;

            if (modifiers == 0)
            {
                lblError.Text    = AppStrings.HotkeyRequireModifier;
                lblError.Visible = true;
                return true;
            }

            uint vk = (uint)keyCode;

            // Block system-critical combos
            if (DangerousKeys.Contains(Pack(modifiers, vk)))
            {
                lblError.Text    = AppStrings.HotkeyReserved;
                lblError.Visible = true;
                return true;
            }

            if (_listeningToggle)
            {
                _toggleModifiers = modifiers;
                _toggleKey = vk;
                lblToggleValue.Text = GlobalHotkeyHelper.Format(modifiers, vk);
            }
            else
            {
                _monthModifiers = modifiers;
                _monthKey = vk;
                lblMonthValue.Text = GlobalHotkeyHelper.Format(modifiers, vk);
            }

            _listeningToggle = false;
            _listeningMonth = false;
            lblError.Visible = false;
            return true; // consumed
        }

        // Keep the base KeyDown handler for compatibility, but it is now superseded by ProcessCmdKey
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (_listeningToggle || _listeningMonth)
            {
                e.SuppressKeyPress = true;
                e.Handled = true;
                return;
            }
            base.OnKeyDown(e);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (_toggleModifiers == _monthModifiers && _toggleKey == _monthKey)
            {
                lblError.Text    = AppStrings.HotkeyDuplicate;
                lblError.Visible = true;
                return;
            }

            IntPtr ownerHandle = _owner != null ? _owner.Handle : IntPtr.Zero;

            // Unregister the current hotkeys, then try registering the new pair to detect conflicts with other apps
            GlobalHotkeyHelper.UnregisterHotKey(ownerHandle, GlobalHotkeyHelper.IdToggleForm);
            GlobalHotkeyHelper.UnregisterHotKey(ownerHandle, GlobalHotkeyHelper.IdMonthView);

            bool ok = GlobalHotkeyHelper.RegisterHotKey(ownerHandle, GlobalHotkeyHelper.IdToggleForm,
                          _toggleModifiers | GlobalHotkeyHelper.MOD_NOREPEAT, _toggleKey)
                   && GlobalHotkeyHelper.RegisterHotKey(ownerHandle, GlobalHotkeyHelper.IdMonthView,
                          _monthModifiers | GlobalHotkeyHelper.MOD_NOREPEAT, _monthKey);

            if (!ok)
            {
                // Restore the old hotkeys since the new pair failed to register (already taken by another app)
                GlobalHotkeyHelper.UnregisterHotKey(ownerHandle, GlobalHotkeyHelper.IdToggleForm);
                GlobalHotkeyHelper.UnregisterHotKey(ownerHandle, GlobalHotkeyHelper.IdMonthView);
                GlobalHotkeyHelper.RegisterHotKey(ownerHandle, GlobalHotkeyHelper.IdToggleForm,
                    _initialToggleModifiers | GlobalHotkeyHelper.MOD_NOREPEAT, _initialToggleKey);
                GlobalHotkeyHelper.RegisterHotKey(ownerHandle, GlobalHotkeyHelper.IdMonthView,
                    _initialMonthModifiers | GlobalHotkeyHelper.MOD_NOREPEAT, _initialMonthKey);

                lblError.Text    = AppStrings.HotkeyConflictApp;
                lblError.Visible = true;
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
