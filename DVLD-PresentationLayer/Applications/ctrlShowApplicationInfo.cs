using DVLD_BusinessLayer;
using DVLD_PresentationLayer.People;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_PresentationLayer.Applications
{
    public partial class ctrlShowApplicationInfo : UserControl
    {
        clsApplication Application;

        public ctrlShowApplicationInfo()
        {
            InitializeComponent();
        }

        public void Find(int ApplicationID)
        {
            Application = clsApplication.Find(ApplicationID);
            if(Application != null)
            {
                _Load();
            }
            else
            {
                MessageBox.Show("Cannot find an application with Application ID: "+ ApplicationID,"" , MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        string _GetApplicantFullName(int PersonID)
        {
            clsPerson Person = clsPerson.Find(PersonID);
            if (Person != null)
            {
                return Person.FullName;
            }
            else return null;
        }

        string _GetCreatedUserName(int UserID)
        {
            clsUser User = clsUser.Find(UserID);
            if (User != null)
            {
                return User.UserName;
            }
            else return null;
        }

        void _Load()
        {
            lblApplicationID.Text = Application.ID.ToString();
            lblApplicationStatus.Text = Application.ApplicationStatus.ToString();
            lblPaidFees.Text = Application.PaidFees.ToString();
            lblApplicantFullName.Text = _GetApplicantFullName(Application.PersonID);
            lblApplicationType.Text = Application.GetApplicationTypeString();
            lblApplicationDate.Text = Application.ApplicationDate.ToString("dd/MM/yyyy");
            lblStatusDate.Text = Application.LastStatusDate.ToString("dd/MM/yyyy");
            lblCreatedUserName.Text = _GetCreatedUserName(Application.CreateUserID);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if(Application != null)
            {
                frmPersonInfo frm = new frmPersonInfo(Application.PersonID);
                frm.ShowDialog();
            }
        }
    }
}
