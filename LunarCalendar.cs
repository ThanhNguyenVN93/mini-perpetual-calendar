using System;

namespace frm_vietnamese_calendar
{
    /// <summary>
    /// Converts a solar (Gregorian) date to the Vietnamese lunar calendar using Ho Ngoc Duc's algorithm.
    /// Calculations are based on GMT+7 (105° E meridian). Has no UI dependency.
    /// </summary>
    public static class LunarCalendar
    {
        private const double PI = Math.PI;
        private const int TimeZone = 7;

        private static readonly string[] ZodiacAnimals = { "Rat", "Buffalo", "Tiger", "Cat", "Dragon", "Snake", "Horse", "Goat", "Monkey", "Rooster", "Dog", "Pig" };
        private static readonly string[] Weekday = { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };

        // Integer part (rounded down), used throughout the algorithm
        private static int INT(double d)
        {
            return (int)Math.Floor(d);
        }

        // Convert a solar date (dd/mm/yyyy) to its Julian Day Number
        private static long JdFromDate(int dd, int mm, int yy)
        {
            long a = (14 - mm) / 12;
            long y = yy + 4800 - a;
            long m = mm + 12 * a - 3;
            // Always use the Proleptic Gregorian formula to match .NET DateTime semantics.
            // The original code had a Julian branch for jd < 2299161 which caused a 9-day
            // lunar date jump around 15/10/1582 when fed .NET DateTime values.
            return dd + (153 * m + 2) / 5 + 365 * y + y / 4 - y / 100 + y / 400 - 32045;
        }

        // Time of the k-th new moon since the new moon of 11/1/1900 (in UT)
        private static double NewMoon(int k)
        {
            double t = k / 1236.85;
            double t2 = t * t;
            double t3 = t2 * t;
            double dr = PI / 180;
            double jd1 = 2415020.75933 + 29.53058868 * k + 0.0001178 * t2 - 0.000000155 * t3;
            jd1 += 0.00033 * Math.Sin((166.56 + 132.87 * t - 0.009173 * t2) * dr);

            double m = 359.2242 + 29.10535608 * k - 0.0000333 * t2 - 0.00000347 * t3;
            double mpr = 306.0253 + 385.81691806 * k + 0.0107306 * t2 + 0.00001236 * t3;
            double f = 21.2964 + 390.67050646 * k - 0.0016528 * t2 - 0.00000239 * t3;

            double c1 = (0.1734 - 0.000393 * t) * Math.Sin(m * dr) + 0.0021 * Math.Sin(2 * dr * m);
            c1 = c1 - 0.4068 * Math.Sin(mpr * dr) + 0.0161 * Math.Sin(dr * 2 * mpr);
            c1 = c1 - 0.0004 * Math.Sin(dr * 3 * mpr);
            c1 = c1 + 0.0104 * Math.Sin(dr * 2 * f) - 0.0051 * Math.Sin(dr * (m + mpr));
            c1 = c1 - 0.0074 * Math.Sin(dr * (m - mpr)) + 0.0004 * Math.Sin(dr * (2 * f + m));
            c1 = c1 - 0.0004 * Math.Sin(dr * (2 * f - m)) - 0.0006 * Math.Sin(dr * (2 * f + mpr));
            c1 = c1 + 0.0010 * Math.Sin(dr * (2 * f - mpr)) + 0.0005 * Math.Sin(dr * (2 * mpr + m));

            double deltaT;
            if (t < -11)
            {
                deltaT = 0.001 + 0.000839 * t + 0.0002261 * t2 - 0.00000845 * t3 - 0.000000081 * t * t3;
            }
            else
            {
                deltaT = -0.000278 + 0.000265 * t + 0.000262 * t2;
            }

            return jd1 + c1 - deltaT;
        }

