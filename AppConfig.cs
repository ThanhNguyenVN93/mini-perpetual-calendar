using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace frm_vietnamese_calendar
{
    [DataContract]
    public class AppConfig
    {
        // Hotkeys
        [DataMember] public int HotkeyToggleModifiers { get; set; }
        [DataMember] public int HotkeyToggleKey { get; set; }
        [DataMember] public int HotkeyMonthModifiers { get; set; }
        [DataMember] public int HotkeyMonthKey { get; set; }

        // Donate
        [DataMember] public bool SkipDonatePrompt { get; set; }

        // Language
        [DataMember] public bool IsVietnamese { get; set; }

        // Window position (-1 = use default center)
        [DataMember] public int WindowLeft { get; set; }
        [DataMember] public int WindowTop { get; set; }

        // Widget behavior
        [DataMember] public bool IsAlwaysOnTop { get; set; }
        [DataMember] public int OpacityPercent { get; set; }
        [DataMember] public bool IsPositionLocked { get; set; }
        [DataMember] public bool IsClickThrough { get; set; }

        // Background image
        [DataMember] public bool BackgroundPaused { get; set; }
        [DataMember] public int BackgroundIntervalSeconds { get; set; }
        [DataMember] public bool BackgroundSolid { get; set; }

        // Theme: "photo" or "bloc"
        [DataMember] public string ThemeName { get; set; }

        // Calendar display
        [DataMember] public bool ShowWeekNumbers { get; set; }
        [DataMember] public bool FullscreenHide { get; set; }

        // Reminders (F-04)
        [DataMember] public CalendarEvent[] Reminders { get; set; }

        // ---- defaults ----
        [OnDeserializing]
        private void SetDefaults(StreamingContext ctx)
        {
            HotkeyToggleModifiers = 2;
            HotkeyToggleKey = 49;
            HotkeyMonthModifiers = 2;
            HotkeyMonthKey = 50;
            SkipDonatePrompt = false;
            IsVietnamese = true;
            WindowLeft = -1;
            WindowTop = -1;
            IsAlwaysOnTop = false;
            OpacityPercent = 100;
            IsPositionLocked = false;
            IsClickThrough = false;
            BackgroundPaused = false;
            BackgroundIntervalSeconds = 300;
            BackgroundSolid = false;
            ThemeName = "photo";
            ShowWeekNumbers = false;
            FullscreenHide = false;
            Reminders = new CalendarEvent[0];
        }

        public AppConfig()
        {
            SetDefaults(default(StreamingContext));
        }

        // ---- persistence ----

        private static string _configPath;

        public static string ConfigPath
        {
            get { return _configPath ?? (_configPath = ResolveConfigPath()); }
        }

        private static string ResolveConfigPath()
        {
            string exeDir = AppDomain.CurrentDomain.BaseDirectory;
            string candidate = Path.Combine(exeDir, "config.json");
            try
            {
                string probe = Path.Combine(exeDir, ".wrtest_" + Guid.NewGuid().ToString("N"));
                File.WriteAllText(probe, "");
                File.Delete(probe);
                return candidate;
            }
            catch
            {
                string appData = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "frm_vietnamese_calendar");
                try { Directory.CreateDirectory(appData); } catch { }
                return Path.Combine(appData, "config.json");
            }
        }

        private static AppConfig _current;
        public static AppConfig Current
        {
            get { return _current ?? (_current = Load()); }
        }

        public static AppConfig Load()
        {
            try
            {
                if (File.Exists(ConfigPath))
                {
                    byte[] bytes = File.ReadAllBytes(ConfigPath);
                    using (MemoryStream ms = new MemoryStream(bytes))
                    {
                        var ser = new DataContractJsonSerializer(typeof(AppConfig));
                        return (AppConfig)ser.ReadObject(ms);
                    }
                }
            }
            catch { }
            return new AppConfig();
        }

        public void Save()
        {
            try
            {
                string dir = Path.GetDirectoryName(ConfigPath);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                string tmp = ConfigPath + ".tmp";
                using (FileStream fs = new FileStream(tmp, FileMode.Create, FileAccess.Write))
                {
                    var settings = new DataContractJsonSerializerSettings { UseSimpleDictionaryFormat = true };
                    var ser = new DataContractJsonSerializer(typeof(AppConfig), settings);
                    ser.WriteObject(fs, this);
                }

                if (File.Exists(ConfigPath))
                    File.Replace(tmp, ConfigPath, null);
                else
                    File.Move(tmp, ConfigPath);
            }
            catch { }
        }

        public static void ResetCurrent()
        {
            _current = null;
            _configPath = null;
        }
    }

    [DataContract]
    public class CalendarEvent
    {
        [DataMember] public string Name { get; set; }
        [DataMember] public int LunarDay { get; set; }
        [DataMember] public int LunarMonth { get; set; }
        [DataMember] public bool IsLunar { get; set; }
        [DataMember] public int SolarDay { get; set; }
        [DataMember] public int SolarMonth { get; set; }
        [DataMember] public int DaysBefore { get; set; }

        [OnDeserializing]
        private void SetDefaults(StreamingContext ctx)
        {
            Name = "";
            IsLunar = true;
            DaysBefore = 3;
        }

        public CalendarEvent() { SetDefaults(default(StreamingContext)); }
    }
}
