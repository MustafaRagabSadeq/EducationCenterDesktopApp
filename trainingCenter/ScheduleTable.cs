using MetroSet_UI.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace trainingCenter
{
    public partial class ScheduleTable : MetroSetForm
    {
        EDPCenterEntities eDPCenterEntities;
        List<Schedule> TecherSchaduling;
        int RoomId;

        #region Constractor
        public ScheduleTable()
        {
            InitializeComponent();
            eDPCenterEntities = new EDPCenterEntities();
            var week = new[]{
            new { Text = "01 ---> 07", Value = "7"},
            new { Text = "08 ---> 14", Value = "14" },
            new { Text = "15 ---> 21", Value = "21" },
            new { Text = "22 ---> 28", Value =  "28"},
            new { Text = "29 ---> 31", Value = "31" },};

            cbweeks.DataSource = week;
            cbweeks.ValueMember = "Value";
            cbweeks.DisplayMember = "Text";

            List<Room> rooms = eDPCenterEntities.Rooms.ToList();
            cbRoom.DataSource = rooms;
            cbRoom.ValueMember = "Room_ID";
            cbRoom.DisplayMember = "Room_Name";
            //cbRoom.SelectedIndex = 0;

        } 
        #endregion

        #region FormLoad
        private async void addSchedule_Load_1(object sender, EventArgs e)
        {

            calendarGridView.Columns["Time"].DefaultCellStyle.Format = "h:mm tt";
            // Set properties for the DataGridView control
            calendarGridView.RowHeadersVisible = false;
            calendarGridView.AllowUserToAddRows = false;
            calendarGridView.DefaultCellStyle.Font = new Font("Arial", 12);
            calendarGridView.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Add rows for each hour from 7am to 11pm
            for (int hour = 7; hour <= 23; hour++)
            {
                DateTime time = new DateTime(2000, 1, 1, hour, 0, 0);
                calendarGridView.Rows.Add(time.ToString("h:mm tt"), "", "", "", "", "");

                DateTime time2 = new DateTime(2000, 1, 1, hour, 30, 0);
                calendarGridView.Rows.Add(time2.ToString("h:mm tt"), "", "", "", "", "");
            }
            cbRoom.SelectedIndex = -1;
            //TecherSchaduling = eDPCenterEntities.Schedules.Where(a => a.Teacher_ID != null).ToList();
            //NewDataGrid(Schedule(), TecherSchaduling);
        } 
        #endregion

        #region Event
        private void cbRoom_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                NewDataGrid(Schedule(), TeacherSchedules());
            }
            catch
            {
                MessageBox.Show("حدث خطأ أثناء تشغيل البرنامج", "خطأ غير متوقع", MessageBoxButtons.OK, MessageBoxIcon.Error);
                eDPCenterEntities.Dispose();
                eDPCenterEntities = new EDPCenterEntities();
            }
            
        }
        private void dateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                NewDataGrid(Schedule(), TeacherSchedules());
            }
            catch
            {
                MessageBox.Show("حدث خطأ أثناء تشغيل البرنامج", "خطأ غير متوقع", MessageBoxButtons.OK, MessageBoxIcon.Error);
                eDPCenterEntities.Dispose();
                eDPCenterEntities = new EDPCenterEntities();
            }

        }
        private void cbweeks_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                NewDataGrid(Schedule(), TeacherSchedules());
            }
            catch
            {
                MessageBox.Show("حدث خطأ أثناء تشغيل البرنامج", "خطأ غير متوقع", MessageBoxButtons.OK, MessageBoxIcon.Error);
                eDPCenterEntities.Dispose();
                eDPCenterEntities = new EDPCenterEntities();
            }
        }
        private void ScheduleTable_FormClosed(object sender, FormClosedEventArgs e)
        {
            eDPCenterEntities.Dispose();
        }
        #endregion

        #region SelectDateFunction
        private DateTime SelectedDate()
        {
            int month =dateTimePicker.Value.Month;
            int year = dateTimePicker.Value.Year;

            int LastDayInWeek;
            bool lll = int.TryParse(cbweeks.SelectedValue.ToString(), out LastDayInWeek);
            if (!lll)
            return new DateTime(year, month, 1);
            else if (LastDayInWeek==31)
            {
                int daysInMonth = DateTime.DaysInMonth(year, month);
                return new DateTime(year, month, daysInMonth);
            }
            else return new DateTime(year, month, LastDayInWeek);
            
        } 
        #endregion

        #region Schedule Function
        private List<Schedule> Schedule()
        {
            
            if (cbRoom.SelectedValue==null)
            {
                RoomId = 0;
            }
            else { bool rombool = int.TryParse(cbRoom.SelectedValue.ToString(), out RoomId); }
            DateTime LastDateOfWeek = SelectedDate().Date;
            DateTime FirstDateOfWeek;
            if (LastDateOfWeek.Day == 31)
                FirstDateOfWeek = LastDateOfWeek.AddDays(-2).Date;
            else FirstDateOfWeek = LastDateOfWeek.AddDays(-6).Date;
            List<Schedule> scheduleDates;
            scheduleDates = eDPCenterEntities.Schedules
                .Where(s => s.Room_ID == RoomId)
                .Where(s => DbFunctions.TruncateTime(s.date) >= FirstDateOfWeek)
                .Where(s => DbFunctions.TruncateTime(s.date) <= LastDateOfWeek)
                .Select(s => s)
                .ToList();
            return scheduleDates;
        }
        #endregion

        #region NewDataGridFunction
        private void NewDataGrid(List<Schedule> schedules, List<Schedule> TecherSchaduling)
        {

            foreach (DataGridViewRow row in calendarGridView.Rows)
            {
                for (int i = row.Cells.Count - 1; i >= 1; i--)
                {
                    row.Cells[i].Value = DBNull.Value;
                    row.Cells[i].Style.BackColor = Color.White;
                }

            }
            if (TecherSchaduling != null)
            {
                foreach (Schedule scheduleT in TecherSchaduling)
                {


                    int dayNumber = 0;
                    switch (scheduleT.week.ToString())
                    {
                        case "السبت":
                            dayNumber = 1;
                            break;
                        case "الاحد":
                            dayNumber = 2;
                            break;
                        case "الاثنين":
                            dayNumber = 3;
                            break;
                        case "الثلاثاء":
                            dayNumber = 4;
                            break;
                        case "الاربعاء":
                            dayNumber = 5;
                            break;
                        case "الخميس":
                            dayNumber = 6;
                            break;
                        case "الجمعه":
                            dayNumber = 7;
                            break;
                    }
                    double Hour = 0;
                    /*if (scheduleT.Start_Time.Value.TotalHours % 1 == 0)
                    {
                        Hour = scheduleT.Start_Time.Value.TotalHours - 7;
                    }
                    else
                    {
                        Hour = scheduleT.Start_Time.Value.TotalHours - 6.5;
                    }*/
                    double tempRoundTeacher = Math.Round(scheduleT.Start_Time.Value.TotalHours * 2, MidpointRounding.AwayFromZero) / 2;
                    switch (tempRoundTeacher)
                    {
                        case 7:
                            Hour = 0;
                            break;
                        case 7.5:
                            Hour = 1;
                            break;
                        case 8:
                            Hour = 2;
                            break;
                        case 8.5:
                            Hour = 3;
                            break;
                        case 9:
                            Hour = 4;
                            break;
                        case 9.5:
                            Hour = 5;
                            break;
                        case 10:
                            Hour = 6;
                            break;
                        case 10.5:
                            Hour = 7;
                            break;
                        case 11:
                            Hour = 8;
                            break;
                        case 11.5:
                            Hour = 9;
                            break;
                        case 12:
                            Hour = 10;
                            break;
                        case 12.5:
                            Hour = 11;
                            break;
                        case 13:
                            Hour = 12;
                            break;
                        case 13.5:
                            Hour = 13;
                            break;
                        case 14:
                            Hour = 14;
                            break;
                        case 14.5:
                            Hour = 15;
                            break;
                        case 15:
                            Hour = 16;
                            break;
                        case 15.5:
                            Hour = 17;
                            break;
                        case 16:
                            Hour = 18;
                            break;
                        case 16.5:
                            Hour = 19;
                            break;
                        case 17:
                            Hour = 20;
                            break;
                        case 17.5:
                            Hour = 21;
                            break;
                        case 18:
                            Hour = 22;
                            break;
                        case 18.5:
                            Hour = 23;
                            break;
                        case 19:
                            Hour = 24;
                            break;
                        case 19.5:
                            Hour = 25;
                            break;
                        case 20:
                            Hour = 26;
                            break;
                        case 20.5:
                            Hour = 27;
                            break;
                        case 21:
                            Hour = 28;
                            break;
                        case 21.5:
                            Hour = 29;
                            break;
                        case 22:
                            Hour = 30;
                            break;
                        case 22.5:
                            Hour = 31;
                            break;
                        case 23:
                            Hour = 32;
                            break;
                        case 23.5:
                            Hour = 33;
                            break;
                    }
                    double duration = scheduleT.End_Time.Value.TotalHours - tempRoundTeacher;


                    for (double i = 0; i < (duration / 0.5); i++)
                    {
                        if (calendarGridView.Rows.Count > 1)
                        {
                            int temp = Convert.ToInt32(Hour + i);
                            calendarGridView.Rows[temp].Cells[dayNumber].Value = scheduleT.GroupName.G_Name.ToString();
                            calendarGridView.Rows[temp].Cells[dayNumber].Style.BackColor = Color.GreenYellow;
                            calendarGridView.Rows[temp].Cells[dayNumber].Style.ForeColor = Color.Black;
                        }
                    }

                }
            }

            if (schedules != null)
            {
                foreach (Schedule schedule in schedules)
            {
                if (schedule.Teacher_ID == null)
                {
                    int dayNumber = 0;
                    switch (schedule.date.DayOfWeek)
                    {
                        case DayOfWeek.Saturday:
                            dayNumber = 1;
                            break;
                        case DayOfWeek.Sunday:
                            dayNumber = 2;
                            break;
                        case DayOfWeek.Monday:
                            dayNumber = 3;
                            break;
                        case DayOfWeek.Tuesday:
                            dayNumber = 4;
                            break;
                        case DayOfWeek.Wednesday:
                            dayNumber = 5;
                            break;
                        case DayOfWeek.Thursday:
                            dayNumber = 6;
                            break;
                        case DayOfWeek.Friday:
                            dayNumber = 7;
                            break;
                    }
                    double Hour = 0;
                        /*if(schedule.Start_Time.Value.TotalHours % 1 ==0)
                        {
                            Hour =schedule.Start_Time.Value.TotalHours - 7;
                        }
                        else
                        {
                            Hour = schedule.Start_Time.Value.TotalHours - 6.5;
                        }*/
                        double tempRound = Math.Round(schedule.Start_Time.Value.TotalHours * 2, MidpointRounding.AwayFromZero) / 2;
                        switch (tempRound)
                    {
                        case 7:
                            Hour = 0;
                            break;
                        case 7.5:
                            Hour = 1;
                            break;
                        case 8:
                            Hour = 2;
                            break;
                        case 8.5:
                            Hour = 3;
                            break;
                        case 9:
                            Hour = 4;
                            break;
                        case 9.5:
                            Hour = 5;
                            break;
                        case 10:
                            Hour = 6;
                            break;
                        case 10.5:
                            Hour = 7;
                            break;
                        case 11:
                            Hour = 8;
                            break;
                        case 11.5:
                            Hour = 9;
                            break;
                        case 12:
                            Hour = 10;
                            break;
                        case 12.5:
                            Hour = 11;
                            break;
                        case 13:
                            Hour = 12;
                            break;
                        case 13.5:
                            Hour = 13;
                            break;
                        case 14:
                            Hour = 14;
                            break;
                        case 14.5:
                            Hour = 15;
                            break;
                        case 15:
                            Hour = 16;
                            break;
                        case 15.5:
                            Hour = 17;
                            break;
                        case 16:
                            Hour = 18;
                            break;
                        case 16.5:
                            Hour = 19;
                            break;
                        case 17:
                            Hour = 20;
                            break;
                        case 17.5:
                            Hour = 21;
                            break;
                        case 18:
                            Hour = 22;
                            break;
                        case 18.5:
                            Hour = 23;
                            break;
                        case 19:
                            Hour = 24;
                            break;
                        case 19.5:
                            Hour = 25;
                            break;
                        case 20:
                            Hour = 26;
                            break;
                        case 20.5:
                            Hour = 27;
                            break;
                        case 21:
                            Hour = 28;
                            break;
                        case 21.5:
                            Hour = 29;
                            break;
                        case 22:
                            Hour = 30;
                            break;
                        case 22.5:
                            Hour = 31;
                            break;
                        case 23:
                            Hour = 32;
                            break;
                        case 23.5:
                            Hour = 33;
                            break;
                    }
                    double durations = schedule.End_Time.Value.TotalHours - tempRound;

                    for (double i = 0; i < (durations / 0.5); i++)
                    {
                        if (calendarGridView.Rows.Count > 1)
                        {
                            int temp = Convert.ToInt32(Hour + i);
                            calendarGridView.Rows[temp].Cells[dayNumber].Value = schedule.Instructor.Name.ToString();
                            calendarGridView.Rows[temp].Cells[dayNumber].Style.BackColor = Color.Red;
                            calendarGridView.Rows[temp].Cells[dayNumber].Style.ForeColor = Color.Black;
                        }
                    }
                }
            }
        }
        }
        
        #endregion


        #region Changing TeacherList By Rom
        private List<Schedule> TeacherSchedules()
        {
            TecherSchaduling = eDPCenterEntities.Schedules.Where(a => a.Teacher_ID != null
            && a.Room_ID == RoomId).ToList();
            return TecherSchaduling;

        }

        #endregion

        private void btnSearchDay_Click(object sender, EventArgs e)
        {
            cbweeks_SelectedIndexChanged(null, null);
        }
    }
}