using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QueryPlatform.Code.Services
{
    public class LeadModel
    {
       
        private int _id;
        private string _Aname;
        private string _departmentname;       
        private int? _departmentid;
        private string _duty;
        private string _phone;
        private string _anycall;
        private string _officeredphone;
        private string _dormitoryredphone;
        private string _officeaddress;
        private string _dormitoryaddress;
        private int? _code;
        private string _namepinyin;
        private string _mark1;
        private string _mark2;
        private int? _importment;
        private string _dutypinyin;
        private string _secretary;
        private string _secretaryredphone;
        private string _secretarydormitoryredphone;
        private string _secretaryanycall;
        private string _secretaryfax;
        private string _secretarydormitoryaddress;
        private string _fax;
        private string _secretaryaddress;
        private string _cphone;
        private string _dormitoryphone;
        private int? _departmentSort;
        private int _AreaCode;

        public int AreaCode
        {
            get { return _AreaCode; }
            set { _AreaCode = value; }
        }

       
        
        /// <summary>
        /// 
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string AName
        {
            set { _Aname = value; }
            get { return _Aname; }
        }
        public string DepartmentName
        {
            get { return _departmentname; }
            set { _departmentname = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? DepartmentID
        {
            set { _departmentid = value; }
            get { return _departmentid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Duty
        {
            set { _duty = value; }
            get { return _duty; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Phone
        {
            set { _phone = value; }
            get { return _phone; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string AnyCall
        {
            set { _anycall = value; }
            get { return _anycall; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string OfficeRedPhone
        {
            set { _officeredphone = value; }
            get { return _officeredphone; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DormitoryRedPhone
        {
            set { _dormitoryredphone = value; }
            get { return _dormitoryredphone; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string OfficeAddress
        {
            set { _officeaddress = value; }
            get { return _officeaddress; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DormitoryAddress
        {
            set { _dormitoryaddress = value; }
            get { return _dormitoryaddress; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Code
        {
            set { _code = value; }
            get { return _code; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string NamePinyin
        {
            set { _namepinyin = value; }
            get { return _namepinyin; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Mark1
        {
            set { _mark1 = value; }
            get { return _mark1; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Mark2
        {
            set { _mark2 = value; }
            get { return _mark2; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Importment
        {
            set { _importment = value; }
            get { return _importment; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DutyPinyin
        {
            set { _dutypinyin = value; }
            get { return _dutypinyin; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Secretary
        {
            set { _secretary = value; }
            get { return _secretary; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SecretaryRedPhone
        {
            set { _secretaryredphone = value; }
            get { return _secretaryredphone; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SecretaryDormitoryRedPhone
        {
            set { _secretarydormitoryredphone = value; }
            get { return _secretarydormitoryredphone; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SecretaryAnyCall
        {
            set { _secretaryanycall = value; }
            get { return _secretaryanycall; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SecretaryFax
        {
            set { _secretaryfax = value; }
            get { return _secretaryfax; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SecretaryDormitoryAddress
        {
            set { _secretarydormitoryaddress = value; }
            get { return _secretarydormitoryaddress; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Fax
        {
            set { _fax = value; }
            get { return _fax; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SecretaryAddress
        {
            set { _secretaryaddress = value; }
            get { return _secretaryaddress; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CPhone
        {
            set { _cphone = value; }
            get { return _cphone; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DormitoryPhone
        {
            set { _dormitoryphone = value; }
            get { return _dormitoryphone; }
        }
        public int? DepartmentSort
        {
            get { return _departmentSort; }
            set { _departmentSort = value; }
        }
        

        private int _Level;
        private bool _HasChild = true;

        private string _identityNo = "";

        public string IdentityNo
        {
            get { return _identityNo; }
            set { _identityNo = value; }
        }


        private string _DepartmentPinyin;

        public string DepartmentPinyin
        {
            get { return _DepartmentPinyin; }
            set { _DepartmentPinyin = value; }
        }

        private string _Area;

        public string Area
        {
            get { return _Area; }
            set { _Area = value; }
        }

        public int Level
        {
            get { return _Level; }
            set { _Level = value; }
        }
        public bool HasChild
        {
            get
            {
                if (this.Level == 3)
                {
                    return false;
                }
                else
                {
                    return _HasChild;
                }
            }
            set { _HasChild = value; }
        }

        public IEnumerable GetChildLead(List<LeadModel> list)
        {
            ArrayList children = new ArrayList();
            if (this.HasChild)
            {
                if(this.Level==1)
                {
                    list.Where(x => x.Area == this.DepartmentName && x.Level == 2).ToList().ForEach(x =>
                    {
                        children.Add(x);
                    });
                }
                else if (this.Level == 2)
                {
                    list.Where(x => x.DepartmentID == this.ID && x.DepartmentName == this.DepartmentName && x.Level == 3).ToList().ForEach(x =>
                    {
                        children.Add(x);
                    });
                }
                else
                { 
                    
                }
            }
            return children;
        }


        public LeadModel CopyModel()
        {
            LeadModel model = new LeadModel();
   
            model.AnyCall = this.AnyCall;
            model.Area = this.Area;
            model.Code = this.Code;
            model.CPhone = this.CPhone;
            model.DepartmentID = this.DepartmentID;
            model.DepartmentName = this.DepartmentName;
            model.DormitoryAddress = this.DormitoryAddress;
            model.DormitoryPhone = this.DormitoryPhone;
            model.DormitoryRedPhone = this.DormitoryRedPhone;
            model.Duty = this.Duty;
            model.DutyPinyin = this.DutyPinyin;
            model.Fax = this.Fax;
            model.HasChild = this.HasChild;
            model.ID = this.ID;
            model.Importment = this.Importment;
            model.Level = this.Level;
            model.Mark1 = this.Mark1;
            model.Mark2 = this.Mark2;
            model.AName = this.AName;
            model.NamePinyin = this.NamePinyin;
            model.OfficeAddress = this.OfficeAddress;
            model.OfficeRedPhone = this.OfficeRedPhone;
            model.Phone = this.Phone;
            model.Secretary = this.Secretary;
            model.SecretaryAddress = this.SecretaryAddress;
            model.SecretaryAnyCall = this.SecretaryAnyCall;
            model.SecretaryDormitoryAddress = this.SecretaryDormitoryAddress;
            model.SecretaryDormitoryRedPhone = this.SecretaryDormitoryRedPhone;
            model.SecretaryFax = this.SecretaryFax;
            model.SecretaryRedPhone = this.SecretaryRedPhone;
            model.IdentityNo = this.IdentityNo;
            model.DepartmentPinyin = this.DepartmentPinyin;
            model.DepartmentSort = this.DepartmentSort;
            model.AreaCode = this.AreaCode;


            return model;
        }



        public void ReplaceModel(LeadModel SourceModel)
        {
            this.AnyCall = SourceModel.AnyCall;
            this.Area = SourceModel.Area;
           // this.Code = SourceModel.Code;
            this.CPhone = SourceModel.CPhone;
            this.DepartmentID = SourceModel.DepartmentID;
            this.DepartmentName = SourceModel.DepartmentName;
            this.DormitoryAddress = SourceModel.DormitoryAddress;
            this.DormitoryPhone = SourceModel.DormitoryPhone;
            this.DormitoryRedPhone = SourceModel.DormitoryRedPhone;
            this.Duty = SourceModel.Duty;
            this.DutyPinyin = SourceModel.DutyPinyin;
            this.Fax = SourceModel.Fax;
            this.HasChild = SourceModel.HasChild;
            this.ID = SourceModel.ID;
            this.Importment = SourceModel.Importment;
            this.Level = SourceModel.Level;
            this.Mark1 = SourceModel.Mark1;
            this.Mark2 = SourceModel.Mark2;
            this.AName = SourceModel.AName;
            this.NamePinyin = SourceModel.NamePinyin;
            this.OfficeAddress = SourceModel.OfficeAddress;
            this.OfficeRedPhone = SourceModel.OfficeRedPhone;
            this.Phone = SourceModel.Phone;
            this.Secretary = SourceModel.Secretary;
            this.SecretaryAddress = SourceModel.SecretaryAddress;
            this.SecretaryAnyCall = SourceModel.SecretaryAnyCall;
            this.SecretaryDormitoryAddress = SourceModel.SecretaryDormitoryAddress;
            this.SecretaryDormitoryRedPhone = SourceModel.SecretaryDormitoryRedPhone;
            this.SecretaryFax = SourceModel.SecretaryFax;
            this.SecretaryRedPhone = SourceModel.SecretaryRedPhone;
            //  this.IdentityNo = SourceModel.IdentityNo;
            this.DepartmentPinyin = SourceModel.DepartmentPinyin;
          //  this.DepartmentSort = SourceModel.DepartmentSort;
            this.AreaCode = SourceModel.AreaCode;
        }

    }
}