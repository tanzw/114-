using QueryPlatform.Code.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;

namespace QueryPlatform.Code.Services
{
    public class LeadService
    {

        Common.AccessData dal = new Common.AccessData();
        public DataSet GetLeadList(string unitName, string name, string duty, string redPhone)
        {
            string sql = "select l.*,d.Department,d.Dict from Lead as l inner join Department as d on l.DepartmentID=d.ID";
            sql += " where ";
            sql += " d.Department like  '%'+ @Department + '%'";
            sql += " and l.Name like '%'+ @Name + '%' ";
            sql += " and Duty like  '%'+ @Duty + '%' ";
            sql += " and ( OfficeRedPhone like '%'+ @RedPhone + '%' ";
            sql += " or DormitoryRedPhone like '%'+ @RedPhone + '%'";
            sql += " or SecretaryRedPhone like '%'+ @RedPhone + '%' )";
            sql += " Order by  l.Code asc,d.Code asc";
            System.Data.OleDb.OleDbParameter[] parameters ={
                                                              new System.Data.OleDb.OleDbParameter("Department",unitName),
                                                              new System.Data.OleDb.OleDbParameter("Name",name),
                                                              new System.Data.OleDb.OleDbParameter("Duty",duty),
                                                              new System.Data.OleDb.OleDbParameter("RedPhone",redPhone)
                                                              };
            DataSet ds = dal.ExecuteDataSet(sql, parameters);
            return ds;
        }

        public Result CheckRedPhoneIsExists(string redPhone)
        {
            string sql = "select * from Lead where ";
            sql += "   OfficeRedPhone like '%'+ @RedPhone + '%' ";
            sql += " or DormitoryRedPhone like '%'+ @RedPhone + '%'";
            sql += " or SecretaryRedPhone like '%'+ @RedPhone + '%'";
            System.Data.OleDb.OleDbParameter[] parameters ={
                                                              new System.Data.OleDb.OleDbParameter("RedPhone",redPhone)
                                                              };
            Result result = new Result();

            DataTable dt = dal.ExecuteDataTable(sql, parameters);
            if (dt.Rows.Count > 0)
            {
                result.Status = ResultStatus.Failure;
                result.Message = "您填写的红机号码和" + dt.Rows[0]["Name"].ToString() + "的";
                //XX（红机类型）红机号码有重复，是否需要保存？";
                if (dt.Rows[0]["OfficeRedPhone"].ToString() == "")
                {
                    result.Message += "办公红机有重复";
                }
                else if (dt.Rows[0]["DormitoryRedPhone"].ToString() == "")
                {
                    result.Message += "住宿红机有重复";
                }
                else
                {
                    result.Message += "秘书红机有重复";
                }
            }
            else
            {
                result.Status = ResultStatus.Success;
            }
            return result;
        }

        public DataSet GetLeadList(string key)
        {
            string sql = "select l.*,d.Department,d.Dict from Lead as l inner join Department as d on l.DepartmentID=d.ID";
            if (!string.IsNullOrWhiteSpace(key))
            {
                sql += " where ";
                sql += " d.Department like  '%'+ @Key + '%'";
                sql += " or l.Name like '%'+ @Key + '%' ";
                sql += " or Duty like  '%'+ @Key + '%' ";
                sql += " or OfficeRedPhone like '%'+ @Key + '%' ";
                sql += " or DormitoryRedPhone like '%'+ @Key + '%'";
                sql += " or SecretaryRedPhone like '%'+ @Key + '%'";
            }
            sql += " Order by  l.Code asc,d.Code asc";
            System.Data.OleDb.OleDbParameter[] parameters ={
                                                              new System.Data.OleDb.OleDbParameter("Key",key)
                                                              };
            DataSet ds = dal.ExecuteDataSet(sql, parameters);
            return ds;

        }

