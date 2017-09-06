using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace QueryPlatform.Frm
{
    public partial class fmTip : Form
    {
        public fmTip()
        {
            InitializeComponent();
            ShowInTaskbar = false;
            this.BackColor = Color.FromArgb(205, 219, 233);
            foreach (var ctl in this.tableLayoutPanel1.Controls)
            {
                if (ctl is Label)
                {
                    var l = ctl as Label;
                    l.Font = new Font("宋体", 22, FontStyle.Bold);
                }
            }
            foreach (var ctl in this.tableLayoutPanel2.Controls)
            {
                if (ctl is Label)
                {
                    var l = ctl as Label;
                    l.Font = new Font("宋体", 22, FontStyle.Bold);
                }
            }
           
        }

         


        public void Setsss(string unitName,string name,string  bghj)
        {
            lbUnitName.Text = unitName;
            lbName.Text = name;
            lbBGHJ.Text = bghj;
        }

        public fmTip(Form f)
        {
            InitializeComponent();

            this.BackColor = Color.FromArgb(205, 219, 233);
            this.MdiParent = f;
        }

        private void fmTip_Resize(object sender, EventArgs e)
        {

        }

       
    }
}
