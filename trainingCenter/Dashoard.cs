using MetroSet_UI.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using trainingCenter.BL;

namespace trainingCenter
{
    public partial class Dashboard : MetroSetForm
    {
        

        EDPCenterEntities eDPCenterEntities;
        User _user;
        public Dashboard(User user)
        {
            InitializeComponent();
            eDPCenterEntities = new EDPCenterEntities();
            if (user.Role == "user")
            {
                btnAddUser.Enabled = false;
                button10.Enabled = false;
                button11.Enabled = false;
                button3.Enabled = false;
            }
            label1.Text = user.Full_Name;
            _user=user;
        }

        private void btnAddTeacher_Click(object sender, EventArgs e)
        {
            addTeacher frm = Application.OpenForms.OfType<addTeacher>().FirstOrDefault();  // get the instance of Form2 if it's already open
            if (frm == null)  // if Form2 is not open, create a new instance and show it
            {
                frm = new addTeacher(_user);
                frm.Show();
            }
            else  // if Form2 is already open, maximize it
            {
                frm.WindowState = FormWindowState.Normal;
                frm.Focus();
            }
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            addUser frm = Application.OpenForms.OfType<addUser>().FirstOrDefault();  // get the instance of Form2 if it's already open
            if (frm == null)  // if Form2 is not open, create a new instance and show it
            {
                frm = new addUser();
                frm.Show();
            }
            else  // if Form2 is already open, maximize it
            {
                frm.WindowState = FormWindowState.Normal;
                frm.Focus();
            }
        }

