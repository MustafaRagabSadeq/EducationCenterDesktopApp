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

namespace trainingCenter
{
    public partial class Login : MetroSet_UI.Forms.MetroSetForm
    {
        EDPCenterEntities EDPDBContext = new EDPCenterEntities();
        IQueryable<User> Users;
        public Login()
        {
            InitializeComponent();
            Users = EDPDBContext.Users;
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            LoginFunc();
        }

        private void TextBoxUsername_Enter(object sender, EventArgs e)
        {
            if (TextBoxUsername.Text == "اسم المستخدم")
                TextBoxUsername.Text = "";
        }

        private void TextBoxUsername_Leave(object sender, EventArgs e)
        {
            if (TextBoxUsername.Text == "")
                TextBoxUsername.Text = "اسم المستخدم";
        }

        private void TextBoxPassphrase_Enter(object sender, EventArgs e)
        {
            if (TextBoxPassphrase.Text == "كلمة المرور")
            {
                TextBoxPassphrase.Text = "";
                TextBoxPassphrase.PasswordChar = '●';
            }
        }
        private void TextBoxPassphrase_Leave(object sender, EventArgs e)
        {
            if (TextBoxPassphrase.Text == "")
            {
                TextBoxPassphrase.PasswordChar = '\0';
                TextBoxPassphrase.Text = "كلمة المرور";
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {
            MaximumSize = MinimumSize = Size;
            TextBoxPassphrase.Text = "كلمة المرور";
            TextBoxUsername.Text = "";
        }

        private void TextBoxPassphrase_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.Equals((char)13))
            {
                LoginFunc();
            }
        }
        private void LoginFunc()
        {
            try
            {
                foreach (var userCredenetials in Users)
                {
                    if (TextBoxUsername.Text == userCredenetials.Username)
                        if (TextBoxPassphrase.Text == userCredenetials.Password)
                        {
                            new Dashboard(userCredenetials).Show();
                            Hide();
                            return;
                        }
                }
                MessageBox.Show("فشل في عملية تسجيل الدخول", "خطأ في التسجيل", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
            }
            catch
            {
                MessageBox.Show("لا يمكن الوصول لقاعدة البيانات", "خطأ غير متوقع", MessageBoxButtons.OK, MessageBoxIcon.Error);
                EDPDBContext.Dispose();
                EDPDBContext = new EDPCenterEntities();
            }
        }

        private void TextBoxUsername_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.Equals((char)13))
            {
                LoginFunc();
            }
        }

        private void Login_FormClosed(object sender, FormClosedEventArgs e)
        {
            EDPDBContext.Dispose();           
        }
    }
}
