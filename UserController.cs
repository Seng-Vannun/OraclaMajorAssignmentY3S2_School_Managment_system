using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Windows.Forms;
using System.Data;
using System.Runtime.InteropServices;

namespace OraclaMajorAssignmentY3S2.Controller.myclass
{
     class UserController:Model.myclass.UserModel
    {
        string Username { get; set; }
        string Password { get; set; }
        public DataTable dt = new DataTable();
        public DataSet ds = new DataSet();
        OracleDataAdapter adp = new OracleDataAdapter();
        public void PullData()
        {
            string sql = "select * from user_tbl";
            OracleCommand cmd = new OracleCommand();
            cmd = new OracleCommand(sql, conn);
            adp.SelectCommand = cmd;
            ds.Clear();
            adp.Fill(ds);
            dt = ds.Tables[0];
        }
        public string login(string _username, string _password)
        {
            string sql = "select * from user_tbl where trim(username) = '"+_username+"' AND password = '"+_password+"'";
            OracleCommand cmd = new OracleCommand();
            cmd = new OracleCommand(sql, conn);
            adp.SelectCommand = cmd;
            ds.Clear();
            adp.Fill(ds);
            
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
                foreach (DataRow r in dt.Rows)
                {
                    Username = r["Username"].ToString();
                    break;
                }
                return "1";
            }
            else
            {
                return "0";
            }
        }
        public void InsertData()
        {
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "adduser";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("p_username", OracleDbType.Varchar2).Value = username;
            cmd.Parameters.Add("p_first_name", OracleDbType.Varchar2).Value = password;
            cmd.Parameters.Add("p_first_name", OracleDbType.Varchar2).Value = first_name;
            cmd.Parameters.Add("p_last_name", OracleDbType.Varchar2).Value = last_name;
            cmd.ExecuteNonQuery();
        }
        public void UpdateData()
        {
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "UpdateUser";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("SpID", OracleDbType.Int32).Value = User_id;
            cmd.Parameters.Add("SpFirst_Name", OracleDbType.Varchar2).Value = first_name;
            cmd.Parameters.Add("SpLast_Name", OracleDbType.Varchar2).Value = last_name;
            cmd.ExecuteNonQuery();
        }
    }
}
