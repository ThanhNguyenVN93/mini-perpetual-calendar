using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace frm_vietnamese_calendar
{
    /// <summary>
    /// F-04: Manage per-user calendar reminders (lunar or solar recurring dates).
    /// Changes are saved to AppConfig.Current.Reminders on OK.
    /// </summary>
    public partial class RemindersForm : Form
    {
        private List<CalendarEvent> _reminders;

        public RemindersForm()
        {
            InitializeComponent();
            Text = AppStrings.RemindersTitle;

            // Clone list so changes are not committed until OK
            _reminders = new List<CalendarEvent>(AppConfig.Current.Reminders ?? new CalendarEvent[0]);
            LoadListView();

            btnAdd.Text    = AppStrings.ReminderAdd;
            btnEdit.Text   = AppStrings.ReminderEdit;
            btnDelete.Text = AppStrings.ReminderDelete;
            btnOk.Text     = "OK";
            btnCancel.Text = AppStrings.IsVietnamese ? "Hủy" : "Cancel";

            SetupListViewColumns();

            listView.SelectedIndexChanged += ListView_SelectedIndexChanged;
            listView.DoubleClick          += (s, e) => EditSelected();
            ListView_SelectedIndexChanged(this, EventArgs.Empty);
        }

        private void SetupListViewColumns()
        {
            listView.Columns.Clear();
            listView.Columns.Add(AppStrings.ReminderName,       160);
            listView.Columns.Add(AppStrings.ReminderDate,        80);
            listView.Columns.Add(AppStrings.ReminderType,        80);
            listView.Columns.Add(AppStrings.ReminderDaysBefore,  60);
        }

        private void LoadListView()
        {
            listView.Items.Clear();
            foreach (var r in _reminders)
            {
                string dateStr = r.IsLunar
                    ? r.LunarDay + "/" + r.LunarMonth
                    : r.SolarDay + "/" + r.SolarMonth;
                string typeStr = r.IsLunar ? AppStrings.ReminderLunar : AppStrings.ReminderSolar;
                var item = new ListViewItem(r.Name ?? "")
                {
                    SubItems = { dateStr, typeStr, r.DaysBefore.ToString() }
                };
                listView.Items.Add(item);
            }
        }

        private void ListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool hasSelection = listView.SelectedIndices.Count > 0;
            btnEdit.Enabled   = hasSelection;
            btnDelete.Enabled = hasSelection;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (var dlg = new EditReminderForm(null))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    _reminders.Add(dlg.Result);
                    LoadListView();
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e) { EditSelected(); }

        private void EditSelected()
        {
            if (listView.SelectedIndices.Count == 0) return;
            int idx = listView.SelectedIndices[0];
            using (var dlg = new EditReminderForm(_reminders[idx]))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    _reminders[idx] = dlg.Result;
                    LoadListView();
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listView.SelectedIndices.Count == 0) return;
            int idx = listView.SelectedIndices[0];
            string msg = AppStrings.IsVietnamese
                ? "Xóa \"" + _reminders[idx].Name + "\"?"
                : "Delete \"" + _reminders[idx].Name + "\"?";
            if (MessageBox.Show(msg, AppStrings.RemindersTitle,
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _reminders.RemoveAt(idx);
                LoadListView();
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            AppConfig.Current.Reminders = _reminders.ToArray();
            AppConfig.Current.Save();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }

    // ---- Inline add/edit dialog ----

    internal class EditReminderForm : Form
    {
        public CalendarEvent Result { get; private set; }

        public EditReminderForm(CalendarEvent existing)
        {
            bool vi = AppStrings.IsVietnamese;
            Text            = existing == null
                ? (vi ? "Thêm nhắc nhở" : "Add reminder")
                : (vi ? "Sửa nhắc nhở" : "Edit reminder");
            FormBorderStyle = FormBorderStyle.FixedDialog;
            ClientSize      = new Size(320, 220);
            StartPosition   = FormStartPosition.CenterParent;
            MaximizeBox     = false;
            MinimizeBox     = false;

            int y = 12;
            int labelW = 120, ctrlX = 135;

            var lblName = new Label { Text = AppStrings.ReminderName + ":", Location = new Point(10, y + 2), Size = new Size(labelW, 20), TextAlign = ContentAlignment.MiddleLeft };
            var txtName = new TextBox { Location = new Point(ctrlX, y), Size = new Size(170, 22) };
            y += 32;

            var lblType = new Label { Text = AppStrings.ReminderType + ":", Location = new Point(10, y + 2), Size = new Size(labelW, 20), TextAlign = ContentAlignment.MiddleLeft };
            // L-08: RadioButtons instead of CheckBoxes — only one type can be selected at a time
            var rdoLunar = new RadioButton { Text = AppStrings.ReminderLunar, Location = new Point(ctrlX, y), Size = new Size(90, 22) };
            var rdoSolar = new RadioButton { Text = AppStrings.ReminderSolar, Location = new Point(ctrlX + 95, y), Size = new Size(90, 22) };
            y += 32;

            var lblDay = new Label { Text = (vi ? "Ngày:" : "Day:"), Location = new Point(10, y + 2), Size = new Size(labelW, 20), TextAlign = ContentAlignment.MiddleLeft };
            var nudDay = new NumericUpDown { Location = new Point(ctrlX, y), Size = new Size(55, 22), Minimum = 1, Maximum = 30, Value = 1 };
            var lblMon = new Label { Text = (vi ? "/ Tháng:" : "/ Month:"), Location = new Point(ctrlX + 60, y + 2), Size = new Size(60, 20), TextAlign = ContentAlignment.MiddleLeft };
            var nudMon = new NumericUpDown { Location = new Point(ctrlX + 120, y), Size = new Size(50, 22), Minimum = 1, Maximum = 12, Value = 1 };
            y += 32;

            var lblBefore = new Label { Text = AppStrings.ReminderDaysBefore + ":", Location = new Point(10, y + 2), Size = new Size(labelW, 20), TextAlign = ContentAlignment.MiddleLeft };
            var nudBefore = new NumericUpDown { Location = new Point(ctrlX, y), Size = new Size(55, 22), Minimum = 0, Maximum = 30, Value = 0 };
            y += 40;

            var btnOk     = new Button { Text = "OK",      Location = new Point(140, y), Size = new Size(80, 28) };
            var btnCancel = new Button { Text = vi ? "Hủy" : "Cancel", Location = new Point(230, y), Size = new Size(80, 28), DialogResult = DialogResult.Cancel };

            AcceptButton = btnOk;
            CancelButton = btnCancel;

            rdoLunar.CheckedChanged += (s, e) => { if (rdoLunar.Checked) nudDay.Maximum = 30; };
            rdoSolar.CheckedChanged += (s, e) => { if (rdoSolar.Checked) nudDay.Maximum = 31; };

            // Pre-populate from existing
            if (existing != null)
            {
                txtName.Text     = existing.Name ?? string.Empty;
                rdoLunar.Checked = existing.IsLunar;
                rdoSolar.Checked = !existing.IsLunar;
                nudDay.Value     = existing.IsLunar ? existing.LunarDay : existing.SolarDay;
                nudMon.Value     = existing.IsLunar ? existing.LunarMonth : existing.SolarMonth;
                nudBefore.Value  = existing.DaysBefore;
            }
            else
            {
                rdoLunar.Checked = true;
            }

            Controls.AddRange(new Control[] {
                lblName, txtName,
                lblType, rdoLunar, rdoSolar,
                lblDay, nudDay, lblMon, nudMon,
                lblBefore, nudBefore,
                btnOk, btnCancel
            });

            btnOk.Click += (s, e) =>
            {
                // L-08: guard against saving with an empty name
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
                DialogResult = DialogResult.OK; // R-01: close dialog and return result
                Close();
            };
        }
    }
}
