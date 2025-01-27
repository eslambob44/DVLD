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
    public partial class frmAddNewLocalLicenseApplication : Form
    {
        clsLocalLicenseApplication Application = clsLocalLicenseApplication.GetAddNewObject();
        public frmAddNewLocalLicenseApplication()
        {
            InitializeComponent();
        }



        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
        }

        void _LoadcbLicneseClasses()
        {
            DataTable dtLicenseClasses = clsLocalLicenseApplication.ListLicenseClasses();
            foreach(DataRow row in dtLicenseClasses.Rows)
            {
                cbLicenseClasses.Items.Add(row[0].ToString());
            }
            cbLicenseClasses.SelectedIndex= 0;
        }
        void _LoadForm()
        {
            lblApplicationDate.Text = Application.ApplicationDate.ToString("dd/MM/yyyy");
            lblApplicationFees.Text = Application.PaidFees.ToString();
            lblCreatedUser.Text = clsUser.Find(clsGlobalSettings.UserID).UserName;
            _LoadcbLicneseClasses();
        }

        private void frmAddNewLocalLicenseApplication_Load(object sender, EventArgs e)
        {
            _LoadForm();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(ctrlFilterPerson1.PersonID ==-1)
            {
                MessageBox.Show("Choose a person first!" , "" , MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Application.PersonID = ctrlFilterPerson1.PersonID;
            Application.CreateUserID = clsGlobalSettings.UserID;
            Application.LicenseClass = (clsLocalLicenseApplication.enLicenseClass)(cbLicenseClasses.SelectedIndex + 1);
            int TestIfThereSameApplication;
            if(( TestIfThereSameApplication= Application.GetSameLicenseClassApplication())!= -1)
            {
                MessageBox.Show(@"Choose another license class,The selected person already have an active application for the selected class with id= "+ TestIfThereSameApplication
                                , "" , MessageBoxButtons.OK , MessageBoxIcon.Error);
                return;
            }

            if(Application.Save())
            {
                lblD_VApplicationID.Text = Application.ID.ToString();
                MessageBox.Show("Application saved successfully");
                btnSave.Enabled = false;
            }
            else
            {
                MessageBox.Show("Cannot save Application");
            }


        }
    }
}
