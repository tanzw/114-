using BrightIdeasSoftware;
using QueryPlatform.Code.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace QueryPlatform.Frm
{
    public partial class fmQuickQuery : Form
    {
        Frm.fmLead fmlead = new Frm.fmLead();
        public fmQuickQuery()
        {
            InitializeComponent();

            //listView1.Columns.Add("单位", 240, HorizontalAlignment.Left);
            //listView1.Columns.Add("姓名", 120, HorizontalAlignment.Left);
            //listView1.Columns.Add("职务", 100, HorizontalAlignment.Left);
            //listView1.Columns.Add("办公红机", 80, HorizontalAlignment.Right);
            //listView1.Columns.Add("住宿红机", 80, HorizontalAlignment.Right);
            //listView1.Columns.Add("秘书红机", 80, HorizontalAlignment.Right);
            ShowInTaskbar = false;
            MaximizeBox = false;
            MinimizeBox = false;
            this.ControlBox = false;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            CreateColumns();

            Code.Services.LeadService service = new Code.Services.LeadService();
            var list = service.GetAllLeadList();
            dataListView1.ShowGroups = false;
            dataListView1.RowHeight = 50;
            dataListView1.FullRowSelect = true;
            dataListView1.AutoGenerateColumns = false;
            dataListView1.DataSource = list;

        }

        OLVColumn OlvUnitColumn;
        OLVColumn OlvNameColumn;
        OLVColumn OlvDutyColumn;
        public void CreateColumns()
        {
            OlvUnitColumn = new OLVColumn() { AspectName = "DepartmentName", Text = "单位", Width = 100, TextAlign = HorizontalAlignment.Left };
            OlvNameColumn = new OLVColumn() { AspectName = "AName", Text = "姓名", Width = 100, TextAlign = HorizontalAlignment.Left };
            OlvDutyColumn = new OLVColumn() { AspectName = "Duty", Text = "职务", Width = 80, TextAlign = HorizontalAlignment.Left };


            dataListView1.Columns.Add(OlvUnitColumn);
            dataListView1.Columns.Add(OlvNameColumn);
            dataListView1.Columns.Add(OlvDutyColumn);
            dataListView1.Columns.Add(new OLVColumn() { AspectName = "OfficeRedPhone", Text = "办公红机", Width = 80, TextAlign = HorizontalAlignment.Right });
            dataListView1.Columns.Add(new OLVColumn() { AspectName = "DormitoryRedPhone", Text = "住宿红机", Width = 80, TextAlign = HorizontalAlignment.Right });
            dataListView1.Columns.Add(new OLVColumn() { AspectName = "SecretaryRedPhone", Text = "秘书红机", Width = 80, TextAlign = HorizontalAlignment.Right });
        }

        public void ReLoadList()
        {
            Code.Services.LeadService service = new Code.Services.LeadService();
            var list = service.GetAllLeadList();
            dataListView1.DataSource = list;
        }



        public void TextAppend(char keyChar)
        {

            textBox1.Text = keyChar.ToString();
            textBox1.Select(textBox1.Text.Length, 0);
        }

        private void fmQuickQuery_Leave(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void fmQuickQuery_Activated(object sender, EventArgs e)
        {
            //textBox1.Text = textBox1.Text;
            //textBox1.Focus();
        }

        private void fmQuickQuery_Deactivate(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void fmQuickQuery_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            TimedFilter(dataListView1, textBox1.Text);
        }


        private void dataListView1_FormatRow(object sender, FormatRowEventArgs e)
        {
            e.Item.Font = new Font("宋体", 16f, FontStyle.Bold);
            e.Item.ForeColor = Color.FromArgb(98, 69, 105);
        }

        private void dataListView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                int selectedId = GetSelecedItemID();
                if (selectedId != 0)
                {
                    fmlead.InitInfo(selectedId, 2);
                    fmlead.StartPosition = FormStartPosition.CenterScreen;
                    fmlead.ShowDialog();
                }
            }
        }

        private void dataListView1_DoubleClick(object sender, EventArgs e)
        {
            int selectedId = GetSelecedItemID();
            if (selectedId != 0)
            {
                fmlead.InitInfo(selectedId, 2);
                fmlead.StartPosition = FormStartPosition.CenterScreen;
                fmlead.ShowDialog();
            }
        }

        private int GetSelecedItemID()
        {
            object obj;
            ListViewItem currentItem;

            currentItem = dataListView1.SelectedItem;
            if (currentItem == null)
            {
                return 0;
            }
            obj = dataListView1.GetModelObject(currentItem.Index);

            var model = obj as LeadModel;
            if (model != null)
            {
                return model.ID;
            }
            else
            {
                return 0;
            }
        }

        public void TimedFilter(ObjectListView olv, string txt)
        {
            string searchKey = txt;
            if (searchKey.StartsWith("#"))
            {
                searchKey = searchKey.Substring(1, searchKey.Length - 1);
                TimedFilter(olv, searchKey, 3);
            }
            else if (searchKey.StartsWith("$"))
            {
                searchKey = searchKey.Substring(1, searchKey.Length - 1);
                TimedFilter(olv, searchKey, 4);
            }
            else if (searchKey.StartsWith("@"))
            {
                searchKey = searchKey.Substring(1, searchKey.Length - 1);
                TimedFilter(olv, searchKey, 5);
            }
            else
            {
                TimedFilter(olv, searchKey, 0);
            }
        }

        public void TimedFilter(ObjectListView olv, string txt, int matchKind)
        {
            TextMatchFilter filter = null;
            if (!String.IsNullOrEmpty(txt))
            {
                switch (matchKind)
                {
                    case 0:
                    default:
                        filter = TextMatchFilter.Contains(olv, txt);
                        break;
                    case 1:
                        filter = TextMatchFilter.Prefix(olv, txt);
                        break;
                    case 2:
                        filter = TextMatchFilter.Regex(olv, txt);
                        break;
                    case 3:
                        filter = TextMatchFilter.Contains(olv, txt);
                        filter.Columns = new OLVColumn[1] { OlvUnitColumn };
                        filter.AdditionalColumns = new OLVColumn[1] { OlvUnitColumn };
                        break;
                    case 4:
                        filter = TextMatchFilter.Contains(olv, txt);
                        filter.Columns = new OLVColumn[1] { OlvNameColumn };
                        filter.AdditionalColumns = new OLVColumn[1] { OlvNameColumn };
                        break;
                    case 5:
                        filter = TextMatchFilter.Contains(olv, txt);
                        filter.Columns = new OLVColumn[1] { OlvDutyColumn };
                        filter.AdditionalColumns = new OLVColumn[1] { OlvDutyColumn };
                        break;
                }
            }

            // Text highlighting requires at least a default renderer
            if (olv.DefaultRenderer == null)
                olv.DefaultRenderer = new HighlightTextRenderer(filter);

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            olv.AdditionalFilter = filter;
            //olv.Invalidate();
            stopWatch.Stop();


        }

        private void fmQuickQuery_FormClosing(object sender, FormClosingEventArgs e)
        {
           
        }
    }
}
