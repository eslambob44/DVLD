using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccessLayer
{
    static public class clsLocalLicenseApplicationDataAccessLayer
    {
        static public int AddLocalLicenseApplication(int ApplicationID , int LicenseClassID)
        {
            int ID = -1;
            SqlConnection Connection = new SqlConnection(clsDataAccessLayerSettings.ConnectionString);
            string Query = @"Insert Into LocalDrivingLicenseApplications values
                            (@ApplicationID , @LicenseClassID)
                            Select SCOPE_IDENTITY();";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            Command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();
                if(Result != null && int.TryParse(Result.ToString() , out int Temp))
                {
                    ID = Temp;
                }
            }
            catch
            {

            }
            finally
            {
                Connection.Close();
            }
            return ID;
        }

        static public DataTable ListLicenseClasses()
        {
            DataTable dtLicenseClasses = new DataTable();
            SqlConnection Connection = new SqlConnection (clsDataAccessLayerSettings.ConnectionString);
            string Query = @"Select ClassName From LicenseClasses ";
            SqlCommand Command = new SqlCommand(Query, Connection);
            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.HasRows)
                {
                    dtLicenseClasses.Load (Reader);
                }
                Reader.Close();
            }
            catch
            {

            }
            finally { Connection.Close(); }
            return dtLicenseClasses;
        }

        static public int GetSameLicenseApplicationClass(int PersonID , int LicenseClassID)
        {
            int ApplicationID = -1;
            SqlConnection Connection = new SqlConnection(clsDataAccessLayerSettings.ConnectionString);
            string Query = @"Select Applications.ApplicationID from LocalDrivingLicenseApplications
                         inner join Applications on
                         Applications.ApplicationID = LocalDrivingLicenseApplications.ApplicationID
                         where ApplicantPersonID = @PersonID and LicenseClassID = @LicenseClassID and not ApplicationStatus  = 2";
            SqlCommand Command = new SqlCommand (Query, Connection);
            Command.Parameters.AddWithValue("@PersonID" , PersonID);
            Command.Parameters.AddWithValue("@LicenseClassID" , LicenseClassID);
            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();
                if (Result != null && int.TryParse(Result.ToString() , out int Temp))
                {
                    ApplicationID = Temp;
                }
            }
            catch
            {

            }
            finally
            {
                Connection.Close ();
            }
            return ApplicationID;
        }
    }
}
