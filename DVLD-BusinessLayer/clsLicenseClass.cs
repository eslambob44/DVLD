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
