# Báo Cáo Kiểm Tra Sau Bản Vá `L-01..L-09` (`LEFTOVER_ISSUES_REPORT.md`)

- **Thời điểm kiểm tra:** 23/11/2026 (Sau khi Claude hoàn tất sửa `L-01` $\rightarrow$ `L-09`).
- **Trạng thái Build:** `0 Errors / 0 Warnings`.
- **Các mục đã sửa thành công (7/9 mục đầu):**
  - ✅ **`L-01`:** Đã thêm `UpdateCalendarDisplay()` vào cuối `ApplyLocalization()` ([`Form1.cs:371`](file:///d:/code/frm_vietnamese_calendar/Form1.cs#L371)).
  - ✅ **`L-02`:** Đã sửa thuật toán vẽ elip Pha Mặt Trăng và vẽ trăng tròn vào ngày Rằm `14..16` ([`Form1.cs:855-913`](file:///d:/code/frm_vietnamese_calendar/Form1.cs#L855-L913)).
  - ✅ **`L-03`:** Đã mở rộng Panel chi tiết thành 3 hàng, cho `_lblDetailAuspicious` gộp 3 cột (`ColumnSpan = 3`) ([`MonthCalendarForm.cs:86-118`](file:///d:/code/frm_vietnamese_calendar/MonthCalendarForm.cs#L86-L118)).
  - ✅ **`L-05`:** Đã thêm `!tleap` và kiểm tra lại nhắc nhở ngay sau khi đóng `RemindersForm` ([`Form1.cs:669, 1008-1012`](file:///d:/code/frm_vietnamese_calendar/Form1.cs#L669)).
  - ✅ **`L-06`:** Đã dời `lblLunarDate` lên `Y = 118` và `Countdown Badge` lên `Y = 152` ([`Form1.Designer.cs:148`](file:///d:/code/frm_vietnamese_calendar/Form1.Designer.cs#L148), [`Form1.cs:848`](file:///d:/code/frm_vietnamese_calendar/Form1.cs#L848)).
  - ✅ **`L-07`:** Đã dịch đầy đủ `ShortcutsHelpForm`, `HotkeySettingsForm` và `FormatBgInterval` ([`Form1.cs:367-370`](file:///d:/code/frm_vietnamese_calendar/Form1.cs#L367-L370)).
  - ✅ **`L-09`:** Đã bổ sung đầy đủ 10 lễ Dương lịch và 9 lễ Âm lịch vào [`VietnameseHolidays.cs`](file:///d:/code/frm_vietnamese_calendar/VietnameseHolidays.cs#L24-L49).

---

## ⚠️ Phần Việc Còn Lại Cần Xử Lý (2 Lỗi Phát Sinh Mới `R-01, R-02` + 7 Mục `L-10..L-16`)

| Mã | Mức độ | Tệp & Dòng | Tóm tắt vấn đề |
| :---: | :---: | :--- | :--- |
| **R-01** | **Nghiêm trọng (Regression)** | [`RemindersForm.cs:170, 202-222`](file:///d:/code/frm_vietnamese_calendar/RemindersForm.cs#L170) | **Bấm nút "OK" trong `EditReminderForm` không đóng được hộp thoại:** Khi sửa `L-08`, Claude bỏ `DialogResult = DialogResult.OK` ở dòng 170 nhưng **quên gán `DialogResult = DialogResult.OK; Close();` ở cuối `btnOk.Click` (dòng 221)** $\rightarrow$ Không thể lưu thêm/sửa bất kỳ nhắc nhở nào! |
| **R-02** | **Cao (Regression)** | [`MonthCalendarForm.cs:440-445`](file:///d:/code/frm_vietnamese_calendar/MonthCalendarForm.cs#L440-L445) | **`JumpToMonthForm` (`L-04`) nhảy tới tháng/năm nhưng quên gán `_selectedDate = target.Date;`:** Làm ô được tô màu xanh trên lưới lịch vẫn là ngày cũ, và तुरंत bị `RenderMonth()` ghi đè lại. |
| **L-10** | **Trung bình - Cao** | [`Form1.cs:196-201`](file:///d:/code/frm_vietnamese_calendar/Form1.cs#L196-L201) | **Bỏ tick "Nền tối đặc" (`_menuBgSolid`) không khôi phục lại ảnh nền:** Khi bỏ tick, `BackgroundImage` vẫn là `null` và `timerBackground` không chạy lại. |
| **L-11** | **Trung bình - Cao** | [`MonthCalendarForm.cs:138-175, 413-430`](file:///d:/code/frm_vietnamese_calendar/MonthCalendarForm.cs#L138-L175) | **Panel chi tiết ngày (`F-07`) không ghi đang xem ngày nào & bị kẹt ở ngày của tháng cũ khi bấm `<` / `>` sang tháng khác.** |
| **L-12** | **Trung bình** | [`DonateForm.cs`](file:///d:/code/frm_vietnamese_calendar/DonateForm.cs), [`HotkeySettingsForm.cs`](file:///d:/code/frm_vietnamese_calendar/HotkeySettingsForm.cs), [`ShortcutsHelpForm.cs`](file:///d:/code/frm_vietnamese_calendar/ShortcutsHelpForm.cs) | **Bo góc Windows 11 (`F-19` - `DwmSetWindowAttribute`) mới chỉ có trên `Form1`**, chưa có trên 3 hộp thoại không viền còn lại. |
| **L-13** | **Trung bình** | [`Form1.cs:131, 1072-1075`](file:///d:/code/frm_vietnamese_calendar/Form1.cs#L131) | **Lỗi lưu/khôi phục vị trí cửa sổ (`F-05`) trên màn hình phụ bên trái (tọa độ âm bị `>= 0` chặn) & khi thoát lúc đang Minimized (`Left/Top = -32000`).** |
| **L-14** | **Trung bình** | [`Form1.cs:159-262`](file:///d:/code/frm_vietnamese_calendar/Form1.cs#L159-L262) | **Menu chuột phải ở System Tray vẫn mở được khi đang có hộp thoại Modal**, cho phép bấm *"Hiện / Ẩn lịch"* hoặc mở chồng hộp thoại thứ 2. |
| **L-15** | **Thấp - Trung bình** | [`IcsExporter.cs:42-55, 88-105`](file:///d:/code/frm_vietnamese_calendar/IcsExporter.cs#L42-L55) | **Xuất `.ics` (`F-24`):** Ngày 30 Âm lịch gặp tháng thiếu (29 ngày) bị tràn sang Mùng 1 tháng sau; lệnh `break;` làm mất sự kiện Âm lịch xuất hiện 2 lần trong 1 năm Dương; thiếu `DTEND;VALUE=DATE:`. |
| **L-16** | **Thấp** | [`Form1.cs:171, 967-981`](file:///d:/code/frm_vietnamese_calendar/Form1.cs#L967-L981) | **Bổ sung `ShowBalloonTip` hướng dẫn khi người dùng bật "Nhấp xuyên" (`Click-Through`)** để họ biết cách tắt từ icon dưới System Tray. |

---

## Chi Tiết Code Sửa Cho 2 Lỗi Regression (`R-01`, `R-02`) & Các Mục `L-10..L-16`

### 1. Sửa `R-01` (Nghiêm trọng): Nút `OK` trong `EditReminderForm` (`RemindersForm.cs` dòng 202–222)
- **Code hiện tại ([`RemindersForm.cs` dòng 170 và 202–222](file:///d:/code/frm_vietnamese_calendar/RemindersForm.cs#L202-L222)):**
  `btnOk` đã bỏ `DialogResult = DialogResult.OK` ở dòng 170 để kiểm tra `txtName.Text` rỗng, nhưng ở cuối sự kiện `btnOk.Click` (dòng 221) lại **thiếu `DialogResult = DialogResult.OK; Close();`**!
- **Cách sửa:**
  ```csharp
  btnOk.Click += (s, e) =>
  {
      if (string.IsNullOrWhiteSpace(txtName.Text))
      {
          MessageBox.Show(vi ? "Vui lòng nhập tên nhắc nhở." : "Please enter a reminder name.",
              vi ? "Thiếu tên" : "Name required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
          return;
      }
      bool isLunar = rdoLunar.Checked;
      Result = new CalendarEvent
      {
          Name       = txtName.Text.Trim(),
          IsLunar    = isLunar,
          LunarDay   = isLunar ? (int)nudDay.Value : 0,
          LunarMonth = isLunar ? (int)nudMon.Value : 0,
          SolarDay   = isLunar ? 0 : (int)nudDay.Value,
          SolarMonth = isLunar ? 0 : (int)nudMon.Value,
          DaysBefore = (int)nudBefore.Value,
      };
      DialogResult = DialogResult.OK; // <-- THÊM DÒNG NÀY ĐỂ ĐÓNG DIALOG VÀ TRẢ KẾT QUẢ VỀ
      Close();
  };
  ```

---

### 2. Sửa `R-02` (Cao): Cập nhật `_selectedDate = target.Date;` trong `MonthCalendarForm.cs` (dòng 434–447) và kiểm tra `ConvertLunar2Solar` trả về `DateTime.MinValue`
- **Vị trí:** [`MonthCalendarForm.cs` (dòng 434–447, 539–550)](file:///d:/code/frm_vietnamese_calendar/MonthCalendarForm.cs#L434-L447)
- **Nguyên nhân:**
  1. Trong `LblMonthYear_Click` (dòng 440–445):
     ```csharp
     DateTime target = dlg.SelectedDate;
     _month = target.Month;
     _year  = target.Year;
     RenderMonth();
     ShowDetail(target);
     ```
     Vì `_selectedDate` chưa được gán bằng `target.Date` trước khi gọi `RenderMonth()`, nên `RenderMonth()` vẫn tô màu xanh cho ngày `_selectedDate` cũ!
  2. Trong `JumpToMonthForm` (dòng 541): `LunarCalendar.ConvertLunar2Solar(d, m, y, false)` có thể trả về `DateTime.MinValue` hoặc ngày nằm ngoài khoảng `1900..2100` mà không ném exception.
- **Cách sửa:**
  ```csharp
  // Trong LblMonthYear_Click (MonthCalendarForm.cs dòng 438):
  if (dlg.ShowDialog(this) == DialogResult.OK)
  {
      DateTime target = dlg.SelectedDate.Date;
      _selectedDate = target; // <-- Gán _selectedDate trước khi RenderMonth()
      _month = target.Month;
      _year  = Clamp(target.Year, MinYear, MaxYear);
      RenderMonth();
  }
  ```

---

### 3. Sửa `L-10` (Trung bình - Cao): Khôi phục ảnh nền khi bỏ tick "Nền tối đặc" (`Form1.cs` dòng 196–201)
```csharp
_menuBgSolid.CheckedChanged += (s, e) =>
{
    AppConfig.Current.BackgroundSolid = _menuBgSolid.Checked;
    AppConfig.Current.Save();
    if (_menuBgSolid.Checked)
    {
        BackgroundImage?.Dispose();
        BackgroundImage = null;
    }
    else if (AppConfig.Current.ThemeName != "bloc")
    {
        Image cached = _bgProvider?.LoadCachedImage();
        if (cached != null) OnBackgroundImageReady(cached);
        _bgProvider?.RequestNewImage();
        if (!AppConfig.Current.BackgroundPaused) timerBackground.Start();
    }
    Invalidate();
};
```

---

### 4. Sửa `L-11` (Trung bình - Cao): Cập nhật `_selectedDate` khi chuyển tháng/năm và hiển thị ngày đang chọn trên Panel chi tiết (`MonthCalendarForm.cs`)
- Trong `ChangeMonth(int delta)` và `ChangeYear(int delta)` ([`MonthCalendarForm.cs` dòng 413–430](file:///d:/code/frm_vietnamese_calendar/MonthCalendarForm.cs#L413-L430)):
  ```csharp
  int maxDay = DateTime.DaysInMonth(_year, _month);
  _selectedDate = new DateTime(_year, _month, Math.Min(_selectedDate.Day, maxDay));
  ```
- Trong `ShowDetail(DateTime date)` ([`MonthCalendarForm.cs` dòng 147](file:///d:/code/frm_vietnamese_calendar/MonthCalendarForm.cs#L147)):
  Hiển thị kèm ngày đang chọn ở đầu hoặc trên tiêu đề để người dùng luôn biết Panel chi tiết đang mô tả ngày nào:
  ```csharp
  _lblDetailCanChi.Text = date.ToString("dd/MM") + " (" + ld + "/" + lm + (leap ? "*" : "") + ") • "
      + dayCC + " / " + monCC + " / " + yearCC;
  ```

---

### 5. Sửa `L-12` (Trung bình): Bổ sung bo góc Windows 11 (`DwmSetWindowAttribute`) cho `DonateForm`, `HotkeySettingsForm`, `ShortcutsHelpForm`
Thêm override `OnHandleCreated` vào 3 form `DonateForm`, `HotkeySettingsForm`, `ShortcutsHelpForm` (hoặc gọi qua một hàm `GlobalHotkeyHelper.EnableRoundedCorners(Handle)` dùng chung):
```csharp
protected override void OnHandleCreated(EventArgs e)
{
    base.OnHandleCreated(e);
    try
    {
        int pref = 2; // DWMWCP_ROUND
        DwmSetWindowAttribute(Handle, 33, ref pref, sizeof(int));
    }
    catch { }
}
```

---

### 6. Sửa `L-13` (Trung bình): Hỗ trợ tọa độ âm của màn hình phụ bên trái & không lưu `-32000` khi đang Minimized (`Form1.cs` dòng 131, 1072–1075)
- Trong `AppConfig.cs`: Đổi giá trị mặc định chưa lưu vị trí thành `WindowLeft = int.MinValue; WindowTop = int.MinValue;` (hoặc trong `Form1.cs` dòng 131 kiểm tra `if (cfg.WindowLeft != -1 || cfg.WindowTop != -1)` thay vì `>= 0` vì `IntersectsWith(testRect)` ở dòng 136 đã kiểm tra an toàn với `Screen.AllScreens`).
- Trong `Form1.OnFormClosed` (dòng 1072–1075):
  ```csharp
  var cfg = AppConfig.Current;
  Rectangle bounds = (WindowState == FormWindowState.Normal) ? Bounds : RestoreBounds;
  if (bounds.Left > -10000 && bounds.Top > -10000)
  {
      cfg.WindowLeft = bounds.Left;
      cfg.WindowTop  = bounds.Top;
  }
  cfg.Save();
  ```

---

### 7. Sửa `L-14` (Trung bình): Chặn mở Menu chuột phải từ System Tray khi đang có hộp thoại Modal (`Form1.cs`)
Trong constructor của `Form1`:
```csharp
contextMenuMain.Opening += (s, e) =>
{
    if (_hotkeySettingsOpen || OwnedForms.Length > 0)
    {
        e.Cancel = true;
        if (OwnedForms.Length > 0)
        {
            OwnedForms[OwnedForms.Length - 1].BringToFront();
            OwnedForms[OwnedForms.Length - 1].Activate();
        }
    }
};
```

---

### 8. Sửa `L-15` (Thấp - Trung bình): Chống tràn ngày 30 tháng thiếu, bỏ `break;` và thêm `DTEND` trong `IcsExporter.cs`
Trong [`IcsExporter.cs` (dòng 42–55, 88–95)](file:///d:/code/frm_vietnamese_calendar/IcsExporter.cs#L42-L55):
```csharp
for (int lunarY = y - 1; lunarY <= y + 1; lunarY++)
{
    try
    {
        DateTime candidate = LunarCalendar.ConvertLunar2Solar(ev.LunarDay, ev.LunarMonth, lunarY, false);
        // Nếu tháng thiếu (29 ngày) mà sự kiện đặt ngày 30, candidate sẽ bị đẩy sang mùng 1 tháng sau -> lùi về ngày 29
        int cld, clm, cly; bool cleap;
        LunarCalendar.ConvertSolar2Lunar(candidate.Day, candidate.Month, candidate.Year, out cld, out clm, out cly, out cleap);
        if (cld != ev.LunarDay && ev.LunarDay == 30 && cld == 1)
        {
            candidate = candidate.AddDays(-1);
        }

        if (candidate.Year == y)
        {
            WriteVEvent(sb, ev.Name, candidate, ev.DaysBefore);
            // Không dùng break; vì sự kiện tháng 11-12 Âm lịch có thể xuất hiện 2 lần trong cùng 1 năm Dương lịch
        }
    }
    catch { }
}
```
Và trong `WriteVEvent`:
```csharp
sb.AppendLine("DTSTART;VALUE=DATE:" + dtStart);
sb.AppendLine("DTEND;VALUE=DATE:" + date.AddDays(1).ToString("yyyyMMdd"));
```

---

### 9. Sửa `L-16` (Thấp): Hiện `BalloonTip` khi bật "Nhấp xuyên" (`Form1.cs` dòng 171)
Trong `_menuClickThrough.CheckedChanged`:
```csharp
_menuClickThrough.CheckedChanged += (s, e) =>
{
    AppConfig.Current.IsClickThrough = _menuClickThrough.Checked;
    ApplyClickThrough(_menuClickThrough.Checked);
    AppConfig.Current.Save();
    if (_menuClickThrough.Checked && notifyIconMain != null && notifyIconMain.Visible)
    {
        notifyIconMain.ShowBalloonTip(2500, AppStrings.MenuClickThrough,
            AppStrings.IsVietnamese
                ? "Đã bật chế độ nhấp xuyên. Chuột phải vào biểu tượng dưới khay hệ thống để tắt."
                : "Click-through enabled. Right-click the system tray icon to disable it.",
            ToolTipIcon.Info);
    }
};
```
