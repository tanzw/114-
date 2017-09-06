using QueryPlatform.Code.Common;
using QueryPlatform.Code.Services;
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
    public partial class fmArea : Form
    {
        int Status = 0;
        public fmArea()
        {
            InitializeComponent();
            ShowInTaskbar = false;
            Code.Common.CommonStyle.SetDataGridStyle(dataGridView1);
            MaximizeBox = false;
            MinimizeBox = false;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        }

        private void fmArea_Load(object sender, EventArgs e)
        {
            SetUserPerimission();
            BindGridDataSource();
        }

        private void BindGridDataSource(string key = "")
        {
            Code.Services.DictService service = new Code.Services.DictService();
            dataGridView1.DataSource = service.GetDictList2(key);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtKey.Text.Trim() == "*")
            {
                BindGridDataSource();
            }
            else
            {
                BindGridDataSource(txtKey.Text.Trim());
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Status = 0;
            txtArea.Text = "";
            txtCode.Text = "";
            txtDistNo.Text = "";
            txtPinyin.Text = "";
        }
        public void SetUserPerimission()
        {
            string str = CacheStrategy.Instance.GetObject(CacheKey.Role).ToString();
            switch (str)
            {
                case "超级管理员":
                case "编辑操作员":
                    this.btnClear.Visible = true;
                    this.btnDel.Visible = true;
                    this.btnSave.Visible = true;
                    break;
                case "查询操作员":
                    this.btnClear.Visible = false;
                    this.btnDel.Visible = false;
                    this.btnSave.Visible = false;
                    break;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Code.Services.DictService service = new Code.Services.DictService();
            string distNo = txtDistNo.Text.Trim();
            string code = txtCode.Text.Trim();
            string area = txtArea.Text.Trim();
            string pinyin = txtPinyin.Text.Trim();
            if (string.IsNullOrWhiteSpace(area))
            {
                MessageBox.Show("请输入地区!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(pinyin))
            {
                MessageBox.Show("请输入简拼!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(distNo))
            {
                MessageBox.Show("请输入区号!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(code))
            {
                MessageBox.Show("请输入邮编!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            bool res = false;
            if (Status == 0)
            {
                res = service.AddDist(distNo, area, code, pinyin);
            }
            else
            {
                res = service.UpdateDist(distNo, area, code, pinyin, currentDistNo, currentArea);
            }
            if (res)
            {
                if (Status == 0)
                {
                    BindGridDataSource();
                }
                else {
                    dataGridView1.CurrentRow.SetValues(area, distNo, code, pinyin);
                }
                MessageBox.Show("保存成功!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("保存失败!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择需要删除的记录！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            Code.Services.DictService service = new Code.Services.DictService();
            if (service.DelDist(currentDistNo, currentArea, currentCode, currentPinyin))
            {
                BindGridDataSource();
                MessageBox.Show("删除成功!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtArea.Text = "";
                txtCode.Text = "";
                txtDistNo.Text = "";
                txtPinyin.Text = "";
            }
            else
            {
                MessageBox.Show("删除失败!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        string currentDistNo = string.Empty;
        string currentArea = string.Empty;
        string currentCode = string.Empty;
        string currentPinyin = string.Empty;
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Status = 1;
            var rowIndex = e.RowIndex;
            
            txtArea.Text = dataGridView1[0, rowIndex].Value.ToString();
            txtCode.Text = dataGridView1[2, rowIndex].Value.ToString();
            txtDistNo.Text = dataGridView1[1, rowIndex].Value.ToString();
            txtPinyin.Text = dataGridView1[3, rowIndex].Value.ToString();
            currentArea = txtArea.Text.Trim();
            currentDistNo = txtDistNo.Text.Trim();
            currentCode = txtCode.Text.Trim();
            currentPinyin = txtPinyin.Text.Trim();
        }

        private void txtArea_TextChanged(object sender, EventArgs e)
        {
            string str = EcanConvertToCh.convertCh(txtArea.Text.Trim());
            txtPinyin.Text = str;
        }
    }
}
