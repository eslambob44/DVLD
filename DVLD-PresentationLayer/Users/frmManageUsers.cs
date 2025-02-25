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

        DataTable _dtUsers;
        DataTable dtUsers 
        { get { return _dtUsers; } 
            set 
            {
                _dtUsers = value;
                _LoadDGV(_dtUsers.DefaultView);
            }
        }
        enum enFilter { None, UserID, UserName, PersonID , FullName , IsActive};
        enFilter _Filter = enFilter.None;
        string _FilterValue = "";
       
        void _LoadDGV(DataView dvUsers)
        {
            dgvUsers.DataSource = dtUsers.DefaultView;
            lblRecords.Text = "#Records:" + dgvUsers.RowCount;
            _ApplyFilter();
        }

        void _ApplyFilter()
        {
            if(_Filter == enFilter.None || _FilterValue =="")
            {
                dtUsers.DefaultView.RowFilter = "";
                return;
            }
            string Filter= $"{_Filter} ";
            if (_Filter == enFilter.UserID) Filter = "Convert(UserID, 'System.String') ";
            else if (_Filter == enFilter.PersonID) Filter = "Convert(PersonID , 'System.String') ";
            else if (_Filter == enFilter.IsActive) Filter = "Convert(IsActive , 'System.String') ";
            Filter +=$"LIKE '%{_FilterValue}%'";
            dtUsers.DefaultView.RowFilter = Filter ;
            
        }

        private void frmManageUsers_Load(object sender, EventArgs e)
        {
            dtUsers = clsUser.ListUsers();
            cbFilter.SelectedIndex = 0;
            cbIsActive.SelectedIndex = 0;
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
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int UserID = GetUserIDFromDGV();
            if(UserID != -1)
            {
                frmAddEditUser frm = new frmAddEditUser(UserID);
                frm.ShowDialog();
                dtUsers = clsUser.ListUsers();
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
        void _ShowFilterControl()
        {
            if (_Filter == enFilter.IsActive)
            {
                mtxtFilter.Visible = false;
                cbIsActive.Visible = true;
                cbIsActive.SelectedIndex = 0;
            }
            else if (cbFilter.SelectedIndex != (int)enFilter.None)
            {
                mtxtFilter.Visible = true;
                cbIsActive.Visible = false;
                mtxtFilter.Clear();
            }
            else
            {
                mtxtFilter.Visible = false;
                cbIsActive.Visible = false;
            }
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            _Filter = (enFilter)cbFilter.SelectedIndex;
            _ShowFilterControl();
            
            
            _FilterValue = "";
            _ApplyFilter();
        }

        private void mtxtFilter_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {
            
        }

        private void cbIsActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbIsActive.SelectedIndex == 0) _FilterValue = "";
            else if (cbIsActive.SelectedIndex == 1) _FilterValue = true.ToString();
            else _FilterValue = false.ToString();

            _ApplyFilter();
        }

        private void mtxtFilter_TextChanged(object sender, EventArgs e)
        {
            _FilterValue = mtxtFilter.Text;
            _ApplyFilter();
        }

        

        private void mtxtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (_Filter == enFilter.UserID || _Filter == enFilter.PersonID)
                e.Handled = (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar));
            
        }
    }
}
