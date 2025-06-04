using DVLD_BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace DVLD_PresentationLayer.Users
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtUserName_Validating(object sender, CancelEventArgs e)
        {
            TextBox txt = sender as TextBox;
            if(string.IsNullOrEmpty(txt.Text) )
            {
                errorProvider1.SetError(txt, "_User name cannot be empty");
            }
            else
            {
                errorProvider1.SetError(txt, "");
            }
        }

        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            TextBox txt = sender as TextBox;
            if (string.IsNullOrEmpty(txt.Text))
            {
                errorProvider1.SetError(txt, "Password cannot be empty");
            }
            else
            {
                errorProvider1.SetError(txt, "");
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            int UserID = clsUser.GetUserID(txtUserName.Text , txtPassword.Text);
            if(UserID != -1)
            {
                if(clsUser.IsUserActive(UserID))
                {
                    clsGlobalSettings.UserID = UserID;
                    if (chkRemember.Checked) RememberUser();
                    else ForgetUser();
                    Form frm = new frmMain();
                    frm.ShowDialog();
                    
                }
                else
                {
                    MessageBox.Show("This account is deactivated please contact your admin");
                }
            }
            else
            {
                MessageBox.Show("User name or password is incorrect!", "Cannot login", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            
        }

        string UserNameValueName = "UserName";
        string PasswordValueName = "Password";
        string keyPath = @"SOFTWARE\DVLD\Login";
        void RememberUser()
        {
            try
            {
                Registry.SetValue(Registry.CurrentUser.Name+"\\" + keyPath ,UserNameValueName, txtUserName.Text, RegistryValueKind.String);
                Registry.SetValue(Registry.CurrentUser.Name + "\\" + keyPath, PasswordValueName, txtPassword.Text, RegistryValueKind.String);
            }
            catch { }

        }
        void ForgetUser()
        {
            using(RegistryKey Key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\DVLD\\Login" , true))
            {
                if(Key != null)
                {
                    Key.DeleteValue(UserNameValueName, false);
                    Key.DeleteValue(PasswordValueName, false);
                }
            }
        }

        

        void LoadRememberedUser( )
        {
            string UserName, Password;
            try
            {
                UserName = (string)Registry.GetValue(Registry.CurrentUser.Name+"\\" + keyPath, UserNameValueName, null);
                Password = (string)Registry.GetValue(Registry.CurrentUser.Name + "\\" + keyPath, PasswordValueName, null);
            }
            catch
            {
                return;
            }
            
            if(string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password))
            { return; }

            else
            {
                txtUserName.Text = UserName;
                txtPassword.Text = Password;
                chkRemember.Checked = true;
                btnLogin.Focus();
            }
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            LoadRememberedUser();
        }
    }
}
