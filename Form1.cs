using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;

namespace frm_vietnamese_calendar
{
    public partial class Form1 : Form
    {
        // ---- P/Invoke ----
        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [DllImport("dwmapi.dll", PreserveSig = false)]
        private static extern void DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int value, int size);

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hwnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hwnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        private static extern bool DestroyIcon(IntPtr handle);

        [DllImport("shell32.dll")]
        private static extern int SHQueryUserNotificationState(out int pquns);

        private const int WM_NCLBUTTONDOWN               = 0xA1;
        private const int HT_CAPTION                     = 0x2;
        private const int DWMWA_WINDOW_CORNER_PREFERENCE = 33;
        private const int DWMWCP_ROUND                   = 2;
        private const int GWL_EXSTYLE                    = -20;
        private const int WS_EX_TRANSPARENT              = 0x00000020;
        private const int WS_EX_LAYERED                  = 0x00080000; // V11-04

        // ---- Colors ----
        private static readonly Color CloseIdleColor     = Color.FromArgb(150, 150, 150);
        private static readonly Color CloseHoverColor    = Color.FromArgb(255, 90, 90);
        private static readonly Color FeedbackIdleColor  = Color.FromArgb(180, 180, 180);
        private static readonly Color FeedbackHoverColor = Color.FromArgb(255, 210, 130);
        private static readonly Color TextOutlineColor   = Color.FromArgb(210, 0, 0, 0);

        private const string FeedbackUrl              = "https://www.facebook.com/PycCodeTools";
        private const string StartupRegistryKeyPath   = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
        private const string StartupRegistryValueName = "frm_vietnamese_calendar";

        // ---- State ----
        private DateTime _lastDisplayedDate;
        private DateTime _lastReminderCheckDate = DateTime.MinValue; // V11-01
        private readonly BackgroundImageProvider _bgProvider;
        private bool _donatePromptShown;
        private bool _isExiting;
        private volatile bool _closing;

        private string _weekdayText   = string.Empty;
        private string _solarDateText = string.Empty;
        private string _lunarDateText = string.Empty;

        private uint _hotkeyToggleModifiers;
        private uint _hotkeyToggleKey;
        private uint _hotkeyMonthModifiers;
        private uint _hotkeyMonthKey;

        private MonthCalendarForm _monthCalendarForm;
        private bool _hotkeySettingsOpen;

        private DateTime _lastMouseDownTime = DateTime.MinValue;
        private Point    _lastMouseDownScreenPos;

        private bool _startupSyncGuard;
        private bool _hiddenByFullscreen;

        // Dynamic tray icon (F-12)
        private IntPtr _dynamicTrayIconHandle = IntPtr.Zero;
        private Icon   _originalTrayIcon;

        // ---- Context menu items (fields for V11-03 ApplyLocalization) ----
        private ToolStripMenuItem _menuShowHide;
        private ToolStripMenuItem _menuAlwaysOnTop;
        private ToolStripMenuItem _menuOpacity;
        private ToolStripMenuItem _menuLockPosition;
        private ToolStripMenuItem _menuClickThrough;
        private ToolStripMenuItem _menuFullscreenHide;
        private ToolStripMenuItem _menuCopyDate;
        private ToolStripMenuItem _menuBg;
        private ToolStripMenuItem _menuBgNext;
        private ToolStripMenuItem _menuBgPauseResume;
        private ToolStripMenuItem _menuBgSolid;
        private ToolStripMenuItem _menuBgInterval;
        private ToolStripMenuItem _menuTheme;
        private ToolStripMenuItem _menuThemePhoto;
        private ToolStripMenuItem _menuThemeBloc;
        private ToolStripMenuItem _menuReminders;
        private ToolStripMenuItem _menuExportIcs;
        private ToolStripMenuItem _menuLanguage;
        private ToolStripMenuItem _menuWeekNumbers;

        private Font _menuShowHideFont;
        private readonly Font _countdownFont = new Font("Segoe UI", 8F, FontStyle.Regular);

        public Form1(EventWaitHandle wakeEvent)
        {
            InitializeComponent();
            DoubleBuffered = true;

            // First-run migration from Properties.Settings
            if (!System.IO.File.Exists(AppConfig.ConfigPath))
            {
                AppConfig.Current.HotkeyToggleModifiers = Properties.Settings.Default.HotkeyToggleModifiers;
                AppConfig.Current.HotkeyToggleKey       = Properties.Settings.Default.HotkeyToggleKey;
                AppConfig.Current.HotkeyMonthModifiers  = Properties.Settings.Default.HotkeyMonthModifiers;
                AppConfig.Current.HotkeyMonthKey        = Properties.Settings.Default.HotkeyMonthKey;
                AppConfig.Current.SkipDonatePrompt      = Properties.Settings.Default.SkipDonatePrompt;
                AppConfig.Current.Save();
            }

            var cfg = AppConfig.Current;

            Opacity  = cfg.OpacityPercent / 100.0;
            TopMost  = cfg.IsAlwaysOnTop;

            if (cfg.WindowLeft != -1 || cfg.WindowTop != -1) // L-13: allow negative coords for left-side monitors
            {
                bool onScreen = false;
                var testRect = new Rectangle(cfg.WindowLeft, cfg.WindowTop, Width, Height);
                foreach (Screen s in Screen.AllScreens)
                    if (s.WorkingArea.IntersectsWith(testRect)) { onScreen = true; break; }
                if (onScreen)
                {
                    StartPosition = FormStartPosition.Manual;
                    Left = cfg.WindowLeft;
                    Top  = cfg.WindowTop;
                }
            }

            _hotkeyToggleModifiers = (uint)cfg.HotkeyToggleModifiers;
            _hotkeyToggleKey       = (uint)cfg.HotkeyToggleKey;
            _hotkeyMonthModifiers  = (uint)cfg.HotkeyMonthModifiers;
            _hotkeyMonthKey        = (uint)cfg.HotkeyMonthKey;

            try { Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath); } catch { }
            _originalTrayIcon = Icon ?? SystemIcons.Application;
            notifyIconMain.Icon        = _originalTrayIcon;
            notifyIconMain.DoubleClick += NotifyIconMain_DoubleClick;
            notifyIconMain.MouseClick  += NotifyIconMain_MouseClick;

