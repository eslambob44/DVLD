using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_BusinessLayer
{
    public class clsLicense
    {
        private int _LicenseID;
        public int LicenseID { get { return _LicenseID; } }
        private enum enMode { AddNew , ReadOnly}
        private enMode _Mode;

        private int _ApplicationID;
        public int ApplicationID
        {
            get { return _ApplicationID; }
            set { if (_Mode == enMode.AddNew) _ApplicationID = value; }
        }
        private int _DriverID;
        public int DriverID
        {
            get { return _DriverID; }
            set { if (_Mode == enMode.AddNew) _DriverID = value; }
        }


        private clsLocalLicenseApplication.enLicenseClass _LicenseClass;
        public clsLocalLicenseApplication.enLicenseClass LicenseClass
        {
            get { return _LicenseClass; }
            set
            {
                if (_Mode == enMode.AddNew)
                {
                    _LicenseClass = value;
                    _ExpirationDate = _IssueDate.AddYears(clsLicenseClass.GetDefaultValidityLength((int)_LicenseClass));
                    _PaidFees = clsLicenseClass.GetLicenseClassFees((int)_LicenseClass);
                }
            }
        }
        private DateTime _IssueDate;
        public DateTime IssueDate { get {  return _IssueDate; } }   
        private DateTime _ExpirationDate;
        public DateTime ExpirationDate { get { return _ExpirationDate; } }
        public string Notes;

        private float _PaidFees;
        public float PaidFees { get { return _PaidFees;} }

        public bool IsActive;

        public enum enIssueReason { FirstTime = 1 , Renew = 2 , ReplacementForDamaged = 3 , ReplacementForLost = 4}

        private enIssueReason _IssueReason;
        public enIssueReason IssueReason
        {
            get { return _IssueReason; }
            set
            {
                if (_Mode == enMode.AddNew) _IssueReason = value;
            }
        }
        private int _CreatedUserID;
        public int CreatedUserID
        {
            get { return _CreatedUserID; }
            set
            {
                if(_Mode == enMode.AddNew) _CreatedUserID = value;
            }
        }


        private clsLicense(int licenseID, int applicationID, int driverID, clsLocalLicenseApplication.enLicenseClass licenseClass, 
            DateTime issueDate, DateTime expirationDate, string notes, float paidFees,  enIssueReason issueReason, int createdUserID , bool isActive)
        {
            _LicenseID = licenseID;
            _Mode = enMode.ReadOnly;
            _ApplicationID = applicationID;
            _DriverID = driverID;
            _LicenseClass = licenseClass;
            _IssueDate = DateTime.Now;
            _ExpirationDate = expirationDate;
            Notes = notes;
            this.IsActive = isActive;
            _IssueReason = issueReason;
            _CreatedUserID = createdUserID;
        }

        private clsLicense()
        {
            _LicenseID = -1;
            _ApplicationID = -1;
            _DriverID = -1;
            LicenseClass = clsLocalLicenseApplication.enLicenseClass.OrdinaryDriving;
            _IssueDate = DateTime.Now;
            Notes = string.Empty;
            IsActive = true;
            _IssueReason = enIssueReason.FirstTime;
            _CreatedUserID = -1;
        }

        



        static public clsLicense GetAddNewObject()
        {
            return new clsLicense();
        }

        private bool _AddNew()
        {
            int LicenseID = clsLicenseDataAccessLayer.AddLicense(_ApplicationID, _DriverID, (int)_LicenseClass,
                _IssueDate, _ExpirationDate, Notes, _PaidFees, IsActive, (int)_IssueReason, CreatedUserID);
            if(LicenseID != -1)
            {
                _LicenseID = LicenseID;
                return true;
            }    
            else return false;
        }

        public bool Save()
        {
            if (_Mode == enMode.ReadOnly) return false;
            if(_IssueReason == enIssueReason.FirstTime)
            {
                if (clsDriver.Find(DriverID).IsDriverHasAnActiveLicense(LicenseClass)) 
                    return false;
            }
            else
            {
                if(!clsDriver.Find(DriverID).IsDriverHasAnActiveLicense(LicenseClass))
                    return false;
            }
            if (_AddNew())
            {
                _Mode = enMode.ReadOnly;
                return true;
            }
            else return false;
            
        }

        
    }
}
