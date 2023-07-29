using MetroSet_UI.Forms;
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
using trainingCenter.BL;

namespace trainingCenter
{
    public partial class addSubject : MetroSetForm
    {
        bool isValidSubject;

        EDPCenterEntities eDPCenterEntities;
        public addSubject()
        {
            InitializeComponent();
            eDPCenterEntities = new EDPCenterEntities();

        }
        private bool checkValidation()
        {
            isValidSubject= Utilities.validateNameInArabic(subNameBox.Text);
            if (!isValidSubject)
            {
                label12.Visible= true;
                return false;
            }
            else
            {
                label12.Visible= false;
                return true;
            }
        }
        
        private void addSubject_Load(object sender, EventArgs e)
        {
            MinimumSize = MaximumSize = Size;
            List<Subject> subjects = eDPCenterEntities.Subjects.ToList();
            NewDataGrid(subjects);
            if (textBox2.Text.Length == 0)
                textBox2.Text = "ادخل الكود او الاسم";
            foreach (DataGridViewColumn c in dataGridView1.Columns)
            {
                c.DefaultCellStyle.Font = new Font("Arial", 17, FontStyle.Bold, GraphicsUnit.Pixel);
                c.DefaultCellStyle.ForeColor = Color.Black;
            }
        }

        private void NewDataGrid(List<Subject> subjects)
        {
            subIdBox.Text = "";
            subNameBox.Text = "";            

            dataGridView1.Rows.Clear();

            foreach (Subject subject in subjects)
            {
                DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[0].Clone();
                row.Cells[0].Value = subject.Sub_ID;
                row.Cells[1].Value = subject.Sub_Name;
                dataGridView1.Rows.Add(row);
            }
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (subNameBox.Text.Length > 0)
                {
                    if (checkValidation())
                    {
                        var query = eDPCenterEntities.Subjects.Where(x => x.Sub_Name == subNameBox.Text).FirstOrDefault();
                        if (query != null)
                        {
                            MessageBox.Show("المادة موجودة بالفعل", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            eDPCenterEntities.Subjects.Add(new Subject { Sub_Name = subNameBox.Text });
                            eDPCenterEntities.SaveChanges();
                            List<Subject> subjects = eDPCenterEntities.Subjects.ToList();
                            NewDataGrid(subjects);
                            MessageBox.Show("تم اضافة المادة بنجاح", "عملية ناجحة", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            label12.Visible = false;
                        }
                    }

                }
                else
                {
                    label12.Visible = true;
                }
            }
            catch
            {
                MessageBox.Show("حدث خطأ أثناء تشغيل البرنامج", "خطأ غير متوقع", MessageBoxButtons.OK, MessageBoxIcon.Error);
                eDPCenterEntities.Dispose();
                eDPCenterEntities = new EDPCenterEntities();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            if (index >= 0)
            {
                DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[index];
                if (row.Cells[0].Value != null)
                {
                    subIdBox.Text = row.Cells[0].Value.ToString();
                    subNameBox.Text = row.Cells[1].Value.ToString();
                }
                else
                {
                    subIdBox.Text = "";
                    subNameBox.Text = "";
                }
            }
            else
            {
                subIdBox.Text = "";
                subNameBox.Text = "";
            }
        }

        private void materialButton3_Click(object sender, EventArgs e)
        {
            try
            {
                if (subIdBox.Text.Length > 0)
                {
                    DialogResult dialogResult = MessageBox.Show("التأكيد على حذف المادة؟", "! تحذير", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        int studId = int.Parse(subIdBox.Text);
                        Teacher_Subject teacher = eDPCenterEntities.Teacher_Subject.Where(x => x.Subject_ID == studId).FirstOrDefault();
                        GroupName group = eDPCenterEntities.GroupNames.Where(x => x.Sub_ID == studId).FirstOrDefault();
                        if (teacher == null && group == null)
                        {
                            Subject subject = eDPCenterEntities.Subjects.Where(x => x.Sub_ID == studId).FirstOrDefault();
                            eDPCenterEntities.Subjects.Remove(subject);
                            eDPCenterEntities.SaveChanges();
                            NewDataGrid(eDPCenterEntities.Subjects.ToList());
                            MessageBox.Show("تم حذف المادة بنجاح", "عملية ناجحة", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("لا يمكنك حذف المادة الدراسية !", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("اختر المادة أولاً", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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
            if (textBox2.Text.Length == 0)
                MessageBox.Show("ادخل قيمة في البحث", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (textBox2.Text == "ادخل الكود او الاسم")
                MessageBox.Show("ادخل قيمة في البحث", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                int subId;
                bool isNumber = int.TryParse(textBox2.Text, out subId);
                List<Subject> subjects;
                if (isNumber)
                {
                    subjects = eDPCenterEntities.Subjects.Where(a => a.Sub_ID == subId).ToList();
                }
                else
                {
                    subjects = eDPCenterEntities.Subjects.Where(a => a.Sub_Name.Contains(textBox2.Text)).ToList();
                }
                if (subjects.Count > 0)
                    NewDataGrid(subjects);
                else
                    MessageBox.Show("لا توجد نتائج", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox2.Text = "ادخل الكود أو الاسم";
            }
        }

        private void materialButton6_Click(object sender, EventArgs e)
        {
            NewDataGrid(eDPCenterEntities.Subjects.ToList());
            textBox2.Text = "ادخل الكود أو الاسم";
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            textBox2.Text = "";
        }
        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (textBox2.Text.Length == 0)
                textBox2.Text = "ادخل الكود أو الاسم";
        }

        private void subNameBox_Leave(object sender, EventArgs e)
        {
            isValidSubject= Utilities.validateNameInArabic(subNameBox.Text);
            if (isValidSubject)
            {
                label12.Visible= false;              
            }
        }

        private void addSubject_FormClosed(object sender, FormClosedEventArgs e)
        {
            eDPCenterEntities.Dispose();
        }
    }
}