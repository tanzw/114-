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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        List<Code.Services.DepartmentModel> dtUnit = null;
        private void BindUnit(string area = "")
        {
            Code.Services.UnitService service = new Code.Services.UnitService();
            dtUnit = service.GetDepartmentList();
            cbbUnit.DropDownStyle = ComboBoxStyle.DropDown;
            if (area == "")
            {
                cbbUnit.DataSource = dtUnit;
            }
            else
            {

                var unit = dtUnit.Where(x => x.Dict == area).OrderBy(x => x.ID).OrderBy(x => x.Code).ToList();
                cbbUnit.DataSource = unit;
                
            }

            cbbUnit.DisplayMember = "Department";
            cbbUnit.ValueMember = "ID";
            cbbUnit.SelectedIndex = -1;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            toolStripButton1.BackColor = Color.FromArgb(153, 204, 255);
            toolStripButton1.Text = "阿斯达斯";
            BindUnit();
            toolTip1.Dispose();

            toolTip1 = new ToolTip();

            toolTip1.AutomaticDelay = 100;
            toolTip1.AutoPopDelay = 5000;
            toolTip1.ReshowDelay = 0;
            //this.toolTip1.ShowAlways = true;
            //this.toolTip1.Show("阿斯达", button1,new Point(100,200), 5000);
            // toolTip1.SetToolTip(button1, "飒沓");

        }
        Thread thread2;
        private void button1_Click(object sender, EventArgs e)
        {
            thread2 = new Thread(threadPro);//创建新线程  
            thread2.Start();
            Thread.Sleep(2000);
        }
        private void threadPro()
        {
            Frm.fmTip fm = new Frm.fmTip();
            fm.ShowDialog();
        }

        //

        private void button1_MouseMove(object sender, MouseEventArgs e)
        {
            //if (!toolTip1.)
            //{
            //    this.button1.Text = "OOO";
            //    this.toolTip1.Show("阿斯达", button1, new Point(100, 200), 5000);
            //}
            //else {
            //    this.button1.Text = "ddd";
            //}
        }

        private void toolStripButton1_CheckedChanged(object sender, EventArgs e)
        {
            //if (sender is ToolStripButton)
            //{
            //    ToolStripButton obj = sender as ToolStripButton;
            //    if (obj.Checked)
            //    {
            //        obj.BackColor = Color.FromArgb(153, 204, 255);
            //    }
            //    else
            //    {
            //        obj.BackColor = System.Drawing.SystemColors.Control;
            //    }
            //}

        }
      
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            //if (sender is ToolStripButton)
            //{
            //    ToolStripButton obj = sender as ToolStripButton;
            //    obj.Checked = true;
            //    if (obj.Checked)
            //    {
            //        obj.BackColor = Color.Red;// Color.FromArgb(153, 204, 255);
            //    }
            //    else
            //    {
            //        obj.BackColor = System.Drawing.SystemColors.Control;
            //    }
            //}
        }
    }
}
