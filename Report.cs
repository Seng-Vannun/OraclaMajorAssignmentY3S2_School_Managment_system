using Guna.UI2.WinForms;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OraclaMajorAssignmentY3S2
{
    public partial class Report : Form
    {
        Controller.myclass.StudentController _student = new Controller.myclass.StudentController();
        Controller.myclass.ScoreController _score = new Controller.myclass.ScoreController();
        private Dictionary<int, Dictionary<int, (double Semester1, double Semester2)>> ScoresBySubject =
            new Dictionary<int, Dictionary<int, (double, double)>>();
        string name = null;
        bool check = true;
        DataTable subjectTable = new DataTable();
        public Report()
        {
            InitializeComponent();
        }
        
        private void Report_Load(object sender, EventArgs e)
        {

            _student.PullData();
            foreach (DataRow r in _student.dt.Rows)
            {
                int status = int.Parse(r["status"].ToString());
                if(status != 2)
                {
                    continue;
                }
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
            }
            getSubject();
            dgStudent.ClearSelection();
            foreach (DataRow r in subjectTable.Rows)
            {
                int subject_id = int.Parse(r["subject_id"].ToString());
                ScoresBySubject[subject_id] = new Dictionary<int, (double, double)>();
                for (int year = 1; year <= 4; year++)
                {
                    ScoresBySubject[subject_id][year] = (0, 0); // Initialize scores to 0 for both semesters
                }
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
                chartFunction();
            }
          
        }
        private int SelectStudent()
        {
            int id = 1;
            try
            {
                if (dgStudent.SelectedRows.Count > 0)
                {
                    id = int.Parse(dgStudent.SelectedRows[0].Cells[0].Value + string.Empty);
                    name = dgStudent.SelectedRows[0].Cells[2].Value.ToString();
                    return id;
                }
                return id;
            }catch(Exception ex)
            {
                return id;
            }
        }
        private void getSubject()
        {
            _score.PullSubject();
            subjectTable = _score.dt.Copy();
        }

        private void dgStudent_SelectionChanged(object sender, EventArgs e)
        {
            chartFunction();
        }
        public void CheckIfStudentHaveScore(int id)
        {
            try
            {
                _score.PullScore(id);
                int score_id = 0;
                foreach (DataRow r in _score.dt.Rows)
                {
                    score_id = int.Parse(r["Score_id"].ToString());
                    if (score_id != 0)
                    {
                        break;
                    }
                }

                if (score_id != 0)
                {
                    check = true;
                    foreach (DataRow r in _score.dt.Rows)
                    {
                        int year = int.Parse(r["year"].ToString());
                        int semester = int.Parse(r["SEMESTER"].ToString());
                        int subject_id = int.Parse(r["subject_id"].ToString());
                        double score = double.Parse(r["score"].ToString());

                        if (ScoresBySubject.ContainsKey(subject_id) && ScoresBySubject[subject_id].ContainsKey(year))
                        {
                            var currentScores = ScoresBySubject[subject_id][year];
                            if (semester == 1)
                                ScoresBySubject[subject_id][year] = (score, currentScores.Item2);
                            else
                                ScoresBySubject[subject_id][year] = (currentScores.Item1, score);
                        }
                    }

                }
                else
                {
                    check= false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during update: " + ex.Message);
            }
        }
        private void chartFunction()
        {
            if (dgStudent.SelectedRows.Count > 0)
            {
                int id = SelectStudent();
                CheckIfStudentHaveScore(id);
                foreach (DataRow r in subjectTable.Rows)
                {
                    int subject_id = int.Parse(r["subject_id"].ToString());
                    var charts = ChartLineSubject1;
                    switch (subject_id)
                    {
                        case 1:
                            charts = ChartLineSubject1;
                            break;
                        case 2:
                            charts = ChartLineSubject2;
                            break;
                        case 3:
                            charts = ChartLineSubject3;
                            break;
                        case 4:
                            charts = ChartLineSubject4;
                            break;
                        case 5:
                            charts = ChartLineSubject5;
                            break;
                    }
                    charts.DataPoints.Clear();
                    if (check)
                    {
                        foreach (var entry in ScoresBySubject[subject_id])
                        {
                            var year = entry.Key;
                            var scores = entry.Value;
                            var tot = (scores.Semester1 + scores.Semester2) / 2;
                            // Adding rows to DataGridView: Year, Semester, Score
                            charts.DataPoints.Add("Semester 1", scores.Semester1);
                            charts.DataPoints.Add("Semester 2", scores.Semester2);
                            charts.DataPoints.Add("Avg Year " + year, tot);
                        }
                    }
                    else
                    {
                        foreach (var entry in ScoresBySubject[subject_id])
                        {
                            // Adding rows to DataGridView: Year, Semester, Score
                            var year = entry.Key;
                            charts.DataPoints.Add("Semester 1", 0);
                            charts.DataPoints.Add("Semester 2", 0);
                            charts.DataPoints.Add("Avg Year " + year, 0);
                        }
                    }
                }
                gunaChart1.Refresh();

            }
        }
    }
}
