using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_BusinessLayer
{
    public class clsInternationalLicense
    {
        private int _InternationalLicenseID;
        public int InternationalLicenseID { get { return _InternationalLicenseID; } }
        enum enMode { AddNew, ReadOnly }
        enMode _Mode;
        private int _ApplicationID;
        public int ApplicationID
        {
            get { return _ApplicationID; }
            set
            {
                if ((_Mode == enMode.AddNew)) { _ApplicationID = value; }
            }
        }

        private int _DriverID;
        public int DriverID
        {
            get { return _DriverID; }
            set
            {
                if (_Mode == enMode.AddNew) { _DriverID = value; }
            }
        }

        private int _LocalLicenseID;
        public int LocalLicenseID
        {
            get { return _LocalLicenseID; }
            set
            {
                if (_Mode == enMode.AddNew) _LocalLicenseID = value;
            }
        }

        private DateTime _IssueDate;
        public DateTime IssueDate { get { return _IssueDate; } }

        private DateTime _ExpirationDate;
        public DateTime ExpirationDate { get { return _ExpirationDate; } }

        public bool IsActive { get; set; }

        private int _CreatedUserID;
        public int CreatedUserID
        {
            get { return _CreatedUserID; }
            set
            {
                if (_Mode == enMode.AddNew) _CreatedUserID = value;
            }
        }

        private clsInternationalLicense(int InternationalLicenseID, int ApplicationID, int DriverID
            , int LocalLicenseID, DateTime IssueDate, DateTime ExpirationDate, bool IsActive, int CreatedUserID)
        {
            _Mode = enMode.ReadOnly;
            _InternationalLicenseID = InternationalLicenseID;
            _ApplicationID = ApplicationID;
            _DriverID = DriverID;
            _LocalLicenseID = LocalLicenseID;
            _IssueDate = IssueDate;
            _ExpirationDate = ExpirationDate;
            this.IsActive = IsActive;
            _CreatedUserID = CreatedUserID;
        }

        private clsInternationalLicense()
        {
            _Mode = enMode.AddNew;
            _InternationalLicenseID = -1;
            _ApplicationID = -1;
            _DriverID = -1;
            _LocalLicenseID = -1;
            _IssueDate = DateTime.Now;
            _ExpirationDate = DateTime.Now.AddYears(1);
            this.IsActive = true;
            _CreatedUserID = -1;
        }

        static public clsInternationalLicense GetAddNewObject()
        {
            return new clsInternationalLicense();
        }

        static public clsInternationalLicense Find(int InternationalLicenseID)
        {
            int ApplicationID = -1, DriverID = -1, LocalLicenseID = -1, CreatedUserID = -1;
            bool IsActive = false;
            DateTime IssueDate = DateTime.Now , ExpirationDate = DateTime.Now;
            if (clsInternationalLicenseDataAccessLayer.FindInternationalLicense(InternationalLicenseID, ref ApplicationID,
                ref DriverID, ref LocalLicenseID, ref IssueDate, ref ExpirationDate, ref IsActive, ref CreatedUserID))
            {
                return new clsInternationalLicense(InternationalLicenseID, ApplicationID, DriverID,
                    LocalLicenseID, IssueDate, ExpirationDate, IsActive, CreatedUserID);
            }
            else return null;
        }

        private bool _AddNew()
        {
            int InternationalLicenseID = clsInternationalLicenseDataAccessLayer.AddInternationalLicense(
                _ApplicationID,_DriverID,_LocalLicenseID,_IssueDate ,_ExpirationDate,IsActive,_CreatedUserID);
            if (InternationalLicenseID != 0)
            {
                _InternationalLicenseID = InternationalLicenseID;
                return true;
            }
            else return false;
        }

        private bool _CheckIfCanAddInternationalLicense()
        {
            clsDriver Driver = clsDriver.Find(DriverID);
            if(Driver != null)
            {
                if (Driver.GetActiveInternationalLicenseID() != -1) return false;
            }
            else return false ;

            clsLicense LocalLicense = clsLicense.FindLicense(_LocalLicenseID);
            if (LocalLicense != null)
            {
                if (!LocalLicense.IsActive) return false;
                if (LocalLicense.ExpirationDate < DateTime.Now) return false;
                if(LocalLicense.IsDetained) return false;
                if(LocalLicense.LicenseClass != 
                    clsLocalLicenseApplication.enLicenseClass.OrdinaryDriving) return false;
                
            }
            else return false;

            return true;
        }

        public bool Save()
        {
            if (_Mode == enMode.AddNew)
            {
                if (_CheckIfCanAddInternationalLicense())
                {
                    if (_AddNew())
                    {
                        _Mode = enMode.ReadOnly;
                        return true;
                    }
                    else return false;
                }
                else return false;
            }
            else return false;
        }

        static public DataTable ListInternationalLicenses()
        {
            return clsInternationalLicenseDataAccessLayer.ListInternationalLicenses();
        }


    }
}
