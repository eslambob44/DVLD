using DVLD_PresentationLayer.People;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_PresentationLayer
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        

        private void peopleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new frmManagePeople();
            childForm.MdiParent = this; 
            childForm.Show();
        }
    }
}
