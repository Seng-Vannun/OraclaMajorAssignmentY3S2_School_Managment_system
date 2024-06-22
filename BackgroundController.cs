using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using OraclaMajorAssignmentY3S2.Model.myclass;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace OraclaMajorAssignmentY3S2.Controller.myclass
{
     class BackgroundController: BackgroundModel
    {
        public DataTable dt = new DataTable();
        public DataSet ds = new DataSet();
        OracleDataAdapter adp = new OracleDataAdapter();     
        public void PullData(int id)
        {
            string sql = "select * from family_background where student_id = " + id;
            OracleCommand cmd = new OracleCommand();
            cmd = new OracleCommand(sql, conn);
            adp.SelectCommand = cmd;
            ds.Clear();
            adp.Fill(ds);
            dt = ds.Tables[0];
        }
        public void InsertData(int id)
        {
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "CreateFamilyBackground";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("p_student_id", OracleDbType.Int32).Value = id;
            cmd.Parameters.Add("p_Father_Name", OracleDbType.Varchar2).Value = Father_Name;
            cmd.Parameters.Add("p_Father_Occupation", OracleDbType.Varchar2).Value = Father_Occupation;
            cmd.Parameters.Add("p_Mother_Name", OracleDbType.Varchar2).Value = Mother_Name;
            cmd.Parameters.Add("p_Mother_Occupation", OracleDbType.Varchar2).Value = Mother_Occupation;
            cmd.Parameters.Add("p_contact_number", OracleDbType.Varchar2).Value = Contact_Number;
            cmd.Parameters.Add("p_Current_Address", OracleDbType.Varchar2).Value = Current_Address;
            cmd.ExecuteNonQuery();
        }
        public void UpdateData(int id)
        {
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "UpdateFamilyBackground";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("p_student_id", OracleDbType.Int32).Value = id;
            cmd.Parameters.Add("p_Father_Name", OracleDbType.Varchar2).Value = Father_Name;
            cmd.Parameters.Add("p_Father_Occupation", OracleDbType.Varchar2).Value = Father_Occupation;
            cmd.Parameters.Add("p_Mother_Name", OracleDbType.Varchar2).Value = Mother_Name;
            cmd.Parameters.Add("p_Mother_Occupation", OracleDbType.Varchar2).Value = Mother_Occupation;
            cmd.Parameters.Add("p_contact_number", OracleDbType.Varchar2).Value = Contact_Number;
            cmd.Parameters.Add("p_Current_Address", OracleDbType.Varchar2).Value = Current_Address;
            cmd.ExecuteNonQuery();
        }
    }
}
