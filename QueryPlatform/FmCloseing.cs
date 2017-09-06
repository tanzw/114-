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
    public partial class FmCloseing : Form
    {
        public FmCloseing()
        {
            InitializeComponent();
        }
        public FmCloseing(Form owner)
        {
            InitializeComponent();
            this.Owner = owner;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }
    }
}
