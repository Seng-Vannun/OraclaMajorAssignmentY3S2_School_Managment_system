using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace OraclaMajorAssignmentY3S2
{
    public partial class Homepage : Form
    {
        Controller.myclass.StudentController _student = new Controller.myclass.StudentController();
        public Homepage()
        {
            InitializeComponent();
        }

        private void Homepage_Load(object sender, EventArgs e)
        {
            try
            {
                _student.PullData();
                int act=0,pen=0,dis = 0;
                foreach (DataRow r in _student.dt.Rows)
                {
                   
                    int status = int.Parse(r["status"].ToString());
                    if (status == 0)
                    {
                        dis++;
                    }
                    else if (status == 2)
                    {
                        act++;
                    }
                    else
                    {
                       pen++;
                    }
                }
                string[] categories = { "Active", "Pending", "Drop Out" };
               
                lbActive.Text = act.ToString();
                lbDisable.Text = dis.ToString();
                lbPending.Text = pen.ToString();
                chartActive.DataPoints.Add(new Guna.Charts.WinForms.LPoint("Month 1", 0));
                ChartPending.DataPoints.Add(new Guna.Charts.WinForms.LPoint("Month 1", 0));
                ChartDisable.DataPoints.Add(new Guna.Charts.WinForms.LPoint("Month 1", 0));
                chartActive.DataPoints.Add(new Guna.Charts.WinForms.LPoint("Month 2", act));
                ChartPending.DataPoints.Add(new Guna.Charts.WinForms.LPoint("Month 2", pen));
                ChartDisable.DataPoints.Add(new Guna.Charts.WinForms.LPoint("Month 2", dis));
                chartActive.DataPoints.Add(new Guna.Charts.WinForms.LPoint("Month 3", act+5));
                ChartPending.DataPoints.Add(new Guna.Charts.WinForms.LPoint("Month 3", pen+4));
                ChartDisable.DataPoints.Add(new Guna.Charts.WinForms.LPoint("Month 3", dis+1));
                // Refresh the chart to display the new data
                gunaChart1.Refresh();
            }
            catch (Exception ex) { MessageBox.Show("Something When Wrong : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }

        }
    }
    
}
