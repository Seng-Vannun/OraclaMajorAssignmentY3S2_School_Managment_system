using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bunifu.UI.WinForms;
using System.Windows.Media.Media3D;
using System.Collections;
using System.Windows.Controls;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;
using System.Net.NetworkInformation;
using System.Net;

namespace OraclaMajorAssignmentY3S2
{
    public partial class Edit : Form
    {
        Controller.myclass.StudentController _student = new Controller.myclass.StudentController();
        Controller.myclass.BackgroundController _background = new Controller.myclass.BackgroundController();
        int id;
        public Edit(int id)
        {
            InitializeComponent();
            FillComboBox();
            this.id = id;
            lbId.Text = id.ToString();
        }
        private void Edit_Load(object sender, EventArgs e)
        {
            try
            {
                _student.PullData(id);
                foreach (DataRow r in _student.dt.Rows)
                {
                    
                    tbFirstname.Text = r["first_name"].ToString();
                    tbLastname.Text = r["last_name"].ToString();
                    DateTime dob = DateTime.Parse(r["DOB"].ToString());
                    System.Drawing.Image img = _student.ConvertToImg(r["PROFILE_PICTURE"]);
                    if (img != null)
                    {
                        guna2PictureBox1.Image = img;
                    }
                    else
                    {
                        guna2PictureBox1.Image = System.Drawing.Image.FromFile("Photos//man.png");
                    }
                    cbGender.SelectedValue = int.Parse(r["gender"].ToString());
                    cbStatus.SelectedValue = int.Parse(r["status"].ToString());
                    dtpDob.Value = dob;
                    tbEmail.Text = r["email"].ToString();
                    tbContact.Text = r["contactnumber"].ToString();
                    break;
                }
                _background.PullData(id);
                foreach (DataRow r in _background.dt.Rows)
                {
                    tbFathername.Text = r["father_name"].ToString();
                    tbFatheroccupation.Text = r["father_occupation"].ToString();
                    tbMothername.Text = r["mother_name"].ToString();
                    tbMotheroccupation.Text = r["mother_occupation"].ToString();
                    tbParentcontact.Text = r["contact_number"].ToString();
                    tbAddress.Text = r["current_address"].ToString();
                    break;
                }
            }
            catch (Exception ex) { MessageBox.Show("Something When Wrong : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        private void btnNext_Click(object sender, EventArgs e)
        {
            InputTabControll.SelectedTab = InputTabControll.TabPages["familyTab"];
        }
        private void btnBack_Click(object sender, EventArgs e)
        {
            InputTabControll.SelectedTab = InputTabControll.TabPages["StudentTab"];

        }

        private void btnCancelFamilyTab_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCancelStudentTab_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnFromfile_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    // Set the filter to include only PNG, JPG and JPEG files.
                    openFileDialog.Filter = "Image files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg";
                    openFileDialog.Title = "Select an Image File";

                    // Optional: Prompt the user to select a file when the dialog opens
                    openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

                    // Show the dialog and check if the result is OK.
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Return the selected file name.
                        guna2PictureBox1.Image =System.Drawing.Image.FromFile(openFileDialog.FileName);
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Something When Wrong : "+ex.Message,"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void btnTakePhoto_Click(object sender, EventArgs e)
        {
            using (Camera cameraForm = new Camera())
            {
                if (cameraForm.ShowDialog() == DialogResult.OK)
                {
                    // Access the CapturedImage property after the dialog has been closed with OK result
                    System.Drawing.Image imageFromDialog = cameraForm.CapturedImage;

                    // Now you can use this image in your parent form, e.g., display it in a PictureBox or process it
                    guna2PictureBox1.Image = imageFromDialog;
                }
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {

           
                bool _validate = ValidateTextboxes();
                if (_validate)
                {
                    byte[] img = ConvertToBLOB(guna2PictureBox1.Image);
                    _student.FirstName = tbFirstname.Text.ToString();
                    _student.LastName = tbLastname.Text.ToString();
                    _student.Gender = cbGender.SelectedValue.ToString();
                    _student.ContactNumber = tbContact.Text.ToString();
                    _student.DateOfBirth = dtpDob.Value;
                    _student.Email = tbEmail.Text.ToString();
                    _student.ProfilePicture= img;
                    _student.Status = int.Parse(cbStatus.SelectedValue.ToString());
                    _background.Father_Name = tbFathername.Text.ToString();
                    _background.Father_Occupation = tbFatheroccupation.Text.ToString();
                    _background.Mother_Name = tbMothername.Text.ToString();
                    _background.Mother_Occupation = tbMotheroccupation.Text.ToString();
                    _background.Contact_Number = tbParentcontact.Text.ToString();
                    _background.Current_Address = tbAddress.Text.ToString();
                    _student.UpdateData(this.id);
                    _background.UpdateData(this.id);
                   
                this.DialogResult = DialogResult.OK;
                this.Close();
                }

           
        }
        private bool ValidateTextboxes()
        {
            bool allValid = true;
            StringBuilder errorMessage = new StringBuilder();

            // Use reflection to get all BunifuTextBox fields from the form
            var textBoxFields = this.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                                     .Where(f => f.FieldType == typeof(BunifuTextBox));

            foreach (var field in textBoxFields)
            {
                var textBox = field.GetValue(this) as BunifuTextBox;
                if (textBox == null || string.IsNullOrWhiteSpace(textBox.Text))
                {
                    // Construct field name by stripping the leading 'txt' or any other prefix you might have
                    string fieldName = field.Name.Substring(2);  // Adjust the index based on your actual field name prefixes
                    errorMessage.AppendLine($"Please fill out the {fieldName} field.");
                    textBox?.Focus();  // Focus only if textBox is not null
                    allValid = false;
                    break;  // or remove to check all fields
                }
            }
            if(cbGender.SelectedValue.ToString() == "0")
            {
                errorMessage.AppendLine($"Please Selected the Gender.");
                allValid = false;
            }
            if (!allValid)
            {
                MessageBox.Show(errorMessage.ToString(), "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return allValid;
            }
            else
            {
                return allValid;
            }
        }
        private byte[] ConvertToBLOB(System.Drawing.Image img)
        {
            if(img == null)
            {
                return null;
            }
            using (var ms = new System.IO.MemoryStream())
            {
                
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Png); // You can change your format to JPEG, etc.
                return ms.ToArray();
            }
        }
        public class ComboBoxItem
        {
            public int Id { get; set; }
            public string Value { get; set; }
        }
        private void FillComboBox()
        {
            List<ComboBoxItem> gender = new List<ComboBoxItem>
            {
            new ComboBoxItem { Id = 0, Value = "Select a Gender......" },
            new ComboBoxItem { Id = 1, Value = "Male" },
            new ComboBoxItem { Id = 2, Value = "Female" },
            new ComboBoxItem { Id = 3, Value = "Other" }
            };
            List<ComboBoxItem> status = new List<ComboBoxItem>
            {
            new ComboBoxItem { Id = 0, Value = "Disable" },
            new ComboBoxItem { Id = 1, Value = "Pending" },
            new ComboBoxItem { Id = 2, Value = "Active" },
            };

            cbGender.DataSource = gender;
            cbGender.DisplayMember = "Value";
            cbGender.ValueMember = "Id";
            // Set the ComboBox to show the placeholder
            cbGender.SelectedIndex = 0;
            cbStatus.DataSource = status;
            cbStatus.DisplayMember = "Value"; 
            cbStatus.ValueMember = "Id";
        }

        private void cbGender_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        
    }
}
