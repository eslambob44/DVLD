using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DVLD_BusinessLayer
{
    public class clsLocalLicenseApplication : clsApplication
    {
        private int _LocalDrivingLicenseApplicationID;
        public int LocalDrivingLicenseApplicationID { get { return _LocalDrivingLicenseApplicationID; } }
        public enum enLicenseClass
        {
            SmallMotorcycle = 1, HeavyMotorcycle = 2, OrdinaryDriving = 3,
            Commercial=4, Agricultural=5, SmallAndMediumBus=6 , TruckAndHeavyVehicle=7
        }
        private enLicenseClass _LicenseClass;
        public enLicenseClass LicenseClass 
        {
            get { return _LicenseClass; }
            set
            {
                if(_Mode == enMode.AddNew)  _LicenseClass = value; 
            }
        }

        private clsLocalLicenseApplication() : base () 
        {
            _LicenseClass = enLicenseClass.OrdinaryDriving;
            ApplicationType = enApplicationType.NewLocalDrivingLicenseService;
        }

        static public DataTable ListLicenseClasses()
        {
            return clsLocalLicenseApplicationDataAccessLayer.ListLicenseClasses();
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
    }
}