        public DataRow GetPreviousLead(int leadID, int code)
        {
            string sql = "select top 1 l.*,d.Department,d.Dict,d.Pinyin,d.ID from Lead as l inner join Department as d on l.DepartmentID=d.ID";
            sql += " where l.ID<@LeadID  or iif(IsNull( l.Code ), 0, l.Code )<@Code ";
            sql += " Order by l.ID asc, l.Code asc";
            System.Data.OleDb.OleDbParameter[] parameters ={
                                                              new System.Data.OleDb.OleDbParameter("LeadID",leadID),
                                                               new System.Data.OleDb.OleDbParameter("Code",code)
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

        public DataRow GetNextLead(int leadID, int code)
        {
            string sql = "select top 1 l.*,d.Department,d.Dict,d.Pinyin,d.ID from Lead as l inner join Department as d on l.DepartmentID=d.ID";
            sql += " where l.ID>@LeadID  or  iif(IsNull( l.Code ), 0, l.Code )>@Code ";
            sql += " Order by l.ID desc, l.Code desc";
            System.Data.OleDb.OleDbParameter[] parameters ={
                                                              new System.Data.OleDb.OleDbParameter("LeadID",leadID),
                                                               new System.Data.OleDb.OleDbParameter("Code",code)
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

        public DataRow GetRowByRedPhone(string redPhone)
        {
            string sql = "select l.*,d.Department,d.Dict,d.Pinyin,d.ID from Lead as l inner join Department as d on l.DepartmentID=d.ID";
            sql += " where ";
            sql += " l.OfficeRedPhone=@RedPhone";
            sql += " Order by  l.Code asc,d.Code asc";
            System.Data.OleDb.OleDbParameter[] parameters ={
                                                              new System.Data.OleDb.OleDbParameter("RedPhone",redPhone)
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

        public DataRow GetRowById(int leadID)
        {
            string sql = "select l.*,d.Department,d.Dict,d.Pinyin,d.ID from Lead as l inner join Department as d on l.DepartmentID=d.ID";
            sql += " where ";
            sql += " l.ID=@LeadID";
            sql += " Order by  l.Code asc,d.Code asc";
            System.Data.OleDb.OleDbParameter[] parameters ={
                                                              new System.Data.OleDb.OleDbParameter("LeadID",leadID)
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

        public DataSet GetLeadByDist(string area)
        {
            string sql = "select l.*,d.Department,d.Dict from Lead as l inner join Department as d on l.DepartmentID=d.ID";
            sql += " where ";
            sql += " d.Dict=@dict";
            sql += " Order by  l.Code asc,d.Code asc";
            System.Data.OleDb.OleDbParameter[] parameters ={
                                                              new System.Data.OleDb.OleDbParameter("dict",area)
                                                              };
            DataSet ds = dal.ExecuteDataSet(sql, parameters);
            return ds;

        }

        public LeadModel GetModel(int leadID)
        {
            string sql = "select l.*,d.Department,d.Dict,d.Code as DepartmentSort,d.Pinyin as DepartmentPinyin,   3 as LLevel from Lead as l inner join Department as d on l.DepartmentID=d.ID";
            sql += " where l.ID=@LeadID";
            System.Data.OleDb.OleDbParameter[] parameters ={
                                                              new System.Data.OleDb.OleDbParameter("LeadID",leadID)
                                                              };
            LeadModel model = null;
            using (IDataReader dataReader = dal.ExecuteReader(sql, parameters))
            {
                if (dataReader.Read())
                {
                    model = ReaderBind(dataReader);
                }
            }
            return model;
        }

        public List<LeadModel> GetAllLeadList()
        {
            List<LeadModel> list = new List<LeadModel>();
            //string sql = "select l.*,d.Department,d.Dict,3 as LLevel from Lead as l inner join Department as d on l.DepartmentID=d.ID";
            //sql += " Order by  l.Code asc,d.Code asc";

            string sql = "select l.*,d.Department,d.Dict,d.Code as DepartmentSort,d.Pinyin as DepartmentPinyin,   3 as LLevel from Lead as l inner join Department as d on l.DepartmentID=d.ID";
            sql += " Order by  l.Code asc,d.Code asc";

            LeadModel model = null;
            using (IDataReader dataReader = dal.ExecuteReader(sql, null))
            {
                while (dataReader.Read())
                {
                    model = ReaderBind(dataReader);
                    model.AreaCode = Config.AreaList.FindIndex(x => x == model.Area);
                    list.Add(model);
                }
            }
            return list;
        }

        public List<LeadModel> GetAllDepartment()
        {
            List<LeadModel> list = new List<LeadModel>();

            string sql = "select  d.Id as DepartmentID, d.Department,d.Dict,d.Code as DepartmentSort,d.Pinyin as DepartmentPinyin,   2 as LLevel from   Department as d ";

            using (IDataReader dataReader = dal.ExecuteReader(sql, null))
            {
                while (dataReader.Read())
                {
                    list.Add(ReaderBindDepartment(dataReader));
                }
            }
            return list;
        }

        public List<LeadModel> GetAllTreeLeadList()
        {
            List<LeadModel> list = GetAllLeadList();
            int index = 1;
            Config.AreaList.ForEach(x =>
            {
                list.Add(new LeadModel() { DepartmentName = x, Area = x, Level = 1 });
                index++;
            });
            List<LeadModel> listDepartment = GetAllDepartment();
            listDepartment.ForEach(x =>
            {
                list.Add(new LeadModel() { ID = x.ID, DepartmentID = x.DepartmentID, DepartmentPinyin = x.DepartmentPinyin, DepartmentName = x.DepartmentName, Code = x.DepartmentSort, DepartmentSort = x.DepartmentSort, Area = x.Area, Level = 2 });
            });

            return list;
        }

        public List<LeadModel> GetSearchLeadList(string unitName, string name, string duty, string redPhone)
        {
            List<OleDbParameter> parameters = new List<OleDbParameter>();
            string sql = "select l.*,d.Department,d.Dict,3 as LLevel from Lead as l inner join Department as d on l.DepartmentID=d.ID";
            sql += " where 1=1 ";
            if (!string.IsNullOrWhiteSpace(unitName))
            {
                sql += " and d.Department like  '%'+ @Department + '%'";
                parameters.Add(new System.Data.OleDb.OleDbParameter("Department", unitName));
            }
            if (!string.IsNullOrWhiteSpace(name))
            {
                sql += " and l.Name like '%'+ @Name + '%' ";
                parameters.Add(new System.Data.OleDb.OleDbParameter("Name", name));
            }
            if (!string.IsNullOrWhiteSpace(duty))
            {
                sql += " and Duty like  '%'+ @Duty + '%' ";
                parameters.Add(new System.Data.OleDb.OleDbParameter("Duty", duty));
            }
            if (!string.IsNullOrWhiteSpace(redPhone))
            {
                sql += " and ( OfficeRedPhone like '%'+ @RedPhone + '%' ";
                sql += " or DormitoryRedPhone like '%'+ @RedPhone + '%'";
                sql += " or SecretaryRedPhone like '%'+ @RedPhone + '%' )";
                parameters.Add(new System.Data.OleDb.OleDbParameter("RedPhone", redPhone));
            }
            sql += " Order by  l.Code asc,d.Code asc";

            List<LeadModel> list = new List<LeadModel>();
            using (IDataReader dataReader = dal.ExecuteReader(sql, parameters.ToArray()))
            {
                while (dataReader.Read())
                {
                    list.Add(ReaderBind(dataReader));
                }
            }
            return list;

        }

        public List<LeadModel> GetLeadByKey(string key)
        {
            string sql = "select l.*,d.Department,d.Dict,3 as LLevel from Lead as l inner join Department as d on l.DepartmentID=d.ID";
            if (!string.IsNullOrWhiteSpace(key))
            {
                sql += " where ";
                sql += " d.Department like  '%'+ @Key + '%'";
                sql += " or l.Name like '%'+ @Key + '%' ";
                sql += " or Duty like  '%'+ @Key + '%' ";
                sql += " or OfficeRedPhone like '%'+ @Key + '%' ";
                sql += " or DormitoryRedPhone like '%'+ @Key + '%'";
                sql += " or SecretaryRedPhone like '%'+ @Key + '%'";
            }
            sql += " Order by  l.Code asc,d.Code asc";
            System.Data.OleDb.OleDbParameter[] parameters ={
                                                              new System.Data.OleDb.OleDbParameter("Key",key)
                                                              };
            List<LeadModel> list = new List<LeadModel>();
            using (IDataReader dataReader = dal.ExecuteReader(sql, parameters))
            {
                while (dataReader.Read())
                {
                    list.Add(ReaderBind(dataReader));
                }
            }
            return list;
        }

        public List<LeadModel> GetTreeLeadList(string area)
        {
            List<LeadModel> list = new List<LeadModel>();
            if (area == "")
            {
                return list;
            }
            else
            {
                list.Add(new LeadModel() { DepartmentName = area, ID = 0, HasChild = true, Level = 1 });
            }

            string sql = "select l.*,d.Department,d.Dict,3 as LLevel from Lead as l inner join Department as d on l.DepartmentID=d.ID";
            sql += " where ";
            sql += " d.Dict=@dict";
            sql += " Order by  l.Code asc,d.Code asc";

            System.Data.OleDb.OleDbParameter[] parameters ={
                                                              new System.Data.OleDb.OleDbParameter("dict",area)
                                                              };


            using (IDataReader dataReader = dal.ExecuteReader(sql, parameters))
            {
                while (dataReader.Read())
                {
                    list.Add(ReaderBind(dataReader));
                }
            }

            string sql1 = "select 2 AS LLevel,Department,Dict, ID, '' as Name,0 as DepartmentID,'' as Duty,'' as Phone,'' as AnyCall,'' as OfficeRedPhone,'' as DormitoryRedPhone,'' as OfficeAddress,'' as DormitoryAddress,0 as Code,'' as NamePinyin,'' as Mark1,'' as Mark2,0 as Importment,'' as DutyPinyin,'' as Secretary,'' as SecretaryRedPhone,'' as SecretaryDormitoryRedPhone,'' as SecretaryAnyCall,'' as SecretaryFax,'' as SecretaryDormitoryAddress,'' as Fax,'' as SecretaryAddress,'' as CPhone,'' as DormitoryPhone from Department";
            sql1 += " Order by  l.Code asc,d.Code asc";
            using (IDataReader dataReader = dal.ExecuteReader(sql1, null))
            {
                while (dataReader.Read())
                {
                    list.Add(ReaderBind(dataReader));
                }
            }
            return list;
        }

        public string GetDepartmentName(string id)
        {
            string sql = "select * from Department where id=@Id";
            System.Data.OleDb.OleDbParameter[] parameters ={
                                                              new System.Data.OleDb.OleDbParameter("Id",id)
                                                              };
            DataTable dt = dal.ExecuteDataTable(sql, parameters);
            if (dt.Rows.Count == 1)
            {
                return dt.Rows[0]["Department"].ToString();
            }
            else
            {
                return "";
            }
            
        }

        public LeadModel ReaderBindDepartment(IDataReader dataReader)
        {
            LeadModel model = new LeadModel();
            object ojb;

            model.DepartmentName = dataReader["Department"].ToString();
            ojb = dataReader["DepartmentID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.DepartmentID = (int)ojb;
                model.ID = (int)ojb;
            }
            model.Level = (int)dataReader["LLevel"];
            model.Area = dataReader["Dict"].ToString();
            model.DepartmentPinyin = dataReader["DepartmentPinyin"].ToString();
            ojb = dataReader["DepartmentSort"];
            if (ojb != null && ojb != DBNull.Value)
            {
                int r = 0;
                int.TryParse(ojb.ToString(), out r);
                model.DepartmentSort = r;
            }
            return model;
        }

        /// <summary>
        /// 对象实体绑定数据
        /// </summary>
        public LeadModel ReaderBind(IDataReader dataReader)
        {
            LeadModel model = new LeadModel();
            object ojb;
            ojb = dataReader["ID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ID = (int)ojb;
            }
            model.AName = dataReader["Name"].ToString();
            model.DepartmentName = dataReader["Department"].ToString();
            ojb = dataReader["DepartmentID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.DepartmentID = (int)ojb;
            }
            model.Duty = dataReader["Duty"].ToString();
            model.Phone = dataReader["Phone"].ToString();
            model.AnyCall = dataReader["AnyCall"].ToString();
            model.OfficeRedPhone = dataReader["OfficeRedPhone"].ToString();
            model.DormitoryRedPhone = dataReader["DormitoryRedPhone"].ToString();
            model.OfficeAddress = dataReader["OfficeAddress"].ToString();
            model.DormitoryAddress = dataReader["DormitoryAddress"].ToString();
            ojb = dataReader["Code"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Code = (int)ojb;
                
            }
            model.NamePinyin = dataReader["NamePinyin"].ToString();
            model.Mark1 = dataReader["Mark1"].ToString();
            model.Mark2 = dataReader["Mark2"].ToString();
            ojb = dataReader["Importment"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Importment = (int)ojb;
            }
            model.DutyPinyin = dataReader["DutyPinyin"].ToString();
            model.Secretary = dataReader["Secretary"].ToString();
            model.SecretaryRedPhone = dataReader["SecretaryRedPhone"].ToString();
            model.SecretaryDormitoryRedPhone = dataReader["SecretaryDormitoryRedPhone"].ToString();
            model.SecretaryAnyCall = dataReader["SecretaryAnyCall"].ToString();
            model.SecretaryFax = dataReader["SecretaryFax"].ToString();
            model.SecretaryDormitoryAddress = dataReader["SecretaryDormitoryAddress"].ToString();
            model.Fax = dataReader["Fax"].ToString();
            model.SecretaryAddress = dataReader["SecretaryAddress"].ToString();
            model.CPhone = dataReader["CPhone"].ToString();
            model.DormitoryPhone = dataReader["DormitoryPhone"].ToString();
            model.Level = (int)dataReader["LLevel"];
            model.Area = dataReader["Dict"].ToString();
            model.DepartmentPinyin = dataReader["DepartmentPinyin"].ToString();
            ojb = dataReader["DepartmentSort"];
            if (ojb != null && ojb != DBNull.Value)
            {
                int r = 0;
                int.TryParse(ojb.ToString(), out r);
                model.DepartmentSort = r;
            }

            return model;
        }


        public int AddLead(LeadModel model)
        {
            OleDbConnection con = new OleDbConnection(dal.strCon);
            try
            {


                con.Open();


                string sql = "Insert into Lead(OfficeAddress,Name,DepartmentID,Duty,Phone,AnyCall,OfficeRedPhone,DormitoryRedPhone,DormitoryAddress,Code,NamePinyin,Mark1,Mark2,Importment,DutyPinyin,Secretary,SecretaryRedPhone,SecretaryDormitoryRedPhone,SecretaryAnyCall,SecretaryFax,SecretaryDormitoryAddress,Fax,SecretaryAddress,CPhone,DormitoryPhone)";
                sql += "values(@OfficeAddress,@Name,@DepartmentID,@Duty,@Phone,@AnyCall,@OfficeRedPhone,@DormitoryRedPhone,@DormitoryAddress,@Code,@NamePinyin,@Mark1,@Mark2,@Importment,@DutyPinyin,@Secretary,@SecretaryRedPhone,@SecretaryDormitoryRedPhone,@SecretaryAnyCall,@SecretaryFax,@SecretaryDormitoryAddress,@Fax,@SecretaryAddress,@CPhone,@DormitoryPhone)";

                System.Data.OleDb.OleDbParameter[] parameters ={
                                                              new System.Data.OleDb.OleDbParameter("OfficeAddress",model.OfficeAddress),
                                                              new System.Data.OleDb.OleDbParameter("Name",model.AName),
                                                              new System.Data.OleDb.OleDbParameter("DepartmentID",model.DepartmentID),
                                                                new System.Data.OleDb.OleDbParameter("Duty",model.Duty),
                                                              new System.Data.OleDb.OleDbParameter("Phone",model.Phone),
                                                              new System.Data.OleDb.OleDbParameter("AnyCall",model.AnyCall),
                                                              new System.Data.OleDb.OleDbParameter("OfficeRedPhone",model.OfficeRedPhone),
                                                              new System.Data.OleDb.OleDbParameter("DormitoryRedPhone",model.DormitoryRedPhone),
                                                              new System.Data.OleDb.OleDbParameter("DormitoryAddress",model.DormitoryAddress),
                                                              new System.Data.OleDb.OleDbParameter("Code",model.Code),
                                                              new System.Data.OleDb.OleDbParameter("NamePinyin",model.NamePinyin),
                                                              new System.Data.OleDb.OleDbParameter("Mark1",model.Mark1),
                                                              new System.Data.OleDb.OleDbParameter("Mark2",model.Mark2),
                                                              new System.Data.OleDb.OleDbParameter("Importment",model.Importment),
                                                              new System.Data.OleDb.OleDbParameter("DutyPinyin",model.DutyPinyin),
                                                              new System.Data.OleDb.OleDbParameter("Secretary",model.Secretary),
                                                              new System.Data.OleDb.OleDbParameter("SecretaryRedPhone",model.SecretaryRedPhone),
                                                              new System.Data.OleDb.OleDbParameter("SecretaryDormitoryRedPhone",model.SecretaryDormitoryRedPhone),
                                                              new System.Data.OleDb.OleDbParameter("SecretaryAnyCall",model.SecretaryAnyCall),
                                                              new System.Data.OleDb.OleDbParameter("SecretaryFax",model.SecretaryFax),
                                                              new System.Data.OleDb.OleDbParameter("SecretaryDormitoryAddress",model.SecretaryDormitoryAddress),
                                                              new System.Data.OleDb.OleDbParameter("Fax",model.Fax),
                                                              new System.Data.OleDb.OleDbParameter("SecretaryAddress",model.SecretaryAddress),
                                                              new System.Data.OleDb.OleDbParameter("CPhone",model.CPhone),
                                                              new System.Data.OleDb.OleDbParameter("DormitoryPhone",model.DormitoryPhone)
                                                          };
                OleDbCommand command = new OleDbCommand(sql, con);
                command.Parameters.AddRange(parameters);

                if (command.ExecuteNonQuery() > 0)
                {
                    command.CommandText = "select  @@IDENTITY";
                  
                    int newId = (int)command.ExecuteScalar();
                    model.ID = newId;
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
        public int AddLead(string name, int DepartmentID, string Duty, string OfficeAddress,
            string Phone, string AnyCall, string OfficeRedPhone, string DormitoryRedPhone,
            string DormitoryAddress, string Code, string NamePinyin, string Mark1, string Mark2,
            string Importment, string DutyPinyin, string Secretary, string SecretaryRedPhone,
            string SecretaryDormitoryRedPhone, string SecretaryAnyCall, string SecretaryFax, string SecretaryDormitoryAddress,
            string Fax, string SecretaryAddress, string CPhone, string DormitoryPhone)
        {
            OleDbConnection con = new OleDbConnection(dal.strCon);
            try
            {


                con.Open();


                string sql = "Insert into Lead(OfficeAddress,Name,DepartmentID,Duty,Phone,AnyCall,OfficeRedPhone,DormitoryRedPhone,DormitoryAddress,Code,NamePinyin,Mark1,Mark2,Importment,DutyPinyin,Secretary,SecretaryRedPhone,SecretaryDormitoryRedPhone,SecretaryAnyCall,SecretaryFax,SecretaryDormitoryAddress,Fax,SecretaryAddress,CPhone,DormitoryPhone)";
                sql += "values(@OfficeAddress,@Name,@DepartmentID,@Duty,@Phone,@AnyCall,@OfficeRedPhone,@DormitoryRedPhone,@DormitoryAddress,@Code,@NamePinyin,@Mark1,@Mark2,@Importment,@DutyPinyin,@Secretary,@SecretaryRedPhone,@SecretaryDormitoryRedPhone,@SecretaryAnyCall,@SecretaryFax,@SecretaryDormitoryAddress,@Fax,@SecretaryAddress,@CPhone,@DormitoryPhone)";

                System.Data.OleDb.OleDbParameter[] parameters ={
                                                              new System.Data.OleDb.OleDbParameter("OfficeAddress",OfficeAddress),
                                                              new System.Data.OleDb.OleDbParameter("Name",name),
                                                              new System.Data.OleDb.OleDbParameter("DepartmentID",DepartmentID),
                                                                new System.Data.OleDb.OleDbParameter("Duty",Duty),
                                                              new System.Data.OleDb.OleDbParameter("Phone",Phone),
                                                              new System.Data.OleDb.OleDbParameter("AnyCall",AnyCall),
                                                              new System.Data.OleDb.OleDbParameter("OfficeRedPhone",OfficeRedPhone),
                                                              new System.Data.OleDb.OleDbParameter("DormitoryRedPhone",DormitoryRedPhone),
                                                              new System.Data.OleDb.OleDbParameter("DormitoryAddress",DormitoryAddress),
                                                              new System.Data.OleDb.OleDbParameter("Code",Code),
                                                              new System.Data.OleDb.OleDbParameter("NamePinyin",NamePinyin),
                                                              new System.Data.OleDb.OleDbParameter("Mark1",Mark1),
                                                              new System.Data.OleDb.OleDbParameter("Mark2",Mark2),
                                                              new System.Data.OleDb.OleDbParameter("Importment",Importment),
                                                              new System.Data.OleDb.OleDbParameter("DutyPinyin",DutyPinyin),
                                                              new System.Data.OleDb.OleDbParameter("Secretary",Secretary),
                                                              new System.Data.OleDb.OleDbParameter("SecretaryRedPhone",SecretaryRedPhone),
                                                              new System.Data.OleDb.OleDbParameter("SecretaryDormitoryRedPhone",SecretaryDormitoryRedPhone),
                                                              new System.Data.OleDb.OleDbParameter("SecretaryAnyCall",SecretaryAnyCall),
                                                              new System.Data.OleDb.OleDbParameter("SecretaryFax",SecretaryFax),
                                                              new System.Data.OleDb.OleDbParameter("SecretaryDormitoryAddress",SecretaryDormitoryAddress),
                                                              new System.Data.OleDb.OleDbParameter("Fax",Fax),
                                                              new System.Data.OleDb.OleDbParameter("SecretaryAddress",SecretaryAddress),
                                                              new System.Data.OleDb.OleDbParameter("CPhone",CPhone),
                                                              new System.Data.OleDb.OleDbParameter("DormitoryPhone",DormitoryPhone)
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

        public bool UpdateLead(int leadID, string name, int DepartmentID, string Duty, string OfficeAddress,
            string Phone, string AnyCall, string OfficeRedPhone, string DormitoryRedPhone,
            string DormitoryAddress, int Code, string NamePinyin, string Mark1, string Mark2,
            string Importment, string DutyPinyin, string Secretary, string SecretaryRedPhone,
            string SecretaryDormitoryRedPhone, string SecretaryAnyCall, string SecretaryFax, string SecretaryDormitoryAddress,
            string Fax, string SecretaryAddress, string CPhone, string DormitoryPhone)
        {
            string sql = "update Lead set ";
            sql += "OfficeAddress=@OfficeAddress,";
            sql += "Name=@Name,";
            sql += "DepartmentID=@DepartmentID,";
            sql += "Duty=@Duty,";
            sql += "Phone=@Phone,";
            sql += "AnyCall=@AnyCall,";
            sql += "OfficeRedPhone=@OfficeRedPhone,";
            sql += "DormitoryRedPhone=@DormitoryRedPhone,";
            sql += "DormitoryAddress=@DormitoryAddress,";
            sql += "Code=@Code,";
            sql += "NamePinyin=@NamePinyin,";
            sql += "Mark1=@Mark1,";
            sql += "Mark2=@Mark2,";
            sql += "Importment=@Importment,";
            sql += "DutyPinyin=@DutyPinyin,";
            sql += "Secretary=@Secretary,";
            sql += "SecretaryRedPhone=@SecretaryRedPhone,";
            sql += "SecretaryDormitoryRedPhone=@SecretaryDormitoryRedPhone,";
            sql += "SecretaryAnyCall=@SecretaryAnyCall,";
            sql += "SecretaryFax=@SecretaryFax,";
            sql += "SecretaryDormitoryAddress=@SecretaryDormitoryAddress,";
            sql += "Fax=@Fax,";
            sql += "SecretaryAddress=@SecretaryAddress,";
            sql += "CPhone=@CPhone,";
            sql += "DormitoryPhone=@DormitoryPhone ";
            sql += " where ID=@ID";
            System.Data.OleDb.OleDbParameter[] parameters ={
                                                              new System.Data.OleDb.OleDbParameter("OfficeAddress",OfficeAddress),
                                                              new System.Data.OleDb.OleDbParameter("Name",name),
                                                              new System.Data.OleDb.OleDbParameter("DepartmentID",DepartmentID),
                                                              new System.Data.OleDb.OleDbParameter("Duty",Duty),
                                                              new System.Data.OleDb.OleDbParameter("Phone",Phone),
                                                              new System.Data.OleDb.OleDbParameter("AnyCall",AnyCall),
                                                              new System.Data.OleDb.OleDbParameter("OfficeRedPhone",OfficeRedPhone),
                                                              new System.Data.OleDb.OleDbParameter("DormitoryRedPhone",DormitoryRedPhone),
                                                              new System.Data.OleDb.OleDbParameter("DormitoryAddress",DormitoryAddress),
                                                              new System.Data.OleDb.OleDbParameter("Code",Code),
                                                              new System.Data.OleDb.OleDbParameter("NamePinyin",NamePinyin),
                                                              new System.Data.OleDb.OleDbParameter("Mark1",Mark1),
                                                              new System.Data.OleDb.OleDbParameter("Mark2",Mark2),
                                                              new System.Data.OleDb.OleDbParameter("Importment",Importment),
                                                              new System.Data.OleDb.OleDbParameter("DutyPinyin",DutyPinyin),
                                                              new System.Data.OleDb.OleDbParameter("Secretary",Secretary),
                                                              new System.Data.OleDb.OleDbParameter("SecretaryRedPhone",SecretaryRedPhone),
                                                              new System.Data.OleDb.OleDbParameter("SecretaryDormitoryRedPhone",SecretaryDormitoryRedPhone),
                                                              new System.Data.OleDb.OleDbParameter("SecretaryAnyCall",SecretaryAnyCall),
                                                              new System.Data.OleDb.OleDbParameter("SecretaryFax",SecretaryFax),
                                                              new System.Data.OleDb.OleDbParameter("SecretaryDormitoryAddress",SecretaryDormitoryAddress),
                                                              new System.Data.OleDb.OleDbParameter("Fax",Fax),
                                                              new System.Data.OleDb.OleDbParameter("SecretaryAddress",SecretaryAddress),
                                                              new System.Data.OleDb.OleDbParameter("CPhone",CPhone),
                                                              new System.Data.OleDb.OleDbParameter("DormitoryPhone",DormitoryPhone),
                                                              new System.Data.OleDb.OleDbParameter("ID",leadID)
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

        public bool DelLead(int leadID)
        {
            string sql = "DELETE FROM Lead where ID=@ID";
            System.Data.OleDb.OleDbParameter[] parameters ={
                                                              new System.Data.OleDb.OleDbParameter("ID",leadID)
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

        public int UpdateSort(List<LeadModel> list)
        {
            string sql = "update Lead set ";
            sql += "Code=@Code ";
            sql += " where ID=@ID";
            int recordCount = 0;
            using (OleDbConnection con = dal.OpenConnection())
            {
                list.ForEach(x =>
                {

                    System.Data.OleDb.OleDbParameter[] parameters ={
                                                              new System.Data.OleDb.OleDbParameter("Code",x.Code),
                                                              new System.Data.OleDb.OleDbParameter("ID",x.ID)
                                                            };
                    recordCount += dal.BatchExecuteNonQuery(sql, parameters, con);
                });
            }
            return recordCount;
        }

        public int UpdateDepartmentSort(List<LeadModel> list)
        {
            string sql = "update Department set ";
            sql += "Code=@Code ";
            sql += " where ID=@ID";
            int recordCount = 0;
            using (OleDbConnection con = dal.OpenConnection())
            {
                list.ForEach(x =>
                {

                    System.Data.OleDb.OleDbParameter[] parameters ={
                                                              new System.Data.OleDb.OleDbParameter("Code",x.Code),
                                                              new System.Data.OleDb.OleDbParameter("ID",x.ID)
                                                            };
                    recordCount += dal.BatchExecuteNonQuery(sql, parameters, con);
                });
            }
            return recordCount;
        }

        public int ClearSort()
        {
            string sql = "update Department set ";
            sql += "Code=null ";

            return dal.ExecuteNonQuery(sql, null);
        }

    }


}
