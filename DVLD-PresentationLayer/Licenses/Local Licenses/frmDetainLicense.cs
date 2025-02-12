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
    public partial class frmDetainLicense : Form
    {

        clsLicense _License;
        public frmDetainLicense()
        {
            InitializeComponent();
        }


        void _LoadForm()
        {
            lblDetainDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            lblCreatedUser.Text = clsUser.Find(clsGlobalSettings.UserID).UserName;
        }

        private void frmDetainLicense_Load(object sender, EventArgs e)
        {
            _LoadForm();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        bool _CheckLicenseForDetain()
        {
            if(_License.IsDetained)
            {
                MessageBox.Show("Selected license is already detained" , "Not allowed" , 
                    MessageBoxButtons.OK,MessageBoxIcon.Error);
                return false;
            }

            if(!_License .IsActive)
            {
                MessageBox.Show("License is not active choose another one", "Not allowed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if(_License.IsLicenseExpired())
            {
                MessageBox.Show("License is expired choose another one", "Not allowed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void ctrlFilterLocalLicenses1_OccurFilterEvent(int obj)
        {
            _License = clsLicense.FindLicense(obj);
            if(_License != null)
            {
                lblLicenseID.Text = _License.LicenseID.ToString();
                llblShowLicenseHistory.Enabled = true;
                llblShowLicenseInfo.Enabled = true;
                btnDetain.Enabled = _CheckLicenseForDetain();
            }
            else
            {
                lblLicenseID.Text = "??";
                llblShowLicenseHistory.Enabled = false;
                llblShowLicenseInfo.Enabled = false;
                btnDetain.Enabled = false;
            }
        }

        private void btnDetain_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(mtxtFees.Text))
            {
                errorProvider1.SetError(mtxtFees,"Must enter fine fees");
                return;
            }
            else
            {
                errorProvider1.SetError(mtxtFees, "");
            }
            if(MessageBox.Show("Are you sure you want to detain this license" , "Confirm" , MessageBoxButtons.YesNo
                ,MessageBoxIcon.Question) == DialogResult.Yes)
            {
                int DetainLicenseID = _License.DetainLicense(int.Parse(mtxtFees.Text.Trim()), clsGlobalSettings.UserID);
                if (DetainLicenseID != -1)
                {
                    btnDetain.Enabled = false;
                    ctrlFilterLocalLicenses1.FilterEnabled = false;
                    lblDetainID.Text = DetainLicenseID.ToString();
                    MessageBox.Show("License Detained successfully with id: " + DetainLicenseID,
                        "License detained", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                }
                else
                {
                    MessageBox.Show("Failed to detain license" , "Error" , MessageBoxButtons.OK,MessageBoxIcon.Error);
                }

            }

            
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
    }
}
