using System;
using System.Collections.Generic;
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
    }
}
