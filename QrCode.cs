using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;
namespace OraclaMajorAssignmentY3S2
{
    public partial class QrCode : Form
    {
        int id;
        public QrCode(int id)
        {
            InitializeComponent();
            this.id = id;
        }

        private void QrCode_Load(object sender, EventArgs e)
        {
            var qrcode = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new ZXing.Common.EncodingOptions
                {
                    Width = 294,
                    Height = 275
          
                }

            };
            Bitmap qrImg = qrcode.Write("http://172.16.6.127:7265/api/Student/" + id.ToString());
            guna2PictureBox1.Image = qrImg;
        }
    }
}
