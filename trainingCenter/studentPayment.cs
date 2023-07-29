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
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Contexts;
using System.Xml.Linq;
using System.Data.Entity;

namespace trainingCenter
{
    public partial class studentPayment : MetroSetForm
    {
        bool isValidCash;
        Attendence _Attendence;
        Student_Group student_Group;
        string transaction_type = "إيرادات";

        EDPCenterEntities eDPCenterEntities;
        public studentPayment(Attendence attendence)
        {
            InitializeComponent();
            eDPCenterEntities = new EDPCenterEntities();
            _Attendence = attendence;
            student_Group = eDPCenterEntities.Student_Group.Where(x => x.St_ID == _Attendence.St_ID && x.G_ID == _Attendence.G_ID).FirstOrDefault();
        }
        private bool checkValidation()
        {

            isValidCash = Utilities.checkDoubleNumber(cashTextBox.Text);
            if (isValidCash)
            {
                cashValidation.Visible = false;
                return true;
            }
            else
            {
                cashValidation.Visible = true;
                return false;
            }
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (checkValidation())
                {
                    DialogResult dialogResult = MessageBox.Show("التأكيد على الدفع؟", "! تحذير", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        double cach = double.Parse(cashTextBox.Text);
                        if (cach > 0)
                        {
                            Daily_Transaction daily_Transactions;
                            if (student_Group != null && student_Group.Payment_Method == "الشهر")
                            {
                                student_Group.St_Balance += cach;

                                daily_Transactions = new Daily_Transaction()
                                {
                                    Person_ID = student_Group.St_ID,
                                    Name = $"دفع الطالب {student_Group.Student.St_Name} لمجموعة {student_Group.GroupName.G_Name}",
                                    Price = double.Parse(cashTextBox.Text),
                                    Transaction_Type = transaction_type,
                                    Date = DateTime.Now
                                };
                                eDPCenterEntities.Daily_Transaction.Add(daily_Transactions);
                                eDPCenterEntities.SaveChanges();
                            }
                            else
                            {
                                GroupName gName1 = eDPCenterEntities.GroupNames.Where(x => x.G_ID == _Attendence.G_ID).FirstOrDefault();

                                gName1.Teacher.T_Balance = (double)(gName1.Teacher.T_Balance + (gName1.G_PriceOfSession * gName1.Teacher.T_Income_Percent));

                                Student st1 = eDPCenterEntities.Students.Where(x => x.St_ID == _Attendence.St_ID).FirstOrDefault();

                                balanceBox.Text = "0";
                                label3.Text = "ثمن الحصة";
                                cashTextBox.Text = gName1.G_PriceOfSession.ToString();
                                cashTextBox.Enabled = false;

                                daily_Transactions = new Daily_Transaction()
                                {
                                    Person_ID = _Attendence.St_ID,
                                    Date = DateTime.Now,
                                    //- (gName1.G_PriceOfSession * gName1.Teacher.T_Income_Percent)
                                    Price = gName1.G_PriceOfSession ,
                                    Transaction_Type = transaction_type,
                                    Name = $"دفع الطالب {st1.St_Name} لمجموعة {gName1.G_Name} بالحصة"
                                };
                                eDPCenterEntities.Daily_Transaction.Add(daily_Transactions);
                                eDPCenterEntities.SaveChanges();
                            }


                            _Attendence.Payment_State = true;
                            Attendence tempAttend = eDPCenterEntities.Attendences.Find(_Attendence.St_Att_ID);

                            //التعديل على الكود 
                            if ((student_Group == null || student_Group.St_Balance >= 0) && tempAttend != null)
                                tempAttend.Payment_State = true;

                            eDPCenterEntities.SaveChanges();
                            MessageBox.Show("تم الدفع بنجاح", "عملية ناجحة", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("ادخل قيمة صحيحة", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

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

        private void studentPayment_Load(object sender, EventArgs e)
        {
            MinimumSize = MaximumSize = Size;
            string groupName = eDPCenterEntities.GroupNames.Where(x=>x.G_ID==_Attendence.G_ID).FirstOrDefault().G_Name;
            string studentName = eDPCenterEntities.Students.Where(x=>x.St_ID==_Attendence.St_ID).FirstOrDefault().St_Name;
            stuNameBox.Text = studentName;
            groupNameBox.Text= groupName;
            stu_IDBox.Text = _Attendence.St_ID.ToString();
            if (student_Group != null && student_Group.Payment_Method=="الشهر")
            {
                balanceBox.Text = student_Group.St_Balance.ToString();
            }
            else
            {
                GroupName gName = eDPCenterEntities.GroupNames.Where(x=>x.G_ID==_Attendence.G_ID).FirstOrDefault();
                balanceBox.Text = "0";
                label3.Text = "ثمن الحصة";
                cashTextBox.Text = gName.G_PriceOfSession.ToString();
                cashTextBox.Enabled = false;
            }
        }

        private void studentPayment_FormClosed(object sender, FormClosedEventArgs e)
        {
            eDPCenterEntities.Dispose();
        }
    }
}