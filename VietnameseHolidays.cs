using System;
using System.Collections.Generic;

namespace frm_vietnamese_calendar
{
    /// <summary>
    /// Vietnamese solar and lunar public holidays.
    /// Solar dates are fixed each year; lunar dates are converted via LunarCalendar.ConvertLunar2Solar.
    /// </summary>
    public static class VietnameseHolidays
    {
        private struct SolarHoliday
        {
            public int Month, Day;
            public string NameVi, NameEn;
        }

        private struct LunarHoliday
        {
            public int LunarMonth, LunarDay;
            public string NameVi, NameEn;
        }

        private static readonly SolarHoliday[] SolarHolidays =
        {
            new SolarHoliday { Month=1,  Day=1,  NameVi="Tết Dương Lịch",          NameEn="New Year's Day"            },
            new SolarHoliday { Month=2,  Day=14, NameVi="Ngày Valentine",           NameEn="Valentine's Day"           }, // L-09
            new SolarHoliday { Month=3,  Day=8,  NameVi="Ngày Quốc Tế Phụ Nữ",     NameEn="International Women's Day" }, // L-09
            new SolarHoliday { Month=4,  Day=30, NameVi="Ngày Giải Phóng",          NameEn="Liberation Day"            },
            new SolarHoliday { Month=5,  Day=1,  NameVi="Quốc Tế Lao Động",         NameEn="International Labor Day"   },
            new SolarHoliday { Month=6,  Day=1,  NameVi="Ngày Quốc Tế Thiếu Nhi",   NameEn="International Children's Day" }, // L-09
            new SolarHoliday { Month=9,  Day=2,  NameVi="Quốc Khánh",               NameEn="National Day"              },
            new SolarHoliday { Month=10, Day=20, NameVi="Ngày Phụ Nữ Việt Nam",      NameEn="Vietnamese Women's Day"    }, // L-09
            new SolarHoliday { Month=11, Day=20, NameVi="Ngày Nhà Giáo Việt Nam",    NameEn="Vietnamese Teachers' Day"  }, // L-09
            new SolarHoliday { Month=12, Day=25, NameVi="Lễ Giáng Sinh",             NameEn="Christmas Day"             }, // L-09
        };

        private static readonly LunarHoliday[] LunarHolidays =
        {
            new LunarHoliday { LunarMonth=1,  LunarDay=1,  NameVi="Tết Nguyên Đán",        NameEn="Lunar New Year (Tết)"    },
            new LunarHoliday { LunarMonth=1,  LunarDay=15, NameVi="Rằm Tháng Giêng",       NameEn="Full Moon 1st Month"     },
            new LunarHoliday { LunarMonth=3,  LunarDay=3,  NameVi="Tết Hàn Thực",          NameEn="Han Shi (Cold Food Day)" }, // L-09
            new LunarHoliday { LunarMonth=3,  LunarDay=10, NameVi="Giỗ Tổ Hùng Vương",     NameEn="Hung Kings' Festival"    },
            new LunarHoliday { LunarMonth=4,  LunarDay=15, NameVi="Lễ Phật Đản",           NameEn="Buddha's Birthday"       }, // L-09
            new LunarHoliday { LunarMonth=5,  LunarDay=5,  NameVi="Tết Đoan Ngọ",          NameEn="Doan Ngo (Dragon Boat)"  }, // L-09
            new LunarHoliday { LunarMonth=7,  LunarDay=15, NameVi="Lễ Vu Lan",             NameEn="Vu Lan (Ghost Festival)" },
            new LunarHoliday { LunarMonth=8,  LunarDay=15, NameVi="Tết Trung Thu",          NameEn="Mid-Autumn Festival"     },
            new LunarHoliday { LunarMonth=12, LunarDay=23, NameVi="Táo Quân về Trời",       NameEn="Kitchen God Festival"    },
        };

        /// <summary>Returns the holiday name for the given solar date, or null if none.</summary>
        public static string GetHolidayName(DateTime date)
        {
            bool vi = AppStrings.IsVietnamese;

            foreach (var h in SolarHolidays)
                if (h.Month == date.Month && h.Day == date.Day)
                    return vi ? h.NameVi : h.NameEn;

            int ld, lm, ly; bool leap;
            LunarCalendar.ConvertSolar2Lunar(date.Day, date.Month, date.Year, out ld, out lm, out ly, out leap);
            if (!leap)
            {
                foreach (var h in LunarHolidays)
                    if (h.LunarMonth == lm && h.LunarDay == ld)
                        return vi ? h.NameVi : h.NameEn;
            }

            return null;
        }

        /// <summary>
        /// Returns a list of (date, name) pairs for all holidays in the given year,
        /// sorted by date. Used for ICS export.
        /// </summary>
        public static List<Tuple<DateTime, string>> GetAllForYear(int year)
        {
            var result = new List<Tuple<DateTime, string>>();
            bool vi = AppStrings.IsVietnamese;

            foreach (var h in SolarHolidays)
            {
                try
                {
                    result.Add(Tuple.Create(new DateTime(year, h.Month, h.Day), vi ? h.NameVi : h.NameEn));
                }
                catch { }
            }

            // For lunar holidays we may need to convert for lunarYear = year-1 and year (month 11+ are in next solar year)
            for (int lunarYear = year - 1; lunarYear <= year; lunarYear++)
            {
                foreach (var h in LunarHolidays)
                {
                    try
                    {
                        DateTime solar = LunarCalendar.ConvertLunar2Solar(h.LunarDay, h.LunarMonth, lunarYear, false);
                        if (solar.Year == year)
                            result.Add(Tuple.Create(solar, vi ? h.NameVi : h.NameEn));
                    }
                    catch { }
                }
            }

            result.Sort((a, b) => a.Item1.CompareTo(b.Item1));
            return result;
        }

        /// <summary>
        /// Returns the name and days-until for the nearest upcoming holiday within 90 days.
        /// Returns null if none found.
        /// </summary>
        public static Tuple<string, int> GetNextHoliday(DateTime from)
        {
            DateTime today = from.Date;
            Tuple<string, int> best = null;

            for (int yearOffset = 0; yearOffset <= 1; yearOffset++)
            {
                int year = today.Year + yearOffset;
                foreach (var pair in GetAllForYear(year))
                {
                    int daysLeft = (pair.Item1.Date - today).Days;
                    if (daysLeft >= 0 && daysLeft <= 90)
                    {
                        if (best == null || daysLeft < best.Item2)
                            best = Tuple.Create(pair.Item2, daysLeft);
                    }
                }
            }

            return best;
        }

        /// <summary>
        /// Returns the countdown text for the widget badge, or empty string if no holiday within 90 days.
        /// </summary>
        public static string GetCountdownText(DateTime from)
        {
            var next = GetNextHoliday(from);
            if (next == null) return string.Empty;
            return AppStrings.FormatCountdown(next.Item1, next.Item2);
        }
    }
}
