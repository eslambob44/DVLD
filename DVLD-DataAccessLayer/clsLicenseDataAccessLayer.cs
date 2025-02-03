using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD_DataAccessLayer
{
    static public class clsLicenseDataAccessLayer
    {

        static public bool IsDriverHasLicenseFromLicenseClass(int DriverID , int LicenseClassID )
        {
            bool IsHasLicense = false;
            SqlConnection Connection = new SqlConnection(clsDataAccessLayerSettings.ConnectionString);
            string Query = @"Select HasClass = 
                            case
                            	when Count(R1.koko) >0 then 1
                            	else 0
                            end
                            from
                            (
                            	Select 1 as koko From Licenses
                            	Where DriverID =@DriverID and LicenseClass =@LicenseClassID
                            )R1";
            SqlCommand Command = new SqlCommand(Query,Connection);
            Command.Parameters.AddWithValue("@DriverID" , DriverID);
            Command.Parameters.AddWithValue("@LicenseClassID" , LicenseClassID);
            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();
                if (Result != null && bool.TryParse(Result.ToString() , out bool Temp))
                {
                    IsHasLicense = Temp;
                }
            }
            catch { }
            finally { Connection.Close(); }
            return IsHasLicense;
        }

        static public int AddLicense(int ApplicationID , int DriverID , int LicenseClassID , DateTime IssueDate , DateTime ExpirationDate,
            string Notes , float PaidFees , bool IsActive , int IssueReason , int CreatedUserID)
        {
            int LicenseID = -1;
            SqlConnection Connection = new SqlConnection(clsDataAccessLayerSettings.ConnectionString);

            string Query = @"INSERT INTO Licenses VALUES
                        (@ApplicationID, @DriverID, @LicenseClassID, @IssueDate, @ExpirationDate, @Notes, 
                        @PaidFees, @IsActive, @IssueReason, @CreatedUserID);
                        SELECT SCOPE_IDENTITY();";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            Command.Parameters.AddWithValue("@DriverID", DriverID);
            Command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
            Command.Parameters.AddWithValue("@IssueDate", IssueDate);
            Command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
            if(string.IsNullOrEmpty(Notes))
                Command.Parameters.AddWithValue("@Notes",  DBNull.Value );
            else
                Command.Parameters.AddWithValue("@Notes", Notes);
            Command.Parameters.AddWithValue("@PaidFees", PaidFees);
            Command.Parameters.AddWithValue("@IsActive", IsActive);
            Command.Parameters.AddWithValue("@IssueReason", IssueReason);
            Command.Parameters.AddWithValue("@CreatedUserID", CreatedUserID);
            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();
                if (Result != null && int.TryParse(Result.ToString() , out int Temp))
                {
                    LicenseID = Temp;
                }
            }
            catch { }
            finally { Connection.Close(); } 
            return LicenseID;

        }



        


    }
}
