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
using static System.Net.Mime.MediaTypeNames;

namespace DVLD_PresentationLayer.Test_Types
{
    public partial class frmUpdateTestType : Form
    {
        clsTestType.enTestType TestTypeID;
        clsTestType TestType;
        public frmUpdateTestType(clsTestType.enTestType TestTypeID)
        {
            InitializeComponent();
            this.TestTypeID = TestTypeID;
        }

        void _FindTestType()
        {
            TestType = clsTestType.Find(TestTypeID);
            if (TestType == null) this.Close();
            else _LoadForm();
        }
        
        void _LoadForm()
        {
            lblTestTypeID.Text = Convert.ToInt32(TestType.ID).ToString();
            txtTestTypeTitle.Text = TestType.Title.ToString();
            txtTestTypeDescription.Text = TestType.Description.ToString();
            mtxtFees.Text = TestType.Fees.ToString();
        }

        private void frmUpdateTestType_Load(object sender, EventArgs e)
        {
            _FindTestType();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtTestTypeTitle_Validating(object sender, CancelEventArgs e)
        {
            TextBox txt = sender as TextBox;
            if(string.IsNullOrEmpty(txt.Text))
            {
                errorProvider1.SetError(txt, "Cannot be empty");
            }
            else
            {
                errorProvider1.SetError(txt, "");
            }
        }

        private void mtxtFees_Validating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrEmpty(mtxtFees.Text))
            {
                errorProvider1.SetError(mtxtFees, "Cannot be empty");
            }
            else if(!float.TryParse(mtxtFees.Text , out float Test))
            {
                errorProvider1.SetError(mtxtFees, "Must enter numeric value");
            }
            else
            {
                errorProvider1.SetError(mtxtFees, "");
            }
        }

        bool _IsValidInput()
        {
            if(string.IsNullOrEmpty(txtTestTypeTitle.Text)) return false;
            if(string.IsNullOrEmpty(txtTestTypeDescription.Text)) return false;
            if(string.IsNullOrEmpty(mtxtFees.Text)) return false;
            if(!float.TryParse(mtxtFees.Text , out float Test)) return false;
            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(!_IsValidInput())
            {
                MessageBox.Show("Input is not valid!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            TestType.Title = txtTestTypeTitle.Text;
            TestType.Description = txtTestTypeDescription.Text;
            TestType.Fees = float.Parse(mtxtFees.Text);
            if(TestType.Save())
            {
                MessageBox.Show("Test Type Updated Successfully");
            }
            else
            {
                MessageBox.Show("Cannot Update Test Type" , "" , MessageBoxButtons.OK , MessageBoxIcon.Error);
            }
        }
    }
}
