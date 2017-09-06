using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QueryPlatform.Code.Services
{
    public class DepartmentModel
    {

        #region Model
        private int _id;
        private string _department;
        private string _dict;
        private int _code;
        private string _pinyin;
        private int _dictCode;
        private int _identityNo;



        public int IdentityNo
        {
            get { return _identityNo; }
            set { _identityNo = value; }
        }
        public int DictCode
        {
            get { return _dictCode; }
            set { _dictCode = value; }
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
        public string Department
        {
            set { _department = value; }
            get { return _department; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Dict
        {
            set { _dict = value; }
            get { return _dict; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Code
        {
            set { _code = value; }
            get { return _code; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Pinyin
        {
            set { _pinyin = value; }
            get { return _pinyin; }
        }
        #endregion Model
    }
}