        // Sun longitude at time jdn, in units of 1/12 of a circle (0..11)
        private static double SunLongitude(double jdn)
        {
            double t = (jdn - 2451545.0) / 36525;
            double t2 = t * t;
            double dr = PI / 180;
            double m = 357.52910 + 35999.05030 * t - 0.0001559 * t2 - 0.00000048 * t * t2;
            double l0 = 280.46645 + 36000.76983 * t + 0.0003032 * t2;
            double dl = (1.914600 - 0.004817 * t - 0.000014 * t2) * Math.Sin(dr * m);
            dl = dl + (0.019993 - 0.000101 * t) * Math.Sin(dr * 2 * m) + 0.000290 * Math.Sin(dr * 3 * m);
            double l = l0 + dl;
            l = l * dr;
            l = l - PI * 2 * INT(l / (PI * 2));
            return INT(l / PI * 6);
        }

        // Sun longitude converted to local time zone (start of day dayNumber)
        private static int GetSunLongitude(double dayNumber, int timeZone)
        {
            return (int)SunLongitude(dayNumber - 0.5 - timeZone / 24.0);
        }

        // Julian day of the k-th new moon, converted to time zone timeZone
        private static long GetNewMoonDay(int k, int timeZone)
        {
            return (long)Math.Floor(NewMoon(k) + 0.5 + timeZone / 24.0);
        }

        // Start day of lunar month 11 (the month containing the winter solstice) of solar year yy
        private static long GetLunarMonth11(int yy, int timeZone)
        {
            double off = JdFromDate(31, 12, yy) - 2415021.076998695;
            int k = INT(off / 29.530588853);
            long nm = GetNewMoonDay(k, timeZone);
            if (GetSunLongitude(nm, timeZone) >= 9)
            {
                nm = GetNewMoonDay(k - 1, timeZone);
            }
            return nm;
        }

        // Determine the leap month (if any) in the lunar year starting from new moon a11
        private static int GetLeapMonthOffset(long a11, int timeZone)
        {
            int k = INT((a11 - 2415021.076998695) / 29.530588853 + 0.5);
            int i = 1;
            int arc = GetSunLongitude(GetNewMoonDay(k + i, timeZone), timeZone);
            int last;
            do
            {
                last = arc;
                i++;
                arc = GetSunLongitude(GetNewMoonDay(k + i, timeZone), timeZone);
            } while (arc != last && i < 14);
            return i - 1;
        }

        /// <summary>
        /// Converts a solar date (dd/mm/yyyy) to the Vietnamese lunar date,
        /// automatically handling leap months.
        /// </summary>
        public static void ConvertSolar2Lunar(int dd, int mm, int yy, out int lunarDay, out int lunarMonth, out int lunarYear, out bool isLeapMonth)
        {
            long dayNumber = JdFromDate(dd, mm, yy);
            int k = INT((dayNumber - 2415021.076998695) / 29.530588853);

            long monthStart = GetNewMoonDay(k + 1, TimeZone);
            if (monthStart > dayNumber)
            {
                monthStart = GetNewMoonDay(k, TimeZone);
            }

            long a11 = GetLunarMonth11(yy, TimeZone);
            long b11 = a11;
            int lunarYearCalc;
            if (a11 >= monthStart)
            {
                lunarYearCalc = yy;
                a11 = GetLunarMonth11(yy - 1, TimeZone);
            }
            else
            {
                lunarYearCalc = yy + 1;
                b11 = GetLunarMonth11(yy + 1, TimeZone);
            }

            lunarDay = (int)(dayNumber - monthStart + 1);
            int diff = INT((monthStart - a11) / 29.0);
            isLeapMonth = false;
            lunarMonth = diff + 11;

            if (b11 - a11 > 365)
            {
                int leapMonthOffset = GetLeapMonthOffset(a11, TimeZone);
                if (diff >= leapMonthOffset)
                {
                    lunarMonth = diff + 10;
                    if (diff == leapMonthOffset)
                    {
                        isLeapMonth = true;
                    }
                }
            }

            if (lunarMonth > 12)
            {
                lunarMonth -= 12;
            }
            if (lunarMonth >= 11 && diff < 4)
            {
                lunarYearCalc -= 1;
            }

            lunarYear = lunarYearCalc;
        }

