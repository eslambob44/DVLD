using DVLD_BusinessLayer;
using DVLD_PresentationLayer.People;
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

namespace DVLD_PresentationLayer
{
    public partial class ctrlShowPersonInfo : UserControl
    {
        clsPerson _Person;
        public ctrlShowPersonInfo()
        {
            InitializeComponent();
            
        }

        public int PersonID { get; set; } = -1;

        public void Find(int PersonID)
        {
            _Person = clsPerson.Find(PersonID);
            if (_Person == null)
            {
                LoadEmptyForm();
                MessageBox.Show("The person not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                _LoadForm();
                this.PersonID = PersonID;
            }
        }

        public void LoadEmptyForm()
        {
            lblPersonIDval.Text = "??";
            lblFullNameval.Text = "??";
            lblEmailval.Text = "??";
            lblPhoneval.Text = "??";
            lblGendorval.Text = "??";
            lblNationalNoval.Text = "??";
            lblDateOfBirthval.Text = "??";
            lblCountryval.Text = "??";
            lblAddressval.Text = "??";
            ctrlUserImage1.SetImage(null, clsPerson.enGendor.Male);
            linkLabel1.Visible = false;
        }

        public void Find(string NationalNo)
        {
            _Person = clsPerson.Find(NationalNo);
            if (_Person == null)
            {
                LoadEmptyForm();
                MessageBox.Show("The person not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                this.PersonID = _Person.ID;
                _LoadForm();
            }
        }
        private void _LoadForm()
        {
            linkLabel1.Visible=true;
            lblPersonIDval.Text = _Person.ID.ToString();
            lblFullNameval.Text = _Person.FullName;
            lblEmailval.Text = _Person.Email;
            lblPhoneval.Text = _Person.Phone;   
            lblNationalNoval.Text= _Person.NationalNumber;
            lblGendorval.Text = _Person.GendorString;
            lblAddressval.Text = _Person.Address;
            lblDateOfBirthval.Text = _Person.DateOfBirth.ToString("yyyy/MMMM/dd");
            lblCountryval.Text = _Person.CountryName;
            ctrlUserImage1.SetImage(_Person.ImagePath, _Person.Gendor);
            pbGendor.Image = (_Person.Gendor == clsPerson.enGendor.Male)?Resources.Man_32:Resources.Woman_32;

        }


        

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (_Person != null)
            {
                frmAddEditPerson frm = new frmAddEditPerson(_Person.ID);
                frm.DataReceived += Find;
                frm.ShowDialog();
            }
        }
    }
}
