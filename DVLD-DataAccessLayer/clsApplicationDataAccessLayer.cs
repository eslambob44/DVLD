using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccessLayer
{
    static public class clsApplicationDataAccessLayer
    {
        static public int AddApplication(int ApplicationPersonID , DateTime ApplicationDate,int ApplicationType , short ApplicationStatus
                                         ,DateTime LastStatusDate , float PaidFees , int CreatedApplicationUserID)
        {
            int ApplicationID = -1;
            SqlConnection Connection = new SqlConnection(clsDataAccessLayerSettings.ConnectionString);
            string Query = @"Insert Into Applications values
                            (@ApplicationPersonID,@ApplicationDate,@ApplicationType,@ApplicationStatus
                            @LastStatusDate,@PaidFees,@CreatedApplicationUserID)
                            Select SCOPE_IDENTITY();";
            SqlCommand Command = new SqlCommand(Query,Connection);
            Command.Parameters.AddWithValue("@ApplicationPersonID", ApplicationPersonID);
            Command.Parameters.AddWithValue("@ApplicationDate", ApplicationDate);
            Command.Parameters.AddWithValue("@ApplicationType", ApplicationType);
            Command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
            Command.Parameters.AddWithValue("@LastStatusDate", LastStatusDate);
            Command.Parameters.AddWithValue("@PaidFees", PaidFees);
            Command.Parameters.AddWithValue("@CreatedApplicationUserID", CreatedApplicationUserID);
            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();
                if(Result != null && int.TryParse(Result.ToString() , out int Temp))
                {
                    ApplicationID= Temp;
                }
            }
            catch
            {

            }
            finally
            {
                Connection.Close();
            }
            return ApplicationID;
        }
    }
}
