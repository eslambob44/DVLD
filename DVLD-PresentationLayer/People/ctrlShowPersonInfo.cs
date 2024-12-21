using DVLD_BusinessLayer;
using DVLD_PresentationLayer.People;
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

        public void Find(int PersonID)
        {
            _Person = clsPerson.Find(PersonID);
            if (_Person == null)
            {
                MessageBox.Show("The person not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                _LoadForm();
            }
        }
        private void _LoadForm()
        {
            lblPersonIDval.Text = _Person.ID.ToString();
            lblFullNameval.Text = _Person.FullName;
            lblEmailval.Text = _Person.Email;
            lblPhoneval.Text = _Person.Phone;   
            lblNationalNoval.Text= _Person.NationalNumber;
            lblGendorval.Text = _Person.GendorString;
            lblAddressval.Text = _Person.Address;
            lblDateOfBirthval.Text = _Person.DateOfBirth.ToString("yyyy/MMMM/dd");
            lblCountyval.Text = _Person.Country;
            ctrlUserImage1.SetImage(_Person.ImagePath, _Person.Gendor);

        }


        

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmAddEditPerson frm = new frmAddEditPerson(_Person.ID);
            frm.DataReceived += Find;
            frm.ShowDialog();
        }
    }
}
