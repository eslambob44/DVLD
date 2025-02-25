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
    public partial class ctrlFilterLocalLicenses : UserControl
    {
        public ctrlFilterLocalLicenses()
        {
            InitializeComponent();
        }

        public event Action<int> OccurFilterEvent;

        private void OccurFilterHandlerEvent(int LicenseID)
        {
            Action<int> Handler = OccurFilterEvent;
            if(Handler != null)
            {
                Handler(LicenseID);
            }
        }

        public void Find(int LicenseID)
        {
            mtxtFilter.Text = LicenseID.ToString();
            Application.DoEvents();
            btnFilter.PerformClick();
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            int LicenseID = int.Parse(mtxtFilter.Text.Trim());
            bool IsFound = ctrlShowLicenseInfo1.Find(LicenseID);
            LicenseID = (IsFound) ? LicenseID : -1;
            OccurFilterHandlerEvent(LicenseID);
        }

        public bool FilterEnabled { 
            set
            {
                gbFilter.Enabled = value;
            } 
        }

        private void mtxtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar));
        }
    }
}
