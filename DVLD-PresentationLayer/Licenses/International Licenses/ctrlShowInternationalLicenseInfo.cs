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

namespace DVLD_PresentationLayer.Licenses.International_Licenses
{
    public partial class ctrlShowInternationalLicenseInfo : UserControl
    {
        public ctrlShowInternationalLicenseInfo()
        {
            InitializeComponent();
        }
        clsPerson _Person;
        clsInternationalLicense _InternationalLicense;

        public bool Find(int InternationalLicenseID)
        {
            _InternationalLicense = clsInternationalLicense.Find(InternationalLicenseID);
            if( _InternationalLicense != null )
            {
                _Person = clsPerson.Find(clsDriver.Find(_InternationalLicense.DriverID).PersonID);
                _LoadForm();
                return true;
            }
            else
            {
                MessageBox.Show("Cannot find international license with id: "+InternationalLicenseID
                    ,"Invalid ID" , MessageBoxButtons.OK, MessageBoxIcon.Error );
                _LoadEmptyForm();
                return false;
            }

        }

        void _LoadEmptyForm()
        {
            lblFullName.Text = "??";
            lblInternationalLicenseID.Text = "??";
            lblLocalLicenseID.Text = "??";
            lblDriverID.Text = "??";
            lblNationalNumber.Text = "??";
            lblIssueDate.Text = "??";
            lblApplicationID.Text = "??";
            lblIsActive.Text = "??";
            lblDateOfBirth.Text = "??";
            lblExpirationDate.Text = "??";
            ctrlUserImage1.SetImage("", clsPerson.enGendor.Male);
            pbGendor.Image = Resources.Man_32;
            lblGendor.Text = "??";
        }

        void _LoadForm()
        {
            lblFullName.Text = _Person.FullName;
            lblInternationalLicenseID.Text = _InternationalLicense.InternationalLicenseID.ToString();
            lblLocalLicenseID.Text = _InternationalLicense.LocalLicenseID.ToString();
            lblDriverID.Text = _InternationalLicense.DriverID.ToString();
            lblNationalNumber.Text = _Person.NationalNumber;
            lblIssueDate.Text = _InternationalLicense.IssueDate.ToString("dd/MM/yyyy");
            lblApplicationID.Text = _InternationalLicense.ApplicationID.ToString();
            lblIsActive.Text = (_InternationalLicense.IsActive) ? "Yes" : "No";
            lblDateOfBirth.Text = _Person.DateOfBirth.ToString("dd/MM/yyyy");
            lblExpirationDate.Text = _InternationalLicense.ExpirationDate.ToString("dd/MM/yyyy");
            ctrlUserImage1.SetImage(_Person.ImagePath , _Person.Gendor);
            pbGendor.Image = (_Person.Gendor == clsPerson.enGendor.Male) ? Resources.Man_32 : Resources.Female_512;
            lblGendor.Text = _Person.Gendor.ToString();
        }
    }
}
