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
                            ,@LastStatusDate,@PaidFees,@CreatedApplicationUserID)
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

        static public bool FindApplication(int ApplicationID, ref int PersonID, ref DateTime ApplicationDate,
                                    ref int ApplicationTypeID, ref short ApplicationStatus, ref DateTime LastStatusDate,
                                    ref float PaidFees, ref int CreatedUserID)
        {
            bool IsFound = false;
            SqlConnection Connection = new SqlConnection(clsDataAccessLayerSettings.ConnectionString);
            string Query = @"Select * From Applications Where ApplicationID = @ApplicationID";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.Read())
                {
                    IsFound = true;
                    PersonID = (int)Reader["ApplicantPersonID"];
                    ApplicationDate = (DateTime)Reader["ApplicationDate"];
                    ApplicationTypeID = (int)Reader["ApplicationTypeID"];
                    ApplicationStatus = (byte)Reader["ApplicationStatus"];
                    LastStatusDate = (DateTime)Reader["LastStatusDate"];
                    PaidFees = (float)(decimal)Reader["PaidFees"];
                    CreatedUserID = (int)Reader["CreatedByUserID"];
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

        static public bool UpdateApplication(int ApplicationID,  int PersonID,  DateTime ApplicationDate,
                                     int ApplicationTypeID,  short ApplicationStatus,  DateTime LastStatusDate,
                                     float PaidFees,  int CreatedUserID)
        {
            bool IsUpdated = false;
            SqlConnection Connection = new SqlConnection(clsDataAccessLayerSettings.ConnectionString);
            string Query = @"Update Applications
                            set ApplicantPersonID = @PersonID ,
                            ApplicationDate = @ApplicationDate , 
                            ApplicationTypeID = @ApplicationTypeID ,
                            ApplicationStatus = @ApplicationStatus , 
                            LastStatusDate = @LastStatusDate , 
                            PaidFees = @PaidFees , 
                            CreatedByUserID = @CreatedUserID
                            Where ApplicationID = @ApplicationID";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@PersonID", PersonID);
            Command.Parameters.AddWithValue("@ApplicationDate", ApplicationDate);
            Command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
            Command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
            Command.Parameters.AddWithValue("@LastStatusDate", LastStatusDate);
            Command.Parameters.AddWithValue("@PaidFees", PaidFees);
            Command.Parameters.AddWithValue("@CreatedUserID", CreatedUserID);
            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();
                if (Result != null && int.TryParse(Result.ToString() , out int Temp))
                {
                    IsUpdated = Temp > 0;
                }
            }
            catch
            {

            }
            finally
            {
                Connection.Close();
            }
            return IsUpdated;


        }


       static public bool UpdateApplicationStatus(int ApplicationID , short ApplicationStatus) 
       {
            bool IsUpdated=false;
            SqlConnection Connection = new SqlConnection(clsDataAccessLayerSettings.ConnectionString);
            string Query = @"Update Applications
                            set ApplicationStatus = @ApplicationStatus
                            Where ApplicationID = @ApplicationID and ApplicationStatus = 1";
            SqlCommand Command = new SqlCommand (Query, Connection);
            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            Command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
            try
            {
                Connection.Open();
                int RowsAffected = Command.ExecuteNonQuery();
            }
            catch { }
            finally { Connection.Close(); }
            return IsUpdated;
        }

    }



    
}
