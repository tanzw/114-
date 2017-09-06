using QueryPlatform.Code.Common;
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
    public partial class Login : BaseForm
    {
        public Login()
        {
            InitializeComponent();
            AcceptButton = btnLogin;
        }
        public Login(string text)
        {
            InitializeComponent();
            AcceptButton = btnLogin;
            this.Text = text;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(txtUserName.Text))
            {
                MessageBox.Show("用户名不能为空!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show("密码不能为空!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            Code.Services.LoginService service = new Code.Services.LoginService();
            Result result = service.Login(txtUserName.Text, txtPassword.Text);
            if (result.Status == ResultStatus.Success)
            {

                string[] strarr = result.Message.Split('|');
                UserName = txtUserName.Text.Trim();
                RealName = strarr[0];
                Role = strarr[1];

                Code.Common.CacheStrategy.Instance.SetObj(CacheKey.UserName, UserName);
                Code.Common.CacheStrategy.Instance.SetObj(CacheKey.RealName, RealName);
                Code.Common.CacheStrategy.Instance.SetObj(CacheKey.Role, Role);

                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            else
            {
                MessageBox.Show("帐号或密码不正确,请重新输入!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
        {

            int WM_KEYDOWN = 256;

            int WM_SYSKEYDOWN = 260;

            if (msg.Msg == WM_KEYDOWN | msg.Msg == WM_SYSKEYDOWN)
            {
                switch (keyData)
                {
                    case Keys.Escape:
                        if (CacheStrategy.Instance.GetObject(CacheKey.UserName) != null)
                        {
                            this.Close();
                        }
                        else
                        {
                            Application.Exit();
                        }
                        break;
                }
            }
            return false;
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (CacheStrategy.Instance.GetObject(CacheKey.UserName) != null)
            {
                this.Close();
            }
            else
            {
                Application.Exit();
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
           
        }

        private void Login_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (CacheStrategy.Instance.GetObject(CacheKey.UserName) != null)
            {
              // this.Close();
            }
            else
            {
                Application.Exit();
            }
        }
    }
}
