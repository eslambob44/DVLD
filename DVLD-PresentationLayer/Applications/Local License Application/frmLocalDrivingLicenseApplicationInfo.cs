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
    public partial class frmLocalDrivingLicenseApplicationInfo : Form
    {
        int ldlApplicationID;
        public frmLocalDrivingLicenseApplicationInfo(int ldlApplicationID)
        {
            InitializeComponent();
            this.ldlApplicationID = ldlApplicationID;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmLocalDrivingLicenseApplicationInfo_Load(object sender, EventArgs e)
        {
            if (!ctrlShowLocalDrivingLicenseApplicationInfo1.Find(ldlApplicationID)) this.Close();
        }
    }
}
