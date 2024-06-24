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
    public partial class Report : Form
    {
        Controller.myclass.StudentController _student = new Controller.myclass.StudentController();
        Controller.myclass.ScoreController _score = new Controller.myclass.ScoreController();
        string name = null;
        public Report()
        {
            InitializeComponent();
        }
        
        private void Report_Load(object sender, EventArgs e)
        {
            _student.PullData();
            foreach (DataRow r in _student.dt.Rows)
            {
                int i = dgStudent.Rows.Add();
                string first_name = r["first_name"].ToString();
                string last_name = r["last_name"].ToString();
                Image img = _student.ConvertToImg(r["PROFILE_PICTURE"]);
                dgStudent.Rows[i].Cells[0].Value = r["ID"];
                if (img != null)
                {
                    dgStudent.Rows[i].Cells[1].Value = img;
                }
                else
                {
                    dgStudent.Rows[i].Cells[1].Value = Image.FromFile("Photos//man.png");
                }
                dgStudent.Rows[i].Cells[2].Value = first_name + " " + last_name;
                name = first_name+" "+last_name;
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            int id = SelectStudent();
            if (id == 0)
            {
                MessageBox.Show("No Student Have Been Select", "Info Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                var Input = new ScoreInput(id,name);
                Input.ShowDialog();
            }
          
        }
        private int SelectStudent()
        {
            int id = 0;
            if (dgStudent.SelectedRows.Count > 0)
            {
                id = int.Parse(dgStudent.SelectedRows[0].Cells[0].Value + string.Empty);
                return id;
            }
            return id;
        }
    }
}