            // ---- Build context menu ----
            _menuShowHideFont = new Font(SystemFonts.MenuFont ?? SystemFonts.DefaultFont, FontStyle.Bold);

            _menuShowHide = new ToolStripMenuItem { Font = _menuShowHideFont };
            _menuShowHide.Click += (s, e) => ToggleFormVisibility();
            contextMenuMain.Items.Insert(0, _menuShowHide);
            contextMenuMain.Items.Insert(1, new ToolStripSeparator());

            // Widget behavior
            _menuAlwaysOnTop  = new ToolStripMenuItem { CheckOnClick = true, Checked = cfg.IsAlwaysOnTop };
            _menuLockPosition = new ToolStripMenuItem { CheckOnClick = true, Checked = cfg.IsPositionLocked };
            _menuClickThrough = new ToolStripMenuItem { CheckOnClick = true, Checked = cfg.IsClickThrough };
            _menuFullscreenHide = new ToolStripMenuItem { CheckOnClick = true, Checked = cfg.FullscreenHide };
            _menuAlwaysOnTop.CheckedChanged   += (s, e) => { AppConfig.Current.IsAlwaysOnTop    = _menuAlwaysOnTop.Checked;    TopMost = _menuAlwaysOnTop.Checked; AppConfig.Current.Save(); };
            _menuLockPosition.CheckedChanged  += (s, e) => { AppConfig.Current.IsPositionLocked  = _menuLockPosition.Checked;  AppConfig.Current.Save(); };
            _menuClickThrough.CheckedChanged += (s, e) =>
            {
                AppConfig.Current.IsClickThrough = _menuClickThrough.Checked;
                ApplyClickThrough(_menuClickThrough.Checked);
                AppConfig.Current.Save();
                // L-16: tell the user how to turn click-through off, since left-click on the form no longer works
                if (_menuClickThrough.Checked && notifyIconMain != null && notifyIconMain.Visible)
                    notifyIconMain.ShowBalloonTip(3000, AppStrings.MenuClickThrough,
                        AppStrings.IsVietnamese
                            ? "Đã bật nhấp xuyên. Chuột phải vào biểu tượng dưới khay hệ thống để tắt."
                            : "Click-through enabled. Right-click the system tray icon to disable it.",
                        ToolTipIcon.Info);
            };
            _menuFullscreenHide.CheckedChanged += (s, e) => { AppConfig.Current.FullscreenHide   = _menuFullscreenHide.Checked; AppConfig.Current.Save(); };

            _menuOpacity = new ToolStripMenuItem();
            foreach (int pct in new[] { 40, 60, 80, 100 })
            {
                int p = pct;
                var sub = new ToolStripMenuItem(p + "%") { CheckOnClick = true, Checked = (cfg.OpacityPercent == p) };
                sub.Click += (s, e) =>
                {
                    foreach (ToolStripMenuItem it in _menuOpacity.DropDownItems) it.Checked = (it == (ToolStripMenuItem)s);
                    AppConfig.Current.OpacityPercent = p;
                    Opacity = p / 100.0;
                    AppConfig.Current.Save();
                };
                _menuOpacity.DropDownItems.Add(sub);
            }

            // Background submenu
            _menuBg = new ToolStripMenuItem();
            _menuBgNext = new ToolStripMenuItem();
            _menuBgNext.Click += (s, e) => _bgProvider?.RequestNewImage();
            _menuBgPauseResume = new ToolStripMenuItem();
            _menuBgPauseResume.Click += MenuBgPauseResume_Click;
            _menuBgSolid = new ToolStripMenuItem { CheckOnClick = true, Checked = cfg.BackgroundSolid };
            _menuBgSolid.CheckedChanged += (s, e) =>
            {
                AppConfig.Current.BackgroundSolid = _menuBgSolid.Checked;
                if (_menuBgSolid.Checked)
                {
                    timerBackground.Stop();
                    BackgroundImage?.Dispose();
                    BackgroundImage = null;
                }
                else if (AppConfig.Current.ThemeName != "bloc") // L-10: restore image when unticked
                {
                    Image cached = _bgProvider?.LoadCachedImage();
                    if (cached != null) OnBackgroundImageReady(cached);
                    _bgProvider?.RequestNewImage();
                    if (!AppConfig.Current.BackgroundPaused) timerBackground.Start();
                }
                AppConfig.Current.Save();
                Invalidate();
            };
            _menuBgInterval = new ToolStripMenuItem();
            foreach (int secs in new[] { 30, 60, 300, 600, 1800 })
            {
                int s2 = secs;
                string label = AppStrings.FormatBgInterval(secs); // L-07: bilingual from start
                var sub = new ToolStripMenuItem(label) { CheckOnClick = true, Checked = (cfg.BackgroundIntervalSeconds == secs) };
                sub.Click += (sen, e) =>
                {
                    foreach (ToolStripMenuItem it in _menuBgInterval.DropDownItems) it.Checked = (it == (ToolStripMenuItem)sen);
                    AppConfig.Current.BackgroundIntervalSeconds = s2;
                    timerBackground.Interval = s2 * 1000;
                    AppConfig.Current.Save();
                };
                _menuBgInterval.DropDownItems.Add(sub);
            }
            _menuBg.DropDownItems.AddRange(new ToolStripItem[] { _menuBgNext, _menuBgPauseResume, new ToolStripSeparator(), _menuBgSolid, _menuBgInterval });

            // Theme submenu
            _menuTheme = new ToolStripMenuItem();
            _menuThemePhoto = new ToolStripMenuItem { CheckOnClick = true, Checked = (cfg.ThemeName != "bloc") };
            _menuThemeBloc  = new ToolStripMenuItem { CheckOnClick = true, Checked = (cfg.ThemeName == "bloc") };
            _menuThemePhoto.Click += (s, e) => SetTheme("photo");
            _menuThemeBloc.Click  += (s, e) => SetTheme("bloc");
            _menuTheme.DropDownItems.AddRange(new ToolStripItem[] { _menuThemePhoto, _menuThemeBloc });

