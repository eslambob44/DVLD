using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
        protected DateTime _ApplicationDate;
        public DateTime ApplicationDate { get { return _ApplicationDate; } }
        public enum enApplicationType { NewLocalDrivingLicenseService = 1, RenewDrivingLicenseService =2
                                , ReplacementForaLostDrivingLicense=3
                                ,ReplacementForADamagedDrivingLicense=4, ReleaseDetainedDrivingLicense=5
                                , NewInternationalLicense}

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
        protected enApplicationStatus _ApplicationStatus;

        
        public enApplicationStatus ApplicationStatus 
        { 
            get { return _ApplicationStatus; }
            set
            {
                if (_Mode == enMode.AddNew)
                {
                    _ApplicationStatus = value;
                    _LastStatusDate = DateTime.Now;
                }
                
            }
        }

        protected DateTime _LastStatusDate;
        public DateTime LastStatusDate { get { return _LastStatusDate; } }
        protected float _PaidFees;
        public float PaidFees { get { return _PaidFees;} }
        protected int _CreatedUserID;
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
            _PersonID = -1;
            _ApplicationDate = DateTime.Now;
            ApplicationType = enApplicationType.NewInternationalLicense;
            ApplicationStatus = enApplicationStatus.New;
            _CreatedUserID = -1;
            


        }
        static public clsApplication GetAddNewObject()
        {
            
            return new clsApplication();
            
        }

        virtual protected bool _AddNewApplication()
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
