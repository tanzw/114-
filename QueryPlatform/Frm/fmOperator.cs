using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QueryPlatform.Frm
{
    public partial class fmOperator : Form
    {
        public fmOperator()
        {
            InitializeComponent();
            Code.Common.CommonStyle.SetDataGridStyle(dataGridView1);
            MaximizeBox = false;
            ShowInTaskbar = false;
            MinimizeBox = false;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        }

        private void fmOperator_Load(object sender, EventArgs e)
        {
            BindGridDataSource();
        }

        public void BindGridDataSource()
        {
            Code.Services.OperatorService service = new Code.Services.OperatorService();
            dataGridView1.DataSource = service.GetOperatorList();
            //dataGridView1.SelectedRows[0].Selected = false;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            fmOperatorEdit fm = new fmOperatorEdit();
            fm.StartPosition = FormStartPosition.CenterScreen;
            if (fm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                BindGridDataSource();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择需要修改的记录！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string userName = dataGridView1[1, dataGridView1.SelectedRows[0].Index].Value.ToString();
            fmOperatorEdit fm = new fmOperatorEdit(userName);
            fm.StartPosition = FormStartPosition.CenterScreen;
            if (fm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                BindGridDataSource();
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择需要删除的记录！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (dataGridView1[1, dataGridView1.SelectedRows[0].Index].Value.ToString().ToLower() == "admin")
            {
                MessageBox.Show("admin用户不能删除！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string userName = dataGridView1[1, dataGridView1.SelectedRows[0].Index].Value.ToString();
            Code.Services.OperatorService service = new Code.Services.OperatorService();
            if (service.DelOperator(userName))
            {
                BindGridDataSource();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
