using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tulpep.NotificationWindow;
namespace OraclaMajorAssignmentY3S2
{
    public partial class DataGrid : Form
    {
        Controller.myclass.StudentController _student = new Controller.myclass.StudentController();
        public DataGrid()
        {
            InitializeComponent();
            
        }
      
        private void DataGrid_Load(object sender, EventArgs e)
        {
            fillDataGrid();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var add = new InputForm();
            add.ShowDialog();
            dgStudent.Rows.Clear();
            dgStudent.Refresh();
            fillDataGrid();
        }
       
        private void btnView_Click(object sender, EventArgs e)
        {      
            int id = SelectStudent();
            if(id == 0)
            {
                MessageBox.Show("No Student Have Been Select", "Info Missing",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
            else
            {
                var view = new View(id);
                view.ShowDialog();
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
                using (var Edit = new Edit(id))
                {
                    ;
                    if (Edit.ShowDialog() == DialogResult.OK)
                    {
                        MessageBox.Show("Good", "Good", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                dgStudent.Rows.Clear();
                dgStudent.Refresh();
                fillDataGrid();
            }
        }

        private void fillDataGrid()
        {
            try
            {
                _student.PullData();
                int act = 0;
                foreach (DataRow r in _student.dt.Rows)
                {
                    int i = dgStudent.Rows.Add();
                    string first_name = r["first_name"].ToString();
                    string last_name = r["last_name"].ToString();
                    DateTime dob = DateTime.Parse(r["DOB"].ToString());
                    var now = DateTime.Today;
                    int age = now.Year - dob.Year;
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
                    dgStudent.Rows[i].Cells[3].Value = age.ToString();
                    dgStudent.Rows[i].Cells[4].Value = dob.ToString("dd/MM/yy");
                    dgStudent.Rows[i].Cells[5].Value = r["email"].ToString();
                    int status = int.Parse(r["status"].ToString());
                    if (status == 0)
                    {
                        dgStudent.Rows[i].Cells[6].Value = "Disable";
                        dgStudent.Rows[i].Cells[6].Style = new DataGridViewCellStyle { ForeColor = Color.Red };
                    }
                    else if (status == 2)
                    {
                        dgStudent.Rows[i].Cells[6].Value = "Active";
                        dgStudent.Rows[i].Cells[6].Style = new DataGridViewCellStyle { ForeColor = Color.Green };
                        act++;
                    }
                    else
                    {
                        dgStudent.Rows[i].Cells[6].Value = "Pending";
                        dgStudent.Rows[i].Cells[6].Style = new DataGridViewCellStyle { ForeColor = Color.FromArgb(149, 117, 11) };
                    }
                    dgStudent.Rows[i].Cells[7].Value = r["contactnumber"].ToString();

                }
                dgStudent.ClearSelection();
                lbStudentCount.Text = act.ToString();
            }
            catch (Exception ex) { MessageBox.Show("Something When Wrong : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }

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

        private void guna2Panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    } 
}
