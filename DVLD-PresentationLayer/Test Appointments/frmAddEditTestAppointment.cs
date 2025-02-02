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

namespace DVLD_PresentationLayer.Test_Appointments
{
    public partial class frmAddEditTestAppointment : Form
    {
        enum enMode { Update , AddNew}
        enMode _Mode;
        clsTestAppointment _TestAppointment;
        clsTestType.enTestType _TestType;
        clsLocalLicenseApplication _LocalDrivingLicenseApplication;
        clsApplication _RetakeTestApplication;
        public frmAddEditTestAppointment(int LocalDrivingLicenseApplicationID , clsTestType.enTestType TestType)
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
            _TestType = TestType;
            _FindLocalDrivingLicenseApplication(LocalDrivingLicenseApplicationID);
            _FindTestAppointment(-1);
            _GetRetakeTest();
            
        }
        public frmAddEditTestAppointment(int TestAppointmentID)
        {
            InitializeComponent();
            _Mode = enMode.Update;
            _FindTestAppointment(TestAppointmentID);
            _FindLocalDrivingLicenseApplication(_TestAppointment.LocalDrivingLicenseApplicationID);
            _GetRetakeTest();
            _TestType = _TestAppointment.TestType;
        }

        void _GetRetakeTest()
        {
            switch(_Mode)
            {
                case enMode.Update:
                    if (_TestAppointment.RetakeTestApplicationID == -1) _RetakeTestApplication = null;
                    else _RetakeTestApplication = clsApplication.Find(_TestAppointment.RetakeTestApplicationID);
                    break;
                case enMode.AddNew:
                    if (_LocalDrivingLicenseApplication.GetNumberOfTriesOnTest(_TestType) != 0)
                    {
                        _RetakeTestApplication = clsApplication.GetAddNewObject();
                        _RetakeTestApplication.ApplicationType = clsApplication.enApplicationType.RetakeTest;
                        _RetakeTestApplication.CreateUserID = clsGlobalSettings.UserID;
                        _RetakeTestApplication.PersonID = _LocalDrivingLicenseApplication.PersonID;
                    }
                    else _RetakeTestApplication = null;
                    break;
            }
        }

        void _FindTestAppointment(int AppointmentID)
        {
            if (AppointmentID == -1)
            {
                _TestAppointment = clsTestAppointment.GetAddNewObject();
                _TestAppointment.TestType = _TestType;
                _TestAppointment.LocalDrivingLicenseApplicationID = _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID;
                _TestAppointment.CreatedUserID = clsGlobalSettings.UserID;
            }
            else
            {
                _TestAppointment = clsTestAppointment.Find(AppointmentID);
                if(_TestAppointment == null)
                {
                    MessageBox.Show("Test appointment not found!",
                    "Not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
            }
        }

        void _FindLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID)
        {
            _LocalDrivingLicenseApplication = clsLocalLicenseApplication.Find(LocalDrivingLicenseApplicationID);
            if( _LocalDrivingLicenseApplication == null )
            {
                MessageBox.Show("local driving license application not found!",
                    "Not found",MessageBoxButtons.OK, MessageBoxIcon.Error );
                this.Close();
            }
        }


        void _SetTitleTextAndImage()
        {
            switch (_TestType)
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
            lblAppointmentTitle.Text = _TestType.ToString() + " Test Appointment";
            gbAppointment.Text = _TestType.ToString() + " Test";
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmAddEditTestAppointment_Load(object sender, EventArgs e)
        {

            _LoadForm();
        }

        void _LoadUpdatedForm()
        {
            dtpAppointmentDate.Value = _TestAppointment.AppointmentDate;
            if(_RetakeTestApplication != null)
            {
                ctrlShowRetakeTestApplicationInfo1.Find(_RetakeTestApplication.ID);
            }
            else
            {
                ctrlShowRetakeTestApplicationInfo1.ShowEmptyForm(_TestType);
                ctrlShowRetakeTestApplicationInfo1.Enabled = false;
            }
        }

        void _LoadAddNewForm()
        {
            dtpAppointmentDate.Value = DateTime.Now;
            if (_RetakeTestApplication != null)
            {
                ctrlShowRetakeTestApplicationInfo1.ShowEmptyForm(_TestType, true);
            }
            else
            {
                ctrlShowRetakeTestApplicationInfo1.ShowEmptyForm(_TestType);
                ctrlShowRetakeTestApplicationInfo1.Enabled = false;
            }
        }

        void _LoadForm()
        {
            _SetTitleTextAndImage();
            lblDrivingClass.Text = _LocalDrivingLicenseApplication.GetLicenseClassString();
            lblFees.Text = _TestAppointment.PaidFees.ToString();
            lblFullName.Text = clsPerson.Find(_LocalDrivingLicenseApplication.PersonID).FullName;
            lblLocalDrivingLicenseApplicationID.Text = _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID.ToString();
            lblTrails.Text = _LocalDrivingLicenseApplication.GetNumberOfTriesOnTest(_TestType).ToString();
            if(_Mode == enMode.Update)
            {
                _LoadUpdatedForm();
                if(_TestAppointment.IsLocked ) _LockForm();
            }
            else
            {
                _LoadAddNewForm();
            }
            dtpAppointmentDate.MinDate = DateTime.Now;

        }

        void _LockForm()
        {
            btnSave.Enabled = false;
            dtpAppointmentDate.Enabled = false;
            lblAppointmentLocked.Visible = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _TestAppointment.AppointmentDate = dtpAppointmentDate.Value;
            switch(_Mode)
            {
                case enMode.Update:
                    if (_TestAppointment.Save()) MessageBox.Show("Appointment updated successfully");
                    else
                    {
                        MessageBox.Show("Cannot update appointment",
                    "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
                case enMode.AddNew:
                    if(_RetakeTestApplication != null)
                    {
                        if (!_RetakeTestApplication.Save())
                        {
                            MessageBox.Show("Cannot add new retake test application",
                            "add new Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                        {
                            _TestAppointment.RetakeTestApplicationID = _RetakeTestApplication.ID;
                        }
                    }
                    if(_TestAppointment.Save())
                    {
                        if(_RetakeTestApplication!= null)
                            ctrlShowRetakeTestApplicationInfo1.Find(_RetakeTestApplication.ID);
                        MessageBox.Show("Appointment Added successfully");
                        _Mode = enMode.Update;
                    }
                    else
                    {
                        MessageBox.Show("Cannot add new appointment",
                    "add new Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return ;
                    }
                    break;
            }
        }
    }
}
