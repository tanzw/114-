using QueryPlatform.Code.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace QueryPlatform.Code.Services
{
    public class LoginService
    {
        Common.AccessData dal = new Common.AccessData();
        public Result Login(string userName, string password)
        {
            string sql = "select * from Operator where Name=@Name and Password2=@Password";
            System.Data.OleDb.OleDbParameter[] parameters ={
                                                               new System.Data.OleDb.OleDbParameter("Name",userName),
                                                               new System.Data.OleDb.OleDbParameter("Password",password)
                                                          };
            DataTable dt = dal.ExecuteDataTable(sql, parameters);

            Result result = new Result();
            if (dt.Rows.Count > 0)
            {
                result.Status = ResultStatus.Success;
                result.Message = dt.Rows[0]["RealName"].ToString();
                result.Message += "|" + dt.Rows[0]["Role"].ToString();
            }
            else
            {
                result.Status = ResultStatus.Failure;
                result.Message = "帐号或者密码不正确";
            }



            return result;
        }
    }
}