            _menuCopyDate = new ToolStripMenuItem();
            _menuCopyDate.Click += (s, e) => CopyDateToClipboard();

            _menuReminders = new ToolStripMenuItem();
            _menuReminders.Click += (s, e) => OpenReminders();

            _menuExportIcs = new ToolStripMenuItem();
            _menuExportIcs.Click += (s, e) => ExportIcs();

            _menuLanguage = new ToolStripMenuItem();
            _menuLanguage.Click += (s, e) => ToggleLanguage();

            _menuWeekNumbers = new ToolStripMenuItem { CheckOnClick = true, Checked = cfg.ShowWeekNumbers };
            _menuWeekNumbers.CheckedChanged += (s, e) => { AppConfig.Current.ShowWeekNumbers = _menuWeekNumbers.Checked; AppConfig.Current.Save(); };

            int pos = 3;
            void Ins(ToolStripItem it) { contextMenuMain.Items.Insert(pos++, it); }
            Ins(new ToolStripSeparator());
            Ins(_menuAlwaysOnTop);
            Ins(_menuOpacity);
            Ins(_menuLockPosition);
            Ins(_menuClickThrough);
            Ins(_menuFullscreenHide);
            Ins(new ToolStripSeparator());
            Ins(_menuCopyDate);
            Ins(new ToolStripSeparator());
            Ins(_menuBg);
            Ins(_menuTheme);
            Ins(new ToolStripSeparator());
            Ins(_menuReminders);
            Ins(_menuExportIcs);
            Ins(new ToolStripSeparator());
            Ins(_menuLanguage);
            Ins(_menuWeekNumbers);
            Ins(new ToolStripSeparator());

            // L-14: block tray context menu when a modal dialog is open (bring existing dialog to front instead)
            contextMenuMain.Opening += (s, e) =>
            {
                if (_hotkeySettingsOpen || OwnedForms.Length > 0)
                {
                    e.Cancel = true;
                    if (OwnedForms.Length > 0)
                    {
                        OwnedForms[OwnedForms.Length - 1].BringToFront();
                        OwnedForms[OwnedForms.Length - 1].Activate();
                    }
                }
            };

            // ---- Mouse / keyboard / context ----
            MouseDown              += Form1_MouseDown;
            lblWeekday.MouseDown   += Form1_MouseDown;
            lblSolarDate.MouseDown += Form1_MouseDown;
            lblLunarDate.MouseDown += Form1_MouseDown;

            ContextMenuStrip              = contextMenuMain;
            lblWeekday.ContextMenuStrip   = contextMenuMain;
            lblSolarDate.ContextMenuStrip = contextMenuMain;
            lblLunarDate.ContextMenuStrip = contextMenuMain;
            lblFeedback.ContextMenuStrip  = contextMenuMain;

            lblClose.ForeColor     = CloseIdleColor;
            lblClose.MouseEnter   += LblClose_MouseEnter;
            lblClose.MouseLeave   += LblClose_MouseLeave;
            lblClose.Click        += LblClose_Click;

            lblFeedback.ForeColor   = FeedbackIdleColor;
            lblFeedback.MouseEnter += LblFeedback_MouseEnter;
            lblFeedback.MouseLeave += LblFeedback_MouseLeave;
            lblFeedback.Click      += (sender, e) => OpenFeedbackLink();

            lblHelp.MouseEnter += LblHelp_MouseEnter;
            lblHelp.MouseLeave += LblHelp_MouseLeave;
            lblHelp.Click      += (sender, e) => OpenShortcutsHelp();

            bool startWithWindows = IsStartupWithWindowsEnabled();
            chkStartWithWindows.Checked      = startWithWindows;
            menuItemStartWithWindows.Checked = startWithWindows;
            menuItemStartWithWindows.CheckedChanged += MenuItemStartWithWindows_CheckedChanged;
            chkStartWithWindows.CheckedChanged      += ChkStartWithWindows_CheckedChanged;

            KeyDown += Form1_KeyDown;
            SystemEvents.TimeChanged     += SystemEvents_TimeChanged;
            SystemEvents.PowerModeChanged += SystemEvents_PowerModeChanged;

            // Apply localized text to all controls/menus (V11-03)
            ApplyLocalization();

            timerRefresh.Start();
            timerBackground.Interval = cfg.BackgroundIntervalSeconds * 1000;
            if (!cfg.BackgroundPaused && !cfg.BackgroundSolid)
                timerBackground.Start();

            _bgProvider = new BackgroundImageProvider(ClientSize.Width, ClientSize.Height);
            _bgProvider.ImageReady += OnBackgroundImageReady;

            if (!cfg.BackgroundSolid && cfg.ThemeName != "bloc")
            {
                Image cachedImage = _bgProvider.LoadCachedImage();
                if (cachedImage != null) OnBackgroundImageReady(cachedImage);
                _bgProvider.RequestNewImage();
            }

            if (cfg.IsClickThrough && IsHandleCreated)
                ApplyClickThrough(true);

