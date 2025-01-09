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
    public partial class ctrlShowUserInfo : UserControl
    {

        clsUser _User;

        int _UserID = -1;
        public int UserID { get { return _UserID; } }

        public void Find(int UserID)
        {
            _User = clsUser.Find(UserID);
            if(_User == null)
            {
                MessageBox.Show("Cannot find user with this id", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoadEmptyForm();
                return;
            }
            else
            {
                _UserID = UserID;
                _LoadForm();
            }

            
        }
        void _LoadForm()
        {
            lblUserID.Text = _UserID.ToString();
            lblUserName.Text = _User.UserName;
            lblIsActive.Text = (_User.IsActive) ? "Yes" : "No";
            ctrlShowPersonInfo1.Find(_User.PersonID);
        }

        public void LoadEmptyForm()
        {
            lblUserID.Text = "??";
            lblUserName.Text = "??";
            lblIsActive.Text = "??";
            ctrlShowPersonInfo1.LoadEmptyForm();
        }
        public ctrlShowUserInfo()
        {
            InitializeComponent();
        }
    }
}
