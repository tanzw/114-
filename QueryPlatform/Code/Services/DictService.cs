using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
namespace QueryPlatform.Code.Services
{
    public class DictService
    {
        Common.AccessData dal = new Common.AccessData();

        public DataTable GetAreaList()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Name");
            Code.Common.Config.AreaList.ForEach(x =>
            {
                dt.Rows.Add(x);
            });
            return dt;
        }
        public DataTable GetDictList()
        {
            string sql = "select  * from District";
            DataSet ds = dal.ExecuteDataSet(sql);
            var dt = ds.Tables[0];
            //DataRow row = dt.NewRow();

            //row["DistNo"] = 0;
            //row["PlaceName"] = "";
            //dt.Rows.InsertAt(row, 0);
            return dt;
        }

        public DataTable GetDictList2(string key = "")
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(key))
                {
                    string sql = "select  * from District where DistNo LIKE '%'+ @Key + '%' or PlaceName  LIKE '%'+ @Key + '%' or Code  LIKE '%'+ @Key + '%' or Pinyin  LIKE '%'+ @Key + '%'";
                    System.Data.OleDb.OleDbParameter[] parameters ={
                                                              new System.Data.OleDb.OleDbParameter("Key",key)
                                                          };
                    DataSet ds = dal.ExecuteDataSet(sql, parameters);
                    var dt = ds.Tables[0];
                    return dt;
                }
                else
                {
                    string sql = "select  * from District";
                    DataSet ds = dal.ExecuteDataSet(sql);
                    var dt = ds.Tables[0];
                    return dt;
                }
            }
            catch
            {
                return null;
            }
        }

        public bool AddDist(string DistNo, string PlaceName, string Code, string pinyin)
        {
            try
            {
                string sql = "insert into District(DistNo,PlaceName,Code,Pinyin)values(@DistNo,@PlaceName,@Code,@Pinyin)";
                System.Data.OleDb.OleDbParameter[] parameters ={
                                                              //new System.Data.OleDb.OleDbParameter("DistNo",DistNo),
                                                              //new System.Data.OleDb.OleDbParameter("PlaceName",PlaceName),
                                                              //new System.Data.OleDb.OleDbParameter("Code",Code),
                                                              //new System.Data.OleDb.OleDbParameter("Pinyin",pinyin)
                                                               new System.Data.OleDb.OleDbParameter("DistNo",  OleDbType.VarChar,4){ Value=DistNo},
                                                              new System.Data.OleDb.OleDbParameter("PlaceName",OleDbType.VarChar,50){Value= PlaceName},
                                                              new System.Data.OleDb.OleDbParameter("Code",OleDbType.VarChar,6){ Value= Code },
                                                              new System.Data.OleDb.OleDbParameter("Pinyin", OleDbType.VarChar,50){Value= pinyin}
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

        public bool UpdateDist(string DistNo, string PlaceName, string Code, string pinyin, string oldDistNo, string oldPlaceName)
        {
            try
            {
                string sql = "Update District set DistNo=@DistNo,PlaceName=@PlaceName,Code=@Code,Pinyin=@PinyinPinyin";
                sql += " where DistNo=@OLDDistNo and PlaceName=@OLDPlaceName";
                System.Data.OleDb.OleDbParameter[] parameters ={
                                                           new System.Data.OleDb.OleDbParameter("DistNo",  OleDbType.VarChar,4){ Value=DistNo},
                                                              new System.Data.OleDb.OleDbParameter("PlaceName",OleDbType.VarChar,50){Value= PlaceName},
                                                              new System.Data.OleDb.OleDbParameter("Code",OleDbType.VarChar,6){ Value= Code },
                                                              new System.Data.OleDb.OleDbParameter("Pinyin", OleDbType.VarChar,50){Value= pinyin},
                                                              new System.Data.OleDb.OleDbParameter("OLDDistNo",oldDistNo),
                                                              new System.Data.OleDb.OleDbParameter("OLDPlaceName",oldPlaceName)
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

        public bool DelDist(string DistNo, string PlaceName, string Code, string pinyin)
        {
            try
            {
                string sql = "DELETE FROM District WHERE  DistNo=@DistNo and PlaceName=@PlaceName and Code=@Code and Pinyin=@PinyinPinyin";
                System.Data.OleDb.OleDbParameter[] parameters ={
                                                          new System.Data.OleDb.OleDbParameter("DistNo",  OleDbType.VarChar,4){ Value=DistNo},
                                                              new System.Data.OleDb.OleDbParameter("PlaceName",OleDbType.VarChar,50){Value= PlaceName},
                                                              new System.Data.OleDb.OleDbParameter("Code",OleDbType.VarChar,6){ Value= Code },
                                                              new System.Data.OleDb.OleDbParameter("Pinyin", OleDbType.VarChar,50){Value= pinyin}
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
