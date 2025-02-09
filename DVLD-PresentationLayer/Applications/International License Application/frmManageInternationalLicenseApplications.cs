using DVLD_BusinessLayer;
using DVLD_PresentationLayer.Licenses;
using DVLD_PresentationLayer.Licenses.International_Licenses;
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

namespace DVLD_PresentationLayer.Applications.International_License_Application
{
    public partial class frmManageInternationalLicenseApplications : Form
    {

        DataTable _dtApplications;
        DataTable dtApplications
        {
            get { return _dtApplications; }
            set
            {
                _dtApplications = value;
                _LoadDGV(_dtApplications.DefaultView);
            }
        }

        void _LoadDGV(DataView dv)
        {
            dgvApplications.DataSource = dv;
            lblRecords.Text = "#Records: " + dgvApplications.Rows.Count;
        }
        public frmManageInternationalLicenseApplications()
        {
            InitializeComponent();
        }

        private void frmMangeInternationalLicenseApplications_Load(object sender, EventArgs e)
        {
            dtApplications = clsInternationalLicense.ListInternationalLicenses();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddInternationalDrivingLicenseApplication_Click(object sender, EventArgs e)
        {

            frmIssueInternationalLicense frm = new frmIssueInternationalLicense();
            frm.ShowDialog();
            dtApplications = clsInternationalLicense.ListInternationalLicenses();
        }

        private void showPersonDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int PersonID = clsDriver.Find((int)dgvApplications.SelectedRows[0].Cells["Driver ID"].Value).PersonID;
            frmPersonInfo frm  = new frmPersonInfo(PersonID);
            frm.ShowDialog();
        }

        private void showLicenseDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int InternationalLicenseID  = (int)dgvApplications.SelectedRows[0].Cells["Int.License ID"].Value;
            frmInternationalLicenseInfo frm  = new frmInternationalLicenseInfo(InternationalLicenseID);
            frm.ShowDialog();
        }

        private void showPersonLicenseHistoryToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            int DriverID = (int)dgvApplications.SelectedRows[0].Cells["Driver ID"].Value;
            frmManageLicenses frm = new frmManageLicenses(DriverID);
            frm.ShowDialog();
        }
    }
}
