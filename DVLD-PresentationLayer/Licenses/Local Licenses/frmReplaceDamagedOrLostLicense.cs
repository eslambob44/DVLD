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
    public partial class frmReplaceDamagedOrLostLicense : Form
    {
        public frmReplaceDamagedOrLostLicense()
        {
            InitializeComponent();
        }

        clsApplication _Application = clsApplication.GetAddNewObject();
        clsLicense _License;
        int _NewLicenseID;
        void _FillApplicationObject()
        {
            _Application.ApplicationType = clsApplication.enApplicationType.ReplacementForADamagedDrivingLicense;
            _Application.CreateUserID = clsGlobalSettings.UserID;
        }

        void _LoadForm()
        {
            lblApplicationDate.Text = _Application.ApplicationDate.ToString("dd/MM/yyyy");
            lblCreatedUser.Text = clsUser.Find(clsGlobalSettings.UserID).UserName;
            lblAppFees.Text = _Application.PaidFees.ToString();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        bool _CheckForValidLicense(clsLicense License)
        {


            if (License.IsLicenseExpired())
            {
                MessageBox.Show("license is not expired yet cannot replaced!", "Valid license", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!License.IsActive)
            {
                MessageBox.Show("license is not active cannot replaced!", "Inactive license", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (License.IsDetained)
            {
                MessageBox.Show("license is detained cannot replaced!", "Detained license", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void rbLostLicense_CheckedChanged(object sender, EventArgs e)
        {
            if (rbLostLicense.Checked) _Application.ApplicationType = clsApplication.enApplicationType.ReplacementForaLostDrivingLicense;
            else if (rbDamagedLicense.Checked) _Application.ApplicationType = clsApplication.enApplicationType.ReplacementForADamagedDrivingLicense;

            lblAppFees.Text = _Application.PaidFees.ToString();
        }

        private void ctrlFilterLocalLicenses1_OccurFilterEvent(int obj)
        {
            if(obj != -1)
            {
                _License = clsLicense.FindLicense(obj);

                llblShowLicenseHistory.Enabled = true;
                lblOldLicenseID.Text = obj.ToString();
                btnReplace.Enabled = _CheckForValidLicense(_License);
                _Application.PersonID = clsDriver.Find(_License.DriverID).PersonID;
            }
            else
            {
                _License = null;
                llblShowLicenseHistory.Enabled= false;
                lblOldLicenseID.Text = "??";
                btnReplace.Enabled = false;
            }

        }

        private void btnReplace_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to replace the license", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if(_Application.Save())
                {
                    _NewLicenseID = (_Application.ApplicationType == clsApplication.enApplicationType.ReplacementForADamagedDrivingLicense)?
                        _License.ReplaceLicenseForDamaged(_Application.ID , clsGlobalSettings.UserID):
                        _License.ReplaceLicenseForLost(_Application.ID , clsGlobalSettings.UserID);
                    if(_NewLicenseID != -1)
                    {
                        lblApplicationID.Text = _Application.ID.ToString(); 
                        lblReplacedLicenseID.Text = _NewLicenseID.ToString();
                        llblShowLicenseInfo.Enabled = true;
                        btnReplace.Enabled = false;
                        ctrlFilterLocalLicenses1.FilterEnabled= false;
                        gbReplacementFor.Enabled = false;
                        MessageBox.Show("License replaced successfully with id: " + _NewLicenseID,
                            "License Issued", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ctrlFilterLocalLicenses1.Find(_License.LicenseID);
                    }
                    else
                    {
                        MessageBox.Show("Failed to replace license", "Replace license failed",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Cannot request application", "Application failed",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            }

        private void llblShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmLocalLicenseInfo frm = new frmLocalLicenseInfo(_NewLicenseID);
            frm.ShowDialog();   
        }

        private void llblShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmManageLicenses frm = new frmManageLicenses(_License.DriverID);
            frm.ShowDialog();
        }

        private void frmReplaceDamagedOrLostLicense_Load(object sender, EventArgs e)
        {
            _FillApplicationObject();
            _LoadForm();
        }
    }
    }

