using DVLD_BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD_PresentationLayer.Applications.Local_License_Application
{
    public partial class ctrlShowLocalDrivingLicenseApplicationInfo : UserControl
    {
        public ctrlShowLocalDrivingLicenseApplicationInfo()
        {
            InitializeComponent();
        }

        clsLocalLicenseApplication LocalLicenseApplication;

        public bool Find(int ldlApplicationID)
        {
            LocalLicenseApplication = clsLocalLicenseApplication.Find(ldlApplicationID);
            if (LocalLicenseApplication != null)
            {
                _Load();
                return true;
            }
            else
            {
                MessageBox.Show("Cannot find a Local Driving License Application with Local Driving License Application ID: " + ldlApplicationID, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoadEmptyForm();
                return false;
            }

            
        }

        public void LoadEmptyForm()
        {
            ctrlShowApplicationInfo1.LoadEmptyForm();
            lblDLApplicationID.Text = "??";
            lblLicenseClass.Text = "??";
            lblPassedTest.Text = "??";
        }

            void _Load()
        {
            lblDLApplicationID.Text = LocalLicenseApplication.LocalDrivingLicenseApplicationID.ToString();
            lblLicenseClass.Text = LocalLicenseApplication.GetLicenseClassString();
            lblPassedTest.Text = LocalLicenseApplication.GetNumberOfPassedTests().ToString();
            ctrlShowApplicationInfo1.Find(LocalLicenseApplication.ID);
        }
    }
}
