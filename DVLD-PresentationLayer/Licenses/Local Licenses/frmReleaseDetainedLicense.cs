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

namespace DVLD_PresentationLayer.Licenses.Local_Licenses
{
    public partial class frmReleaseDetainedLicense : Form
    {

        clsApplication _Application;
        clsLicense _License;

        public frmReleaseDetainedLicense()
        {
            InitializeComponent();
            _Application = clsApplication.GetAddNewObject();
        }

        public frmReleaseDetainedLicense(int LicenseID)
        {
            InitializeComponent();
            _Application = clsApplication.GetAddNewObject();
            _License = clsLicense.FindLicense(LicenseID);

        }

        void _FillApplicationObject()
        {
            _Application .ApplicationType = clsApplication.enApplicationType.ReleaseDetainedDrivingLicense;
            _Application.CreateUserID = clsGlobalSettings.UserID; 
        }

        private void frmReplaceDetainedLicense_Load(object sender, EventArgs e)
        {
            if(_License!= null)
            {
                ctrlFilterLocalLicenses1.Find(_License.LicenseID);
                ctrlFilterLocalLicenses1.FilterEnabled = false;
            }
            _FillApplicationObject();
            _LoadForm();
        }

        bool _CheckForReleaseLicense()
        {
            
            if (!_License.IsDetained)
            {
                MessageBox.Show("Selected license is not detained", "Not allowed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!_License.IsActive)
            {
                MessageBox.Show("License is not active choose another one", "Not allowed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
            
        }

        void _LoadForm()
        {
            lblCreatedUser.Text = clsUser.Find(_Application.CreateUserID).UserName;
            lblAppFees.Text = _Application.PaidFees.ToString();
        }

        private void ctrlFilterLocalLicenses1_OccurFilterEvent(int obj)
        {
            _License = clsLicense.FindLicense(obj);
            if(_License != null)
            {
                clsDetainedLicense _DetainedLicense = clsDetainedLicense.FindByLicenseID(obj);
                if(_DetainedLicense != null)
                {
                    lblDetainID.Text = _DetainedLicense.DetainID.ToString();
                    lblDetainDate.Text = _DetainedLicense.DetainDate.ToString("dd/MM/yyyy");
                    lblFineFees.Text = _DetainedLicense.FineFees.ToString();
                    lblTotalFees.Text = (_Application.PaidFees + _DetainedLicense.FineFees).ToString();

                }

                else
                {
                    lblDetainID.Text = "??";
                    lblDetainDate.Text = "??";
                    lblFineFees.Text = "??";
                    lblTotalFees.Text = "??";
                }

                lblLicenseID.Text = _License.LicenseID.ToString();
                llblShowLicenseHistory.Enabled = true;
                llblShowLicenseInfo.Enabled = true;
                btnRelease.Enabled = _CheckForReleaseLicense();

            }
            else
            {
                lblDetainID.Text = "??";
                lblDetainDate.Text = "??";
                lblFineFees.Text = "??";
                lblLicenseID.Text = "??";
                lblTotalFees.Text = "??";
                llblShowLicenseHistory.Enabled = false;
                llblShowLicenseInfo.Enabled = false;
                btnRelease.Enabled = false;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void llblShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmManageLicenses frm = new frmManageLicenses(_License.DriverID);
            frm.ShowDialog();
        }

        private void llblShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmLocalLicenseInfo frm = new frmLocalLicenseInfo(_License.LicenseID);
            frm.ShowDialog();
        }

        private void btnRelease_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure you want to release license" , "Confirm" ,
                MessageBoxButtons.YesNo , MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _Application.PersonID = clsDriver.Find(_License.DriverID).PersonID;
                if(_Application.Save())
                {
                    if(_License.ReleaseDetainedLicense(_Application.CreateUserID , _Application.ID))
                    {
                        _Application.CompleteApplication();
                        lblApplicationID.Text = _Application.ID.ToString();
                        btnRelease.Enabled = false; 
                        ctrlFilterLocalLicenses1.Enabled = false;
                        MessageBox.Show("License released successfully", "License released"
                            , MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        _Application.CancelApplication();

                        MessageBox.Show("cannot release license", "Error"
                            , MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }
                else
                {
                    MessageBox.Show("cannot request application", "Error"
                            , MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}
