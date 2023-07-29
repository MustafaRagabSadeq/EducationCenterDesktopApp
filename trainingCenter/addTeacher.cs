using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace trainingCenter
{
    public partial class addTeacher : MetroSet_UI.Forms.MetroSetForm
    {
        static EDPCenterEntities EDPDBContext = new EDPCenterEntities();       
        bool isValidName, isValidPhone, isValidAddress, isValidCash, isValidPercentage;
        int TeacherReportID = 0;
        User _users;
        public addTeacher(User user)
        {
            InitializeComponent();                      
            dataGridViewTeachers.Columns[0].DefaultCellStyle.ForeColor = Color.Black;
            dataGridViewTeachers.Columns[1].DefaultCellStyle.ForeColor = Color.Black;
            dataGridViewTeachers.Columns[2].DefaultCellStyle.ForeColor = Color.Black;
            dataGridViewTeachers.Columns[3].DefaultCellStyle.ForeColor = Color.Black;
            dataGridViewTeachers.Columns[4].DefaultCellStyle.ForeColor = Color.Black;
            dataGridViewTeachers.Columns[5].DefaultCellStyle.ForeColor = Color.Black;
            _users= user;
            if (_users.Role=="user" ) 
            {
                materialDelete.Hide();
                materialButton2.Hide();
            }
        }

        private void materialButtonAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValid())
                {
                    DialogResult dialogResult = MessageBox.Show("هل أنت متأكد من الإضافة؟", "إضافة مدرس", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.OK)
                    {
                        Teacher teacher = new Teacher
                        {
                            T_Name = textBox_TName.Text,
                            T_Address = textBox_TAddress.Text,
                            T_Income_Percent = Convert.ToDouble(textBox_TPercentage.Text) / 100.0d,
                            T_Phone = textBox_TPhone.Text,
                            T_Balance = textBox_TBalance.Text == "" ? 0 : int.Parse(textBox_TBalance.Text)
                        };
                        EDPDBContext.Teachers.Add(teacher);
                        EDPDBContext.SaveChanges();
                        textBox_TID.Text = "";
                        textBox_TAddress.Text = "";
                        textBox_TName.Text = "";
                        textBox_TBalance.Text = "0";
                        textBox_TPhone.Text = "";
                        textBox_TPercentage.Text = "";
                        List<Teacher> teachers = EDPDBContext.Teachers.ToList();
                        NewDataGrid(teachers);
                    }
                }
            }
            catch
            {
                MessageBox.Show("حدث خطأ أثناء تشغيل البرنامج", "خطأ غير متوقع", MessageBoxButtons.OK, MessageBoxIcon.Error);
                EDPDBContext.Dispose();
                EDPDBContext = new EDPCenterEntities();
            }
        }

        private void NewDataGrid(List<Teacher> teachers)
        {
            dataGridViewTeachers.Rows.Clear();

            foreach (Teacher item in teachers)
            {
                DataGridViewRow row = (DataGridViewRow)dataGridViewTeachers.Rows[0].Clone();
                row.Cells[0].Value = item.T_ID;
                row.Cells[1].Value = item.T_Name;
                row.Cells[2].Value = item.T_Address;
                row.Cells[3].Value = item.T_Phone;
                row.Cells[4].Value = item.T_Balance;
                row.Cells[5].Value = item.T_Income_Percent;                
                dataGridViewTeachers.Rows.Add(row);
            }
        }

        private void materialEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox_TID.Text.Length == 0)
                    MessageBox.Show("اختر المدرس أولاً", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    if (IsValid())
                    {
                        DialogResult dialogResult = MessageBox.Show("هل أنت متأكد من التعديل؟", "تعديل مدرس", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (dialogResult == DialogResult.OK)
                        {
                            int id = int.Parse(textBox_TID.Text);
                            Teacher teacher = EDPDBContext.Teachers.Where(t => t.T_ID == id).FirstOrDefault();
                            teacher.T_Phone = textBox_TPhone.Text;
                            teacher.T_Name = textBox_TName.Text;
                            teacher.T_Address = textBox_TAddress.Text;
                            teacher.T_Balance = int.Parse(textBox_TBalance.Text);
                            teacher.T_Income_Percent = double.Parse(textBox_TPercentage.Text) / 100d;
                            EDPDBContext.SaveChanges();
                            List<Teacher> teachers = EDPDBContext.Teachers.ToList();
                            NewDataGrid(teachers);
                            MessageBox.Show("تم تعديل بيانات المدرس بنجاح", "عملية ناجحة", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            textBox_TID.Text = "";
                            textBox_TAddress.Text = "";
                            textBox_TName.Text = "";
                            textBox_TBalance.Text = "0";
                            textBox_TPhone.Text = "";
                            textBox_TPercentage.Text = "";
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("حدث خطأ أثناء تشغيل البرنامج", "خطأ غير متوقع", MessageBoxButtons.OK, MessageBoxIcon.Error);
                EDPDBContext.Dispose();
                EDPDBContext = new EDPCenterEntities();
            }
        }

        private void materialDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox_TID.Text.Length > 0)
                {
                    DialogResult dialogResult = MessageBox.Show("هل تريد حذف المدرس؟ لن يمكن استعادة بيانات العنصر المحذوف", "! تحذير", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        int id = int.Parse(textBox_TID.Text);
                        Teacher teacher = EDPDBContext.Teachers.Where(t => t.T_ID == id).FirstOrDefault();
                        List<Teacher_Year> teacher1 = EDPDBContext.Teacher_Year.Where(t => t.Teacher_ID == id).ToList();
                        EDPDBContext.Teacher_Year.RemoveRange(teacher1);

                        List<GroupName> teacherGroup = EDPDBContext.GroupNames.Where(t => t.Teacher_ID == id).ToList();
                        EDPDBContext.GroupNames.RemoveRange(teacherGroup);

                        List<Teacher_Subject> teacherSup = EDPDBContext.Teacher_Subject.Where(t => t.Teacher_ID == id).ToList();
                        EDPDBContext.Teacher_Subject.RemoveRange(teacherSup);

                        List<Schedule> teacherschdel = EDPDBContext.Schedules.Where(t => t.Teacher_ID == id).ToList();
                        EDPDBContext.Schedules.RemoveRange(teacherschdel);

                        EDPDBContext.Teachers.Remove(teacher);
                        EDPDBContext.SaveChanges();
                        List<Teacher> teachers = EDPDBContext.Teachers.ToList();
                        NewDataGrid(teachers);
                        MessageBox.Show("تم حذف بيانات المدرس بنجاح", "عملية ناجحة", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        textBox_TID.Text = "";
                        textBox_TAddress.Text = "";
                        textBox_TName.Text = "";
                        textBox_TBalance.Text = "0";
                        textBox_TPhone.Text = "";
                        textBox_TPercentage.Text = "";
                    }
                }
                else
                {
                    MessageBox.Show("اختر المدرس أولاً", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch
            {
                MessageBox.Show("حدث خطأ أثناء تشغيل البرنامج", "خطأ غير متوقع", MessageBoxButtons.OK, MessageBoxIcon.Error);
                EDPDBContext.Dispose();
                EDPDBContext = new EDPCenterEntities();
            }
        }

        

        private void materialButton6_Click(object sender, EventArgs e)
        {
            textBox_Filter.Text = "ادخل الكود او الاسم";
            List<Teacher> teacher = EDPDBContext.Teachers.ToList();
            NewDataGrid(teacher);
        }

        private void materialButton5_Click(object sender, EventArgs e)
        {
            if (textBox_Filter.Text.Length == 0)
                MessageBox.Show("ادخل قيمة في البحث", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (textBox_Filter.Text == "ادخل الكود او الاسم")
                MessageBox.Show("ادخل قيمة في البحث", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                int id;
                bool isNumber = int.TryParse(textBox_Filter.Text, out id);

                List<Teacher> teachers;
                if (isNumber)
                {
                    teachers = EDPDBContext.Teachers.Where(t => t.T_ID == id).ToList();
                }
                else
                {
                    teachers = EDPDBContext.Teachers.Where(t => t.T_Name.Contains(textBox_Filter.Text)).ToList();
                }
                if (teachers.Count > 0)
                    NewDataGrid(teachers);
                else
                    MessageBox.Show("لا توجد نتائج", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridViewTeachers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            if (index >= 0)
            {
                DataGridViewRow row = dataGridViewTeachers.Rows[index];
                if (row.Cells[0].Value != null)
                {
                    textBox_TID.Text = row.Cells[0].Value.ToString();
                    textBox_TName.Text = row.Cells[1].Value.ToString();
                    textBox_TAddress.Text = row.Cells[2].Value.ToString();
                    textBox_TPhone.Text = row.Cells[3].Value.ToString();
                    textBox_TBalance.Text = row.Cells[4].Value.ToString();
                    textBox_TPercentage.Text = (Convert.ToDouble(row.Cells[5].Value) * 100).ToString();
                }                
            } 
        }

        public bool IsValid()
        {
            //checked validation
            isValidName = BL.Utilities.validateNameInArabic(textBox_TName.Text);
            isValidAddress = BL.Utilities.validateNameInArabic(textBox_TAddress.Text);
            isValidPhone = BL.Utilities.ValidatPhoneNumber(textBox_TPhone.Text);
            isValidCash = BL.Utilities.checkDoubleNumber(textBox_TBalance.Text);
            isValidPercentage = 
                textBox_TPercentage.Text.Length> 0 
                && BL.Utilities.checkDoubleNumber(textBox_TBalance.Text)
                && double.Parse(textBox_TPercentage.Text) >= 0.0
                && double.Parse(textBox_TPercentage.Text) <= 100.0 ? true : false;
            
            if (!isValidName)
            {
                lblNameValid.Visible = true;
                return false;
            }

            if (!isValidAddress)
            {
                lblAddressValid.Visible = true;
                return false;
            }

            if (!isValidPhone)
            {
                lblPhoneValid.Visible = true;
                return false;
            }

            if (!isValidPercentage)
            {
                lblPercentageValid.Visible = true;
                return false;
            }
            
            else
            {
                lblNameValid.Visible = false;
                lblAddressValid.Visible = false;
                lblPhoneValid.Visible = false;
                lblPercentageValid.Visible = false;
                return true;
            }
        }

        private void textBox_TName_Leave(object sender, EventArgs e)
        {
            isValidName = BL.Utilities.validateNameInArabic(textBox_TName.Text);
            if (isValidName)
                lblNameValid.Visible = false;
        }

        private void addTeacher_Load(object sender, EventArgs e)
        {
            MinimumSize = MaximumSize = Size;
            textBox_Filter.Text = "ادخل الكود او الاسم";
            textBox_TBalance.Text = "0";
            foreach (DataGridViewColumn c in dataGridViewTeachers.Columns)
            {
                c.DefaultCellStyle.Font = new Font("Arial", 17, FontStyle.Bold, GraphicsUnit.Pixel);
                c.DefaultCellStyle.ForeColor = Color.Black;
            }
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox_TID.Text != "")
                {
                    TeacherReportID = Convert.ToInt32(textBox_TID.Text);
                    TeacherReport teacherreport = new TeacherReport(TeacherReportID);
                    teacherreport.Show();
                }
                else MessageBox.Show("اختر مدرس أولاً", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch
            {
                MessageBox.Show("حدث خطأ أثناء تشغيل البرنامج", "خطأ غير متوقع", MessageBoxButtons.OK, MessageBoxIcon.Error);
                EDPDBContext.Dispose();
                EDPDBContext = new EDPCenterEntities();
            }
        }

        private void materialButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox_TID.Text != "")
                {
                    if (textBox_TBalance.Text == "0")
                    {
                        MessageBox.Show("لا يوجد رصيد لهذا المدرس", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        EDPCenterEntities EDPDBContext2 = new EDPCenterEntities();
                        int x = Convert.ToInt32(textBox_TID.Text);
                        Teacher teacher = (from s in EDPDBContext2.Teachers
                                           where s.T_ID == x
                                           select s).FirstOrDefault();
                        addOutcomeTeacher outcomeT = new addOutcomeTeacher(teacher);
                        outcomeT.ShowDialog();
                    }
                }
                else
                {
                    MessageBox.Show("اختر مدرس أولاً", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch
            {
                MessageBox.Show("حدث خطأ أثناء تشغيل البرنامج", "خطأ غير متوقع", MessageBoxButtons.OK, MessageBoxIcon.Error);
                EDPDBContext.Dispose();
                EDPDBContext = new EDPCenterEntities();
            }
        }

        private void materialButton3_Click(object sender, EventArgs e)
        {
            EDPCenterEntities eDp = new EDPCenterEntities();
            List<Teacher> teacher = eDp.Teachers.ToList();
            NewDataGrid(teacher);
            textBox_TBalance.Text = "";
        }

        private void addTeacher_Activated(object sender, EventArgs e)
        {
            EDPCenterEntities eDp = new EDPCenterEntities();
            List<Teacher> teacher = eDp.Teachers.ToList();
            NewDataGrid(teacher);
            textBox_TBalance.Text = "0";
        }

        private void textBox_Filter_Enter(object sender, EventArgs e)
        {
            textBox_Filter.Text = "";
        }

        private void textBox_Filter_Leave(object sender, EventArgs e)
        {
            if (textBox_Filter.Text.Length == 0)
                textBox_Filter.Text = "ادخل الكود او الاسم";
        }

        private void addTeacher_FormClosed(object sender, FormClosedEventArgs e)
        {
            EDPDBContext.Dispose();
        }

        private void textBox_TPhone_Leave(object sender, EventArgs e)
        {
            isValidPhone = BL.Utilities.ValidatPhoneNumber(textBox_TPhone.Text);
            if (isValidPhone)
                lblPhoneValid.Visible = false;
        }
        private void textBox_TAddress_Leave(object sender, EventArgs e)
        {
            isValidAddress = BL.Utilities.validateNameInArabic(textBox_TAddress.Text);
            if (isValidAddress)
                lblAddressValid.Visible = false;
        }
        private void textBox_TPercentage_Leave(object sender, EventArgs e)
        {
            isValidPercentage =
                   BL.Utilities.checkDoubleNumber(textBox_TBalance.Text)
                   && textBox_TPercentage.Text.Length > 0
                   && double.Parse(textBox_TPercentage.Text) >= 0.0 && double.Parse(textBox_TPercentage.Text) <= 100.0 ? true : false;
            if (isValidPercentage)
                lblPercentageValid.Visible = false;
        }
        private void textBox_TBalance_Leave(object sender, EventArgs e)
        {
            isValidCash = BL.Utilities.checkDoubleNumber(textBox_TBalance.Text);
            if (isValidCash)
                lblBalanceValidation.Visible = false;
        }
    }
}