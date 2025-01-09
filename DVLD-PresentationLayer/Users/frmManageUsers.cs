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
            return (int)dgvUsers.SelectedRows[0].Cells[0].Value;
        }

        
    }
}
