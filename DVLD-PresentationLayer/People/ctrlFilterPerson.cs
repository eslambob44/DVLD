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
    public partial class ctrlFilterPerson : UserControl
    {
        enum enFilter { PersonID = 0 , NationalNo=1}
        enFilter _Filter;
        public ctrlFilterPerson()
        {
            InitializeComponent();
        }

        private bool _FilterEnable = true;

        public bool FilterEnabled
        {
            get
            {
                return _FilterEnable;
            }
            set
            {
                _FilterEnable = value;
                if(_FilterEnable )
                {
                    EnableFilter();
                }
                else
                {
                    DisableFilter();
                }
            }
        }

        void EnableFilter()
        {
            mtxt.Enabled = true;
            cbFilter.Enabled = true;
            btnFind.Enabled = true;
            btnAddNew.Enabled = true;

        }

        void DisableFilter()
        {
            mtxt.Enabled = false;
            cbFilter.Enabled = false;
            btnFind.Enabled = false;
            btnAddNew.Enabled = false;

        }

        public int PersonID { get; set; } = -1;

        void ChangeMaskInmtxt()
        {
            switch(_Filter)
            {
                case enFilter.PersonID:
                    mtxt.Mask = "00000";
                    break;
                case enFilter.NationalNo:
                    mtxt.Mask = "";
                    break;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _Filter = (enFilter)cbFilter.SelectedIndex;
            mtxt.Clear();
            ChangeMaskInmtxt();
        }

        private void ctrlFilterPerson_Load(object sender, EventArgs e)
        {
            cbFilter.SelectedIndex = 0;
            PersonID = -1;
        }

        public event Action<int> EventPersonChanged; 

        public void FindHandler(int PersonID)
        {
            Action<int> Handler = EventPersonChanged;
            if(Handler != null)
            {
                Handler(PersonID);
            }
        }

        void FindPerson()
        {
            if (string.IsNullOrEmpty(mtxt.Text)) return;
            switch (_Filter)
            {
                case enFilter.PersonID:
                    ctrlShowPersonInfo1.Find(int.Parse(mtxt.Text));
                    break;
                case enFilter.NationalNo:
                    ctrlShowPersonInfo1.Find(mtxt.Text);
                    break;
            }
            this.PersonID = ctrlShowPersonInfo1.PersonID;
            FindHandler(this.PersonID);
            

        }

        public void FindPerson(int PersonID)
        {
            cbFilter.SelectedIndex = (int)enFilter.PersonID;
            mtxt.Text = PersonID.ToString();
            FindPerson();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            FindPerson();
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            frmAddEditPerson frm = new frmAddEditPerson(-1);
            frm.DataReceived += FindPerson;
            frm.ShowDialog();
        }
    }
}
