using QueryPlatform.Code.Common;
using QueryPlatform.Code.Services;
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
    public partial class fmLead : Form
    {
        /// <summary>
        /// 新增：0
        /// 查询：1
        /// 更新：2
        /// </summary>
        int Status = 0;


        LeadModel CurrentModel = null;
        List<DepartmentModel> DepartmentSource = null;
        List<LeadModel> ItemsSource = null;

        public delegate void QueryList(int LeadId);//声明委托
        public event QueryList OnQueryListHandler = null;//定义委托事件

        public delegate List<LeadModel> GetItemsSource();
        public event GetItemsSource OnGetItemsSource = null;

        public fmLead()
        {
            InitializeComponent();
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            txtUpdateRecord.ReadOnly = true;


        }

        public void InitInfo(int id, int _status = 1)
        {
            Status = _status;
            Code.Services.LeadService service = new LeadService();
            CurrentModel = service.GetModel(id);

        }

        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
        {


            int WM_KEYDOWN = 256;

            int WM_SYSKEYDOWN = 260;

            if (msg.Msg == WM_KEYDOWN | msg.Msg == WM_SYSKEYDOWN)
            {
                switch (keyData)
                {
                    case Keys.Escape:
                        关闭ToolStripMenuItem_Click(null, null);
                        break;
                }
            }
            return false;
        }


        public void SetUserPerimission()
        {
            string str = CacheStrategy.Instance.GetObject(CacheKey.Role).ToString();
            switch (str)
            {
                case "超级管理员":
                case "编辑操作员":
                    this.保存ToolStripMenuItem.Visible = true;
                    this.删除ToolStripMenuItem.Visible = true;
                    break;
                case "查询操作员":
                    this.保存ToolStripMenuItem.Visible = false;
                    this.删除ToolStripMenuItem.Visible = false;
                    break;
            }
        }


        private void fmLead_Load(object sender, EventArgs e)
        {
            //保存ToolStripMenuItem.Enabled = false;
            Code.Services.UnitService service = new Code.Services.UnitService();
            DepartmentSource = service.GetDepartmentList();
            //DepartmentSource = DepartmentSource.OrderBy(x => x.ID).OrderBy(x => x.Code).ToList();
            SetBtnSave();
            SetUserPerimission();
            BindDist();
            BindUnit();
            if (OnGetItemsSource != null)
            {
                ItemsSource = OnGetItemsSource();
            }
            if (Status == 1)
            {
                查询ToolStripMenuItem_Click(null, null);
            }
            else if (Status == 2)
            {
                更新ToolStripMenuItem_Click(null, null);
            }
            else
            {
                新增ToolStripMenuItem_Click(null, null);
            }

            if (CurrentModel != null)
            {
                BindValue(CurrentModel);
            }
        }

        private void BindDist()
        {
            Code.Services.DictService service = new Code.Services.DictService();
            cbbArea.DropDownStyle = ComboBoxStyle.DropDownList;
            DataTable dt = service.GetAreaList();
            cbbArea.DataSource = dt;
            cbbArea.DisplayMember = "Name";
            cbbArea.ValueMember = "Name";
            cbbArea.SelectedIndex = -1;
        }


        private void BindUnit(string area = "")
        {

            cbbUnit.DropDownStyle = ComboBoxStyle.DropDown;
            if (area == "")
            {
                cbbUnit.DataSource = DepartmentSource;
            }
            else
            {

                var unit = DepartmentSource.Where(x => x.Dict == area).OrderBy(x => x.ID).OrderBy(x => x.Code).ToList();
                cbbUnit.DataSource = unit;
            }

            cbbUnit.DisplayMember = "Department";
            cbbUnit.ValueMember = "ID";
            cbbUnit.SelectedIndex = -1;

        }

        private void 查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedToolButton(this.查询ToolStripMenuItem);
            Status = 1;
            SetBtnSave();
            foreach (Control ctl in this.groupBox1.Controls)
            {
                if (ctl is TextBox && ctl.Name != "txtYHBGHJ")
                {
                    ctl.Text = string.Empty;
                    ctl.Enabled = false;
                }
            }
            cbbArea.Enabled = false;
            cbbUnit.Enabled = false;
            cbbArea.SelectedIndex = -1;
            cbbUnit.SelectedIndex = -1;
            cbbUnit.Text = "";



            txtYHBGHJ.Text = string.Empty;
        }

        private void 新增ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedToolButton(this.新增ToolStripMenuItem);
            Status = 0;
            SetBtnSave();
            foreach (Control ctl in this.groupBox1.Controls)
            {
                if (ctl is TextBox)
                {
                    if (ctl.Name == "txtUnitPY")
                    {
                        ctl.Enabled = true;
                    }
                    else
                    {
                        ctl.Text = string.Empty;
                        ctl.Enabled = true;
                    }
                }
            }
            // cbbArea.SelectedIndex = -1;
            //  cbbUnit.SelectedIndex = -1;
            cbbArea.Enabled = true;
            cbbUnit.Enabled = true;
            //  cbbUnit.Text = "";
            CurrentModel = null;
        }

        private void 更新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedToolButton(this.更新ToolStripMenuItem);
            if (CurrentModel == null)
            {
                MessageBox.Show("当前记录为空,请选择记录!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Status = 2;
            SetBtnSave();
            foreach (Control ctl in this.groupBox1.Controls)
            {
                if (ctl is TextBox && ctl.Name != "txtYHBGHJ")
                {
                    ctl.Enabled = true;
                }
            }
            cbbArea.Enabled = true;
            cbbUnit.Enabled = true;

        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedToolButton(this.删除ToolStripMenuItem);
            if (CurrentModel == null)
            {
                MessageBox.Show("当前记录为空,请选择记录!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("您确定删除记录?", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
            {
                Code.Services.LeadService service = new Code.Services.LeadService();
                if (service.DelLead(CurrentModel.ID))
                {
                    MessageBox.Show("删除成功!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    查询ToolStripMenuItem_Click(null, null);
                    if (OnQueryListHandler != null)
                    {
                        OnQueryListHandler(CurrentModel.ID);
                    }

                }
                else
                {
                    MessageBox.Show("删除失败!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedToolButton(this.保存ToolStripMenuItem);
            Code.Services.LeadService service = new Code.Services.LeadService();
            string name = txtYHXM.Text.Trim();
            string msbgdz = txtMSBGDZ.Text.Trim();
            string mscz = txtMSCZ.Text.Trim();
            string mshj = txtMSHJ.Text.Trim();
            string msxm = txtMSName.Text.Trim();
            string mspj = txtMSPJ.Text.Trim();
            string mssj = txtMSSJ.Text.Trim();
            string mszzdz = txtMSZZDZ.Text.Trim();
            string mark1 = txtRemark.Text.Trim();
            string mark2 = txtUpdateRecord.Text.Trim();
            string unitpy = txtUnitPY.Text.Trim();
            string yhbgdz = txtYHBGDZ.Text.Trim();
            string yhbghj = txtYHBGHJ.Text.Trim();
            string yhbgpj = txtYHBGPJ.Text.Trim();
            string yhcz = txtYHCZ.Text.Trim();
            string yhsj = txtYHSJ.Text.Trim();
            string yhssdz = txtYHSSDZ.Text.Trim();
            string yhxmjp = txtYHXMJP.Text.Trim();
            string yhzshj = txtYHZSHJ.Text.Trim();
            string yhzspj = txtYHZSPJ.Text.Trim();
            string yhzw = txtZW.Text.Trim();
            string area = cbbArea.SelectedIndex == -1 ? "" : cbbArea.SelectedValue.ToString();
            string unitID = cbbUnit.SelectedIndex == -1 ? "0" : cbbUnit.SelectedValue.ToString();

              
            if (yhbghj != "")
            {
                Result r = service.CheckRedPhoneIsExists(yhbghj);
                if (r.Status != ResultStatus.Success)
                {
                    if (MessageBox.Show(r.Message + ",是否继续保存?", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                    {
                        this.txtYHBGHJ.Text = "";
                        this.txtYHBGHJ.Focus();
                        return;
                    }
                }
            }
            if (unitID == "0" && cbbUnit.Text.Trim() == "")
            {
                MessageBox.Show("请选择单位或者输入新的单位名称", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (unitID == "0" && cbbUnit.Text.Trim() != "")
            {
                Code.Services.UnitService uService = new UnitService();
                unitID = uService.AddUnit(cbbUnit.Text.Trim(), txtUnitPY.Text.Trim(), cbbArea.Text.Trim()).ToString();
            }

            int res = 0;
            if (Status == 0)
            {
                StringBuilder sb = new StringBuilder(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                sb.Append("  [" + CacheStrategy.Instance.GetObject(CacheKey.RealName) + "]");
                sb.Append("建立信息");
                mark2 = sb.ToString();
                res = service.AddLead(name, int.Parse(unitID), yhzw, yhbgdz, yhbgpj, yhsj, yhbghj, yhzshj, yhssdz, "0", yhxmjp, mark1, mark2, "0", "", msxm, mshj, mspj, mssj, mscz, mszzdz, yhcz, msbgdz, "", yhzspj);
                CurrentModel = service.GetModel(res);
                //CurrentModel.ID = res;
            }
            else if (Status == 2)
            {
                StringBuilder sb = new StringBuilder(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                sb.Append("  [" + CacheStrategy.Instance.GetObject(CacheKey.RealName) + "]");
                sb.Append("更新信息\r\n");
                //sb.Append(txtUnitPY.Text == currentRow["Pinyin"].ToString()? "" : "秘书传真：" + currentRow["SecretaryFax"].ToString() + " -> " + txtMSCZ.Text.Trim() + "\r\n");
                // sb.Append(txtUpdateRecord.Text == currentRow["Mark2"].ToString()? "" : "秘书传真：" + currentRow["SecretaryFax"].ToString() + " -> " + txtMSCZ.Text.Trim() + "\r\n");
                //sb.Append(unitID == currentRow["DepartmentID"].ToString() ? "" : "单位：" + UnitName(currentRow["DepartmentID"].ToString()) + " -> " + cbbUnit.Text + "\r\n");

                //sb.Append(txtYHXM.Text == currentRow["Name"].ToString() ? "" : "姓名：" + currentRow["Name"].ToString() + " -> " + txtYHXM.Text.Trim() + "\r\n");
                //sb.Append(txtZW.Text == currentRow["Duty"].ToString() ? "" : "职务：" + currentRow["Duty"].ToString() + " -> " + txtZW.Text.Trim() + "\r\n");
                //sb.Append(txtYHXMJP.Text == currentRow["NamePinyin"].ToString() ? "" : "姓名简拼：" + currentRow["NamePinyin"].ToString() + " -> " + txtYHXMJP.Text.Trim() + "\r\n");
                //sb.Append(txtYHBGHJ.Text == currentRow["OfficeRedPhone"].ToString() ? "" : "办公红机：" + currentRow["OfficeRedPhone"].ToString() + " -> " + txtYHBGHJ.Text.Trim() + "\r\n");
                //sb.Append(txtYHZSHJ.Text == currentRow["DormitoryRedPhone"].ToString() ? "" : "住宿红机：" + currentRow["DormitoryRedPhone"].ToString() + " -> " + txtYHZSHJ.Text.Trim() + "\r\n");
                //sb.Append(txtYHBGPJ.Text == currentRow["Phone"].ToString() ? "" : "办公普机：" + currentRow["Phone"].ToString() + " -> " + txtYHBGPJ.Text.Trim() + "\r\n");
                //sb.Append(txtYHZSPJ.Text == currentRow["DormitoryPhone"].ToString() ? "" : "住宿普机：" + currentRow["DormitoryPhone"].ToString() + " -> " + txtYHZSPJ.Text.Trim() + "\r\n");
                //sb.Append(txtYHSJ.Text == currentRow["AnyCall"].ToString() ? "" : "手机：" + currentRow["AnyCall"].ToString() + " -> " + txtYHSJ.Text.Trim() + "\r\n");
                //sb.Append(txtYHCZ.Text == currentRow["FAX"].ToString() ? "" : "传真：" + currentRow["FAX"].ToString() + " -> " + txtYHCZ.Text.Trim() + "\r\n");
                //sb.Append(txtYHBGDZ.Text == currentRow["OfficeAddress"].ToString() ? "" : "办公地址：" + currentRow["OfficeAddress"].ToString() + " -> " + txtYHBGDZ.Text.Trim() + "\r\n");
                //sb.Append(txtYHSSDZ.Text == currentRow["DormitoryAddress"].ToString() ? "" : "宿舍地址：" + currentRow["DormitoryAddress"].ToString() + " -> " + txtYHSSDZ.Text.Trim() + "\r\n");
                //sb.Append(txtMSName.Text == currentRow["Secretary"].ToString() ? "" : "秘书姓名：" + currentRow["Secretary"].ToString() + " -> " + txtMSName.Text.Trim() + "\r\n");
                //sb.Append(txtMSHJ.Text == currentRow["SecretaryRedPhone"].ToString() ? "" : "秘书红机：" + currentRow["SecretaryRedPhone"].ToString() + " -> " + txtMSHJ.Text.Trim() + "\r\n");
                //sb.Append(txtMSPJ.Text == currentRow["SecretaryDormitoryRedPhone"].ToString() ? "" : "秘书普机：" + currentRow["SecretaryDormitoryRedPhone"].ToString() + " -> " + txtMSPJ.Text.Trim() + "\r\n");
                //sb.Append(txtMSSJ.Text == currentRow["SecretaryAnyCall"].ToString() ? "" : "秘书手机：" + currentRow["SecretaryAnyCall"].ToString() + " -> " + txtMSSJ.Text.Trim() + "\r\n");
                //sb.Append(txtMSCZ.Text == currentRow["SecretaryFax"].ToString() ? "" : "秘书传真：" + currentRow["SecretaryFax"].ToString() + " -> " + txtMSCZ.Text.Trim() + "\r\n");
                //sb.Append(txtMSBGDZ.Text == currentRow["SecretaryAddress"].ToString() ? "" : "秘书办公地址：" + currentRow["SecretaryAddress"].ToString() + " -> " + txtMSBGDZ.Text.Trim() + "\r\n");
                //sb.Append(txtMSZZDZ.Text == currentRow["SecretaryDormitoryAddress"].ToString() ? "" : "秘书住宅地址：" + currentRow["SecretaryDormitoryAddress"].ToString() + " -> " + txtMSZZDZ.Text.Trim() + "\r\n");

                sb.Append(unitID == CurrentModel.DepartmentID.ToString() ? "" : "单位：" + CurrentModel.DepartmentName + " -> " + cbbUnit.Text + "\r\n");

                sb.Append(txtYHXM.Text == CurrentModel.AName ? "" : "姓名：" + CurrentModel.AName + " -> " + txtYHXM.Text.Trim() + "\r\n");
                sb.Append(txtZW.Text == CurrentModel.Duty ? "" : "职务：" + CurrentModel.Duty + " -> " + txtZW.Text.Trim() + "\r\n");
                sb.Append(txtYHXMJP.Text == CurrentModel.NamePinyin ? "" : "姓名简拼：" + CurrentModel.NamePinyin + " -> " + txtYHXMJP.Text.Trim() + "\r\n");
                sb.Append(txtYHBGHJ.Text == CurrentModel.OfficeRedPhone ? "" : "办公红机：" + CurrentModel.OfficeRedPhone + " -> " + txtYHBGHJ.Text.Trim() + "\r\n");
                sb.Append(txtYHZSHJ.Text == CurrentModel.DormitoryRedPhone ? "" : "住宿红机：" + CurrentModel.DormitoryRedPhone + " -> " + txtYHZSHJ.Text.Trim() + "\r\n");
                sb.Append(txtYHBGPJ.Text == CurrentModel.Phone ? "" : "办公普机：" + CurrentModel.Phone + " -> " + txtYHBGPJ.Text.Trim() + "\r\n");
                sb.Append(txtYHZSPJ.Text == CurrentModel.DormitoryPhone ? "" : "住宿普机：" + CurrentModel.DormitoryPhone + " -> " + txtYHZSPJ.Text.Trim() + "\r\n");
                sb.Append(txtYHSJ.Text == CurrentModel.AnyCall ? "" : "手机：" + CurrentModel.AnyCall + " -> " + txtYHSJ.Text.Trim() + "\r\n");
                sb.Append(txtYHCZ.Text == CurrentModel.Fax ? "" : "传真：" + CurrentModel.Fax + " -> " + txtYHCZ.Text.Trim() + "\r\n");
                sb.Append(txtYHBGDZ.Text == CurrentModel.OfficeAddress ? "" : "办公地址：" + CurrentModel.OfficeAddress + " -> " + txtYHBGDZ.Text.Trim() + "\r\n");
                sb.Append(txtYHSSDZ.Text == CurrentModel.DormitoryAddress ? "" : "宿舍地址：" + CurrentModel.DormitoryAddress + " -> " + txtYHSSDZ.Text.Trim() + "\r\n");
                sb.Append(txtMSName.Text == CurrentModel.Secretary ? "" : "秘书姓名：" + CurrentModel.Secretary + " -> " + txtMSName.Text.Trim() + "\r\n");
                sb.Append(txtMSHJ.Text == CurrentModel.SecretaryRedPhone ? "" : "秘书红机：" + CurrentModel.SecretaryRedPhone + " -> " + txtMSHJ.Text.Trim() + "\r\n");
                sb.Append(txtMSPJ.Text == CurrentModel.SecretaryDormitoryRedPhone ? "" : "秘书普机：" + CurrentModel.SecretaryDormitoryRedPhone + " -> " + txtMSPJ.Text.Trim() + "\r\n");
                sb.Append(txtMSSJ.Text == CurrentModel.SecretaryAnyCall ? "" : "秘书手机：" + CurrentModel.SecretaryAnyCall + " -> " + txtMSSJ.Text.Trim() + "\r\n");
                sb.Append(txtMSCZ.Text == CurrentModel.SecretaryFax ? "" : "秘书传真：" + CurrentModel.SecretaryFax + " -> " + txtMSCZ.Text.Trim() + "\r\n");
                sb.Append(txtMSBGDZ.Text == CurrentModel.SecretaryAddress ? "" : "秘书办公地址：" + CurrentModel.SecretaryAddress + " -> " + txtMSBGDZ.Text.Trim() + "\r\n");
                sb.Append(txtMSZZDZ.Text == CurrentModel.SecretaryDormitoryAddress ? "" : "秘书住宅地址：" + CurrentModel.SecretaryDormitoryAddress + " -> " + txtMSZZDZ.Text.Trim() + "\r\n");

                //sb.Append(txtRemark.Text == currentRow["Mark1"].ToString() ? "" : "备注：" + currentRow["Mark1"].ToString() + " -> " + txtRemark.Text.Trim() + "\r\n");


                mark2 = sb.ToString() + mark2;
                txtUpdateRecord.Text = mark2;
                int code = CurrentModel.Code == null ? 0 : (int)CurrentModel.Code;
                res = Convert.ToInt32(service.UpdateLead(CurrentModel.ID, name, int.Parse(unitID), yhzw, yhbgdz, yhbgpj, yhsj, yhbghj, yhzshj, yhssdz, code, yhxmjp, mark1, mark2, "0", "", msxm, mshj, mspj, mssj, mscz, mszzdz, yhcz, msbgdz, "", yhzspj));
            }
            else
            {

            }
            if (res > 0)
            {
                if (Status == 0)
                {
                    MessageBox.Show("保存成功,用户在最后一条记录,尚未排序!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("保存成功", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                if (OnQueryListHandler != null)
                {
                    OnQueryListHandler(CurrentModel.ID);
                }
            }
            else
            {
                MessageBox.Show("保存失败!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void 前一个ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedToolButton(this.前一个ToolStripMenuItem);
            if (CurrentModel == null)
            {
                MessageBox.Show("当前记录为空,请选择记录!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            Code.Common.Iterator<LeadModel> iteratorSS = new ConcreteIterator<LeadModel>(ItemsSource);
            var cModel = ItemsSource.FirstOrDefault(x => x.ID == CurrentModel.ID);
            if (cModel != null)
            {
                iteratorSS.SetCurrent(cModel);
                if (iteratorSS.MoveBefore())
                {
                    CurrentModel = iteratorSS.Before();
                    BindValue(CurrentModel);
                }
                else
                {
                    MessageBox.Show("已经到了最前的记录!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

        }

        private void 下一个ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedToolButton(this.下一个ToolStripMenuItem);
            if (CurrentModel == null)
            {
                MessageBox.Show("当前记录为空,请选择记录!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            Code.Common.Iterator<LeadModel> iteratorSS = new ConcreteIterator<LeadModel>(ItemsSource);
            var cModel = ItemsSource.FirstOrDefault(x => x.ID == CurrentModel.ID);
            if (cModel != null)
            {
                iteratorSS.SetCurrent(cModel);
                if (iteratorSS.MoveNext())
                {
                    CurrentModel = iteratorSS.Next();
                    BindValue(CurrentModel);
                }
                else
                {
                    MessageBox.Show("已经到了最后的记录!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

            }
        }

        private void 关闭ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtYHBGHJ_TextChanged(object sender, EventArgs e)
        {
            if (Status == 1 && txtYHBGHJ.Text.Trim() != string.Empty)
            {
                CurrentModel = ItemsSource.FirstOrDefault(x => x.OfficeRedPhone==txtYHBGHJ.Text);
                if (CurrentModel != null)
                {
                    BindValue(CurrentModel);
                }
                else {
                    foreach (Control ctl in this.groupBox1.Controls)
                    {
                        if (ctl is TextBox && ctl.Name != "txtYHBGHJ")
                        {
                            ctl.Text = string.Empty;
                            ctl.Enabled = false;
                        }
                    }
                    cbbArea.Enabled = false;
                    cbbUnit.Enabled = false;
                    cbbArea.SelectedIndex = -1;
                    cbbUnit.SelectedIndex = -1;
                    cbbUnit.Text = "";
                }
            }
        }

        private void BindValue(LeadModel model)
        {
            cbbArea.SelectedIndexChanged -= new EventHandler(cbbArea_SelectedIndexChanged);
            cbbUnit.SelectedIndexChanged -= new EventHandler(cbbUnit_SelectedIndexChanged);
            cbbArea.SelectedValue = model.Area;
            cbbUnit.SelectedValue = model.DepartmentID;

            txtMSBGDZ.Text = model.SecretaryAddress;
            txtMSCZ.Text = model.SecretaryFax;
            txtMSHJ.Text = model.SecretaryRedPhone;
            txtMSName.Text = model.Secretary;
            txtMSPJ.Text = model.SecretaryDormitoryRedPhone;
            txtMSSJ.Text = model.SecretaryAnyCall;
            txtMSZZDZ.Text = model.SecretaryDormitoryAddress;
            txtRemark.Text = model.Mark1;
            txtUnitPY.Text = model.DepartmentPinyin;
            txtUpdateRecord.Text = model.Mark2;
            txtYHBGDZ.Text = model.OfficeAddress;
            txtYHBGHJ.Text = model.OfficeRedPhone;
            txtYHBGPJ.Text = model.Phone;
            txtYHCZ.Text = model.Fax;
            txtYHSJ.Text = model.AnyCall;
            txtYHSSDZ.Text = model.DormitoryAddress;
            txtYHXM.Text = model.AName;
            txtYHXMJP.Text = model.NamePinyin;
            txtYHZSHJ.Text = model.DormitoryRedPhone;
            txtYHZSPJ.Text = model.DormitoryPhone;
            txtZW.Text = model.Duty;
            txtUnitPY.Text = model.DepartmentPinyin;
            lbID.Text = model.ID.ToString();
            cbbArea.SelectedIndexChanged += new EventHandler(cbbArea_SelectedIndexChanged);
            cbbUnit.SelectedIndexChanged += new EventHandler(cbbUnit_SelectedIndexChanged);
        }

        private void SetBtnSave()
        {
            if (Status == 0 || Status == 2)
            {
                保存ToolStripMenuItem.Enabled = true;
            }
            else
            {
                保存ToolStripMenuItem.Enabled = false;
            }
        }

        private void SelectedToolButton(ToolStripButton btn)
        {
            for (int i = 0; i < menuStrip1.Items.Count; i++)
            {
                if (menuStrip1.Items[i] is ToolStripButton)
                {
                    ToolStripButton t = menuStrip1.Items[i] as ToolStripButton;
                    if (t == btn)
                    {
                        t.Checked = true;
                        t.CheckState = CheckState.Checked;
                        t.BackgroundImage = QueryPlatform.Properties.Resources.地区背景;
                    }
                    else
                    {
                        t.BackgroundImage = null;
                        t.Checked = false;
                        t.CheckState = CheckState.Unchecked;
                    }
                }
            }
        }

        private string UnitName(string unitID)
        {
            Code.Services.LeadService service = new LeadService();
            return service.GetDepartmentName(unitID);
        }

        private void txtYHXM_TextChanged(object sender, EventArgs e)
        {
            string str = EcanConvertToCh.convertCh(txtYHXM.Text.Trim());
            txtYHXMJP.Text = str;
        }

        private void cbbUnit_TextChanged(object sender, EventArgs e)
        {
            var model = DepartmentSource.FirstOrDefault(x => x.Department == cbbUnit.Text.Trim());
            if (model == null)
            {
                string str = EcanConvertToCh.convertCh(cbbUnit.Text.Trim());
                txtUnitPY.Text = str;
            }
            else
            {
                txtUnitPY.Text = model.Pinyin;
            }
        }

        private void cbbUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbUnit.SelectedIndex == -1)
            {
                cbbArea.SelectedIndex = -1;
            }
            else
            {
                var departmentModel = DepartmentSource.FirstOrDefault(x => x.Department == cbbUnit.Text);

                if (departmentModel != null)
                {
                    if (!string.IsNullOrWhiteSpace(departmentModel.Dict))
                    {
                        if (cbbArea.Text != departmentModel.Dict)
                        {
                            cbbArea.SelectedIndexChanged -= new EventHandler(cbbArea_SelectedIndexChanged);
                            cbbArea.SelectedValue = departmentModel.Dict;
                        }
                    }
                    else
                    {
                        cbbArea.SelectedIndexChanged -= new EventHandler(cbbArea_SelectedIndexChanged);
                        cbbArea.SelectedIndex = -1;
                    }
                }
                cbbArea.SelectedIndexChanged += new EventHandler(cbbArea_SelectedIndexChanged);
            }
        }

        private void cbbArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbArea.SelectedIndex == -1 || cbbArea.Text == "System.Data.DataRowView")
            {
                cbbUnit.SelectedIndexChanged -= new EventHandler(cbbUnit_SelectedIndexChanged);
                BindUnit("");
                cbbUnit.SelectedIndexChanged += new EventHandler(cbbUnit_SelectedIndexChanged);

            }
            else
            {
                cbbUnit.SelectedIndexChanged -= new EventHandler(cbbUnit_SelectedIndexChanged);
                BindUnit(cbbArea.Text.Trim());
                cbbUnit.SelectedIndexChanged += new EventHandler(cbbUnit_SelectedIndexChanged);
            }
        }



    }
}
