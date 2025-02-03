using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccessLayer
{
    static public class clsLicenseClassDataAccessLayer
    {
        static public float GetLicenseClassFees(int LicenseClassID)
        {
            float Fees = -1;
            SqlConnection Connection = new SqlConnection(clsDataAccessLayerSettings.ConnectionString);
            string Query = @"Select ClassFees From LicenseClasses Where LicenseClassID = @LicenseClassID";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@LicenseClassID" , LicenseClassID);
            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();
                if(Result != null && float.TryParse(Result.ToString() , out float Temp))
                    Fees = Temp;
            }
            catch { }
            finally { Connection.Close(); }
            return Fees;
        }


        static public int GetDefaultValidityLength(int LicenseClassID)
        {
            int DefaultValidityLength = -1;
            SqlConnection Connection = new SqlConnection(clsDataAccessLayerSettings.ConnectionString);
            string Query = @"Select MinimumAllowedAge From LicenseClasses Where LicenseClassID = @LicenseClassID";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();
                if (Result != null && int.TryParse(Result.ToString(), out int Temp))
                    DefaultValidityLength = Temp;
            }
            catch { }
            finally { Connection.Close(); }
            return DefaultValidityLength;
        }


        static public int GetMinimumAllowedAge(int LicenseClassID)
        {
            int MinimumAllowedAge = -1;
            SqlConnection Connection = new SqlConnection(clsDataAccessLayerSettings.ConnectionString);
            string Query = @"Select MinimumAllowedAge From LicenseClasses Where LicenseClassID = @LicenseClassID";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();
                if (Result != null && int.TryParse(Result.ToString(), out int Temp))
                    MinimumAllowedAge = Temp;
            }
            catch { }
            finally { Connection.Close(); }
            return MinimumAllowedAge;
        }
    }
}
