using QueryPlatform.Code.Common;
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
    public partial class fmChangePwd : Form
    {
        public fmChangePwd()
        {
            InitializeComponent();
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            AcceptButton = btnOK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtPassword.Text != txtPassword2.Text)
            {
                MessageBox.Show("两次输入新密码不一致,请重新输入!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword2.Focus();
                return;
            }
            Code.Services.OperatorService service = new Code.Services.OperatorService();

            Code.Common.Result result= service.ChangePassword(CacheStrategy.Instance.GetObject(CacheKey.UserName).ToString(), txtOldPwd.Text, txtPassword.Text);
            if (result.Status == ResultStatus.Success)
            {
                MessageBox.Show("更换成功!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            else
            {
                MessageBox.Show("更换失败," + result.Message + "!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
