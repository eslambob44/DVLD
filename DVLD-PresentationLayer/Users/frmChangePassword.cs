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

namespace DVLD_PresentationLayer.Users
{
    public partial class frmChangePassword : Form
    {
        clsUser _User = null;
        int _UserID = -1;
        public frmChangePassword(int UserID)
        {
            InitializeComponent();
            _UserID = UserID;
        }

        private void txtCurrentPassword_Validating(object sender, CancelEventArgs e)
        {
            if(!_User.IsCorrectPassword(txtCurrentPassword.Text))
            {
                e.Cancel= true;
                errorProvider1.SetError(txtCurrentPassword,"Wrong Password");
                return;
            }
            else
            {
                errorProvider1.SetError(txtCurrentPassword, "");
            }
        }

        private void frmChangePassword_Load(object sender, EventArgs e)
        {
            if(_UserID == -1)
            {
                this.Close();
            }
            _User = clsUser.Find(_UserID);
            ctrlShowUserInfo1.Find(_UserID);
        }

        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtNewPassword.Text))
            {
                e.Cancel= true;
                errorProvider1.SetError(txtNewPassword, "password cannot by empty");
                return;
            }
            else
            {
                errorProvider1.SetError(txtNewPassword, "");
            }
        }

        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if (txtConfirmPassword.Text != txtNewPassword.Text)
            {

                e.Cancel = true;
                errorProvider1.SetError(txtConfirmPassword, "the password should be the same");
                return;
            }
            else
            {
                errorProvider1.SetError(txtConfirmPassword, "");
            }
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("input are invalid", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if(_User.ChangePassword(txtCurrentPassword.Text , txtNewPassword.Text))
            {
                MessageBox.Show("Password Changed Successfully");
            }
            else
            {
                MessageBox.Show("Failed to change password", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
