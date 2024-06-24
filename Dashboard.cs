using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace OraclaMajorAssignmentY3S2
{
    public partial class Dashboard : Form
    {
        protected string HoverButton;
        public Dashboard()
        {
            InitializeComponent();
        }
        private void Dashboard_Load(object sender, EventArgs e)
        {
            container(new Homepage());
            menuStrip('1');
            HoverButton = "home";
        }
        private void HomepageMenuButton_Click(object sender, EventArgs e)
        {
            if (HoverButton != "home")
            {
                unHoverBtn(HoverButton);
                container(new Homepage());
                menuStrip('1');
                HoverButton = "home";
            }

        }
        private void btnDashboard_Click(object sender, EventArgs e)
        {
            if (HoverButton != "dashboard")
            {
                unHoverBtn(HoverButton);
                container(new DataGrid());
                menuStrip('2');
                HoverButton = "dashboard";
            }
        }
        private void btnreport_Click(object sender, EventArgs e)
        {
            if (HoverButton != "report")
            {
                unHoverBtn(HoverButton);
                container(new Report());
                menuStrip('3');
                HoverButton = "report";
            }
        }
        private void container(object _form)
        {
            if (ContainerPanel.Controls.Count > 0) ContainerPanel.Controls.Clear();
            Form _fm = _form as Form;
            _fm.TopLevel= false;
            _fm.FormBorderStyle = FormBorderStyle.None;
            _fm.Dock= DockStyle.Fill;
            ContainerPanel.Controls.Add(_fm);
            ContainerPanel.Tag= _fm;
            _fm.Show();
        }

        private void menuStrip(char _id)
        {
            switch (_id) 
            {
                case '1':
                    lbDashTilte.Text = "Homepage";
                    pbDashTilte.Image = Properties.Resources.homepage;
                    HomepageMenuButton.Image = Properties.Resources.home__1_;
                    HomepageMenuButton.FillColor = Color.FromArgb(4, 16, 36);
                    HomepageMenuButton.ForeColor = Color.FromArgb(130, 133, 236);
                    break;
                case '2':
                    lbDashTilte.Text = "Dashboard";
                    pbDashTilte.Image = Properties.Resources.visual_data;
                    btnDashboard.Image = Properties.Resources.dashboard__1_;
                    btnDashboard.FillColor = Color.FromArgb(4, 16, 36);
                    btnDashboard.ForeColor = Color.FromArgb(130, 133, 236);
                    break;
                case '3':
                    lbDashTilte.Text = "Report";
                    pbDashTilte.Image = Properties.Resources.error;
                    btnreport.Image = Properties.Resources.report__2_;
                    btnreport.FillColor = Color.FromArgb(4, 16, 36);
                    btnreport.ForeColor = Color.FromArgb(130, 133, 236);
                    break;
            }
        }
        private void unHoverBtn(string hover)
        {
            if(hover == "home")
            {
                HomepageMenuButton.Image = Properties.Resources.home;
                HomepageMenuButton.FillColor = Color.FromArgb(16, 85, 171);
            }
            else if (hover == "dashboard")
            {
                btnDashboard.Image = Properties.Resources.dashboard;
                btnDashboard.FillColor = Color.FromArgb(16, 85, 171);
             
            }
            else if (hover == "report")
            {
                btnreport.Image = Properties.Resources.report__1_;
                btnreport.FillColor = Color.FromArgb(16, 85, 171);

            }
        }

       
    }
}
