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

namespace DVLD_PresentationLayer.Test_Types
{
    
    public partial class frmManageTestTypes : Form
    {
        private DataTable _dtTestTypes;
        DataTable dtTestTypes 
        {
            get 
            {
                return _dtTestTypes;
            }
            set
            {
                _dtTestTypes = value;
                dgvTestTypes.DataSource = _dtTestTypes.DefaultView;
                lblRecords.Text ="#Records: "+ _dtTestTypes.Rows.Count;
            }
            
        }
        public frmManageTestTypes()
        {
            InitializeComponent();
        }

        private void frmManageTestTypes_Load(object sender, EventArgs e)
        {
            dtTestTypes = clsTestType.ListTestTypes();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();   
        }

        private void editTestTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int TestTypeID = (int)dgvTestTypes.SelectedRows[0].Cells[0].Value;
            frmUpdateTestType frm = new frmUpdateTestType(TestTypeID);
            frm.ShowDialog();
            dtTestTypes = clsTestType.ListTestTypes();
        }
    }
}
