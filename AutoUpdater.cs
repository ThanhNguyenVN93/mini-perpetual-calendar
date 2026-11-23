using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace frm_vietnamese_calendar
{
    // Downloads the new exe to a temp file, then launches a hidden batch script that:
    //   1. Waits for this process to exit
    //   2. Moves the temp file over the current exe
    //   3. Restarts the app
    // Windows allows moving/replacing a running exe as long as you do not write to it in-place.
    internal static class AutoUpdater
    {
        public static void DownloadAndInstall(Control uiControl, string downloadUrl, string fallbackPageUrl = null)
        {
            if (string.IsNullOrEmpty(downloadUrl) ||
                !downloadUrl.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
            {
                if (!string.IsNullOrEmpty(fallbackPageUrl))
                {
                    try { Process.Start(fallbackPageUrl); } catch { }
                }
                return;
            }

            string tempExe  = Path.Combine(Path.GetTempPath(), "MiniPerpetualCalendar_update.exe");
            string exePath  = Application.ExecutablePath;
            int    pid      = Process.GetCurrentProcess().Id;

            ThreadPool.QueueUserWorkItem(_ =>
            {
                try
                {
                    try { ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12; } catch { }

                    using (var wc = new WebClient())
                    {
                        wc.Headers["User-Agent"] = "MiniPerpetualCalendar/" + UpdateChecker.CurrentVersion;
                        wc.DownloadFile(downloadUrl, tempExe);
                    }

                    // Verify downloaded file is a valid PE executable (starts with "MZ")
                    var fileInfo = new FileInfo(tempExe);
                    if (!fileInfo.Exists || fileInfo.Length < 10240)
                        throw new InvalidDataException(AppStrings.IsVietnamese
                            ? "Tệp tải về không hợp lệ hoặc bị hỏng."
                            : "Downloaded update file is invalid or incomplete.");

                    byte[] header = new byte[2];
                    using (var fs = new FileStream(tempExe, FileMode.Open, FileAccess.Read))
                    {
                        if (fs.Read(header, 0, 2) < 2 || header[0] != 'M' || header[1] != 'Z')
                            throw new InvalidDataException(AppStrings.IsVietnamese
                                ? "Tệp tải về không phải là chương trình thực thi (.exe)."
                                : "Downloaded file is not a valid Windows executable.");
                    }

                    // Build bat: wait for PID, swap file, restart
                    string batPath = Path.Combine(Path.GetTempPath(), "mpc_selfupdate.bat");
                    string bat =
                        "@echo off\r\n" +
                        "set /a tries=0\r\n" +
                        ":wait\r\n" +
                        "tasklist /fi \"PID eq " + pid + "\" /nh 2>nul | findstr /i \"MiniPerpetualCalendar\" >nul\r\n" +
                        "if not errorlevel 1 (\r\n" +
                        "    set /a tries+=1\r\n" +
                        "    if %tries% lss 30 (ping 127.0.0.1 -n 2 >nul & goto wait)\r\n" +
                        ")\r\n" +
                        "move /y \"" + tempExe + "\" \"" + exePath + "\" >nul\r\n" +
                        "start \"\" \"" + exePath + "\"\r\n" +
                        "del \"%~f0\"\r\n";
                    File.WriteAllText(batPath, bat);

                    if (uiControl.IsHandleCreated && !uiControl.IsDisposed)
                        uiControl.BeginInvoke(new Action(() =>
                        {
                            Process.Start(new ProcessStartInfo("cmd.exe", "/c \"" + batPath + "\"")
                            {
                                WindowStyle    = ProcessWindowStyle.Hidden,
                                CreateNoWindow = true,
                            });
                            Application.Exit();
                        }));
                }
                catch (Exception ex)
                {
                    if (uiControl.IsHandleCreated && !uiControl.IsDisposed)
                        uiControl.BeginInvoke(new Action(() =>
                            MessageBox.Show(
                                (AppStrings.IsVietnamese ? "Cập nhật thất bại: " : "Update failed: ") + ex.Message,
                                AppStrings.IsVietnamese ? "Lỗi cập nhật" : "Update error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error)));
                }
            });
        }
    }
}
