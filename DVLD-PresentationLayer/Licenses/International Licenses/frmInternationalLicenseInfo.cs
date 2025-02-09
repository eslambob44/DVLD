using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_PresentationLayer.Licenses.International_Licenses
{
    public partial class frmInternationalLicenseInfo : Form
    {
        public frmInternationalLicenseInfo(int InternationalLicenseID)
        {
            InitializeComponent();
            if(!ctrlShowInternationalLicenseInfo1.Find(InternationalLicenseID)) this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
