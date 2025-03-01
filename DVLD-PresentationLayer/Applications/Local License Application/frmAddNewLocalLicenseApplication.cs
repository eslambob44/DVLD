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

namespace DVLD_PresentationLayer.Applications.Local_License_Application
{
    public partial class frmAddNewLocalLicenseApplication : Form
    {
        clsLocalLicenseApplication Application = clsLocalLicenseApplication.GetAddNewObject();
        public frmAddNewLocalLicenseApplication()
        {
            InitializeComponent();
        }



        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
        }

        void _LoadcbLicneseClasses()
        {
            DataTable dtLicenseClasses = clsLocalLicenseApplication.ListLicenseClasses();
            foreach(DataRow row in dtLicenseClasses.Rows)
            {
                cbLicenseClasses.Items.Add(row[0].ToString());
            }
            cbLicenseClasses.SelectedIndex= 0;
        }
        void _LoadForm()
        {
            lblApplicationDate.Text = Application.ApplicationDate.ToString("dd/MM/yyyy");
            lblApplicationFees.Text = Application.PaidFees.ToString();
            lblCreatedUser.Text = clsUser.Find(clsGlobalSettings.UserID).UserName;
            _LoadcbLicneseClasses();
        }

        private void frmAddNewLocalLicenseApplication_Load(object sender, EventArgs e)
        {
            _LoadForm();
        }

        bool _ValidateIfThereIsntActiveApplication()
        {
            int TestIfThereSameApplication;
            if ((TestIfThereSameApplication = Application.GetActiveApplicationWithSameLicenseClass()) != -1)
            {
                MessageBox.Show(@"Choose another license class,The selected person already have an active application for the selected class with id= " + TestIfThereSameApplication
                                , "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        bool _ValidateIfPersonHasLicenseWithSameClass()
        {
            clsDriver Driver;
            if ((Driver = clsDriver.FindByPersonID(ctrlFilterPerson1.PersonID)) != null)
            {
                if (Driver.IsDriverHasAnActiveLicense((clsLicenseClass.enLicenseClass)(cbLicenseClasses.SelectedIndex + 1)))
                {
                    MessageBox.Show(@"Person already have an active license with the same class"
                                    , "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            return true;
        }

        int _GetAge(DateTime BirthDay)
        {
            int Age = DateTime.Now.Year - BirthDay.Year;
            BirthDay = BirthDay.AddYears(Age);
            if (DateTime.Now < BirthDay) Age--;
            return Age;
        }

        bool _ValidateIfPersonMeetMinimumAgeForLicense()
        {
            clsPerson Person = clsPerson.Find(ctrlFilterPerson1.PersonID);
            int MinimumAge = clsLicenseClass.GetMinimumAllowedAge(cbLicenseClasses.SelectedIndex + 1);
            if (_GetAge(Person.DateOfBirth) < MinimumAge)
            {
                MessageBox.Show("Person must meet minimum age: "+MinimumAge+" Year for apply for this license", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            } return true;
        }

        bool _Validate()
        {
            
            if(!_ValidateIfThereIsntActiveApplication()) return false;
            if(!_ValidateIfPersonHasLicenseWithSameClass()) return false;
            if(!_ValidateIfPersonMeetMinimumAgeForLicense()) return false;
            

            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(ctrlFilterPerson1.PersonID ==-1)
            {
                MessageBox.Show("Choose a person first!" , "" , MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Application.PersonID = ctrlFilterPerson1.PersonID;
            Application.CreateUserID = clsGlobalSettings.UserID;
            Application.LicenseClass = (clsLicenseClass.enLicenseClass)(cbLicenseClasses.SelectedIndex + 1);

            if (!_Validate()) return;

            if(Application.Save())
            {
                lblD_VApplicationID.Text = Application.ID.ToString();
                MessageBox.Show("Application saved successfully");
                btnSave.Enabled = false;
            }
            else
            {
                MessageBox.Show("Cannot save Application");
            }


        }
    }
}
