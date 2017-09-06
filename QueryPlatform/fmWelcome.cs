using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace QueryPlatform
{
    public partial class fmWelcome : Form
    {
        public fmWelcome()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            pictureBox1.Image = Image.FromFile(System.Environment.CurrentDirectory + "\\Splash.bmp");
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void fmWelcome_Load(object sender, EventArgs e)
        {
            this.timer1.Start();
            this.timer1.Interval = 5000;
          
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            fmMain2 fm = new fmMain2();
            fm.Show();
          
            this.timer1.Stop();
            this.Close();
            this.Dispose();

        }

        private void fmWelcome_FormClosed(object sender, FormClosedEventArgs e)
        {
            //关闭定时器
            //this.timer1.Stop();
        }
    }
}
