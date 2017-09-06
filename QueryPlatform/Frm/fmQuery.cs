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
    public partial class fmQuery : Form
    {
        public delegate void QueryList(string unitName,string name,string duty,string redPhone );//声明委托
        public event QueryList OnQueryListHandler = null;//定义委托事件

        public fmQuery()
        {
            InitializeComponent();
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        }      

        private void fmQuery_Load(object sender, EventArgs e)
        {
            BindUnit();
        }

        private void BindUnit()
        {
            Code.Services.UnitService service = new Code.Services.UnitService();
            var list = service.GetDepartmentList();
            cbbUnit.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbUnit.DataSource = list;
            cbbUnit.DisplayMember = "Department";
            cbbUnit.ValueMember = "ID";
            cbbUnit.SelectedIndex = -1;
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            if (OnQueryListHandler != null)
            {
                this.DialogResult = DialogResult.OK;
                OnQueryListHandler(cbbUnit.Text, txtName.Text.Trim(), txtDuty.Text.Trim(), txtRedPhone.Text.Trim());
            }
          
        }
    }
}
