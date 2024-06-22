using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
namespace OraclaMajorAssignmentY3S2
{
    public partial class Login : Form
    {
        Controller.myclass.UserController user = new Controller.myclass.UserController();
        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string username = tbUsername.Text.ToString();
                string password = tbPassword.Text.ToString();
                if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
                {
                    var result = user.login(username,password);
                    if(result == "1") 
                    {
                        var dash = new Dashboard();
                        dash.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("also no");
                    }
                }
                else { MessageBox.Show("no"); }
               
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
