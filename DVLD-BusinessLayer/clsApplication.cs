using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_BusinessLayer
{
    public class clsApplication
    {
        private enum enMode { Update , AddNew}
        private enMode _Mode;
        private int _ID;
        public int ID { get { return _ID; } }
        private int _PersonID;
        public int PersonID { get { return _PersonID; } }
        private DateTime _ApplicationDate;
        public DateTime ApplicationDate { get { return _ApplicationDate; } }
        public enum enApplicationType { NewLocalDrivingLicenseService = 1, RenewDrivingLicenseService =2
                                , ReplacementForaLostDrivingLicense=3
                                ,ReplacementForADamagedDrivingLicense=4, ReleaseDetainedDrivingLicense=5
                                , NewInternationalLicense}

        private enApplicationType __ApplicationType;
        private enApplicationType _ApplicationType { get { return __ApplicationType; }
            set
            {
                __ApplicationType = value;
                _PaidFees = clsApplicationType.GetApplicationTypeFees((int)__ApplicationType);
            }
        }
        public enApplicationType ApplicationType { get { return _ApplicationType; } }
        public enum enApplicationStatus { New = 0 ,Completed = 1 , Canceled =2}
        private enApplicationStatus __ApplicationStatus;
        private enApplicationStatus _ApplicationStatus {
            get { return __ApplicationStatus; }
            set
            {
                __ApplicationStatus = value;
                _LastStatusDate = DateTime.Now;
            } 
        }
        public enApplicationStatus ApplicationStatus { get { return _ApplicationStatus; } }

        private DateTime _LastStatusDate;
        public DateTime LastStatusDate { get { return _LastStatusDate; } }
        private float _PaidFees;
        public float PaidFees { get { return _PaidFees;} }
        private int _CreatedUserID;
        public int CreateUserID { get { return _CreatedUserID;} }


        private clsApplication(int PersonID, enApplicationType ApplicationType, int CreatedUserID)
        {
            _PersonID = PersonID;
            _ApplicationDate = DateTime.Now;
            _ApplicationType = ApplicationType;
            _ApplicationStatus = enApplicationStatus.New;
            _CreatedUserID = CreatedUserID;
            _Mode = enMode.AddNew;


        }
        static public clsApplication GetAddNewObject(int PersonID , enApplicationType ApplicationType , int CreatedUserID)
        {
            if (clsPerson.IsPersonExists(PersonID) && clsUser.Find(CreatedUserID).UserID != -1)
            {
                return new clsApplication(PersonID, ApplicationType, CreatedUserID);
            }
            else return null;
        }

        private bool _AddNewApplication()
        {
            int AppID = clsApplicationDataAccessLayer.AddApplication(_PersonID , ApplicationDate,(int)_ApplicationType
                         ,(short)_ApplicationStatus , _LastStatusDate , _PaidFees , _CreatedUserID);
            if(AppID != -1)
            {
                this._ID = AppID;
                return true;
            }
            else
            {
                return false;   
            }
        }

        virtual public bool Save()
        {
            switch(_Mode)
            {
                case enMode.AddNew:
                    if(_AddNewApplication())
                    {
                        _Mode = enMode.Update;
                        return true;
                    }
                    else return false;
            }
            return false;
        }

        
    }
}
