using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace frm_vietnamese_calendar
{
    /// <summary>
    /// Full-month calendar with lunar dates, detail panel, week numbers, and keyboard navigation.
    /// </summary>
    public partial class MonthCalendarForm : Form
    {
        private const int MinYear = 1900;
        private const int MaxYear = 2100;

        private readonly Font _dayFontRegular = new Font("Segoe UI", 10.5F, FontStyle.Regular);
        private readonly Font _dayFontToday   = new Font("Segoe UI", 10.5F, FontStyle.Bold);
        private readonly Font _lunarFont      = new Font("Segoe UI", 7.5F,  FontStyle.Regular);
        private readonly Font _headerFont     = new Font("Segoe UI", 9.5F,  FontStyle.Bold);
        private readonly Font _weekNumFont    = new Font("Segoe UI", 7.5F,  FontStyle.Regular);
        private readonly Font _detailFont     = new Font("Segoe UI", 8.5F,  FontStyle.Regular);
        private readonly Font _detailBoldFont = new Font("Segoe UI", 8.5F,  FontStyle.Bold);

        private int _year;
        private int _month;
        private bool _showWeekNumbers;
        private DateTime _selectedDate;

        // V11-06: track selected cell so we avoid full RenderMonth on day click
        private Panel _selectedCellPanel;
        private bool  _selectedCellIsToday;

        private TableLayoutPanel _tblCalendar;

        // F-07: detail panel
        private Panel _pnlDetail;
        private Label _lblDetailCanChi;
        private Label _lblDetailSolarTerm;
        private Label _lblDetailNapAm;
        private Label _lblDetailConflict;
        private Label _lblDetailAuspicious;
        private Label _lblDetailHoliday;

        public MonthCalendarForm(DateTime initialDate, Icon appIcon)
        {
            InitializeComponent();
            DoubleBuffered = true;

            if (appIcon != null) Icon = appIcon;

            _year          = Clamp(initialDate.Year, MinYear, MaxYear);
            _month         = initialDate.Month;
            _selectedDate  = initialDate.Date;
            _showWeekNumbers = AppConfig.Current.ShowWeekNumbers;

            // F-07: build detail panel before BuildGridSkeleton so dock ordering is correct
            BuildDetailPanel();

            // Expand form height to accommodate detail panel
            ClientSize = new Size(560, 570);

            // F-08: click on month/year label to jump
            lblMonthYear.Cursor = Cursors.Hand;
            lblMonthYear.Click += LblMonthYear_Click;

            // F-13: mouse wheel
            MouseWheel += MonthCalendarForm_MouseWheel;

            BuildGridSkeleton();
            RenderMonth();
        }

        // ---- Detail panel (F-07) ----

        private void BuildDetailPanel()
        {
            _pnlDetail = new Panel
            {
                Dock      = DockStyle.Bottom,
                Height    = 102, // L-03: 3 rows — extra height for auspicious-hours row
                BackColor = Color.FromArgb(248, 248, 252),
                Padding   = new Padding(8, 4, 8, 4),
            };

            // L-03: 3-column × 3-row TableLayout; row 3 spans all cols for auspicious hours
            var tbl = new TableLayoutPanel
            {
                Dock        = DockStyle.Fill,
                ColumnCount = 3,
                RowCount    = 3,
                BackColor   = Color.Transparent,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
            };
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3f));
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3f));
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.4f));
            tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 34f));
            tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 33f));
            tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 33f));

            _lblDetailCanChi      = MakeDetailLabel();
            _lblDetailSolarTerm   = MakeDetailLabel();
            _lblDetailNapAm       = MakeDetailLabel();
            _lblDetailConflict    = MakeDetailLabel();
            _lblDetailHoliday     = MakeDetailLabel();
            _lblDetailHoliday.ForeColor = Color.FromArgb(180, 20, 20);
            _lblDetailAuspicious  = MakeDetailLabel();

            tbl.Controls.Add(_lblDetailCanChi,     0, 0);
            tbl.Controls.Add(_lblDetailSolarTerm,  1, 0);
            tbl.Controls.Add(_lblDetailNapAm,      2, 0);
            tbl.Controls.Add(_lblDetailConflict,   0, 1);
            tbl.Controls.Add(_lblDetailHoliday,    1, 1);
            tbl.SetColumnSpan(_lblDetailHoliday, 2);
            // L-03: auspicious hours span the full width of row 2 so text never truncates
            tbl.Controls.Add(_lblDetailAuspicious, 0, 2);
            tbl.SetColumnSpan(_lblDetailAuspicious, 3);

            var sep = new Panel { Dock = DockStyle.Top, Height = 1, BackColor = Color.FromArgb(210, 210, 210) };
            _pnlDetail.Controls.Add(tbl);
            _pnlDetail.Controls.Add(sep);

            Controls.Add(_pnlDetail);
        }

        private Label MakeDetailLabel()
        {
            return new Label
            {
                Dock      = DockStyle.Fill,
                Font      = _detailFont,
                ForeColor = Color.FromArgb(50, 50, 80),
                TextAlign = ContentAlignment.MiddleLeft,
                AutoEllipsis = true,
            };
        }

        private void ShowDetail(DateTime date)
        {
            int ld, lm, ly; bool leap;
            LunarCalendar.ConvertSolar2Lunar(date.Day, date.Month, date.Year, out ld, out lm, out ly, out leap);

            // Can Chi
            string dayCC  = LunarCalendar.GetDayCanChi(date);
            string monCC  = LunarCalendar.GetMonthCanChi(lm, ly);
            string yearCC = LunarCalendar.GetYearCanChi(ly);
            // L-11: prefix with the date being described so the user always knows which day the panel refers to
            string datePfx = date.ToString("dd/MM") + " (" + ld + "/" + lm + (leap ? "*" : "") + ") • ";
            _lblDetailCanChi.Text = datePfx + dayCC + " / " + monCC + " / " + yearCC;

            // Solar term
            int termIdx = LunarCalendar.GetSolarTermIndex(date);
            string term = termIdx >= 0 ? AppStrings.GetSolarTermName(termIdx) : "-";
            _lblDetailSolarTerm.Text = AppStrings.LabelSolarTerm + ": " + term;

            // Nap Am — V11-06: day-level instead of year-level
            string napAm = LunarCalendar.GetDayNapAmName(date);
            _lblDetailNapAm.Text = AppStrings.LabelNapAm + ": " + napAm;

            // Conflict (Tuổi Xung) — V11-06: day-level
            string conflict = LunarCalendar.GetDayConflictingBranchName(date);
            _lblDetailConflict.Text = AppStrings.FormatConflictBranch(conflict);

            // Auspicious hours
            bool[] auspicious = LunarCalendar.GetAuspiciousHours(date);
            string hoursText = "";
            for (int i = 0; i < 12; i++)
            {
                if (auspicious[i])
                    hoursText += (hoursText.Length > 0 ? ", " : "") + AppStrings.HourBranchTimes[i];
            }
            _lblDetailAuspicious.Text = AppStrings.AuspiciousHoursHeader + ": " + (hoursText.Length > 0 ? hoursText : "-");

            // Holiday
            string holiday = VietnameseHolidays.GetHolidayName(date);
            _lblDetailHoliday.Text = holiday != null ? (AppStrings.LabelHoliday + ": " + holiday) : string.Empty;
        }

        // ---- Grid skeleton (F-25 week numbers) ----

        private void BuildGridSkeleton()
        {
            int cols = _showWeekNumbers ? 8 : 7;

            _tblCalendar = new TableLayoutPanel
            {
                Dock            = DockStyle.Fill,
                ColumnCount     = cols,
                RowCount        = 7,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
            };

            if (_showWeekNumbers)
            {
                _tblCalendar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 32f));
                for (int c = 0; c < 7; c++)
                    _tblCalendar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / 7f));
            }
            else
            {
                for (int c = 0; c < 7; c++)
                    _tblCalendar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / 7f));
            }

            _tblCalendar.RowStyles.Add(new RowStyle(SizeType.Absolute, 28f));
            for (int r = 0; r < 6; r++)
                _tblCalendar.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / 6f));

            // Week-number column header
            if (_showWeekNumbers)
            {
                var lblWkHdr = new Label
                {
                    Text      = "W#",
                    Dock      = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font      = _weekNumFont,
                    BackColor = Color.FromArgb(230, 230, 240),
                    ForeColor = Color.FromArgb(100, 100, 130),
                };
                _tblCalendar.Controls.Add(lblWkHdr, 0, 0);
            }

            // Day-of-week headers
            for (int c = 0; c < 7; c++)
            {
                int dayCol = _showWeekNumbers ? c + 1 : c;
                var lbl = new Label
                {
                    Text      = AppStrings.GetShortWeekdayName(c),
                    Dock      = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font      = _headerFont,
                    BackColor = Color.FromArgb(240, 240, 240),
                    ForeColor = c == 0 ? Color.FromArgb(178, 34, 34) : Color.FromArgb(60, 60, 60),
                };
                _tblCalendar.Controls.Add(lbl, dayCol, 0);
            }

            pnlGrid.Controls.Add(_tblCalendar);
        }

        // ---- Month rendering ----

        private void RenderMonth()
        {
            lblMonthYear.Text  = AppStrings.FormatMonthYear(_month, _year);
            btnToday.Text      = AppStrings.TodayButton;
            Text               = AppStrings.MonthCalendarTitle;
            UpdateNavButtons();

            _tblCalendar.SuspendLayout();
            for (int i = _tblCalendar.Controls.Count - 1; i >= 0; i--)
            {
                Control ctrl = _tblCalendar.Controls[i];
                if (_tblCalendar.GetRow(ctrl) > 0)
                {
                    _tblCalendar.Controls.Remove(ctrl);
                    ctrl.Dispose();
                }
            }

            DateTime firstOfMonth = new DateTime(_year, _month, 1);
            int startDow = (int)firstOfMonth.DayOfWeek; // 0=Sun
            DateTime today = DateTime.Now.Date;

            DateTime cellDate;
            if (startDow == 0)
                cellDate = firstOfMonth;
            else
            {
                try { cellDate = firstOfMonth.AddDays(-startDow); }
                catch { cellDate = firstOfMonth; startDow = 0; }
            }

            var cal = CultureInfo.InvariantCulture.Calendar;

            for (int row = 1; row <= 6; row++)
            {
                // Week number column (F-25)
                if (_showWeekNumbers)
                {
                    int wn = cal.GetWeekOfYear(cellDate, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                    var lblWk = new Label
                    {
                        Text      = wn.ToString(),
                        Dock      = DockStyle.Fill,
                        TextAlign = ContentAlignment.MiddleCenter,
                        Font      = _weekNumFont,
                        BackColor = Color.FromArgb(240, 240, 248),
                        ForeColor = Color.FromArgb(100, 100, 130),
                    };
                    _tblCalendar.Controls.Add(lblWk, 0, row);
                }

                for (int col = 0; col < 7; col++)
                {
                    int gridCol = _showWeekNumbers ? col + 1 : col;

                    if (cellDate > DateTime.MaxValue.Date)
                    {
                        _tblCalendar.Controls.Add(
                            new Panel { Dock = DockStyle.Fill, BackColor = Color.White }, gridCol, row);
                        continue;
                    }

                    bool inCurrentMonth = cellDate.Month == _month && cellDate.Year == _year;
                    bool isToday        = cellDate.Date == today;
                    bool isSelected     = cellDate.Date == _selectedDate.Date;
                    var cell = BuildDayCell(cellDate, inCurrentMonth, isToday, isSelected);
                    _tblCalendar.Controls.Add(cell, gridCol, row);

                    try { cellDate = cellDate.AddDays(1); }
                    catch { cellDate = DateTime.MaxValue.Date; }
                }
            }
            _tblCalendar.ResumeLayout(true);

            // Refresh detail panel for selected date
            ShowDetail(_selectedDate);
        }

        private void UpdateNavButtons()
        {
            btnPrev.Enabled = !(_year == MinYear && _month == 1);
            btnNext.Enabled = !(_year == MaxYear && _month == 12);
        }

        private Panel BuildDayCell(DateTime date, bool inCurrentMonth, bool isToday, bool isSelected)
        {
            Color bg = isToday    ? Color.FromArgb(255, 230, 150)
                     : isSelected ? Color.FromArgb(200, 220, 255)
                     :              Color.White;

            var cell = new Panel { Dock = DockStyle.Fill, Margin = new Padding(0), BackColor = bg };

            var lblDay = new Label
            {
                Text      = date.Day.ToString(),
                Dock      = DockStyle.Top,
                Height    = 22,
                TextAlign = ContentAlignment.MiddleCenter,
                Font      = isToday ? _dayFontToday : _dayFontRegular,
                ForeColor = inCurrentMonth ? Color.FromArgb(30, 30, 30) : Color.FromArgb(190, 190, 190),
            };

            int ld, lm, ly; bool leap;
            LunarCalendar.ConvertSolar2Lunar(date.Day, date.Month, date.Year, out ld, out lm, out ly, out leap);
            string leapMark = leap ? "*" : string.Empty;
            string lunarText = ld == 1 ? ld + "/" + lm + leapMark : ld.ToString();

            // Show holiday indicator (red dot or bold)
            string holiday = VietnameseHolidays.GetHolidayName(date);
            Color lunarColor = holiday != null
                ? Color.FromArgb(200, 0, 0)
                : (inCurrentMonth ? Color.FromArgb(178, 34, 34) : Color.FromArgb(220, 180, 180));

            if (holiday != null)
                lunarText += " •";

            var lblLunar = new Label
            {
                Text      = lunarText,
                Dock      = DockStyle.Fill,
                TextAlign = ContentAlignment.TopCenter,
                Font      = _lunarFont,
                ForeColor = lunarColor,
            };

            // F-07: click on day to show detail
            // V11-06: update only the two panels' colors instead of full RenderMonth
            DateTime clickDate  = date;
            bool     cellIsToday = isToday;
            EventHandler onClick = (s, e) =>
            {
                // Restore the previously selected cell's color
                if (_selectedCellPanel != null && !_selectedCellPanel.IsDisposed)
                    _selectedCellPanel.BackColor = _selectedCellIsToday
                        ? Color.FromArgb(255, 230, 150)
                        : Color.White;

                _selectedDate         = clickDate;
                _selectedCellPanel    = cell;
                _selectedCellIsToday  = cellIsToday;
                cell.BackColor        = Color.FromArgb(200, 220, 255);
                ShowDetail(clickDate);
            };
            cell.Click      += onClick;
            lblDay.Click    += onClick;
            lblLunar.Click  += onClick;

            if (isSelected)
            {
                _selectedCellPanel   = cell;
                _selectedCellIsToday = isToday;
            }

            cell.Controls.Add(lblLunar);
            cell.Controls.Add(lblDay);
            return cell;
        }

        // ---- Navigation ----

        private void btnPrev_Click(object sender, EventArgs e)  { ChangeMonth(-1); }
        private void btnNext_Click(object sender, EventArgs e)  { ChangeMonth(+1); }
        private void btnToday_Click(object sender, EventArgs e)
        {
            _year  = Clamp(DateTime.Now.Year,  MinYear, MaxYear);
            _month = DateTime.Now.Month;
            _selectedDate = DateTime.Now.Date;
            RenderMonth();
        }

        private void ChangeMonth(int delta)
        {
            int m = _month + delta, y = _year;
            if (m < 1)  { m = 12; y--; }
            if (m > 12) { m = 1;  y++; }
            if (y < MinYear) { y = MinYear; m = 1; }
            if (y > MaxYear) { y = MaxYear; m = 12; }
            _month = m; _year = y;
            // L-11: keep _selectedDate in-month so the detail panel reflects the new month
            int maxDay = DateTime.DaysInMonth(_year, _month);
            _selectedDate = new DateTime(_year, _month, Math.Min(_selectedDate.Day, maxDay));
            RenderMonth();
        }

        private void ChangeYear(int delta)
        {
            int y = Clamp(_year + delta, MinYear, MaxYear);
            if (y == _year) return;
            _year = y;
            // L-11: keep _selectedDate in-month
            int maxDay = DateTime.DaysInMonth(_year, _month);
            _selectedDate = new DateTime(_year, _month, Math.Min(_selectedDate.Day, maxDay));
            RenderMonth();
        }

        // ---- F-08: jump to month/year ----

        private void LblMonthYear_Click(object sender, EventArgs e)
        {
            using (var dlg = new JumpToMonthForm(_month, _year))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    DateTime target = dlg.SelectedDate.Date; // R-02: .Date to strip time component
                    _selectedDate = target;                   // R-02: set before RenderMonth so highlight is correct
                    _month = target.Month;
                    _year  = Clamp(target.Year, MinYear, MaxYear);
                    RenderMonth();
                }
            }
        }

        // ---- F-13: keyboard navigation (V11-02: ProcessCmdKey captures arrows before Button controls) ----

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Escape:                     Close(); return true;
                case Keys.Left:                       ChangeMonth(-1); return true;
                case Keys.Control | Keys.Left:
                case Keys.Up:                         ChangeYear(-1); return true;
                case Keys.Right:                      ChangeMonth(+1); return true;
                case Keys.Control | Keys.Right:
                case Keys.Down:                       ChangeYear(+1); return true;
                case Keys.T:                          btnToday_Click(this, EventArgs.Empty); return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        // F-13: mouse wheel scrolls months
        private void MonthCalendarForm_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0) ChangeMonth(-1);
            else             ChangeMonth(+1);
        }

        // ---- Utilities ----

        private static int Clamp(int value, int min, int max)
        {
            return value < min ? min : value > max ? max : value;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dayFontRegular?.Dispose();
                _dayFontToday?.Dispose();
                _lunarFont?.Dispose();
                _headerFont?.Dispose();
                _weekNumFont?.Dispose();
                _detailFont?.Dispose();
                _detailBoldFont?.Dispose();
                components?.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    // ---- F-08: jump-to-month dialog (L-04: added day + solar/lunar mode) ----

    internal class JumpToMonthForm : Form
    {
        public DateTime SelectedDate  { get; private set; }
        public int      SelectedMonth { get { return SelectedDate.Month; } }
        public int      SelectedYear  { get { return SelectedDate.Year;  } }

        public JumpToMonthForm(int currentMonth, int currentYear)
        {
            bool vi         = AppStrings.IsVietnamese;
            Text            = vi ? "Đến ngày..." : "Go to date...";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            ClientSize      = new Size(260, 160);
            StartPosition   = FormStartPosition.CenterParent;
            MaximizeBox     = false;
            MinimizeBox     = false;

            // L-04: solar/lunar mode radio buttons
            var rdoSolar = new RadioButton { Text = vi ? "Dương lịch" : "Solar",  Location = new Point(10, 10), Size = new Size(110, 22), Checked = true };
            var rdoLunar = new RadioButton { Text = vi ? "Âm lịch"   : "Lunar",  Location = new Point(130, 10), Size = new Size(110, 22) };

            var lblDay   = new Label { Text = vi ? "Ngày:" : "Day:",   Location = new Point(10, 42),  Size = new Size(55, 23), TextAlign = ContentAlignment.MiddleLeft };
            var nudDay   = new NumericUpDown { Location = new Point(70, 39),  Size = new Size(50, 23), Minimum = 1, Maximum = 31, Value = DateTime.Now.Day };
            var lblMonth = new Label { Text = vi ? "Tháng:" : "Month:", Location = new Point(10, 74),  Size = new Size(55, 23), TextAlign = ContentAlignment.MiddleLeft };
            var nudMonth = new NumericUpDown { Location = new Point(70, 71),  Size = new Size(50, 23), Minimum = 1, Maximum = 12, Value = currentMonth };
            var lblYear  = new Label { Text = vi ? "Năm:" : "Year:",   Location = new Point(10, 106), Size = new Size(55, 23), TextAlign = ContentAlignment.MiddleLeft };
            var nudYear  = new NumericUpDown { Location = new Point(70, 103), Size = new Size(70, 23), Minimum = 1900, Maximum = 2100, Value = currentYear };
            var btnOk    = new Button { Text = "OK",                    Location = new Point(170, 125), Size = new Size(75, 25) };
            var lblError = new Label { ForeColor = Color.Red, Location = new Point(10, 128), Size = new Size(155, 20), Font = new Font("Segoe UI", 8f) };

            rdoLunar.CheckedChanged += (s, e) => { nudDay.Maximum = rdoLunar.Checked ? 30 : 31; };

            AcceptButton = btnOk;
            Controls.AddRange(new Control[] { rdoSolar, rdoLunar, lblDay, nudDay, lblMonth, nudMonth, lblYear, nudYear, btnOk, lblError });

            btnOk.Click += (s, e) =>
            {
                int d = (int)nudDay.Value, m = (int)nudMonth.Value, y = (int)nudYear.Value;
                if (rdoLunar.Checked)
                {
                    try
                    {
                        DateTime solar = LunarCalendar.ConvertLunar2Solar(d, m, y, false);
                        SelectedDate   = solar;
                        DialogResult   = DialogResult.OK;
                        Close();
                    }
                    catch
                    {
                        lblError.Text = vi ? "Ngày âm không hợp lệ" : "Invalid lunar date";
                    }
                }
                else
                {
                    try
                    {
                        SelectedDate = new DateTime(y, m, d);
                        DialogResult = DialogResult.OK;
                        Close();
                    }
                    catch
                    {
                        lblError.Text = vi ? "Ngày dương không hợp lệ" : "Invalid solar date";
                    }
                }
            };

            SelectedDate = new DateTime(currentYear, currentMonth, 1);
        }
    }
}
