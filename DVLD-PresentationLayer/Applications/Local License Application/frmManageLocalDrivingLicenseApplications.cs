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
    public partial class frmManageLocalDrivingLicenseApplications : Form
    {
        DataTable _dtLocalDrivingLicenseApplications;

        DataTable dtLocalDrivingLicenseApplications
        {
            get { return _dtLocalDrivingLicenseApplications;}
            set
            {
                _dtLocalDrivingLicenseApplications = value;
                _LoadDGV(_dtLocalDrivingLicenseApplications.DefaultView);
            }
        }
        void _LoadDGV(DataView dvLocalDrivingLicenseApplication)
        {
            dgvApplications.DataSource = dvLocalDrivingLicenseApplication;
            lblRecords.Text = "#Records: "+dgvApplications.Rows.Count.ToString();
            _ApplyFilter();
        }

        void _ApplyFilter()
        {
            string Filter="";
            switch(_Filter)
            {
                case enFilter.ldlAppID:
                    Filter = $"Convert(L.D.L.AppID , System.String) LIKE '%{mtxtFilter.Text}%'";
                break;
                case enFilter.None:
                    break;
                case enFilter.Status:
                    if (cbStatus.SelectedIndex != 0) Filter = $"Status = '{cbStatus.Text}'";
                    break;
                default:
                    Filter = $"{_Filter.ToString()} LIKE '%{mtxtFilter.Text}%'";
                    break;
            }
            _dtLocalDrivingLicenseApplications.DefaultView.RowFilter = Filter;
        }
        public frmManageLocalDrivingLicenseApplications()
        {
            InitializeComponent();
        }

        private void frmManageLocalDrivingLicenseApplications_Load(object sender, EventArgs e)
        {
            dtLocalDrivingLicenseApplications=clsLocalLicenseApplication.ListLocalLicenseApplications();
            cbFilter.SelectedIndex = 0;
            cbStatus.SelectedIndex = 0;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddLocalDrivingLicenseApplication_Click(object sender, EventArgs e)
        {
            frmAddNewLocalLicenseApplication frm = new frmAddNewLocalLicenseApplication();
            frm.ShowDialog();
            dtLocalDrivingLicenseApplications = clsLocalLicenseApplication.ListLocalLicenseApplications();
        }

        clsLocalLicenseApplication.enApplicationStatus GetSelectedApplicationStatus()
        {
            string stringStatus =dgvApplications.SelectedRows[0].Cells["Status"].Value.ToString();
            foreach (clsApplication.enApplicationStatus Status in Enum.GetValues(typeof(clsApplication.enApplicationStatus)))
            {
                if (stringStatus == Status.ToString()) return Status;
            }
            return clsApplication.enApplicationStatus.New;
        }

        int GetSelectedldlApplicationID()
        {
            return (int)dgvApplications.SelectedRows[0].Cells[0].Value;
        }

        void _ShowCanceledCMS()
        {
            _DisableAllToolStripMenuItems();
            showApplicationDetailsToolStripMenuItem.Enabled = true;
            showPersonLicenseHistoryToolStripMenuItem.Enabled = true;
        }

        void _ShowCompletedCMS()
        {
            _ShowCanceledCMS();
            showLicenseToolStripMenuItem.Enabled= true;
        }

        void _ShowScheduleTests(clsLocalLicenseApplication.enActiveTest ActiveTest)
        {
            switch(ActiveTest)
            {
                case clsLocalLicenseApplication.enActiveTest.VisionTest:
                    sechduleTestsToolStripMenuItem.Enabled= true;
                    scheduleVisionTestToolStripMenuItem.Enabled = true;
                    break;
                case clsLocalLicenseApplication.enActiveTest.WrittenTest:
                    sechduleTestsToolStripMenuItem.Enabled = true;
                    scheduleWrittenTestToolStripMenuItem.Enabled = true;
                    break;
                case clsLocalLicenseApplication.enActiveTest.PracticalTest:
                    sechduleTestsToolStripMenuItem.Enabled = true;
                    scheduleStreetTestToolStripMenuItem.Enabled = true;
                    break;
                case clsLocalLicenseApplication.enActiveTest.TestCompleted:
                    issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = true;
                    break;
            }
        }

        void _ShowNewCMS()
        {
            _ShowCanceledCMS();
            deleteApplicationToolStripMenuItem.Enabled= true;
            cancelApplicationToolStripMenuItem.Enabled= true;
            int ldlApplicationID = GetSelectedldlApplicationID();
            clsLocalLicenseApplication.enActiveTest ActiveTest = clsLocalLicenseApplication.GetActiveTest(ldlApplicationID);
            _ShowScheduleTests(ActiveTest);
        }

        void _DisableAllToolStripMenuItems()
        {
            showApplicationDetailsToolStripMenuItem.Enabled = false ;
            showPersonLicenseHistoryToolStripMenuItem.Enabled = false;
            editApplicationToolStripMenuItem.Enabled = false;
            deleteApplicationToolStripMenuItem.Enabled = false;
            cancelApplicationToolStripMenuItem.Enabled = false;
            sechduleTestsToolStripMenuItem.Enabled = false;
            issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = false;
            showLicenseToolStripMenuItem.Enabled = false;
            scheduleVisionTestToolStripMenuItem.Enabled = false;
            scheduleWrittenTestToolStripMenuItem.Enabled = false;
            scheduleStreetTestToolStripMenuItem.Enabled = false;
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            clsLocalLicenseApplication.enApplicationStatus ApplicationStatus = GetSelectedApplicationStatus();
            switch(ApplicationStatus)
            {
                case clsLocalLicenseApplication.enApplicationStatus.Canceled:
                    _ShowCanceledCMS();
                    break;
                case clsLocalLicenseApplication.enApplicationStatus.Completed:
                    _ShowCompletedCMS();
                    break;
                case clsLocalLicenseApplication.enApplicationStatus.New:
                    _ShowNewCMS();
                    break;
            }
        }

        enum enFilter { None =0,ldlAppID =1 ,NationalNo=2,FullName=3,Status=4 };
        enFilter _Filter;


        void _ShowFilterControls()
        {
            mtxtFilter.Visible = true;
            cbStatus.Visible = false;
            switch (_Filter)
            {
                
                case enFilter.None:
                    mtxtFilter.Visible = false;
                    cbStatus.Visible = false;
                    break;
                case enFilter.ldlAppID:
                    mtxtFilter.Mask = "0000000000";
                    break;
                case enFilter.NationalNo:
                case enFilter.FullName:
                    mtxtFilter.Mask = "";
                    break;
                case enFilter.Status:
                    mtxtFilter.Visible = false;
                    cbStatus.Visible = true;
                    break;
            }
        }
        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            mtxtFilter.Text = "";
            cbStatus.SelectedIndex = 0;
            _Filter = (enFilter)cbFilter.SelectedIndex;
            _ShowFilterControls();
            


        }

        private void mtxtFilter_TextChanged(object sender, EventArgs e)
        {
            _ApplyFilter();
        }
    }
}
