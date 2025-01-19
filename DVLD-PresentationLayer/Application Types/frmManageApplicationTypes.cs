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
    public partial class frmManageApplicationTypes : Form
    {
        public frmManageApplicationTypes()
        {
            InitializeComponent();
        }
        DataTable _dtApplicationTypes = new DataTable();
        DataTable dtApplicationTypes 
        {
            get { return _dtApplicationTypes; }
            set
            {
                _dtApplicationTypes = value;
                dgvApplicationTypes.DataSource = _dtApplicationTypes.DefaultView;
                lblRecords.Text = "#Records: "+dgvApplicationTypes.Rows.Count;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmManageApplicationTypes_Load(object sender, EventArgs e)
        {
            dtApplicationTypes = clsApplicationType.ListApplicationTypes();
        }

        private void editApplicationTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ApplicationTypeID = (int)dgvApplicationTypes.SelectedRows[0].Cells[0].Value;
            frmUpdateApplicationType frm = new frmUpdateApplicationType(ApplicationTypeID);
            frm.ShowDialog();
            dtApplicationTypes= clsApplicationType.ListApplicationTypes();
        }
    }
}
