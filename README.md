# MiniPerpetualCalendar 🗓️

A lightweight Windows desktop widget that displays the current Vietnamese solar and lunar calendar — always visible, always up to date.

![Platform](https://img.shields.io/badge/platform-Windows%2010%2F11-blue)
![Framework](https://img.shields.io/badge/.NET-4.5.2%20WinForms-blueviolet)
![Version](https://img.shields.io/badge/version-1.0.1-green)
![Language](https://img.shields.io/badge/language-Vi%20%2F%20En-orange)

---

## Features

| # | Feature |
|---|---------|
| F-01 | Displays day-of-week, solar date, and full lunar date (day/month/year Can Chi) |
| F-02 | Current solar term (24 Tiết Khí) |
| F-03 | Giờ Hoàng Đạo — auspicious hours for the day |
| F-04 | Ngũ Hành Nạp Âm (NapAm) element for the day |
| F-05 | Remembers window position across restarts (multi-monitor safe) |
| F-06 | Always-on-top, opacity, position lock, click-through modes |
| F-07 | Month calendar popup with day-detail panel |
| F-08 | Jump-to-date dialog (solar or lunar input) |
| F-09 | Vietnamese & international public holidays highlighted |
| F-10 | Holiday countdown badge |
| F-11 | Start with Windows |
| F-12 | Dynamic system tray icon showing today's date |
| F-13 | Keyboard navigation in month calendar |
| F-14 | Single-instance enforcement with wake-up on second launch |
| F-15 | Bilingual UI (Vietnamese / English) — toggle at runtime |
| F-16 | Dark-gradient bloc theme + photo background theme |
| F-17 | Background image auto-rotation with configurable interval |
| F-18 | Week numbers in month calendar |
| F-19 | Windows 11 rounded corners on all dialogs |
| F-20 | Global hotkeys (show/hide + open month calendar) — fully customizable |
| F-21 | Ctrl+C copies today's date to clipboard |
| F-22 | Daily reminder notifications (solar or lunar dates, advance notice) |
| F-23 | Moon phase indicator |
| F-24 | Export reminders & holidays to `.ics` (RFC 5545) |
| F-25 | Auto-hide when a fullscreen app is active |
| F-26 | Donation prompt (skippable, with QR codes) |

---

## Screenshots

> _Coming soon_

---

## Requirements

- Windows 10 / 11
- .NET Framework 4.5.2 (included in Windows 10+)

---

## Build

Open `frm_vietnamese_calendar.slnx` in Visual Studio 2022 and build, or:

```powershell
msbuild frm_vietnamese_calendar.csproj /p:Configuration=Release
```

The output is a single signed executable: `bin\Release\MiniPerpetualCalendar.exe`.

---

## Usage

- **Right-click** the widget to open the context menu (opacity, theme, reminders, language, etc.)
- **Double-click** the widget to open the month calendar
- **Ctrl+C** on the widget to copy today's date to the clipboard
- **System tray icon** shows today's solar day; right-click for the same menu

---

## Lunar calendar algorithm

Based on the [Ho Ngoc Duc](https://www.informatik.uni-leipzig.de/~duc/amlich/) Vietnamese lunar calendar algorithm — JDN-based, accurate for 1900–2100.

---

## License

MIT
