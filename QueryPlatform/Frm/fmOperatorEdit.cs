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
    public partial class fmOperatorEdit : Form
    {
        int Status = 0;//0为添加,1为编辑
        int _id = 0;
        string name = "";
        string adminPWD = "";
        public fmOperatorEdit()
        {
            InitializeComponent();
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        }

        public fmOperatorEdit(int id)
        {
            InitializeComponent();
            MaximizeBox = false;
            ShowInTaskbar = false;
            MinimizeBox = false;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            _id = id;
        }
        public fmOperatorEdit(string userName)
        {
            InitializeComponent();
            MaximizeBox = false;
            ShowInTaskbar = false;
            MinimizeBox = false;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            name = userName;
        }
        private void fmOperatorEdit_Load(object sender, EventArgs e)
        {
            BindRole();
            if (_id != 0)
            {
                Code.Services.OperatorService service = new Code.Services.OperatorService();
                DataRow row = service.GetRole(_id);
                if (row == null)
                {
                    MessageBox.Show("记录不存在！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    txtUserName.ReadOnly = true;
                    txtID.Text = _id.ToString();
                    txtPassword.Text = row["Password2"].ToString();
                    adminPWD = row["Password2"].ToString();
                    txtPhone.Text = row["Phone"].ToString();
                    txtRealName.Text = row["RealName"].ToString();
                    txtPassword2.Text = row["Password2"].ToString();
                    txtUserName.Text = row["Name"].ToString();
                    cbbRole.SelectedValue = row["Role"].ToString();
                }
            }

            if (name != "")
            {
                Code.Services.OperatorService service = new Code.Services.OperatorService();
                DataRow row = service.GetRole(name);
                if (row == null)
                {
                    MessageBox.Show("记录不存在！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    txtUserName.ReadOnly = true;
                    _id = Convert.ToInt32(row["ID"]);
                    txtID.Text = row["ID"].ToString();
                    txtPassword.Text = row["Password2"].ToString();
                    adminPWD = row["Password2"].ToString();
                    txtPhone.Text = row["Phone"].ToString();
                    txtRealName.Text = row["RealName"].ToString();
                    txtPassword2.Text = row["Password2"].ToString();
                    txtUserName.Text = row["Name"].ToString();
                    cbbRole.SelectedValue = row["Role"].ToString();
                }
            }
        }

        private void BindRole()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Name");
            dt.Rows.Add("超级管理员");
            dt.Rows.Add("编辑操作员");
            dt.Rows.Add("查询操作员");
            cbbRole.DataSource = dt;
            cbbRole.DisplayMember = "Name";
            cbbRole.ValueMember = "Name";
            cbbRole.DropDownStyle = ComboBoxStyle.DropDownList;          
            cbbRole.SelectedIndex = -1;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtUserName.Text == Code.Common.CacheStrategy.Instance.GetObject(Code.Common.CacheKey.UserName).ToString())
            {
                if (txtPassword.Text != adminPWD)
                {
                    MessageBox.Show("不允许修改当前用户的密码,请在更换密码中修改！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            else
            {
                if (txtPassword.Text != txtPassword2.Text)
                {
                    MessageBox.Show("两次密码输入不一致,请重新输入！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
    
            Code.Services.OperatorService service = new Code.Services.OperatorService();
            if (service.CheckUserName(txtUserName.Text.Trim(), _id))
            {
                MessageBox.Show("对不起,您输入的登录帐号已经存在,请重新输入！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
               
            if (_id != 0)
            {
                if (service.UpdateOperator(txtUserName.Text.Trim(), txtPassword.Text, txtRealName.Text.Trim(), cbbRole.Text, txtPhone.Text.Trim(), txtID.Text.Trim()))
                {
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                }
                else
                {
                    this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                }

            }
            else
            {
                if (service.AddOperator(txtUserName.Text.Trim(), txtPassword.Text, txtRealName.Text.Trim(), cbbRole.Text, txtPhone.Text.Trim(), txtID.Text.Trim()))
                {
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                }
                else
                {
                    this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
