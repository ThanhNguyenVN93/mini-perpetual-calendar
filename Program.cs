using System;
using System.Threading;
using System.Windows.Forms;

namespace frm_vietnamese_calendar
{
    internal static class Program
    {
        private const string SingleInstanceMutexName = "frm_vietnamese_calendar_SingleInstance";
        internal const string WakeEventName = "frm_vietnamese_calendar_WakeEvent";

        [STAThread]
        static void Main()
        {
            Application.ThreadException += (sender, e) =>
            {
                try { Application.Exit(); } catch { }
            };
            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                try { Application.Exit(); } catch { }
            };

            bool createdNew;
            using (var singleInstanceMutex = new Mutex(true, SingleInstanceMutexName, out createdNew))
            {
                if (!createdNew)
                {
                    // F-14: signal the running instance to restore itself instead of showing a message box
                    try
                    {
                        using (var ev = EventWaitHandle.OpenExisting(WakeEventName))
                            ev.Set();
                    }
                    catch { }
                    return;
                }

                // Create the named event so a future second instance can signal us
                using (var wakeEvent = new EventWaitHandle(false, EventResetMode.AutoReset, WakeEventName))
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new Form1(wakeEvent));
                }
            }
        }
    }
}
