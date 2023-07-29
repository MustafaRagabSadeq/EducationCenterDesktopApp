using MetroSet_UI.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace trainingCenter
{
    public partial class ScheduleForm : MetroSetForm
    {

        EDPCenterEntities eDPCenterEntities;
        List<Schedule> AllSchedules;
        List<Instructor> InstractursNames;
        List<Teacher> TeachersNames;
        public ScheduleForm()
        {
            InitializeComponent();
            eDPCenterEntities = new EDPCenterEntities();
            AllSchedules = eDPCenterEntities.Schedules.ToList();
            InstractursNames = eDPCenterEntities.Instructors.ToList();
            TeachersNames = eDPCenterEntities.Teachers.ToList();
        }

        private void addSchedule_Load_1(object sender, EventArgs e)
        {



            DateTime min = DateTime.Parse("7:00 AM");
            DateTime max = DateTime.Parse("11:30 PM");
            dtpStartTime.MinDate = min;
            dtpStartTime.MaxDate = max;


            DateTime min2 = DateTime.Parse("7:30 AM");
            DateTime max2 = DateTime.Parse("11:59 PM");
            dateTimePickerEND.MinDate = min2;
            dateTimePickerEND.MaxDate = max2;



            cbtype.SelectedIndex = 0;
            MinimumSize = MaximumSize = Size;
            NewDataGrid(eDPCenterEntities.Schedules.ToList());
            var weekS = cbDayOfWeek.Text;
            var st_time = dateTimePickerEND.Value.TimeOfDay;
            var en_time = dtpStartTime.Value.TimeOfDay;
            List<Room> rooms = (from r in eDPCenterEntities.Rooms
                                select r).ToList();

            List<GroupName> groupNames = eDPCenterEntities.GroupNames.ToList();

            comboBox1.DataSource = groupNames;
            comboBox1.ValueMember = "G_ID";
            comboBox1.DisplayMember = "G_Name";

            cbRoom.DataSource = rooms;
            cbRoom.ValueMember = "Room_ID";
            cbRoom.DisplayMember = "Room_Name";

            NewDataGrid(eDPCenterEntities.Schedules.ToList());

            cbSearchRoom.DataSource = rooms;
            cbSearchRoom.ValueMember = "Room_ID";
            cbSearchRoom.DisplayMember = "Room_Name";


            var week = new[]
        {
            new { Text = "السبت", Value = "Saturday"},
            new { Text = "الاحد", Value = "Sunday" },
            new { Text = "الاثنين", Value = "Monday" },
            new { Text = "الثلاثاء", Value =  "Tuesday"},
            new { Text = "الاربعاء", Value = "Wednesday" },
            new { Text = "الخميس", Value = "Thursday" },
            new { Text ="الجمعه", Value = "Friday" },
         };
            cbDayOfWeek.DataSource = week;
            cbDayOfWeek.ValueMember = "Value";
            cbDayOfWeek.DisplayMember = "Text";
            string day = dtpDateOfRev.Value.DayOfWeek.ToString();
            cbDayOfWeek.SelectedValue = day;
        }
        ///edit
        private void materialButton2_Click(object sender, EventArgs e)
        {

            if (txtSchadulingID.Text.Length > 0)
            {
                int TId = Convert.ToInt32(cbName.SelectedValue.ToString());
                int groupId = Convert.ToInt32(cbGroup.SelectedValue.ToString());
                int subId = Convert.ToInt32(cbSubject.SelectedValue.ToString());
                int roomId = Convert.ToInt32(cbRoom.SelectedValue.ToString());
                var st_time = dateTimePickerEND.Value.TimeOfDay;
                var en_time = dtpStartTime.Value.TimeOfDay;
                var weekS = cbDayOfWeek.Text;
                var schodul = (from b in eDPCenterEntities.Schedules
                               where (b.Room_ID == roomId &&
                               (st_time >= b.Start_Time && st_time <= b.End_Time)
                               && b.week.Contains(txtSchadulingID.Text))
                               select b).FirstOrDefault();
                if (schodul == null)
                {
                    int id = Convert.ToInt32(txtSchadulingID.Text);
                    Schedule s = eDPCenterEntities.Schedules.Where(c => c.Schedule_ID == id).FirstOrDefault();
                    s.Teacher_ID = TId;
                    s.Group_ID = groupId;
                    s.Subject_ID = subId;
                    s.Room_ID = roomId;
                    s.week = weekS;
                    s.Start_Time = dateTimePickerEND.Value.TimeOfDay;
                    s.End_Time = dtpStartTime.Value.TimeOfDay;
                    s.date = dtpDateOfRev.Value;
                    eDPCenterEntities.SaveChanges();
                    txtSchadulingID.Text = "";
                    MessageBox.Show("تم التعديل بنجاح", "عملية ناجحة", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    List<Schedule> schedules = eDPCenterEntities.Schedules.ToList();
                    NewDataGrid(schedules);
                }
                else
                {
                    MessageBox.Show("القاعة ليست متاحة في هذا الوقت", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("اختر قاعة أولاً", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void NewDataGrid(List<Schedule> schedules)
        {
            txtSchadulingID.Text = "";
            cbName.Text = "";
            cbGroup.Text = "";
            cbRoom.Text = "";
            cbSubject.Text = "";

            string day = DateTime.Now.DayOfWeek.ToString();
            cbDayOfWeek.SelectedValue = day;

            /*dtpDateOfRev.Value = DateTime.Now;
            dateTimePickerEND.Value = DateTime.Now;
            dtpStartTime.Value = DateTime.Now;*/

            dataGridView1.Rows.Clear();

            foreach (Schedule schedule in schedules)
            {

                if (schedule.Teacher_ID != null)
                {
                    DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[0].Clone();
                    row.Cells[0].Value = schedule.Schedule_ID.ToString();
                    row.Cells[1].Value = schedule.Teacher.T_Name.ToString();
                    row.Cells[2].Value = schedule.GroupName.G_Name.ToString();
                    row.Cells[3].Value = schedule.Room.Room_Name.ToString();
                    row.Cells[4].Value = schedule.Subject.Sub_Name.ToString();
                    row.Cells[5].Value = "دائم";

                    //string time12HrsFormat = now.ToString("h:mm tt");

                    /*string date= schedule.Start_Time.ToString();
                    int hours = Convert.ToInt32(date.Substring(0, 2));
                    int minutes = Convert.ToInt32(date.Substring(3, 2));
                    int seconds = Convert.ToInt32(date.Substring(6,2));

                    DateTime temp = 
                        new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,hours,minutes,seconds);

                    row.Cells[6].Value = temp.ToString("h:mm tt");*/

                    //row.Cells[6].Value = schedule.Start_Time.ToString();

                    row.Cells[6].Value = GetStringOfDate(schedule.Start_Time.Value);
                    row.Cells[7].Value = GetStringOfDate(schedule.End_Time.Value);

                    //row.Cells[7].Value = schedule.End_Time.ToString();
                    row.Cells[8].Value = schedule.week.ToString();
                    row.DefaultCellStyle.BackColor = Color.GreenYellow;
                    row.DefaultCellStyle.ForeColor = Color.Black;
                    dataGridView1.Rows.Add(row);
                }
                else
                {
                    DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[0].Clone();
                    row.Cells[0].Value = schedule.Schedule_ID.ToString();
                    row.Cells[1].Value = schedule.Instructor.Name.ToString();
                    row.Cells[2].Value = "مدرب";
                    row.Cells[3].Value = schedule.Room.Room_Name.ToString();
                    row.Cells[4].Value = "مدرب";
                    row.Cells[5].Value = schedule.date.ToString();
                    row.Cells[6].Value = GetStringOfDate(schedule.Start_Time.Value);
                    row.Cells[7].Value = GetStringOfDate(schedule.End_Time.Value);
                    row.Cells[8].Value = schedule.week.ToString();
                    row.DefaultCellStyle.BackColor = Color.DarkRed;
                    row.DefaultCellStyle.ForeColor = Color.LightGray;
                    dataGridView1.Rows.Add(row);
                }

            }
        }

        //select from datagridview
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            if (index >= 0)
            {
                DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[index];

                if (row.Cells[0].Value != null)
                {

                    txtSchadulingID.Text = row.Cells[0].Value.ToString();
                }
            }

        }

        //Select from compbox teacher
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                double temp;
                bool NotEmptyGroups = double.TryParse(cbName.SelectedValue.ToString(), out temp);
                if (NotEmptyGroups)
                {
                    List<GroupName> groupNames = eDPCenterEntities.GroupNames.Where(m => m.Teacher_ID == temp).ToList();
                    cbGroup.DataSource = groupNames;
                    cbGroup.ValueMember = "G_ID";
                    cbGroup.DisplayMember = "G_Name";

                    Teacher teachers = (from s in eDPCenterEntities.Teachers
                                        where s.T_ID == temp
                                        select s).FirstOrDefault();

                    List<Subject> subjects = (from s in eDPCenterEntities.Subjects
                                              from st in eDPCenterEntities.Teacher_Subject
                                              where s.Sub_ID == st.Subject_ID && st.Teacher_ID == temp
                                              select s).Distinct().ToList();

                    int teach_ID = Convert.ToInt32(cbName.SelectedValue.ToString());

                    GroupName groupName =
                        eDPCenterEntities.GroupNames.FirstOrDefault(x => x.Teacher_ID == teach_ID);

                    if (subjects.Count == 0 || groupName == null)
                    {
                        cbSubject.DataSource = null;
                        cbGroup.DataSource = null;
                        cbGroup.Text = "";
                    }
                    else
                    {
                        cbSubject.DataSource = subjects;
                        cbSubject.ValueMember = "Sub_ID";
                        cbSubject.DisplayMember = "Sub_Name";
                    }
                }
                
            }
            catch
            {
                MessageBox.Show("حدث خطأ أثناء تشغيل البرنامج", "خطأ غير متوقع", MessageBoxButtons.OK, MessageBoxIcon.Error);
                eDPCenterEntities.Dispose();
                eDPCenterEntities = new EDPCenterEntities();
            }

        }

        // add 
        private void btnadd_Click(object sender, EventArgs e)
        {
            try
            {
                if (cbtype.SelectedIndex == 0)
                {

                    if (cbName.Text.Length > 0 && cbGroup.Text.Length > 0 && cbRoom.Text.Length > 0 && cbSubject.Text.Length > 0 && cbDayOfWeek.Text.Length > 0)
                    {
                        int TId = Convert.ToInt32(cbName.SelectedValue.ToString());
                        int groupId = Convert.ToInt32(cbGroup.SelectedValue.ToString());
                        int subId = Convert.ToInt32(cbSubject.SelectedValue.ToString());
                        int roomId = Convert.ToInt32(cbRoom.SelectedValue.ToString());
                        var st_time = dtpStartTime.Value.TimeOfDay;
                        var en_time = dateTimePickerEND.Value.TimeOfDay;
                        var weekS = cbDayOfWeek.Text;
                        string theDate = dtpDateOfRev.Value.ToString("M/d/yyyy");

                        if (en_time > st_time)
                        {

                            var schodul = eDPCenterEntities.Schedules.ToList()
                               .Where(a =>
                               (((a.Start_Time >= st_time && a.End_Time <= en_time) || (a.End_Time > st_time && a.End_Time <= en_time)) && a.Instructor_ID != null && a.Room_ID == roomId && a.week == weekS) ||
                               a.Instructor_ID == null && ((a.Start_Time >= st_time && a.End_Time <= en_time) || (a.End_Time > st_time && a.End_Time <= en_time)) && a.week == weekS && a.Room_ID == roomId).ToList();

                            if (schodul.Count == 0)
                            {
                                Schedule s = new Schedule()
                                {
                                    Teacher_ID = TId,
                                    Group_ID = groupId,
                                    Subject_ID = subId,
                                    Room_ID = roomId,
                                    week = weekS,
                                    Start_Time = dtpStartTime.Value.TimeOfDay,
                                    End_Time = dateTimePickerEND.Value.TimeOfDay,
                                    date = dtpDateOfRev.Value
                                };
                                eDPCenterEntities.Schedules.Add(s);
                                eDPCenterEntities.SaveChanges();
                                MessageBox.Show("تم حجز القاعة بنجاح", "عملية ناجحة", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                List<Schedule> schedules = eDPCenterEntities.Schedules.ToList();
                                NewDataGrid(schedules);
                            }
                            else
                            {
                                MessageBox.Show("القاعة ليست متاحة في هذا الوقت", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else { MessageBox.Show("ادخل الوقت بشكل صحيح", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error); }

                    }
                    else { MessageBox.Show("قم بإدخال البيانات أولاً", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
                else
                {
                    if (cbName.Text.Length > 0 && cbRoom.Text.Length > 0 && cbDayOfWeek.Text.Length > 0)
                    {

                        int IId = Convert.ToInt32(cbName.SelectedValue.ToString());
                        int roomId = Convert.ToInt32(cbRoom.SelectedValue.ToString());
                        var st_time = dtpStartTime.Value.TimeOfDay;
                        var en_time = dateTimePickerEND.Value.TimeOfDay;
                        var weekS = cbDayOfWeek.Text;
                        string theDate = dtpDateOfRev.Value.ToString("M/d/yyyy");


                        if (en_time > st_time)
                        {
                            /*
                             var schodul = eDPCenterEntities.Schedules.ToList()
                                .Where(a => (((a.Start_Time < en_time && a.Start_Time > st_time) || (a.End_Time > st_time && a.End_Time < en_time)) && a.Instructor_ID != null && a.Room_ID == roomId && a.date.ToString().Contains(theDate)) ||
                                a.Instructor_ID == null && ((a.Start_Time < en_time && a.Start_Time > st_time) || (a.End_Time > st_time && a.End_Time < en_time)) && a.week == weekS && a.Room_ID == roomId).ToList();
                             */
                            var schodul = eDPCenterEntities.Schedules.ToList()
                              .Where(a => (((a.Start_Time >= st_time && a.End_Time <= en_time) || (a.End_Time > st_time && a.End_Time <= en_time)) && a.Instructor_ID == null && a.Room_ID == roomId) ||
                              a.Teacher_ID == null && ((a.Start_Time >= st_time && a.End_Time <= en_time) || (a.End_Time > st_time && a.End_Time <= en_time)) && a.week == weekS && a.Room_ID == roomId).ToList();
                            if (schodul.Count == 0)
                            {
                                Schedule s = new Schedule()
                                {
                                    Instructor_ID = IId,
                                    Start_Time = dtpStartTime.Value.TimeOfDay,
                                    End_Time = dateTimePickerEND.Value.TimeOfDay,
                                    date = dtpDateOfRev.Value,
                                    Room_ID = roomId,
                                    week = weekS,

                                };
                                eDPCenterEntities.Schedules.Add(s);
                                eDPCenterEntities.SaveChanges();
                                MessageBox.Show("تم حجز القاعة بنجاح", "عملية ناجحة", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                List<Schedule> schedules = eDPCenterEntities.Schedules.ToList();
                                NewDataGrid(schedules);
                            }
                            else
                            {
                                MessageBox.Show("القاعة ليست متاحة في هذا الوقت", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else { MessageBox.Show("ادخل الوقت بشكل صحيح", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error); }

                    }
                    else { MessageBox.Show("قم بإدخال البيانات أولاً", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error); }

                }
            }
            catch
            {
                MessageBox.Show("حدث خطأ أثناء تشغيل البرنامج", "خطأ غير متوقع", MessageBoxButtons.OK, MessageBoxIcon.Error);
                eDPCenterEntities.Dispose();
                eDPCenterEntities = new EDPCenterEntities();
            }
        }

        //delete
        private void materialButton3_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtSchadulingID.Text.Length > 0)
                {
                    DialogResult dialogResult = MessageBox.Show("هل أنت متأكد من الحذف", "حذف", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                    if (dialogResult == DialogResult.OK)
                    {
                        int id = Convert.ToInt32(txtSchadulingID.Text);
                        Schedule s = eDPCenterEntities.Schedules.Where(c => c.Schedule_ID == id).FirstOrDefault();
                        eDPCenterEntities.Schedules.Remove(s);
                        eDPCenterEntities.SaveChanges();
                        MessageBox.Show("تم الحذف بنجاح", "عملية ناجحة", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        List<Schedule> schedules = eDPCenterEntities.Schedules.ToList();
                        NewDataGrid(schedules);
                    }
                }
                else
                {
                    MessageBox.Show("اختر الميعاد أولاً", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch
            {
                MessageBox.Show("حدث خطأ أثناء تشغيل البرنامج", "خطأ غير متوقع", MessageBoxButtons.OK, MessageBoxIcon.Error);
                eDPCenterEntities.Dispose();
                eDPCenterEntities = new EDPCenterEntities();
            }
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            try
            {
                string day = cbSearchDay.Text;
                EDPCenterEntities cont = new EDPCenterEntities();
                List<Schedule> schedules = cont.Schedules.Where(x => x.week == day).ToList();
                NewDataGrid(schedules);
            }
            catch
            {
                MessageBox.Show("حدث خطأ أثناء تشغيل البرنامج", "خطأ غير متوقع", MessageBoxButtons.OK, MessageBoxIcon.Error);
                eDPCenterEntities.Dispose();
                eDPCenterEntities = new EDPCenterEntities();
            }

        }

        private void materialButton4_Click(object sender, EventArgs e)
        {
            try
            {
                int day = Convert.ToInt32(cbSearchRoom.SelectedValue);
                EDPCenterEntities cont = new EDPCenterEntities();
                List<Schedule> schedules = cont.Schedules.Where(x => x.Room_ID == day).ToList();
                NewDataGrid(schedules);
            }
            catch
            {
                MessageBox.Show("حدث خطأ أثناء تشغيل البرنامج", "خطأ غير متوقع", MessageBoxButtons.OK, MessageBoxIcon.Error);
                eDPCenterEntities.Dispose();
                eDPCenterEntities = new EDPCenterEntities();
            }
        }

        private void materialButton5_Click(object sender, EventArgs e)
        {
            NewDataGrid(eDPCenterEntities.Schedules.ToList());
        }


        #region كومبو اختيار نوع الحجز
        private void comboBox8_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbtype.SelectedIndex == 0)
                {
                    string day = dtpDateOfRev.Value.DayOfWeek.ToString();
                    cbDayOfWeek.SelectedValue = day;
                    cbDayOfWeek.Enabled = true;
                    dtpDateOfRev.Enabled = false;
                    cbGroup.Enabled = true;
                    cbSubject.Enabled = true;
                    cbName.DataSource = TeachersNames;
                    cbName.DisplayMember = "T_Name";
                    cbName.ValueMember = "T_ID";

                }
                else
                {
                    string day = dtpDateOfRev.Value.DayOfWeek.ToString();
                    cbDayOfWeek.SelectedValue = day;
                    cbDayOfWeek.Enabled = false;
                    dtpDateOfRev.Enabled = true;
                    cbGroup.Enabled = false;
                    cbSubject.Enabled = false;
                    cbName.DataSource = InstractursNames;
                    cbName.DisplayMember = "Name";
                    cbName.ValueMember = "ID";
                }
            }
            catch
            {
                MessageBox.Show("حدث خطأ أثناء تشغيل البرنامج", "خطأ غير متوقع", MessageBoxButtons.OK, MessageBoxIcon.Error);
                eDPCenterEntities.Dispose();
                eDPCenterEntities = new EDPCenterEntities();
            }
        }
        #endregion

        private void dtpDateOfRev_ValueChanged(object sender, EventArgs e)
        {
            string day = dtpDateOfRev.Value.DayOfWeek.ToString();
            cbDayOfWeek.SelectedValue = day;
        }

        private void materialButton1_Click_1(object sender, EventArgs e)
        {
            try
            {
                int gID = Convert.ToInt32(comboBox1.SelectedValue.ToString());
                EDPCenterEntities cont = new EDPCenterEntities();

                List<Schedule> schedules = cont.Schedules.Where(x => x.Group_ID == gID).ToList();
                NewDataGrid(schedules);
            }
            catch
            {
                MessageBox.Show("تأكد من البيانات المكتوبة", "خطأ غير متوقع", MessageBoxButtons.OK, MessageBoxIcon.Error);
                eDPCenterEntities.Dispose();
                eDPCenterEntities = new EDPCenterEntities();
            }
        }

        private void ScheduleForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            eDPCenterEntities.Dispose();
        }
        private string GetStringOfDate(TimeSpan timeSpan)
        {
            string date = timeSpan.ToString();
            int hours = Convert.ToInt32(date.Substring(0, 2));
            int minutes = Convert.ToInt32(date.Substring(3, 2));
            int seconds = Convert.ToInt32(date.Substring(6, 2));

            DateTime temp =
                new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hours, minutes, seconds);

            return temp.ToString("h:mm tt");
        }

        private async void materialButton2_Click_1(object sender, EventArgs e)
        {
            ScheduleTable frm = Application.OpenForms.OfType<ScheduleTable>().FirstOrDefault();  // get the instance of Form2 if it's already open
            if (frm == null)  // if Form2 is not open, create a new instance and show it
            {
                frm = new ScheduleTable();
                frm.Show();
            }
            else  // if Form2 is already open, maximize it
            {
                frm.WindowState = FormWindowState.Normal;
                frm.Focus();
            }

        }

        private void materialButton3_Click_1(object sender, EventArgs e)
        {
            addSchedule_Load_1(null, null);
        }

        private void txtSchadulingID_TextChanged(object sender, EventArgs e)
        {

        }

        private void cbSubject_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
