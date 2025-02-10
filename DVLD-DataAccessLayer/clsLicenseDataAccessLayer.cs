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
                if (Result != null && int.TryParse(Result.ToString() , out int Temp))
                {
                    IsHasLicense = (Temp ==1);
                }
            }
            catch { }
            finally { Connection.Close(); }
            return IsHasLicense;
        }

        static public int AddLicense(int ApplicationID , int DriverID , int LicenseClassID , DateTime IssueDate , DateTime ExpirationDate,
            string Notes , float PaidFees , bool IsActive , byte IssueReason , int CreatedUserID)
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



        static public bool FindLicense(int LicenseID , ref int ApplicationID , ref int DriverID , ref int LicenseClassID,
            ref DateTime IssueDate , ref DateTime ExpirationDate , ref string Notes , ref float PaidFees ,
            ref bool IsActive , ref byte IssueReason , ref int CreatedUserID)
        {
            bool IsFound = false;
            SqlConnection Connection = new SqlConnection(clsDataAccessLayerSettings.ConnectionString);
            string Query = "Select * From Licenses where LicenseID = @LicenseID";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@LicenseID" , LicenseID);
            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if(Reader.Read())
                {
                    IsFound = true;
                    ApplicationID = (int)Reader["ApplicationID"];
                    DriverID = (int)Reader["DriverID"];
                    LicenseClassID = (int)Reader["LicenseClass"];
                    IssueDate = (DateTime)Reader["IssueDate"];
                    ExpirationDate = (DateTime)Reader["ExpirationDate"];
                    Notes = (Reader["Notes"] == DBNull.Value) ? string.Empty : (string)Reader["Notes"];
                    PaidFees = (float)(decimal)Reader["PaidFees"];
                    IsActive = (bool)Reader["IsActive"];
                    IssueReason = (byte)Reader["IssueReason"];
                    CreatedUserID = (int)Reader["CreatedByUserID"];
                }
                Reader.Close();
            }
            catch { }
            finally { Connection.Close(); }
            return IsFound;
        }

        static public bool FindLicenseByApplicationID(int ApplicationID, ref int LicenseID, ref int DriverID, ref int LicenseClassID,
            ref DateTime IssueDate, ref DateTime ExpirationDate, ref string Notes, ref float PaidFees,
            ref bool IsActive, ref byte IssueReason, ref int CreatedUserID)
        {
            bool IsFound = false;
            SqlConnection Connection = new SqlConnection(clsDataAccessLayerSettings.ConnectionString);
            string Query = "Select * From Licenses where ApplicationID = @ApplicationID";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.Read())
                {
                    IsFound = true;
                    LicenseID = (int)Reader["LicenseID"];
                    DriverID = (int)Reader["DriverID"];
                    LicenseClassID = (int)Reader["LicenseClassID"];
                    IssueDate = (DateTime)Reader["IssueDate"];
                    ExpirationDate = (DateTime)Reader["ExpirationDate"];
                    Notes = (Reader["Notes"] == DBNull.Value) ? string.Empty : (string)Reader["Notes"];
                    PaidFees = (float)(decimal)Reader["PaidFees"];
                    IsActive = (bool)Reader["IsActive"];
                    IssueReason = (byte)Reader["IssueReason"];
                    CreatedUserID = (int)Reader["CreatedByUserID"];
                }
                Reader.Close();
            }
            catch { }
            finally { Connection.Close(); }
            return IsFound;
        }

        static  public bool IsLicenseDetained(int LicenseID)
        {
            bool IsDetained = false;
            SqlConnection Connection = new SqlConnection(clsDataAccessLayerSettings.ConnectionString);
            string Query = @"Select Count(LicenseID)   From DetainedLicenses
                            Where IsReleased = 0 and LicenseID = @LicenseID;";
            SqlCommand command = new SqlCommand(Query, Connection);
            command.Parameters.AddWithValue("@LicenseID" , LicenseID);
            try
            {
                Connection.Open();
                object Result = command.ExecuteScalar();
                if(Result != null)
                {
                    IsDetained = int.Parse(Result.ToString())!=0;
                }
            }
            catch { }
            finally { Connection.Close(); }
            return IsDetained;
        }

        static public bool UpdateLicenseActiveStatus(int LicenseID , bool IsActive)
        {
            bool IsUpdated = false;
            SqlConnection Connection = new SqlConnection(clsDataAccessLayerSettings.ConnectionString);
            string Query = @"Update Licenses
                            set IsActive = @IsActive
                            Where LicenseID = @LicenseID";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@IsActive" , IsActive);
            Command.Parameters.AddWithValue("@LicenseID" , LicenseID );
            try
            {
                Connection.Open();
                int RowsAffected = Command.ExecuteNonQuery();   
                IsUpdated  = RowsAffected > 0;
            }
            catch { }
            finally { Connection.Close(); }
            return IsUpdated;
        }
    }
}
