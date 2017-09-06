using BrightIdeasSoftware;
using QueryPlatform.Code.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using QueryPlatform.Code.Services;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Configuration;

namespace QueryPlatform
{
    public partial class fmMain2 : Form
    {
        int LastQueryType = 0;

        Frm.fmLead fmlead = new Frm.fmLead();
        Frm.fmQuickQuery fmQQ = null;// new Frm.fmQuickQuery();
        Form4 fmTip = new Form4();
        public fmMain2()
        {
            InitializeComponent();
            this.ListViewItemsSource = new List<ListViewItem>();
            this.CurrentCacheItemsSource = new List<ListViewItem>();

            #region 初始化窗体信息
            toolStripStatusLabel1.Text = "";
            toolStripStatusLabel2.Text = "";
            toolStripStatusLabel3.Text = "";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;

            panel1.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory + "\\picbar.bmp");
            panel1.BackColor = Color.Transparent;

            lbTitle.Text = "浙江省专用通信局 -- 114号码查询系统";
            lbTitle.BackColor = Color.Transparent;
            lbTitle.Font = Config.TitleFont;
            lbTitle.ForeColor = Color.FromArgb(226, 141, 65); //Color.FromName("");
            #endregion

            toolStripTextBox1.AutoSize = false;
            toolStripTextBox1.Height = 40;

            if (Config.ShowMode == 0)
            {
                fmQQ = new Frm.fmQuickQuery();
            }

            Code.Services.LeadService s = new Code.Services.LeadService();

            InitObjectListView();
            InitTreeListView();
            InitAreaControls();

            fmTip.BindControls(objectListView1, this);
        }

        protected List<LeadModel> ItemsSource
        {
            get;
            private set;
        }

        protected List<ListViewItem> ListViewItemsSource
        {
            get;
            private set;
        }

        protected List<LeadModel> TreeDataSourceList
        {
            get;
            private set;
        }

        protected List<ListViewItem> CurrentCacheItemsSource
        {
            get;
            private set;
        }

        private void LoadListViewItems(List<ListViewItem> items)
        {
            objectListView1.SetSelectedIndex(-1);
            objectListView1.Items.Clear();
            objectListView1.VirtualMode = true;
            if (items == null)
            {
                return;
            }
            int index = 1;
            CurrentCacheItemsSource.ForEach(x =>
            {
                x.SubItems[0].Text = index.ToString();
                index++;
            });
            objectListView1.GridLines = false;
            objectListView1.FullRowSelect = true;
            objectListView1.View = View.Details;
            objectListView1.Scrollable = true;
            objectListView1.MultiSelect = false;
            objectListView1.HeaderStyle = ColumnHeaderStyle.Clickable;
            objectListView1.Visible = true;

            objectListView1.VirtualListSize = items.Count;

            objectListView1.RetrieveVirtualItem += new RetrieveVirtualItemEventHandler(listView_RetrieveVirtualItem);
            objectListView1.Refresh();
            fmTip.HideGlass();
            this.toolStripStatusLabel1.Text = "查询完成";
        }

