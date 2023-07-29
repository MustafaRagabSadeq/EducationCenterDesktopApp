using MetroSet_UI.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using trainingCenter.BL;
using DGVPrinterHelper;

namespace trainingCenter
{
    public partial class totalIncomes : MetroSetForm
    {
        bool isValidName;
        bool isValidNumber;
        string transactionType = "مصروفات";
        double totalRevenue = 0d, totalExpenses = 0d;
        int index = 0;


        EDPCenterEntities eDPCenterEntities;
        public totalIncomes()
        {
            InitializeComponent();
            eDPCenterEntities = new EDPCenterEntities();

            // set date to current date
            dateTimePickerStart.Text = DateTime.Now.ToString();
        }

        // validate fields
       private bool checkValidation()
        {
            isValidName = Utilities.validateNameWithNumberInArabic(nameBox.Text);
            isValidNumber = Utilities.checkDoubleNumber(numberBox.Text);

            if (!isValidName)
            {
                label3.Visible =true;
                return false;
            }
            if (!isValidNumber)
            {
                label12.Visible =true;
                return false;
            }
            else
            {
                label3.Visible =false;
                label12.Visible =false;
                return true;
            }
        }

        private void nameBox_Leave(object sender, EventArgs e)
        {
            if (!isValidName)
            {
                label3.Visible =false;              
            }
        }

        private void numberBox_Leave(object sender, EventArgs e)
        {
            if (!isValidNumber)
            {
                label12.Visible =false;
               
            }

        }


        private void addoutcomes_Load(object sender, EventArgs e)
        {
            MinimumSize = MaximumSize = Size;
            if (textBoxItem.Text.Length == 0)
                textBoxItem.Text = "ادخل التاريخ (الشهر-اليوم-السنة) او البند";

            List<Total_Transaction> total_Transactions= eDPCenterEntities.Total_Transaction.ToList();
            NewDataGrid(total_Transactions);
            if (textBoxItem.Text.Length == 0)
                textBoxItem.Text = "ادخل التاريخ (الشهر-اليوم-السنة) او البند";
            foreach (DataGridViewColumn c in dataGridViewTotalTransactions.Columns)
            {
                c.DefaultCellStyle.Font = new Font("Arial", 17, FontStyle.Bold, GraphicsUnit.Pixel);
                c.DefaultCellStyle.ForeColor = Color.Black;
            }
        }


