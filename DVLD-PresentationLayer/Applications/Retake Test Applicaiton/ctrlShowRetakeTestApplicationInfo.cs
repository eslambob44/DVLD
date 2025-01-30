using DVLD_BusinessLayer;
using System;
using System.Windows.Forms;

namespace DVLD_PresentationLayer.Applications.Retake_Test_Application
{
    public partial class ctrlShowRetakeTestApplicationInfo : UserControl
    {

        float _AppFees = 0;
        float _TotalFees = 0;
        int _RetakeTestApplicationID;
        public ctrlShowRetakeTestApplicationInfo()
        {
            InitializeComponent();

        }

        public void Find(int RetakeTestApplicationID)
        {
            _RetakeTestApplicationID = RetakeTestApplicationID;
            clsApplication RetakeTestApplication = clsApplication.Find(RetakeTestApplicationID);
            
            if (RetakeTestApplication != null)
            {
                if (RetakeTestApplication.ApplicationType == clsApplication.enApplicationType.RetakeTest)
                {
                    _AppFees = RetakeTestApplication.PaidFees;
                    float AppointmentFees = clsTestAppointment.FindByRetakeTestApplicationID(RetakeTestApplicationID).PaidFees;
                    _TotalFees = _AppFees + AppointmentFees;
                    _LoadForm(); 
                    
                }
                else
                {
                    MessageBox.Show("Cannot display non retake test application", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

            }
            else
            {
                MessageBox.Show("Cannot find an application with id: " + _RetakeTestApplicationID, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
        }

        public void ShowEmptyForm(clsTestType.enTestType TestType , bool EnableRetakeTestApplication = false)
        {
            _RetakeTestApplicationID = -1;
            _AppFees = (EnableRetakeTestApplication) ?
                clsApplicationType.GetApplicationTypeFees((int)clsApplication.enApplicationType.RetakeTest) : 0;

            _TotalFees = clsTestType.GetTestFees((int)TestType) + _AppFees;
            _LoadForm();
        }



        void _LoadForm()
        {
            lblTotalFees.Text = _TotalFees.ToString();
            lblRetakeTestApplicationID.Text=(_RetakeTestApplicationID!=-1)?_RetakeTestApplicationID.ToString():"??";
            lblRetakeTestApplicationFees.Text = _AppFees.ToString();

        }

        

        private void ctrlShowRetakeTestApplicationInfo_Load(object sender, EventArgs e)
        {
            
        }
    }
}
