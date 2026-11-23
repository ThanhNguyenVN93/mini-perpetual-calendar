using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace frm_vietnamese_calendar
{
    // Constants, P/Invoke declarations, and shared utilities for global hotkeys,
    // used by both Form1 (register/handle) and HotkeySettingsForm (capture new keys + check conflicts)
    internal static class GlobalHotkeyHelper
    {
        public const int WM_HOTKEY = 0x0312;

        public const uint MOD_ALT = 0x0001;
        public const uint MOD_CONTROL = 0x0002;
        public const uint MOD_SHIFT = 0x0004;
        public const uint MOD_NOREPEAT = 0x4000;

        public const int IdToggleForm = 1;
        public const int IdMonthView = 2;

        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        // Pretty-print a key combo, e.g.: Ctrl + Alt + 1
        public static string Format(uint modifiers, uint vk)
        {
            List<string> parts = new List<string>();
            if ((modifiers & MOD_CONTROL) != 0)
            {
                parts.Add("Ctrl");
            }
            if ((modifiers & MOD_ALT) != 0)
            {
                parts.Add("Alt");
            }
            if ((modifiers & MOD_SHIFT) != 0)
            {
                parts.Add("Shift");
            }

            parts.Add(KeyName(vk));
            return string.Join(" + ", parts);
        }

        private static string KeyName(uint vk)
        {
            if (vk >= '0' && vk <= '9') return ((char)vk).ToString();
            if (vk >= 'A' && vk <= 'Z') return ((char)vk).ToString();
            switch ((Keys)vk)
            {
                case Keys.Next:      return "PageDown";
                case Keys.Prior:     return "PageUp";
                case Keys.Back:      return "Backspace";
                case Keys.Capital:   return "CapsLock";
                case Keys.Delete:    return "Delete";
                case Keys.Insert:    return "Insert";
                case Keys.Home:      return "Home";
                case Keys.End:       return "End";
                case Keys.Up:        return "Up";
                case Keys.Down:      return "Down";
                case Keys.Left:      return "Left";
                case Keys.Right:     return "Right";
                case Keys.Return:    return "Enter";
                case Keys.Space:     return "Space";
                case Keys.Tab:       return "Tab";
                case Keys.F1:        return "F1";
                case Keys.F2:        return "F2";
                case Keys.F3:        return "F3";
                case Keys.F4:        return "F4";
                case Keys.F5:        return "F5";
                case Keys.F6:        return "F6";
                case Keys.F7:        return "F7";
                case Keys.F8:        return "F8";
                case Keys.F9:        return "F9";
                case Keys.F10:       return "F10";
                case Keys.F11:       return "F11";
                case Keys.F12:       return "F12";
                default:             return ((Keys)vk).ToString();
            }
        }
    }
}
