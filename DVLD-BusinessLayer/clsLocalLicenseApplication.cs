using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DVLD_BusinessLayer
{
    public class clsLocalLicenseApplication : clsApplication
    {
        public enum enActiveTest {VisionTest=1,WrittenTest=2,PracticalTest=3,TestCompleted = 0 , Error =-1 }
        private int _LocalDrivingLicenseApplicationID;
        public int LocalDrivingLicenseApplicationID { get { return _LocalDrivingLicenseApplicationID; } }
        
        private clsLicenseClass.enLicenseClass _LicenseClass;
        public clsLicenseClass.enLicenseClass LicenseClass 
        {
            get { return _LicenseClass; }
            set
            {
                if(_Mode == enMode.AddNew)  _LicenseClass = value; 
            }
        }

        private clsLocalLicenseApplication() : base () 
        {
            _LicenseClass = clsLicenseClass.enLicenseClass.OrdinaryDriving;
            ApplicationType = enApplicationType.NewLocalDrivingLicenseService;
        }

        private clsLocalLicenseApplication(int ldlID , clsLicenseClass.enLicenseClass LicenseClass, int ApplicationId,
            int PersonID , DateTime ApplicationDate , enApplicationType ApplicationType , float PaidFees 
            , enApplicationStatus ApplicationStatus , int CreatedUserID , DateTime LastStatusDate )
            :base(ApplicationId , PersonID , ApplicationDate , ApplicationType , PaidFees , ApplicationStatus ,
                 CreatedUserID,LastStatusDate)
        {
            this._LocalDrivingLicenseApplicationID = ldlID;
            this._LicenseClass = LicenseClass;
        }

        new static public clsLocalLicenseApplication Find(int LocalDrivingLicenseID)
        {
            int ApplicationId = -1;
            int LicenseClass=-1;
            
            
            if (clsLocalLicenseApplicationDataAccessLayer.FindLocalLicenseApplication(LocalDrivingLicenseID,
                ref ApplicationId, ref LicenseClass))
            {
                clsApplication Application = clsApplication.Find(ApplicationId);
                return new clsLocalLicenseApplication(LocalDrivingLicenseID,(clsLicenseClass.enLicenseClass)LicenseClass,
                    ApplicationId,Application.PersonID,Application.ApplicationDate , Application.ApplicationType,
                    Application.PaidFees,Application.ApplicationStatus,Application.CreateUserID,Application.LastStatusDate);
                
            }
            else return null;
            
        }

        static public DataTable ListLicenseClasses()
        {
            return clsLocalLicenseApplicationDataAccessLayer.ListLicenseClasses();
        }

        static public DataTable ListLocalLicenseApplications()
        {
            return clsLocalLicenseApplicationDataAccessLayer.ListLocalLicenseApplications();
        }

        new static public clsLocalLicenseApplication GetAddNewObject()
        {

            return new clsLocalLicenseApplication();
            
        }

        override protected bool _AddNewApplication()
        {
            if (GetActiveApplicationWithSameLicenseClass() == -1)
            {
                if (base._AddNewApplication())
                {
                    this._LocalDrivingLicenseApplicationID = clsLocalLicenseApplicationDataAccessLayer.AddLocalLicenseApplication(ID, (int)LicenseClass);
                    return true;
                }
                else return false;
                
            }
            else return false;
        }

        public int GetActiveApplicationWithSameLicenseClass()
        {
            return clsLocalLicenseApplicationDataAccessLayer.GetActiveApplicationWithSameLicenseClass(PersonID, (int)LicenseClass);
        }

        override public bool Save()
        {
            switch (_Mode)
            {
                case enMode.AddNew:
                    if (this._AddNewApplication())
                    {
                        _Mode = enMode.Update;
                        return true;
                    }
                    else return false;
            }
            return false;
        }

        static public bool DeleteApplication (int LocalDrivingLicenseApplicationID)
        {
            return clsLocalLicenseApplicationDataAccessLayer.DeleteApplication(LocalDrivingLicenseApplicationID);
        }

        static public enActiveTest GetActiveTest(int LocalDrivingLicenseID)
        {
            return (enActiveTest)clsLocalLicenseApplicationDataAccessLayer.GetActiveTest(LocalDrivingLicenseID);
        }

        public enActiveTest GetActiveTest()
        {
            return GetActiveTest(LocalDrivingLicenseApplicationID);
        }

        public int GetNumberOfPassedTests()
        {
            return clsLocalLicenseApplicationDataAccessLayer.GetNumberOfPassedTests(LocalDrivingLicenseApplicationID);
        }

        static public string GetLicenseClassString(clsLicenseClass.enLicenseClass LicenseClass)
        {
            switch(LicenseClass)
            {
                case clsLicenseClass.enLicenseClass.SmallMotorcycle:
                    return "Class 1 - Small Motorcycle";
                case clsLicenseClass.enLicenseClass.HeavyMotorcycle:
                    return "Class 2 - Heavy Motorcycle License";
                case clsLicenseClass.enLicenseClass.OrdinaryDriving:
                    return "Class 3 - Ordinary driving license";
                case clsLicenseClass.enLicenseClass.Commercial:
                    return "Class 4 - Commercial";
                case clsLicenseClass.enLicenseClass.Agricultural:
                    return "Class 5 - Agricultural";
                case clsLicenseClass.enLicenseClass.SmallAndMediumBus:
                    return "Class 6 - Small and medium bus";
                case clsLicenseClass.enLicenseClass.TruckAndHeavyVehicle:
                    return "Class 7 - Truck and heavy vehicle";
                default: return null;
            }
        }

        public string GetLicenseClassString()
        {
            return GetLicenseClassString(LicenseClass);
        }

        
        public bool IsHasActiveAppointment(clsTestType.enTestType testType)
        {
            return clsLocalLicenseApplicationDataAccessLayer.IsApplicationHaveActiveSameAppointment(LocalDrivingLicenseApplicationID, (int)testType);
        }

        public bool IsTestPassed(clsTestType.enTestType testType)
        {
            return clsLocalLicenseApplicationDataAccessLayer.IsPassedTheTest(LocalDrivingLicenseApplicationID, (int)testType);
        }

        public int GetNumberOfTriesOnTest(clsTestType.enTestType testType)
        {
            return clsLocalLicenseApplicationDataAccessLayer.GetNumberOfTriesInTest(LocalDrivingLicenseApplicationID, (int)testType);
        }

        public DataTable ListAppointmentsBasedOnTestType(clsTestType.enTestType TestType)
        {
            return clsTestAppointmentDataAccessLayer.ListAppointmentsByTestType(LocalDrivingLicenseApplicationID,(int)TestType);
        }

        public int IssueLicenseFirstTime(string Notes  , int CreatedUserID)
        {
            if (IsHaveLicense()) return -1;
            clsDriver Driver;
            if (clsDriver.IsPersonADriver(PersonID))
            {
                Driver = clsDriver.FindByPersonID(PersonID);
                if (Driver == null) return -1;
            }
            else
            {
                Driver = clsDriver.GetAddNewObject();
                Driver.CreatedUserID = CreatedUserID;
                Driver.PersonID = PersonID;
                if (!Driver.Save()) return -1;
            }

            clsLicense License = clsLicense.GetAddNewObject();
            License.ApplicationID = ID;
            License.CreatedUserID = CreatedUserID;
            License.DriverID = Driver.DriverID;
            License.IssueReason = clsLicense.enIssueReason.FirstTime;
            License.LicenseClass = LicenseClass;
            License.Notes = Notes;
            if (License.Save()) return License.LicenseID;
            else return -1;
        }
        public bool IsHaveLicense()
        {
            return clsLicense.FindLicenseByApplication(this.ID) != null;
        }
    }

    
}
