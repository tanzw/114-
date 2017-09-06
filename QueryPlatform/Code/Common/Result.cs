using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QueryPlatform.Code.Common
{
    public class Result
    {
        private ResultStatus _Status;
        private string _Message;
        private object _Value;
      
        public ResultStatus Status
        {
            get { return _Status; }
            set { _Status = value; }
        }
        public string Message
        {
            get { return _Message; }
            set { _Message = value; }
        }
        public object Value
        {
            get { return _Value; }
            set { _Value = value; }
        }
    }


    public enum ResultStatus
    {
        Success,
        Failure,
        Warning,
        Error,
    }
}
