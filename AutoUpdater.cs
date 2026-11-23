using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace frm_vietnamese_calendar
{
    /// <summary>
    /// Self-updater using native Windows in-place executable rename (no batch scripts or cmd.exe).
    /// Windows allows renaming a currently running executable on disk, then moving the new executable
    /// into its original path and launching the updated binary directly.
    /// </summary>
    internal static class AutoUpdater
    {
        public static void CleanupOldExecutable()
        {
            try
            {
                string oldExe = Application.ExecutablePath + ".old";
                if (File.Exists(oldExe))
                    File.Delete(oldExe);
            }
            catch
            {
                // Ignore if still locked briefly; it will be cleaned up on the next launch
            }
        }

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

            string exePath = Application.ExecutablePath;
            string newExe  = exePath + ".new";
            string oldExe  = exePath + ".old";

            ThreadPool.QueueUserWorkItem(_ =>
            {
                try
                {
                    try { ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12; } catch { }

                    if (File.Exists(newExe))
                        File.Delete(newExe);

                    using (var wc = new WebClient())
                    {
                        wc.Headers["User-Agent"] = "MiniPerpetualCalendar/" + UpdateChecker.CurrentVersion;
                        wc.DownloadFile(downloadUrl, newExe);
                    }

                    // Verify downloaded file is a valid PE executable (starts with "MZ")
                    var fileInfo = new FileInfo(newExe);
                    if (!fileInfo.Exists || fileInfo.Length < 10240)
                        throw new InvalidDataException(AppStrings.IsVietnamese
                            ? "Tệp tải về không hợp lệ hoặc bị hỏng."
                            : "Downloaded update file is invalid or incomplete.");

                    byte[] header = new byte[2];
                    using (var fs = new FileStream(newExe, FileMode.Open, FileAccess.Read))
                    {
                        if (fs.Read(header, 0, 2) < 2 || header[0] != 'M' || header[1] != 'Z')
                            throw new InvalidDataException(AppStrings.IsVietnamese
                                ? "Tệp tải về không phải là chương trình thực thi (.exe)."
                                : "Downloaded file is not a valid Windows executable.");
                    }

                    if (File.Exists(oldExe))
                        File.Delete(oldExe);

                    // Swap running executable with the newly downloaded one without invoking cmd.exe
                    File.Move(exePath, oldExe);
                    File.Move(newExe, exePath);

                    if (uiControl.IsHandleCreated && !uiControl.IsDisposed)
                    {
                        uiControl.BeginInvoke(new Action(() =>
                        {
                            try { Process.Start(exePath); } catch { }
                            Application.Exit();
                        }));
                    }
                }
                catch (Exception ex)
                {
                    try { if (File.Exists(newExe)) File.Delete(newExe); } catch { }

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