        // English zodiac animal name for a lunar year, e.g. 2026 -> "Horse"
        private static string GetZodiacAnimalName(int lunarYear)
        {
            // ((year + 8) % 12 + 12) % 12 is safe for negative years (C# % keeps sign for negatives)
            return ZodiacAnimals[((lunarYear + 8) % 12 + 12) % 12];
        }

        /// <summary>
        /// Returns the Vietnamese day of week name in English, e.g. "Saturday".
        /// </summary>
        public static string GetVietnameseWeekday(DayOfWeek dayOfWeek)
        {
            return Weekday[(int)dayOfWeek];
        }

        /// <summary>
        /// Returns the full lunar date string, e.g. "28/5, Year of the Horse" or "3/5 (leap month), Year of the Horse".
        /// </summary>
        public static string GetLunarDateString(DateTime date)
        {
            int lunarDay, lunarMonth, lunarYear;
            bool isLeapMonth;
            ConvertSolar2Lunar(date.Day, date.Month, date.Year, out lunarDay, out lunarMonth, out lunarYear, out isLeapMonth);
            string leapSuffix = isLeapMonth ? " (leap month)" : string.Empty;
            return lunarDay + "/" + lunarMonth + leapSuffix + ", Year of the " + GetZodiacAnimalName(lunarYear);
        }

        /// <summary>
        /// Returns a compact lunar date string for the system tray tooltip, which has a hard 63-character
        /// limit on .NET Framework. Example: "13/6, Horse" or "3/5 (leap), Buffalo".
        /// </summary>
        public static string GetLunarDateShort(DateTime date)
        {
            int lunarDay, lunarMonth, lunarYear;
            bool isLeapMonth;
            ConvertSolar2Lunar(date.Day, date.Month, date.Year, out lunarDay, out lunarMonth, out lunarYear, out isLeapMonth);
            string leapSuffix = isLeapMonth ? " (leap)" : string.Empty;
            return lunarDay + "/" + lunarMonth + leapSuffix + ", " + GetZodiacAnimalName(lunarYear);
        }

        // ---- F-02: Can Chi (Heavenly Stems + Earthly Branches) ----

        private static readonly string[] HeavenlyStems =
            { "Giáp", "Ất", "Bính", "Đinh", "Mậu", "Kỷ", "Canh", "Tân", "Nhâm", "Quý" };

        private static readonly string[] EarthlyBranches =
            { "Tý", "Sửu", "Dần", "Mão", "Thìn", "Tỵ", "Ngọ", "Mùi", "Thân", "Dậu", "Tuất", "Hợi" };

        /// <summary>Year Can Chi, e.g. "Giáp Thìn" for 2024.</summary>
        public static string GetYearCanChi(int lunarYear)
        {
            int stem   = ((lunarYear - 4) % 10 + 10) % 10;
            int branch = ((lunarYear - 4) % 12 + 12) % 12;
            return HeavenlyStems[stem] + " " + EarthlyBranches[branch];
        }

        /// <summary>Month Can Chi based on lunar month and year stem.</summary>
        public static string GetMonthCanChi(int lunarMonth, int lunarYear)
        {
            int yearStem   = ((lunarYear - 4) % 10 + 10) % 10;
            int stemBase   = (yearStem % 5 * 2 + 2) % 10;
            int monthStem  = (stemBase + lunarMonth - 1) % 10;
            int monthBranch = (lunarMonth + 1) % 12; // month 1=Dần(2), month 11=Tý(0), month 12=Sửu(1)
            return HeavenlyStems[monthStem] + " " + EarthlyBranches[monthBranch];
        }

        /// <summary>Day Can Chi based on Julian Day Number.</summary>
        public static string GetDayCanChi(DateTime date)
        {
            long jd = JdFromDate(date.Day, date.Month, date.Year);
            int stem   = (int)(((jd + 9) % 10 + 10) % 10);
            int branch = (int)(((jd + 1) % 12 + 12) % 12);
            return HeavenlyStems[stem] + " " + EarthlyBranches[branch];
        }

