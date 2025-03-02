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

namespace DVLD_PresentationLayer.Licenses
{
    public partial class frmIssueLicense : Form
    {
        clsLocalLicenseApplication _LocalDrivingLicenseApplication;
        public frmIssueLicense(int LocalDrivingLicenseApplicationID)
        {
            InitializeComponent();
            
            GetLocalDrivingLicenseApplication(LocalDrivingLicenseApplicationID);
        }

        void GetLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID)
        {
            if (LocalDrivingLicenseApplicationID == -1) this.Close();

            _LocalDrivingLicenseApplication = clsLocalLicenseApplication.Find(LocalDrivingLicenseApplicationID);
            if (_LocalDrivingLicenseApplication == null) this.Close();
        }


       



        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmAddLicense_FirstTime_Load(object sender, EventArgs e)
        {
            if(_LocalDrivingLicenseApplication.IsHaveLicense())
            {
                MessageBox.Show("This person already has a license from this class","",MessageBoxButtons.OK,MessageBoxIcon.Question);
                this.Close();
            }
            _LoadForm();
        }

        void _LoadForm()
        {
            ctrlShowLocalDrivingLicenseApplicationInfo1.Find(_LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID);
            lblLicenseFees.Text = clsLicenseClass.GetLicenseClassFees((int)_LocalDrivingLicenseApplication.LicenseClass).ToString();
        }

        private void btnIssue_Click(object sender, EventArgs e)
        {
            int LicenseID = -1;
            if( (LicenseID = _LocalDrivingLicenseApplication.IssueLicenseFirstTime(txtNotes.Text , clsGlobalSettings.UserID)) != -1)
            {
                btnIssue.Enabled = false;
                ctrlShowLocalDrivingLicenseApplicationInfo1.Find(_LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID);
                _LocalDrivingLicenseApplication.CompleteApplication();
                MessageBox.Show("License issued successfully with license id: "+LicenseID , "Issued" , MessageBoxButtons.OK, MessageBoxIcon.Information);
                

            }
            else
            {
                MessageBox.Show("Cannot issue license" , "Error" , MessageBoxButtons.OK,MessageBoxIcon.Error); 
            }
            
        }
    }
}
