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
    public partial class fmRedQuery : Form
    {
        public fmRedQuery()
        {
            InitializeComponent();
            ShowInTaskbar = false;
            listView1.GridLines = true;
            listView1.FullRowSelect = true;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        public void BindDict()
        {
            Code.Services.DictService service = new Code.Services.DictService();
            cbbDict.DataSource = service.GetDictList();
            cbbDict.DisplayMember = "PlaceName";
            cbbDict.ValueMember = "PlaceName";
        }

        private void fmRedQuery_Load(object sender, EventArgs e)
        {
            MaximizeBox = false;
            MinimizeBox = false;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            BindDict();
            CreateColums();
            label6.BackColor = Color.FromArgb(216, 128, 96);
            label8.BackColor = Color.FromArgb(216, 192, 64);
            label4.BackColor = Color.FromArgb(0, 192, 0);
            label1.BackColor = Color.FromArgb(64, 128, 216);
            AddData();
        }

        public void CreateColums()
        {
            listView1.BorderStyle = BorderStyle.None;
            for (int i = 0; i < 10; i++)
            {
                listView1.Columns.Add("列" + i, 48, HorizontalAlignment.Center);
            }
            listView1.HeaderStyle = ColumnHeaderStyle.None;
        }

        public void AddData()
        {
            int initValue = 2000;
            int tempValue = initValue;
            this.listView1.BeginUpdate(); 
            for (int i = 0; i < 200; i++)
            {
                ListViewItem lvi = new ListViewItem();


                lvi.SubItems[0].Text = tempValue.ToString();
                lvi.SubItems[0].Font = new Font("宋体", 12, FontStyle.Bold);
                lvi.SubItems[0].ForeColor = Color.FromArgb(255, 255, 255);
                lvi.SubItems[0].BackColor = Color.FromArgb(216, 128, 96);
                for (int j = 0; j < 10; j++)
                {
                    tempValue = tempValue + 1;
                    lvi.SubItems.Add(tempValue.ToString());
                }
                listView1.Items.Add(lvi);
            }
            this.listView1.EndUpdate(); 

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }
    }
}
