using MetroSet_UI.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using trainingCenter.BL;

namespace trainingCenter
{

    public partial class addOutcomeTeacher : MetroSetForm
    {
        EDPCenterEntities context = new EDPCenterEntities();
        public addOutcomeTeacher()
        {
            InitializeComponent();
        }
        private Teacher _teacher;
        public addOutcomeTeacher(Teacher t)
        {
            InitializeComponent();
            _teacher = t;
        }

        private void addOutComeTeacher_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.None;
            MaximumSize = MinimumSize = Size;
            txtTeacherID.Text= _teacher.T_ID.ToString();
            txtTname.Text= _teacher.T_Name;
            txtTbalance.Text= _teacher.T_Balance.ToString();
        }       

        private void btnok_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtMonaytoOut.Text.Length > 0)
                {
                    double money;
                    bool Isvalid = double.TryParse(txtMonaytoOut.Text, out money);
                    if (Isvalid && money > 0)
                    {
                        if (money <= Convert.ToDouble(txtTbalance.Text))
                        {
                            Teacher tech = context.Teachers.Where(a => a.T_ID == _teacher.T_ID).FirstOrDefault();
                            tech.T_Balance -= money;
                            context.SaveChanges();
                            Daily_Transaction daily = new Daily_Transaction()
                            {
                                Person_ID = _teacher.T_ID,
                                Name = ($"تم سحب رصيد للمدرس {_teacher.T_Name}"),
                                Price = money,
                                Transaction_Type = "مصروفات",
                                Date = DateTime.Now
                            };
                            context.Daily_Transaction.Add(daily);
                            context.SaveChanges();
                            MessageBox.Show("تم سحب المبلغ بنجاح", "عملية ناجحة", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("المبلغ المسحوب اكبر من الرصيد", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show("ادخل قيمة صحيحة", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("ادخل قيمة المبلغ المطلوب سحبه", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch
            {
                MessageBox.Show("حدث خطأ أثناء تشغيل البرنامج", "خطأ غير متوقع", MessageBoxButtons.OK, MessageBoxIcon.Error);
                context.Dispose();
                context = new EDPCenterEntities();
            }
        }
        private void btncancel_Click(object sender, EventArgs e)
        {           
            this.Close(); 
        }

        private void addOutcomeTeacher_FormClosed(object sender, FormClosedEventArgs e)
        {
            context.Dispose();
        }
    } 
}
