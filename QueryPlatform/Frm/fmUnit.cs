using QueryPlatform.Code.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QueryPlatform
{
    public partial class fmUnit : Form
    {
        public fmUnit()
        {
            InitializeComponent();
            Code.Common.CommonStyle.SetDataGridStyle(dataGridView1);
            MaximizeBox = false;
            ShowInTaskbar = false;
            MinimizeBox = false;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        }

        private void fmUnit_Load(object sender, EventArgs e)
        {
            BindGridDataSource();
            BindDict();
        }

        public void BindGridDataSource()
        {
            Code.Services.UnitService service = new Code.Services.UnitService();
            foreach (DataGridViewColumn col in this.dataGridView1.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.DataSource = service.GetDepartmentList();
            dataGridView1.SelectedRows[0].Selected = false;
        }

        public void BindDict()
        {
            Code.Services.DictService service = new Code.Services.DictService();
            cbbDict.DataSource = service.GetAreaList();
            cbbDict.DisplayMember = "Name";
            cbbDict.ValueMember = "Name";
            cbbDict.SelectedIndex = -1;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择您要修改的记录!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                Code.Services.UnitService service = new Code.Services.UnitService();
                if (service.Update(int.Parse(lbRowID.Text), txtName.Text.Trim(), txtPinyin.Text.Trim(), cbbDict.SelectedValue.ToString()))
                {
                    MessageBox.Show("修改成功", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //BindGridDataSource();
                    dataGridView1.CurrentRow.SetValues(dataGridView1.CurrentRow.Cells[0].Value, int.Parse(lbRowID.Text), txtName.Text.Trim(), cbbDict.SelectedValue.ToString(), txtPinyin.Text.Trim());
                }
                else
                {
                    MessageBox.Show("修改失败", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择您要删除的记录!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (MessageBox.Show("确定删除?", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    Code.Services.UnitService service = new Code.Services.UnitService();
                    Code.Common.Result result = service.CheckHasUser(int.Parse(lbRowID.Text));
                    if (result.Status == Code.Common.ResultStatus.Success)
                    {
                        if (service.Delete(int.Parse(lbRowID.Text)))
                        {
                            MessageBox.Show("删除成功", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            BindGridDataSource();
                            txtName.Text = "";
                            txtPinyin.Text = "";
                            cbbDict.SelectedIndex = -1;
                        }
                        else
                        {
                            MessageBox.Show("删除失败", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("删除失败," + result.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    
                }
                
            }
        }

        private void dataGridView1_CurrentCellChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var crow = dataGridView1.CurrentRow;
            lbRowID.Text = dataGridView1[1, crow.Index].Value.ToString();
            txtName.Text = dataGridView1[2, crow.Index].Value.ToString();
            txtPinyin.Text = dataGridView1[4, crow.Index].Value.ToString();
            cbbDict.SelectedValue = dataGridView1[3, crow.Index].Value.ToString();

            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                if (row.Index != crow.Index)
                {
                    row.Selected = false;
                }
            }
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            string str = EcanConvertToCh.convertCh(txtName.Text.Trim());
            txtPinyin.Text = str;
            //Code.Services.PinyinService service = new Code.Services.PinyinService();
            //txtPinyin.Text = service.GetChineseToPinyin(txtName.Text.Trim());
        }

       
    }
}
