using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccessLayer
{
    static public class clsDetainedLicenseDataAccessLayer
    {
        static public int AddDetainedLicense(int LicenseID, DateTime DetainDate, float FineFees,
                int CreatedUserID)
        {
            int DetainedLicenseID = -1;
            SqlConnection Connection = new SqlConnection(clsDataAccessLayerSettings.ConnectionString);
            string query = @"
            INSERT INTO DetainedLicenses VALUES 
            (, @LicenseID, @DetainDate, @FineFees, @CreatedByUserID, @IsReleased, @ReleaseDate,
                @ReleasedByUserID, @ReleaseApplicationID);
                Select SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, Connection);

            command.Parameters.AddWithValue("@LicenseID", LicenseID);
            command.Parameters.AddWithValue("@DetainDate", DetainDate);
            command.Parameters.AddWithValue("@FineFees", FineFees);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedUserID);
            command.Parameters.AddWithValue("@IsReleased", false);
            command.Parameters.AddWithValue("@ReleaseDate", DBNull.Value);
            command.Parameters.AddWithValue("@ReleasedByUserID", DBNull.Value);
            command.Parameters.AddWithValue("@ReleaseApplicationID", DBNull.Value);

            try
            {
                Connection.Open();
                object Result = command.ExecuteScalar();
                if (Result != null && int.TryParse(Result.ToString(), out int Temp))
                {
                    DetainedLicenseID = Temp;
                }
            }
            catch { }
            finally { Connection.Close(); }
            return DetainedLicenseID;
        }


        static public bool ReleaseLicense(int LicenseID, DateTime ReleaseDate, int ReleasedUserID, int ApplicationID)
        {
            bool IsReleased = false;
            SqlConnection Connection = new SqlConnection(clsDataAccessLayerSettings.ConnectionString);
            string Query = @"UPDATE DetainedLicenses
            SET 
                IsReleased = True,
                ReleaseDate = @ReleaseDate,
                ReleasedByUserID = @ReleasedByUserID,
                ReleaseApplicationID = @ReleaseApplicationID
            WHERE LicenseID = @LicenseID and IsReleased = False;";
            SqlCommand command = new SqlCommand(Query, Connection);
            command.Parameters.AddWithValue("@ReleaseDate", ReleaseDate);
            command.Parameters.AddWithValue("@ReleasedByUserID", ReleasedUserID);
            command.Parameters.AddWithValue("ReleaseApplicationID", ApplicationID);
            command.Parameters.AddWithValue("@LicenseID", LicenseID);
            try
            {
                Connection.Open();
                int RowsAffected = command.ExecuteNonQuery();
                IsReleased = RowsAffected > 0;
            }
            catch { }
            finally { Connection.Close(); }
            return IsReleased;

        }

        static public bool Find(int detainID,
        ref int licenseID,
        ref DateTime detainDate,
        ref float fineFees,
        ref int createdByUserID,
        ref bool isReleased,
        ref DateTime releaseDate,
        ref int releasedByUserID,
        ref int releaseApplicationID)
        {
            bool IsFound = false;
            SqlConnection Connection = new SqlConnection(clsDataAccessLayerSettings.ConnectionString);
            string Query = @"Select * From DetainedLicenses
                            WHERE DetainID = @DetainID;";
            SqlCommand command = new SqlCommand(Query, Connection);
            command.Parameters.AddWithValue("@DetainID", detainID);
            try
            {
                Connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    IsFound = true;
                    licenseID = reader.GetInt32(reader.GetOrdinal("LicenseID"));
                    detainDate = reader.GetDateTime(reader.GetOrdinal("DetainDate"));
                    fineFees = (float)(reader.GetDecimal(reader.GetOrdinal("FineFees")));
                    createdByUserID = reader.GetInt32(reader.GetOrdinal("CreatedByUserID"));
                    isReleased = reader.GetBoolean(reader.GetOrdinal("IsReleased"));

                    if (reader["ReleaseDate"] == DBNull.Value)
                    {
                        releaseDate = DateTime.MinValue;
                    }
                    else
                    {
                        releaseDate = (DateTime)reader["ReleaseDate"];
                    }

                    if (reader["ReleaseApplicationID"] == DBNull.Value)
                    {
                        releaseApplicationID = -1;
                    }
                    else
                    {
                        releaseApplicationID = (int)reader["ReleaseApplicationID"];
                    }
                    if (reader["ReleasedByUserID"] == DBNull.Value)
                    {
                        releasedByUserID = -1;
                    }
                    else
                    {
                        releasedByUserID = (int)reader["ReleasedByUserID"];
                    }
                }
                reader.Close();
            }
            catch { }
            finally { Connection.Close(); }
            return IsFound;
        }



        static public bool Find(int licenseID,
        ref int detainID,
        ref DateTime detainDate,
        ref float fineFees,
        ref int createdByUserID,
        ref DateTime releaseDate,
        ref int releasedByUserID,
        ref int releaseApplicationID)
        {
            bool IsFound = false;
            SqlConnection Connection = new SqlConnection(clsDataAccessLayerSettings.ConnectionString);
            string Query = @"Select * From DetainedLicenses
                            WHERE LicenseID = @LicenseID;";
            SqlCommand command = new SqlCommand(Query, Connection);
            command.Parameters.AddWithValue("@LicenseID", licenseID);
            try
            {
                Connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    IsFound = true;
                    detainID = reader.GetInt32(reader.GetOrdinal("DetainID"));
                    detainDate = reader.GetDateTime(reader.GetOrdinal("DetainDate"));
                    fineFees = (float)(reader.GetDecimal(reader.GetOrdinal("FineFees")));
                    createdByUserID = reader.GetInt32(reader.GetOrdinal("CreatedByUserID"));

                    if (reader["ReleaseDate"] == DBNull.Value)
                    {
                        releaseDate = DateTime.MinValue;
                    }
                    else
                    {
                        releaseDate = (DateTime)reader["ReleaseDate"];
                    }

                    if (reader["ReleaseApplicationID"] == DBNull.Value)
                    {
                        releaseApplicationID = -1;
                    }
                    else
                    {
                        releaseApplicationID = (int)reader["ReleaseApplicationID"];
                    }
                    if (reader["ReleasedByUserID"] == DBNull.Value)
                    {
                        releasedByUserID = -1;
                    }
                    else
                    {
                        releasedByUserID = (int)reader["ReleasedByUserID"];
                    }
                }
                reader.Close();
            }
            catch { }
            finally { Connection.Close(); }
            return IsFound;
        }


        static public DataTable ListDetainedLicenses()
        {
            DataTable dtDetainedLicenses = new DataTable();
            SqlConnection Connection = new SqlConnection(clsDataAccessLayerSettings.ConnectionString);
            string Query = @"Select DetainedLicenses.DetainID as [D.ID] , DetainedLicenses.LicenseID as [L.ID]
                            , DetainedLicenses.DetainDate as [D.Date] , DetainedLicenses.IsReleased as [Is Released]
                            , FineFees as [Fine Fees] , [Release Date] =ReleaseDate
                            ,[N.NO.] = People.NationalNo , [Full Name] = FirstName+' ' + SecondName + ' ' + ThirdName + ' ' + LastName,
                            [Release App ID] = ReleaseApplicationID
                            
                            From DetainedLicenses
                            full join Applications
                            on Applications.ApplicationID = DetainedLicenses.ReleaseApplicationID
                            inner join 
                            (
                            	Licenses inner join 
                            	(
                            		Drivers inner join People 
                            		on Drivers.PersonID = People.PersonID
                            	)
                            	on Licenses.DriverID = Drivers.DriverID
                            )
                            on Licenses.LicenseID = DetainedLicenses.LicenseID;";
            SqlCommand Command = new SqlCommand(Query, Connection);
            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if(Reader.HasRows)
                    dtDetainedLicenses.Load(Reader);
                Reader.Close();
            }
            catch { }
            finally { Connection.Close(); }
            return dtDetainedLicenses;
        }
    }
}
