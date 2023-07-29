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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using DGVPrinterHelper;

namespace trainingCenter
{
    public partial class StudentReport : MetroSetForm
    {
        int Student_IDR=0;

        EDPCenterEntities eDPCenterEntities;

        public StudentReport(int Student_ID)
        {
            InitializeComponent();
            eDPCenterEntities = new EDPCenterEntities();
            Student_IDR = Student_ID;
           

        }

        private void StudentReport_Load(object sender, EventArgs e)
        {
            MinimumSize = MaximumSize = Size;
            stu_IDBox.Text = Student_IDR.ToString();
            Student _student = eDPCenterEntities.Students.Where(a => a.St_ID == Student_IDR).FirstOrDefault();
            stuNameBox.Text = _student.St_Name.ToString();
            phoneBox.Text = _student.St_Phone.ToString();
            List<GroupName> groups = (from s in eDPCenterEntities.Student_Group
                                      from g in eDPCenterEntities.GroupNames
                                      where (s.G_ID == g.G_ID && s.St_ID == Student_IDR)
                                      select g).ToList();
            GroupsBox.DataSource = groups;
            GroupsBox.ValueMember = "G_ID";
            GroupsBox.DisplayMember = "G_Name";
            foreach (DataGridViewColumn c in dataGridView1.Columns)
            {
                c.DefaultCellStyle.Font = new Font("Arial", 17, FontStyle.Bold, GraphicsUnit.Pixel);
                c.DefaultCellStyle.ForeColor = Color.Black;
            }
        }
        private void GroupsBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int groupId = Convert.ToInt32(GroupsBox.SelectedValue.ToString());
            List<Attendence> att = (from at in eDPCenterEntities.Attendences
                                    where (at.G_ID == groupId && at.St_ID == Student_IDR)
                                    select at).ToList();

            NewDataGrid(att);
        }

        private void NewDataGrid(List<Attendence> attendences)
        {

            dataGridView1.Rows.Clear();
            foreach (Attendence attendence in attendences)
            {
                string studName = eDPCenterEntities.Students.Where(x => x.St_ID == attendence.St_ID).FirstOrDefault().St_Name;
                GroupName groupName = eDPCenterEntities.GroupNames.Where(x => x.G_ID == attendence.G_ID).FirstOrDefault();
                Teacher teacherName = eDPCenterEntities.Teachers.Where(x => x.T_ID == groupName.Teacher_ID).FirstOrDefault();

                DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[0].Clone();
                row.Cells[0].Value = attendence.St_ID;
                row.Cells[1].Value = studName;
                row.Cells[2].Value = teacherName.T_Name;
                row.Cells[3].Value = groupName.G_Name;
                row.Cells[4].Value = attendence.Att_Date;
                //row.Cells[5].Value = attendence.Payment_State;

                if (attendence.Payment_State == false)
                {
                    row.Cells[5].Value = "غير دافع";
                }
                else
                {
                    row.Cells[5].Value = "دافع";


                }

                dataGridView1.Rows.Add(row);
            }

        }

        private void metroSetButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void materialButton3_Click(object sender, EventArgs e)
        {
            try
            {
                DGVPrinter printer = new DGVPrinter();
                printer.Title = ("  تقرير حضور الطالب ");
                printer.SubTitle = ("العرض حسب المجموعة");
                printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
                printer.PageNumbers = true;
                printer.PageNumberInHeader = false;
                printer.PorportionalColumns = true;
                printer.HeaderCellAlignment = StringAlignment.Near;
                printer.Footer = "منصة تطوير التعليم - EDP Training Center";
                printer.FooterSpacing = 15;
                printer.ColumnWidth = DGVPrinter.ColumnWidthSetting.DataWidth;
                printer.TableAlignment = DGVPrinter.Alignment.Center;
                printer.PrintNoDisplay(dataGridView1);
            }
            catch
            {
                MessageBox.Show("حدث خطأ أثناء تشغيل البرنامج، الرجاء التأكد من اغلاق الملف الذي تريد استبداله", "خطأ غير متوقع", MessageBoxButtons.OK, MessageBoxIcon.Error);
                eDPCenterEntities.Dispose();
                eDPCenterEntities = new EDPCenterEntities();
            }
        }

        private void StudentReport_FormClosed(object sender, FormClosedEventArgs e)
        {
            eDPCenterEntities.Dispose();
        }

        private void btnSearchDay_Click(object sender, EventArgs e)
        {
            StudentReport_Load(null, null);

            eDPCenterEntities.Dispose();
            eDPCenterEntities= new EDPCenterEntities();
            int groupId = Convert.ToInt32(GroupsBox.SelectedValue.ToString());
            List<Attendence> att = (from at in eDPCenterEntities.Attendences
                                    where (at.G_ID == groupId && at.St_ID == Student_IDR)
                                    select at).ToList();

            NewDataGrid(att);
        }
    }
}