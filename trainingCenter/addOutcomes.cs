using DGVPrinterHelper;
using MetroSet_UI.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using trainingCenter.BL;

namespace trainingCenter
{
    public partial class addoutcomes : MetroSetForm
    {
        bool isValidName;
        bool isValidNumber;
        string transactionType = "مصروفات";
        double totalRevenue = 0d, totalExpenses = 0d;
        int index = 0;


        EDPCenterEntities eDPCenterEntities;
        List<Daily_Transaction> AllData;
        public addoutcomes(User user)
        {
            InitializeComponent();
            eDPCenterEntities = new EDPCenterEntities();
            AllData = eDPCenterEntities.Daily_Transaction.ToList();
            // set date to current date
            dateTimePicker1.Text = DateTime.Now.ToString();
            if (user.Role == "user")
            {
                materialButton4.Hide();
                materialButton2.Hide();
            }
        }

        // validate fields
        private bool checkValidation()
        {
            isValidName = Utilities.validateNameInArabic(nameBox.Text);
            isValidNumber = Utilities.checkDoubleNumber(numberBox.Text);

            if (!isValidName)
            {
                label3.Visible = true;
                return false;
            }
            if (!isValidNumber)
            {
                label12.Visible = true;
                return false;
            }
            else
            {
                label3.Visible = false;
                label12.Visible = false;
                return true;
            }
        }

        private void nameBox_Leave(object sender, EventArgs e)
        {
            if (!isValidName)
            {
                label3.Visible = false;
            }
        }

        private void numberBox_Leave(object sender, EventArgs e)
        {
            if (!isValidNumber)
            {
                label12.Visible = false;
            }
        }


        private void addoutcomes_Load(object sender, EventArgs e)
        {
            MinimumSize = MaximumSize = Size;
            if (textBox2.Text.Length == 0)
                textBox2.Text = "ادخل التاريخ (الشهر-اليوم-السنة) او البند";

            List<Daily_Transaction> daily_Transactions = eDPCenterEntities.Daily_Transaction.ToList();
            NewDataGrid(daily_Transactions);
            if (textBox2.Text.Length == 0)
                textBox2.Text = "ادخل التاريخ (الشهر-اليوم-السنة) او البند";

            foreach (DataGridViewColumn c in dataGridViewTransactions.Columns)
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
                    Daily_Transaction daily_Transaction = new Daily_Transaction()
                    {
                        Name = nameBox.Text,
                        Price = price,
                        Date = DateTime.Now,
                        Transaction_Type = transactionType

                    };
                    eDPCenterEntities.Daily_Transaction.Add(daily_Transaction);
                    eDPCenterEntities.SaveChanges();
                    MessageBox.Show("تم اضافة البند بنجاح", "عملية ناجحة", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    List<Daily_Transaction> daily_Transactions = eDPCenterEntities.Daily_Transaction.ToList();
                    NewDataGrid(daily_Transactions);

                }
            }
            catch
            {
                MessageBox.Show("حدث خطأ أثناء تشغيل البرنامج", "خطأ غير متوقع", MessageBoxButtons.OK, MessageBoxIcon.Error);
                eDPCenterEntities.Dispose();
                eDPCenterEntities = new EDPCenterEntities();
            }
        }

