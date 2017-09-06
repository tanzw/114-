using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.OleDb;
using System.Reflection;
using System.Data;

namespace QueryPlatform.Code.Common
{
    public class AccessData
    {
        public readonly string strCon = string.Format("{0}{1}{2}{3}", "provider=microsoft.jet.oledb.4.0; Data Source=", System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"\" + Config.DB + ";", "Jet OleDb:Database Password=" + Config.Password + ";");

        public OleDbConnection OpenConnection()
        {
            try
            {
                OleDbConnection con = new OleDbConnection(strCon);
                con.Open();
                return con;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        
    
                 
        public int BatchExecuteNonQuery(string text, OleDbParameter[] parameters,OleDbConnection con)
        {
           
                OleDbCommand command = new OleDbCommand(text, con);
                CreateParameters(command, parameters);
                return command.ExecuteNonQuery();
            
        }
        public int ExecuteNonQuery(string text, OleDbParameter[] parameters)
        {
            using (OleDbConnection con = OpenConnection())
            {
                OleDbCommand command = new OleDbCommand(text, con);
                CreateParameters(command, parameters);
                return command.ExecuteNonQuery();
            }
        }
        public int ExecuteNonQuery(string text)
        {
            using (OleDbConnection con = OpenConnection())
            {
                OleDbCommand command = new OleDbCommand(text, con);

                return command.ExecuteNonQuery();
            }
        }
        public object ExecuteScalar(string text, OleDbParameter[] parameters)
        {
            using (OleDbConnection con = OpenConnection())
            {
                OleDbCommand command = new OleDbCommand(text, con);
                CreateParameters(command, parameters);
                return command.ExecuteScalar();
            }
        }

        public OleDbDataReader ExecuteReader(string text, OleDbParameter[] parameters)
        {

            OleDbConnection con = OpenConnection();
                OleDbCommand command = new OleDbCommand(text, con);
                CreateParameters(command, parameters);
                return command.ExecuteReader();
            
        }

        public OleDbDataReader ExecuteReader(string text)
        {
            using (OleDbConnection con = OpenConnection())
            {
                OleDbCommand command = new OleDbCommand(text, con);

                return command.ExecuteReader();
            }
        }

        public DataSet ExecuteDataSet(string text, OleDbParameter[] parameters)
        {
            using (OleDbConnection con = OpenConnection())
            {
                OleDbCommand command = new OleDbCommand(text, con);
                CreateParameters(command, parameters);
                OleDbDataAdapter da = new OleDbDataAdapter(command);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
        }

        public DataSet ExecuteDataSet(string text)
        {
            using (OleDbConnection con = OpenConnection())
            {
                OleDbCommand command = new OleDbCommand(text, con);

                OleDbDataAdapter da = new OleDbDataAdapter(command);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
        }

        public DataTable ExecuteDataTable(string text, OleDbParameter[] parameters)
        {
            using (OleDbConnection con = OpenConnection())
            {
                OleDbCommand command = new OleDbCommand(text, con);
                CreateParameters(command, parameters);
                OleDbDataAdapter da = new OleDbDataAdapter(command);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public DataTable ExecuteDataTable(string text)
        {
            using (OleDbConnection con = OpenConnection())
            {
                OleDbCommand command = new OleDbCommand(text, con);

                OleDbDataAdapter da = new OleDbDataAdapter(command);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        private void CreateParameters(OleDbCommand command, OleDbParameter[] parameters)
        {
            if (parameters != null)
            {
                command.Parameters.AddRange(parameters);
            }
        }




    }
}