        private void materialButton1_Click(object sender, EventArgs e)
        {

            try
            {
                if (checkValidation())
                {
                    double price = double.Parse(numberBox.Text);
                    Total_Transaction total_Transaction = new Total_Transaction()
                    {
                        Name = nameBox.Text,
                        Price = price,
                        Date = DateTime.Now,
                        Transaction_Type = transactionType

                    };
                    eDPCenterEntities.Total_Transaction.Add(total_Transaction);
                    eDPCenterEntities.SaveChanges();
                    MessageBox.Show("تم اضافة البند بنجاح", "عملية ناجحة", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    List<Total_Transaction> total_Transactions = eDPCenterEntities.Total_Transaction.ToList();
                    NewDataGrid(total_Transactions);
                }
            }
            catch
            {
                MessageBox.Show("حدث خطأ أثناء تشغيل البرنامج", "خطأ غير متوقع", MessageBoxButtons.OK, MessageBoxIcon.Error);
                eDPCenterEntities.Dispose();
                eDPCenterEntities = new EDPCenterEntities();
            }
        }
        private void NewDataGrid(List<Total_Transaction> total_Transactions)
        {
            nameBox.Text = "";
            numberBox.Text = "";
            textBoxItem.Text = "ادخل البند";

            dataGridViewTotalTransactions.Rows.Clear();

            foreach (Total_Transaction total_Transaction in total_Transactions)
            {
                DataGridViewRow row = (DataGridViewRow)dataGridViewTotalTransactions.Rows[0].Clone();
                row.Cells[0].Value = total_Transaction.ID;
                row.Cells[1].Value = total_Transaction.Name;
                row.Cells[2].Value = total_Transaction.Person_ID;
                row.Cells[3].Value = total_Transaction.Date.ToString();
                row.Cells[4].Value = total_Transaction.Transaction_Type;
                row.Cells[5].Value = total_Transaction.Price;
                dataGridViewTotalTransactions.Rows.Add(row);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;

            if (index >= 0)
            {
                DataGridViewRow row = (DataGridViewRow)dataGridViewTotalTransactions.Rows[index];
                if (row.Cells[0].Value != null)
                {
                    nameBox.Text = row.Cells[1].Value.ToString();
                    numberBox.Text = row.Cells[5].Value.ToString();
                    productBox.Text = row.Cells[0].Value.ToString();
                    DateTime tempDate = Convert.ToDateTime(row.Cells[3].Value.ToString());
                    dateTimePickerStart.Value = tempDate;
                }                
            }
        }  
        
        private void materialButton5_Click(object sender, EventArgs e)
        {
            if (textBoxItem.Visible )
            {
                if (textBoxItem.Text.Length == 0)
                    MessageBox.Show("ادخل اسم البند", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else if (textBoxItem.Text == "ادخل البند")
                    MessageBox.Show("ادخل اسم البند", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                {
                    List<Total_Transaction> total_Transactions;
                    total_Transactions = eDPCenterEntities.Total_Transaction.Where(a => a.Name.Contains(textBoxItem.Text)).ToList();
                    if (total_Transactions.Count > 0)
                        NewDataGrid(total_Transactions);
                    else
                        MessageBox.Show("لا توجد نتائج", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
        private void materialButton7_Click(object sender, EventArgs e)
        {
            if (dateTimePickerEnd.Value >= dateTimePickerStart.Value)
            {
                List<Total_Transaction> total_Transactions;
                DateTime StartDate = dateTimePickerStart.Value.Date;
                DateTime EndDate = dateTimePickerEnd.Value.Date;
                total_Transactions = eDPCenterEntities.Total_Transaction.ToList().Where(a => a.Date.Value.Date >= StartDate && a.Date.Value.Date <= EndDate).ToList();
                if (total_Transactions.Count > 0)
                    NewDataGrid(total_Transactions);
                else
                    MessageBox.Show("لا توجد نتائج", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void materialButton6_Click(object sender, EventArgs e)
        {
            NewDataGrid(eDPCenterEntities.Total_Transaction.ToList());
        }
        private void textBox2_Enter(object sender, EventArgs e)
        {
            textBoxItem.Text = "";
        }
        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (textBoxItem.Text.Length == 0)
                textBoxItem.Text = "ادخل البند";
        }

        private void materialButton8_Click(object sender, EventArgs e)
        {
            try
            {
                DGVPrinter printer = new DGVPrinter();
                index = 0; totalExpenses = 0; totalRevenue = 0;
                foreach (DataGridViewRow row in dataGridViewTotalTransactions.Rows)
                {
                    if (row.Cells[4].Value.ToString() == "مصروفات")
                        totalExpenses += (double)row.Cells[5].Value;
                    else
                        totalRevenue += (double)row.Cells[5].Value;
                    if (++index == dataGridViewTotalTransactions.Rows.Count - 1) { index = 0; break; }
                }

                printer.Title = "تقرير التعاملات المالية";
                printer.SubTitle = textBoxItem.Text == "ادخل البند" ? " حتي " + dateTimePickerEnd.Value.ToString() + " " + dateTimePickerStart.Value.ToString() + " بداية من" : textBoxItem.Text.ToString();
                printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
                printer.PageNumbers = true;
                printer.PageNumberInHeader = false;
                printer.PorportionalColumns = true;
                printer.ColumnWidth = DGVPrinter.ColumnWidthSetting.DataWidth;
                printer.TableAlignment = DGVPrinter.Alignment.Center;
                printer.HeaderCellAlignment = StringAlignment.Near;
                printer.Footer = $"اجمالي المصروفات: {totalExpenses} \r\n اجمالي الوارد: {totalRevenue} \r\n منصة تطوير التعليم - EDP Training Center Minia Branch";
                printer.FooterSpacing = 15;
                printer.printDocument.DefaultPageSettings.Landscape = true;
                printer.PrintNoDisplay(dataGridViewTotalTransactions);
            }
            catch
            {
                MessageBox.Show("حدث خطأ أثناء تشغيل البرنامج", "خطأ غير متوقع", MessageBoxButtons.OK, MessageBoxIcon.Error);
                eDPCenterEntities.Dispose();
                eDPCenterEntities = new EDPCenterEntities();
            }
        }

        private void textBoxItem_Enter(object sender, EventArgs e)
        {
            if (textBoxItem.Text == "ادخل البند")
                textBoxItem.Text = "";
        }

        private void materialButton2_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show("هل أنت متأكد من حذف جميع السجلات المالية؟", "تنبيه!", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.OK)
                {
                    DialogResult dialogResult1 = MessageBox.Show("البيانات المحذوفة لا يمكن استعادتها مرة اخرى؟", "تنبيه!", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                    if (dialogResult1 == DialogResult.OK)
                    {
                        foreach (Total_Transaction item in eDPCenterEntities.Total_Transaction)
                        {
                            eDPCenterEntities.Total_Transaction.Remove(item);
                        }
                        eDPCenterEntities.SaveChanges();
                        dataGridViewTotalTransactions.Rows.Clear();
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

        private void totalIncomes_FormClosed(object sender, FormClosedEventArgs e)
        {
            eDPCenterEntities.Dispose();
        }

        private void textBoxItem_Leave(object sender, EventArgs e)
        {
            if (textBoxItem.Text == "")
                textBoxItem.Text = "ادخل البند";
        }


    }
}