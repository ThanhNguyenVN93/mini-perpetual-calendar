using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace frm_vietnamese_calendar
{
    /// <summary>
    /// Periodically downloads a random background image from Lorem Picsum, running in the background
    /// without blocking the UI. Has no UI dependency; it only raises the ImageReady event when a new
    /// image is ready.
    /// </summary>
    public class BackgroundImageProvider : IDisposable
    {
        private static readonly string CacheFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "frm_vietnamese_calendar",
            "background_cache.jpg");

        private readonly int _width;
        private readonly int _height;
        private readonly WebClient _webClient;
        private readonly SynchronizationContext _uiContext;

        public event Action<Image> ImageReady;

        public BackgroundImageProvider(int width, int height)
        {
            _width = width;
            _height = height;
            // Capture UI context at construction time (always called from UI thread)
            _uiContext = SynchronizationContext.Current ?? new SynchronizationContext();

            try
            {
                // Add TLS 1.2 without removing existing protocols (Picsum requires TLS 1.2)
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
            }
            catch
            {
                // Ignore if the environment doesn't support it
            }

            _webClient = new WebClient();
            _webClient.DownloadDataCompleted += WebClient_DownloadDataCompleted;
        }

        // Load the image saved from the previous run (if any), used to display immediately on app startup or when offline
        public Image LoadCachedImage()
        {
            try
            {
                if (File.Exists(CacheFilePath))
                {
                    using (FileStream fs = new FileStream(CacheFilePath, FileMode.Open, FileAccess.Read))
                    using (Image temp = Image.FromStream(fs))
                    {
                        return new Bitmap(temp);
                    }
                }
            }
            catch
            {
                // If the cache is invalid or corrupted, ignore it and treat it as if there's no cache
            }
            return null;
        }

        // Save the just-downloaded image to disk using an atomic write (tmp then replace) to avoid
        // leaving a half-written file if the app is killed mid-write
        private void SaveToCache(byte[] data)
        {
            try
            {
                string dir = Path.GetDirectoryName(CacheFilePath);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                string tmpPath = CacheFilePath + ".tmp";
                File.WriteAllBytes(tmpPath, data);
                if (File.Exists(CacheFilePath))
                {
                    File.Replace(tmpPath, CacheFilePath, null);
                }
                else
                {
                    File.Move(tmpPath, CacheFilePath);
                }
            }
            catch
            {
                // If the cache can't be written (e.g. out of disk space, no permission), ignore it
            }
        }

        // Start downloading a new random image, skipping if a request is already in progress
        public void RequestNewImage()
        {
            if (_webClient.IsBusy)
            {
                return;
            }

            string url = "https://picsum.photos/" + _width + "/" + _height + "?random=" + DateTime.Now.Ticks;
            try
            {
                _webClient.DownloadDataAsync(new Uri(url));
            }
            catch
            {
                // Network error: ignore it, keep the current background image, the timer will retry next time
            }
        }

        private void WebClient_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            if (e.Cancelled || e.Error != null || e.Result == null)
            {
                return;
            }

            byte[] data = e.Result;

            // Decode the bitmap and write cache on a background thread so the UI thread is never blocked.
            // SaveToCache is only called after a successful decode — corrupt data or captive-portal HTML
            // must not overwrite a valid cached image (C-03).
            Task.Run(() =>
            {
                try
                {
                    Bitmap bmp;
                    using (MemoryStream ms = new MemoryStream(data))
                    using (Image temp = Image.FromStream(ms))
                    {
                        bmp = new Bitmap(temp); // detach from the MemoryStream
                    }

                    SaveToCache(data); // I/O stays on background thread, only reached on valid image
                    _uiContext.Post(_ => ImageReady?.Invoke(bmp), null);
                }
                catch
                {
                    // Invalid image data (e.g. captive portal HTML): discard, keep existing cache
                }
            });
        }

        // Cancel any running request and dispose the WebClient
        public void Dispose()
        {
            try
            {
                _webClient.CancelAsync();
            }
            catch
            {
            }
            _webClient.DownloadDataCompleted -= WebClient_DownloadDataCompleted;
            _webClient.Dispose();
        }
    }
}
