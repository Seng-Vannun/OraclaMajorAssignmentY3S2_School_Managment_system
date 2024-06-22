using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
namespace OraclaMajorAssignmentY3S2
{
    public partial class Camera : Form
    {
        Capture _capture;
        public Image CapturedImage { get; private set; }
        public Camera()
        {
            InitializeComponent();
        }
        private void Camera_Load(object sender, EventArgs e)
        {
            try
            {
                btnRetake.Visible= false;
                _capture = new Capture();
                Application.Idle += streaming;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something Went Wrong : " + ex.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
        private void btnCancelStudentTab_Click(object sender, EventArgs e)
        {
            Application.Idle -= streaming;
            _capture.Dispose();
            _capture = null;
            this.Close();
            
        }

        private void streaming(object sender, EventArgs e)
        {
            try
            {
                using (var img = _capture.QueryFrame())
                {
                    
                    if (img != null)
                    {
                        using (var processedImage = img.ToImage<Bgr, byte>())
                        {
                            guna2PictureBox1.Image?.Dispose();  // Dispose the previous image if it exists
                            guna2PictureBox1.Image = processedImage.Bitmap;
                        }
                    }
                }
            }

            catch (Exception ex) { 
                MessageBox.Show("Something Went Wrong : " + ex.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Error); 
                Application.Idle += streaming; }
        }
            private void btnCapture_Click(object sender, EventArgs e)
        {
            Application.Idle -= streaming;
            guna2PictureBox1.Image = guna2PictureBox1.Image;
            btnRetake.Visible = true;
        }

        private void btnRetake_Click(object sender, EventArgs e)
        {
            try
            {               
                _capture.Dispose();
                _capture = null;
                _capture = new Capture();
                Application.Idle += streaming;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something Went Wrong : " + ex.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Application.Idle -= streaming;
            CapturedImage = guna2PictureBox1.Image;
            this.DialogResult = DialogResult.OK;
            if (_capture != null)
            {
                _capture.Dispose();
                _capture = null;
            }
            this.Close();

        }

    }
}
