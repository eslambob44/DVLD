using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static DVLD_BusinessLayer.clsApplication;

namespace DVLD_BusinessLayer
{
    public class clsApplication
    {
        protected enum enMode { Update , AddNew}
        protected enMode _Mode;
        protected int _ID;
        public int ID { get { return _ID; } }
        protected int _PersonID;
        public int PersonID { 
            
            get { return _PersonID; }
            set
            {
                if(_Mode == enMode.AddNew) _PersonID = value;
            }
        
        }
        private DateTime _ApplicationDate;
        public DateTime ApplicationDate { get { return _ApplicationDate; } }
        public enum enApplicationType { NewLocalDrivingLicenseService = 1, RenewDrivingLicenseService =2
                                , ReplacementForaLostDrivingLicense=3
                                ,ReplacementForADamagedDrivingLicense=4, ReleaseDetainedDrivingLicense=5
                                , NewInternationalLicense=6}

        protected enApplicationType _ApplicationType;
        
        public enApplicationType ApplicationType 
        {
            
            get { return _ApplicationType; }
            set
            {
                if (_Mode == enMode.AddNew)
                {
                    _ApplicationType = value;
                    _PaidFees = clsApplicationType.GetApplicationTypeFees((int)_ApplicationType);
                }
            }
        
        }
        public enum enApplicationStatus { New = 1 ,Completed = 3 , Canceled =2}
        private enApplicationStatus __ApplicationStatus;
        private enApplicationStatus _OriginalStatus;
        
        private enApplicationStatus _ApplicationStatus 
        { 
            get { return __ApplicationStatus; }
            set
            {
                __ApplicationStatus = value;
                 _LastStatusDate = DateTime.Now;
            }
        }

        public enApplicationStatus ApplicationStatus { get { return __ApplicationStatus; } }

        private DateTime _LastStatusDate;
        public DateTime LastStatusDate { get { return _LastStatusDate; } }
        private float _PaidFees;
        public float PaidFees { get { return _PaidFees;} }
        private int _CreatedUserID;
        public int CreateUserID 
        { 
            get { return _CreatedUserID; }
            set
            {
                if(_Mode == enMode.AddNew)  _CreatedUserID = value; 
            }       
        }


        protected clsApplication()
        {
            _Mode = enMode.AddNew;
            _ID = -1;
            _PersonID = -1;
            _ApplicationDate = DateTime.Now;
            this.ApplicationType = enApplicationType.NewInternationalLicense;
            this._ApplicationStatus = enApplicationStatus.New;
            _CreatedUserID = -1;
        }

        protected clsApplication(int ID , int PersonID , DateTime ApplicationDate , enApplicationType ApplicationType,
               float PaidFees, enApplicationStatus ApplicationStatus , int CreatedUserID , DateTime LastStatusDate)
        {
            _Mode = enMode.Update;
            _ID = ID;
            _PersonID = PersonID;
            this._ApplicationDate = ApplicationDate;
            this._ApplicationType = ApplicationType;
            this.__ApplicationStatus = ApplicationStatus;
            this._CreatedUserID = CreatedUserID;
            _PaidFees = PaidFees;
            this._LastStatusDate = LastStatusDate;
            _OriginalStatus=ApplicationStatus;
        }
        static public clsApplication GetAddNewObject()
        {
            
            return new clsApplication();
            
        }

        static public clsApplication Find(int ApplicationID)
        {
            int ID=-1, PersonID = -1, CreatedPersonID = -1;
            DateTime ApplicationDate =DateTime.Now, LastStatusUpdate = DateTime.Now;
            int ApplicationTypeID = -1;
            short ApplicationStatus = -1;
            float PaidFees = -1;
            if (clsApplicationDataAccessLayer.FindApplication(ApplicationID, ref PersonID, ref ApplicationDate,
                ref ApplicationTypeID, ref ApplicationStatus, ref LastStatusUpdate, ref PaidFees, ref CreatedPersonID))
            {
                return new clsApplication(ApplicationID, PersonID, ApplicationDate, (enApplicationType)ApplicationTypeID,
                    PaidFees, (enApplicationStatus)ApplicationStatus, CreatedPersonID, LastStatusUpdate);
            }
            else return null;
        }

        virtual protected bool _AddNewApplication()
        {
            int AppID = clsApplicationDataAccessLayer.AddApplication(_PersonID , ApplicationDate,(int)_ApplicationType
                         ,(short)__ApplicationStatus , _LastStatusDate , _PaidFees , _CreatedUserID);
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

        virtual protected bool _Update()
        {
            return clsApplicationDataAccessLayer.UpdateApplication(_ID , PersonID,ApplicationDate , (int)ApplicationType,
                        (short)_ApplicationStatus,LastStatusDate, _PaidFees , _CreatedUserID);
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
                case enMode.Update:
                    if (_Update())
                    {
                        _OriginalStatus = __ApplicationStatus;
                        return true;
                    }
                    else return false;
                    
            }
            return false;
        }

        public bool CancelApplication()
        {
            if (_Mode == enMode.Update && _OriginalStatus == enApplicationStatus.New)
            {
                _ApplicationStatus = enApplicationStatus.Canceled;
                return true;
            }
            return false;
        }

        virtual public bool CompleteApplication()
        {
            if (_Mode == enMode.Update && _OriginalStatus == enApplicationStatus.New)
            {
                _ApplicationStatus = enApplicationStatus.Completed;
                return true;
            }
            return false;
        }

        static public bool CancelApplication(int ApplicationID)
        {
            return clsApplicationDataAccessLayer.UpdateApplicationStatus(ApplicationID, (short)enApplicationStatus.Canceled);
        }

        static public bool CompleteApplication(int ApplicationID)
        {
            return clsApplicationDataAccessLayer.UpdateApplicationStatus(ApplicationID, (short)enApplicationStatus.Completed);
        }

        static public string GetApplicationTypeString(enApplicationType ApplicationType)
        {
            switch(ApplicationType)
            {
                case enApplicationType.NewLocalDrivingLicenseService:
                    return "New LocalDriving License Service";
                case enApplicationType.RenewDrivingLicenseService:
                    return "Renew Driving License Service";
                case enApplicationType.ReplacementForaLostDrivingLicense:
                    return "Replacement For a Lost Driving License";
                case enApplicationType.ReplacementForADamagedDrivingLicense:
                    return "Replacement For A Damaged Driving License";
                case enApplicationType.ReleaseDetainedDrivingLicense:
                    return "Release Detained Driving License";
                case enApplicationType.NewInternationalLicense:
                    return "New International License";
                default:
                    return null;
            }
        }

        public string GetApplicationTypeString()
        {
            return GetApplicationTypeString(_ApplicationType);
        }




    }
}
