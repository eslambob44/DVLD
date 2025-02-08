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
    public partial class frmLocalLicenseInfo : Form
    {
        public frmLocalLicenseInfo(int LicenseID)
        {
            InitializeComponent();
            if(!ctrlShowLicenseInfo1.Find(LicenseID)) this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
