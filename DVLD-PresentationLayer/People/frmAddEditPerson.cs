using DVLD_BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_PresentationLayer.People
{
    public partial class frmAddEditPerson : Form
    {
        clsPerson _Person;
        clsPerson.enGendor _Gendor = clsPerson.enGendor.Male;
        enum enMode { Update =0 , AddNew = 1}
        enMode _Mode;
        public frmAddEditPerson(int PersonID)
        {
            _FindPerson(PersonID);
            InitializeComponent();
        }

        void _FindPerson(int PersonID)
        {
            if (PersonID == -1)
            {
                _Person = clsPerson.GetAddNewObject();
                _Mode = enMode.AddNew;
            }
            else
            {
                _Person = clsPerson.Find(PersonID);
                _Mode = enMode.Update;
            }
        }

        void _LoadCountries()
        {
            cbCountries.Items.Clear();
            DataTable dtCountries = clsCountry.ListCountries();
            foreach(DataRow drCountry in  dtCountries.Rows)
            {
                cbCountries.Items.Add((string)drCountry["CountryName"]);
            }
            cbCountries.SelectedIndex = 0;
        }

        void _LoadForm()
        {
            dtpDateOfBirth.MaxDate = DateTime.Now.AddYears(-18);
            _LoadCountries();
            if(_Mode == enMode.Update)
            {
                lblFormName.Text = "Update person";
                lblPersonIDval.Text = _Person.ID.ToString();
                txtFirstName.Text = _Person.FirstName;
                txtLastName.Text = _Person.LastName;
                txtSecondName.Text = _Person.SecondName;
                txtThirdName.Text = _Person.ThirdName;
                txtNationalNumber.Text = _Person.NationalNumber;
                dtpDateOfBirth.Value = _Person.DateOfBirth; 
                if(_Person.Gendor == clsPerson.enGendor.Female)
                {
                    rbGendorFemale.Checked = true;
                }
                txtPhone.Text = _Person.Phone;  
                txtEmail.Text = _Person.Email;
                cbCountries.SelectedIndex = cbCountries.FindString(_Person.CountryName);
                ctrlUserImage1.SetImage(_Person.ImagePath , _Person.Gendor);
                txtAddress.Text = _Person.Address;
            }
        }

        private void frmAddEditPerson_Load(object sender, EventArgs e)
        {
            if(_Person == null)
            {
                MessageBox.Show("Couldn't find person","Error" , MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }

            _LoadForm();
        }

        private void ctrlUserImage1_EventUserImageChanged(bool obj)
        {
            lblRemoveImage.Visible = obj;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        bool _ValidateIsEmpty(string Value)
        {
            return string.IsNullOrEmpty(Value);
        }
        private void txt_ValidatingNotEmpty(object sender, CancelEventArgs e)
        {
            TextBox txt = sender as TextBox;
            if(_ValidateIsEmpty(txt.Text))
            {
                errorProvider1.SetError(txt, "Must enter a value");
            }
            else
            {
                errorProvider1.SetError(txt, "");
            }
        }

        private void txtNationalNumber_Validating(object sender, CancelEventArgs e)
        {
            string NationalityNumber = txtNationalNumber.Text;
            if (string.IsNullOrEmpty(txtNationalNumber.Text))
            {
                errorProvider1.SetError(txtNationalNumber, "Must enter a National number");
            }
            else if(_ValidateNationalNumber(txtNationalNumber.Text))
            {
                errorProvider1.SetError(txtNationalNumber, "person with this national number already exists in this system");
            }
            else
            {
                errorProvider1.SetError(txtNationalNumber, "");
            }
        }

        private void rbGendor_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if(rb.Name == rbGendorMale.Name)
            {
                ctrlUserImage1.SetImage(ctrlUserImage1.ImageLocation, clsPerson.enGendor.Male);
                _Gendor = clsPerson.enGendor.Male;
            }
            else
            {
                ctrlUserImage1.SetImage(ctrlUserImage1.ImageLocation, clsPerson.enGendor.Female);
                _Gendor= clsPerson.enGendor.Female;
            }
        }

        private void txtPhone_KeyPress(object sender, KeyPressEventArgs e)
        {

            if(!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        bool IsValidEmail(string Email)
        {
            return (Email.IndexOf("@") != -1 && Email.IndexOf(".com") != -1);
        }

        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            if (!IsValidEmail(txtEmail.Text) && !string.IsNullOrEmpty(txtEmail.Text))
            {
                errorProvider1.SetError(txtEmail, "the email is not valid ");
            }
            else
            {
                errorProvider1.SetError(txtEmail, "");
            }
        }

        

        private void lblSetImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if(openFileDialog1.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            string ImagePath = openFileDialog1.FileName;
            
            ctrlUserImage1.SetImage(ImagePath,_Gendor);
            
        }

        private void lblRemoveImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ctrlUserImage1.SetImage(null, _Gendor);
            lblSetImage.Focus();
        }

        bool _ValidateNationalNumber(string NationalNumber)
        {
            if(_Mode == enMode.AddNew)
            {
                return clsPerson.IsPersonExists(NationalNumber);
            }
            else if(_Mode == enMode.Update)
            {
                return (clsPerson.IsPersonExists(NationalNumber) && NationalNumber != _Person.NationalNumber);
            }
            return false;
        }


        public delegate void DataReceivedHandler(int PersonID);
        public event DataReceivedHandler DataReceived;

        private void btnSave_Click(object sender, EventArgs e)
        {
            if( this.ValidateChildren() ) 
            {
                MessageBox.Show("Some field are invalid", "cant save", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _Person.NationalNumber = txtNationalNumber.Text;
            _Person.FirstName = txtFirstName.Text;
            _Person.LastName = txtLastName.Text;
            _Person.SecondName = txtSecondName.Text;
            _Person.ThirdName = txtThirdName.Text;
            _Person.Email = txtEmail.Text;
            _Person.Address = txtAddress.Text;
            _Person.Gendor = _Gendor;
            _Person.Phone = txtPhone.Text;
            _Person.NationalityCountryID = clsCountry.GetCountryID(cbCountries.Text);
            _Person.ImagePath = ctrlUserImage1.ImageLocation;
            _Person.DateOfBirth = dtpDateOfBirth.Value;
            if(_Person.Save())
            {
                MessageBox.Show("Person saved successfully");
                _Mode = enMode.Update;
                lblPersonIDval.Text = _Person.ID.ToString();
                ctrlUserImage1.SetImage(_Person.ImagePath, _Person.Gendor);
                lblFormName.Text = "Update person";
                if(DataReceived != null)
                {
                    DataReceived.Invoke(_Person.ID);
                }
            }
        }
    }
}
