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
        public  const string CurrentVersion  = "1.0.3";

        /// <summary>
        /// Checks the GitHub Releases API on a background thread.
        /// onNewVersion(tag, pageUrl, downloadUrl) is called on the UI thread when a newer release exists.
        /// onUpToDate (optional) is called on the UI thread when already on the latest version.
        /// </summary>
        public static void CheckAsync(System.Windows.Forms.Control uiControl,
                                      Action<string, string, string> onNewVersion,
                                      Action onUpToDate = null)
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
                        string tag  = ParseField(json, "tag_name");
                        if (tag != null && IsNewer(tag))
                        {
                            string pageUrl     = ReleasePageBase + tag;
                            string downloadUrl = ParseField(json, "browser_download_url") ?? pageUrl;
                            if (uiControl.IsHandleCreated && !uiControl.IsDisposed)
                                uiControl.BeginInvoke(onNewVersion, tag, pageUrl, downloadUrl);
                        }
                        else if (onUpToDate != null && uiControl.IsHandleCreated && !uiControl.IsDisposed)
                            uiControl.BeginInvoke(onUpToDate);
                    }
                }
                catch
                {
                    if (onUpToDate != null && uiControl.IsHandleCreated && !uiControl.IsDisposed)
                        uiControl.BeginInvoke(onUpToDate);
                }
            });
        }

        private static string ParseField(string json, string field)
        {
            var m = Regex.Match(json, "\"" + field + "\"\\s*:\\s*\"([^\"]+)\"");
            return m.Success ? m.Groups[1].Value : null;
        }

        public static bool IsNewer(string tag)
        {
            string s = tag.TrimStart('v', 'V');
            Version remote, current;
            return Version.TryParse(s, out remote) &&
                   Version.TryParse(CurrentVersion, out current) &&
                   remote > current;
        }
    }
}
