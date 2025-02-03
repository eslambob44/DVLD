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
        clsDriver _Driver;
        clsLicense _License = clsLicense.GetAddNewObject();
        public frmIssueLicense(int LocalDrivingLicenseApplicationID)
        {
            InitializeComponent();
            
            GetLocalDrivingLicenseApplication(LocalDrivingLicenseApplicationID);
            GetDriver(_LocalDrivingLicenseApplication.PersonID);
        }

        void GetLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID)
        {
            if (LocalDrivingLicenseApplicationID == -1) this.Close();

            _LocalDrivingLicenseApplication = clsLocalLicenseApplication.Find(LocalDrivingLicenseApplicationID);
            if (_LocalDrivingLicenseApplication == null) this.Close();
        }


        void GetDriver(int PersonID)
        {
            if (PersonID == -1) this.Close();

            if (clsDriver.IsPersonADriver(PersonID))
            {
                _Driver = clsDriver.FindByPersonID(PersonID); 
            }
            else
            {
                _Driver = clsDriver.GetAddNewObject();
                _Driver.PersonID = PersonID;
                _Driver.CreatedUserID = clsGlobalSettings.UserID;
                if(!_Driver.Save()) this.Close();
            }
        }



        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmAddLicense_FirstTime_Load(object sender, EventArgs e)
        {
            if(_Driver.IsDriverHasAnActiveLicense(_LocalDrivingLicenseApplication.LicenseClass))
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
            _License.ApplicationID = _LocalDrivingLicenseApplication.ID;
            _License.CreatedUserID = clsGlobalSettings.UserID ;
            _License.DriverID = _Driver.DriverID;
            _License.IssueReason = clsLicense.enIssueReason.FirstTime;
            _License.LicenseClass = _LocalDrivingLicenseApplication.LicenseClass;
            _License.Notes = txtNotes.Text;
            if(_License.Save())
            {
                btnIssue.Enabled = false;
                ctrlShowLocalDrivingLicenseApplicationInfo1.Find(_LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID);
                _LocalDrivingLicenseApplication.CompleteApplication();
                MessageBox.Show("License issued successfully with license id: "+_License.LicenseID , "Issued" , MessageBoxButtons.OK, MessageBoxIcon.Information);
                

            }
            else
            {
                MessageBox.Show("Cannot issue license" , "Error" , MessageBoxButtons.OK,MessageBoxIcon.Error); 
            }
            
        }
    }
}
