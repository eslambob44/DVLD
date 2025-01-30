using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccessLayer
{
    static public class clsTestAppointmentDataAccessLayer
    {
        static public int AddNewAppointment(int TestTypeID , int LocalDrivingLicenseApplicationID 
            , DateTime AppointmentDate , float PaidFees , int CreatedUserID , bool IsLocked ,int RetakeTestID)
        {
            int AppointmentID = -1;
            SqlConnection Connection = new SqlConnection(clsDataAccessLayerSettings.ConnectionString);
            string Query = @"Insert Into TestAppointments Values
                            (@TestTypeID , @LocalDrivingLicenseApplicationID , @AppointmentDate , @PaidFees
                            , @CreatedUserID , @IsLocked,@RetakeTestID);
                            SELECT SCOPE_IDENTITY();";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            Command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
            Command.Parameters.AddWithValue("@PaidFees", PaidFees);
            Command.Parameters.AddWithValue("@CreatedUserID", CreatedUserID);
            Command.Parameters.AddWithValue("@IsLocked", IsLocked);
            if (RetakeTestID == -1)
            { Command.Parameters.AddWithValue("@RetakeTestID", DBNull.Value); }
            else Command.Parameters.AddWithValue("@RetakeTestID", RetakeTestID);
            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();
                if (Result!=null && int.TryParse(Result.ToString() , out int Temp))
                {
                    AppointmentID = Temp;
                }
            }
            catch { }
            finally { Connection.Close(); }
            return AppointmentID;

        }

        static public bool UpdateAppointment(int AppointmentID, DateTime AppointmentDate)
        {
            bool IsUpdated = false;
            SqlConnection Connection = new SqlConnection (clsDataAccessLayerSettings.ConnectionString);
            string Query = @"Update TestAppointments
                            Set AppointmentDate = @AppointmentDate
                            Where TestAppointmentID = @AppointmentID";
            SqlCommand Command = new SqlCommand (Query, Connection);
            Command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
            Command.Parameters.AddWithValue("@AppointmentID", AppointmentID);
            try
            {
                Connection.Open();
                int RowsAffected = Command.ExecuteNonQuery();
                IsUpdated = RowsAffected > 0;

            }
            catch { }
            finally { Connection.Close(); }
            return IsUpdated;
        }

        static public bool FindAppointment(int AppointmentID , ref int TestTypeID , ref int LocalDrivingLicenseApplicationID,
            ref DateTime AppointmentDate , ref float PaidFees , ref int CreatedUserID ,ref bool IsLocked 
            , ref int RetakeTestApplicationID)
        {
            bool IsFound = true;
            SqlConnection Connection = new SqlConnection(clsDataAccessLayerSettings.ConnectionString);
            string Query = @"Select * From TestAppointments
                            Where @TestAppointmentID = @AppointmentID";
            SqlCommand Command = new SqlCommand (Query, Connection);
            Command.Parameters.AddWithValue("@AppointmentID", AppointmentID);
            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {
                    IsFound = true;
                    TestTypeID = (int)Reader["TestTypeID"];
                    LocalDrivingLicenseApplicationID = (int)Reader["LocalDrivingLicenseApplicationID"];
                    AppointmentDate = (DateTime)Reader["AppointmentDate"];
                    PaidFees = (float)(decimal)Reader["PaidFees"];
                    CreatedUserID = (int)Reader["CreatedByUserID"];
                    IsLocked = (bool)Reader["IsLocked"];
                    if (Reader["RetakeTestApplicationID"] == DBNull.Value) RetakeTestApplicationID = -1;
                    else RetakeTestApplicationID = (int)Reader["RetakeTestApplicationID"];
                }
                Reader.Close();
            }
            catch { }
            finally { Connection.Close(); }
            return IsFound;
        }


        static public bool FindAppointmentByRetakeTestApplicationID(int RetakeTestApplicationID, ref int TestTypeID, ref int LocalDrivingLicenseApplicationID,
            ref DateTime AppointmentDate, ref float PaidFees, ref int CreatedUserID, ref bool IsLocked
            , ref int AppointmentID)
        {
            bool IsFound = true;
            SqlConnection Connection = new SqlConnection(clsDataAccessLayerSettings.ConnectionString);
            string Query = @"Select * From TestAppointments
                            Where @RetakeTestApplicationID = @RetakeTestApplicationID";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@RetakeTestApplicationID", RetakeTestApplicationID);
            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {
                    IsFound = true;
                    TestTypeID = (int)Reader["TestTypeID"];
                    LocalDrivingLicenseApplicationID = (int)Reader["LocalDrivingLicenseApplicationID"];
                    AppointmentDate = (DateTime)Reader["AppointmentDate"];
                    PaidFees = (float)(decimal)Reader["PaidFees"];
                    CreatedUserID = (int)Reader["CreatedByUserID"];
                    IsLocked = (bool)Reader["IsLocked"];
                    AppointmentID = (int)Reader["AppointmentID"];
                }
                Reader.Close();
            }
            catch { }
            finally { Connection.Close(); }
            return IsFound;
        }

        static public DataTable ListAppointmentsByTestType(int LocalDrivingLicenseApplicationID ,int TestTypeID)
        {
            DataTable dtAppointments = new DataTable();
            SqlConnection Connection = new SqlConnection (clsDataAccessLayerSettings.ConnectionString);
            string Query = @"Select TestAppointmentID as [Appointment ID] , AppointmentDate as [Appointment Date],
                            PaidFees as [Paid Fees] , IsLocked as [Is Locked]
                            From TestAppointments Where TestTypeID = @TestTypeID
                            and LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if(Reader.HasRows)
                {
                    dtAppointments.Load(Reader);
                }
                Reader.Close();
            }
            catch { }
            finally { Connection.Close(); }
            return dtAppointments;  
        }

        static public bool LockAppointment(int AppointmentID)
        {
            bool IsUpdated = false;
            SqlConnection Connection = new SqlConnection(clsDataAccessLayerSettings.ConnectionString);
            string Query = @"Update TestAppointments
                            Set IsLocked = 1
                            Where TestAppointmentID = @AppointmentID";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@AppointmentID", AppointmentID);
            try
            {
                Connection.Open();
                int RowsAffected = Command.ExecuteNonQuery();
                IsUpdated = RowsAffected > 0;

            }
            catch { }
            finally { Connection.Close(); }
            return IsUpdated;
        }

        

    }

    
}