        /// <summary>Returns the earthly branch name for a lunar year.</summary>
        public static string GetYearBranchName(int lunarYear)
        {
            return EarthlyBranches[((lunarYear - 4) % 12 + 12) % 12];
        }

        // ---- F-02: ConvertLunar2Solar ----

        private static DateTime DateFromJd(long jd)
        {
            // JdFromDate(1,1,1) = 1721426 — inverse using DateTime.AddDays
            return new DateTime(1, 1, 1).AddDays(jd - 1721426L);
        }

        /// <summary>
        /// Converts a Vietnamese lunar date back to its solar (Gregorian) date.
        /// Returns DateTime.MinValue for an invalid input (e.g. wrong leap-month flag).
        /// </summary>
        public static DateTime ConvertLunar2Solar(int lunarDay, int lunarMonth, int lunarYear, bool isLeap)
        {
            long a11, b11;
            if (lunarMonth < 11)
            {
                a11 = GetLunarMonth11(lunarYear - 1, TimeZone);
                b11 = GetLunarMonth11(lunarYear, TimeZone);
            }
            else
            {
                a11 = GetLunarMonth11(lunarYear, TimeZone);
                b11 = GetLunarMonth11(lunarYear + 1, TimeZone);
            }

            int k   = INT((a11 - 2415021.076998695) / 29.530588853 + 0.5);
            int off = lunarMonth - 11;
            if (off < 0) off += 12;

            if (b11 - a11 > 365)
            {
                int leapOff    = GetLeapMonthOffset(a11, TimeZone);
                int leapMonth  = leapOff - 2;
                if (leapMonth < 0) leapMonth += 12;
                if (isLeap && lunarMonth != leapMonth) return DateTime.MinValue;
                if (isLeap || off >= leapOff) off++;
            }

            long monthStart = GetNewMoonDay(k + off, TimeZone);
            return DateFromJd(monthStart + lunarDay - 1);
        }

        // ---- F-09: 24 Solar Terms (Tiết Khí) ----

        private static double SunLongitudeDeg(double jdn)
        {
            double t  = (jdn - 2451545.0) / 36525;
            double t2 = t * t;
            double dr = PI / 180;
            double m  = 357.52910 + 35999.05030 * t - 0.0001559 * t2 - 0.00000048 * t * t2;
            double l0 = 280.46645 + 36000.76983 * t + 0.0003032 * t2;
            double dl = (1.914600 - 0.004817 * t - 0.000014 * t2) * Math.Sin(dr * m)
                      + (0.019993 - 0.000101 * t) * Math.Sin(dr * 2 * m)
                      + 0.000290 * Math.Sin(dr * 3 * m);
            double l = l0 + dl;
            return l - 360.0 * Math.Floor(l / 360.0);
        }

        /// <summary>
        /// Returns the 0-based solar term index (0=Xuân Phân at 0°, 1=Thanh Minh at 15°, …).
        /// Use AppStrings.GetSolarTermName(index) for the display name.
        /// </summary>
        public static int GetSolarTermIndex(DateTime date)
        {
            double jdn = JdFromDate(date.Day, date.Month, date.Year) - 0.5 - TimeZone / 24.0;
            double deg = SunLongitudeDeg(jdn);
            return (int)(deg / 15.0) % 24;
        }

        // ---- F-22: Giờ Hoàng Đạo (Auspicious Hours) ----

        // For each day branch (0=Tý..11=Hợi), 6 auspicious hour-branch indices
        private static readonly int[][] HoangDaoHours =
        {
            new[] {0,1,3,6,8,9},   // Tý
            new[] {2,3,5,8,10,11}, // Sửu
            new[] {0,4,5,7,10,11}, // Dần
            new[] {0,2,3,6,7,9},   // Mão
            new[] {2,4,5,8,9,11},  // Thìn
            new[] {0,1,4,7,10,11}, // Tỵ
            new[] {0,1,3,6,8,9},   // Ngọ
            new[] {2,3,5,8,10,11}, // Mùi
            new[] {0,4,5,7,10,11}, // Thân
            new[] {0,2,3,6,7,9},   // Dậu
            new[] {2,4,5,8,9,11},  // Tuất
            new[] {0,1,4,7,10,11}, // Hợi
        };