        //递归去掉红机前面的区号
        private string RemoveAreaCode(string str)
        {
            if (!str.Contains('-'))
            {
                return str;
            }
            string[] arr = str.Split(',');
            string valueItem = string.Empty;
            string resultValue = string.Empty;
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i].Contains('-'))
                {
                    valueItem = arr[i].Remove(0, arr[i].IndexOf('-') + 1);
                }
                else
                {
                    valueItem = arr[i];
                }
                resultValue = valueItem + ",";
            }
            if (resultValue.EndsWith(","))
            {
                resultValue = resultValue.Remove(resultValue.Length - 1, 1);
            }
            return resultValue;
        }

        public void GetItemsSource()
        {
            LeadService service = new LeadService();
            var list = service.GetAllTreeLeadList();
            ItemsSource = list.Where(x => x.Level == 3).OrderBy(x => x.ID).OrderBy(x => x.Code).OrderBy(x => x.DepartmentSort).OrderBy(x => x.AreaCode).ToList();
            ListViewItemsSource = GetAllListViewItems(list.Where(x => x.Level == 3).OrderBy(x => x.ID).OrderBy(x => x.Code).OrderBy(x => x.DepartmentSort).OrderBy(x => x.AreaCode).ToList());
            TreeDataSourceList = list.OrderBy(x => x.ID).OrderBy(x => x.Code).OrderBy(x => x.DepartmentSort).OrderBy(x => x.AreaCode).ToList();
        }

        private List<ListViewItem> GetAllListViewItems(List<LeadModel> list)
        {

            List<ListViewItem> items = new List<ListViewItem>();
            ListViewItem item = null;
            int i = 1;

            Font font = Config.QueryListFont;
            Color defaultForeColor = System.Drawing.SystemColors.WindowText;
            Color defaultBackColor = System.Drawing.SystemColors.Window;

            list.ForEach(x =>
            {
                item = new ListViewItem();
                item.UseItemStyleForSubItems = false;
                item.SubItems[0].Text = i.ToString();
                item.SubItems[0].ForeColor = defaultForeColor;
                item.SubItems[0].Font = font;
                //x.OfficeRedPhone = RemoveAreaCode(x.OfficeRedPhone);
                //x.DormitoryRedPhone = RemoveAreaCode(x.DormitoryRedPhone);
                //x.SecretaryRedPhone = RemoveAreaCode(x.SecretaryRedPhone);
                item.SubItems.Add(x.DepartmentName, defaultForeColor, defaultBackColor, font);
                item.SubItems.Add(x.AName, defaultForeColor, defaultBackColor, font);
                item.SubItems.Add(x.Duty, defaultForeColor, defaultBackColor, font);
                item.SubItems.Add(RemoveAreaCode(x.OfficeRedPhone), defaultForeColor, defaultBackColor, font);
                item.SubItems.Add(RemoveAreaCode(x.DormitoryRedPhone), defaultForeColor, defaultBackColor, font);
                item.SubItems.Add(RemoveAreaCode(x.SecretaryRedPhone), defaultForeColor, defaultBackColor, font);
                item.SubItems.Add(x.Phone, defaultForeColor, defaultBackColor, font);
                item.SubItems.Add(x.DormitoryPhone, defaultForeColor, defaultBackColor, font);
                item.SubItems.Add(x.AnyCall, defaultForeColor, defaultBackColor, font);
                item.SubItems.Add(x.Fax, defaultForeColor, defaultBackColor, font);
                item.SubItems.Add(x.Secretary, defaultForeColor, defaultBackColor, font);
                item.SubItems.Add(x.SecretaryDormitoryRedPhone, defaultForeColor, defaultBackColor, font);
                item.SubItems.Add(x.SecretaryAnyCall, defaultForeColor, defaultBackColor, font);
                item.SubItems.Add(x.SecretaryFax, defaultForeColor, defaultBackColor, font);
                item.SubItems.Add(x.OfficeAddress, defaultForeColor, defaultBackColor, font);
                item.SubItems.Add(x.DormitoryAddress, defaultForeColor, defaultBackColor, font);
                item.SubItems.Add(x.SecretaryAddress, defaultForeColor, defaultBackColor, font);
                item.SubItems.Add(x.SecretaryDormitoryAddress, defaultForeColor, defaultBackColor, font);
                item.SubItems.Add(x.Mark1, defaultForeColor, defaultBackColor, font);
                item.SubItems.Add(x.Mark2, defaultForeColor, defaultBackColor, font);
                item.SubItems.Add(x.NamePinyin, defaultForeColor, defaultBackColor, font);
                item.SubItems.Add(x.DutyPinyin, defaultForeColor, defaultBackColor, font);
                item.SubItems.Add(x.DepartmentPinyin, defaultForeColor, defaultBackColor, font);
                item.SubItems.Add(x.Area, defaultForeColor, defaultBackColor, font);
                item.SubItems.Add(x.ID.ToString(), defaultForeColor, defaultBackColor, font);


                item.SubItems[2].ForeColor = Color.FromArgb(128, 0, 54);
                item.SubItems[4].ForeColor = Color.FromArgb(38, 38, 248);
                item.SubItems[5].ForeColor = Color.FromArgb(38, 38, 248);
                item.SubItems[6].ForeColor = Color.FromArgb(38, 38, 248);
                //item.SubItems[5].ForeColor = Color.FromArgb(38, 38, 248);//TODO:绿色

                item.Tag = x;
                items.Add(item);
            });
            return items;
        }

        private void InitObjectListView()
        {
            objectListView1.View = View.Details;

            ImageList imgList = new ImageList();
            imgList.ImageSize = new Size(1, 50);//分别是宽和高 

            objectListView1.SmallImageList = imgList;

            objectListView1.KeyDown += new KeyEventHandler(ListView_KeyDown);

            objectListView1.DoubleClick += new System.EventHandler(this.ListView_DoubleClick);
            objectListView1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ListView_KeyPress);
            objectListView1.MouseClick += new MouseEventHandler(ListView_Click);

            CreateColums(objectListView1);
        }

        private void InitTreeListView()
        {
            treeListView1.RowHeight = 50;
            treeListView1.FullRowSelect = true;
            treeListView1.AlternateRowBackColor = Color.FromArgb(255, 217, 217);
            treeListView1.UseOverlays = false;
            treeListView1.UseHotItem = false;
            treeListView1.UseHyperlinks = false;
            treeListView1.Font = Config.TreeListFont;
            var headstyle = new HeaderStateStyle();
            headstyle.Font = new Font("宋体", 12, FontStyle.Bold);
            treeListView1.HeaderFormatStyle = new HeaderFormatStyle();
            treeListView1.HeaderFormatStyle.Normal = headstyle;
            treeListView1.HeaderFormatStyle.Hot = headstyle;

            treeListView1.HotItemStyle = new HotItemStyle();
            treeListView1.HotItemStyle.BackColor = treeListView1.SelectedBackColor;

            treeListView1.KeyDown += new KeyEventHandler(ListView_KeyDown);
            treeListView1.FormatCell += new System.EventHandler<BrightIdeasSoftware.FormatCellEventArgs>(this.ListView_FormatCell);
            treeListView1.FormatRow += new System.EventHandler<BrightIdeasSoftware.FormatRowEventArgs>(this.ListView_FormatRow);
            treeListView1.DoubleClick += new System.EventHandler(this.ListView_DoubleClick);
            treeListView1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ListView_KeyPress);

            CreateTreeColums(treeListView1);

            treeListView1.IsSimpleDragSource = true;
            treeListView1.IsSimpleDropSink = true;

            treeListView1.CanExpandGetter = delegate(object x)
            {
                return ((LeadModel)x).HasChild;
            };
            this.treeListView1.ChildrenGetter = delegate(object x)
            {
                try
                {
                    return GetChildLead((LeadModel)x);
                }
                catch (UnauthorizedAccessException ex)
                {
                    return new ArrayList();
                }
            };

            treeListView1.ModelCanDrop += delegate(object sender, ModelDropEventArgs e)
            {
                #region 拖动排序
                e.Effect = DragDropEffects.None;
                var sourceModel = e.SourceModels[0] as LeadModel;

                if (sourceModel.Level == 1)
                {
                    e.InfoMessage = "地区不允许拖动";
                    return;
                }
                if (sourceModel == e.TargetModel)
                {
                    return;
                }

                if (e.TargetModel == null)
                    return;

                if (e.TargetModel is Code.Services.LeadModel)
                {
                    var TargetModel = e.TargetModel as LeadModel;

                    if (sourceModel.Level != TargetModel.Level)
                    {
                        e.InfoMessage = "只能拖到同级的目标位置";
                    }
                    else
                    {
                        if (TargetModel.Level == 3)
                        {
                            if (TargetModel.DepartmentID == sourceModel.DepartmentID)
                            {
                                e.Effect = e.StandardDropActionFromKeys;
                            }
                            else
                            {
                                e.InfoMessage = "不能拖动到其他单位";
                            }
                        }
                        else if (sourceModel.Level == 2)
                        {
                            if (TargetModel.Level == 1)
                            {
                                e.InfoMessage = "单位不能拖动到区域外面";
                            }
                            else
                            {
                                e.Effect = e.StandardDropActionFromKeys;
                            }
                        }
                        else
                        {
                            e.InfoMessage = "区域信息不能拖动";
                        }
                    }
                }
                else
                    e.InfoMessage = "不允许拖动到当前位置";
                #endregion
            };

            treeListView1.ModelDropped += delegate(object sender, ModelDropEventArgs e)
            {
                try
                {
                    #region 拖动排序
                    var sourceModel = e.SourceModels[0] as LeadModel;
                    var TargetModel = e.TargetModel as LeadModel;

                    if (sourceModel.Level == 2)
                    {
                        treeListView1.Collapse(sourceModel);
                        ArrayList l = (ArrayList)treeListView1.GetChildren(sourceModel);

                        treeListView1.RemoveObjects(l);
                        treeListView1.Collapse(TargetModel);
                        ArrayList l2 = (ArrayList)treeListView1.GetChildren(TargetModel);
                        // treeListView1.MoveObjects(0, l);
                        treeListView1.RemoveObjects(l2);
                        #region 从新获取新的对象
                        int cs = 0;
                        foreach (object val in treeListView1.TreeModel.objectList)
                        {
                            if (cs == 2)
                            {
                                break;
                            }
                            LeadModel tm = val as LeadModel;
                            if (tm.DepartmentName == sourceModel.DepartmentName && tm.Level == sourceModel.Level)
                            {
                                cs++;
                                sourceModel = tm;
                                continue;
                            }
                            if (tm.DepartmentName == TargetModel.DepartmentName && tm.Level == TargetModel.Level)
                            {
                                cs++;
                                TargetModel = tm;
                                continue;
                            }
                        }
                        #endregion

                    }
                    LeadModel ParentModel = treeListView1.GetParent(sourceModel) as LeadModel;

                    var ParentIndex = treeListView1.TreeModel.GetObjectIndex(ParentModel);
                    var StartIndex = ParentIndex + 1;
                    var SourceIndex = treeListView1.TreeModel.GetObjectIndex(sourceModel);


                    var count = 0;
                    if (SourceIndex > -1)
                    {
                        count = (e.DropTargetIndex > SourceIndex ? e.DropTargetIndex + 1 : SourceIndex + 1) - StartIndex;
                    }
                    else
                    {
                        if (sourceModel.Level == 2)
                        {
                            count = TreeDataSourceList.Count(x => x.Area == area && x.Level == sourceModel.Level);
                        }
                        else
                        {
                            count = TreeDataSourceList.Count(x => x.Area == area && x.Level == sourceModel.Level && x.DepartmentID == sourceModel.DepartmentID);
                        }
                    }

                    List<LeadModel> T_SortList;
                    if (TargetModel.Level == 2)
                    {
                        T_SortList = TreeDataSourceList.Where(x => x.Area == area && x.Level == 2).OrderBy(x => x.ID).OrderBy(x => x.DepartmentSort).Take(count).ToList();
                    }
                    else
                    {
                        T_SortList = TreeDataSourceList.Where(x => x.Area == area && x.Level == 3 && x.DepartmentID == TargetModel.DepartmentID).OrderBy(x => x.ID).OrderBy(x => x.Code).OrderBy(x => x.DepartmentSort).Take(count).ToList();
                    }

                    int i = 1;

                    //List<LeadModel> SortList = new List<LeadModel>();
                    if (sourceModel.Level == 2)
                    {
                        T_SortList.ForEach(x =>
                        {
                            x.Code = i;
                            x.DepartmentSort = i;
                            i++;
                        });
                    }
                    else
                    {
                        T_SortList.ForEach(x =>
                        {
                            x.Code = i;
                            i++;
                        });
                    }


                    if (SourceIndex < e.DropTargetIndex)
                    {
                        var TempModel = sourceModel.CopyModel();
                        for (int j = SourceIndex + 1; j < e.DropTargetIndex; j++)
                        {
                            var mm = treeListView1.TreeModel.GetNthObject(j);
                            var bb = mm as LeadModel;
                            var ff = treeListView1.TreeModel.GetNthObject(j - 1);
                            var ffMM = ff as LeadModel;
                            ffMM.ReplaceModel(bb);
                        }
                        var tttt = treeListView1.TreeModel.GetNthObject(e.DropTargetIndex - 1);
                        var ttMM = tttt as LeadModel;
                        ttMM.ReplaceModel(TempModel);

                    }
                    else if (SourceIndex > e.DropTargetIndex)
                    {
                        var TempModel = sourceModel.CopyModel();

                        for (int j = SourceIndex; j > e.DropTargetIndex; j--)
                        {
                            var mm = treeListView1.TreeModel.GetNthObject(j);
                            var bb = mm as LeadModel;
                            var ff = treeListView1.TreeModel.GetNthObject(j - 1);
                            var ffMM = ff as LeadModel;

                            bb.ReplaceModel(ffMM);
                        }
                        TargetModel.ReplaceModel(TempModel);


                    }
                    else
                    {

                    }

                    //if (sourceModel.Level == 3)
                    //{
                    //    var TempModel = sourceModel.CopyModel();
                    //    sourceModel.ReplaceModel(TargetModel);
                    //    TargetModel.ReplaceModel(TempModel);
                    //}
                    //else
                    //{
                    //    var TempModel = sourceModel.CopyModel();
                    //    sourceModel.ReplaceModel(TargetModel);
                    //    TargetModel.ReplaceModel(TempModel);
                    //}
                    //var nSModel = SortList.FirstOrDefault(x => x.ID == sourceModel.ID && x.Level == sourceModel.Level);
                    //var nTModel = SortList.FirstOrDefault(x => x.ID == TargetModel.ID && x.Level == TargetModel.Level);

                    //if (nSModel == null)
                    //{
                    //    nSModel = TreeDataSourceList.FirstOrDefault(x => x.ID == sourceModel.ID && x.Level == sourceModel.Level);
                    //    SortList.Add(nSModel);
                    //}
                    //if (nTModel == null)
                    //{
                    //    nTModel = TreeDataSourceList.FirstOrDefault(x => x.ID == TargetModel.ID && x.Level == TargetModel.Level);
                    //    SortList.Add(nTModel);
                    //}

                    //var sds = nSModel.Code;
                    //nSModel.Code = nTModel.Code;
                    //nTModel.Code = sds;

                    Code.Services.LeadService service = new LeadService();
                    if (sourceModel.Level == 2)
                    {
                        service.UpdateDepartmentSort(T_SortList);
                    }
                    else
                    {
                        service.UpdateSort(T_SortList);
                    }
                    GetItemsSource();
                    #endregion
                }
                catch
                {
                }
            };
        }

        private void InitAreaControls()
        {
            刷新ToolStripSplitButton.Font = Config.AreaFont;

            Config.AreaList.ForEach(x =>
            {
                ToolStripButton toolStripButton = new ToolStripButton();
                toolStripButton.Text = x;
                //toolStripButton.BackColor = Color.Red;
                toolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
                toolStripButton.Font = Config.AreaFont;
                toolStripButton.Click += new System.EventHandler(this.AreaToolStripButton_Click);
                toolStrip1.Items.Add(toolStripButton);
            });


        }

        public void CreateColums(ListView listview)
        {
            //  listView1.HeaderStyle= 




            listview.Columns.Add("序号", 115, HorizontalAlignment.Right);
            //listview.Columns.Add(new ColumnHeader() { Name = "序号", TextAlign = HorizontalAlignment.Right, ImageList = imgList, Width = 115, Text = "序号" });
            listview.Columns.Add("单位", 320, HorizontalAlignment.Left);
            listview.Columns.Add("姓名", 130, HorizontalAlignment.Left);
            listview.Columns.Add("职务", 150, HorizontalAlignment.Left);
            listview.Columns.Add("办公红机", 120, HorizontalAlignment.Right);
            listview.Columns.Add("住宿红机", 120, HorizontalAlignment.Right);
            listview.Columns.Add("秘书红机", 120, HorizontalAlignment.Right);
            listview.Columns.Add("办公普机", 185, HorizontalAlignment.Right);
            listview.Columns.Add("住宿普机", 225, HorizontalAlignment.Right);
            listview.Columns.Add("手机", 215, HorizontalAlignment.Right);
            listview.Columns.Add("传真", 225, HorizontalAlignment.Right);
            listview.Columns.Add("秘书", 160, HorizontalAlignment.Left);
            listview.Columns.Add("秘书普机", 160, HorizontalAlignment.Right);
            listview.Columns.Add("秘书手机", 165, HorizontalAlignment.Right);
            listview.Columns.Add("秘书传真", 225, HorizontalAlignment.Right);
            listview.Columns.Add("办公地址", 375, HorizontalAlignment.Left);
            listview.Columns.Add("住宿地址", 400, HorizontalAlignment.Left);
            listview.Columns.Add("秘书办公地址", 375, HorizontalAlignment.Left);
            listview.Columns.Add("秘书住宿地址", 400, HorizontalAlignment.Left);
            listview.Columns.Add("备注1", 380, HorizontalAlignment.Left);
            listview.Columns.Add("备注2", 380, HorizontalAlignment.Left);

        }

        public void CreateTreeColums(ObjectListView listview)
        {
            listview.Columns.Add(new OLVColumn() { AspectName = "IdentityNo", Text = "序号", Width = 125, TextAlign = HorizontalAlignment.Right });
            listview.Columns.Add(new OLVColumn() { AspectName = "DepartmentName", Text = "单位", Width = 320, TextAlign = HorizontalAlignment.Left });
            listview.Columns.Add(new OLVColumn() { AspectName = "AName", Text = "姓名", Width = 130, TextAlign = HorizontalAlignment.Left });
            listview.Columns.Add(new OLVColumn() { AspectName = "Duty", Text = "职务", Width = 150, TextAlign = HorizontalAlignment.Left });
            listview.Columns.Add(new OLVColumn() { AspectName = "OfficeRedPhone", Text = "办公红机", Width = 120, TextAlign = HorizontalAlignment.Right });
            listview.Columns.Add(new OLVColumn() { AspectName = "DormitoryRedPhone", Text = "住宿红机", Width = 120, TextAlign = HorizontalAlignment.Right });
            listview.Columns.Add(new OLVColumn() { AspectName = "SecretaryRedPhone", Text = "秘书红机", Width = 120, TextAlign = HorizontalAlignment.Right });
            listview.Columns.Add(new OLVColumn() { AspectName = "Phone", Text = "办公普机", Width = 185, TextAlign = HorizontalAlignment.Right });
            listview.Columns.Add(new OLVColumn() { AspectName = "DormitoryPhone", Text = "住宿普机", Width = 225, TextAlign = HorizontalAlignment.Right });
            listview.Columns.Add(new OLVColumn() { AspectName = "AnyCall", Text = "手机", Width = 215, TextAlign = HorizontalAlignment.Right });
            listview.Columns.Add(new OLVColumn() { AspectName = "Fax", Text = "传真", Width = 225, TextAlign = HorizontalAlignment.Right });
            listview.Columns.Add(new OLVColumn() { AspectName = "Secretary", Text = "秘书", Width = 160, TextAlign = HorizontalAlignment.Left });
            listview.Columns.Add(new OLVColumn() { AspectName = "SecretaryDormitoryRedPhone", Text = "秘书普机", Width = 160, TextAlign = HorizontalAlignment.Right });
            listview.Columns.Add(new OLVColumn() { AspectName = "SecretaryAnyCall", Text = "秘书手机", Width = 165, TextAlign = HorizontalAlignment.Right });
            listview.Columns.Add(new OLVColumn() { AspectName = "SecretaryFax", Text = "秘书传真", Width = 225, TextAlign = HorizontalAlignment.Right });
            listview.Columns.Add(new OLVColumn() { AspectName = "OfficeAddress", Text = "办公地址", Width = 375, TextAlign = HorizontalAlignment.Left });
            listview.Columns.Add(new OLVColumn() { AspectName = "DormitoryAddress", Text = "住宿地址", Width = 400, TextAlign = HorizontalAlignment.Left });
            listview.Columns.Add(new OLVColumn() { AspectName = "SecretaryAddress", Text = "秘书办公地址", Width = 375, TextAlign = HorizontalAlignment.Left });
            listview.Columns.Add(new OLVColumn() { AspectName = "SecretaryDormitoryAddress", Text = "秘书住宿地址", Width = 400, TextAlign = HorizontalAlignment.Left });
            listview.Columns.Add(new OLVColumn() { AspectName = "Mark1", Text = "备注1", Width = 380, TextAlign = HorizontalAlignment.Left });
            listview.Columns.Add(new OLVColumn() { AspectName = "Mark2", Text = "备注2", Width = 380, TextAlign = HorizontalAlignment.Left });
        }

        public void SetUserPerimission()
        {
            string str = CacheStrategy.Instance.GetObject(CacheKey.Role).ToString();
            switch (str)
            {
                case "超级管理员":
                    this.区号查询ToolStripMenuItem.Visible = true;
                    this.数据备份ToolStripMenuItem.Visible = true;
                    this.换班ToolStripMenuItem.Visible = true;
                    this.更换密码ToolStripMenuItem.Visible = true;
                    this.退出ToolStripMenuItem.Visible = true;
                    this.单位管理ToolStripMenuItem.Visible = true;
                    this.操作员管理toolStripMenuItem.Visible = true;
                    break;
                case "编辑操作员":
                    this.区号查询ToolStripMenuItem.Visible = true;
                    this.数据备份ToolStripMenuItem.Visible = true;
                    this.换班ToolStripMenuItem.Visible = true;
                    this.更换密码ToolStripMenuItem.Visible = true;
                    this.退出ToolStripMenuItem.Visible = true;
                    this.单位管理ToolStripMenuItem.Visible = true;
                    this.操作员管理toolStripMenuItem.Visible = false;
                    break;
                case "查询操作员":
                    this.区号查询ToolStripMenuItem.Visible = true;
                    this.数据备份ToolStripMenuItem.Visible = true;
                    this.换班ToolStripMenuItem.Visible = true;
                    this.更换密码ToolStripMenuItem.Visible = true;
                    this.退出ToolStripMenuItem.Visible = true;
                    this.单位管理ToolStripMenuItem.Visible = false;
                    this.操作员管理toolStripMenuItem.Visible = false;
                    break;
            }
        }

        Code.Common.Iterator<ToolStripButton> iteratorss;
        private void fmMain_Load(object sender, EventArgs e)
        {
            GetItemsSource();
            List<ToolStripButton> listArea = new List<ToolStripButton>();

            for (int i = 0; i < toolStrip1.Items.Count; i++)
            {
                if (toolStrip1.Items[i] is ToolStripButton)
                {

                    listArea.Add(toolStrip1.Items[i] as ToolStripButton);
                }
            }
            iteratorss = new ConcreteIterator<ToolStripButton>(listArea);


            if (CacheStrategy.Instance.GetObject(Code.Common.CacheKey.UserName) == null)
            {
                Login fm = new Login();
                fm.StartPosition = FormStartPosition.CenterScreen;
                DialogResult dr = fm.ShowDialog();
                if (dr == System.Windows.Forms.DialogResult.OK)
                {
                    toolStripStatusLabel1.Text = "登录成功";
                    toolStripStatusLabel2.Text = "操作员：" + CacheStrategy.Instance.GetObject(CacheKey.RealName).ToString() + "[" + CacheStrategy.Instance.GetObject(CacheKey.Role) + "]";
                    SetUserPerimission();
                }
                else if (dr == System.Windows.Forms.DialogResult.Cancel)
                {
                    return;
                }
            }
            fmlead.OnGetItemsSource += new Frm.fmLead.GetItemsSource(() =>
            {
                return ItemsSource;
            });

            fmlead.OnQueryListHandler += new Frm.fmLead.QueryList((LeadId) =>
            {
                if (fmQQ != null)
                {
                    fmQQ.ReLoadList();
                }
                GetItemsSource();
                if (Config.QueryMode == 1)
                {
                    var m = ListViewItemsSource.FirstOrDefault(x => x.SubItems[x.SubItems.Count - 1].Text == LeadId.ToString());
                    if (m == null)
                    {
                        CurrentCacheItemsSource.RemoveAll(x => x.SubItems[x.SubItems.Count - 1].Text == LeadId.ToString());
                        LoadListViewItems(CurrentCacheItemsSource);
                        //objectListView1.VirtualListSize = CurrentCacheItemsSource.Count;
                        //objectListView1.Refresh();
                    }
                    else
                    {
                        var index = CurrentCacheItemsSource.FindIndex(x => x.SubItems[x.SubItems.Count - 1].Text == LeadId.ToString());
                        if (index >= 0)
                        {
                            m.SubItems[0].Text = (index + 1).ToString();
                            CurrentCacheItemsSource[index] = m;
                            //LoadListViewItems(CurrentCacheItemsSource);
                            objectListView1.Refresh();
                        }
                        else
                        {
                            CurrentCacheItemsSource.Add(m);
                            LoadListViewItems(CurrentCacheItemsSource);
                            objectListView1.Refresh();
                        }
                    }
                }
                else
                {
                    刷新ToolStripSplitButton_ButtonClick(null, null);
                }

            });
            if (Config.ShowMode != 1)
            {
                toolStripTextBox1.ReadOnly = true;
                toolStripTextBox1.Enabled = false;
            }
            else
            {
                toolStripTextBox1.Enabled = true;
                toolStripTextBox1.ReadOnly = false;
            }
            if (Config.QueryMode == 0)
            {
                treeListView1.Visible = true;
                objectListView1.Visible = false;

            }
            else
            {
                treeListView1.Visible = false;
                objectListView1.Visible = true;
            }
        }

        #region 工具菜单

        private void 单位管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fmUnit fm = new fmUnit();
            fm.StartPosition = FormStartPosition.CenterScreen;
            fm.ShowDialog();
        }

        private void 操作员管理toolStripMenuItem_Click(object sender, EventArgs e)
        {
            Frm.fmOperator fm = new Frm.fmOperator();
            fm.StartPosition = FormStartPosition.CenterScreen;
            fm.ShowDialog();
        }

        private void 红机查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Frm.fmRedQuery fm = new Frm.fmRedQuery();
            fm.StartPosition = FormStartPosition.CenterScreen;
            fm.Show();
        }

        private void 区号查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Frm.fmArea fm = new Frm.fmArea();
            fm.StartPosition = FormStartPosition.CenterScreen;
            fm.ShowDialog();
        }

        private void 数据备份ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "Access files(*.mdb)|*.mdb";
            saveFileDialog1.FilterIndex = 0;
            saveFileDialog1.RestoreDirectory = true; //保存对话框是否记忆上次打开的目录 
            saveFileDialog1.OverwritePrompt = false;
            //saveFileDialog.CreatePrompt = true;
            saveFileDialog1.Title = "数据备份";
            DateTime now = DateTime.Now;
            string str = Config.DB.Replace(".mdf", "");
            saveFileDialog1.FileName = str + "[" + now.Year.ToString().PadLeft(2) + now.Month.ToString().PadLeft(2, '0') + now.Day.ToString().PadLeft(2, '0') + "]";

            //点了保存按钮进入 
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (saveFileDialog1.FileName.Trim() == "")
                {
                    MessageBox.Show("请输入要保存的文件名", "提示");
                    return;
                }
                string sourcePath = System.Environment.CurrentDirectory + "\\" + Config.DB;
                string targetPath = saveFileDialog1.FileName.ToString();
                System.IO.File.Copy(sourcePath, targetPath, true);
            }
        }

        private void 系统设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Frm.fmSetting fm = new Frm.fmSetting();
            fm.ShowModeChangeHandler += new Frm.fmSetting.ShowModeChange(() =>
            {
                if (fmQQ == null)
                {
                    fmQQ = new Frm.fmQuickQuery();
                }
            });
            fm.StartPosition = FormStartPosition.CenterScreen;
            if (fm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (Config.ShowMode != 1)
                {
                    toolStripTextBox1.ReadOnly = true;
                    toolStripTextBox1.Enabled = false;
                }
                else
                {
                    toolStripTextBox1.Enabled = true;
                    toolStripTextBox1.ReadOnly = false;
                }
            }
        }

        #endregion

        #region 其他菜单
        private void 查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Frm.fmQuery fm = new Frm.fmQuery();
            fm.StartPosition = FormStartPosition.CenterScreen;
            fm.OnQueryListHandler += new Frm.fmQuery.QueryList(new Action<string, string, string, string>((quName, qname, qduty, qredPhone) =>
            {
                unitName = quName;
                redPhone = qredPhone;
                name = qname;
                duty = qduty;
                Query(unitName, redPhone, name, duty);
            }));

            fm.ShowDialog();
        }

        private void 编辑ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedId = GetSelecedItemID();
            if (selectedId != 0)
            {

                fmlead.InitInfo(selectedId, 2);
                fmlead.StartPosition = FormStartPosition.CenterScreen;
                fmlead.ShowDialog();
            }
            else
            {
                fmlead.InitInfo(0, 0);
                fmlead.StartPosition = FormStartPosition.CenterScreen;
                fmlead.ShowDialog();
            }

        }

        private void 疑问ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Frm.fmArea fm = new Frm.fmArea();
            fm.StartPosition = FormStartPosition.CenterScreen;
            fm.ShowDialog();
        }
        #endregion

        #region 系统菜单

        private void 换班ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Login fm = new Login("换班");
            fm.StartPosition = FormStartPosition.CenterScreen;
            if (fm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                toolStripStatusLabel1.Text = "登录成功";
                toolStripStatusLabel2.Text = "操作员：" + CacheStrategy.Instance.GetObject(CacheKey.RealName).ToString() + "[" + CacheStrategy.Instance.GetObject(CacheKey.Role) + "]";
                SetUserPerimission();
            }
        }

        private void 更换密码ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Frm.fmChangePwd fm = new Frm.fmChangePwd();
            fm.StartPosition = FormStartPosition.CenterScreen;
            fm.ShowDialog();
        }

        private void 打印ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 打印预览VToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认退出?", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
            {
                Application.Exit();
            }
        }

        #endregion

        #region ListView 事件


        private void ListView_DoubleClick(object sender, EventArgs e)
        {
            int selectedId = GetSelecedItemID();
            if (selectedId != 0)
            {
                fmTip.HideGlass();
                fmlead.InitInfo(selectedId, 2);
                fmlead.StartPosition = FormStartPosition.CenterScreen;
                fmlead.ShowDialog();

            }
        }

        bool IsEsc = false;
        private void ListView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                IsEsc = true;
            }
            else
            {
                IsEsc = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                if (iteratorss.MoveNext())
                {
                    AreaToolStripButton_Click(iteratorss.Next(), null);
                }
                else
                {
                    AreaToolStripButton_Click(iteratorss.First(), null);
                }
            }
            if (e.KeyCode == Keys.Left)
            {
                if (iteratorss.MoveBefore())
                {
                    AreaToolStripButton_Click(iteratorss.Before(), null);
                }
                else
                {

                    AreaToolStripButton_Click(iteratorss.Last(), null);
                }
            }
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                int selectedId = GetSelecedItemID();
                if (selectedId != 0)
                {
                    fmTip.HideGlass();
                    fmlead.InitInfo(selectedId, 2);
                    fmlead.StartPosition = FormStartPosition.CenterScreen;
                    fmlead.ShowDialog();

                }
            }



        }

        private void ListView_FormatCell(object sender, FormatCellEventArgs e)
        {
            e.Item.SubItems[2].ForeColor = Color.FromArgb(128, 0, 54);
            if (e.Item.SubItems[6].Text.Contains('*'))
            {
                e.Item.SubItems[6].ForeColor = Color.FromArgb(0, 255, 0);
            }
            else
            {
                e.Item.SubItems[6].ForeColor = Color.FromArgb(38, 38, 248);
            }
            if (e.Item.SubItems[4].Text.Contains('*'))
            {
                e.Item.SubItems[4].ForeColor = Color.FromArgb(0, 255, 0);
            }
            else
            {
                e.Item.SubItems[4].ForeColor = Color.FromArgb(38, 38, 248);
            }
            if (e.Item.SubItems[5].Text.Contains('*'))
            {
                e.Item.SubItems[5].ForeColor = Color.FromArgb(0, 255, 0);
            }
            else
            {
                e.Item.SubItems[5].ForeColor = Color.FromArgb(38, 38, 248);
            }

        }

        private void ListView_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (IsEsc)
            {
                fmTip.HideGlass();
                return;
            }
            if (Config.ShowMode == 1)
            {
                toolStripTextBox1.Text = toolStripTextBox1.Text + e.KeyChar.ToString();
                toolStripTextBox1.Select(toolStripTextBox1.Text.Length, 0);
                toolStripTextBox1.Focus();
                return;
            }

            if (e.KeyChar.ToString() == "\r")
            {
                return;
            }
            if (!fmQQ.Visible)
            {
                Point p = new Point(this.Width - 20 - fmQQ.Width, this.Height - 30 - fmQQ.Height);
                fmTip.HideGlass();
                fmQQ.PointToScreen(p);
                fmQQ.Location = p;
                fmQQ.Show();
                fmQQ.TextAppend(e.KeyChar);
            }
        }

        private void ListView_FormatRow(object sender, FormatRowEventArgs e)
        {
            if (e.RowIndex % 2 == 0 || e.RowIndex == 0)
            {
                e.Item.BackColor = Color.FromArgb(201, 201, 201);
            }
        }

        private void ListView_Click(object sender, MouseEventArgs e)
        {
            var selecedIndex = objectListView1.SelectedIndices.Count > 0 ? objectListView1.SelectedIndices[0] : 0;
            objectListView1.SetSelectedIndex(selecedIndex);
            objectListView1.Refresh();

            var model = this.CurrentCacheItemsSource[selecedIndex].Tag as LeadModel;
            fmTip.ShowModel(model, e.X, e.Y);



            // fmTip.Show(this);
            //fmMain2.ShowWindow(fmTip.Handle, 8);
            //fmTip.Invalidate();
            //return Convert.ToInt32(this.CurrentCacheItemsSource[selecedIndex].SubItems[this.CurrentCacheItemsSource[selecedIndex].SubItems.Count - 1].Text);
        }

        void listView_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            if (this.CurrentCacheItemsSource == null || this.CurrentCacheItemsSource.Count == 0)
            {
                return;
            }
            if (e.ItemIndex < CurrentCacheItemsSource.Count)
            {


                //if (this.CurrentCacheItemsSource[e.ItemIndex].SubItems[6].Text.Contains('*'))
                //{
                //    this.CurrentCacheItemsSource[e.ItemIndex].SubItems[6].ForeColor = Color.FromArgb(0, 255, 0);
                //}
                //else
                //{
                //    this.CurrentCacheItemsSource[e.ItemIndex].SubItems[6].ForeColor = Color.FromArgb(38, 38, 248);
                //}
                //if (this.CurrentCacheItemsSource[e.ItemIndex].SubItems[4].Text.Contains('*'))
                //{
                //    this.CurrentCacheItemsSource[e.ItemIndex].SubItems[4].ForeColor = Color.FromArgb(0, 255, 0);
                //}
                //else
                //{
                //    this.CurrentCacheItemsSource[e.ItemIndex].SubItems[4].ForeColor = Color.FromArgb(38, 38, 248);
                //}
                //if (this.CurrentCacheItemsSource[e.ItemIndex].SubItems[5].Text.Contains('*'))
                //{
                //    this.CurrentCacheItemsSource[e.ItemIndex].SubItems[5].ForeColor = Color.FromArgb(0, 255, 0);
                //}
                //else
                //{
                //    this.CurrentCacheItemsSource[e.ItemIndex].SubItems[5].ForeColor = Color.FromArgb(38, 38, 248);
                //}
                //this.CurrentCacheItemsSource[e.ItemIndex].SubItems[0].ForeColor = Color.Black;
                //this.CurrentCacheItemsSource[e.ItemIndex].SubItems[1].ForeColor = Color.Black;
                //this.CurrentCacheItemsSource[e.ItemIndex].SubItems[2].ForeColor = Color.FromArgb(128, 0, 54);
                //this.CurrentCacheItemsSource[e.ItemIndex].SubItems[3].ForeColor = Color.Black;
                //// this.CurrentCacheItemsSource[e.ItemIndex].SubItems[4].ForeColor = Color.FromArgb(38, 38, 248);
                ////this.CurrentCacheItemsSource[e.ItemIndex].SubItems[5].ForeColor = Color.FromArgb(38, 38, 248);
                ////this.CurrentCacheItemsSource[e.ItemIndex].SubItems[6].ForeColor = Color.FromArgb(38, 38, 248);
                //this.CurrentCacheItemsSource[e.ItemIndex].SubItems[7].ForeColor = Color.Black;
                //this.CurrentCacheItemsSource[e.ItemIndex].SubItems[8].ForeColor = Color.Black;
                //this.CurrentCacheItemsSource[e.ItemIndex].SubItems[9].ForeColor = Color.Black;
                //this.CurrentCacheItemsSource[e.ItemIndex].SubItems[10].ForeColor = Color.Black;
                //this.CurrentCacheItemsSource[e.ItemIndex].SubItems[11].ForeColor = Color.Black;
                //this.CurrentCacheItemsSource[e.ItemIndex].SubItems[12].ForeColor = Color.Black;
                //this.CurrentCacheItemsSource[e.ItemIndex].SubItems[13].ForeColor = Color.Black;
                //this.CurrentCacheItemsSource[e.ItemIndex].SubItems[14].ForeColor = Color.Black;
                //this.CurrentCacheItemsSource[e.ItemIndex].SubItems[15].ForeColor = Color.Black;
                //this.CurrentCacheItemsSource[e.ItemIndex].SubItems[16].ForeColor = Color.Black;
                //this.CurrentCacheItemsSource[e.ItemIndex].SubItems[17].ForeColor = Color.Black;
                //this.CurrentCacheItemsSource[e.ItemIndex].SubItems[18].ForeColor = Color.Black;
                //this.CurrentCacheItemsSource[e.ItemIndex].SubItems[19].ForeColor = Color.Black;
                //this.CurrentCacheItemsSource[e.ItemIndex].SubItems[20].ForeColor = Color.Black;
                //}

                e.Item = this.CurrentCacheItemsSource[e.ItemIndex];


            }
            if (e.ItemIndex == this.CurrentCacheItemsSource.Count)
            {
                this.CurrentCacheItemsSource = null;
            }
        }


        #endregion

        string area = "";
        string unitName = "";
        string duty = "";
        string name = "";
        string redPhone = "";

        #region 地区菜单事件
        private void AreaToolStripButton_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripButton)
            {

                ToolStripButton t = sender as ToolStripButton;

                for (int i = 1; i < toolStrip1.Items.Count; i++)
                {
                    if (toolStrip1.Items[i] is ToolStripButton)
                    {
                        if (toolStrip1.Items[i].Text == t.Text)
                        {

                            iteratorss.SetCurrent(t);

                            t.Checked = true;
                            t.CheckState = CheckState.Checked;

                            t.BackgroundImage = QueryPlatform.Properties.Resources.地区背景;
                            //QueryPlatform.Properties.Resources.地区背景;
                            //System.Resources.ResourceManager manager = new System.Resources.ResourceManager("QueryPlatform.Properties.Resources", Assembly.GetExecutingAssembly());
                            //Object target = manager.GetObject(t.Text);//获取到指定的资源的值  
                            //t.BackgroundImage = target as Image;
                        }
                        else
                        {
                            ToolStripButton sd = toolStrip1.Items[i] as ToolStripButton;
                            sd.BackgroundImage = null;
                            sd.Checked = false;
                            //toolStrip1.Items[i].BackgroundImage = null;
                        }
                    }
                }

                area = t.Text;
                Query(area);


            }
        }

        private void 定位模式ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Config.QueryMode = 0;
            this.查询模式ToolStripMenuItem.Checked = false;
            this.查询模式ToolStripMenuItem.CheckState = CheckState.Unchecked;
            this.定位模式ToolStripMenuItem.Checked = true;
            this.定位模式ToolStripMenuItem.CheckState = CheckState.Checked;
            this.objectListView1.Visible = false;
            this.treeListView1.Visible = true;
            Query(area);
        }

        private void 查询模式ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Config.QueryMode = 1;
            this.查询模式ToolStripMenuItem.Checked = true;
            this.查询模式ToolStripMenuItem.CheckState = CheckState.Checked;
            this.定位模式ToolStripMenuItem.Checked = false;
            this.定位模式ToolStripMenuItem.CheckState = CheckState.Unchecked;
            this.objectListView1.Visible = true;
            this.treeListView1.Visible = false;
            Query(area);
        }

        private void 刷新ToolStripSplitButton_ButtonClick(object sender, EventArgs e)
        {
            GetItemsSource();
            if (LastQueryType == 1)
            {
                Query(area);
            }
            else if (LastQueryType == 2)
            {
                QueryByKey(toolStripTextBox1.Text.Trim());
            }
            else if (LastQueryType == 3)
            {
                Query(unitName, redPhone, name, duty);
            }
            else
            {

            }
        }

        private void toolStripTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Config.QueryMode = 1;
                this.查询模式ToolStripMenuItem.Checked = true;
                this.查询模式ToolStripMenuItem.CheckState = CheckState.Checked;
                this.定位模式ToolStripMenuItem.Checked = false;
                this.定位模式ToolStripMenuItem.CheckState = CheckState.Unchecked;
                this.objectListView1.Visible = true;
                this.treeListView1.Visible = false;

                Code.Services.LeadService service = new LeadService();
                string searchKey = toolStripTextBox1.Text.Trim();
                if (searchKey.StartsWith("#"))
                {
                    searchKey = searchKey.Substring(1, searchKey.Length - 1);
                    QueryListPattern(new QueryCondition() { UnitName = searchKey, UnitNamePinyin = searchKey });
                }
                else if (searchKey.StartsWith("$"))
                {
                    searchKey = searchKey.Substring(1, searchKey.Length - 1);
                    QueryListPattern(new QueryCondition() { Name = searchKey, NamePinyin = searchKey });

                }
                else if (searchKey.StartsWith("@"))
                {
                    searchKey = searchKey.Substring(1, searchKey.Length - 1);
                    QueryListPattern(new QueryCondition() { Duty = searchKey, DutyPinyin = searchKey });
                }
                else if (searchKey.StartsWith("."))
                {
                    searchKey = searchKey.Substring(1, searchKey.Length - 1);
                    QueryListPattern(new QueryCondition() { Phone = searchKey });
                }
                else if (searchKey.StartsWith("+"))
                {
                    searchKey = searchKey.Substring(1, searchKey.Length - 1);
                    QueryListPattern(new QueryCondition() { AnyCall = searchKey });
                }
                else if (searchKey.StartsWith("*"))
                {
                    searchKey = searchKey.Substring(1, searchKey.Length - 1);
                    QueryListPattern(new QueryCondition() { Address = searchKey });
                }
                else
                {
                    QueryByKey(toolStripTextBox1.Text.Trim());
                }
                for (int i = 1; i < toolStrip1.Items.Count; i++)
                {
                    if (toolStrip1.Items[i] is ToolStripButton)
                    {
                        ToolStripButton sd = toolStrip1.Items[i] as ToolStripButton;
                        sd.BackgroundImage = null;
                        sd.Checked = false;
                    }
                }
                this.toolStripTextBox1.Text = "";
            }
        }
        #endregion

        private void Query(string _area)
        {
            LastQueryType = 1;
            if (Config.QueryMode == 1)
            {
                #region 查询模式
                QueryCondition condition = new QueryCondition();
                condition.Area = _area;
                QueryListPattern(condition);
                if (CurrentCacheItemsSource.Count > 0)
                {
                    objectListView1.EnsureVisible(0);
                }
                #endregion
            }
            else
            {
                #region 定位模式
                this.treeListView1.Roots = TreeDataSourceList.Where(x => x.DepartmentName == area && x.Level == 1);
                this.treeListView1.Refresh();
                #endregion
            }
            this.toolStripStatusLabel1.Text = "查询完成";


        }

        private void QueryListPattern(QueryCondition condition)
        {
            IEnumerable<ListViewItem> queryResult;
            bool result = false;
            if (!string.IsNullOrWhiteSpace(condition.Area))
            {
                queryResult = ListViewItemsSource.Where(x => x.SubItems[x.SubItems.Count - 2].Text == condition.Area);
                CurrentCacheItemsSource = queryResult.ToList();
                result = true;
            }
            if (!string.IsNullOrWhiteSpace(condition.Duty) && !string.IsNullOrWhiteSpace(condition.DutyPinyin))
            {
                queryResult = ListViewItemsSource.Where(x => x.SubItems[3].Text.ToUpper() == condition.Duty || x.SubItems[x.SubItems.Count - 4].Text.ToUpper() == condition.DutyPinyin);
                CurrentCacheItemsSource = queryResult.ToList();
                result = true;
            }
            if (!string.IsNullOrWhiteSpace(condition.Name) && !string.IsNullOrWhiteSpace(condition.NamePinyin))
            {
                condition.Name = condition.Name.ToUpper();
                condition.NamePinyin = condition.NamePinyin.ToUpper();
                queryResult = ListViewItemsSource.Where(x => x.SubItems[2].Text.ToUpper() == condition.Name || x.SubItems[x.SubItems.Count - 5].Text.ToUpper() == condition.NamePinyin);
                CurrentCacheItemsSource = queryResult.ToList();
                result = true;
            }
            if (!string.IsNullOrWhiteSpace(condition.UnitName) && !string.IsNullOrWhiteSpace(condition.UnitNamePinyin))
            {
                condition.UnitName = condition.UnitName.ToUpper();
                condition.UnitNamePinyin = condition.UnitNamePinyin.ToUpper();
                queryResult = ListViewItemsSource.Where(x => x.SubItems[1].Text.ToUpper() == condition.UnitName || x.SubItems[x.SubItems.Count - 3].Text.ToUpper() == condition.UnitNamePinyin);
                CurrentCacheItemsSource = queryResult.ToList();
                result = true;
            }
            if (!string.IsNullOrWhiteSpace(condition.RedPhone))
            {
                condition.RedPhone = condition.RedPhone.ToUpper();
                queryResult = ListViewItemsSource.Where(x => x.SubItems[4].Text.ToUpper().Contains(condition.RedPhone) || x.SubItems[5].Text.ToUpper().Contains(condition.RedPhone) || x.SubItems[6].Text.ToUpper().Contains(condition.RedPhone));
                CurrentCacheItemsSource = queryResult.ToList();
                result = true;
            }
            if (!string.IsNullOrWhiteSpace(condition.Phone))
            {
                condition.Phone = condition.Phone.ToUpper();
                queryResult = ListViewItemsSource.Where(x => x.SubItems[7].Text.ToUpper().Contains(condition.Phone) || x.SubItems[8].Text.ToUpper().Contains(condition.Phone) || x.SubItems[12].Text.ToUpper().Contains(condition.Phone));
                CurrentCacheItemsSource = queryResult.ToList();
                result = true;
            }
            if (!string.IsNullOrWhiteSpace(condition.AnyCall))
            {
                condition.AnyCall = condition.AnyCall.ToUpper();
                queryResult = ListViewItemsSource.Where(x => x.SubItems[9].Text.ToUpper().Contains(condition.AnyCall) || x.SubItems[13].Text.ToUpper().Contains(condition.AnyCall));
                CurrentCacheItemsSource = queryResult.ToList();
                result = true;
            }
            if (!string.IsNullOrWhiteSpace(condition.Address))
            {
                condition.Address = condition.Address.ToUpper();
                queryResult = ListViewItemsSource.Where(x =>
                       x.SubItems[15].Text.ToUpper().Contains(condition.Address) || x.SubItems[16].Text.ToUpper().Contains(condition.Address)
                    || x.SubItems[17].Text.ToUpper().Contains(condition.Address) || x.SubItems[18].Text.ToUpper().Contains(condition.Address));
                //   x => x.SubItems[16].Text.ToUpper().Contains(condition.) || x.SubItems[17].Text.ToUpper().Contains(condition.Address));
                CurrentCacheItemsSource = queryResult.ToList();
                result = true;
            }
            if (result)
            {
                LoadListViewItems(CurrentCacheItemsSource);
            }
        }

        public IEnumerable GetChildLead(LeadModel model)
        {
            //TreeDataSourceList
            ArrayList children = new ArrayList();
            if (model.HasChild)
            {
                if (model.Level == 1)
                {
                    TreeDataSourceList.Where(x => x.Area == model.Area && x.Level == 2).OrderBy(x => x.ID).OrderBy(x => x.DepartmentSort).ToList().ForEach(x =>
                    {
                        if (TreeDataSourceList.Count(y => y.Area == x.Area && y.DepartmentID == x.ID && y.Level == 3) > 0)
                        {
                            x.HasChild = true;
                        }
                        else
                        {
                            x.HasChild = false;
                        }
                        children.Add(x);
                    });
                }
                else if (model.Level == 2)
                {
                    int index = 1;
                    TreeDataSourceList.Where(x => x.Area == model.Area && x.DepartmentID == model.ID && x.Level == 3).ToList().ForEach(x =>
                    {
                        x.HasChild = false;
                        x.IdentityNo = index.ToString();
                        children.Add(x);
                        index++;
                    });
                }
                else
                {

                }
            }
            return children;
        }

        private void QueryByKey(string key)
        {
            key = key.ToUpper();
            LastQueryType = 2;
            Config.QueryMode = 1;
            this.查询模式ToolStripMenuItem.Checked = true;
            this.查询模式ToolStripMenuItem.CheckState = CheckState.Checked;
            this.定位模式ToolStripMenuItem.Checked = false;
            this.定位模式ToolStripMenuItem.CheckState = CheckState.Unchecked;
            this.objectListView1.Visible = true;
            this.treeListView1.Visible = false;
            CurrentCacheItemsSource = ListViewItemsSource.Where(x => x.SubItems[1].Text.ToUpper().Contains(key) || x.SubItems[2].Text.ToUpper().Contains(key)
              || x.SubItems[4].Text.ToUpper().Contains(key) || x.SubItems[5].Text.ToUpper().Contains(key)
              || x.SubItems[6].Text.ToUpper().Contains(key) || x.SubItems[x.SubItems.Count - 2].Text.ToUpper().Contains(key) || x.SubItems[x.SubItems.Count - 3].Text.ToUpper().Contains(key)
              || x.SubItems[x.SubItems.Count - 5].Text.ToUpper().Contains(key)).ToList();
            LoadListViewItems(CurrentCacheItemsSource);
            if (CurrentCacheItemsSource.Count > 0)
            {
                objectListView1.EnsureVisible(0);
            }
        }

        private void Query(string unitName, string RedPhone, string name, string duty)
        {
            LastQueryType = 3;
            this.查询模式ToolStripMenuItem.Checked = true;
            this.查询模式ToolStripMenuItem.CheckState = CheckState.Checked;
            this.定位模式ToolStripMenuItem.Checked = false;
            this.定位模式ToolStripMenuItem.CheckState = CheckState.Unchecked;
            this.objectListView1.Visible = true;
            this.treeListView1.Visible = false;

            var queryResult = ListViewItemsSource.Where(x => 1 == 1);
            if (!string.IsNullOrWhiteSpace(unitName))
            {
                queryResult = queryResult.Where(x => x.SubItems[1].Text == unitName || x.SubItems[x.SubItems.Count - 3].Text == unitName);
            }
            if (!string.IsNullOrWhiteSpace(duty))
            {
                queryResult = queryResult.Where(x => x.SubItems[3].Text == duty || x.SubItems[x.SubItems.Count - 4].Text == duty);
            }
            if (!string.IsNullOrWhiteSpace(RedPhone))
            {
                queryResult = queryResult.Where(x => x.SubItems[4].Text == RedPhone || x.SubItems[5].Text == RedPhone || x.SubItems[6].Text == RedPhone);
            }
            if (!string.IsNullOrWhiteSpace(name))
            {
                queryResult = queryResult.Where(x => x.SubItems[2].Text == name || x.SubItems[x.SubItems.Count - 5].Text == name);
            }

            CurrentCacheItemsSource = queryResult.ToList();
            LoadListViewItems(CurrentCacheItemsSource);
            if (CurrentCacheItemsSource.Count > 0)
            {
                objectListView1.EnsureVisible(0);
            }

        }

        private int GetSelecedItemID()
        {
            object obj;
            if (Config.QueryMode == 1)
            {
                if (objectListView1.SelectedIndices.Count > 0)
                {
                    var selecedIndex = objectListView1.SelectedIndices.Count > 0 ? objectListView1.SelectedIndices[0] : 0;
                    return Convert.ToInt32(this.CurrentCacheItemsSource[selecedIndex].SubItems[this.CurrentCacheItemsSource[selecedIndex].SubItems.Count - 1].Text);
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                ListViewItem currentItem;
                currentItem = treeListView1.SelectedItem;// treeListView1.SelectedIndex;.Count > 0 ? treeListView1.SelectedItems[0] : null;
                if (currentItem == null)
                {
                    return 0;
                }
                obj = treeListView1.GetModelObject(currentItem.Index);
                var model = obj as LeadModel;
                if (model != null)
                {
                    if (model.Level == 3)
                    {
                        return model.ID;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }
            }

        }


        private void objectListView1_VisibleChanged(object sender, EventArgs e)
        {
            if (fmTip != null)
            {
                if (!objectListView1.Visible)
                {
                    fmTip.HideGlass();
                }
            }
        }


        protected override void OnClosing(CancelEventArgs e)
        {

            fmTip.HideGlass();
            FmCloseing fc = new FmCloseing(this);
            fc.StartPosition = FormStartPosition.CenterScreen;
            //MessageBox.Show("确认退出?", "系统提示", MessageBoxButtons.OKCancel);
            if (MessageBox.Show("确认退出?", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
            {
                TimeSpan ts = DateTime.Now - Code.Common.Config.LastBackDate;

                if (ts.Days > Code.Common.Config.Date)
                {
                    string sourcePath = System.Environment.CurrentDirectory + "\\" + Config.DB;
                    string str = Config.DB.Replace(".mdb", "");
                    DateTime now = DateTime.Now;
                    str = str + "[" + now.Year.ToString().PadLeft(2) + now.Month.ToString().PadLeft(2, '0') + now.Day.ToString().PadLeft(2, '0') + "]";
                    string targetPath = System.Environment.CurrentDirectory + "\\" + str + ".mdb";
                    System.IO.File.Copy(sourcePath, targetPath, true);
                    Code.Common.Config.LastBackDate = DateTime.Now;
                    Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    config.AppSettings.Settings["LastBackDate"].Value = DateTime.Now.ToString("yyyy-MM-dd");
                    config.Save();
                }

                Application.Exit();
            }
            else
            {
                e.Cancel = true;
            }
            base.OnClosing(e);
        }

        private void fmMain2_Move(object sender, EventArgs e)
        {
            fmTip.HideGlass();
        }

        private void fmMain2_Resize(object sender, EventArgs e)
        {
            fmTip.HideGlass();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(toolStripStatusLabel2.Text);
            //System.OperatingSystem osInfo = System.Environment.OSVersion;
            ////获取<a href="https://www.baidu.com/s?wd=%E6%93%8D%E4%BD%9C%E7%B3%BB%E7%BB%9F&tn=44039180_cpr&fenlei=mv6quAkxTZn0IZRqIHckPjm4nH00T1d9PHRLrH7-uHb1m1I9n1NB0AP8IA3qPjfsn1bkrjKxmLKz0ZNzUjdCIZwsrBtEXh9GuA7EQhF9pywdQhPEUiqkIyN1IA-EUBtkPWmLrjbYn1n4nWDsPjDvnHfL" target="_blank" class="baidu-highlight">操作系统</a>ID
            //System.PlatformID platformID = osInfo.Platform;
            ////获取主版本号
            //int versionMajor = osInfo.Version.Major;
            ////获取副版本号
            //int versionMinor = osInfo.Version.Minor;
        }
    }

    public class QueryCondition
    {

        public string Area { get; set; }
        public string UnitName { get; set; }
        public string UnitNamePinyin { get; set; }
        public string Duty { get; set; }
        public string DutyPinyin { get; set; }
        public string Name { get; set; }
        public string NamePinyin { get; set; }
        public string RedPhone { get; set; }
        public string Phone { get; set; }
        public string AnyCall { get; set; }
        public string Address { get; set; }
    }


}
