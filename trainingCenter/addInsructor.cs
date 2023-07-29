using trainingCenter.BL;
using MetroSet_UI.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Reflection;
using System.Runtime.Remoting.Contexts;


namespace trainingCenter

{
    public partial class addInstructor : MetroSetForm
    {
        EDPCenterEntities eDPCenterEntities;
        bool isValidName;
        bool isValidPhone;
        public addInstructor()
        {
            InitializeComponent();
            eDPCenterEntities = new EDPCenterEntities();
        }

        #region checkValidation Functaion
        public bool checkValidation()
        {

            isValidName = Utilities.validateNameInArabic(txtbInstructorName.Text);
            isValidPhone = Utilities.ValidatPhoneNumber(txtbInstructorPhone.Text);

            if (!isValidName)
            {
                lblValidNameArabic.Visible = true;
                return false;
            }
            if (!isValidPhone)
            {
                lblValidPhoneNumber.Visible = true;
                return false;
            }
            else
            {
                lblValidNameArabic.Visible = false;
                lblValidPhoneNumber.Visible = false;
                return true;
            }
        }
        #endregion

        #region FormLoad
        private void addInstructor_Load(object sender, EventArgs e)
        {
            MinimumSize = MaximumSize = Size;
            #region [نص البحث]
            if (txtbInstractureSearch.Text.Length == 0)
                txtbInstractureSearch.Text = "ادخل الكود او الاسم";
            #endregion

            #region TO get row data from DB &Added in dgv form
            EDPCenterEntities x = new EDPCenterEntities();
            List<Instructor> instr = x.Instructors.ToList();
            NewDataGrid(instr);
            #endregion
            foreach (DataGridViewColumn c in dgvInstructure.Columns)
            {
                c.DefaultCellStyle.Font = new Font("Arial", 17, FontStyle.Bold, GraphicsUnit.Pixel);
                c.DefaultCellStyle.ForeColor = Color.Black;
            }
        } 
        #endregion

