using DVLD_BusinessLayer;
using DVLD_PresentationLayer.Licenses;
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

namespace DVLD_PresentationLayer.Drivers
{
    public partial class frmManageDrivers : Form
    {

        DataTable _dtDrivers;
        DataTable dtDrivers
        {
            get { return _dtDrivers; }
            set
            {
                _dtDrivers= value;
                _LoadDGV(_dtDrivers.DefaultView);
            }
        }

        enum enFilter { None=0 , DriverID , PersonID , NationalNo , FullName}
        enFilter _Filter;

        void _LoadDGV(DataView dvDrivers)
        {
            dgvDrivers.DataSource= _dtDrivers;
            _ApplyFilter();
            lblRecords.Text = "#Records: " + dgvDrivers.Rows.Count.ToString();

        }

        void _ApplyFilter()
        {
            string Filter;
            if (_Filter == enFilter.None) Filter = "";
            else if (_Filter == enFilter.DriverID || _Filter == enFilter.PersonID)
                Filter = $"Convert([{cbFilter.Text}],System.String) LIKE '{mtxtFilter.Text}%'";
            else
                Filter = $"[{cbFilter.Text}] LIKE '{mtxtFilter.Text}%'";
            dtDrivers.DefaultView.RowFilter = Filter;   
        }
        public frmManageDrivers()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmManageDrivers_Load(object sender, EventArgs e)
        {
            dtDrivers = clsDriver.ListDrivers();
            cbFilter.SelectedIndex = 0;
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            _Filter = (enFilter)cbFilter.SelectedIndex;
            mtxtFilter.Clear();
            if (_Filter == enFilter.None) mtxtFilter.Visible = false;
            else mtxtFilter.Visible = true;

            
        }

        private void mtxtFilter_TextChanged(object sender, EventArgs e)
        {
            _ApplyFilter();
        }

        private void showPersonInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int PersonID  = int.Parse(dgvDrivers.SelectedRows[0].Cells["Person ID"].Value.ToString());
            frmPersonInfo frm = new frmPersonInfo(PersonID);
            frm.ShowDialog();   
        }

        private void showDriverLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int DriverID = int.Parse(dgvDrivers.SelectedRows[0].Cells["Driver ID"].Value.ToString());
            frmManageLicenses frm = new frmManageLicenses(DriverID);
            frm.ShowDialog();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            int DriverID = (int)dgvDrivers.SelectedRows[0].Cells["Driver ID"].Value;
            clsDriver Driver = clsDriver.Find(DriverID);
            issueInternationalLicenseToolStripMenuItem.Enabled = (Driver.GetActiveInternationalLicenseID()==-1);
            issueInternationalLicenseToolStripMenuItem.Enabled &= Driver.IsDriverHasAnActiveLicense(clsLicenseClass.enLicenseClass.OrdinaryDriving);
        }

        private void mtxtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (_Filter == enFilter.DriverID || _Filter == enFilter.PersonID)
                e.Handled = (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar));
        }
    }
}
