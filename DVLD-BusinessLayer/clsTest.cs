using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_BusinessLayer
{
    public class clsTest
    {
        private int _TestID;
        public int TestID { get { return _TestID; } }

        private enum enMode { AddNew, ReadOnly };
        enMode _Mode;
        private int _TestAppointmentID;
        public int TestAppointmentID
        {
            get { return _TestAppointmentID; }
            set
            {
                if (_Mode == enMode.AddNew) _TestAppointmentID = value;
            }
        }
        bool _TestResult;
        public bool TestResult 
        {
            get { return _TestResult; }
            set
            {
                if(_Mode == enMode.AddNew) _TestResult = value; 
            }
        }

        private string _Notes;
        public string Notes
        {
            get { return _Notes; }
            set
            {
                if(_Mode == enMode.AddNew) Notes = value;
            }
        }

        private int _CreatedUserID;
        public int CreatedUserID
        {
            get { return _CreatedUserID; }
            set
            {
                if(_Mode == enMode.AddNew) CreatedUserID = value;
            }
        }

        clsTest()
        {
            _TestID = -1;
            _Mode = enMode.AddNew;
            _TestAppointmentID = -1;
            _Notes=string.Empty;
            _TestResult = false;
            _CreatedUserID = -1;
        }

        clsTest(int testID, int testAppointmentID, bool testResult,  string notes, int createdUserID)
        {
            _TestID = testID;
            _Mode = enMode.ReadOnly;
            _TestAppointmentID = testAppointmentID;
            _TestResult = testResult;
           _Notes = notes;
            _CreatedUserID = createdUserID;
        }

        static public clsTest GetAddNewObject()
        {
            return new clsTest();
        }

        static public clsTest Find(int TestAppointmentID)
        {
            int TestID = -1;
            bool TestResult = false;
            string Notes = string.Empty;
            int CreatedUserID = -1;
            if (clsTestDataAccessLayer.Find(TestAppointmentID, ref TestID, ref TestResult, ref Notes, ref CreatedUserID))
            {
                return new clsTest(TestID, TestAppointmentID, TestResult, Notes, CreatedUserID);
            }
            else return null;
        }

        public bool Save()
        {
            if (_Mode == enMode.ReadOnly) return false;
            int TestID = clsTestDataAccessLayer.AddTest(_TestAppointmentID,_TestResult , _Notes, _CreatedUserID);
            if(TestID != -1)
            {
                _TestID = TestID;
                _Mode = enMode.ReadOnly;
                return true;
            }
            else return false;
        }
    }
}
