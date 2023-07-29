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
    public partial class addUser : MetroSet_UI.Forms.MetroSetForm
    {
        static EDPCenterEntities EDPDBContext;       
        bool isValidPhone, isValidSalary;
        public addUser()
        {
            InitializeComponent();           
            EDPDBContext = new EDPCenterEntities();
            dataGridViewUsers.Columns[0].DefaultCellStyle.ForeColor = Color.Black;
            dataGridViewUsers.Columns[1].DefaultCellStyle.ForeColor = Color.Black;
            dataGridViewUsers.Columns[2].DefaultCellStyle.ForeColor = Color.Black;
            dataGridViewUsers.Columns[3].DefaultCellStyle.ForeColor = Color.Black;
            dataGridViewUsers.Columns[4].DefaultCellStyle.ForeColor = Color.Black;
            dataGridViewUsers.Columns[5].DefaultCellStyle.ForeColor = Color.Black;
            dataGridViewUsers.Columns[6].DefaultCellStyle.ForeColor = Color.Black;
            metroComboBox_Role.SelectedIndex = metroComboBox_Role.Items.Count - 1;
            List<User> users = EDPDBContext.Users.ToList();
            NewDataGrid(users);
        }

        private void materialButtonAdd_Click(object sender, EventArgs e)
        {
            try
            {
                double sal = 0;
                if (textBox_Username.Text.Length > 0 && textBox_UserPhone.Text.Length > 0 && textBox_Passphrase.Text.Length > 0 && textBox_UserFullName.Text.Length > 0)
                {

                    if (IsValid())
                    {
                        User userFound = EDPDBContext.Users.Where(x => x.Username == textBox_Username.Text).FirstOrDefault();
                        if (userFound != null)
                            MessageBox.Show("هذا المستخدم موجود بالفعل", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        else
                        {
                            User user = new User
                            {
                                Username = textBox_Username.Text,
                                Password = textBox_Passphrase.Text,
                                Full_Name = textBox_UserFullName.Text,
                                Role = metroComboBox_Role.SelectedItem.ToString(),
                                Phone = textBox_UserPhone.Text,
                                Salary = double.TryParse(textBox_FixedSalary.Text, out sal) ? sal : sal,
                            };
                            EDPDBContext.Users.Add(user);
                            EDPDBContext.SaveChanges();
                            MessageBox.Show("تم إضافة المستخدم بنجاح", "عملية ناجحة", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        List<User> users = EDPDBContext.Users.ToList();
                        NewDataGrid(users);
                        textBox_Username.Text = "";
                        textBox_Passphrase.Text = "";
                        textBox_UserFullName.Text = "";
                        textBox_UserPhone.Text = "";
                        textBox_FixedSalary.Text = "";
                    }
                }
                else
                {
                    MessageBox.Show("من فضلك ادخل بيانات صحيحة", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch
            {
                MessageBox.Show("حدث خطأ أثناء تشغيل البرنامج", "خطأ غير متوقع", MessageBoxButtons.OK, MessageBoxIcon.Error);
                EDPDBContext.Dispose();
                EDPDBContext = new EDPCenterEntities();
            }
        }

        private void NewDataGrid(List<User> users)
        {
            dataGridViewUsers.Rows.Clear();
            foreach (User item in users)
            {
                DataGridViewRow row = (DataGridViewRow)dataGridViewUsers.Rows[0].Clone();
                row.Cells[0].Value = item.ID;
                row.Cells[1].Value = item.Full_Name;
                row.Cells[2].Value = item.Username;
                row.Cells[3].Value = item.Password;
                row.Cells[4].Value = item.Phone;
                row.Cells[5].Value = item.Role;
                row.Cells[6].Value = item.Salary;
                dataGridViewUsers.Rows.Add(row);
            }
        }

        private void materialButton3_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox_UserID.Text.Length > 0)
                {
                    int id = int.Parse(textBox_UserID.Text);
                    User user1 = EDPDBContext.Users.ToList().FirstOrDefault(x => x.ID == id);

                    List<User> admins = EDPDBContext.Users.Where(x => x.Role == "admin").ToList();
                    if (user1.Role == "admin" && admins.Count() == 1)
                        MessageBox.Show("لا يمكن حذف اخر ادمن", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                    {
                        DialogResult dialogResult = MessageBox.Show("التأكيد على حذف المستخدم؟", "! تحذير", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dialogResult == DialogResult.Yes)
                        {
                            id = int.Parse(textBox_UserID.Text);
                            User user = EDPDBContext.Users.Where(u => u.ID == id).FirstOrDefault();
                            EDPDBContext.Users.Remove(user);
                            EDPDBContext.SaveChanges();
                            List<User> users = EDPDBContext.Users.ToList();
                            NewDataGrid(users);
                            MessageBox.Show("تم حذف المستخدم بنجاح", "عملية ناجحة", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            textBox_UserID.Text = "";
                            textBox_Passphrase.Text = "";
                            textBox_Username.Text = "";
                            textBox_UserFullName.Text = "";
                            textBox_UserPhone.Text = "";
                            textBox_FixedSalary.Text = "";
                        }
                    }
                }
                else
                {
                    MessageBox.Show("اختر مستخدم أولاً", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch
            {
                MessageBox.Show("حدث خطأ أثناء تشغيل البرنامج", "خطأ غير متوقع", MessageBoxButtons.OK, MessageBoxIcon.Error);
                EDPDBContext.Dispose();
                EDPDBContext = new EDPCenterEntities();
            }

        }

        private void dataGridViewUsers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridViewUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            if (index < 0) return;
            DataGridViewRow row = dataGridViewUsers.Rows[index];
            if (row.Cells[0].Value != null)
            {
                textBox_UserID.Text = row.Cells[0].Value.ToString();
                textBox_UserFullName.Text = row.Cells[1].Value.ToString();
                textBox_Username.Text = row.Cells[2].Value.ToString();
                textBox_Passphrase.Text = row.Cells[3].Value.ToString();
                textBox_UserPhone.Text = row.Cells[4].Value.ToString();
                metroComboBox_Role.Text = row.Cells[5].Value.ToString();
                textBox_FixedSalary.Text = row.Cells[6].Value != null ? row.Cells[6].Value.ToString() : "";
            }
            else
            {
                textBox_Username.Text = "";
                textBox_Passphrase.Text = "";
                textBox_UserFullName.Text = "";
                textBox_UserPhone.Text = "";
                textBox_FixedSalary.Text = "";
            }
            
        }

        private void materialEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox_UserID.Text.Length == 0)
                    MessageBox.Show("اختر مستخدم أولاً", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    MessageBox.Show("سيتم تغيير جميع البيانات ما عدا الصلاحية", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    int id = int.Parse(textBox_UserID.Text);
                    User user = EDPDBContext.Users.Where(u => u.ID == id).FirstOrDefault();
                    user.Username = textBox_Username.Text;
                    user.Password = textBox_Passphrase.Text;
                    user.Full_Name = textBox_UserFullName.Text;
                    user.Phone = textBox_UserPhone.Text;
                    user.Salary = double.TryParse(textBox_FixedSalary.Text, out double x) ? double.Parse(textBox_FixedSalary.Text) : 0;
                    EDPDBContext.SaveChanges();
                    List<User> users = EDPDBContext.Users.ToList();
                    NewDataGrid(users);
                    MessageBox.Show("تم تعديل بيانات المستخدم بنجاح", "عملية ناجحة", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textBox_UserID.Text = "";
                    textBox_Passphrase.Text = "";
                    textBox_Username.Text = "";
                    textBox_UserFullName.Text = "";
                    textBox_UserPhone.Text = "";
                    textBox_FixedSalary.Text = "";
                }
            }
            catch
            {
                MessageBox.Show("حدث خطأ أثناء تشغيل البرنامج", "خطأ غير متوقع", MessageBoxButtons.OK, MessageBoxIcon.Error);
                EDPDBContext.Dispose();
                EDPDBContext = new EDPCenterEntities();
            }
        }

        private void textBox_UserPhone_Leave(object sender, EventArgs e)
        {
            isValidPhone = textBox_UserPhone.Text != "" ? BL.Utilities.ValidatPhoneNumber(textBox_UserPhone.Text) : true;
            if (!isValidPhone)
                lblPhoneValidation.Visible = true;
        }

        private void textBox_FixedSalary_Leave(object sender, EventArgs e)
        {
            isValidSalary = textBox_FixedSalary.Text != "" ? BL.Utilities.checkDoubleNumber(textBox_FixedSalary.Text) : true;
            if (!isValidSalary)
                lblSalaryValidation.Visible = true;
        }

        private void addUser_Load(object sender, EventArgs e)
        {
            MinimumSize = MaximumSize = Size;
            foreach (DataGridViewColumn c in dataGridViewUsers.Columns)
            {
                c.DefaultCellStyle.Font = new Font("Arial", 17, FontStyle.Bold, GraphicsUnit.Pixel);
                c.DefaultCellStyle.ForeColor = Color.Black;
            }
        }

        private void addUser_FormClosed(object sender, FormClosedEventArgs e)
        {
            EDPDBContext.Dispose();
        }

        public bool IsValid()
        {
            //checked validation
            isValidSalary = textBox_FixedSalary.Text != "" ? BL.Utilities.checkDoubleNumber(textBox_FixedSalary.Text) : true;
            isValidPhone = textBox_UserPhone.Text != "" ? BL.Utilities.ValidatPhoneNumber(textBox_UserPhone.Text) : true;
            if (!isValidPhone)
            {
                lblPhoneValidation.Visible = true;
                return false;
            }
            if (!isValidSalary)
            {
                lblSalaryValidation.Visible = true;
                return false;
            }
            else
            {
                lblPhoneValidation.Visible = false;
                lblSalaryValidation.Visible = false;
                return true;
            }
        }
    }
}