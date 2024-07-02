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
    public partial class View : Form
    {
        Controller.myclass.StudentController _student = new Controller.myclass.StudentController();
        Controller.myclass.BackgroundController _background = new Controller.myclass.BackgroundController();
        int id = 0;
        public View(int id)
        {
            InitializeComponent();
            this.id = id;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void View_Load(object sender, EventArgs e)
        {
            try
            {
                _student.PullData(id);
                foreach (DataRow r in _student.dt.Rows)
                {
                    
                    string first_name = r["first_name"].ToString();
                    string last_name = r["last_name"].ToString();
                    DateTime dob = DateTime.Parse(r["DOB"].ToString());
                    var now = DateTime.Today;
                    int age = now.Year - dob.Year;
                    Image img = _student.ConvertToImg(r["PROFILE_PICTURE"]);
                    lbId.Text = r["ID"].ToString();
                    if (img != null)
                    {
                        guna2PictureBox1.Image = img;
                    }
                    else
                    {
                        guna2PictureBox1.Image = Image.FromFile("Photos//man.png");
                    }
                    lbFullname.Text = first_name + " " + last_name;
                    lbAge.Text = age.ToString();
                    lbGender.Text = Gender(int.Parse(r["gender"].ToString()));
                    lbDob.Text = dob.ToString("dd/MM/yy");
                    int status = int.Parse(r["status"].ToString());
                    if (status == 0)
                    {
                        lbStatus.Text = "Disable";
                        lbStatus.ForeColor = Color.Red;
                    }
                    else if (status == 2)
                    {
                        lbStatus.Text = "Active";
                        lbStatus.ForeColor = Color.Green;
                    }
                    else
                    {
                        lbStatus.Text = "Pending";
                        lbStatus.ForeColor = Color.FromArgb(149, 117, 11) ;
                    }
                    lbContact.Text = r["contactnumber"].ToString();
                    break;
                }
                _background.PullData(id);
                foreach(DataRow r in _background.dt.Rows)
                {
                    lbFathername.Text = r["father_name"].ToString();
                    lbFatheroccupation.Text = r["father_occupation"].ToString() ;
                    lbMothername.Text = r["mother_name"].ToString();
                    lbMotherOccupation.Text = r["mother_occupation"].ToString();
                    lbParentcontact.Text = r["contact_number"].ToString();
                    lbAddress.Text  = r["current_address"].ToString();
                    break;
                }
            }
            catch (Exception ex) { MessageBox.Show("Something When Wrong : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }

        }
        private string Gender(int gender)
        {
            switch(gender)
            {
                case 1:
                    return "Male";
                    break;
                case 2:
                    return "Female";
                    break; 
                case 3:
                    return "Other";
                    break;
            }
            return "Error";
        }

        private void btnQr_Click(object sender, EventArgs e)
        {
            var qr = new QrCode(id);
            qr.ShowDialog();
        }
    }
}
