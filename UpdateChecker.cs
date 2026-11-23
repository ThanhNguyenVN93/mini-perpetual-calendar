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
        public  const string CurrentVersion  = "1.0.8";

        /// <summary>
        /// Checks the GitHub Releases API on a background thread.
        /// onNewVersion(tag, pageUrl, downloadUrl) is called on the UI thread when a newer release exists.
        /// onUpToDate (optional) is called on the UI thread when already on the latest version.
        /// </summary>
        // onNewVersion(tag, pageUrl, downloadUrl, releaseNotes)
        public static void CheckAsync(System.Windows.Forms.Control uiControl,
                                      Action<string, string, string, string> onNewVersion,
                                      Action onUpToDate = null,
                                      Action<string> onError = null)
        {
            ThreadPool.QueueUserWorkItem(_ =>
            {
                try
                {
                    try { ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12; } catch { }

                    var req = (HttpWebRequest)WebRequest.Create(ApiUrl);
                    req.UserAgent        = "MiniPerpetualCalendar/" + CurrentVersion;
                    req.Accept           = "application/vnd.github.v3+json";
                    req.Timeout          = 10000;
                    req.ReadWriteTimeout = 10000;

                    using (var resp   = (HttpWebResponse)req.GetResponse())
                    using (var stream = resp.GetResponseStream())
                    using (var reader = new StreamReader(stream))
                    {
                        string json = reader.ReadToEnd();
                        string tag  = ParseField(json, "tag_name");
                        if (tag != null && IsNewer(tag))
                        {
                            string pageUrl     = ReleasePageBase + tag;
                            string downloadUrl = ParseField(json, "browser_download_url");
                            string notes       = ParseBody(json) ?? string.Empty;
                            InvokeOnUi(uiControl, () => onNewVersion(tag, pageUrl, downloadUrl, notes));
                        }
                        else if (onUpToDate != null)
                        {
                            InvokeOnUi(uiControl, onUpToDate);
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (onError != null)
                        InvokeOnUi(uiControl, () => onError(ex.Message));
                    else if (onUpToDate != null)
                        InvokeOnUi(uiControl, onUpToDate);
                }
            });
        }

        private static void InvokeOnUi(System.Windows.Forms.Control uiControl, Action action)
        {
            if (uiControl == null || action == null || uiControl.IsDisposed) return;
            try
            {
                if (uiControl.IsHandleCreated)
                    uiControl.BeginInvoke(action);
            }
            catch { }
        }

        private static string ParseField(string json, string field)
        {
            var m = Regex.Match(json, "\"" + field + "\"\\s*:\\s*\"([^\"]+)\"");
            return m.Success ? m.Groups[1].Value : null;
        }

        private static string ParseBody(string json)
        {
            var m = Regex.Match(json, "\"body\"\\s*:\\s*\"((?:[^\"\\\\]|\\\\.)*)\"");
            if (!m.Success) return null;
            string raw = m.Groups[1].Value;
            return raw.Replace("\\r\\n", "\n").Replace("\\n", "\n").Replace("\\r", "\n")
                      .Replace("\\t", "\t").Replace("\\\"", "\"").Replace("\\\\", "\\").Trim();
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
