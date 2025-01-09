using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccessLayer
{
    static public class clsUsersDataAccessLayer
    {
        static public int GetUserID(string UserName , string Password)
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

        static public DataTable ListUsers()
        {
            DataTable dtUsers = new DataTable();

            SqlConnection Connection = new SqlConnection(clsDataAccessLayerSettings.ConnectionString);
            string Query = @"Select UserID , Users.PersonID ,
                            FullName = (FirstName + ' ' + SecondName + ' ' + ThirdName + ' ' + LastName) ,
                            UserName , IsActive   From Users
                            inner join People 
                            on Users.PersonID = People.PersonID;";
            SqlCommand Command = new SqlCommand(Query,Connection);
            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if(Reader.HasRows)
                {
                    dtUsers.Load(Reader);
                }
                Reader.Close();
            }
            catch
            {

            }
            finally
            {
                Connection.Close();
            }
            return dtUsers;
        }

        static public int AddNewUser(int PersonID , string UserName , string Password , bool IsActive)
        {
            int UserID = -1;
            SqlConnection Connection = new SqlConnection(clsDataAccessLayerSettings.ConnectionString);
            string Query = @"Insert into Users( UserName , Password , IsActive , PersonID)
                            values( @UserName , @Password , @IsActive , @PersonID);
                            Select SCOPE_IDENTITY(); ";
            SqlCommand Command = new SqlCommand(Query,Connection);
            Command.Parameters.AddWithValue("@UserName", UserName);
            Command.Parameters.AddWithValue("@Password", Password);
            Command.Parameters.AddWithValue("@IsActive", IsActive);
            Command.Parameters.AddWithValue("@PersonID", PersonID);
            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();
                if(Result != null && int.TryParse(Result.ToString() , out int Temp)) 
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

        static public int GetUserID(int PersonID)
        {
            int IsUserExists  = -1;
            SqlConnection Connection = new SqlConnection(clsDataAccessLayerSettings.ConnectionString);
            string Query = "Select UserID From Users Where PersonID = @PersonID";
            SqlCommand Command = new SqlCommand(Query,Connection);
            Command.Parameters.AddWithValue("@PersonID", PersonID);
            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();
                if (Result != null && int.TryParse(Result.ToString(), out int Temp))
                {
                    IsUserExists = Temp;
                }
            }
            catch
            {

            }
            finally
            {
                Connection.Close();
            }
            return IsUserExists;
        }

        static public bool UpdateUser(int UserID , string UserName, bool IsActive)
        {
            int RowsAffected = 0;
            SqlConnection Connection = new SqlConnection(clsDataAccessLayerSettings.ConnectionString);
            string Query = @"Update Users
                            set UserName = @UserName,
	                        IsActive = @IsActive 
	                        where UserID = @UserID ";
            SqlCommand Command = new SqlCommand(Query ,Connection);
            Command.Parameters.AddWithValue("@UserID", UserID);
            Command.Parameters.AddWithValue("@UserName", UserName);
            Command.Parameters.AddWithValue("@IsActive" , IsActive);

            try
            {
                Connection.Open();
                RowsAffected = Command.ExecuteNonQuery();
            }
            catch
            {

            }
            finally 
            {
                Connection.Close(); 
            }
            return RowsAffected > 0;

        }

        static public bool DeleteUser(int UserID)
        {
            int RowsAffected = -1;
            SqlConnection Connection = new SqlConnection(clsDataAccessLayerSettings.ConnectionString);
            string Query = @"Delete From Users Where UserID = @UserID";
            SqlCommand Command = new SqlCommand (Query ,Connection);
            Command.Parameters.AddWithValue("@UserID", UserID);
            try
            {
                Connection.Open();
                RowsAffected = Command.ExecuteNonQuery();
            }
            catch
            {

            }
            finally
            {
                Connection.Close();
            }
            return RowsAffected > 0;
        }

        static public bool IsCorrectPassword(int UserID , string Password)
        {
            bool IsCorrect = false;
            SqlConnection Connection = new SqlConnection(clsDataAccessLayerSettings.ConnectionString);
            string Query = @"Select 1 From Users Where Password = @Password and UserID = @UserID";
            SqlCommand Command = new SqlCommand (Query ,Connection);
            Command.Parameters.AddWithValue("@UserID", UserID);
            Command.Parameters.AddWithValue("@Password", Password);
            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();
                if(Result != null && int.TryParse(Result.ToString() , out int Temp))
                {
                    IsCorrect = true;
                }
            }
            catch
            {

            }
            finally
            {
                Connection.Close();
            }
            return IsCorrect;

        }

        static public bool ChangePassword(int UserID, string OldPassword, string NewPassword)
        {
            int RowsAffected = -1;
            SqlConnection Connection = new SqlConnection(clsDataAccessLayerSettings.ConnectionString);
            string Query = @"Update Users
                            set Password = @NewPassword 
                            where UserID = @UserID and Password = @Password";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@UserID", UserID);
            Command.Parameters.AddWithValue("@Password", OldPassword);
            Command.Parameters.AddWithValue("@NewPassword", NewPassword);
            try
            {
                Connection.Open();
                RowsAffected = Command.ExecuteNonQuery();
            }
            catch
            {

            }
            finally
            {
                Connection.Close();
            }
            return RowsAffected > 0;
        }

        static public bool FindUser(int UserID , ref int PersonID , ref string UserName , ref bool IsActive)
        {
            bool IsFound = false;
            SqlConnection Connection  =new SqlConnection(clsDataAccessLayerSettings.ConnectionString);
            string Query = @"Select UserName , IsActive , PersonID From Users Where UserID = @UserID";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@UserID", UserID);
            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if(Reader.Read())
                {
                    IsFound = true;
                    PersonID = (int)Reader["PersonID"];
                    UserName = (string)Reader["UserName"];
                    IsActive = (bool)Reader["IsActive"];
                }
                Reader.Close();
            }
            catch
            {

            }
            finally 
            {
                Connection.Close(); 
            }
            return IsFound;
        }

        static public bool IsUserNameUsed(string UserName)
        {
            bool IsUsed = false;
            SqlConnection Connection = new SqlConnection(clsDataAccessLayerSettings.ConnectionString);
            string Query = @"Select COUNT(UserName) From Users Where UserName = @UserName";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@UserName", UserName);
            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();
                if (Result != null && int.TryParse(Result.ToString() , out int Temp))
                {
                    IsUsed = (Temp != 0);
                }
            }
            catch
            {

            }
            finally
            {
                Connection.Close();
            }
            return IsUsed;
        }
    }


}
