using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_BusinessLayer
{

    
    static public class clsLicenseClass
    {
        public enum enLicenseClass
        {
            SmallMotorcycle = 1, HeavyMotorcycle = 2, OrdinaryDriving = 3,
            Commercial = 4, Agricultural = 5, SmallAndMediumBus = 6, TruckAndHeavyVehicle = 7
        }
        static public float GetLicenseClassFees(int LicenseClassID)
        {
            return clsLicenseClassDataAccessLayer.GetLicenseClassFees(LicenseClassID);
        }

        static public int GetDefaultValidityLength(int LicenseClassID)
        {
            return clsLicenseClassDataAccessLayer.GetDefaultValidityLength(LicenseClassID);
        }

        static public int GetMinimumAllowedAge(int LicenseClassID)
        {
            return clsLicenseClassDataAccessLayer.GetMinimumAllowedAge(LicenseClassID);
        }
    }
}
