using BrightIdeasSoftware;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QueryPlatform
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            InitTree();
        }
        private void InitTree()
        {
            TreeListView.TreeRenderer renderer = this.treeListView1.TreeColumnRenderer;
            renderer.LinePen = new Pen(Color.Firebrick, 0.5f);
            renderer.LinePen.DashStyle = DashStyle.Dot;
            treeListView1.FullRowSelect = true;
            Code.Services.LeadService service = new Code.Services.LeadService();
            var list= service.GetTreeLeadList("浙江");
             
            CreateColums(treeListView1);
            

            treeListView1.RowHeight = 50;
            // We just want to get the children of the given directory.
            // This becomes a little complicated when we can't (for whatever reason). We need to report the error 
            // to the user, but we can't just call MessageBox.Show() directly, since that would stall the UI thread
            // leaving the tree in a potentially undefined state (not good). We also don't want to keep trying to
            // get the contents of the given directory if the tree is refreshed. To get around the first problem,
            // we immediately return an empty list of children and use BeginInvoke to show the MessageBox at the 
            // earliest opportunity. We get around the second problem by collapsing the branch again, so it's children
            // will not be fetched when the tree is refreshed. The user could still explicitly unroll it again --
            // that's their problem :)

            treeListView1.IsSimpleDragSource = true;
            treeListView1.IsSimpleDropSink = true;
            //treeListView1.DragSource = new SimpleDragSource();
            //this.treeListView1.DropSink = new RearrangingDropSink(true);

            treeListView1.ModelCanDrop += delegate(object sender, ModelDropEventArgs e)
            {
                e.Effect = DragDropEffects.None;
                if (e.TargetModel == null)
                    return;
                
                if (e.TargetModel is Code.Services.LeadModel)
                    e.Effect = e.StandardDropActionFromKeys;
                else
                    e.InfoMessage = "Can only drop on directories";
            };

            treeListView1.ModelDropped += delegate(object sender, ModelDropEventArgs e)
            {
                MessageBox.Show(e.DropTargetItem.Text);
               // treeListView1.UpdateVirtualListSize();
            };

            this.treeListView1.CanExpandGetter = delegate(object x)
            {
                return ((Code.Services.LeadModel)x).HasChild;
            };

            this.treeListView1.ChildrenGetter = delegate(object x)
            {
                try
                {
                    return ((Code.Services.LeadModel)x).GetChildLead(list);
                }
                catch (UnauthorizedAccessException ex)
                {
                    this.BeginInvoke((MethodInvoker)delegate()
                    {
                        this.treeListView1.Collapse(x);
                        MessageBox.Show(this, ex.Message, "ObjectListViewDemo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    });
                    return new ArrayList();
                }
            };

            // Once those two delegates are in place, the TreeListView starts working
            // after setting the Roots property.

            // List all drives as the roots of the tree
            ArrayList roots = new ArrayList();

            Code.Services.LeadModel model = new Code.Services.LeadModel();
            model.AName = "浙江";
            model.ID = 0;
            model.Level = 1;
            roots.Add(model);
            this.treeListView1.Roots = roots;
            //this.treeListView1.u
        }
        public void CreateColums(TreeListView listview)
        {
            //  listView1.HeaderStyle= 
            listview.Columns.Add(new OLVColumn() { AspectName = "ID", Text = "标题id", TextAlign = HorizontalAlignment.Center });
            listview.Columns.Add(new OLVColumn() { AspectName = "Name", Text = "标题Name", TextAlign = HorizontalAlignment.Center });
            listview.Columns.Add(new OLVColumn() { AspectName = "Name", Text = "标题", TextAlign = HorizontalAlignment.Center });
            
            //listview.Columns.Add( "序号", 90, HorizontalAlignment.Center);
            //listview.Columns.Add("单位", 320, HorizontalAlignment.Left);
            //listview.Columns.Add("姓名", 130, HorizontalAlignment.Left);
            //listview.Columns.Add("职务", 150, HorizontalAlignment.Left);
            //listview.Columns.Add("办公红机", 120, HorizontalAlignment.Right);
            //listview.Columns.Add("住宿红机", 120, HorizontalAlignment.Right);
            //listview.Columns.Add("秘书红机", 120, HorizontalAlignment.Right);
            //listview.Columns.Add("办公普机", 185, HorizontalAlignment.Right);
            //listview.Columns.Add("住宿普机", 225, HorizontalAlignment.Right);
            //listview.Columns.Add("手机", 215, HorizontalAlignment.Right);
            //listview.Columns.Add("传真", 225, HorizontalAlignment.Right);
            //listview.Columns.Add("秘书", 160, HorizontalAlignment.Left);
            //listview.Columns.Add("秘书普机", 160, HorizontalAlignment.Right);
            //listview.Columns.Add("秘书手机", 165, HorizontalAlignment.Right);
            //listview.Columns.Add("秘书传真", 225, HorizontalAlignment.Right);
            //listview.Columns.Add("办公地址", 375, HorizontalAlignment.Left);
            //listview.Columns.Add("住宿地址", 400, HorizontalAlignment.Left);
            //listview.Columns.Add("秘书办公地址", 375, HorizontalAlignment.Left);
            //listview.Columns.Add("秘书住宿地址", 400, HorizontalAlignment.Left);
            //listview.Columns.Add("备注1", 380, HorizontalAlignment.Left);
            //listview.Columns.Add("备注2", 380, HorizontalAlignment.Left);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var d = treeListView1.TreeModel;
            //var m = d.objectList[0] as Code.Services.LeadModel;
           // m.ID = 11;
            var m = d.GetNthObject(1) as Code.Services.LeadModel;
            m.AName = "dddd";

            var m1 = d.GetNthObject(2) as Code.Services.LeadModel;
            m1.AName = "s";
              
        }
    }
}
