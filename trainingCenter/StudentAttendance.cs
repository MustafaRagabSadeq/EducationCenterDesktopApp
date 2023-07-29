using trainingCenter.BL;
using MetroSet_UI.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Runtime.Remoting.Contexts;
using System.Data.Entity;
using static System.ComponentModel.Design.ObjectSelectorEditor;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;
using System.Web.Configuration;

namespace trainingCenter
{
    public partial class StudentAttendance : MetroSetForm
    {

        EDPCenterEntities eDPCenterEntities;
        User user;
        public StudentAttendance(User _user)
        {

            InitializeComponent();
            eDPCenterEntities = new EDPCenterEntities();
            MinimumSize = MaximumSize = Size;
            user = _user;
            if (user.Role == "user")
            {
                materialButton2.Hide();
            }

        }

        private void StudentAttendance_Load(object sender, EventArgs e)
        {
            MinimumSize = MaximumSize = Size;
            List<Teacher> TeacherNames = eDPCenterEntities.Teachers.ToList();
            comboBox2.DataSource = TeacherNames;
            comboBox2.ValueMember = "T_ID";
            comboBox2.DisplayMember = "T_Name";


            DateTime shortDate = DateTime.Now.Date;
            //int currGroupId = Convert.ToInt32(comboBox3.SelectedValue.ToString());

            var list1 =
                from o in eDPCenterEntities.Attendences
                where DbFunctions.TruncateTime(o.Att_Date) == shortDate
                select o;
            NewDataGrid(list1);

            //adjust color of Data Grid View
            dataGridView1.Columns[0].DefaultCellStyle.ForeColor = Color.Black;
            dataGridView1.Columns[1].DefaultCellStyle.ForeColor = Color.Black;
            dataGridView1.Columns[2].DefaultCellStyle.ForeColor = Color.Black;
            dataGridView1.Columns[3].DefaultCellStyle.ForeColor = Color.Black;
            dataGridView1.Columns[4].DefaultCellStyle.ForeColor = Color.Black;
            dataGridView1.Columns[5].DefaultCellStyle.ForeColor = Color.Black;
            foreach (DataGridViewColumn c in dataGridView1.Columns)
            {
                c.DefaultCellStyle.Font = new Font("Arial", 17, FontStyle.Bold, GraphicsUnit.Pixel);
                c.DefaultCellStyle.ForeColor = Color.Black;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var temp = Convert.ToInt32(comboBox2.SelectedValue.ToString());
                List<GroupName> groupNames = eDPCenterEntities.GroupNames.Where(m => m.Teacher_ID == temp).ToList();
                comboBox3.DataSource = groupNames;
                comboBox3.ValueMember = "G_ID";
                comboBox3.DisplayMember = "G_Name";
            }
            catch
            {

            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            #region Fill Student Combo box 

            var teacher = Convert.ToInt32(comboBox2.SelectedValue.ToString());
            var groupId = Convert.ToInt32(comboBox3.SelectedValue.ToString());


            List<Student> studnet = (from g in eDPCenterEntities.Student_Group
                                     from s in eDPCenterEntities.Students
                                     where g.St_ID == s.St_ID && g.G_ID == groupId
                                     select s).ToList();

            comboBox1.DataSource = studnet;
            comboBox1.ValueMember = "St_Id";
            comboBox1.DisplayMember = "St_Name";
            #endregion


            #region Filter the data grid
            DateTime shortDate = DateTime.Now.Date;

            var list1 =
                        from o in eDPCenterEntities.Attendences
                        where DbFunctions.TruncateTime(o.Att_Date) == shortDate
                        select o;
            NewDataGrid(list1);
            #endregion

        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int studentId = Convert.ToInt32(comboBox1.SelectedValue.ToString());
            string phone = eDPCenterEntities.Students.Where(x => x.St_ID == studentId).Select(x => x.St_Phone).FirstOrDefault();
            if (phone != null)
            {
                textBox2.Text = phone;
            }
            stu_IDBox.Text = studentId.ToString();
        }

        private void materialButton4_Click(object sender, EventArgs e)
        {
            addStudent addStudent = new addStudent();
            addStudent.Show();
        }

        private void stu_IDBox_KeyPress(object sender, KeyPressEventArgs e)
        {


            if (e.KeyChar.Equals((char)13))
            {
                attendenceRecord();
            }

        }
        private void attendenceRecord()
        {
            try
            {
                int studentId;
                bool isNum = int.TryParse(stu_IDBox.Text, out studentId);
                if (studentId > 0 && isNum)
                {
                    Student student = eDPCenterEntities.Students.Where(x => x.St_ID == studentId).FirstOrDefault();
                    if (student != null)
                    {
                        if (comboBox3.SelectedValue != null)
                        {
                            int groupId = Convert.ToInt32(comboBox3.SelectedValue.ToString());
                            int studId = Convert.ToInt32(Convert.ToInt32(stu_IDBox.Text));
                            bool isPaid = false;

                           // where DbFunctions.TruncateTime(o.Att_Date) == shortDate

                            DateTime todayDate = DateTime.Now.Date;
                            Attendence attTemp =(from item in eDPCenterEntities.Attendences
                                                where item.St_ID == studId && item.G_ID == groupId &&
                                                DbFunctions.TruncateTime(item.Att_Date) == todayDate select item).FirstOrDefault();
                            if(attTemp != null)
                            {
                                DialogResult dialogResult = MessageBox.Show("هذا الطالب قد سجل حضوره فى هذا اليوم فى هذه المجموعة من قبل، هل انت متأكد من تسجيله مرة اخري؟", "تنبيه!", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning,MessageBoxDefaultButton.Button2);
                                if (dialogResult == DialogResult.OK)
                                {
                                    Student_Group studentPaid =
                                eDPCenterEntities.Student_Group.Where(x => x.G_ID == groupId && x.St_ID == studentId)
                                .FirstOrDefault();
                                    if (studentPaid == null)
                                    {
                                        Attendence attendence = new Attendence
                                        {
                                            G_ID = groupId,
                                            St_ID = studId,
                                            Att_Date = DateTime.Now,
                                            Payment_State = false,
                                        };
                                        eDPCenterEntities.Attendences.Add(attendence);
                                        eDPCenterEntities.SaveChanges();
                                    }
                                    else
                                    {
                                        if (studentPaid.Payment_Method == "الشهر")
                                        {
                                            //// Must implement minus price of session

                                            GroupName GName = eDPCenterEntities.GroupNames.Find(groupId);
                                            double? priceOfSession = GName.G_PriceOfSession;
                                            studentPaid.St_Balance -= priceOfSession;
                                            //Add teacher percent

                                            #region Daily_tranasction Outcome

                                            Teacher tech = eDPCenterEntities.Teachers.Where(x => x.T_ID == GName.Teacher_ID).FirstOrDefault();
                                            double? valueAdded = tech.T_Income_Percent * priceOfSession;
                                            tech.T_Balance += Math.Round((double)valueAdded, 3);

                                            //Daily_Transaction dtOutcome = new Daily_Transaction()
                                            //{
                                            //    Date = DateTime.Now,
                                            //    Name = $"خصم حصة من الطالب {student.St_Name} لمجموعة {GName.G_Name}",
                                            //    Transaction_Type = "مصروفات",
                                            //    Price = valueAdded,
                                            //    Person_ID = student.St_ID,
                                            //};
                                            #endregion

                                            //eDPCenterEntities.Daily_Transaction.Add(dtOutcome);
                                            eDPCenterEntities.SaveChanges();

                                            double? balance = eDPCenterEntities.Student_Group
                                                .Where(x => x.St_ID == studId && x.G_ID == groupId).FirstOrDefault().St_Balance;
                                            isPaid = balance >= 0 ? true : false;
                                        }
                                        else
                                        {
                                            isPaid = false;
                                        }
                                        eDPCenterEntities.Entry(studentPaid).State = EntityState.Modified;
                                        Attendence attendence = new Attendence
                                        {
                                            G_ID = groupId,
                                            St_ID = studId,
                                            Att_Date = DateTime.Now,
                                            Payment_State = isPaid,
                                        };
                                        eDPCenterEntities.Attendences.Add(attendence);
                                        eDPCenterEntities.SaveChanges();
                                    }
                                }
                            }
                            else
                            {
                                Student_Group studentPaid =
                                eDPCenterEntities.Student_Group.Where(x => x.G_ID == groupId && x.St_ID == studentId)
                                .FirstOrDefault();
                                if (studentPaid == null)
                                {
                                    Attendence attendence = new Attendence
                                    {
                                        G_ID = groupId,
                                        St_ID = studId,
                                        Att_Date = DateTime.Now,
                                        Payment_State = false,
                                    };
                                    eDPCenterEntities.Attendences.Add(attendence);
                                    eDPCenterEntities.SaveChanges();
                                }
                                else
                                {
                                    if (studentPaid.Payment_Method == "الشهر")
                                    {
                                        //// Must implement minus price of session

                                        GroupName GName = eDPCenterEntities.GroupNames.Find(groupId);
                                        double? priceOfSession = GName.G_PriceOfSession;
                                        studentPaid.St_Balance -= priceOfSession;
                                        //Add teacher percent

                                        #region Daily_tranasction Outcome

                                        Teacher tech = eDPCenterEntities.Teachers.Where(x => x.T_ID == GName.Teacher_ID).FirstOrDefault();
                                        double? valueAdded = tech.T_Income_Percent * priceOfSession;
                                        tech.T_Balance += Math.Round((double)valueAdded, 3);

                                        //Daily_Transaction dtOutcome = new Daily_Transaction()
                                        //{
                                        //    Date = DateTime.Now,
                                        //    Name = $"خصم حصة من الطالب {student.St_Name} لمجموعة {GName.G_Name}",
                                        //    Transaction_Type = "مصروفات",
                                        //    Price = valueAdded,
                                        //    Person_ID = student.St_ID,
                                        //};
                                        #endregion

                                        //eDPCenterEntities.Daily_Transaction.Add(dtOutcome);
                                        eDPCenterEntities.SaveChanges();

                                        double? balance = eDPCenterEntities.Student_Group
                                            .Where(x => x.St_ID == studId && x.G_ID == groupId).FirstOrDefault().St_Balance;
                                        isPaid = balance >= 0 ? true : false;
                                    }
                                    else
                                    {
                                        isPaid = false;
                                    }
                                    eDPCenterEntities.Entry(studentPaid).State = EntityState.Modified;
                                    Attendence attendence = new Attendence
                                    {
                                        G_ID = groupId,
                                        St_ID = studId,
                                        Att_Date = DateTime.Now,
                                        Payment_State = isPaid,
                                    };
                                    eDPCenterEntities.Attendences.Add(attendence);
                                    eDPCenterEntities.SaveChanges();
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("اختر اسم المدرس و المجموعة أولاً", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("لا يوجد طالب بهذا الكود", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    DateTime shortDate = DateTime.Now.Date;
                    int currGroupId = Convert.ToInt32(comboBox3.SelectedValue.ToString());

                    var list1 =
                        from o in eDPCenterEntities.Attendences
                        where DbFunctions.TruncateTime(o.Att_Date) == shortDate
                        select o;

                    NewDataGrid(list1);
                }
                else
                {
                    MessageBox.Show("برجاء ادخال قيمة صحيحة", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch
            {
                MessageBox.Show("حدث خطأ أثناء تشغيل البرنامج", "خطأ غير متوقع", MessageBoxButtons.OK, MessageBoxIcon.Error);
                eDPCenterEntities.Dispose();
                eDPCenterEntities = new EDPCenterEntities();
            }
        }

        private void NewDataGrid(IEnumerable<Attendence> attendences)
        {
            if (comboBox3.SelectedValue != null)
            {
                var currGroupId = Convert.ToInt32(comboBox3.SelectedValue.ToString());

                attendences = attendences.Where(o => o.G_ID == currGroupId);
            }

            dataGridView1.Rows.Clear();
            foreach (Attendence attendence in attendences)
            {
                string studName = eDPCenterEntities.Students.Where(x => x.St_ID == attendence.St_ID).FirstOrDefault().St_Name;
                GroupName groupName = eDPCenterEntities.GroupNames.Where(x => x.G_ID == attendence.G_ID).FirstOrDefault();
                if (groupName != null)
                {
                    Teacher teacherName = eDPCenterEntities.Teachers.Where(x => x.T_ID == groupName.Teacher_ID).FirstOrDefault();
                    DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[0].Clone();
                    row.Cells[0].Value = attendence.St_ID;
                    row.Cells[1].Value = studName;
                    row.Cells[2].Value = teacherName.T_Name;
                    row.Cells[3].Value = groupName.G_Name;
                    row.Cells[4].Value = attendence.Att_Date;
                    row.Cells[5].Value = attendence.Payment_State;

                    if (row.Cells[5].Value.ToString() == "False")
                    {
                        row.DefaultCellStyle.BackColor = Color.DarkRed;
                        row.DefaultCellStyle.ForeColor = Color.LightGray;
                        row.Cells[5].Value = "غير دافع";
                    }
                    else
                    {
                        row.DefaultCellStyle.BackColor = Color.White;
                        row.DefaultCellStyle.ForeColor = Color.Black;
                        row.Cells[5].Value = "دافع";
                    }
                    row.Cells[6].Value = attendence.St_Att_ID;

                    dataGridView1.Rows.Add(row);
                }
            }
            this.dataGridView1.Sort(this.dataGridView1.Columns["St_Address"], ListSortDirection.Ascending);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            if (index >= 0)
            {
                DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[index];
                if (row.Cells[0].Value != null)
                {

                    int studId = Convert.ToInt32(row.Cells[0].Value);
                    string attDate = row.Cells[4].Value.ToString();

                    Attendence attendence =
                        eDPCenterEntities
                        .Attendences.ToList()
                        .Where(a => a.Att_Date.ToString() == attDate && a.St_ID == studId)
                        .FirstOrDefault();
                    studentPayment stPayment = new studentPayment(attendence);
                    stPayment.ShowDialog();
                }
            }
        }
        private void materialButton1_Click(object sender, EventArgs e)
        {
            attendenceRecord();
        }

        private void StudentAttendance_Activated(object sender, EventArgs e)
        {
            eDPCenterEntities = new EDPCenterEntities();

            /*var list1 = 
                eDPCenterEntities.Attendences.ToList().Where(a=>a.Att_Date.Date==DateTime.Now.Date);*/
            DateTime shortDate = DateTime.Now.Date;
            var list1 =
                from o in eDPCenterEntities.Attendences
                where DbFunctions.TruncateTime(o.Att_Date) == shortDate
                select o;
            NewDataGrid(list1);
            /*
            List<Teacher> TeacherNames = eDPCenterEntities.Teachers.ToList();
            comboBox2.DataSource=null;
            comboBox2.DataSource = TeacherNames;
            comboBox2.ValueMember = "T_ID";
            comboBox2.DisplayMember = "T_Name";*/
        }

        //حذف سجلات الحضور
        private void materialButton2_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show("هل أنت متأكد من حذف جميع سجلات حضور الطلبة؟", "تنبيه!", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.OK)
                {
                    DialogResult dialogResult1 = MessageBox.Show("البيانات المحذوفة لا يمكن استعادتها مرة اخرى؟", "تنبيه!", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                    if (dialogResult1 == DialogResult.OK)
                    {
                        foreach (Attendence item in eDPCenterEntities.Attendences)
                        {
                            eDPCenterEntities.Attendences.Remove(item);
                        }
                        eDPCenterEntities.SaveChanges();
                        dataGridView1.Rows.Clear();
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

        private void StudentAttendance_FormClosed(object sender, FormClosedEventArgs e)
        {
            eDPCenterEntities.Dispose();
        }

        private void materialButton3_Click(object sender, EventArgs e)
        {
            StudentAttendance_Load(null,null);
        }
    }
}
