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

namespace DVLD_PresentationLayer.Licenses
{
    public partial class frmManageLicenses : Form
    {
        clsDriver _Driver;
        public frmManageLicenses(int DriverID)
        {
            InitializeComponent();
            _FindDriver(DriverID);
        }

        DataTable _dtLocalLicenses;
        DataTable dtLocalLicenses
        {
            get { return _dtLocalLicenses; }
            set
            {
                _dtLocalLicenses = value;
                _LoadLocalDGV(_dtLocalLicenses.DefaultView);
            }
        }
        void _LoadLocalDGV(DataView dv)
        {
            dgvLocalLicensesHistory.DataSource = dv;
            lblLocalRecords.Text = "#Records: " + dgvLocalLicensesHistory.Rows.Count;
            
        }

        void _FindDriver(int DriverID) 
        {
            _Driver = clsDriver.Find(DriverID);
            if(_Driver == null)
                this.Close();
        }

        private void frmManageLicenses_Load(object sender, EventArgs e)
        {
            ctrlFilterPerson1.FindPerson(_Driver.PersonID);
            ctrlFilterPerson1.FilterEnabled = false;
            dtLocalLicenses = _Driver.ListLocalLicenses();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
