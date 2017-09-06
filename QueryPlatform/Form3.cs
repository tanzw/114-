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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            listViewEx1._rowBackColor1 = Color.Red;
            listViewEx1._rowBackColor2 = Color.Yellow;
            listViewEx1.FullRowSelect = true;
            ImageList imgList = new ImageList();
            imgList.ImageSize = new Size(1, 50);//分别是宽和高 
            listViewEx1.SmallImageList = imgList;

            
            ListViewItem lvi = new ListViewItem();
            lvi.SubItems[0].Text = "1";
            lvi.ImageIndex = 1;
            lvi.SubItems.Add("张双双大幅杀跌防守对方撒旦发生的发生的三");
            lvi.SubItems.Add("18");

            listViewEx1.Items.Add(lvi);

            ListViewItem lvi1 = new ListViewItem();
            lvi1.SubItems[0].Text = "2";
            lvi1.SubItems.Add("李四");
            lvi1.SubItems.Add("25");
            listViewEx1.Items.Add(lvi1);

            ListViewItem lvi2 = new ListViewItem();
            lvi2.SubItems[0].Text = "3";
            lvi2.SubItems.Add("王五");
            lvi2.SubItems.Add("215");
            listViewEx1.Items.Add(lvi2);

            ListViewItem lvi3 = new ListViewItem();
            lvi3.SubItems[0].Text = "4";
            lvi3.SubItems.Add("sdf");
            lvi3.SubItems.Add("5");
        
            listViewEx1.Items.Add(lvi3);
        }
    }
}
