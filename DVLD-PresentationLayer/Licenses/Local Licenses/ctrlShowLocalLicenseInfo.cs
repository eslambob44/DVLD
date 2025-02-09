using DVLD_BusinessLayer;
using DVLD_PresentationLayer.Properties;
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
    public partial class ctrlShowLocalLicenseInfo : UserControl
    {

        clsLicense _License;
        clsDriver _Driver;
        clsPerson _Person;
        public ctrlShowLocalLicenseInfo()
        {
            InitializeComponent();
            
        }

        public bool Find(int LicenseID)
        {



            bool IsFound = _FindLicense(LicenseID);
            if (IsFound)
            {
                _Driver = clsDriver.Find(_License.DriverID);
                _Person = clsPerson.Find(_Driver.PersonID);
            }
            if (!IsFound)
            {
                MessageBox.Show("License with license id: " + LicenseID + " not found", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _LoadEmptyForm();
                return false;
            }
            else
            {
                _LoadForm();
                return true;
            }
        }

        public bool FindByApplicationID(int ApplicationID)
        {
            clsLicense License = clsLicense.FindLicenseByApplication(ApplicationID);
            if (License != null) return Find(License.LicenseID);
            else return Find(-1);
        }

        bool _FindLicense(int LicenseID)
        {
            _License = clsLicense.FindLicense(LicenseID);
            if( _License == null ) 
            {
                return false;
            }
            return true;
        }

        private void ctrlShowLicenseInfo_Load(object sender, EventArgs e)
        {
            
        }


        void _LoadEmptyForm()
        {
            lblClass.Text = "??";
            lblFullName.Text = "??";
            lblLicenseID.Text = "??";
            lblNationalNumber.Text = "??";
            lblGendor.Text = "??";
            lblIssueDate.Text = "??";
            lblIssueReason.Text = "??";
            lblNotes.Text = "??";
            lblIsActive.Text = "??";
            lblDateOfBirth.Text = "??";
            lblDriverID.Text = "??";
            lblExpirationDate.Text = "??";
            lblIsDetained.Text = "??";
            pbGendor.Image =  Resources.Man_32;
            ctrlUserImage1.SetImage("", clsPerson.enGendor.Male);
        }

        void _LoadForm()
        {
            lblClass.Text = clsLocalLicenseApplication.GetLicenseClassString(_License.LicenseClass);
            lblFullName.Text = _Person.FullName;
            lblLicenseID.Text = _License.LicenseID.ToString();
            lblNationalNumber.Text = _Person.NationalNumber;
            lblGendor.Text = _Person.GendorString;
            lblIssueDate.Text = _License.IssueDate.ToString("dd/MM/yyyy");
            lblIssueReason.Text = _License.IssueReason.ToString();
            lblNotes.Text = (string.IsNullOrEmpty(_License.Notes))?"No Notes":_License.Notes;
            lblIsActive.Text = (_License.IsActive) ? "Yes" : "No";
            lblDateOfBirth.Text = _Person.DateOfBirth.ToString("dd/MM/yyyy");
            lblDriverID.Text = _Driver.DriverID.ToString();
            lblExpirationDate.Text = _License.ExpirationDate.ToString("dd/MM/yyyy");
            lblIsDetained.Text = (_License.IsDetained) ? "Yes" : "No";
            pbGendor.Image = (_Person.Gendor==clsPerson.enGendor.Male)?Resources.Man_32:Resources.Woman_32;
            ctrlUserImage1.SetImage(_Person.ImagePath, _Person.Gendor);
        }
    }
}
