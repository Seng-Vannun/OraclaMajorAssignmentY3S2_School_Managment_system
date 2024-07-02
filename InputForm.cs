using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

namespace OraclaMajorAssignmentY3S2
{
    public partial class InputForm : Form
    {
        Controller.myclass.StudentController _studentController = new Controller.myclass.StudentController();
        Controller.myclass.BackgroundController _backgroundController = new Controller.myclass.BackgroundController();
        public InputForm()
        {
            InitializeComponent();
            FillComboBox();

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

            try
            {
                bool _validate = ValidateTextboxes();
                if (_validate)
                {
                    byte[] img = ConvertToBLOB(guna2PictureBox1.Image);
                    _studentController.FirstName = tbFirstname.Text.ToString();
                    _studentController.LastName = tbLastname.Text.ToString();
                    _studentController.Gender = cbGender.SelectedValue.ToString();
                    _studentController.ContactNumber = tbContact.Text.ToString();
                    _studentController.DateOfBirth = dtpDob.Value;
                    _studentController.Email = tbEmail.Text.ToString();
                    _studentController.ProfilePicture= img;
                    _backgroundController.Father_Name = tbFathername.Text.ToString();
                    _backgroundController.Father_Occupation = tbFatheroccupation.Text.ToString();
                    _backgroundController.Mother_Name = tbMothername.Text.ToString();
                    _backgroundController.Mother_Occupation = tbMotheroccupation.Text.ToString();
                    _backgroundController.Contact_Number = tbParentcontact.Text.ToString();
                    _backgroundController.Current_Address = tbAddress.Text.ToString();
                    int id =_studentController.InsertData();
                    _backgroundController.InsertData(id);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }

            }catch(Exception ex)
            {
                MessageBox.Show("Some Thing Went Wrong \n Exception: " + ex.Message);
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
            List<ComboBoxItem> items = new List<ComboBoxItem>
            {
            new ComboBoxItem { Id = 0, Value = "Select a Gender......" },
            new ComboBoxItem { Id = 1, Value = "Male" },
            new ComboBoxItem { Id = 2, Value = "Female" },
            new ComboBoxItem { Id = 3, Value = "Other" }
            };
            
            cbGender.DataSource = items;
            cbGender.DisplayMember = "Value";
            cbGender.ValueMember = "Id";
            // Set the ComboBox to show the placeholder
            cbGender.SelectedIndex = 0;
        }

        private void cbGender_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
