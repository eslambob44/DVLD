using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccessLayer
{
    static public class clsUsersDataAccessLayer
    {
        static public int FindUser(string UserName , string Password)
        {
            int UserID = -1;
            SqlConnection Connection = new SqlConnection(clsDataAccessLayerSettings.ConnectionString);
            string Query = "Select UserID From Users Where UserName = @UserName and Password = @Password";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@UserName", UserName);
            Command.Parameters.AddWithValue("@Password", Password);
            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();
                if (Result != null && int.TryParse(Result.ToString() , out int Temp))
                {
                    UserID = Temp;
                }
            }
            catch
            {

            }
            finally
            {
                Connection.Close();
            }
            return UserID;
        }

        static public bool IsUserActive(int UserID)
        {
            bool IsUserActive = false;
            SqlConnection Connection = new SqlConnection(clsDataAccessLayerSettings.ConnectionString);
            string Query = "Select IsActive From Users Where UserID = @UserID";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@UserID" , UserID);
            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();
                if(Result != null && bool.TryParse(Result.ToString() ,out bool Temp))
                {
                    IsUserActive = Temp;
                }
            }
            catch
            {

            }
            finally
            {
                Connection.Close();
            }
            return IsUserActive;
        }

    }
}
