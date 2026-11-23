using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace frm_vietnamese_calendar
{
    /// <summary>
    /// F-24: exports CalendarEvent reminders and Vietnamese holidays to RFC 5545 .ics format.
    /// </summary>
    public static class IcsExporter
    {
        public static void Export(string filePath, CalendarEvent[] events, int years)
        {
            var sb = new StringBuilder();
            int startYear = DateTime.Now.Year;
            int endYear   = startYear + years - 1;

            sb.AppendLine("BEGIN:VCALENDAR");
            sb.AppendLine("VERSION:2.0");
            sb.AppendLine("PRODID:-//MiniPerpetualCalendar//v1.0.0//EN");
            sb.AppendLine("CALSCALE:GREGORIAN");
            sb.AppendLine("METHOD:PUBLISH");

            // User-defined reminders
            if (events != null)
            {
                foreach (var ev in events)
                {
                    if (string.IsNullOrEmpty(ev.Name)) continue;

                    for (int y = startYear; y <= endYear; y++)
                    {
                        try
                        {
                            DateTime eventDate;
                            if (ev.IsLunar)
                            {
                                // Convert lunar date to solar for each year (the lunar year that contains
                                // this month number maps to different solar years)
                                for (int lunarY = y - 1; lunarY <= y + 1; lunarY++)
                                {
                                    try
                                    {
                                        DateTime candidate = LunarCalendar.ConvertLunar2Solar(
                                            ev.LunarDay, ev.LunarMonth, lunarY, false);
                                        // L-15: if short month (29 days) pushed day 30 → mùng 1 next month, back up 1 day
                                        if (ev.LunarDay == 30)
                                        {
                                            int cld, clm, cly; bool cleap;
                                            LunarCalendar.ConvertSolar2Lunar(candidate.Day, candidate.Month, candidate.Year,
                                                out cld, out clm, out cly, out cleap);
                                            if (cld == 1) candidate = candidate.AddDays(-1);
                                        }
                                        if (candidate.Year == y)
                                            WriteVEvent(sb, ev.Name, candidate, ev.DaysBefore);
                                        // L-15: no break — lunar month 11-12 events can appear twice in one solar year
                                    }
                                    catch { }
                                }
                            }
                            else
                            {
                                eventDate = new DateTime(y, ev.SolarMonth, ev.SolarDay);
                                WriteVEvent(sb, ev.Name, eventDate, ev.DaysBefore);
                            }
                        }
                        catch { }
                    }
                }
            }

            // Vietnamese public holidays
            for (int y = startYear; y <= endYear; y++)
            {
                foreach (var pair in VietnameseHolidays.GetAllForYear(y))
                {
                    WriteVEvent(sb, pair.Item2, pair.Item1, 0);
                }
            }

            sb.AppendLine("END:VCALENDAR");

            File.WriteAllText(filePath, sb.ToString(), new UTF8Encoding(false));
        }

        private static void WriteVEvent(StringBuilder sb, string name, DateTime date, int daysBefore)
        {
            string uid = Guid.NewGuid().ToString("N") + "@MiniPerpetualCalendar";
            string dtStamp = DateTime.UtcNow.ToString("yyyyMMdd'T'HHmmss'Z'");
            string dtStart = date.ToString("yyyyMMdd");

            sb.AppendLine("BEGIN:VEVENT");
            sb.AppendLine("UID:" + uid);
            sb.AppendLine("DTSTAMP:" + dtStamp);
            sb.AppendLine("DTSTART;VALUE=DATE:" + dtStart);
            sb.AppendLine("DTEND;VALUE=DATE:" + date.AddDays(1).ToString("yyyyMMdd")); // L-15: RFC 5545 requires DTEND for all-day events
            sb.AppendLine("SUMMARY:" + EscapeIcsText(name));
            sb.AppendLine("TRANSP:TRANSPARENT");

            if (daysBefore > 0)
            {
                sb.AppendLine("BEGIN:VALARM");
                sb.AppendLine("TRIGGER:-P" + daysBefore + "D");
                sb.AppendLine("ACTION:DISPLAY");
                sb.AppendLine("DESCRIPTION:" + EscapeIcsText(name));
                sb.AppendLine("END:VALARM");
            }

            sb.AppendLine("END:VEVENT");
        }

        private static string EscapeIcsText(string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            return text.Replace("\\", "\\\\").Replace(";", "\\;").Replace(",", "\\,").Replace("\n", "\\n");
        }
    }

    // ---- Simple dialog to pick how many years to export ----

    internal class IcsYearsForm : Form
    {
        public int Years { get; private set; } = 5;

        public IcsYearsForm()
        {
            bool vi = AppStrings.IsVietnamese;
            Text            = AppStrings.IcsExportTitle;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            ClientSize      = new Size(230, 95);
            StartPosition   = FormStartPosition.CenterParent;
            MaximizeBox     = false;
            MinimizeBox     = false;

            var lbl = new Label { Text = AppStrings.IcsExportYears, Location = new Point(10, 12), Size = new Size(140, 30), TextAlign = ContentAlignment.MiddleLeft };
            var nud = new NumericUpDown { Location = new Point(155, 14), Size = new Size(55, 22), Minimum = 1, Maximum = 10, Value = 5 };
            var btnOk = new Button { Text = "OK", Location = new Point(60, 55), Size = new Size(70, 28), DialogResult = DialogResult.OK };
            var btnCancel = new Button { Text = vi ? "Hủy" : "Cancel", Location = new Point(140, 55), Size = new Size(80, 28), DialogResult = DialogResult.Cancel };

            AcceptButton = btnOk;
            CancelButton = btnCancel;
            Controls.AddRange(new Control[] { lbl, nud, btnOk, btnCancel });

            btnOk.Click += (s, e) => { Years = (int)nud.Value; };
        }
    }
}
