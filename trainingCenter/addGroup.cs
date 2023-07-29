﻿using MetroSet_UI.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Metadata.Edm;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using trainingCenter.BL;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace trainingCenter
{
    public partial class addGroup : MetroSetForm
    {
        EDPCenterEntities context = new EDPCenterEntities();
        public addGroup()
        {
            InitializeComponent();
        }
        private void addGroup_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;
            MinimumSize = MaximumSize = Size;

            var teachers = (from c in context.Teachers
                           select c).ToList();
            comboBox1.DataSource=teachers;
            comboBox1.ValueMember = "T_ID";
            comboBox1.DisplayMember = "T_Name";

            var A_Year = (from a in context.AcademicYears select a).ToArray();
            comboBox2.DataSource=A_Year;
            comboBox2.ValueMember = "ID";
            comboBox2.DisplayMember = "Name";

            List<GroupName> groups = context.GroupNames.ToList();
            NewDataGrid(groups);
            Hidinglabel();
            cbSubject.DataSource = context.Subjects.ToList();
            cbSubject.ValueMember = "Sub_ID";
            cbSubject.DisplayMember = "Sub_Name";
            foreach (DataGridViewColumn c in dataGridView1.Columns)
            {
                c.DefaultCellStyle.Font = new Font("Arial", 17, FontStyle.Bold, GraphicsUnit.Pixel);
                c.DefaultCellStyle.ForeColor = Color.Black;
            }

        }
        //add group
        private void materialButton1_Click(object sender, EventArgs e)
        {
            try
            {
                Hidinglabel();
                GroupName group = new GroupName();
                bool n1 = false;
                bool n2 = false;
                bool n3 = false;
                bool n4 = false;
                bool n5 = false;
                bool n6 = false;
                bool n7 = false;
                bool n8 = false;
                bool n9 = false;
                if (Utilities.validateNameWithNumberInArabic(textBox1.Text))
                {
                    group.G_Name = textBox1.Text;
                    n1 = true;
                }
                else
                {
                    label10.Visible = true;
                }
                if (Utilities.ValidateIntegerNumbers(textBox8.Text))
                {
                    group.G_NoOfSession = Convert.ToInt32(textBox8.Text);
                    n2 = true;
                }
                else
                {
                    label13.Visible = true;
                }
                if (Utilities.ValidateFloatNumbers(textBox7.Text))
                {
                    group.G_PriceOfSession = Convert.ToDouble(textBox7.Text);
                    n3 = true;
                }
                else
                {
                    label14.Visible = true;
                }
                if (Utilities.validateNameWithNumberInArabic(textBox3.Text))
                {
                    group.Grade = textBox3.Text;
                    n4 = true;
                }
                else
                {
                    label12.Visible = true;
                }
                if (Utilities.ValidateIntegerNumbers(textBox4.Text))
                {
                    group.G_Capacity = Convert.ToInt32(textBox4.Text);
                    n5 = true;
                }
                else
                {
                    label11.Visible = true;
                }
                if (Utilities.ValidateFloatNumbers(textBox7.Text) && Utilities.ValidateFloatNumbers(textBox8.Text))
                {
                    group.G_TotalPrice = Convert.ToDouble(textBox7.Text) * Convert.ToInt32(textBox8.Text);
                    n6 = true;
                }
                if (comboBox1.Text != "" && comboBox1.Text != null)
                {
                    group.Teacher_ID = Convert.ToInt32(comboBox1.SelectedValue.ToString());
                    n7 = true;
                }

                if (comboBox2.Text != "" && comboBox2.Text != null)
                {
                    group.AcademicYear_ID = Convert.ToInt32(comboBox2.SelectedValue.ToString());
                    n8 = true;
                }
                if (cbSubject.Text != "" && cbSubject.Text != null)
                {
                    group.Sub_ID = Convert.ToInt32(cbSubject.SelectedValue.ToString());
                    n9 = true;
                }
                if (n1 && n2 && n3 && n4 && n5 && n6 && n7 && n8 && n9)
                {
                    DialogResult dialogResult = MessageBox.Show("هل أنت متأكد من الإضافة", "إضافة قاعة", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.OK)
                    {
                        int TeachID = int.Parse(comboBox1.SelectedValue.ToString());
                        int SubjID = int.Parse(cbSubject.SelectedValue.ToString());

                        Teacher_Subject teacher_Subject =
                            context.Teacher_Subject.
                            FirstOrDefault(x => x.Teacher_ID == TeachID && x.Subject_ID == SubjID);

                        if (teacher_Subject == null)
                        {
                            Teacher_Subject temp_teacher_Subject = new Teacher_Subject
                            {
                                Teacher_ID = TeachID,
                                Subject_ID = SubjID
                            };
                            context.Teacher_Subject.Add(temp_teacher_Subject);
                        }
                        Hidinglabel();
                        group.G_DateOFCreation = DateTime.Now;
                        context.GroupNames.Add(group);
                        context.SaveChanges();
                        textBox1.Text = textBox3.Text = textBox4.Text = textBox7.Text = textBox8.Text = txtCode.Text = "";
                        MessageBox.Show("تم إضافة المجموعة بنجاح", "عملية ناجحة", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        List<GroupName> groups = context.GroupNames.ToList();
                        NewDataGrid(groups);
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
        //search
        private void materialButton5_Click(object sender, EventArgs e)
        {
            Hidinglabel();
            if (textBox2.Text.Length == 0)
            {
                label15.Visible = true;               
            }
            else
            {
                int groupID;
                bool isNumber = int.TryParse(textBox2.Text, out groupID);
                List<GroupName> groups;

                if (isNumber)
                {
                    groups = (from g in context.GroupNames where g.G_ID == groupID select g).ToList();
                }
                else
                {
                    groups = (from gg in context.GroupNames where gg.G_Name.Contains(textBox2.Text) select gg).ToList();
                }
                if (groups.Count > 0)
                {
                    label15.Visible = false;
                    NewDataGrid(groups);
                }
                else
                {
                    MessageBox.Show("لا توجد مجموعة بهذا الاسم", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }             
        }
        //show all
        private void materialButton6_Click(object sender, EventArgs e)
        {
            Hidinglabel();
            List<GroupName> groups = context.GroupNames.ToList();
            NewDataGrid(groups);
        }
        //delete
        private void materialButton3_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtCode.Text.Length > 0)
                {
                    Hidinglabel();
                    int selectedIndex = dataGridView1.SelectedCells[0].RowIndex;
                    int id = Convert.ToInt32(dataGridView1.Rows[selectedIndex].Cells[0].Value);
                    if (id > 0)
                    {
                        if (txtCode.Text.Length > 0)
                        {
                            DialogResult dialogResult = MessageBox.Show("هل أنت متأكد من الحذف", "حذف مجموعة", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                            if (dialogResult == DialogResult.OK)
                            {

                                var group = (from gg in context.GroupNames where gg.G_ID == id select gg).FirstOrDefault();
                                context.GroupNames.Remove(group);
                                context.SaveChanges();
                                MessageBox.Show("تم حذف المجموعة بنجاح", "حذف مجموعة", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                List<GroupName> groups = context.GroupNames.ToList();
                                NewDataGrid(groups);
                                textBox1.Text = textBox3.Text = textBox4.Text = textBox7.Text = textBox8.Text = txtCode.Text = "";
                            }
                        }
                        else
                        { MessageBox.Show("اختر مجموعة للحذف", "خطأ في الحذف", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
                    }
                    else
                    { MessageBox.Show("اختر مجموعة للحذف", "خطأ في الحذف", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
                }
                else
                    MessageBox.Show("اختر مجموعة للحذف", "خطأ في الحذف", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch
            {
                MessageBox.Show("حدث خطأ أثناء تشغيل البرنامج", "خطأ غير متوقع", MessageBoxButtons.OK, MessageBoxIcon.Error);
                context.Dispose();
                context = new EDPCenterEntities();
            }
        }
        //update
        private void materialButton2_Click(object sender, EventArgs e)
        {
            try
            {
                Hidinglabel();
                if (txtCode.Text.Length > 0)
                {
                    int selectedIndex = dataGridView1.SelectedCells[0].RowIndex;
                    int id = Convert.ToInt32(dataGridView1.Rows[selectedIndex].Cells[0].Value);
                    var group = (from gg in context.GroupNames where gg.G_ID == id select gg).FirstOrDefault();
                    bool n1 = false;
                    bool n2 = false;
                    bool n3 = false;
                    bool n4 = false;
                    bool n5 = false;
                    bool n6 = false;
                    bool n7 = false;
                    bool n8 = false;
                    bool n9 = false;
                    if (Utilities.validateNameWithNumberInArabic(textBox1.Text))
                    {
                        group.G_Name = textBox1.Text;
                        n1 = true;
                    }
                    else
                    {
                        label10.Visible = true;
                    }
                    if (Utilities.ValidateIntegerNumbers(textBox8.Text))
                    {
                        group.G_NoOfSession = Convert.ToInt32(textBox8.Text);
                        n2 = true;
                    }
                    else
                    {
                        label13.Visible = true;
                    }
                    if (Utilities.ValidateFloatNumbers(textBox7.Text))
                    {
                        group.G_PriceOfSession = Convert.ToDouble(textBox7.Text);
                        n3 = true;
                    }
                    else
                    {
                        label14.Visible = true;
                    }
                    if (Utilities.validateNameWithNumberInArabic(textBox3.Text))
                    {
                        group.Grade = textBox3.Text;
                        n4 = true;
                    }
                    else
                    {
                        label12.Visible = true;
                    }
                    if (Utilities.ValidateIntegerNumbers(textBox4.Text))
                    {
                        group.G_Capacity = Convert.ToInt32(textBox4.Text);
                        n5 = true;
                    }
                    else
                    {
                        label11.Visible = true;
                    }
                    if (Utilities.ValidateFloatNumbers(textBox7.Text) && Utilities.ValidateFloatNumbers(textBox8.Text))
                    {
                        group.G_TotalPrice = Convert.ToDouble(textBox7.Text) * Convert.ToInt32(textBox8.Text);
                        n6 = true;
                    }
                    if (comboBox1.Text != "" && comboBox1.Text != null)
                    {/*
                        int tID = Convert.ToInt32(comboBox1.SelectedValue.ToString());
                        int subID = Convert.ToInt32(cbSubject.SelectedValue.ToString());

                        Teacher_Subject teacher_Subject =
                            context.Teacher_Subject.FirstOrDefault(x => x.Teacher_ID == tID && x.Subject_ID == subID);*/

                        group.Teacher_ID = Convert.ToInt32(comboBox1.SelectedValue.ToString());
                        n7 = true;

                    }

                    if (comboBox2.Text != "" && comboBox2.Text != null)
                    {
                        group.AcademicYear_ID = Convert.ToInt32(comboBox2.SelectedValue.ToString());
                        n8 = true;
                    }
                    if (cbSubject.Text != "" && cbSubject.Text != null)
                    {
                        group.Sub_ID = Convert.ToInt32(cbSubject.SelectedValue.ToString());
                        n9 = true;
                    }
                    if (n1 && n2 && n3 && n4 && n5 && n6 && n7 && n8 && n9)
                    {
                        //group.G_DateOFCreation = dateTimePicker1.Value;
                        int Teach_ID = int.Parse(comboBox1.SelectedValue.ToString());
                        int Subj_ID = int.Parse(cbSubject.SelectedValue.ToString());
                        
                        Teacher_Subject teacher_Subject =
                            context.Teacher_Subject
                            .Where(x=>x.Teacher_ID== Teach_ID && x.Subject_ID== Subj_ID).FirstOrDefault();
                        if (teacher_Subject == null) 
                        {
                            Teacher_Subject teach_Subj = new Teacher_Subject
                            {
                                Teacher_ID = int.Parse(comboBox1.SelectedValue.ToString()),
                                Subject_ID = int.Parse(cbSubject.SelectedValue.ToString())
                            };
                            context.Teacher_Subject.Add(teach_Subj);
                        }

                        context.SaveChanges();
                        textBox1.Text = textBox3.Text = textBox4.Text = textBox7.Text = textBox8.Text = "";
                        MessageBox.Show("تم تعديل المجموعة بنجاح", "عملية ناجحة", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Hidinglabel();
                        txtCode.Text = "";
                        List<GroupName> groups = context.GroupNames.ToList();
                        NewDataGrid(groups);
                    }
                }
                else
                {
                    MessageBox.Show("اختر مجموعة للتعديل", "خطأ في التعديل", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
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
            Hidinglabel();
            int selectedIndex = dataGridView1.SelectedCells[0].RowIndex;
            int id = Convert.ToInt32(dataGridView1.Rows[selectedIndex].Cells[0].Value);
            if (id>0)
            {
                txtCode.Text = dataGridView1.Rows[selectedIndex].Cells[0].Value.ToString();
                textBox1.Text = dataGridView1.Rows[selectedIndex].Cells[1].Value.ToString();
                textBox4.Text = dataGridView1.Rows[selectedIndex].Cells[7].Value.ToString();
                textBox3.Text = dataGridView1.Rows[selectedIndex].Cells[4].Value.ToString();
                textBox7.Text = dataGridView1.Rows[selectedIndex].Cells[8].Value.ToString();
                textBox8.Text = dataGridView1.Rows[selectedIndex].Cells[9].Value.ToString();
                cbSubject.Text = dataGridView1.Rows[selectedIndex].Cells[3].Value.ToString();
                string name = dataGridView1.Rows[selectedIndex].Cells[2].Value.ToString();
                comboBox1.SelectedValue = (from t in context.Teachers
                           where(t.T_Name == name)
                           select t.T_ID).FirstOrDefault();

                string name2 = dataGridView1.Rows[selectedIndex].Cells[5].Value.ToString();
                comboBox2.SelectedValue = (from a in context.AcademicYears
                                           where (a.Name == name2)
                                           select a.ID).FirstOrDefault();

                dateTimePicker1.Text = dataGridView1.Rows[selectedIndex].Cells[6].Value.ToString();
            }
        }

        private void NewDataGrid(List<GroupName> groupNames)
        {
            dataGridView1.Rows.Clear();

            foreach (GroupName group in groupNames)
            {
                DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[0].Clone();
                row.Cells[0].Value = group.G_ID;
                row.Cells[1].Value = group.G_Name;
                row.Cells[2].Value = group.Teacher.T_Name;
                row.Cells[3].Value = group.Subject.Sub_Name;
                row.Cells[4].Value = group.Grade;
                row.Cells[5].Value = group.AcademicYear.Name;
                row.Cells[6].Value = group.G_DateOFCreation.ToString();
                row.Cells[7].Value = group.G_Capacity.ToString();
                row.Cells[8].Value = group.G_PriceOfSession.ToString();
                row.Cells[9].Value = group.G_NoOfSession.ToString();
                row.Cells[10].Value = group.G_TotalPrice.ToString();
                dataGridView1.Rows.Add(row);
            }
        }
        private void Hidinglabel()
        {
            label10.Visible = false;
            label11.Visible = false;
            label12.Visible = false;
            label13.Visible = false;
            label14.Visible = false;
            label15.Visible = false;
            label18.Visible = false;
        }

        private void addGroup_FormClosed(object sender, FormClosedEventArgs e)
        {
            context.Dispose();
        }

        private void materialButton4_Click(object sender, EventArgs e)
        {
            addGroup_Load(null, null);
        }
    }
}