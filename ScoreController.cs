using OraclaMajorAssignmentY3S2.Model.myclass;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace OraclaMajorAssignmentY3S2.Controller.myclass
{
    class ScoreController:ScoreModel
    {
        public DataTable dt = new DataTable();
        public DataSet ds = new DataSet();
        OracleDataAdapter adp = new OracleDataAdapter();
        public void PullScore(int id)
        {
            string sql = "select * from scores where student_id = " + id;
            OracleCommand cmd = new OracleCommand();
            cmd = new OracleCommand(sql, conn);
            adp.SelectCommand = cmd;
            ds.Clear();
            adp.Fill(ds);
            dt = ds.Tables[0];
        }
        public void PullSubject()
        {
            string sql = "select * from Subject";
            OracleCommand cmd = new OracleCommand();
            cmd = new OracleCommand(sql, conn);
            adp.SelectCommand = cmd;
            ds.Clear();
            adp.Fill(ds);
            dt = ds.Tables[0];
        }
        public void InsertScore(int id)
        {

            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "InsertScore";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("p_student_id", OracleDbType.Int32).Value = id;
            cmd.Parameters.Add("p_subject_id", OracleDbType.Int32).Value = Subject_id;
            cmd.Parameters.Add("p_year", OracleDbType.Int32).Value = Year;
            cmd.Parameters.Add("p_semester", OracleDbType.Int32).Value = Semester;
            cmd.Parameters.Add("p_score", OracleDbType.Int32).Value = Score;
            cmd.ExecuteNonQuery();
        }
        public void InsertSubject()
        {

            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "InsertSubject";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("p_subject_name", OracleDbType.Varchar2).Value = "Mathematics";
            cmd.Parameters.Add("p_subject_desc", OracleDbType.Varchar2).Value = "Advanced Calculus";
            cmd.ExecuteNonQuery();
        }
        public void UpdateData(int id,int semester,double score)
        {
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "UpdateScore";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("p_student_id", OracleDbType.Int32).Value = id;
            cmd.Parameters.Add("p_subject_id", OracleDbType.Int32).Value = Subject_id;
            cmd.Parameters.Add("p_year", OracleDbType.Int32).Value = Year;
            cmd.Parameters.Add("p_semester", OracleDbType.Int32).Value = semester;
            cmd.Parameters.Add("p_score", OracleDbType.Double).Value = score;
            cmd.ExecuteNonQuery();
        }
    }
}
