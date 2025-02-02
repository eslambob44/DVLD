using DVLD_BusinessLayer;
using DVLD_PresentationLayer.Properties;
using DVLD_PresentationLayer.Test;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_PresentationLayer.Test_Appointments
{
    public partial class frmManageTestAppointments : Form
    {
        int _localDrivingLicenseApplicationID;
        clsLocalLicenseApplication _Application;
        clsTestType.enTestType _TestType;
        DataTable _dtAppointments;
        DataTable dtAppointments
        {
            get { return _dtAppointments; }
            set
            {
                _dtAppointments = value;
                _LoadDGV(dtAppointments.DefaultView);
            }
        }

        public frmManageTestAppointments(int LocalDrivingLicenseApplicationID , clsTestType.enTestType TestType)
        {
            InitializeComponent();
            _localDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            _TestType = TestType;

        }

        void _LoadDGV(DataView dv)
        {
            dgvAppointments.DataSource = dv;
            lblRecords.Text = "#Records: " + dgvAppointments.Rows.Count;
        }

        void _SetTitleTextAndImage()
        {
            switch( _TestType)
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
            
        }

        void _LoadForm()
        {
            _SetTitleTextAndImage();
            ctrlShowLocalDrivingLicenseApplicationInfo1.Find(_localDrivingLicenseApplicationID);
            dtAppointments=_Application.ListAppointmentsBasedOnTestType(_TestType);
        }

        void _FindApplication()
        {
            _Application = clsLocalLicenseApplication.Find(_localDrivingLicenseApplicationID);
            if(_Application == null)
            {
                MessageBox.Show("application with ID: " + _localDrivingLicenseApplicationID + " cannot be found!",
                    "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void frmManageTestAppointments_Load(object sender, EventArgs e)
        {
            _FindApplication();
            _LoadForm();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddTestAppointment_Click(object sender, EventArgs e)
        {
            
            if(_Application.IsTestPassed(_TestType))
            {
                MessageBox.Show("This person already passed the test before, You can only retake failed tests",
                    "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            else if (_Application.IsHasActiveAppointment(_TestType))
            {
                MessageBox.Show("Person already have an active appointment for this test, You cannot add new appointment",
                    "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                frmAddEditTestAppointment frm = new frmAddEditTestAppointment(_localDrivingLicenseApplicationID, _TestType);
                frm.ShowDialog();
                dtAppointments = _Application.ListAppointmentsBasedOnTestType(_TestType);
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int AppointmentID = int.Parse(dgvAppointments.SelectedRows[0].Cells[0].Value.ToString());
            frmAddEditTestAppointment frm = new frmAddEditTestAppointment(AppointmentID);
            frm.ShowDialog();
            dtAppointments = _Application.ListAppointmentsBasedOnTestType(_TestType);
        }

        private void takeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int AppointmentID = int.Parse(dgvAppointments.SelectedRows[0].Cells[0].Value.ToString());
            clsTestAppointment Appointment = clsTestAppointment.Find(AppointmentID);
            if (Appointment != null)
            {
                if(Appointment.GetTestID() == -1)
                {
                    frmTakeTest frm = new frmTakeTest(AppointmentID);
                    frm.ShowDialog();
                    dtAppointments = _Application.ListAppointmentsBasedOnTestType(_TestType);
                }
                else
                {
                    MessageBox.Show("test already taken, Cannot take a taken test","",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }
        }
    }
}
