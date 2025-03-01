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

        void RememberUser(string FilePath = @"D:\RememberedUser.txt", string Delim = "#//#")
        {
            string Content = txtUserName.Text + Delim + txtPassword.Text;
            try
            {
                StreamWriter Writer = new StreamWriter(FilePath);
                Writer.Write(Content);
                Writer.Close();
            }
            catch
            {

            }

        }
        void ForgetUser(string FilePath = @"D:\RememberedUser.txt", string Delim = "#//#")
        {
            string Content = "";
            try
            {
                StreamWriter Writer = new StreamWriter(FilePath);
                Writer.Write(Content);
                Writer.Close();
            }
            catch
            {

            }
        }

        string OpenRememberedUserFile(string FilePath = @"D:\RememberedUser.txt")
        {
            string Content = null;
            if (File.Exists(FilePath))
            {
                try
                {
                    StreamReader Reader = new StreamReader(FilePath);
                    Content = Reader.ReadToEnd();
                    Reader.Close();
                }
                catch(Exception e)
                {
                    return null;
                }
                return Content;
            }
            return null;
        }

        void LoadRememberedUser( string Delim = "#//#")
        {
            string Content = OpenRememberedUserFile();
            if(!string.IsNullOrEmpty(Content))
            {
                string[] Delims = {Delim};
                string[] UserDate = Content.Split(Delims , StringSplitOptions.RemoveEmptyEntries);
                txtUserName.Text = UserDate[0];
                txtPassword.Text = UserDate[1];
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
