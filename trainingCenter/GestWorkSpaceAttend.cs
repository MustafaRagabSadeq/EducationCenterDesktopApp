using MetroSet_UI.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Metadata.Edm;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using trainingCenter.BL;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace trainingCenter
{
    public partial class GestWorkSpaceAttend : MetroSetForm
    {
        EDPCenterEntities context = new EDPCenterEntities();
        public GestWorkSpaceAttend()
        {
            InitializeComponent();
        }
        private void addGroup_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;
            MinimumSize = MaximumSize = Size;
            NewDataGrid(listOfAttendGuest());
            foreach (DataGridViewColumn c in dataGridView1.Columns)
            {
                c.DefaultCellStyle.Font = new Font("Arial", 17, FontStyle.Bold, GraphicsUnit.Pixel);
                c.DefaultCellStyle.ForeColor = Color.Black;
            }
        }
        //حساب التكلفه
        public void CostValue(Guest_WorkSpace_Attend guest)
        {
            DateTime dt = DateTime.Now;
            double cost1 = context.Costs.Find(2).Payment;
            double cost2 = context.Costs.Find(3).Payment;

            double intervail = (DateTime.Parse(textBox8.Text) - DateTime.Parse(textBox5.Text)).TotalHours;
            intervail=Math.Ceiling(intervail);

            double price = 0;

            if (intervail <= 1)
            {
                price = cost1;
            }
            else
            {
                intervail -= 1;
                price = cost1 + (intervail * cost2);
            }
            textBox4.Text=price.ToString();
            textBox7.Text=price.ToString();
            guest.Cost = price;
            context.SaveChanges();
            NewDataGrid(listOfAttendGuest());
        }
        public Guest_WorkSpace_Attend PastGuest()
        {
            Guest_WorkSpace_Attend guest;
            if (textBox1.Text.Length > 0)
            {
                int ID = int.Parse(textBox1.Text);
                DateTime dt = DateTime.Now;
                guest = context.Guest_WorkSpace_Attend
                    .OrderByDescending(x=>x.Guest_WorkSpace_ID).FirstOrDefault(x=>x.Guest_ID==ID && x.Payment==null);
                return guest;
            }
            else
            {
                return guest = new Guest_WorkSpace_Attend();
            }

        }
        /// list من حضور الضيوف
        public List<Guest_WorkSpace_Attend> listOfAttendGuest()
        {
            DateTime dt = DateTime.Now;
            List<Guest_WorkSpace_Attend> guests = context.Guest_WorkSpace_Attend.Where(g => g.Day.Day == dt.Day || g.End_Time == null || g.Payment == null).ToList();
            return guests;

        }
        private void NewDataGrid(List<Guest_WorkSpace_Attend> guestAttend)
        {
            dataGridView1.Rows.Clear();

            foreach (Guest_WorkSpace_Attend group in guestAttend)
            {
                int x = group.Guest_ID;
                var name = (from g in context.Guest_workspace
                           where g.ID== x
                           select g.Name).FirstOrDefault();
                DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[0].Clone();
                row.Cells[0].Value = group.Guest_ID;
                row.Cells[1].Value = name;
                row.Cells[2].Value = group.Start_Time;
                row.Cells[3].Value = group.End_Time;
                row.Cells[4].Value = group.Cost;
                row.Cells[5].Value = group.Discount;
                row.Cells[6].Value = group.Payment;

                dataGridView1.Rows.Add(row);
                if (row.Cells[3].Value == null && row.Cells[4].Value == null)
                {
                    row.DefaultCellStyle.BackColor = Color.SkyBlue;
                    row.DefaultCellStyle.ForeColor = Color.Black;
                }
                else if (row.Cells[6].Value == null)
                {
                    row.DefaultCellStyle.BackColor = Color.Red;
                    row.DefaultCellStyle.ForeColor = Color.White;
                }
                else
                {
                    row.DefaultCellStyle.BackColor = Color.YellowGreen;
                    row.DefaultCellStyle.ForeColor = Color.Black;
                }
            }
        }
        private void btnAttend_Click(object sender, EventArgs e)
        {
            NewDataGrid(listOfAttendGuest());
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox3.Text = textBox4.Text = textBox7.Text = "";
            textBox6.Text = "0";
            textBox5.Text = DateTime.Now.ToString();
            label15.Visible = false;
            if (textBox2.Text.Length == 0)
            {
                label15.Visible = true;
            }
            else
            {
                int guestID;
                bool isNumber = int.TryParse(textBox2.Text, out guestID);
                Guest_workspace guest;

                if (isNumber)
                {
                    guest = (from g in context.Guest_workspace where g.ID == guestID select g).FirstOrDefault();
                }
                else
                {
                    guest = (from gg in context.Guest_workspace where gg.Name.Contains(textBox2.Text) select gg).FirstOrDefault();
                }
                if (guest != null)
                {
                    textBox1.Text = guest.ID.ToString();
                    textBox3.Text = guest.Name; 
                }
                else
                {
                    MessageBox.Show("لا يوجد عميل بهذا الاسم أو الكود", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
        
        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text.Length > 0 && textBox3.Text.Length > 0)
                {
                    int id = Convert.ToInt32(textBox1.Text);
                    var guest = PastGuest();
                    if (guest != null && guest.Payment == null)
                    {
                        MessageBox.Show("هذا العميل موجود بالفعل", "خطأ في الإضافة", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        Guest_WorkSpace_Attend attentGest = new Guest_WorkSpace_Attend()
                        {

                            Guest_ID = id,
                            Start_Time = DateTime.Now,
                            Day = DateTime.Now,
                            Discount = 0

                        };
                        context.Guest_WorkSpace_Attend.Add(attentGest);
                        context.SaveChanges();
                        MessageBox.Show("تم بدء الجلسة", "عملية ناجحة", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        NewDataGrid(listOfAttendGuest());
                        textBox1.Text = textBox3.Text = "";
                    }
                }

                else
                {
                    MessageBox.Show("ابحث عن العميل أولاً", "خطأ في الإضافة", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                textBox6.Text = "0";
                textBox4.Text = textBox7.Text = "";
            }
            catch
            {
                MessageBox.Show("حدث خطأ أثناء تشغيل البرنامج", "خطأ غير متوقع", MessageBoxButtons.OK, MessageBoxIcon.Error);
                context.Dispose();
                context = new EDPCenterEntities();
            }
        }
        private void btnEnd_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox4.Text.Length > 0)
                {
                    MessageBox.Show("لقد غادر العميل مسبقاً", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    Guest_WorkSpace_Attend guest_A = PastGuest();

                    if (textBox1.Text.Length > 0 && textBox3.Text.Length > 0)
                    {
                        if (guest_A != null)
                        {
                            guest_A.End_Time = DateTime.Now;
                            context.SaveChanges();
                            NewDataGrid(listOfAttendGuest());
                            CostValue(guest_A);
                            MessageBox.Show("تم تسجيل مغادرة العميل", "عملية ناجحة", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("إبدأ الجلسة أولاً", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show("اختر العميل أولاً لإنهاء الجلسة", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch
            {
                MessageBox.Show("حدث خطأ أثناء تشغيل البرنامج", "خطأ غير متوقع", MessageBoxButtons.OK, MessageBoxIcon.Error);
                context.Dispose();
                context = new EDPCenterEntities();
            }
        }
        private void btnPay_Click(object sender, EventArgs e)
        {
            try
            {
                Guest_WorkSpace_Attend guest_A = PastGuest();

                if (textBox1.Text.Length > 0 && textBox3.Text.Length > 0 && textBox4.Text.Length > 0)
                {
                    if (guest_A != null)
                    {
                        if (guest_A.Payment != null)
                        {
                            MessageBox.Show("تم الدفع مسبقاً", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            textBox1.Text = textBox3.Text = textBox4.Text = textBox7.Text = "";
                            textBox6.Text = 0.ToString();
                            textBox5.Text = textBox8.Text = DateTime.Now.ToString();
                        }
                        else
                        {
                            Guest_WorkSpace_Attend guest = PastGuest();
                            guest.Cost = Convert.ToDouble(textBox4.Text);
                            guest.Discount = Convert.ToDouble(textBox6.Text);

                            if (guest.Discount>=0 && guest.Discount<= guest.Cost)
                            {
                                guest.Payment = guest.Cost - guest.Discount;
                                int x = guest.Guest_ID;
                                var name = (from g in context.Guest_workspace
                                            where g.ID == x
                                            select g.Name).FirstOrDefault();
                                Daily_Transaction daily = new Daily_Transaction()
                                {
                                    Person_ID = guest.Guest_ID,
                                    Name = $" حساب العميل {name} في قاعة اللإستذكار ",
                                    Price = guest.Payment,
                                    Transaction_Type = "إيرادات",
                                    Date = DateTime.Now
                                };
                                context.Daily_Transaction.Add(daily);
                                context.SaveChanges();
                                NewDataGrid(listOfAttendGuest());
                                MessageBox.Show("تم تسجيل عملية الدفع", "عملية ناجحة", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                textBox1.Text = textBox3.Text = textBox4.Text = textBox7.Text = "";
                                textBox6.Text = 0.ToString();
                                textBox5.Text = textBox8.Text = DateTime.Now.ToString();
                            }
                            else
                            {
                                MessageBox.Show("ادخل قيمة صحيحة للخصم", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            
                        }

                    }
                    else
                    {
                        MessageBox.Show("تم الدفع مسبقاً", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    }

                }
                else { MessageBox.Show("اختر عن العميل أولاً ثم اضغط على إنهاء الجلسة", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            }
            catch
            {
                MessageBox.Show("حدث خطأ أثناء تشغيل البرنامج", "خطأ غير متوقع", MessageBoxButtons.OK, MessageBoxIcon.Error);
                context.Dispose();
                context = new EDPCenterEntities();
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
                    textBox1.Text = row.Cells[0].Value.ToString();
                    textBox3.Text = row.Cells[1].Value.ToString();
                    textBox5.Text = row.Cells[2].Value.ToString();
                    if (row.Cells[3].Value != null)
                    {
                        textBox8.Text = row.Cells[3].Value.ToString();
                        if (row.Cells[4].Value != null)
                        {
                            textBox4.Text = row.Cells[4].Value.ToString();
                            textBox6.Text = row.Cells[5].Value.ToString();
                            if (row.Cells[6].Value != null)
                                textBox7.Text = row.Cells[6].Value.ToString();
                            else
                               textBox7.Text = (Convert.ToInt32(textBox4.Text)- Convert.ToInt32(textBox6.Text)).ToString();
                        }
                        else
                        {textBox4.Text = textBox7.Text = "";}
                    }
                    else
                    {
                        textBox8.Text = DateTime.Now.ToString();
                        textBox4.Text = textBox7.Text = "";
                    }
                }
            }
            else
            {                
                textBox1.Text = textBox3.Text = textBox4.Text = textBox7.Text = "";
                textBox5.Text = textBox8.Text = DateTime.Now.ToString();
            }

        }        
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox1.Text = textBox3.Text = textBox4.Text = textBox7.Text = "";
            textBox6.Text = 0.ToString();
            textBox5.Text = DateTime.Now.ToString();
        }
        private void btnDis_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text.Length > 0)
                {
                    if (textBox4.Text.Length > 0)
                    {
                        Guest_WorkSpace_Attend guest_A = PastGuest();
                        if (guest_A.Payment != null)
                        {
                            MessageBox.Show("تم الدفع مسبقاً", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            if (textBox6.Text.Length > 0)
                            {
                                double discount;
                                if (double.TryParse(textBox6.Text, out discount))
                                {
                                    double? cost = Convert.ToDouble(textBox4.Text);
                                    if (discount>=0 && discount<= cost)
                                    {                                       
                                        double? payment = cost - discount;
                                        textBox7.Text = payment.ToString();
                                    }
                                    else
                                    {
                                        MessageBox.Show("ادخل قيمة صحيحة للخصم", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    }
                                    
                                }
                                else
                                {
                                    MessageBox.Show("ادخل قيمة رقمية للخصم", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                            }
                            else
                            {
                                MessageBox.Show("ادخل قيمة رقمية للخصم", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("اضفط على إنهاء الجلسة أولاً", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else MessageBox.Show("ادخل العميل أولاً", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch
            {
                MessageBox.Show("حدث خطأ أثناء تشغيل البرنامج", "خطأ غير متوقع", MessageBoxButtons.OK, MessageBoxIcon.Error);
                context.Dispose();
                context = new EDPCenterEntities();
            }
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            addGestWorkSpace addGest = new addGestWorkSpace();
            addGest.ShowDialog();
        }

        private void GestWorkSpaceAttend_FormClosed(object sender, FormClosedEventArgs e)
        {
            context.Dispose();
        }        
    }
}