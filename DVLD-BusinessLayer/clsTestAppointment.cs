using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_BusinessLayer
{
    public class clsTestAppointment
    {
        enum enMode { Update , AddNew }
        enMode _Mode;
        private int _ID;
        public int ID { get { return _ID; } }

        private clsTestType.enTestType _TestType;
        public clsTestType.enTestType TestType
        {
            get { return _TestType; }
            set
            {
                if(_Mode == enMode.AddNew)
                {
                    _TestType = value;
                    _PaidFees = clsTestType.GetTestFees((int)_TestType);
                }
            }
        }
        private float _PaidFees;
        public float PaidFees {  get { return _PaidFees; } }

        private int _LocalDrivingLicenseApplicationID;
        public int LocalDrivingLicenseApplicationID
        {
            get { return _LocalDrivingLicenseApplicationID; }
            set
            {
                if(_Mode == enMode.AddNew) _LocalDrivingLicenseApplicationID = value;
            }
        }

        public DateTime AppointmentDate { get;set; }

        private int _CreatedUserID;
        public int CreatedUserID
        {
            get { return _CreatedUserID; }
            set
            {
                if(_Mode == enMode.AddNew) _CreatedUserID= value;   
            }
        }

        private bool _IsLocked;
        public bool IsLocked { get { return _IsLocked; }  }

        private int _RetakeTestApplicationID;
        public int RetakeTestApplicationID
        {
            get { return _RetakeTestApplicationID; }
            set
            {
                if( _Mode == enMode.AddNew) _RetakeTestApplicationID = value;
            }
        }

        private clsTestAppointment()
        {
            _Mode = enMode.AddNew;
            _ID = -1;
            TestType = clsTestType.enTestType.Vision;
            _LocalDrivingLicenseApplicationID = -1;
            AppointmentDate = DateTime.Now;
            CreatedUserID = -1;
            _IsLocked = false;
            _RetakeTestApplicationID = -1;
        }

        private clsTestAppointment( int iD, clsTestType.enTestType testType, float paidFees, int localDrivingLicenseApplicationID
            , DateTime appointmentDate, int createdUserID, bool isLocked , int RetakeTestApplicationID)
        {
            _Mode = enMode.Update;
            _ID = iD;
            _TestType = testType;
            _PaidFees = paidFees;
            _LocalDrivingLicenseApplicationID = localDrivingLicenseApplicationID;
            AppointmentDate = appointmentDate;
            _CreatedUserID = createdUserID;
            _IsLocked = isLocked;
            _RetakeTestApplicationID=RetakeTestApplicationID;
        }

        static public clsTestAppointment Find(int AppointmentID)
        {
            int testTypeID = -1;
            float paidFees = 0.0f;
            int localDrivingLicenseApplicationID = 0;
            DateTime appointmentDate = DateTime.MinValue;
            int createdUserID = 0;
            bool isLocked = false;
            int RetakeTestApplicationID=-1;

            if (clsTestAppointmentDataAccessLayer.FindAppointment(AppointmentID, ref testTypeID, ref localDrivingLicenseApplicationID
                , ref appointmentDate, ref paidFees, ref createdUserID, ref isLocked ,ref RetakeTestApplicationID))
            {
                return new clsTestAppointment(AppointmentID, (clsTestType.enTestType)testTypeID, paidFees
                    , localDrivingLicenseApplicationID, appointmentDate, createdUserID, isLocked, RetakeTestApplicationID);
            }
            else return null;
        }

        static public clsTestAppointment FindByRetakeTestApplicationID(int RetakeTestApplicationID)
        {
            int testTypeID = -1;
            float paidFees = 0.0f;
            int localDrivingLicenseApplicationID = 0;
            DateTime appointmentDate = DateTime.MinValue;
            int createdUserID = 0;
            bool isLocked = false;
            int TestAppointmentID = -1;

            if (clsTestAppointmentDataAccessLayer.FindAppointmentByRetakeTestApplicationID(RetakeTestApplicationID, ref testTypeID, ref localDrivingLicenseApplicationID
                , ref appointmentDate, ref paidFees, ref createdUserID, ref isLocked, ref TestAppointmentID))
            {
                return new clsTestAppointment(TestAppointmentID, (clsTestType.enTestType)testTypeID, paidFees
                    , localDrivingLicenseApplicationID, appointmentDate, createdUserID, isLocked, RetakeTestApplicationID);
            }
            else return null;
        }

        static public clsTestAppointment GetAddNewObject()
        {
            return new clsTestAppointment();
        }

        private bool _AddNewAppointment()
        {

            clsLocalLicenseApplication Application = clsLocalLicenseApplication.Find(LocalDrivingLicenseApplicationID);
            if (Application == null) return false;

            if (Application.IsHasActiveAppointment(TestType) || Application.IsTestPassed(TestType)) return false;


            _ID = clsTestAppointmentDataAccessLayer.AddNewAppointment((int)TestType, _LocalDrivingLicenseApplicationID
                , AppointmentDate, _PaidFees, _CreatedUserID, _IsLocked,_RetakeTestApplicationID);
            return (_ID != -1);
        }

        private bool _UpdateAppointment()
        {
            return clsTestAppointmentDataAccessLayer.UpdateAppointment(ID, AppointmentDate);
        }

        public bool Save()
        {
            switch(_Mode)
            {
                case enMode.AddNew:
                    if (_AddNewAppointment())
                    {
                        _Mode = enMode.Update;
                        return true;
                    }
                    else return false;
                case enMode.Update:
                    return _UpdateAppointment();
                default: return false;  
            }
        }

        static public bool LockAppointment(int TestAppointmentID)
        {
            return clsTestAppointmentDataAccessLayer.LockAppointment(TestAppointmentID);
        }

        public bool Lock()
        {
            return LockAppointment(ID);
        }

        public int GetTestID(int TestAppointmentID)
        {
            return clsTestAppointmentDataAccessLayer.GetTestID(TestAppointmentID);
        }

    }
}
