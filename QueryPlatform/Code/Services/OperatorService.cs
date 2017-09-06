using QueryPlatform.Code.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace QueryPlatform.Code.Services
{
    public class OperatorService
    {
        Common.AccessData dal = new Common.AccessData();
        public DataTable GetOperatorList()
        {
            string sql = "select * from Operator";
            DataSet ds = dal.ExecuteDataSet(sql);
            return ds.Tables[0];
        }

        public bool CheckUserName(string userName,int id)
        {
            
            string sql = "select * from Operator where Name=@Name";
            System.Data.OleDb.OleDbParameter[] parameters ={
                                                            
                                                              new System.Data.OleDb.OleDbParameter("Name",userName)
                                                           
                                                          };
            DataTable dt = dal.ExecuteDataTable(sql, parameters);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["ID"].ToString() == id.ToString())
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        public bool AddOperator(string userName, string password, string realName, string role, string phone, string id)
        {
            string sql = "insert into Operator(ID,Name,Password2,RealName,Role,Phone)values(@ID,@Name,@Password2,@RealName,@Role,@Phone)";
            System.Data.OleDb.OleDbParameter[] parameters ={
                                                              new System.Data.OleDb.OleDbParameter("ID",id),
                                                              new System.Data.OleDb.OleDbParameter("Name",userName),
                                                              new System.Data.OleDb.OleDbParameter("Password2",password),
                                                              new System.Data.OleDb.OleDbParameter("RealName",realName),
                                                              new System.Data.OleDb.OleDbParameter("Role",role),
                                                              new System.Data.OleDb.OleDbParameter("Phone",phone)
                                                          };
            if (dal.ExecuteNonQuery(sql, parameters) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }


        }

        public bool UpdateOperator(string userName, string password, string realName, string role, string phone, string id)
        {
            string sql = "update Operator set Name=@Name, RealName=@RealName,Role=@Role,Phone=@Phone where ID=@ID";
            //

            System.Data.OleDb.OleDbParameter[] parameters ={
                                                                new System.Data.OleDb.OleDbParameter("Name",userName),
                                                             
                                                             
                                                              //new System.Data.OleDb.OleDbParameter("Password2",password),
                                                               new System.Data.OleDb.OleDbParameter("RealName",realName),
                                                               new System.Data.OleDb.OleDbParameter("Role",role),
                                                              new System.Data.OleDb.OleDbParameter("Phone",phone),
                                                               new System.Data.OleDb.OleDbParameter("ID",id)
                                                          };
            if (dal.ExecuteNonQuery(sql, parameters) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Code.Common.Result ChangePassword(string userName, string oldPwd, string password)
        {
            Result result = new Result();
            string sql = "select count(0) from Operator where Name=@Name and Password2=@oldPwd";
            System.Data.OleDb.OleDbParameter[] parameters ={
                                                              new System.Data.OleDb.OleDbParameter("Name",userName),
                                                              new System.Data.OleDb.OleDbParameter("OldPWD",oldPwd)
                                                          };

            object obj = dal.ExecuteScalar(sql, parameters);
            if (obj != null && obj != DBNull.Value)
            {
                int count = Convert.ToInt32(obj);
                if (count > 0)
                {
                    string sql1 = "Update Operator set Password2=@Password2 where Name=@Name and Password2=@OldPWD";

                    System.Data.OleDb.OleDbParameter[] parameters1 ={
                                                                new System.Data.OleDb.OleDbParameter("Password2",password),
                                                              new System.Data.OleDb.OleDbParameter("Name",userName),
                                                              new System.Data.OleDb.OleDbParameter("OldPWD",oldPwd)
                                                          };
                    if (dal.ExecuteNonQuery(sql1, parameters1) > 0)
                    {
                        result.Status = ResultStatus.Success;
                    }
                    else
                    {
                        result.Status = ResultStatus.Failure;
                    }
                }
                else
                {
                    result.Status = ResultStatus.Failure;
                    result.Message = "旧密码输入错误";
                }
            }
            else
            {
                result.Status = ResultStatus.Failure;
                result.Message = "旧密码输入错误";
            }
            return result;
        }

        public DataRow GetRole(int id)
        {
            string sql = "select * from Operator where ID=@ID";
            System.Data.OleDb.OleDbParameter[] parameters ={
                                                              new System.Data.OleDb.OleDbParameter("ID",id)
                                                          };
            DataTable dt = dal.ExecuteDataTable(sql, parameters);
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0];
            }
            else
            {
                return null;
            }
        }

        public DataRow GetRole(string name)
        {
            string sql = "select * from Operator where Name=@Name";
            System.Data.OleDb.OleDbParameter[] parameters ={
                                                              new System.Data.OleDb.OleDbParameter("Name",name)
                                                          };
            DataTable dt = dal.ExecuteDataTable(sql, parameters);
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0];
            }
            else
            {
                return null;
            }
        }

        public bool DelOperator(string name)
        {
            string sql = "DELETE FROM Operator WHERE Name=@Name";
            System.Data.OleDb.OleDbParameter[] parameters ={
                                                              new System.Data.OleDb.OleDbParameter("Name",name)
                                                          };
            if (dal.ExecuteNonQuery(sql, parameters) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
