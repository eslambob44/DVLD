using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_BusinessLayer
{
    public class clsDetainedLicense
    {
        // Public read-only properties with private setters
        public int DetainID { get; private set; }
        public int LicenseID { get; private set; }
        public DateTime DetainDate { get; private set; }
        public float FineFees { get; private set; }
        public int CreatedByUserID { get; private set; }
        public bool IsReleased { get; private set; }
        public DateTime ReleaseDate { get; private set; }
        public int? ReleasedByUserID { get; private set; }
        public int? ReleaseApplicationID { get; private set; }

        // Constructor to initialize all properties
        public clsDetainedLicense(
            int detainID,
            int licenseID,
            DateTime detainDate,
            float fineFees,
            int createdByUserID,
            bool isReleased,
            DateTime releaseDate,
            int releasedByUserID,
            int releaseApplicationID)
        {
            DetainID = detainID;
            LicenseID = licenseID;
            DetainDate = detainDate;
            FineFees = fineFees;
            CreatedByUserID = createdByUserID;
            IsReleased = isReleased;
            ReleaseDate = releaseDate;
            ReleasedByUserID = releasedByUserID;
            ReleaseApplicationID = releaseApplicationID;
        }

        static public clsDetainedLicense FindByLicenseID(int LicenseID)
        {
            int detainID = -1;
            DateTime detainDate = DateTime.MinValue;
            float fineFees = -1;
            int createdByUserID = -1;
            bool isReleased = false;
            DateTime releaseDate= DateTime.MinValue;
            int releasedByUserID = -1;
            int releaseApplicationID = -1;
            if (clsDetainedLicenseDataAccessLayer.Find(LicenseID, ref detainID, ref detainDate, ref fineFees
                , ref createdByUserID, ref releaseDate, ref releasedByUserID, ref releaseApplicationID))
            {
                return new clsDetainedLicense(detainID, LicenseID, detainDate, fineFees, createdByUserID
                    , isReleased, releaseDate, releasedByUserID, releaseApplicationID);
            }
            else return null;
        }
   
        static public clsDetainedLicense Find(int DetainedLicenseID)
        {
            int LicenseID = -1;
            DateTime detainDate = DateTime.MinValue;
            float fineFees = -1;
            int createdByUserID = -1;
            bool isReleased = false;
            DateTime releaseDate = DateTime.MinValue;
            int releasedByUserID = -1;
            int releaseApplicationID = -1;
            if (clsDetainedLicenseDataAccessLayer.Find(DetainedLicenseID, ref LicenseID, ref detainDate, ref fineFees
                , ref createdByUserID,ref isReleased, ref releaseDate, ref releasedByUserID, ref releaseApplicationID))
            {
                return new clsDetainedLicense(DetainedLicenseID, LicenseID, detainDate, fineFees, createdByUserID
                    , isReleased, releaseDate, releasedByUserID, releaseApplicationID);
            }
            else return null;
        }

        static public DataTable ListDetainedLicenses()
        {
            return clsDetainedLicenseDataAccessLayer.ListDetainedLicenses();
        }

        internal static int DetainLicense(int LicenseID , float FineFees , int CreatedUserID)
        {
            return clsDetainedLicenseDataAccessLayer.AddDetainedLicense(LicenseID,DateTime.Now,FineFees, CreatedUserID);
        }

        internal static bool ReleaseDetainedLicense(int LicenseID , int ReleasedUserID , int ReleasedApplicationID)
        {
            return clsDetainedLicenseDataAccessLayer.ReleaseLicense(LicenseID,DateTime.Now,ReleasedUserID , ReleasedApplicationID);
        }


    }
}
