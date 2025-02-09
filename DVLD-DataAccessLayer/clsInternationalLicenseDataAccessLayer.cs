using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccessLayer
{
    static public class clsInternationalLicenseDataAccessLayer
    {
        static public int AddInternationalLicense(int ApplicationID , int DriverID , int LocalLicenseID 
            , DateTime IssueDate , DateTime ExpirationDate , bool IsActive , int CreatedUserID)
        {
            int InternationalLicenseID = -1;
            SqlConnection Connection = new SqlConnection(clsDataAccessLayerSettings.ConnectionString);
            string Query = @"INSERT INTO [dbo].[InternationalLicenses]
                             VALUES
                                   (@ApplicationID,
	                        		@DriverID,
	                        		@LocalLicenseID,
	                        		@IssueDate,
	                        		@ExpirationDate,
	                        		@IsActive,
	                        		@CreatedUserID);
                                    Select SCOPE_IDENTITY();";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            Command.Parameters.AddWithValue("@DriverID", DriverID);
            Command.Parameters.AddWithValue("@LocalLicenseID", LocalLicenseID);
            Command.Parameters.AddWithValue("@IssueDate", IssueDate);
            Command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
            Command.Parameters.AddWithValue("@IsActive", IsActive);
            Command.Parameters.AddWithValue("@CreatedUserID", CreatedUserID);
            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();
                if(Result != null&& int.TryParse(Result.ToString() , out int Temp))
                {
                    InternationalLicenseID = Temp;
                }
            }
            catch { }
            finally { Connection.Close(); }
            return InternationalLicenseID;
        }

        static public bool FindInternationalLicense(int InternationalLicenseID, ref int ApplicationID,
            ref int DriverID, ref int LocalLicenseID, ref DateTime IssueDate, ref DateTime ExpirationDate
            , ref bool IsActive, ref int CreatedUserID)
        {
            bool IsFound = false;
            SqlConnection Connection = new SqlConnection(clsDataAccessLayerSettings.ConnectionString);
            string Query = @"Select * From [InternationalLicenses] Where InternationalLicenseID = @InternationalLicenseID";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);
            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if(Reader.Read())
                {
                    IsFound = true;
                    ApplicationID = (int)Reader["ApplicationID"];
                    DriverID = (int)Reader["DriverID"];
                    LocalLicenseID = (int)Reader["IssuedUsingLocalLicenseID"];
                    IssueDate = (DateTime)Reader["IssueDate"];
                    ExpirationDate = (DateTime)Reader["ExpirationDate"];
                    IsActive = (bool)Reader["IsActive"];
                    CreatedUserID = (int)Reader["CreatedByUserID"];
                }
                Reader.Close();
            }
            catch { }
            finally { Connection.Close(); }
            return IsFound; 
        }

        static public int GetActiveInternationalLicenseID(int DriverID)
        {
            int InternationalLicenseID = -1;
            SqlConnection Connection = new SqlConnection(clsDataAccessLayerSettings.ConnectionString);
            string Query = @"Select InternationalLicenseID From InternationalLicenses
                            Where DriverID = @DriverID and IsActive=1;";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@DriverID", DriverID);
            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();
                if(Result != null && int.TryParse(Result.ToString() , out int Temp))
                {
                    InternationalLicenseID= Temp;
                }
            }
            catch { }
            finally { Connection.Close(); }
            return InternationalLicenseID;
        }

        static public DataTable ListInternationalLicenses(int DriverID)
        {
            DataTable dtInternationalLicenses = new DataTable();
            SqlConnection Connection = new SqlConnection(clsDataAccessLayerSettings.ConnectionString);
            string Query = @"SELECT  [InternationalLicenseID] as [Int.License ID]
                            ,[ApplicationID] as [Application ID]
                            ,[IssuedUsingLocalLicenseID] as [L.License ID]
                            ,[IssueDate] as [Issue Date],[ExpirationDate] as [Expiration Date]
                            ,[IsActive] as [Is Active] FROM [InternationalLicenses]
                            Where DriverID = @DriverID";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@DriverID" , DriverID);
            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader(); 
                if(Reader.HasRows)
                {
                    dtInternationalLicenses.Load(Reader);
                }
                Reader.Close();
            }
            catch { }
            finally { Connection.Close(); }
            return dtInternationalLicenses;
        }

        static public DataTable ListInternationalLicenses()
        {
            DataTable dtInternationalLicenses = new DataTable();
            SqlConnection Connection = new SqlConnection(clsDataAccessLayerSettings.ConnectionString);
            string Query = @"SELECT  [InternationalLicenseID] as [Int.License ID]
                            ,[ApplicationID] as [Application ID]
                            ,[DriverID] as [Driver ID]
                            ,[IssuedUsingLocalLicenseID] as [L.License ID]
                            ,[IssueDate] as [Issue Date],[ExpirationDate] as [Expiration Date]
                            ,[IsActive] as [Is Active] FROM [InternationalLicenses]";
            SqlCommand Command = new SqlCommand(Query, Connection);
            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.HasRows)
                {
                    dtInternationalLicenses.Load(Reader);
                }
                Reader.Close();
            }
            catch { }
            finally { Connection.Close(); }
            return dtInternationalLicenses;
        }
    }
}
