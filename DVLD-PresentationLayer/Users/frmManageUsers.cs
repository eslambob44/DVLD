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
    public partial class frmManageUsers : Form
    {
        public frmManageUsers()
        {
            InitializeComponent();
        }

        DataTable dtUsers;
       
        void _LoadDGV(DataView dvUsers)
        {
            dgvUsers.DataSource = dtUsers.DefaultView;
            lblRecords.Text = "#Records:" + dgvUsers.RowCount;
        }

        private void frmManageUsers_Load(object sender, EventArgs e)
        {
            dtUsers = clsUser.ListUsers();
            _LoadDGV(dtUsers.DefaultView);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            frmAddEditUser frm = new frmAddEditUser(-1);
            frm.ShowDialog();
            dtUsers = clsUser.ListUsers();
            _LoadDGV(dtUsers.DefaultView);
        }

        int GetUserIDFromDGV()
        {
            if (dgvUsers.Rows.Count != 0)
                return (int)dgvUsers.SelectedRows[0].Cells[0].Value;
            else return -1;
        }

        private void addNewUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddEditUser frm = new frmAddEditUser(-1);
            frm.ShowDialog();
            dtUsers = clsUser.ListUsers();
            _LoadDGV(dtUsers.DefaultView);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int UserID = GetUserIDFromDGV();
            if(UserID != -1)
            {
                frmAddEditUser frm = new frmAddEditUser(UserID);
                frm.ShowDialog();
                dtUsers = clsUser.ListUsers();
                _LoadDGV(dtUsers.DefaultView);
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int UserID = GetUserIDFromDGV();
            if (UserID != -1)
            {
                if(UserID == clsGlobalSettings.UserID)
                {
                    MessageBox.Show("Cannot delete current user", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (MessageBox.Show("Are you sure you want to delete this user", "",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if(clsUser.DeleteUser(UserID))
                    {
                        dtUsers = clsUser.ListUsers();
                        _LoadDGV(dtUsers.DefaultView);
                        MessageBox.Show("_User Deleted Successfully");
                    }
                    else
                    {
                        MessageBox.Show("user is connected with other operations cannot delete",
                            "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int UserID = GetUserIDFromDGV();
            if (UserID != -1)
            {
                frmUserInfo frm = new frmUserInfo(UserID);
                frm.ShowDialog();
            }
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int UserID = GetUserIDFromDGV();
            if (UserID != -1)
            {
                frmChangePassword frm = new frmChangePassword(UserID);
                frm.ShowDialog();
            }
        }
    }
}
