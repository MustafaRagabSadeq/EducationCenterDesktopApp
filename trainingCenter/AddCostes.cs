using trainingCenter.BL;
using MetroSet_UI.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Reflection;
using System.Linq.Expressions;

namespace trainingCenter
{
    public partial class AddCostes : MetroSetForm
    {
        #region ALL Form Var
        EDPCenterEntities eDPCenterEntities;
        bool isValidPayment;
        double pp = 0.0;
        #endregion
        public AddCostes()
        {
            InitializeComponent();
            eDPCenterEntities = new EDPCenterEntities();
        }
        #region checkValidation Functaion
        public bool checkValidation()
        {
            isValidPayment = (double.TryParse(txtbCostspayment.Text, out pp));
            if (!isValidPayment)
            {
                lblValidCostspayment.Visible = true;
                return false;
            }
            else
            {
                lblValidCostspayment.Visible = false;
                return true;
            }
        }
        #endregion

        #region FormLoad
        private void AddCostes_Load(object sender, EventArgs e)
        {
            MinimumSize = MaximumSize = Size;
            #region TO get row data from DB &Added in dgv form
            EDPCenterEntities x = new EDPCenterEntities();
            List<Cost> costs = x.Costs.ToList();
            NewDataGrid(costs);
            foreach (var item in costs)
            {
                cbCostesName.Items.Add(item.Name);
            }
            #endregion
            foreach (DataGridViewColumn c in dgvCostes.Columns)
            {
                c.DefaultCellStyle.Font = new Font("Arial", 17, FontStyle.Bold, GraphicsUnit.Pixel);
                c.DefaultCellStyle.ForeColor = Color.Black;
            }
        }
        #endregion

        #region button[تعديل]
        private void btnEdite_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtbCostesID.Text.Length == 0)
                    MessageBox.Show("اختر الخدمة أولاً", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    if (checkValidation())
                    {
                        int costtId = int.Parse(txtbCostesID.Text);
                        Cost Cost = eDPCenterEntities.Costs.Where(x => x.Cost_ID == costtId).FirstOrDefault();
                        Cost.Name = cbCostesName.Text;
                        Cost.Payment = pp;
                        eDPCenterEntities.SaveChanges();
                        NewDataGrid(eDPCenterEntities.Costs.ToList());
                        MessageBox.Show("تم تعديل بيانات الخدمة", "عملية ناجحة", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        #endregion

        #region dataGridView1_CellClick
        private void dgvCostes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            DataGridViewRow row;
            // To determine row number
            if (e.RowIndex >= 0)
            {
                row = (DataGridViewRow)dgvCostes.Rows[index];
                txtbCostesID.Text = row.Cells[0].Value?.ToString() ?? "";
                cbCostesName.Text = row.Cells[1].Value?.ToString() ?? "";
                txtbCostspayment.Text = row.Cells[2].Value?.ToString() ?? "";
            }           
            index = 0;
            row = (DataGridViewRow)dgvCostes.Rows[index];
        }
        #endregion

        #region Button[عرض الكل]
        private void btnAll_Click(object sender, EventArgs e)
        {
            NewDataGrid(eDPCenterEntities.Costs.ToList());
        }
        #endregion

        #region [TextBoxLeave(Name,payments)Validation]

        private void txtbCostspayment_Leave(object sender, EventArgs e)
        {
            checkValidation();
        }
        #endregion

        #region NewdataGrid Function
        private void NewDataGrid(List<Cost> Costs)
        {
            txtbCostesID.Text = "";
            cbCostesName.Text = "";
            txtbCostspayment.Text = "";
            dgvCostes.Rows.Clear();

            foreach (Cost cost in Costs)
            {
                DataGridViewRow row = (DataGridViewRow)dgvCostes.Rows[0].Clone();
                row.Cells[0].Value = cost.Cost_ID;
                row.Cells[1].Value = cost.Name;
                row.Cells[2].Value = cost.Payment;
                dgvCostes.Rows.Add(row);
            }
        }

        #endregion

        #region ComboBoxSelect
        private void cbCostesName_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Cost costlist = eDPCenterEntities.Costs.FirstOrDefault(a => a.Name == cbCostesName.SelectedItem.ToString());
            txtbCostesID.Text = costlist.Cost_ID.ToString();
            txtbCostspayment.Text = costlist.Payment.ToString();
        }
        #endregion

        #region ClearTXT When Enter
        private void txtbCostspayment_Enter(object sender, EventArgs e)
        {
            txtbCostspayment.Text = "";
        }
        #endregion

        private void AddCostes_FormClosed(object sender, FormClosedEventArgs e)
        {
            eDPCenterEntities.Dispose();
        }
    }
}