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
        public static void DownloadAndInstall(Control uiControl, string downloadUrl)
        {
            string tempExe  = Path.Combine(Path.GetTempPath(), "MiniPerpetualCalendar_update.exe");
            string exePath  = Application.ExecutablePath;
            int    pid      = Process.GetCurrentProcess().Id;

            // Show a tray balloon while downloading if possible
            var notify = uiControl as Form;

            ThreadPool.QueueUserWorkItem(_ =>
            {
                try
                {
                    using (var wc = new WebClient())
                    {
                        wc.Headers["User-Agent"] = "MiniPerpetualCalendar/" + UpdateChecker.CurrentVersion;
                        wc.DownloadFile(downloadUrl, tempExe);
                    }

                    // Build bat: wait for PID, swap file, restart
                    string batPath = Path.Combine(Path.GetTempPath(), "mpc_selfupdate.bat");
                    string bat =
                        "@echo off\r\n" +
                        ":wait\r\n" +
                        "tasklist /fi \"PID eq " + pid + "\" 2>nul | find /i \"" + pid + "\" >nul\r\n" +
                        "if not errorlevel 1 (timeout /t 1 /nobreak >nul & goto wait)\r\n" +
                        "move /y \"" + tempExe + "\" \"" + exePath + "\"\r\n" +
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
