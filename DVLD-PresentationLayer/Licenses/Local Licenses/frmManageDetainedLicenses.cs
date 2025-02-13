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

namespace DVLD_PresentationLayer.Licenses.Local_Licenses
{
    public partial class frmManageDetainedLicenses : Form
    {

        DataTable _dtDetainedLicenses;
        DataTable dtDetainedLicenses
        {
            get {  return _dtDetainedLicenses;}
            set
            {
                _dtDetainedLicenses = value;
                _LoadDGV(_dtDetainedLicenses.DefaultView);
            }
        }

        void _LoadDGV(DataView dv)
        {
            dgvLicenses.DataSource = dv;
            lblRecords.Text = "#Records: " + dgvLicenses.Rows.Count;
            _ApplyFilter();
        }

        void _ApplyFilter()
        {

            string Filter = "";
            string ColumnName = _GetColumnFilterTitle();
            if(!string .IsNullOrEmpty(mtxtFilter.Text))
            {
                Filter = $"{ColumnName} LIKE '%{mtxtFilter.Text.Trim()}%'";
                
            }
            dtDetainedLicenses.DefaultView.RowFilter = Filter;
        }

        string _GetColumnFilterTitle()
        {
            switch(_Filter)
            {
                case enFilter.None:
                    return "";
                case enFilter.DetainID:
                    return "Convert([D.ID] , 'System.String')";
                case enFilter.IsReleased:
                    return "[Is Released]";
                case enFilter.NationalNo:
                    return "[N.NO.]";
                case enFilter.FullName:
                    return "[Full Name]";
                case enFilter.ReleasedApplicationID:
                    return "Convert([Release App ID] , 'System.String')";
                default: return "";
            }
        }
        public frmManageDetainedLicenses()
        {
            InitializeComponent();
        }

        private void frmManageDetainedLicenses_Load(object sender, EventArgs e)
        {
            dtDetainedLicenses = clsDetainedLicense.ListDetainedLicenses();
            cbFilter.SelectedIndex = 0;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDetainLicense_Click(object sender, EventArgs e)
        {
            frmDetainLicense frm = new frmDetainLicense();
            frm.ShowDialog();
            dtDetainedLicenses = clsDetainedLicense.ListDetainedLicenses();
        }

        private void btnReleaseLicense_Click(object sender, EventArgs e)
        {
            frmReleaseDetainedLicense frm = new frmReleaseDetainedLicense();
            frm.ShowDialog();
            dtDetainedLicenses = clsDetainedLicense.ListDetainedLicenses();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            releaseLicenseToolStripMenuItem.Enabled =!(bool)(dgvLicenses.SelectedRows[0].Cells["Is Released"].Value);
        }

        private void showPersonDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int PersonID = clsPerson.Find((string)dgvLicenses.SelectedRows[0].Cells["N.No."].Value).ID;
            frmPersonInfo frm = new frmPersonInfo(PersonID);
            frm.ShowDialog();
        }

        private void showLicenseDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LicenseID = (int)dgvLicenses.SelectedRows[0].Cells["L.ID"].Value;
            frmLocalLicenseInfo frm = new frmLocalLicenseInfo(LicenseID);
            frm.ShowDialog();
        }

        private void eToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LicenseID = (int)dgvLicenses.SelectedRows[0].Cells["L.ID"].Value;
            int DriverID = clsLicense.FindLicense(LicenseID).DriverID;
            frmManageLicenses frm = new frmManageLicenses(DriverID);    
            frm.ShowDialog();
        }

        private void releaseLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LicenseID = (int)dgvLicenses.SelectedRows[0].Cells["L.ID"].Value;
            frmReleaseDetainedLicense frm = new frmReleaseDetainedLicense(LicenseID);   
            frm.ShowDialog();
            dtDetainedLicenses = clsDetainedLicense.ListDetainedLicenses();
        }

        enum enFilter { None, DetainID, IsReleased, NationalNo, FullName, ReleasedApplicationID }
        enFilter _Filter = enFilter.None;

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            _Filter = (enFilter)cbFilter.SelectedIndex;
            mtxtFilter.Clear();
            if (_Filter == enFilter.None)
                mtxtFilter.Visible = false;

            else
            {
                mtxtFilter.Visible=true;
                if(_Filter == enFilter.DetainID || _Filter == enFilter.ReleasedApplicationID)
                {
                    mtxtFilter.Mask = "00000000";
                }
                else
                {
                    mtxtFilter.Mask = "";
                }
            }
        }

        private void mtxtFilter_TextChanged(object sender, EventArgs e)
        {
            _ApplyFilter();
        }
    }
}
