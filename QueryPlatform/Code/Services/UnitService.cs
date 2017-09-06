using QueryPlatform.Code.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;

namespace QueryPlatform.Code.Services
{
    public class UnitService
    {
        Common.AccessData dal = new Common.AccessData();
        public List<DepartmentModel> GetDepartmentList()
        {
            string sql = "select * from Department";

            List<DepartmentModel> list = new List<DepartmentModel>();
            DepartmentModel model = null;
            int i = 1;
            using (IDataReader dataReader = dal.ExecuteReader(sql, null))
            {
                while (dataReader.Read())
                {
                    model = ReaderBind(dataReader);

                    model.DictCode = Config.AreaList.FindIndex(x => x == model.Dict);
                    list.Add(model);

                }
            }
            list = list.OrderBy(x => x.ID).OrderBy(x => x.Code).OrderBy(x => x.DictCode).ToList();
            list.ForEach(x =>
            {
                x.IdentityNo = i;
                i++;
            });
            return list;
        }

        /// <summary>
        /// 对象实体绑定数据
        /// </summary>
        public DepartmentModel ReaderBind(IDataReader dataReader)
        {
            DepartmentModel model = new DepartmentModel();
            object ojb;
            ojb = dataReader["ID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ID = (int)ojb;
            }
            model.Department = dataReader["Department"].ToString();
            model.Dict = dataReader["Dict"].ToString();
            ojb = dataReader["Code"];
            if (ojb != null && ojb != DBNull.Value)
            {
                if (!string.IsNullOrWhiteSpace(ojb.ToString()))
                {
                    model.Code = Convert.ToInt32(ojb);
                }
                else
                {
                    model.Code = 0;
                }
            }
          
            model.Pinyin = dataReader["Pinyin"].ToString();
            return model;
        }


        public int AddUnit(string name, string pinyin, string area)
        {
            OleDbConnection con = new OleDbConnection(dal.strCon);
            try
            {


                con.Open();


                string sql = "insert into Department(Department,Dict,Code,Pinyin)values(@Department,@Dict,@Code,@Pinyin)";
                System.Data.OleDb.OleDbParameter[] parameters ={
                                                               new System.Data.OleDb.OleDbParameter("Department", System.Data.OleDb.OleDbType.VarChar,50){Value=name},
                                                               new System.Data.OleDb.OleDbParameter("Dict", System.Data.OleDb.OleDbType.VarChar,50){Value=area},
                                                                new System.Data.OleDb.OleDbParameter("Code", System.Data.OleDb.OleDbType.VarChar,50){Value=""},
                                                               new System.Data.OleDb.OleDbParameter("Pinyin",System.Data.OleDb.OleDbType.VarChar,50){Value=pinyin}
                                                               };
                OleDbCommand command = new OleDbCommand(sql, con);
                command.Parameters.AddRange(parameters);

                if (command.ExecuteNonQuery() > 0)
                {
                    command.CommandText = "select  @@IDENTITY";
                    int newId = (int)command.ExecuteScalar();
                    return newId;
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                return 0;
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                    con.Dispose();
                }
            }

        }

        public bool Update(int id, string name, string pinyin, string area)
        {
            try
            {
                string sql = "update Department set Department=@Department,Dict=@Dict,Pinyin=@Pinyin where ID=@ID";
                System.Data.OleDb.OleDbParameter[] parameters ={
                                                            new System.Data.OleDb.OleDbParameter("Department", System.Data.OleDb.OleDbType.VarChar,50){Value=name},
                                                               new System.Data.OleDb.OleDbParameter("Dict", System.Data.OleDb.OleDbType.VarChar,50){Value=area},
                                                               new System.Data.OleDb.OleDbParameter("Pinyin",System.Data.OleDb.OleDbType.VarChar,50){Value=pinyin},
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
            catch
            {
                return false;
            }
        }

        public bool UpdateSort(int id, string code)
        {
            try
            {
                string sql = "update Department set Code=@Code where ID=@ID";
                System.Data.OleDb.OleDbParameter[] parameters ={
                                                               new System.Data.OleDb.OleDbParameter("Code",code),
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
            catch
            {
                return false;
            }

        }

        public Result CheckHasUser(int id)
        {

            Result result = new Result();

            result.Status = ResultStatus.Success;
            try
            {
                string sql = "select count(0) from Lead where DepartmentID=@DepartmentID";
                System.Data.OleDb.OleDbParameter[] parameters ={
                                                              new System.Data.OleDb.OleDbParameter("ID",id)
                                                          };
                object obj = dal.ExecuteScalar(sql, parameters);
                if (obj != null && obj != DBNull.Value)
                {
                    int a = Convert.ToInt32(obj);
                    if (a > 0)
                    {
                        result.Status = ResultStatus.Failure;
                        result.Message = "该单位存在注册用户,无法删除";
                    }
                    else
                    {
                        result.Status = ResultStatus.Success;
                    }
                }
                else
                {
                    result.Status = ResultStatus.Success;
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Status = ResultStatus.Error;
                result.Message = ex.Message;
                return result;
            }


        }

        public bool Delete(int id)
        {
            try
            {
                string sql = "delete from Department where ID=@ID";
                System.Data.OleDb.OleDbParameter[] parameters ={
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
            catch
            {
                return false;
            }

        }


    }
}
