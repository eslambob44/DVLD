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

namespace DVLD_PresentationLayer.Application_Types
{
    public partial class frmUpdateApplicationType : Form
    {
        int ApplicationTypesID;
        clsApplicationType ApplicationType;
        public frmUpdateApplicationType(int ApplicationTypesID)
        {
            InitializeComponent();
            this.ApplicationTypesID = ApplicationTypesID;
        }

        void _FindApplicationType()
        {
            ApplicationType = clsApplicationType.Find(ApplicationTypesID);
            if(ApplicationType == null)
            {
                this.Close();
            }
        }

        void _LoadForm()
        {
            lblApplicationTypeID.Text = ApplicationTypesID.ToString();
            txtApplicationTypeTitle.Text = ApplicationType.Title;
            mtxtFees.Text = ApplicationType.Fees.ToString();
        }

        private void frmUpdateApplicationType_Load(object sender, EventArgs e)
        {
            _FindApplicationType();
            _LoadForm();

        }

        private void txtApplicationTypeTitle_Validating(object sender, CancelEventArgs e)
        {
            
            if(string.IsNullOrEmpty(txtApplicationTypeTitle.Text))
            {
                errorProvider1.SetError(txtApplicationTypeTitle, "Cannot be empty");
            }
            else
            {
                errorProvider1.SetError(txtApplicationTypeTitle, "");
            }
        }

        bool _ValidateInput()
        {
            if (string.IsNullOrEmpty(txtApplicationTypeTitle.Text)) return false;
            if (string.IsNullOrEmpty(mtxtFees.Text)) return false;
            if (!float.TryParse(mtxtFees.Text, out float Temp)) return false;
            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(!_ValidateInput())
            {
                MessageBox.Show("Some input are invalid!" , "" , MessageBoxButtons.OK , MessageBoxIcon.Error);
                return;
            }
            ApplicationType.Title = txtApplicationTypeTitle.Text;
            ApplicationType.Fees=float.Parse(mtxtFees.Text);

            if(ApplicationType.Save())
            {
                MessageBox.Show("Application Type saved successfully");
            }
            else
            {
                MessageBox.Show("Cannot save Application Type", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void mtxtFees_Validating(object sender, CancelEventArgs e)
        {
            if(!float.TryParse(mtxtFees.Text, out float Temp))
            {
                errorProvider1.SetError(mtxtFees, "Must Enter numeric value");
            }
            else
            {
                errorProvider1.SetError(mtxtFees, "");
            }
        }
    }
}
