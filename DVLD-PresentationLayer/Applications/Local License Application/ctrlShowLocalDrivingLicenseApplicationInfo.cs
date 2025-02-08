using DVLD_BusinessLayer;
using DVLD_PresentationLayer.Licenses;
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
        
        int GetLicenseID()
        {
            int LicenseID = -1;
            clsLicense license = clsLicense.FindLicenseByApplication(LocalLicenseApplication.ID);
            if(license != null) 
            {
                LicenseID  = license.LicenseID;
            }
            return LicenseID;

        }
        

        void _Load()
        {
            lblDLApplicationID.Text = LocalLicenseApplication.LocalDrivingLicenseApplicationID.ToString();
            lblLicenseClass.Text = LocalLicenseApplication.GetLicenseClassString();
            lblPassedTest.Text = LocalLicenseApplication.GetNumberOfPassedTests().ToString();
            ctrlShowApplicationInfo1.Find(LocalLicenseApplication.ID);
            if(GetLicenseID() != -1) lblViewLicenseInfo.Enabled = true;
        }

        private void lblViewLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            int LicenseID = GetLicenseID();
            if(LicenseID != -1)
            {
                frmLicenseInfo frm = new frmLicenseInfo(LicenseID);
                frm.ShowDialog();
            }
        }
    }
}
