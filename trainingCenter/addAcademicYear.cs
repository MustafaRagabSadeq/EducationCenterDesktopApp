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
    public partial class addAcademicYear : MetroSetForm
    {
        bool isValidYear;

        EDPCenterEntities eDPCenterEntities;
        public addAcademicYear()
        {
            InitializeComponent();
            eDPCenterEntities = new EDPCenterEntities();
        }

        private bool checkValidation()
        {
            isValidYear = Utilities.checkDropDownList(academicYearBox.SelectedItem);
            if (!isValidYear)
            {
                label15.Visible = true;
                return false;
            }
            else
            {
                return true;
            }
        }
        private void NewDataGrid(List<AcademicYear> academicYears)
        {
            yearIdBox.Text = "";
            academicYearBox.SelectionStart = academicYearBox.Text.Length;
            textBox2.Text = "";
            dataGridView1.Rows.Clear();
            foreach (AcademicYear academicYear in academicYears)
            {
                DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[0].Clone();
                row.Cells[0].Value = academicYear.ID;
                row.Cells[1].Value = academicYear.Name;
                dataGridView1.Rows.Add(row);
            }
        }
        private void addAcademicYear_Load(object sender, EventArgs e)
        {
            MinimumSize = MaximumSize = Size;
            List<AcademicYear> academicYears = eDPCenterEntities.AcademicYears.ToList();
            NewDataGrid(academicYears);
            if (textBox2.Text.Length == 0)
                textBox2.Text = "ادخل الكود او الاسم";
            foreach (DataGridViewColumn c in dataGridView1.Columns)
            {
                c.DefaultCellStyle.Font = new Font("Arial", 17, FontStyle.Bold, GraphicsUnit.Pixel);
                c.DefaultCellStyle.ForeColor = Color.Black;
            }
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            if (checkValidation())
            {
                try
                {
                    // check if the year already exsist in database
                    var query = eDPCenterEntities.AcademicYears.Where(x => x.Name == academicYearBox.Text).FirstOrDefault();
                    if (query != null)
                    {
                        MessageBox.Show("الصف الدراسى موجود بالفعل", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        AcademicYear academic = new AcademicYear() { Name = academicYearBox.Text };

                        eDPCenterEntities.AcademicYears.Add(academic);
                        eDPCenterEntities.SaveChanges();
                        List<AcademicYear> academicYears = eDPCenterEntities.AcademicYears.ToList();
                        NewDataGrid(academicYears);
                        MessageBox.Show("تم اضافة الصف الدراسي", "عملية ناجحة", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                }
                catch
                {
                    MessageBox.Show("حدث خطأ أثناء تشغيل البرنامج", "خطأ غير متوقع", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    eDPCenterEntities.Dispose();
                    eDPCenterEntities = new EDPCenterEntities();
                }               
            }
        }

        private void materialButton3_Click(object sender, EventArgs e)
        {
            try
            {
                if (yearIdBox.Text.Length > 0)
                {
                    DialogResult dialogResult = MessageBox.Show("التأكيد على حذف المادة؟", "! تحذير", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        int yearId = int.Parse(yearIdBox.Text);
                        GroupName group = eDPCenterEntities.GroupNames.Where(x => x.AcademicYear_ID == yearId).FirstOrDefault();

                        Teacher_Year teacher = eDPCenterEntities.Teacher_Year.Where(x => x.AcademicYear_ID == yearId).FirstOrDefault();


                        if (group == null && teacher == null)
                        {
                            AcademicYear academicYear = eDPCenterEntities.AcademicYears.Where(x => x.ID == yearId).FirstOrDefault();
                            eDPCenterEntities.AcademicYears.Remove(academicYear);
                            eDPCenterEntities.SaveChanges();
                            NewDataGrid(eDPCenterEntities.AcademicYears.ToList());
                            MessageBox.Show("تم حذف الصف الدراسي", "عملية ناجحة", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("لا يمكنك حذف الصف الدراسي", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                    }
                }
                else
                {
                    MessageBox.Show("اختر الصف الدراسي أولاً", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("ادخل قيمة في البحث", "خطأ في البحث", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (textBox2.Text == "ادخل الكود او الاسم")
                MessageBox.Show("ادخل قيمة في البحث", "خطأ في البحث", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                int yearId;
                bool isNumber = int.TryParse(textBox2.Text, out yearId);

                List<AcademicYear> academicYears;
                if (isNumber)
                {
                    academicYears = eDPCenterEntities.AcademicYears.Where(a => a.ID == yearId).ToList();

                }
                else
                {
                    academicYears = eDPCenterEntities.AcademicYears.Where(a => a.Name.Contains(textBox2.Text)).ToList();                }
                if (academicYears.Count > 0)
                    NewDataGrid(academicYears);
                else
                    MessageBox.Show("لا توجد نتائج", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void materialButton6_Click(object sender, EventArgs e)
        {
            NewDataGrid(eDPCenterEntities.AcademicYears.ToList());
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;        

            if (index >= 0)
            {
                DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[index];
                if (row.Cells[0].Value != null)
                {
                    yearIdBox.Text = row.Cells[0].Value.ToString();
                    academicYearBox.Text = row.Cells[1].Value.ToString();
                }
                else
                {
                    yearIdBox.Text = "";
                }
            }
        }


        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (textBox2.Text.Length==0)
            {
                textBox2.Text = "ادخل الكود او الاسم";
            }
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            textBox2.Text = "";
        }

        private void addAcademicYear_FormClosed(object sender, FormClosedEventArgs e)
        {
            eDPCenterEntities.Dispose();
        }

        private void addAcademicYear_Activated(object sender, EventArgs e)
        {
            
        }
    }
}