        private void materialButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (checkValidation())
                {
                    if (productBox.Text.Length > 0)
                    {
                        DateTime date = DateTime.Now;
                        double price = double.Parse(numberBox.Text);
                        double productNo = double.Parse(productBox.Text);
                        Daily_Transaction daily_Transaction = eDPCenterEntities.Daily_Transaction.Where(x => x.ID == productNo).FirstOrDefault();

                        if (daily_Transaction != null)
                        {
                            daily_Transaction.Name = nameBox.Text;
                            daily_Transaction.Price = price;
                            daily_Transaction.Date = date;
                            eDPCenterEntities.SaveChanges();
                            NewDataGrid(eDPCenterEntities.Daily_Transaction.ToList());
                            MessageBox.Show("تم تعديل البند بنجاح", "عملية ناجحة", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("فشل التعديل", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("الرجاء اختيار بند من الاسفل فى الجدول", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void materialButton4_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("التأكيد على تصفية اليوم ؟", "! تحذير", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                foreach (Daily_Transaction daily_Transaction in eDPCenterEntities.Daily_Transaction)
                {

                    eDPCenterEntities.Total_Transaction.Add(new Total_Transaction
                    {
                        Date = daily_Transaction.Date,
                        Price = daily_Transaction.Price,
                        Person_ID = daily_Transaction.Person_ID,
                        Name = daily_Transaction.Name,
                        Transaction_Type = daily_Transaction.Transaction_Type,
                    });

                    eDPCenterEntities.Daily_Transaction.Remove(daily_Transaction);
                }
                eDPCenterEntities.SaveChanges();
                dataGridViewTransactions.Rows.Clear();
                dataGridViewTransactions.Refresh();
            }
        }


        private void NewDataGrid(List<Daily_Transaction> daily_Transactions)
        {
            nameBox.Text = "";
            numberBox.Text = "";
            textBox2.Text = "ادخل البند";

            dataGridViewTransactions.Rows.Clear();

            foreach (Daily_Transaction daily_Transaction in daily_Transactions)
            {
                DataGridViewRow row = (DataGridViewRow)dataGridViewTransactions.Rows[0].Clone();
                row.Cells[0].Value = daily_Transaction.ID;
                row.Cells[1].Value = daily_Transaction.Name;
                row.Cells[2].Value = daily_Transaction.Person_ID;
                row.Cells[3].Value = daily_Transaction.Date.ToString();
                row.Cells[4].Value = daily_Transaction.Transaction_Type;
                row.Cells[5].Value = daily_Transaction.Price;
                dataGridViewTransactions.Rows.Add(row);
            }
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            textBox2.Text = "";
        }
        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (textBox2.Text.Length == 0)
                textBox2.Text = "ادخل البند";
        }

        private void materialButton5_Click(object sender, EventArgs e)
        {
            if (textBox2.Visible == true)
            {
                if (textBox2.Text.Length == 0)
                    MessageBox.Show("ادخل اسم البند", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (textBox2.Text == "ادخل البند")
                    MessageBox.Show("ادخل اسم البند", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    List<Daily_Transaction> daily_Transactions;
                    daily_Transactions = eDPCenterEntities.Daily_Transaction.Where(a => a.Name.Contains(textBox2.Text)).ToList();
                    if (daily_Transactions.Count > 0)
                        NewDataGrid(daily_Transactions);
                    else
                        MessageBox.Show("لا توجد نتائج", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                NewDataGrid(eDPCenterEntities.Daily_Transaction.ToList());
            }
            textBox2.Visible = true;
            
        }

        private void materialButton6_Click(object sender, EventArgs e)
        {
            NewDataGrid(eDPCenterEntities.Daily_Transaction.ToList());
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            if (index >= 0)
            {
                DataGridViewRow row = (DataGridViewRow)dataGridViewTransactions.Rows[index];
                if (row.Cells[0].Value != null)
                {
                    nameBox.Text = row.Cells[1].Value.ToString();
                    DateTime tempDate = Convert.ToDateTime(row.Cells[3].Value.ToString());
                    numberBox.Text = row.Cells[5].Value.ToString();
                    productBox.Text = row.Cells[0].Value.ToString();
                }
            }
        }

        private void addoutcomes_FormClosed(object sender, FormClosedEventArgs e)
        {
            eDPCenterEntities.Dispose();
        }

        private void materialButton8_Click(object sender, EventArgs e)
        {
            try
            {
                index = 0; totalExpenses = 0; totalRevenue = 0;
                DGVPrinter printer = new DGVPrinter();
                if (dataGridViewTransactions.Rows.Count > 1)
                {
                    foreach (DataGridViewRow row in dataGridViewTransactions.Rows)
                    {
                        if (row.Cells[4].Value.ToString() == "مصروفات")
                            totalExpenses += (double)row.Cells[5].Value;
                        else
                            totalRevenue += (double)row.Cells[5].Value;
                        if (++index == dataGridViewTransactions.Rows.Count - 1) { index = 0; break; }
                    }

                    printer.Title = "تقرير المصروفات والايراد اليومي";
                    printer.SubTitle = textBox2.Text == "ادخل البند" ? "تاريخ انشاء التقرير" + dateTimePicker2.Value.ToString() : textBox2.Text.ToString();
                    printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
                    printer.PageNumbers = true;
                    printer.PageNumberInHeader = false;
                    printer.PorportionalColumns = true;
                    printer.ColumnWidth = DGVPrinter.ColumnWidthSetting.DataWidth;
                    printer.TableAlignment = DGVPrinter.Alignment.Center;
                    printer.HeaderCellAlignment = StringAlignment.Near;
                    printer.Footer = $"اجمالي المصروفات: {totalExpenses} \r\n اجمالي الوارد: {totalRevenue} \r\n منصة تطوير التعليم - EDP Training Center Minia Branch";
                    //منصة تطوير التعليم - EDP Training Center Minia Branch
                    printer.FooterSpacing = 15;
                    printer.printDocument.DefaultPageSettings.Landscape = true;
                    printer.PrintNoDisplay(dataGridViewTransactions);
                }
                else
                {
                    MessageBox.Show("لا توجد نتائج", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
}