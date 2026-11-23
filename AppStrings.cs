using System;
using System.Windows.Forms;

namespace frm_vietnamese_calendar
{
    /// <summary>
    /// All user-visible strings, bilingual Vietnamese (default) / English.
    /// Set IsVietnamese before the first form opens; it reads from AppConfig.Current.
    /// </summary>
    public static class AppStrings
    {
        public static bool IsVietnamese
        {
            get { return AppConfig.Current.IsVietnamese; }
        }

        // ---- Day names ----
        private static readonly string[] WeekdaysVi = { "Chủ Nhật", "Thứ Hai", "Thứ Ba", "Thứ Tư", "Thứ Năm", "Thứ Sáu", "Thứ Bảy" };
        private static readonly string[] WeekdaysEn = { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
        private static readonly string[] ShortWeekdaysVi = { "CN", "T2", "T3", "T4", "T5", "T6", "T7" };
        private static readonly string[] ShortWeekdaysEn = { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };

        public static string GetWeekdayName(DayOfWeek dow)
        {
            return IsVietnamese ? WeekdaysVi[(int)dow] : WeekdaysEn[(int)dow];
        }

        public static string GetShortWeekdayName(int col) // col 0=Sun..6=Sat
        {
            return IsVietnamese ? ShortWeekdaysVi[col] : ShortWeekdaysEn[col];
        }

        // ---- Zodiac / Can Chi ----
        // Zodiac names stay Vietnamese in both languages (they are proper nouns)
        public static readonly string[] ZodiacNames = {
            "Tý", "Sửu", "Dần", "Mão", "Thìn", "Tỵ",
            "Ngọ", "Mùi", "Thân", "Dậu", "Tuất", "Hợi"
        };

        // ---- Date format prefixes ----
        public static string TodayPrefix  { get { return IsVietnamese ? "Hôm nay: " : "Today: "; } }
        public static string DatePrefix   { get { return IsVietnamese ? "Ngày " : "Date "; } }
        public static string LunarPrefix  { get { return IsVietnamese ? "Âm lịch: " : "Lunar: "; } }
        public static string YearOfPrefix { get { return IsVietnamese ? "Năm " : "Year of "; } }
        public static string LeapMonthSuffix      { get { return IsVietnamese ? " (tháng nhuận)" : " (leap month)"; } }
        public static string LeapMonthShortSuffix { get { return IsVietnamese ? " (nhuận)" : " (leap)"; } }

        // ---- Compound date formatters ----
        public static string FormatWeekday(DayOfWeek dow)
        {
            return TodayPrefix + GetWeekdayName(dow);
        }

        public static string FormatSolarDate(DateTime date)
        {
            return DatePrefix + date.ToString("dd/MM/yyyy");
        }

        public static string FormatLunarDate(DateTime date)
        {
            int ld, lm, ly; bool leap;
            LunarCalendar.ConvertSolar2Lunar(date.Day, date.Month, date.Year, out ld, out lm, out ly, out leap);
            string leapSuffix = leap ? LeapMonthSuffix : string.Empty;
            return LunarPrefix + ld + "/" + lm + leapSuffix + ", " + YearOfPrefix + LunarCalendar.GetYearCanChi(ly);
        }

        public static string FormatLunarDateShort(DateTime date)
        {
            int ld, lm, ly; bool leap;
            LunarCalendar.ConvertSolar2Lunar(date.Day, date.Month, date.Year, out ld, out lm, out ly, out leap);
            string leapSuffix = leap ? LeapMonthShortSuffix : string.Empty;
            return ld + "/" + lm + leapSuffix + " " + LunarCalendar.GetYearCanChi(ly);
        }

        public static string FormatTrayTooltip(DateTime date)
        {
            // Tray tooltip has 63-char limit on .NET Framework
            string weekday = GetWeekdayName(date.DayOfWeek);
            string lunar = FormatLunarDateShort(date);
            string candidate = weekday + " " + date.ToString("dd/MM/yyyy") + " - " + lunar;
            if (candidate.Length <= 63) return candidate;
            // Trim zodiac if still too long
            int ld, lm, ly; bool leap;
            LunarCalendar.ConvertSolar2Lunar(date.Day, date.Month, date.Year, out ld, out lm, out ly, out leap);
            string leapSuffix = leap ? " (n)" : string.Empty;
            return weekday + " " + date.ToString("dd/MM") + " - " + ld + "/" + lm + leapSuffix;
        }

        // ---- Month calendar ----
        public static string MonthCalendarTitle { get { return IsVietnamese ? "Lịch tháng" : "Month calendar"; } }
        public static string TodayButton       { get { return IsVietnamese ? "Hôm nay" : "Today"; } }

        public static string FormatMonthYear(int month, int year)
        {
            return IsVietnamese
                ? "Tháng " + month + " - " + year
                : "Month " + month + " - " + year;
        }

        // ---- Solar terms ----
        public static readonly string[] SolarTermsVi = {
            "Xuân Phân", "Thanh Minh", "Cốc Vũ", "Lập Hạ",
            "Tiểu Mãn", "Mang Chủng", "Hạ Chí", "Tiểu Thử",
            "Đại Thử", "Lập Thu", "Xử Thử", "Bạch Lộ",
            "Thu Phân", "Hàn Lộ", "Sương Giáng", "Lập Đông",
            "Tiểu Tuyết", "Đại Tuyết", "Đông Chí", "Tiểu Hàn",
            "Đại Hàn", "Lập Xuân", "Vũ Thủy", "Kinh Trập"
        };
        public static readonly string[] SolarTermsEn = {
            "Vernal Equinox", "Clear & Bright", "Grain Rain", "Start of Summer",
            "Grain Buds", "Grain in Ear", "Summer Solstice", "Minor Heat",
            "Major Heat", "Start of Autumn", "End of Heat", "White Dew",
            "Autumnal Equinox", "Cold Dew", "Frost's Descent", "Start of Winter",
            "Minor Snow", "Major Snow", "Winter Solstice", "Minor Cold",
            "Major Cold", "Start of Spring", "Rain Water", "Awakening of Insects"
        };

        public static string GetSolarTermName(int index)
        {
            if (index < 0 || index >= 24) return string.Empty;
            return IsVietnamese ? SolarTermsVi[index] : SolarTermsEn[index];
        }

        // ---- Auspicious hours ----
        public static string AuspiciousHoursHeader { get { return IsVietnamese ? "Giờ Hoàng Đạo" : "Auspicious Hours"; } }
        public static string Auspicious   { get { return IsVietnamese ? "Hoàng Đạo" : "Auspicious"; } }
        public static string Inauspicious { get { return IsVietnamese ? "Hắc Đạo" : "Inauspicious"; } }

        // Hour branch time ranges
        public static readonly string[] HourBranchTimes = {
            "Tý (23-1)", "Sửu (1-3)", "Dần (3-5)", "Mão (5-7)",
            "Thìn (7-9)", "Tỵ (9-11)", "Ngọ (11-13)", "Mùi (13-15)",
            "Thân (15-17)", "Dậu (17-19)", "Tuất (19-21)", "Hợi (21-23)"
        };

        // ---- Five elements ----
        public static readonly string[] FiveElementsVi = { "Kim", "Mộc", "Thủy", "Hỏa", "Thổ" };
        public static readonly string[] FiveElementsEn = { "Metal", "Wood", "Water", "Fire", "Earth" };

        // ---- Detail panel labels ----
        public static string LabelCanChi         { get { return IsVietnamese ? "Can Chi" : "Can Chi"; } }
        public static string LabelSolarTerm      { get { return IsVietnamese ? "Tiết Khí" : "Solar Term"; } }
        public static string LabelNapAm          { get { return IsVietnamese ? "Nạp Âm" : "Nap Am"; } }
        public static string LabelConflict       { get { return IsVietnamese ? "Tuổi Xung" : "Conflicts"; } }
        public static string LabelAuspiciousHours { get { return AuspiciousHoursHeader; } }
        public static string LabelHoliday        { get { return IsVietnamese ? "Ngày Lễ" : "Holiday"; } }

        // ---- Conflict display ----
        public static string FormatConflictBranch(string branchName)
        {
            return IsVietnamese
                ? "Xung với tuổi " + branchName
                : "Conflicts with " + branchName;
        }

        // ---- Countdown ----
        public static string FormatCountdown(string holidayName, int daysLeft)
        {
            if (daysLeft == 0)
                return IsVietnamese ? "Hôm nay: " + holidayName : "Today: " + holidayName;
            return IsVietnamese
                ? daysLeft + " ngày nữa: " + holidayName
                : daysLeft + " days to: " + holidayName;
        }

        // ---- Reminders ----
        public static string RemindersTitle  { get { return IsVietnamese ? "Nhắc nhở" : "Reminders"; } }
        public static string ReminderAdd     { get { return IsVietnamese ? "Thêm" : "Add"; } }
        public static string ReminderEdit    { get { return IsVietnamese ? "Sửa" : "Edit"; } }
        public static string ReminderDelete  { get { return IsVietnamese ? "Xóa" : "Delete"; } }
        public static string ReminderName    { get { return IsVietnamese ? "Tên sự kiện" : "Event name"; } }
        public static string ReminderDate    { get { return IsVietnamese ? "Ngày/tháng" : "Day/Month"; } }
        public static string ReminderType    { get { return IsVietnamese ? "Loại" : "Type"; } }
        public static string ReminderLunar   { get { return IsVietnamese ? "Âm lịch" : "Lunar"; } }
        public static string ReminderSolar   { get { return IsVietnamese ? "Dương lịch" : "Solar"; } }
        public static string ReminderDaysBefore { get { return IsVietnamese ? "Nhắc trước (ngày)" : "Notify before (days)"; } }

        // ---- ICS export ----
        public static string IcsExportTitle  { get { return IsVietnamese ? "Xuất lịch .ics" : "Export .ics calendar"; } }
        public static string IcsExportYears  { get { return IsVietnamese ? "Số năm xuất (1-10)" : "Years to export (1-10)"; } }
        public static string IcsExportDone   { get { return IsVietnamese ? "Đã xuất thành công!" : "Export successful!"; } }

        // ---- Context menu ----
        public static string MenuShowHide      { get { return IsVietnamese ? "Hiện / Ẩn lịch" : "Show / Hide calendar"; } }
        public static string MenuMonthView     { get { return IsVietnamese ? "Xem lịch tháng" : "View month calendar"; } }
        public static string MenuAlwaysOnTop   { get { return IsVietnamese ? "Luôn trên cùng" : "Always on Top"; } }
        public static string MenuOpacity       { get { return IsVietnamese ? "Độ trong suốt" : "Opacity"; } }
        public static string MenuLockPosition  { get { return IsVietnamese ? "Khóa vị trí" : "Lock Position"; } }
        public static string MenuClickThrough  { get { return IsVietnamese ? "Nhấp xuyên" : "Click Through"; } }
        public static string MenuCopyDate      { get { return IsVietnamese ? "Sao chép ngày" : "Copy Date"; } }
        public static string MenuBackground    { get { return IsVietnamese ? "Ảnh nền" : "Background"; } }
        public static string MenuBgNext        { get { return IsVietnamese ? "Ảnh tiếp theo" : "Next Image"; } }
        public static string MenuBgPause       { get { return IsVietnamese ? "Tạm dừng" : "Pause"; } }
        public static string MenuBgResume      { get { return IsVietnamese ? "Tiếp tục" : "Resume"; } }
        public static string MenuBgSolid       { get { return IsVietnamese ? "Nền tối đặc" : "Solid Dark"; } }
        public static string MenuBgInterval    { get { return IsVietnamese ? "Khoảng thời gian" : "Change interval"; } }
        public static string MenuTheme         { get { return IsVietnamese ? "Giao diện" : "Theme"; } }
        public static string MenuThemePhoto    { get { return IsVietnamese ? "Ảnh nền" : "Photo"; } }
        public static string MenuThemeBloc     { get { return IsVietnamese ? "Lịch tờ" : "Bloc Calendar"; } }
        public static string MenuReminders     { get { return IsVietnamese ? "Nhắc nhở..." : "Reminders..."; } }
        public static string MenuExportIcs     { get { return IsVietnamese ? "Xuất .ics..." : "Export .ics..."; } }
        public static string MenuStartWindows  { get { return IsVietnamese ? "Khởi động cùng Windows" : "Start with Windows"; } }
        public static string MenuHotkeys       { get { return IsVietnamese ? "Đổi phím tắt..." : "Change hotkeys..."; } }
        public static string MenuCheckUpdate   { get { return IsVietnamese ? "Kiểm tra cập nhật..." : "Check for updates..."; } }
        public static string MenuAbout        { get { return IsVietnamese ? "Giới thiệu" : "About"; } }
        public static string MenuFeedback      { get { return IsVietnamese ? "Phản hồi" : "Feedback"; } }
        public static string MenuLanguage      { get { return IsVietnamese ? "English" : "Tiếng Việt"; } }
        public static string MenuFullscreenHide { get { return IsVietnamese ? "Ẩn khi toàn màn hình" : "Hide when fullscreen"; } }
        public static string MenuWeekNumbers   { get { return IsVietnamese ? "Hiện số tuần" : "Show week numbers"; } }
        public static string MenuExit          { get { return IsVietnamese ? "Thoát" : "Exit"; } }

        // ---- About / Update ----
        public static string AboutTitle       { get { return IsVietnamese ? "Giới thiệu" : "About"; } }
        public static string AboutBody        { get { return IsVietnamese
            ? "MiniPerpetualCalendar v" + UpdateChecker.CurrentVersion + "\n\nLịch Âm – Dương trên desktop Windows.\nThuật toán lịch âm: Hồ Ngọc Đức\n\nhttps://github.com/ThanhNguyenVN93/mini-perpetual-calendar"
            : "MiniPerpetualCalendar v" + UpdateChecker.CurrentVersion + "\n\nVietnamese lunar/solar calendar widget for Windows.\nLunar algorithm by Ho Ngoc Duc\n\nhttps://github.com/ThanhNguyenVN93/mini-perpetual-calendar"; } }
        public static string UpdateChecking      { get { return IsVietnamese ? "Đang kiểm tra..." : "Checking..."; } }
        public static string UpdateUpToDate      { get { return IsVietnamese ? "Bạn đang dùng phiên bản mới nhất." : "You're on the latest version."; } }
        public static string UpdateAvailableAuto { get { return IsVietnamese ? "Có phiên bản mới! Bấm vào đây để tải xuống." : "New version available! Click to download."; } }
        public static string UpdateNowPrompt     { get { return IsVietnamese
            ? "Có phiên bản mới!\n\nBấm 'Yes' để tự động cập nhật và khởi động lại.\nBấm 'No' để mở trang tải xuống."
            : "A new version is available!\n\nClick 'Yes' to update automatically and restart.\nClick 'No' to open the download page."; } }
        public static string UpdateDownloading   { get { return IsVietnamese ? "Đang tải xuống bản cập nhật, vui lòng đợi..." : "Downloading update, please wait..."; } }

        // ---- Hotkey balloon ----
        public static string HotkeyConflictTitle { get { return IsVietnamese ? "Xung đột phím tắt" : "Hotkey conflict"; } }
        public static string HotkeyConflictBody  { get { return IsVietnamese
            ? "Một hoặc nhiều phím tắt không thể đăng ký. Vào 'Đổi phím tắt...' để chọn tổ hợp khác."
            : "One or more hotkeys could not be registered (already in use). Open 'Change hotkeys...' to assign a different combo."; } }

        // ---- Single instance ----
        public static string AlreadyRunningTitle { get { return "Mini Perpetual Calendar"; } }
        public static string AlreadyRunningBody  { get { return IsVietnamese
            ? "Mini Perpetual Calendar đang chạy."
            : "Mini Perpetual Calendar is already running."; } }

        // ---- ShortcutsHelpForm (V11-08) ----
        public static string ShortcutsHelpTitle  { get { return IsVietnamese ? "Hướng dẫn sử dụng" : "Shortcuts & Tips"; } }

        public static string BuildShortcutsContent(string toggleHotkey, string monthHotkey)
        {
            if (IsVietnamese)
                return
                    "•  Kéo bằng chuột trái: di chuyển cửa sổ\n\n" +
                    "•  Nhấp đúp / chuột phải > Xem lịch tháng: mở lịch tháng\n\n" +
                    "•  Chuột phải: mở menu (lịch tháng, khởi động cùng Windows,\n" +
                    "   đổi phím tắt, phản hồi, thoát)\n\n" +
                    "•  ESC / nút ×: ẩn xuống khay hệ thống (không thoát)\n\n" +
                    "•  Nhấp / nhấp đúp vào biểu tượng khay: khôi phục cửa sổ\n\n" +
                    "•  " + toggleHotkey + ": hiện/ẩn cửa sổ (từ bất kỳ đâu)\n\n" +
                    "•  " + monthHotkey + ": mở lịch tháng (từ bất kỳ đâu)\n\n" +
                    "•  Chuột phải > Đổi phím tắt...: tùy chỉnh 2 phím tắt trên";
            else
                return
                    "•  Left-drag: move the window\n\n" +
                    "•  Double-click / right-click > View month calendar: opens the month calendar\n\n" +
                    "•  Right-click: opens the menu (month calendar, start with\n" +
                    "   Windows, change hotkeys, feedback, exit)\n\n" +
                    "•  ESC / × button: hides to the system tray (does not exit)\n\n" +
                    "•  Click / double-click the tray icon: restores the window\n\n" +
                    "•  " + toggleHotkey + ": show/hide the window (from anywhere)\n\n" +
                    "•  " + monthHotkey + ": open the month calendar (from anywhere)\n\n" +
                    "•  Right-click > Change hotkeys...: customize the 2 combos above";
        }

        // ---- Background interval labels (L-07) ----
        public static string FormatBgInterval(int secs)
        {
            if (secs < 60) return IsVietnamese ? secs + " giây" : secs + "s";
            int mins = secs / 60;
            return IsVietnamese ? mins + " phút" : mins + " min";
        }

        // ---- HotkeySettingsForm labels (L-07) ----
        public static string HotkeySettingsTitle  { get { return MenuHotkeys; } }
        public static string HotkeyToggleCaption  { get { return IsVietnamese ? "Hiện/Ẩn cửa sổ:"   : "Show/hide window:"; } }
        public static string HotkeyMonthCaption   { get { return IsVietnamese ? "Mở lịch tháng:"    : "Open month calendar:"; } }
        public static string HotkeyHint           { get { return IsVietnamese
            ? "Nhấp vào ô rồi nhấn tổ hợp phím mới. Yêu cầu Ctrl/Alt/Shift + 1 phím khác. Nhấn Esc để hủy."
            : "Click a field then press the new key combo. Requires Ctrl/Alt/Shift plus 1 other key. Press Esc to cancel."; } }
        public static string HotkeySave           { get { return IsVietnamese ? "Lưu"   : "Save"; } }
        public static string HotkeyCancel         { get { return IsVietnamese ? "Hủy"   : "Cancel"; } }

        // ---- HotkeySettingsForm (V11-08) ----
        public static string HotkeyPressCombo    { get { return IsVietnamese ? "Nhấn tổ hợp phím..." : "Press a key combo..."; } }
        public static string HotkeyRequireModifier { get { return IsVietnamese
            ? "Cần ít nhất 1 phím bổ trợ (Ctrl/Alt/Shift)."
            : "At least 1 modifier key (Ctrl/Alt/Shift) is required."; } }
        public static string HotkeyReserved      { get { return IsVietnamese
            ? "Tổ hợp này được hệ thống bảo lưu. Vui lòng chọn tổ hợp khác."
            : "This key combo is reserved by the system. Choose a different one."; } }
        public static string HotkeyDuplicate     { get { return IsVietnamese
            ? "Hai phím tắt không được giống nhau."
            : "The two hotkeys must not be the same."; } }
        public static string HotkeyConflictApp   { get { return IsVietnamese
            ? "Tổ hợp này đã được app khác sử dụng. Vui lòng chọn tổ hợp khác."
            : "This key combo is already used by another app. Please choose a different one."; } }

        // ---- DonateForm (V11-08) ----
        public static string DonateTitle         { get { return IsVietnamese ? "Ủng hộ tác giả" : "Support the author"; } }
        public static string DonateHeading       { get { return IsVietnamese
            ? "Cảm ơn bạn đã sử dụng Mini Perpetual Calendar!"
            : "Thank you for using Mini Perpetual Calendar!"; } }
        public static string DonateMessage       { get { return IsVietnamese
            ? "Ứng dụng này được phát triển miễn phí, phi lợi nhuận. Nếu bạn thấy hữu ích, bạn có thể mời tác giả một ly cà phê để có thêm động lực duy trì và cải thiện."
            : "This app is developed for free, non-profit. If you find it useful, you can buy me a coffee for extra motivation to keep maintaining and improving it."; } }
        public static string DonateEncourage     { get { return IsVietnamese
            ? "Mỗi sự ủng hộ đều rất có ý nghĩa!"
            : "Every bit of support means a lot!"; } }
        public static string DonateHint          { get { return IsVietnamese
            ? "Nhấn 'Có' để xem mã QR quyên góp."
            : "Click 'Yes' to see the donation QR codes."; } }
        public static string DonateDontShow      { get { return IsVietnamese ? "Không hiện lại" : "Don't show this again"; } }
        public static string DonateYes           { get { return IsVietnamese ? "Có" : "Yes"; } }
        public static string DonateNo            { get { return IsVietnamese ? "Không" : "No"; } }
        public static string DonateThanks        { get { return IsVietnamese
            ? "Cảm ơn rất nhiều! Đóng cửa sổ này để tiếp tục."
            : "Thank you so much! Close this window to continue."; } }
    }
}