        #region button [اضافه]
        private void addinstructor_btbevent(object sender, EventArgs e)
        {
            try
            {
                if (checkValidation())
                {
                    string Name = txtbInstructorName.Text;
                    string phone = txtbInstructorPhone.Text;
                    Instructor instructory = eDPCenterEntities.Instructors.Where(x => x.Name == Name).Where(y => y.Phone == phone).FirstOrDefault();

                    if (instructory == null)
                    {
                        Instructor instructor = new Instructor()
                        {
                            Name = txtbInstructorName.Text,
                            Phone = txtbInstructorPhone.Text
                        };
                        eDPCenterEntities.Instructors.Add(instructor);
                        eDPCenterEntities.SaveChanges();
                        MessageBox.Show("تم اضافة المدرب بنجاح", "عملية ناجحة", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        List<Instructor> instructors = eDPCenterEntities.Instructors.ToList();
                        NewDataGrid(instructors);
                    }
                    else
                        MessageBox.Show($"هذا المدرب موجود بالفعل و كوده هو {instructory.ID}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        
        #region button [البحث]
        private void btnserche(object sender, EventArgs e)
        {
            // check if search field is empty
            if (txtbInstractureSearch.Text.Length == 0)
                MessageBox.Show("ادخل قية في البحث", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (txtbInstractureSearch.Text == "ادخل الكود او الاسم")
                MessageBox.Show("ادخل قية في البحث", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //get Data from DB
            else
            {
                int InstId;
                bool isNumber = int.TryParse(txtbInstractureSearch.Text, out InstId);

                List<Instructor> instructor;
                if (isNumber)
                {
                    instructor = eDPCenterEntities.Instructors.Where(a => a.ID == InstId).ToList();
                }
                else
                {
                    instructor = eDPCenterEntities.Instructors.Where(a => a.Name.Contains(txtbInstractureSearch.Text)).ToList();
                }
                if (instructor.Count > 0)
                    NewDataGrid(instructor);
                else
                    MessageBox.Show("لا توجد نتائج", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion
       
        #region button[تعديل]
        private void materialButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtbInstructorID.Text.Length == 0)
                    MessageBox.Show("اختر المدرب أولاً", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    if (checkValidation())
                    {

                        int InstId = int.Parse(txtbInstructorID.Text);
                        Instructor instructor = eDPCenterEntities.Instructors.Where(x => x.ID == InstId).FirstOrDefault();
                        instructor.Name = txtbInstructorName.Text;
                        instructor.Phone = txtbInstructorPhone.Text;

                        eDPCenterEntities.SaveChanges();
                        NewDataGrid(eDPCenterEntities.Instructors.ToList());
                        MessageBox.Show("تم تعديل البيانات بنجاح", "عملية ناجحة", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        #endregion
         
        #region Button [حذف]
        private void materialButton3_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtbInstructorID.Text.Length == 0)
                    MessageBox.Show("اختر المدرب أولاً", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    DialogResult dialogResult = MessageBox.Show("هل أنت متأكد من الحذف", "حذف مدرب", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        int InstId = int.Parse(txtbInstructorID.Text);
                        Instructor instructor = eDPCenterEntities.Instructors.Where(x => x.ID == InstId).FirstOrDefault();
                        eDPCenterEntities.Instructors.Remove(instructor);
                        eDPCenterEntities.SaveChanges();
                        NewDataGrid(eDPCenterEntities.Instructors.ToList());
                        MessageBox.Show("تم حذف المدرب بنجاح", "عملية ناجحة", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        #endregion

        #region dataGridView1_CellClick
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            DataGridViewRow row ;
            // To determine row number
            if (e.RowIndex >= 0)
            {
                row = (DataGridViewRow)dgvInstructure.Rows[index];
                txtbInstructorID.Text = row.Cells[0].Value?.ToString() ?? "";
                txtbInstructorName.Text = row.Cells[1].Value?.ToString() ?? "";
                txtbInstructorPhone.Text = row.Cells[2].Value?.ToString() ?? "";
            }                     
            index = 0;
            row = (DataGridViewRow)dgvInstructure.Rows[index];
        }
        #endregion

        #region [مربع البحث النص]
        private void txtbSearch_Enter(object sender, EventArgs e)
        {
            txtbInstractureSearch.Text = "";
        }
        private void txtbSearch_Leave(object sender, EventArgs e)
        {
            if(txtbInstractureSearch.Text=="")
            txtbInstractureSearch.Text = "ادخل الكود او الاسم";
        }
        #endregion
        
        #region Button[عرض الكل]
        private void btnShowAll_Click(object sender, EventArgs e)
        {
            NewDataGrid(eDPCenterEntities.Instructors.ToList());
        }
        #endregion

        #region [TextBoxLeave(Name,Phone)Validation]
        private void instructorName_Leave(object sender, EventArgs e)
        {            
            checkValidation();
        }
        private void phoneinstructor_Leave(object sender, EventArgs e)
        {            
            checkValidation();
        }
        #endregion

        #region NewdataGrid Function
        private void NewDataGrid(List<Instructor> instructors)
        {
            txtbInstructorID.Text = "";
            txtbInstructorName.Text = "";
            txtbInstructorPhone.Text = "";
            dgvInstructure.Rows.Clear();

            foreach (Instructor instr in instructors)
            {
                DataGridViewRow row = (DataGridViewRow)dgvInstructure.Rows[0].Clone();
                row.Cells[0].Value = instr.ID;
                row.Cells[1].Value = instr.Name;
                row.Cells[2].Value = instr.Phone;
                dgvInstructure.Rows.Add(row);
            }
        }
        #endregion

        #region WhenLeaveTXT
        private void txtbInstructorName_Enter(object sender, EventArgs e)
        {
            txtbInstructorName.Text = "";
        }
        private void txtbInstructorPhone_Enter(object sender, EventArgs e)
        {
            txtbInstructorPhone.Text = "";
        }
        #endregion

        private void metroSetButton4_Click(object sender, EventArgs e)
        {           
            this.Close();
        }

        private void addInstructor_FormClosed(object sender, FormClosedEventArgs e)
        {
            eDPCenterEntities.Dispose();
        }
    }
}
