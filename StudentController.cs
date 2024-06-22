using OraclaMajorAssignmentY3S2.Model.myclass;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace OraclaMajorAssignmentY3S2.Controller.myclass
{
    class StudentController:StudentModel
    {
        Controller.myclass.BackgroundController backgroundController = new Controller.myclass.BackgroundController();
        public DataTable dt = new DataTable();
        public DataSet ds = new DataSet();
        OracleDataAdapter adp = new OracleDataAdapter();
        public void PullData()
        {
            string sql = "select * from Student";
            OracleCommand cmd = new OracleCommand();
            cmd = new OracleCommand(sql, conn);
            adp.SelectCommand = cmd;
            ds.Clear();
            adp.Fill(ds);
            dt = ds.Tables[0];
        }
        public void PullData(int id)
        {
            string sql = "select * from Student where id = " + id;
            OracleCommand cmd = new OracleCommand();
            cmd = new OracleCommand(sql, conn);
            adp.SelectCommand = cmd;
            ds.Clear();
            adp.Fill(ds);
            dt = ds.Tables[0];
        }
        public int InsertData()
        {

            int id = 0;
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "CreateStudent";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("p_First_Name", OracleDbType.Varchar2).Value = FirstName;
            cmd.Parameters.Add("p_Last_Name", OracleDbType.Varchar2).Value = LastName;
            cmd.Parameters.Add("p_Gender", OracleDbType.Varchar2).Value = "1";
            cmd.Parameters.Add("p_ContactNumber", OracleDbType.Varchar2).Value = ContactNumber;
            cmd.Parameters.Add("p_DOB", OracleDbType.Date).Value = DateOfBirth;
            cmd.Parameters.Add("p_Email", OracleDbType.Varchar2).Value = Email;
            cmd.Parameters.Add("p_Profile_Picture", OracleDbType.Blob).Value=ProfilePicture;
            cmd.Parameters.Add("p_Status", OracleDbType.Int32).Value = 1;
            cmd.Parameters.Add("p_id", OracleDbType.Int32, ParameterDirection.Output);
            cmd.ExecuteNonQuery();

            if (cmd.Parameters["p_id"].Value is Oracle.ManagedDataAccess.Types.OracleDecimal oracleDecimal)
            {
                if (!oracleDecimal.IsNull) // Check if the value is not null
                {
                    id = (int)oracleDecimal.Value;  // Convert OracleDecimal to int
                }
            }
            return id;
        }
        public void UpdateData(int id)
        {
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "UpdateStudent";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("p_ID",OracleDbType.Int32).Value = id;
            cmd.Parameters.Add("p_First_Name", OracleDbType.Varchar2).Value = FirstName;
            cmd.Parameters.Add("p_Last_Name", OracleDbType.Varchar2).Value = LastName;
            cmd.Parameters.Add("p_Gender", OracleDbType.Varchar2).Value = Gender;
            cmd.Parameters.Add("p_ContactNumber", OracleDbType.Varchar2).Value = ContactNumber;
            cmd.Parameters.Add("p_DOB", OracleDbType.Date).Value = DateOfBirth;
            cmd.Parameters.Add("p_Email", OracleDbType.Varchar2).Value = Email;
            cmd.Parameters.Add("p_Profile_Picture", OracleDbType.Blob).Value = ProfilePicture;
            cmd.Parameters.Add("p_Status", OracleDbType.Int32).Value = Status;
            cmd.ExecuteNonQuery();
        }
        public void ActivceStatus()
        {
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "UpdateStudentStatus";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("p_ID", OracleDbType.Int32).Value = ID;
            cmd.Parameters.Add("p_Status", OracleDbType.Int32).Value = 2;
            cmd.ExecuteNonQuery();
        }
        public void DisableStatus()
        {
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "UpdateStudentStatus";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("p_ID", OracleDbType.Int32).Value = ID;
            cmd.Parameters.Add("p_Status", OracleDbType.Int32).Value = 0;
            cmd.ExecuteNonQuery();
        }
        public Image ConvertToImg(object _Blob)
        {

            if (_Blob is DBNull)
            {
                return null;
            }
            byte[] Blob = (byte[])_Blob;
            {
                using (var ms = new System.IO.MemoryStream(Blob))
                {
                    Image img = Image.FromStream(ms);
                    return img;
                }
            }
        }
    }
}
