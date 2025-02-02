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
using static DVLD_BusinessLayer.clsTestType;

namespace DVLD_PresentationLayer.Test
{
    public partial class frmTakeTest : Form
    {

        clsTestAppointment _TestAppointment;
        clsLocalLicenseApplication _LocalDrivingLicenseApplication;
        clsTest _Test;

        enum enResult { Fail,Pass,None}
        enResult _Result=enResult.None;
        public frmTakeTest(int TestAppointmentID)
        {
            InitializeComponent();
            _FindAppointment(TestAppointmentID);
            _FindLocalDrivingLicenseApplication(_TestAppointment.LocalDrivingLicenseApplicationID);
            _Test = clsTest.GetAddNewObject();
            _Test.TestAppointmentID = TestAppointmentID;
            _Test.CreatedUserID = clsGlobalSettings.UserID;
        }

        void _FindAppointment(int TestAppointmentID) 
        {
            _TestAppointment = clsTestAppointment.Find(TestAppointmentID);
            if (_TestAppointment == null)
            {
                MessageBox.Show("Test appointment not found!",
                "Not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        void _FindLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID)
        {
            _LocalDrivingLicenseApplication = clsLocalLicenseApplication.Find(LocalDrivingLicenseApplicationID);
            if (_LocalDrivingLicenseApplication == null)
            {
                MessageBox.Show("local driving license application not found!",
                    "Not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void _SetTitleTextAndImage()
        {
            switch (_TestAppointment.TestType)
            {
                case clsTestType.enTestType.Vision:
                    pbAppointment.Image = Resources.Vision_512;

                    break;
                case clsTestType.enTestType.Written:
                    pbAppointment.Image = Resources.Written_Test_512;
                    break;
                case clsTestType.enTestType.Street:
                    pbAppointment.Image = Resources.driving_test_512;
                    break;
            }
            lblAppointmentTitle.Text = _TestAppointment.TestType.ToString() + " Test Appointment";
            gbAppointment.Text = _TestAppointment.TestType.ToString() + " Test";
        }

        void _LoadForm()
        {
            _SetTitleTextAndImage();
            lblDrivingClass.Text = _LocalDrivingLicenseApplication.GetLicenseClassString();
            lblFees.Text = _TestAppointment.PaidFees.ToString();
            lblFullName.Text = clsPerson.Find(_LocalDrivingLicenseApplication.PersonID).FullName;
            lblLocalDrivingLicenseApplicationID.Text = _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID.ToString();
            lblTrails.Text = _LocalDrivingLicenseApplication.GetNumberOfTriesOnTest(_TestAppointment.TestType).ToString();
            lblAppointmentDate.Text = _TestAppointment.AppointmentDate.ToString("dd/MM/yyyy");
        }

        private void frmTakeTest_Load(object sender, EventArgs e)
        {
            _LoadForm();
        }

        private void rbPass_Click(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            _Result = (enResult)int.Parse(rb.Tag.ToString());
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(_Result == enResult.None)
            {
                MessageBox.Show("Choose result for the test!","" , MessageBoxButtons.OK, MessageBoxIcon.Error); 
                return;
            }
            if(MessageBox.Show("Are you sure you want to save? After that you cannot change the pass/fail results after you save?..",
                "Confirm",MessageBoxButtons.YesNo,MessageBoxIcon.Warning)== DialogResult.Yes)
            {
                _Test.TestResult = (_Result == enResult.Pass);
                _Test.Notes = txtNotes.Text;
                if(_Test.Save())
                {
                    _TestAppointment.Lock();
                    btnSave.Enabled = false;
                    lblTestID.Text = _Test.TestID.ToString();
                    MessageBox.Show("Data saved successfully","",MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                }
                else
                {
                    MessageBox.Show("Cannot save test result","",MessageBoxButtons.OK,MessageBoxIcon.Error);    
                }
            }
        }
    }
}