        /// <summary>
        /// Returns a 12-element bool array; result[i] = true if the i-th two-hour period
        /// (0=Tý 23-1h, 1=Sửu 1-3h, … 11=Hợi 21-23h) is Hoàng Đạo for the given date.
        /// </summary>
        public static bool[] GetAuspiciousHours(DateTime date)
        {
            long jd = JdFromDate(date.Day, date.Month, date.Year);
            int dayBranch = (int)(((jd + 1) % 12 + 12) % 12);
            bool[] result = new bool[12];
            foreach (int h in HoangDaoHours[dayBranch])
                result[h] = true;
            return result;
        }

        // ---- F-22: Ngũ Hành Nạp Âm ----

        private static readonly string[] NapAmNames =
        {
            "Hải Trung Kim", "Lư Trung Hỏa", "Đại Lâm Mộc", "Lộ Bàng Thổ", "Kiếm Phong Kim",
            "Sơn Đầu Hỏa",  "Giản Hạ Thủy", "Thành Đầu Thổ", "Bạch Lạp Kim", "Dương Liễu Mộc",
            "Tuyền Trung Thủy", "Ốc Thượng Thổ", "Tích Lịch Hỏa", "Tùng Bách Mộc", "Trường Lưu Thủy",
            "Sa Trung Kim",  "Sơn Hạ Hỏa",  "Bình Địa Mộc",  "Bích Thượng Thổ", "Kim Bạc Kim",
            "Phúc Đăng Hỏa", "Thiên Hà Thủy", "Đại Trạch Thổ", "Thoa Xuyến Kim", "Tang Chá Mộc",
            "Đại Khê Thủy",  "Sa Trung Thổ", "Thiên Thượng Hỏa", "Thạch Lựu Mộc", "Đại Hải Thủy"
        };

        /// <summary>Returns the Nạp Âm name for a lunar year (e.g. "Phúc Đăng Hỏa" for 2024).</summary>
        public static string GetNapAmName(int lunarYear)
        {
            int idx = (int)(((lunarYear - 4) % 60 + 60) % 60) / 2;
            return NapAmNames[idx];
        }

        // ---- F-22: Tuổi Xung (Conflicting Earthly Branch) ----

        /// <summary>Returns the earthly branch name that conflicts with the given lunar year (6 positions opposite).</summary>
        public static string GetConflictingBranchName(int lunarYear)
        {
            int branch = ((lunarYear - 4) % 12 + 12) % 12;
            return EarthlyBranches[(branch + 6) % 12];
        }

        // ---- V11-06: Day-level Nạp Âm and Tuổi Xung ----

        /// <summary>Returns the Nạp Âm name for the given solar date (day-based JDN sexagenary index).</summary>
        public static string GetDayNapAmName(DateTime date)
        {
            long jd  = JdFromDate(date.Day, date.Month, date.Year);
            int  idx = (int)(((jd + 49) % 60 + 60) % 60) / 2;
            return NapAmNames[idx];
        }

        /// <summary>Returns the conflicting Can Chi for the given solar date (day stem + branch 6 positions opposite).</summary>
        public static string GetDayConflictingBranchName(DateTime date)
        {
            long jd            = JdFromDate(date.Day, date.Month, date.Year);
            int dayStem        = (int)(((jd + 9) % 10 + 10) % 10);
            int dayBranch      = (int)(((jd + 1) % 12 + 12) % 12);
            int conflictBranch = (dayBranch + 6) % 12;
            int conflictStem   = (dayStem   + 6) % 10;
            return HeavenlyStems[conflictStem] + " " + EarthlyBranches[conflictBranch];
        }
    }
}
