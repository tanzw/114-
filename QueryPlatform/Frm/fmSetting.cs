using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QueryPlatform.Frm
{
    public partial class fmSetting : Form
    {

        public delegate void ShowModeChange();//声明委托
        /// <summary>
        /// 更改快捷查询方式
        /// </summary>
        public event ShowModeChange ShowModeChangeHandler = null;//定义委托事件

        public fmSetting()
        {
            InitializeComponent();

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("??", "ddd", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                Code.Common.Config.DB = textBox1.Text.Trim();
                Code.Common.Config.Date = Convert.ToInt32(textBox3.Text.Trim());
                Code.Common.Config.Password = textBox2.Text;
                Code.Common.Config.ShowMode = radioButton1.Checked ? 1 : 0;
               // Code.Common.Config.ShowWelcome = checkBox1.Checked ? 1 : 0;
              //  Code.Common.Config.WaitTime = Convert.ToInt32(textBox4.Text.Trim());
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["DB"].Value = textBox1.Text.Trim();
                config.AppSettings.Settings["Date"].Value = textBox3.Text.Trim();
                config.AppSettings.Settings["Password"].Value = textBox2.Text;
                config.AppSettings.Settings["ShowMode"].Value = radioButton1.Checked ? "1" : "0";
             //   config.AppSettings.Settings["ShowWelcome"].Value = checkBox1.Checked ? "1" : "0";
             //   config.AppSettings.Settings["WaitTime"].Value = Code.Common.Config.WaitTime.ToString();
                config.Save();
                if (ShowModeChangeHandler != null)
                {
                    if (!radioButton1.Checked)
                    {
                        ShowModeChangeHandler();
                    }
                }
                DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            catch (Exception ex)
            {
                DialogResult = System.Windows.Forms.DialogResult.None;
            }
        }

        private void fmSetting_Load(object sender, EventArgs e)
        {
            textBox1.Text = Code.Common.Config.DB;
            textBox2.Text = Code.Common.Config.Password;
            textBox3.Text = Code.Common.Config.Date.ToString();
           // textBox4.Text = Code.Common.Config.WaitTime.ToString();
            if (Code.Common.Config.ShowMode == 1)
            {
                radioButton1.Checked = true;
                radioButton2.Checked = false;
            }
            else
            {
                radioButton1.Checked = false;
                radioButton2.Checked = true;
            }

           
        }
    }
}
