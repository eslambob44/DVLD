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

namespace DVLD_PresentationLayer.Licenses.International_Licenses
{
    public partial class frmIssueInternationalLicense : Form
    {
        public frmIssueInternationalLicense()
        {
            InitializeComponent();
        }

        public frmIssueInternationalLicense(int LicenseID)
        {
            InitializeComponent();
            ctrlFilterLocalLicenses1.Find(LicenseID);
        }

        clsApplication _InternationalLicenseApplication = clsApplication.GetAddNewObject();
        clsInternationalLicense _InternationalLicense = clsInternationalLicense.GetAddNewObject();
        clsLicense _LocalLicense;

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void _FillApplicationObject()
        {
            _InternationalLicenseApplication.ApplicationType = clsApplication.enApplicationType.NewInternationalLicense;
            _InternationalLicenseApplication.CreateUserID = clsGlobalSettings.UserID;
        }

        void _LoadForm()
        {
            lblApplicationDate.Text = _InternationalLicenseApplication.ApplicationDate.ToString("dd/MM/yyyy");
            lblCreatedUser.Text = clsUser.Find(_InternationalLicenseApplication.CreateUserID).UserName;
            lblFees.Text = _InternationalLicenseApplication.PaidFees.ToString();
            lblIssueDate.Text = _InternationalLicense.IssueDate.ToString("dd/MM/yyyy");
            lblExpirationDateID.Text = _InternationalLicense.ExpirationDate.ToString("dd/MM/yyyy");
            
        }

        private void frmIssueInternationalLicense_Load(object sender, EventArgs e)
        {
            _FillApplicationObject();
            _LoadForm();
        }

        bool CheckIfDriverHasAnActiveInternationalLicense()
        {
            clsDriver Driver = clsDriver.Find(_LocalLicense.DriverID);
            int DriverInternationalLicenseID = Driver.GetActiveInternationalLicenseID();
            if (DriverInternationalLicenseID != -1)
            {
                MessageBox.Show("Driver already has an active international license with ID: " + DriverInternationalLicenseID
                   , "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        

        bool CheckIfCanIssueInternationalLicense()
        {
            if(!CheckIfDriverHasAnActiveInternationalLicense()) return false;   

            if(!_LocalLicense.IsActive)
            {
                MessageBox.Show("Cannot issue an international license with locked license" , "Error"
                        ,MessageBoxButtons.OK,MessageBoxIcon.Error);    
                return false;
            }
            if(_LocalLicense.IsDetained)
            {
                MessageBox.Show("Cannot issue an international license with Detained license", "Error"
                        , MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if(_LocalLicense.ExpirationDate < DateTime.Now)
            {
                MessageBox.Show("Cannot issue an international license with Expired license", "Error"
                        , MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if(_LocalLicense.LicenseClass != clsLicenseClass.enLicenseClass.OrdinaryDriving)
            {
                MessageBox.Show("Cannot issue an international license with non ordinary license", "Error"
                        , MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void ctrlFilterLocalLicenses1_OccurFilterEvent(int obj)
        {
            if(obj != -1)
            {
                _LocalLicense = clsLicense.FindLicense(obj);
                llblShowLicenseHistory.Enabled = true;
                btnIssue.Enabled = CheckIfCanIssueInternationalLicense();
                lblLocalLicenseID.Text = _LocalLicense.LicenseID.ToString();
            }
            else
            {
                _LocalLicense = null;
                llblShowLicenseHistory.Enabled = false;
                btnIssue.Enabled = false;
                lblLocalLicenseID.Text = "??";
            }
        }

        private void btnIssue_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure you want to issue international license" , "Confirm" 
                ,MessageBoxButtons.YesNo , MessageBoxIcon.Question) == DialogResult.Yes)
            {
                clsDriver Driver = clsDriver.Find(_LocalLicense.DriverID);
                _InternationalLicenseApplication.PersonID = Driver.PersonID;
                _InternationalLicense.CreatedUserID = clsGlobalSettings.UserID;
                _InternationalLicense.DriverID = Driver.DriverID;
                _InternationalLicense.LocalLicenseID = _LocalLicense.LicenseID;
                _InternationalLicense.IsActive = true;
                if(_InternationalLicenseApplication.Save()) 
                {
                    _InternationalLicense.ApplicationID = _InternationalLicenseApplication.ID;
                    if (_InternationalLicense.Save())
                    {
                        btnIssue.Enabled = false;
                        llblShowLicenseInfo.Enabled = true;
                        MessageBox.Show("License Issued Successfully" , "License Issued"
                            ,MessageBoxButtons.OK,MessageBoxIcon.Information );
                        _InternationalLicenseApplication.CompleteApplication();
                        lblInternationalLicenseID.Text = _InternationalLicense.InternationalLicenseID.ToString();
                        lblApplicationID.Text = _InternationalLicenseApplication.ID.ToString();
                        ctrlFilterLocalLicenses1.FilterEnabled = false;
                    }
                    else
                    {
                        MessageBox.Show("Cannot issue license", "Error"
                            , MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                
                }
                else
                {
                    MessageBox.Show("Cannot request an application", "Error"
                            , MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
              
            
        }

        private void llblShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmManageLicenses frm = new frmManageLicenses(_LocalLicense.DriverID);
            frm.ShowDialog();
        }

        private void llblShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmInternationalLicenseInfo frm = new frmInternationalLicenseInfo(_InternationalLicense.InternationalLicenseID);
            frm.ShowDialog();
        }
    }
}