            // F-14: background thread watching for second-instance wake signal
            var t = new Thread(() =>
            {
                while (!_closing)
                {
                    if (wakeEvent.WaitOne(500) && !_closing && IsHandleCreated)
                        BeginInvoke(new Action(RestoreFromTray));
                }
            });
            t.IsBackground = true;
            t.Start();
        }

        // ---- V11-03: Localization ----

        private void ApplyLocalization()
        {
            chkStartWithWindows.Text = AppStrings.MenuStartWindows;
            lblFeedback.Text         = AppStrings.MenuFeedback;
            lblHelp.Text             = "?";

            _menuShowHide.Text            = AppStrings.MenuShowHide;
            menuItemMonthView.Text        = AppStrings.MenuMonthView;
            _menuAlwaysOnTop.Text         = AppStrings.MenuAlwaysOnTop;
            _menuOpacity.Text             = AppStrings.MenuOpacity;
            _menuLockPosition.Text        = AppStrings.MenuLockPosition;
            _menuClickThrough.Text        = AppStrings.MenuClickThrough;
            _menuFullscreenHide.Text      = AppStrings.MenuFullscreenHide;
            _menuCopyDate.Text            = AppStrings.MenuCopyDate;
            _menuBg.Text                  = AppStrings.MenuBackground;
            _menuBgNext.Text              = AppStrings.MenuBgNext;
            _menuBgPauseResume.Text       = AppConfig.Current.BackgroundPaused ? AppStrings.MenuBgResume : AppStrings.MenuBgPause;
            _menuBgSolid.Text             = AppStrings.MenuBgSolid;
            _menuBgInterval.Text          = AppStrings.MenuBgInterval;
            _menuTheme.Text               = AppStrings.MenuTheme;
            _menuThemePhoto.Text          = AppStrings.MenuThemePhoto;
            _menuThemeBloc.Text           = AppStrings.MenuThemeBloc;
            _menuReminders.Text           = AppStrings.MenuReminders;
            _menuExportIcs.Text           = AppStrings.MenuExportIcs;
            _menuLanguage.Text            = AppStrings.MenuLanguage;
            _menuWeekNumbers.Text         = AppStrings.MenuWeekNumbers;
            menuItemStartWithWindows.Text = AppStrings.MenuStartWindows;
            menuItemChangeHotkeys.Text    = AppStrings.MenuHotkeys;
            menuItemFeedback.Text         = AppStrings.MenuFeedback;
            menuItemExit.Text             = AppStrings.MenuExit;

            // L-07: re-label background interval sub-items bilingually
            int[] bgIntervals = new[] { 30, 60, 300, 600, 1800 };
            for (int i = 0; i < bgIntervals.Length && i < _menuBgInterval.DropDownItems.Count; i++)
                _menuBgInterval.DropDownItems[i].Text = AppStrings.FormatBgInterval(bgIntervals[i]);

            UpdateCalendarDisplay(); // L-01: populate 3 date lines on startup and after language toggle
        }

        // ---- Handle created / destroyed ----

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            // F-19: DWM rounded corners (Windows 11 only)
            try
            {
                int pref = DWMWCP_ROUND;
                DwmSetWindowAttribute(Handle, DWMWA_WINDOW_CORNER_PREFERENCE, ref pref, sizeof(int));
            }
            catch { }

            bool ok1 = GlobalHotkeyHelper.RegisterHotKey(Handle, GlobalHotkeyHelper.IdToggleForm,
                _hotkeyToggleModifiers | GlobalHotkeyHelper.MOD_NOREPEAT, _hotkeyToggleKey);
            bool ok2 = GlobalHotkeyHelper.RegisterHotKey(Handle, GlobalHotkeyHelper.IdMonthView,
                _hotkeyMonthModifiers | GlobalHotkeyHelper.MOD_NOREPEAT, _hotkeyMonthKey);

            if ((!ok1 || !ok2) && notifyIconMain != null && notifyIconMain.Visible)
            {
                notifyIconMain.ShowBalloonTip(4000, AppStrings.HotkeyConflictTitle,
                    AppStrings.HotkeyConflictBody, ToolTipIcon.Warning);
            }

            if (AppConfig.Current.IsClickThrough)
                ApplyClickThrough(true);
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            GlobalHotkeyHelper.UnregisterHotKey(Handle, GlobalHotkeyHelper.IdToggleForm);
            GlobalHotkeyHelper.UnregisterHotKey(Handle, GlobalHotkeyHelper.IdMonthView);
            base.OnHandleDestroyed(e);
        }

        // ---- WndProc ----

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == GlobalHotkeyHelper.WM_HOTKEY)
            {
                int id = m.WParam.ToInt32();
                bool anyModalOpen = _hotkeySettingsOpen || OwnedForms.Length > 0;
                if (id == GlobalHotkeyHelper.IdToggleForm && !anyModalOpen)
                    ToggleFormVisibility();
                else if (id == GlobalHotkeyHelper.IdMonthView && !_hotkeySettingsOpen)
                {
                    if (OwnedForms.Length == 0 || _monthCalendarForm != null)
                        OpenMonthCalendar();
                }
            }
            base.WndProc(ref m);
        }

        // ---- F-21: Ctrl+C copy date ----

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.C))
            {
                CopyDateToClipboard();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void CopyDateToClipboard()
        {
            try
            {
                string text = AppStrings.FormatSolarDate(DateTime.Now) + " (" + AppStrings.FormatLunarDate(DateTime.Now) + ")";
                Clipboard.SetText(text);
                notifyIconMain.ShowBalloonTip(1500, "", AppStrings.IsVietnamese ? "Đã sao chép!" : "Copied!", ToolTipIcon.Info);
            }
            catch { }
        }

        // ---- Hotkey settings ----

        private void menuItemChangeHotkeys_Click(object sender, EventArgs e)
        {
            _hotkeySettingsOpen = true;
            try
            {
                using (var dlg = new HotkeySettingsForm(
                    this, _hotkeyToggleModifiers, _hotkeyToggleKey, _hotkeyMonthModifiers, _hotkeyMonthKey))
                {
                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        _hotkeyToggleModifiers = dlg.ToggleModifiers;
                        _hotkeyToggleKey       = dlg.ToggleKey;
                        _hotkeyMonthModifiers  = dlg.MonthModifiers;
                        _hotkeyMonthKey        = dlg.MonthKey;
                        var cfg = AppConfig.Current;
                        cfg.HotkeyToggleModifiers = (int)_hotkeyToggleModifiers;
                        cfg.HotkeyToggleKey       = (int)_hotkeyToggleKey;
                        cfg.HotkeyMonthModifiers  = (int)_hotkeyMonthModifiers;
                        cfg.HotkeyMonthKey        = (int)_hotkeyMonthKey;
                        cfg.Save();
                    }
                }
            }
            finally { _hotkeySettingsOpen = false; }
        }

        // ---- Visibility ----

        private void ToggleFormVisibility()
        {
            if (Visible && WindowState != FormWindowState.Minimized) Hide();
            else RestoreFromTray();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) Close();
        }

        // ---- Drag (with position lock) ----

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            DateTime now = DateTime.UtcNow;
            Point screenPos = Cursor.Position;
            Size dblSize = SystemInformation.DoubleClickSize;

            bool isDoubleClick =
                (now - _lastMouseDownTime).TotalMilliseconds <= SystemInformation.DoubleClickTime &&
                Math.Abs(screenPos.X - _lastMouseDownScreenPos.X) <= dblSize.Width &&
                Math.Abs(screenPos.Y - _lastMouseDownScreenPos.Y) <= dblSize.Height;

            if (isDoubleClick)
            {
                _lastMouseDownTime = DateTime.MinValue;
                OpenMonthCalendar();
                return;
            }

            _lastMouseDownTime      = now;
            _lastMouseDownScreenPos = screenPos;

            if (!AppConfig.Current.IsPositionLocked)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, (IntPtr)HT_CAPTION, IntPtr.Zero);
            }
        }

        // ---- Labels ----

        private void LblClose_MouseEnter(object s, EventArgs e)    { lblClose.ForeColor = CloseHoverColor; }
        private void LblClose_MouseLeave(object s, EventArgs e)    { lblClose.ForeColor = CloseIdleColor; }
        private void LblClose_Click(object s, EventArgs e)         { Close(); }
        private void LblFeedback_MouseEnter(object s, EventArgs e) { lblFeedback.ForeColor = FeedbackHoverColor; }
        private void LblFeedback_MouseLeave(object s, EventArgs e) { lblFeedback.ForeColor = FeedbackIdleColor; }
        private void LblHelp_MouseEnter(object s, EventArgs e)     { lblHelp.ForeColor = FeedbackHoverColor; }
        private void LblHelp_MouseLeave(object s, EventArgs e)     { lblHelp.ForeColor = FeedbackIdleColor; }

        private void OpenShortcutsHelp()
        {
            using (var frm = new ShortcutsHelpForm(
                _hotkeyToggleModifiers, _hotkeyToggleKey, _hotkeyMonthModifiers, _hotkeyMonthKey))
                frm.ShowDialog(this);
        }

        // ---- Tray icon ----

        private void NotifyIconMain_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) RestoreFromTray();
        }

        private void NotifyIconMain_DoubleClick(object sender, EventArgs e) { RestoreFromTray(); }

        private void RestoreFromTray()
        {
            Show();
            WindowState = FormWindowState.Normal;
            BringToFront();
            Activate();
        }

        // ---- Start with Windows ----

        private void MenuItemStartWithWindows_CheckedChanged(object sender, EventArgs e)
        {
            if (_startupSyncGuard) return;
            _startupSyncGuard = true;
            chkStartWithWindows.Checked = menuItemStartWithWindows.Checked;
            _startupSyncGuard = false;
            ApplyStartWithWindows(menuItemStartWithWindows.Checked);
        }

        private void ChkStartWithWindows_CheckedChanged(object sender, EventArgs e)
        {
            if (_startupSyncGuard) return;
            _startupSyncGuard = true;
            menuItemStartWithWindows.Checked = chkStartWithWindows.Checked;
            _startupSyncGuard = false;
            ApplyStartWithWindows(chkStartWithWindows.Checked);
        }

        private void ApplyStartWithWindows(bool enabled)
        {
            try { SetStartupWithWindowsEnabled(enabled); }
            catch
            {
                bool actual = IsStartupWithWindowsEnabled();
                _startupSyncGuard = true;
                chkStartWithWindows.Checked = actual;
                menuItemStartWithWindows.Checked = actual;
                _startupSyncGuard = false;
            }
        }

        private static bool IsStartupWithWindowsEnabled()
        {
            try
            {
                using (var key = Registry.CurrentUser.OpenSubKey(StartupRegistryKeyPath, false))
                {
                    string val = key?.GetValue(StartupRegistryValueName) as string;
                    return !string.IsNullOrEmpty(val)
                        && string.Equals(val.Trim('"'), Application.ExecutablePath, StringComparison.OrdinalIgnoreCase);
                }
            }
            catch { return false; }
        }

        private static void SetStartupWithWindowsEnabled(bool enabled)
        {
            using (var key = Registry.CurrentUser.OpenSubKey(StartupRegistryKeyPath, true))
            {
                if (key == null) return;
                if (enabled) key.SetValue(StartupRegistryValueName, "\"" + Application.ExecutablePath + "\"");
                else         key.DeleteValue(StartupRegistryValueName, false);
            }
        }

        // ---- Calendar display ----

        private void UpdateCalendarDisplay()
        {
            DateTime now = DateTime.Now;
            _lastDisplayedDate = now.Date;

            _weekdayText   = AppStrings.FormatWeekday(now.DayOfWeek);
            _solarDateText = AppStrings.FormatSolarDate(now);
            _lunarDateText = AppStrings.FormatLunarDate(now);
            Invalidate();

            notifyIconMain.Text = AppStrings.FormatTrayTooltip(now);
            UpdateDynamicTrayIcon(now.Day);

            CheckDailyReminders(now.Date); // V11-01
        }

        // ---- V11-01: Daily reminder balloon notifications ----

        private void CheckDailyReminders(DateTime today)
        {
            if (_lastReminderCheckDate == today) return;
            _lastReminderCheckDate = today;

            var messages = new List<string>();
            bool vi = AppStrings.IsVietnamese;

            int ld, lm, ly; bool leap;
            LunarCalendar.ConvertSolar2Lunar(today.Day, today.Month, today.Year, out ld, out lm, out ly, out leap);
            if (ld == 1)
                messages.Add(vi ? "Hôm nay là Mùng 1 tháng " + lm + " Âm lịch"
                                : "Today is the 1st of Lunar month " + lm);
            else if (ld == 15)
                messages.Add(vi ? "Hôm nay là Rằm tháng " + lm + " Âm lịch"
                                : "Today is Full Moon (15th) of Lunar month " + lm);

            var reminders = AppConfig.Current.Reminders;
            if (reminders != null)
            {
                foreach (var ev in reminders)
                {
                    if (ev == null || string.IsNullOrEmpty(ev.Name)) continue;

                    for (int offset = 0; offset <= ev.DaysBefore; offset++)
                    {
                        DateTime targetDate = today.AddDays(offset);
                        bool matched;
                        if (ev.IsLunar)
                        {
                            int tld, tlm, tly; bool tleap;
                            LunarCalendar.ConvertSolar2Lunar(targetDate.Day, targetDate.Month, targetDate.Year,
                                out tld, out tlm, out tly, out tleap);
                            matched = !tleap && tld == ev.LunarDay && tlm == ev.LunarMonth; // L-05: skip intercalary months
                        }
                        else
                        {
                            matched = (targetDate.Day == ev.SolarDay && targetDate.Month == ev.SolarMonth);
                        }

                        if (matched)
                        {
                            messages.Add(offset == 0
                                ? (vi ? "Hôm nay: " : "Today: ") + ev.Name
                                : AppStrings.FormatCountdown(ev.Name, offset));
                            break;
                        }
                    }
                }
            }

            if (messages.Count > 0 && notifyIconMain != null && notifyIconMain.Visible)
                notifyIconMain.ShowBalloonTip(6000, AppStrings.RemindersTitle,
                    string.Join("\n", messages), ToolTipIcon.Info);
        }

        // ---- F-12: Dynamic tray icon ----

        private void UpdateDynamicTrayIcon(int day)
        {
            try
            {
                const int size = 16;
                using (var bmp = new Bitmap(size, size))
                using (var g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.Transparent);
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    using (var br = new SolidBrush(Color.FromArgb(200, 30, 30))) // V11-05 fix: using
                        g.FillEllipse(br, 0, 0, size - 1, size - 1);
                    string txt = day.ToString();
                    float fs = txt.Length > 1 ? 7f : 9f;
                    using (var f  = new Font("Arial", fs, FontStyle.Bold))
                    using (var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                        g.DrawString(txt, f, Brushes.White, new RectangleF(0, 0, size, size), sf);

                    IntPtr hIcon = bmp.GetHicon();
                    IntPtr old   = _dynamicTrayIconHandle;
                    _dynamicTrayIconHandle = hIcon;
                    notifyIconMain.Icon = Icon.FromHandle(hIcon);
                    if (old != IntPtr.Zero) DestroyIcon(old);
                }
            }
            catch { }
        }

        // ---- Timers ----

        private void timerRefresh_Tick(object sender, EventArgs e)
        {
            if (DateTime.Now.Date != _lastDisplayedDate)
                UpdateCalendarDisplay();

            // F-26: auto-hide when a fullscreen app is active
            if (AppConfig.Current.FullscreenHide)
            {
                bool fs = IsFullScreenApp();
                if (fs && Visible)         { _hiddenByFullscreen = true; Hide(); }
                else if (!fs && _hiddenByFullscreen) { _hiddenByFullscreen = false; RestoreFromTray(); }
            }
        }

        private void timerBackground_Tick(object sender, EventArgs e)
        {
            if (!Visible || AppConfig.Current.BackgroundSolid || AppConfig.Current.ThemeName == "bloc") return;
            _bgProvider?.RequestNewImage();
        }

        private void MenuBgPauseResume_Click(object sender, EventArgs e)
        {
            var cfg = AppConfig.Current;
            cfg.BackgroundPaused = !cfg.BackgroundPaused;
            if (cfg.BackgroundPaused) { timerBackground.Stop(); _menuBgPauseResume.Text = AppStrings.MenuBgResume; }
            else                      { timerBackground.Start(); _menuBgPauseResume.Text = AppStrings.MenuBgPause; }
            cfg.Save();
        }

        // ---- System events ----

        private void SystemEvents_TimeChanged(object sender, EventArgs e)
        {
            if (!IsDisposed && IsHandleCreated) BeginInvoke(new Action(UpdateCalendarDisplay));
        }

        private void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
            if (e.Mode == PowerModes.Resume && !IsDisposed && IsHandleCreated)
                BeginInvoke(new Action(UpdateCalendarDisplay));
        }

        // ---- Background image ----

        private void OnBackgroundImageReady(Image img)
        {
            if (IsDisposed) { img?.Dispose(); return; }
            Image old = BackgroundImage;
            BackgroundImage = img;
            BackgroundImageLayout = ImageLayout.Stretch;
            old?.Dispose();
        }

        // ---- Painting (V11-07 fix: call base.OnPaint(e) in OnPaint, not inside DrawPhotoTheme) ----

        protected override void OnPaint(PaintEventArgs e)
        {
            if (AppConfig.Current.ThemeName == "bloc")
                DrawBlocTheme(e.Graphics);
            else
            {
                base.OnPaint(e); // draws BackgroundImage — no PaintEventArgs allocation
                DrawPhotoOverlay(e.Graphics);
            }
        }

        private void DrawPhotoOverlay(Graphics g)
        {
            using (var overlay = new SolidBrush(Color.FromArgb(45, 0, 0, 0)))
                g.FillRectangle(overlay, 0, 0, ClientSize.Width, ClientSize.Height);

            using (var backdrop = new SolidBrush(Color.FromArgb(130, 0, 0, 0)))
            {
                // V11-07: draw backdrop for all 3 bottom controls
                Rectangle chkRect = chkStartWithWindows.Bounds; chkRect.Inflate(3, 2);
                g.FillRectangle(backdrop, chkRect);
                Rectangle helpRect = lblHelp.Bounds; helpRect.Inflate(-4, 1);
                g.FillRectangle(backdrop, helpRect);
                Rectangle fbRect = lblFeedback.Bounds; fbRect.Inflate(3, 2);
                fbRect.Width = Math.Min(fbRect.Right, ClientSize.Width) - fbRect.X;
                g.FillRectangle(backdrop, fbRect);
            }

            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            DrawOutlinedText(g, _weekdayText,   lblWeekday.Font,   lblWeekday.ForeColor,   lblWeekday.Bounds,   2f);
            DrawOutlinedText(g, _solarDateText, lblSolarDate.Font, lblSolarDate.ForeColor, lblSolarDate.Bounds, 3f);
            DrawOutlinedText(g, _lunarDateText, lblLunarDate.Font, lblLunarDate.ForeColor, lblLunarDate.Bounds, 2.5f);
            DrawExtras(g);
        }

        private void DrawBlocTheme(Graphics g)
        {
            using (var brush = new LinearGradientBrush(ClientRectangle,
                Color.FromArgb(210, 25, 25), Color.FromArgb(140, 10, 10), LinearGradientMode.Vertical))
                g.FillRectangle(brush, ClientRectangle);

            using (var gold = new SolidBrush(Color.FromArgb(210, 170, 50)))
                g.FillRectangle(gold, 0, 0, ClientSize.Width, 5);

            // V11-07: add backdrop for all 3 bottom controls in bloc theme too
            using (var backdrop = new SolidBrush(Color.FromArgb(80, 0, 0, 0)))
            {
                Rectangle chkRect = chkStartWithWindows.Bounds; chkRect.Inflate(3, 2);
                g.FillRectangle(backdrop, chkRect);
                Rectangle helpRect = lblHelp.Bounds; helpRect.Inflate(-4, 1);
                g.FillRectangle(backdrop, helpRect);
                Rectangle fbRect = lblFeedback.Bounds; fbRect.Inflate(3, 2);
                fbRect.Width = Math.Min(fbRect.Right, ClientSize.Width) - fbRect.X;
                g.FillRectangle(backdrop, fbRect);
            }

            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            DrawOutlinedText(g, _weekdayText,   lblWeekday.Font,   Color.FromArgb(255, 230, 180), lblWeekday.Bounds,   2f);
            DrawOutlinedText(g, _solarDateText, lblSolarDate.Font, Color.White,                   lblSolarDate.Bounds, 3f);
            DrawOutlinedText(g, _lunarDateText, lblLunarDate.Font, Color.FromArgb(255, 215, 100), lblLunarDate.Bounds, 2.5f);
            DrawExtras(g);
        }

        private void DrawExtras(Graphics g)
        {
            // F-28: countdown badge
            string countdown = VietnameseHolidays.GetCountdownText(DateTime.Now);
            if (!string.IsNullOrEmpty(countdown))
                DrawOutlinedText(g, countdown, _countdownFont, Color.FromArgb(255, 230, 100),
                    new Rectangle(0, 152, ClientSize.Width, 16), 1.5f); // L-06: moved up to avoid overlap with chkStartWithWindows

            // F-23: moon phase dot
            DrawMoonPhase(g);
        }

        // L-02: correct ellipse-terminator moon phase algorithm
        private void DrawMoonPhase(Graphics g)
        {
            int ld, lm, ly; bool leap;
            LunarCalendar.ConvertSolar2Lunar(DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year,
                out ld, out lm, out ly, out leap);

            int cx = 18, cy = 18, r = 8;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Color darkColor = Color.FromArgb(180, 20, 20, 40);
            Color litColor  = Color.FromArgb(235, 245, 240, 190);

            using (var darkBrush = new SolidBrush(darkColor))
                g.FillEllipse(darkBrush, cx - r, cy - r, r * 2, r * 2);

            if (ld <= 1 || ld >= 30)
            {
                using (var ringPen = new Pen(Color.FromArgb(90, 220, 220, 255), 1f))
                    g.DrawEllipse(ringPen, cx - r, cy - r, r * 2, r * 2);
                return;
            }

            if (ld >= 14 && ld <= 16)
            {
                using (var litBrush = new SolidBrush(litColor))
                    g.FillEllipse(litBrush, cx - r, cy - r, r * 2, r * 2);
                return;
            }

            double phase = (ld - 1) / 29.5;
            float terminatorRx = (float)(r * Math.Abs(Math.Cos(2.0 * Math.PI * phase)));

            using (var litBrush   = new SolidBrush(litColor))
            using (var darkBrush2 = new SolidBrush(darkColor))
            using (var clipPath   = new GraphicsPath())
            {
                clipPath.AddEllipse(cx - r, cy - r, r * 2, r * 2);
                g.SetClip(clipPath);

                if (phase < 0.5)
                {
                    g.FillRectangle(litBrush, cx, cy - r, r, r * 2);
                    if (phase < 0.25)
                        g.FillEllipse(darkBrush2, cx - terminatorRx, cy - r, terminatorRx * 2, r * 2);
                    else
                        g.FillEllipse(litBrush, cx - terminatorRx, cy - r, terminatorRx * 2, r * 2);
                }
                else
                {
                    g.FillRectangle(litBrush, cx - r, cy - r, r, r * 2);
                    if (phase < 0.75)
                        g.FillEllipse(litBrush, cx - terminatorRx, cy - r, terminatorRx * 2, r * 2);
                    else
                        g.FillEllipse(darkBrush2, cx - terminatorRx, cy - r, terminatorRx * 2, r * 2);
                }

                g.ResetClip();
            }
        }

        // ---- Outlined text ----

        private static void DrawOutlinedText(Graphics g, string text, Font font, Color fillColor, Rectangle bounds, float outlineWidth)
        {
            if (string.IsNullOrEmpty(text)) return;
            using (var format = new StringFormat
            {
                Alignment     = StringAlignment.Center,
                LineAlignment = StringAlignment.Center,
                Trimming      = StringTrimming.None,
                FormatFlags   = StringFormatFlags.NoWrap | StringFormatFlags.MeasureTrailingSpaces,
            })
            {
                float emSize  = font.Size * g.DpiY / 72f;
                SizeF measured = g.MeasureString(text, font, int.MaxValue, format);
                float maxWidth = bounds.Width - 4f;
                if (measured.Width > maxWidth && measured.Width > 0)
                    emSize *= maxWidth / measured.Width;

                using (var path = new GraphicsPath())
                {
                    path.AddString(text, font.FontFamily, (int)font.Style, emSize, bounds, format);
                    using (var pen  = new Pen(TextOutlineColor, outlineWidth) { LineJoin = LineJoin.Round })
                    using (var fill = new SolidBrush(fillColor))
                    {
                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        g.DrawPath(pen, path);
                        g.FillPath(fill, path);
                    }
                }
            }
        }

        // ---- Theme ----

        private void SetTheme(string name)
        {
            AppConfig.Current.ThemeName = name;
            AppConfig.Current.Save();
            _menuThemePhoto.Checked = (name != "bloc");
            _menuThemeBloc.Checked  = (name == "bloc");
            if (name == "bloc") { BackgroundImage?.Dispose(); BackgroundImage = null; }
            else if (BackgroundImage == null)
            {
                Image cached = _bgProvider?.LoadCachedImage();
                if (cached != null) OnBackgroundImageReady(cached);
            }
            Invalidate();
        }

        // ---- V11-04: Click-through (needs WS_EX_LAYERED when Opacity=100%) ----

        private void ApplyClickThrough(bool enable)
        {
            if (!IsHandleCreated) return;

            // WS_EX_TRANSPARENT only works when WS_EX_LAYERED is also set.
            // If opacity is 100%, WinForms removes WS_EX_LAYERED, so we nudge opacity slightly.
            if (enable && Opacity >= 1.0)
                Opacity = 0.996;
            else if (!enable && AppConfig.Current.OpacityPercent == 100)
                Opacity = 1.0;

            int ex = GetWindowLong(Handle, GWL_EXSTYLE);
            SetWindowLong(Handle, GWL_EXSTYLE,
                enable ? (ex | WS_EX_LAYERED | WS_EX_TRANSPARENT) : (ex & ~WS_EX_TRANSPARENT));
        }

        // ---- F-26: Fullscreen detection ----

        private static bool IsFullScreenApp()
        {
            int state;
            if (SHQueryUserNotificationState(out state) != 0) return false;
            return state == 3 || state == 4 || state == 7;
        }

        // ---- Language toggle (V11-03: calls ApplyLocalization) ----

        private void ToggleLanguage()
        {
            AppConfig.Current.IsVietnamese = !AppConfig.Current.IsVietnamese;
            AppConfig.Current.Save();
            ApplyLocalization();
            Invalidate();
        }

        // ---- Reminders & ICS ----

        private void OpenReminders()
        {
            using (var frm = new RemindersForm())
            {
                if (frm.ShowDialog(this) == DialogResult.OK) // L-05: re-check reminders after user edits them
                {
                    _lastReminderCheckDate = DateTime.MinValue;
                    CheckDailyReminders(DateTime.Now.Date);
                }
            }
        }

        private void ExportIcs()
        {
            using (var dlg = new SaveFileDialog
            {
                Title    = AppStrings.IcsExportTitle,
                Filter   = "iCalendar (*.ics)|*.ics",
                FileName = "vietnamese_calendar.ics"
            })
            {
                if (dlg.ShowDialog(this) != DialogResult.OK) return;

                int years = 5;
                using (var yearsDlg = new IcsYearsForm())
                {
                    if (yearsDlg.ShowDialog(this) == DialogResult.OK)
                        years = yearsDlg.Years;
                }

                IcsExporter.Export(dlg.FileName, AppConfig.Current.Reminders, years);
                MessageBox.Show(AppStrings.IcsExportDone, AppStrings.IcsExportTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // ---- Closing ----

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            bool isRealExit = _isExiting
                || e.CloseReason == CloseReason.WindowsShutDown
                || e.CloseReason == CloseReason.TaskManagerClosing
                || e.CloseReason == CloseReason.ApplicationExitCall;

            if (!isRealExit) { e.Cancel = true; Hide(); return; }

            bool shouldPrompt = _isExiting && !_donatePromptShown && !AppConfig.Current.SkipDonatePrompt;
            if (shouldPrompt)
            {
                _donatePromptShown = true;
                using (var donateForm = new DonateForm())
                {
                    donateForm.ShowDialog(this);
                    if (donateForm.SkipNextTime)
                    {
                        AppConfig.Current.SkipDonatePrompt = true;
                        AppConfig.Current.Save();
                    }
                }
            }
            base.OnFormClosing(e);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _closing = true;

            var cfg = AppConfig.Current;
            // L-13: use RestoreBounds when minimized so we don't save -32000; guard extreme coords
            Rectangle bounds = (WindowState == FormWindowState.Normal) ? Bounds : RestoreBounds;
            if (bounds.Left > -10000 && bounds.Top > -10000)
            {
                cfg.WindowLeft = bounds.Left;
                cfg.WindowTop  = bounds.Top;
            }
            cfg.Save();

            SystemEvents.TimeChanged     -= SystemEvents_TimeChanged;
            SystemEvents.PowerModeChanged -= SystemEvents_PowerModeChanged;
            notifyIconMain.Visible = false;
            _bgProvider?.Dispose();
            BackgroundImage?.Dispose();
            _menuShowHideFont?.Dispose();
            _countdownFont?.Dispose();
            if (_dynamicTrayIconHandle != IntPtr.Zero) DestroyIcon(_dynamicTrayIconHandle);
            base.OnFormClosed(e);
        }

        // ---- Month calendar ----

        private void OpenMonthCalendar()
        {
            if (_monthCalendarForm != null && !_monthCalendarForm.IsDisposed)
            {
                if (_monthCalendarForm.WindowState == FormWindowState.Minimized)
                    _monthCalendarForm.WindowState = FormWindowState.Normal;
                _monthCalendarForm.BringToFront();
                _monthCalendarForm.Activate();
                return;
            }
            using (_monthCalendarForm = new MonthCalendarForm(DateTime.Now, Icon))
                _monthCalendarForm.ShowDialog(this);
            _monthCalendarForm = null;
        }

        private void menuItemMonthView_Click(object sender, EventArgs e)  { OpenMonthCalendar(); }
        private void menuItemFeedback_Click(object sender, EventArgs e)   { OpenFeedbackLink(); }
        private void menuItemExit_Click(object sender, EventArgs e)       { _isExiting = true; Close(); }

        private void OpenFeedbackLink()
        {
            try { Process.Start(FeedbackUrl); } catch { }
        }
    }
}
