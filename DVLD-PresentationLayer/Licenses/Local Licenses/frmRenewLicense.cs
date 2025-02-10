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
    public partial class frmRenewLicense : Form
    {

        clsApplication _Application = clsApplication.GetAddNewObject();
        clsLicense _OldLicense;
        int _NewLicenseID;
        
        public frmRenewLicense()
        {
            InitializeComponent();
        }

        void _FillApplicationObject()
        {
            _Application.ApplicationType = clsApplication.enApplicationType.RenewDrivingLicenseService;
            _Application.CreateUserID = clsGlobalSettings.UserID;
        }

        private void frmRenewLicense_Load(object sender, EventArgs e)
        {
            _FillApplicationObject();
            _LoadForm();
        }

        void _LoadForm()
        {
            lblApplicationDate.Text = _Application.ApplicationDate.ToString("dd/MM/yyyy");
            lblIssueDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            lblAppFees.Text = _Application.PaidFees.ToString();
            clsUser User = clsUser.Find(clsGlobalSettings.UserID);
            lblCreatedUser.Text = User.UserName;
        }

        bool _IsLicenseCanRenewed(clsLicense License)
        {
            if(!License.IsLicenseExpired())
            {
                MessageBox.Show("license is not expired yet cannot Renewed!", "Valid license", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if(!License.IsActive)
            {
                MessageBox.Show("license is not active cannot Renewed!","Inactive license" , MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if(License.IsDetained)
            {
                MessageBox.Show("license is detained cannot Renewed!", "Detained license", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void ctrlFilterLocalLicenses1_OccurFilterEvent(int obj)
        {
            
            _OldLicense = clsLicense.FindLicense(obj);
            if ( _OldLicense != null)
            {
                llblShowLicenseHistory.Enabled = true;
                float LicenseFees = clsLicenseClass.GetLicenseClassFees((int)_OldLicense.LicenseClass);
                lblLicenseFees.Text = LicenseFees.ToString();
                lblTotalFees.Text = (LicenseFees + _Application.PaidFees).ToString();
                lblExpirationDateID.Text = DateTime.Now.AddYears
                    (clsLicenseClass.GetDefaultValidityLength((int)_OldLicense.LicenseClass)).ToString("dd/MM/yyyy");
                lblOldLicenseID.Text = _OldLicense.LicenseID.ToString();
                _Application.PersonID = clsDriver.Find(_OldLicense.DriverID).PersonID;
                btnRenew.Enabled = _IsLicenseCanRenewed(_OldLicense);
                
            }
            else
            {


                llblShowLicenseHistory.Enabled = false;
                lblLicenseFees.Text = "??";
                lblTotalFees.Text = "??";
                lblExpirationDateID.Text = "??";
                lblOldLicenseID.Text = "??";
                btnRenew.Enabled = false;
                _Application.PersonID = -1;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void llblShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            int DriverID = _OldLicense.DriverID;
            frmManageLicenses frm = new frmManageLicenses(DriverID);
            frm.ShowDialog();
        }

        private void btnIssue_Click(object sender, EventArgs e)
        {
            
            if(MessageBox.Show("Are you sure you want to renew the license" , "Confirm" , 
                MessageBoxButtons.YesNo , MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if(_Application.Save())
                {
                    _NewLicenseID = _OldLicense.RenewLicense(_Application.ID , clsGlobalSettings.UserID , txtNotes.Text);
                    if(_NewLicenseID != -1)
                    {
                        lblApplicationID.Text = _Application.ID.ToString();
                        lblRenewdLicenseID.Text = _NewLicenseID.ToString();
                        btnRenew.Enabled = false;
                        ctrlFilterLocalLicenses1.FilterEnabled = false;
                        llblShowLicenseInfo.Enabled = true;
                        MessageBox.Show("License renewed successfully with id: "+ _NewLicenseID,
                            "License Issued" , MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ctrlFilterLocalLicenses1.Find(_OldLicense.LicenseID);
                        txtNotes.Enabled = false;

                    }
                    else
                    {
                        MessageBox.Show("Failed to renew license", "Renew license failed",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Cannot request application" , "Application failed" ,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void llblShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmLocalLicenseInfo frm = new frmLocalLicenseInfo(_NewLicenseID);
            frm.ShowDialog();
        }
    }
}
