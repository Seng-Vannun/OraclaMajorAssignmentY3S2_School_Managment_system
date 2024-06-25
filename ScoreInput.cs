using Guna.UI2.WinForms;
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
    public partial class ScoreInput : Form
    {
        private Dictionary<int, Dictionary<int, (double Semester1, double Semester2)>> ScoresBySubject =
             new Dictionary<int, Dictionary<int, (double, double)>>();
        bool checkComboxbox = false;
        Controller.myclass.StudentController _student = new Controller.myclass.StudentController();
        Controller.myclass.ScoreController _score = new Controller.myclass.ScoreController();
        int id;
        DataTable subjectTable = new DataTable();
        public ScoreInput(int id , string fullname)
        {
            InitializeComponent();
            this.id = id;
            lbFullname.Text = fullname;
            lbId.Text = id.ToString();
            fillsubject();

        }
        private void ScoreInput_Load(object sender, EventArgs e)
        {
            
            foreach (DataRow r in subjectTable.Rows)
            {
                int subject_id = int.Parse(r["subject_id"].ToString());
                ScoresBySubject[subject_id] = new Dictionary<int, (double, double)>();
                for (int year = 1; year <= 4; year++)
                {
                    ScoresBySubject[subject_id][year] = (0, 0); // Initialize scores to 0 for both semesters
                }
            }
            CheckIfStudentHaveScore(id);
        }
        private void cbSubjct_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.IsHandleCreated) return;
            guna2DataGridView1.Rows.Clear();
            if (checkComboxbox)
            {
                if (cbSubjct.SelectedValue != null && !string.IsNullOrWhiteSpace(cbSubjct.SelectedValue.ToString()))
                {
                    int selectedSubject = int.Parse(cbSubjct.SelectedValue.ToString());
                    foreach (var entry in ScoresBySubject[selectedSubject])
                    {
                        var year = entry.Key;
                        var scores = entry.Value;
                        // Adding rows to DataGridView: Year, Semester, Score
                        guna2DataGridView1.Rows.Add(year, scores.Semester1, scores.Semester2);
                    }
                }
            }
        }
        private void guna2DataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int selectedSubject = int.Parse(cbSubjct.SelectedValue.ToString());
            int year = Convert.ToInt32(guna2DataGridView1.Rows[e.RowIndex].Cells[0].Value);
            double score1, score2;

            if (double.TryParse(guna2DataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString(), out score1) &&
                double.TryParse(guna2DataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString(), out score2))
            {
                // Update scores in the dictionary
                ScoresBySubject[selectedSubject][year] = (score1, score2);
            }
            else
            {
                // Reset to old values if invalid input
                var scores = ScoresBySubject[selectedSubject][year];
                guna2DataGridView1.Rows[e.RowIndex].Cells[1].Value = scores.Semester1;
                guna2DataGridView1.Rows[e.RowIndex].Cells[2].Value = scores.Semester2;
            }
        }
        private void fillsubject()
        {
            _score.PullSubject();
            subjectTable = _score.dt.Copy();
            cbSubjct.DataSource = subjectTable;
            cbSubjct.DisplayMember= "Name";
            cbSubjct.ValueMember = "SUBJECT_ID";
          
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

                if (score_id == 0)
                {
                    foreach (var subject in ScoresBySubject)
                    {
                        foreach (var year in subject.Value.Keys)
                        {
                            for (int semester = 1; semester <= 2; semester++)
                            {
                                _score.Subject_id = subject.Key;
                                _score.Year = year;
                                _score.Semester = semester;
                                _score.Score = 0;
                                _score.InsertScore(id); // Consider batching these inserts if possible
                            }
                        }
                    }
                }
                else
                {
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
                int selectedSubject = 1;
                guna2DataGridView1.Rows.Clear();
                // Populate DataGridView with years, semesters, and scores for the selected subject
                foreach (var entry in ScoresBySubject[selectedSubject])
                {
                    var year = entry.Key;
                    var scores = entry.Value;
                    // Adding rows to DataGridView: Year, Semester, Score
                    guna2DataGridView1.Rows.Add(year, scores.Semester1, scores.Semester2);
                }
                checkComboxbox = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during update: " + ex.Message);
            }
        }
        private void btnCancelStudentTab_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (var subjectEntry in ScoresBySubject)
                {
                    int subjectId = subjectEntry.Key;
                    foreach (var yearEntry in subjectEntry.Value)
                    {
                        int year = yearEntry.Key;
                        (double scoreSem1, double scoreSem2) = yearEntry.Value;
                        _score.Subject_id = subjectId;
                        _score.Year = year;
                        // Update for Semester 1
                        _score.UpdateData(id,1, scoreSem1);
                        // Update for Semester 2
                        _score.UpdateData(id, 2, scoreSem2);
                    }
                }
                this.Close();
            }
            catch (Exception ex)
            {
               MessageBox.Show("Error during update: " + ex.Message);
            }
        }
    }
}
