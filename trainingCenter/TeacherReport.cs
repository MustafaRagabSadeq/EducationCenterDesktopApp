using DGVPrinterHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace trainingCenter
{
    public partial class TeacherReport : MetroSet_UI.Forms.MetroSetForm
    {
        EDPCenterEntities eDPCenterEntities;
        int TeacherReportIDs = 0;
        List<GroupName> groupName = null; 

        public TeacherReport(int TeacherReportID)
        {
            InitializeComponent();
            
            TeacherReportIDs =TeacherReportID;
        }

        private void TeacherReport_Load(object sender, EventArgs e)
        {
            MinimumSize = MaximumSize = Size;
            eDPCenterEntities = new EDPCenterEntities();
            textBox_TID.Text = TeacherReportIDs.ToString();
            Teacher teacher = eDPCenterEntities.Teachers.Where(a=>a.T_ID== TeacherReportIDs).FirstOrDefault();
            
            textBox_TName.Text=teacher.T_Name;
            textBox_TPhone.Text = teacher.T_Phone;
            textBox_TPercentage.Text=(teacher.T_Income_Percent*100).ToString();
            textBox_TBalance.Text=teacher.T_Balance.ToString();
            groupName =eDPCenterEntities.GroupNames.Where(a=>a.Teacher_ID== TeacherReportIDs).ToList();
            txtGroupsCount.Text= groupName.Count.ToString();
            NewDataGrid(groupName);
            foreach (DataGridViewColumn c in dgvTecherReport.Columns)
            {
                c.DefaultCellStyle.Font = new Font("Arial", 17, FontStyle.Bold, GraphicsUnit.Pixel);
                c.DefaultCellStyle.ForeColor = Color.Black;
            }
        }
        private void NewDataGrid(List<GroupName> groupNames)
        {
            dgvTecherReport.Rows.Clear();

            foreach (GroupName group in groupNames)
            {
                int noOfStuds = eDPCenterEntities.Student_Group.Where(x=>x.G_ID==group.G_ID).Count();
                DataGridViewRow row = (DataGridViewRow)dgvTecherReport.Rows[0].Clone();
                row.Cells[0].Value = group.G_ID;
                row.Cells[1].Value = group.G_Name;
                row.Cells[2].Value = group.Teacher.T_Name;
                row.Cells[3].Value = group.Grade;
                row.Cells[4].Value = group.AcademicYear.Name;
                row.Cells[5].Value = group.G_DateOFCreation.ToString();
                row.Cells[6].Value = noOfStuds.ToString();
                row.Cells[7].Value = group.G_PriceOfSession.ToString();
                row.Cells[8].Value = group.G_NoOfSession.ToString();
                row.Cells[9].Value = group.G_TotalPrice.ToString();
                dgvTecherReport.Rows.Add(row);
            }
        }
        private void metroSetButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void materialDelete_Click(object sender, EventArgs e)
        {
            try
            {
                DGVPrinter printer = new DGVPrinter();
                printer.Title = ($"  تقرير مجموعات استاذ  {textBox_TName.Text} ");
                printer.SubTitle = ($"الرصيد =  {textBox_TBalance.Text}  ,عدد المجموعات =  {txtGroupsCount.Text}  النسبة %  =  {textBox_TPercentage.Text} ");
                printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
                printer.PageNumbers = true;
                printer.PageNumberInHeader = false;
                printer.PorportionalColumns = true;
                printer.HeaderCellAlignment = StringAlignment.Near;
                printer.Footer = "منصة تطوير التعليم - EDP Training Center";
                printer.FooterSpacing = 15;
                printer.ColumnWidth = DGVPrinter.ColumnWidthSetting.DataWidth;
                printer.TableAlignment = DGVPrinter.Alignment.Center;
                printer.PrintNoDisplay(dgvTecherReport);
            }
            catch
            {
                MessageBox.Show("حدث خطأ أثناء تشغيل البرنامج", "خطأ غير متوقع", MessageBoxButtons.OK, MessageBoxIcon.Error);
                eDPCenterEntities.Dispose();
                eDPCenterEntities = new EDPCenterEntities();
            }
        }

        private void TeacherReport_FormClosed(object sender, FormClosedEventArgs e)
        {
            eDPCenterEntities.Dispose();
        }

        private void btnSearchDay_Click(object sender, EventArgs e)
        {
            TeacherReport_Load(null,null);
        }
    }
}