        private void Dashboard_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void button1_MouseHover(object sender, EventArgs e)
        {
            button1.BackColor = Color.FromArgb(148, 40, 173);
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            button1.BackColor = Color.FromArgb(211,61, 245);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            addStudent frm = Application.OpenForms.OfType<addStudent>().FirstOrDefault();  // get the instance of Form2 if it's already open
            if (frm == null)  // if Form2 is not open, create a new instance and show it
            {
                frm = new addStudent();
                frm.Show();
            }
            else  // if Form2 is already open, maximize it
            {
                frm.WindowState = FormWindowState.Normal;
                frm.Focus();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int Ntechers = eDPCenterEntities.Teachers.Count();
            int NSubject = eDPCenterEntities.Subjects.Count();
            int NAcdamic = eDPCenterEntities.AcademicYears.Count();

            if (Ntechers == 0 || NSubject == 0 || NAcdamic == 0)
            {
                DialogResult result = MessageBox.Show("يجب تسجيل بيانات مدرس و سنة دراسية ومادة دراسية اولاً",
                        "لا توجد بيانات "
                            , MessageBoxButtons.OK, MessageBoxIcon.Question);
            }
            else
            {
                addGroup frm = Application.OpenForms.OfType<addGroup>().FirstOrDefault();  // get the instance of Form2 if it's already open
                if (frm == null)  // if Form2 is not open, create a new instance and show it
                {
                    frm = new addGroup();
                    frm.Show();
                }
                else  // if Form2 is already open, maximize it
                {
                    frm.WindowState = FormWindowState.Normal;
                    frm.Focus();
                }
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            addSubject frm = Application.OpenForms.OfType<addSubject>().FirstOrDefault();  // get the instance of Form2 if it's already open
            if (frm == null)  // if Form2 is not open, create a new instance and show it
            {
                frm = new addSubject();
                frm.Show();
            }
            else  // if Form2 is already open, maximize it
            {
                frm.WindowState = FormWindowState.Normal;
                frm.Focus();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            addAcademicYear frm = Application.OpenForms.OfType<addAcademicYear>().FirstOrDefault();  // get the instance of Form2 if it's already open
            if (frm == null)  // if Form2 is not open, create a new instance and show it
            {
                frm = new addAcademicYear();
                frm.Show();
            }
            else  // if Form2 is already open, maximize it
            {
                frm.WindowState = FormWindowState.Normal;
                frm.Focus();
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            StudentAttendance frm = Application.OpenForms.OfType<StudentAttendance>().FirstOrDefault();  // get the instance of Form2 if it's already open
            if (frm == null)  // if Form2 is not open, create a new instance and show it
            {
                frm = new StudentAttendance(_user);
                frm.Show();
            }
            else  // if Form2 is already open, maximize it
            {
                frm.WindowState = FormWindowState.Normal;
                frm.Focus();
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            GestWorkSpaceAttend frm = Application.OpenForms.OfType<GestWorkSpaceAttend>().FirstOrDefault();  // get the instance of Form2 if it's already open
            if (frm == null)  // if Form2 is not open, create a new instance and show it
            {
                frm = new GestWorkSpaceAttend();
                frm.Show();
            }
            else  // if Form2 is already open, maximize it
            {
                frm.WindowState = FormWindowState.Normal;
                frm.Focus();
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            AddCostes frm = Application.OpenForms.OfType<AddCostes>().FirstOrDefault();  // get the instance of Form2 if it's already open
            if (frm == null)  // if Form2 is not open, create a new instance and show it
            {
                frm = new AddCostes();
                frm.Show();
            }
            else  // if Form2 is already open, maximize it
            {
                frm.WindowState = FormWindowState.Normal;
                frm.Focus();
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            totalIncomes frm = Application.OpenForms.OfType<totalIncomes>().FirstOrDefault();  // get the instance of Form2 if it's already open
            if (frm == null)  // if Form2 is not open, create a new instance and show it
            {
                frm = new totalIncomes();
                frm.Show();
            }
            else  // if Form2 is already open, maximize it
            {
                frm.WindowState = FormWindowState.Normal;
                frm.Focus();
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            ScheduleForm frm = Application.OpenForms.OfType<ScheduleForm>().FirstOrDefault();  // get the instance of Form2 if it's already open
            if (frm == null)  // if Form2 is not open, create a new instance and show it
            {
                frm = new ScheduleForm();
                frm.Show();
            }
            else  // if Form2 is already open, maximize it
            {
                frm.WindowState = FormWindowState.Normal;
                frm.Focus();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            addoutcomes frm = Application.OpenForms.OfType<addoutcomes>().FirstOrDefault();  // get the instance of Form2 if it's already open
            if (frm == null)  // if Form2 is not open, create a new instance and show it
            {
                frm = new addoutcomes(_user);
                frm.Show();
            }
            else  // if Form2 is already open, maximize it
            {
                frm.WindowState = FormWindowState.Normal;
                frm.Focus();
            }
        }

        private void button4_MouseHover(object sender, EventArgs e)
        {
            button4.BackColor = Color.FromArgb(148, 40, 173);
        }

        private void button4_MouseLeave(object sender, EventArgs e)
        {
            button4.BackColor = Color.FromArgb(211, 61, 245);
        }

        private void button3_MouseHover(object sender, EventArgs e)
        {
            btnAddTeacher.BackColor = Color.FromArgb(13, 96, 140);
        }

        private void button3_MouseLeave(object sender, EventArgs e)
        {
            btnAddTeacher.BackColor = Color.FromArgb(28, 163, 235);
        }

        private void button2_MouseHover(object sender, EventArgs e)
        {
            button2.BackColor = Color.FromArgb(13, 96, 140);
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            button2.BackColor = Color.FromArgb(28, 163, 235);
        }

        private void button7_MouseHover(object sender, EventArgs e)
        {
            button7.BackColor = Color.FromArgb(13, 96, 140);
        }

        private void button7_MouseLeave(object sender, EventArgs e)
        {
            button7.BackColor = Color.FromArgb(28, 163, 235);
        }

        private void button5_MouseHover(object sender, EventArgs e)
        {
            btnAddUser.BackColor = Color.FromArgb(3, 179, 63);
        }

        private void button5_MouseLeave(object sender, EventArgs e)
        {
            btnAddUser.BackColor = Color.FromArgb(3, 244, 87);
        }

        private void button13_MouseLeave(object sender, EventArgs e)
        {
            button13.BackColor = Color.FromArgb(209, 124, 10);
        }

        private void button13_MouseHover(object sender, EventArgs e)
        {
            button13.BackColor = Color.FromArgb(147, 88, 8);
        }

        private void button15_MouseHover(object sender, EventArgs e)
        {
            button15.BackColor = Color.FromArgb(3, 179, 64);
        }

        private void button15_MouseLeave(object sender, EventArgs e)
        {
            button15.BackColor = Color.FromArgb(3, 244, 87);
        }

        private void button12_MouseHover(object sender, EventArgs e)
        {
            button12.BackColor = Color.FromArgb(4, 72, 78);
        }

        private void button12_MouseLeave(object sender, EventArgs e)
        {
            button12.BackColor = Color.FromArgb(10, 130, 140);
        }

        private void button11_MouseHover(object sender, EventArgs e)
        {
            button11.BackColor = Color.FromArgb(182, 15, 97);
        }

        private void button11_MouseLeave(object sender, EventArgs e)
        {
            button11.BackColor = Color.FromArgb(245, 13, 127);
        }

        private void button10_MouseHover(object sender, EventArgs e)
        {
            button10.BackColor = Color.FromArgb(182, 15, 97);
        }

        private void button10_MouseLeave(object sender, EventArgs e)
        {
            button10.BackColor = Color.FromArgb(245, 13, 127);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            addStudentGroup frm = Application.OpenForms.OfType<addStudentGroup>().FirstOrDefault();  // get the instance of Form2 if it's already open
            if (frm == null)  // if Form2 is not open, create a new instance and show it
            {
                frm = new addStudentGroup();
                frm.Show();
            }
            else  // if Form2 is already open, maximize it
            {
                frm.WindowState = FormWindowState.Normal;
                frm.Focus();
            }

        }

        private void button14_Click(object sender, EventArgs e)
        {
            addGestWorkSpace frm = Application.OpenForms.OfType<addGestWorkSpace>().FirstOrDefault();  // get the instance of Form2 if it's already open
            if (frm == null)  // if Form2 is not open, create a new instance and show it
            {
                frm = new addGestWorkSpace();
                frm.Show();
            }
            else  // if Form2 is already open, maximize it
            {
                frm.WindowState = FormWindowState.Normal;
                frm.Focus();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            AttendenceInstructers frm = Application.OpenForms.OfType<AttendenceInstructers>().FirstOrDefault();  // get the instance of Form2 if it's already open
            if (frm == null)  // if Form2 is not open, create a new instance and show it
            {
                frm = new AttendenceInstructers();
                frm.Show();
            }
            else  // if Form2 is already open, maximize it
            {
                frm.WindowState = FormWindowState.Normal;
                frm.Focus();
            }
        }

        private void button14_MouseHover(object sender, EventArgs e)
        {
            button14.BackColor = Color.FromArgb(3, 179, 63);
        }

        private void button14_MouseLeave(object sender, EventArgs e)
        {
            button14.BackColor = Color.FromArgb(3, 244, 87);
        }

        private void button9_MouseHover(object sender, EventArgs e)
        {
            button9.BackColor = Color.FromArgb(147, 88, 8);
        }

        private void button9_MouseLeave(object sender, EventArgs e)
        {
            button9.BackColor = Color.FromArgb(209, 124, 10);
        }

        private void button6_MouseHover(object sender, EventArgs e)
        {
            button6.BackColor = Color.FromArgb(4, 72, 78);
        }

        private void button6_MouseLeave(object sender, EventArgs e)
        {
            button6.BackColor = Color.FromArgb(10, 130, 140);
        }

        private void button8_MouseHover(object sender, EventArgs e)
        {
            button8.BackColor = Color.FromArgb(182, 15, 97);
        }

        private void button8_MouseLeave(object sender, EventArgs e)
        {
            button8.BackColor = Color.FromArgb(245, 13, 127);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            addRoom frm = Application.OpenForms.OfType<addRoom>().FirstOrDefault();  // get the instance of Form2 if it's already open
            if (frm == null)  // if Form2 is not open, create a new instance and show it
            {
                frm = new addRoom();
                frm.Show();
            }
            else  // if Form2 is already open, maximize it
            {
                frm.WindowState = FormWindowState.Normal;
                frm.Focus();
            }
        }

        private void button3_MouseHover_1(object sender, EventArgs e)
        {
            button3.BackColor = Color.FromArgb(148, 40, 173);
        }

        private void button3_MouseLeave_1(object sender, EventArgs e)
        {
            button3.BackColor = Color.FromArgb(211, 61 , 245);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Hide();
        }

        private void button5_MouseHover_1(object sender, EventArgs e)
        {
            button5.BackColor = Color.FromArgb(4, 72, 78);
        }

        private void button5_MouseLeave_1(object sender, EventArgs e)
        {
            button5.BackColor = Color.FromArgb(10, 130, 140);
        }

        private void button5_Click(object sender, EventArgs e)
        {

            addInstructor frm = Application.OpenForms.OfType<addInstructor>().FirstOrDefault();  // get the instance of Form2 if it's already open
            if (frm == null)  // if Form2 is not open, create a new instance and show it
            {
                frm = new addInstructor();
                frm.Show();
            }
            else  // if Form2 is already open, maximize it
            {
                frm.WindowState = FormWindowState.Normal;
                frm.Focus();
            }

        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            MinimumSize = MaximumSize = Size;
        }

        private void label2_MouseHover(object sender, EventArgs e)
        {
            label2.ForeColor = Color.FromArgb(25, 90, 200);
        }

        private void label2_MouseLeave(object sender, EventArgs e)
        {
            label2.ForeColor = Color.White;
        }

        private void label2_Click(object sender, EventArgs e)
        {
            About about = new About();
            about.Show();
        }

    }
}
