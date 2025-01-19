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

namespace DVLD_PresentationLayer.People
{
    public partial class frmManagePeople : Form
    {
        public frmManagePeople()
        {
            InitializeComponent();
            _dtPeople = clsPerson.ListPeople();
        }

        enum enFilter { None=0, PersonID =1 , NationalNo =2 , FirstName = 3 , SecondName = 4,
                        ThirdName = 5 , LastName = 6 , Nationality = 7 , Gendor = 8 ,
                        Phone = 9 , Email = 10 };
        enFilter _Filter = enFilter.None;
        
        DataTable _dtPeople;
        DataTable dtPeople
        {
            set
            {
                _dtPeople = value;
                _LoadDGV(_dtPeople.DefaultView);
            }
        }

        int GetSelectedPersonID()
        {
            
            if (dgvPeople.SelectedRows.Count == 0) return -1;
            else
            {
                return (int)dgvPeople.SelectedRows[0].Cells[0].Value;
            }
        }

        void _LoadDGV(DataView dvPeople)
        {
            
            dgvPeople.DataSource = dvPeople;
            lblRecords.Text = "#Records: "+dvPeople.Count;
            _ApplyFilter();
        }

        void _ApplyFilter()
        {
            if (_Filter == enFilter.None) _dtPeople.DefaultView.RowFilter = null;
            else if (_Filter == enFilter.PersonID)
            {
                _dtPeople.DefaultView.RowFilter = $"Convert(PersonID , 'System.String') LIKE '%{mtxtFilter.Text}%'";
            }
            else
            {
                _dtPeople.DefaultView.RowFilter = $"{_Filter} LIKE '%{mtxtFilter.Text}%'";
            }
        }


        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            frmAddEditPerson frm = new frmAddEditPerson(-1);
            frm.ShowDialog();
            dtPeople = clsPerson.ListPeople();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmManagePeople_Load(object sender, EventArgs e)
        {
            _LoadDGV(_dtPeople.DefaultView);
            cbFilter.SelectedIndex = 0;
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int PersonID = GetSelectedPersonID();
            frmPersonInfo frm = new frmPersonInfo(PersonID);
            frm.ShowDialog();
        }

        private void addNewPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddEditPerson frm = new frmAddEditPerson(-1);
            frm.ShowDialog();
            dtPeople = clsPerson.ListPeople();

        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int PersonID = GetSelectedPersonID();
            frmAddEditPerson frm= new frmAddEditPerson(PersonID);
            frm.ShowDialog();
            dtPeople = clsPerson.ListPeople();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure you want to delete this person","Warning",MessageBoxButtons.YesNo,MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                int PersonID = GetSelectedPersonID();
                if(clsPerson.Delete(PersonID))
                {
                    MessageBox.Show("Person deleted successfully");
                    dtPeople = clsPerson.ListPeople();

                }
                else
                {
                    MessageBox.Show("This person connected somewhere else Cannot Delete", "", MessageBoxButtons.OK, MessageBoxIcon.Error); 
                }
            }
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            mtxtFilter.Clear();
            _Filter = (enFilter)cbFilter.SelectedIndex;
            if (cbFilter.SelectedIndex == 0)
            {
                mtxtFilter.Visible = false;
                return;
            }
            else
            {
                mtxtFilter.Visible = true;
            }

            if(_Filter == enFilter.Phone ||  _Filter == enFilter.PersonID)
            {
                mtxtFilter.Mask = "00000000000";
            }
            else
            {
                mtxtFilter.Mask = "";
            }
        }

        private void mtxtFilter_TextChanged(object sender, EventArgs e)
        {
            _ApplyFilter();
        }
    }
}
