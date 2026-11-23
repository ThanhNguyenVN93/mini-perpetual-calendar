using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;

namespace frm_vietnamese_calendar
{
    internal static class UpdateChecker
    {
        private const string ApiUrl          = "https://api.github.com/repos/ThanhNguyenVN93/mini-perpetual-calendar/releases/latest";
        private const string ReleasePageBase = "https://github.com/ThanhNguyenVN93/mini-perpetual-calendar/releases/tag/";
        private const string CurrentVersion  = "1.0.0";

        /// <summary>
        /// Checks the GitHub Releases API on a background thread.
        /// Calls <paramref name="onNewVersion"/> on the UI thread when a newer release exists.
        /// </summary>
        public static void CheckAsync(System.Windows.Forms.Control uiControl, Action<string, string> onNewVersion)
        {
            ThreadPool.QueueUserWorkItem(_ =>
            {
                try
                {
                    var req = (HttpWebRequest)WebRequest.Create(ApiUrl);
                    req.UserAgent        = "MiniPerpetualCalendar/" + CurrentVersion;
                    req.Timeout          = 8000;
                    req.ReadWriteTimeout = 8000;

                    using (var resp   = (HttpWebResponse)req.GetResponse())
                    using (var stream = resp.GetResponseStream())
                    using (var reader = new StreamReader(stream))
                    {
                        string json = reader.ReadToEnd();
                        string tag  = ParseTagName(json);
                        if (tag != null && IsNewer(tag))
                        {
                            string url = ReleasePageBase + tag;
                            if (uiControl.IsHandleCreated && !uiControl.IsDisposed)
                                uiControl.BeginInvoke(onNewVersion, tag, url);
                        }
                    }
                }
                catch { }
            });
        }

        private static string ParseTagName(string json)
        {
            var m = Regex.Match(json, "\"tag_name\"\\s*:\\s*\"([^\"]+)\"");
            return m.Success ? m.Groups[1].Value : null;
        }

        private static bool IsNewer(string tag)
        {
            string s = tag.TrimStart('v', 'V');
            Version remote, current;
            return Version.TryParse(s, out remote) &&
                   Version.TryParse(CurrentVersion, out current) &&
                   remote > current;
        }
    }
}
