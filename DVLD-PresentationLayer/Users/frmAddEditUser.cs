using DVLD_BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_PresentationLayer.Users
{
    public partial class frmAddEditUser : Form
    {
        enum enMode { Update , AddNew}
        enMode _Mode;


        int _UserID;
        clsUser _User;
        public frmAddEditUser(int UserID)
        {
            InitializeComponent();
            _UserID = UserID;
        }

        

        void _LoadUpdateForm()
        {
            lblTitle.Text = "Update User";
            ctrlFilterPerson1.FindPerson(_User.PersonID);
            ctrlFilterPerson1.FilterEnabled = false;
            lblUserID.Text = _User.UserID.ToString();
            txtUserName.Text = _User.UserName.ToString();
            txtPassword.Enabled = false;
            txtConfirmPassword.Enabled = false;
            chkIsActive.Checked = _User.IsActive;
        }
        

        void _LoadUser()
        {
            if(_UserID == -1)
            {
                _User = clsUser.GetAddNewUser();
                _Mode = enMode.AddNew;
            }
            else
            {
                _User = clsUser.Find(_UserID);
                if( _User == null )
                {
                    MessageBox.Show("User Cannot found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    _Mode = enMode.Update;
                }
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmAddEditUser_Load(object sender, EventArgs e)
        {
            tpLoginInfo.Enabled = false;
            _LoadUser();
            if (_Mode == enMode.Update) _LoadUpdateForm();
        }

        private void ctrlFilterPerson1_EventPersonChanged(int obj)
        {
            if(obj != -1 && clsUser.IsPersonAUser(obj) && _Mode == enMode.AddNew)
            {
                MessageBox.Show("This person is already a user!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tpLoginInfo.Enabled = false;
            }
            else
            {
                tpLoginInfo.Enabled = true;
            }
            
        }

        private void txtUserName_Validating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrEmpty(txtUserName.Text))
            {
                errorProvider1.SetError(txtUserName, "user name cannot by empty");
                e.Cancel = true;
            }
            else if(_Mode == enMode.AddNew && clsUser.IsUserNameUsed(txtUserName.Text))
            {
                errorProvider1.SetError(txtUserName, "user name already exists");
                e.Cancel = true;
            }
            else if(_Mode == enMode.Update && clsUser.IsUserNameUsed(txtUserName.Text) && _User.UserName != txtUserName.Text)
            {
                errorProvider1.SetError(txtUserName, "user name already exists");
                e.Cancel = true;
            }
            else if (!txtUserName.Enabled)
            {
                errorProvider1.SetError(txtUserName, "Person is already exists");
                e.Cancel = true;
            }
            else
            {
                errorProvider1.SetError(txtUserName, "");
                e.Cancel = false;
            }
        }

        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPassword.Text) && _Mode == enMode.AddNew)
            {
                errorProvider1.SetError(txtPassword, "password cannot by empty");
                e.Cancel = true;
            }
            else if(!txtPassword.Enabled && _Mode == enMode.AddNew)
            {
                errorProvider1.SetError(txtPassword, "Person is already exists");
                e.Cancel=true;
            }
            else
            {
                errorProvider1.SetError(txtPassword, "");
                e.Cancel = false;
            }
        }

        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if(txtConfirmPassword.Text != txtPassword.Text)
            {
                errorProvider1.SetError(txtConfirmPassword,"the password should be the same");
                e.Cancel = true;
            }
            else if (!txtConfirmPassword.Enabled && _Mode == enMode.AddNew)
            {
                errorProvider1.SetError(txtConfirmPassword, "Person is already exists");
                e.Cancel = true;
            }
            else
            {
                errorProvider1.SetError(txtConfirmPassword, "");
                e.Cancel = false;
            }
        }

        


        private void btnSave_Click(object sender, EventArgs e)
        {
            if(!this.ValidateChildren())
            {
                MessageBox.Show("Some input are invalid!" , "" , MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _User.PersonID = ctrlFilterPerson1.PersonID;
            _User.UserName = txtUserName.Text;
            if(_Mode == enMode.AddNew) _User.Password = txtPassword.Text;
            _User.IsActive = chkIsActive.Checked;
            if(_User.Save())
            {
                MessageBox.Show("User Saved Successfully");
                _Mode = enMode.Update;
                _LoadUpdateForm();
            }
            else
            {
                MessageBox.Show("Failed to save user" , "" , MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
