using System;
using System.Collections.Generic;
using System.Data;
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

        static public DataTable ListLicenseClasses()
        {
            DataTable dtLicenseClasses = new DataTable();
            SqlConnection Connection = new SqlConnection (clsDataAccessLayerSettings.ConnectionString);
            string Query = @"Select ClassName From LicenseClasses ";
            SqlCommand Command = new SqlCommand(Query, Connection);
            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.HasRows)
                {
                    dtLicenseClasses.Load (Reader);
                }
                Reader.Close();
            }
            catch
            {

            }
            finally { Connection.Close(); }
            return dtLicenseClasses;
        }

        static public int GetActiveApplicationWithSameLicenseClass(int PersonID , int LicenseClassID)
        {
            int ApplicationID = -1;
            SqlConnection Connection = new SqlConnection(clsDataAccessLayerSettings.ConnectionString);
            string Query = @"Select Applications.ApplicationID from LocalDrivingLicenseApplications
                         inner join Applications on
                         Applications.ApplicationID = LocalDrivingLicenseApplications.ApplicationID
                         where ApplicantPersonID = @PersonID and LicenseClassID = @LicenseClassID and ApplicationStatus  = 1";
            SqlCommand Command = new SqlCommand (Query, Connection);
            Command.Parameters.AddWithValue("@PersonID" , PersonID);
            Command.Parameters.AddWithValue("@LicenseClassID" , LicenseClassID);
            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();
                if (Result != null && int.TryParse(Result.ToString() , out int Temp))
                {
                    ApplicationID = Temp;
                }
            }
            catch
            {

            }
            finally
            {
                Connection.Close ();
            }
            return ApplicationID;
        }

        static public DataTable ListLocalLicenseApplications()
        {
            DataTable dtLicenseApplications = new DataTable();
            SqlConnection Connection = new SqlConnection (clsDataAccessLayerSettings.ConnectionString);
            string Query = @"Select [L.D.L.AppID] = LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID ,[Driving Class] = ClassName ,NationalNo 
                            , FullName = (FirstName + ' '+SecondName + ' ' + ThirdName + ' '+LastName ) , ApplicationDate ,
                            [Passed Tests] = 
                            (
                            	Select Count(Tests.TestResult) from Tests inner join TestAppointments 
                            	on TestAppointments.TestAppointmentID = Tests.TestAppointmentID 
                            	Where Tests.TestResult = 1 
                            	and TestAppointments.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID
                            ) ,
                            Status = 
                            case
                            	when ApplicationStatus = 1 then 'New'
                            	when ApplicationStatus = 3 then 'Completed'
                            	when ApplicationStatus = 2 then 'Canceled'
                            END
                            From Applications
                            inner join 
                            (
                            	LocalDrivingLicenseApplications inner join LicenseClasses 
                            	on LocalDrivingLicenseApplications.LicenseClassID = LicenseClasses.LicenseClassID
                            )
                            on Applications.ApplicationID = LocalDrivingLicenseApplications.ApplicationID
                            inner join People on Applications.ApplicantPersonID = People.PersonID
                            ";
            SqlCommand Command = new SqlCommand(Query, Connection);
            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if(Reader .HasRows)
                {
                    dtLicenseApplications.Load(Reader);
                }
                Reader.Close ();
            }
            catch
            {

            }
            finally
            {
                Connection.Close ();
            }
            return dtLicenseApplications;
        }

        static public bool FindLocalLicenseApplication(int ldlApplication , ref int ApplicationID , ref int LicenseClassID,
                                    ref int PersonID, ref DateTime ApplicationDate,
                                    ref int ApplicationTypeID, ref short ApplicationStatus, ref DateTime LastStatusDate,
                                    ref float PaidFees, ref int CreatedUserID)
        {
            bool IsFound = false;
            SqlConnection Connection = new SqlConnection(clsDataAccessLayerSettings.ConnectionString);
            string Query = @"Select * From LocalDrivingLicenseApplications
                            Where LocalDrivingLicenseApplicationID = @ldlApplication";
            SqlCommand command = new SqlCommand(Query, Connection);
            command.Parameters.AddWithValue("@ldlApplication" , ldlApplication);
            try
            {
                Connection.Open();
                SqlDataReader Reader = command.ExecuteReader();

                if (Reader.Read())
                {
                    ApplicationID = (int)Reader["ApplicationID"];
                    LicenseClassID = (int)Reader["LicenseClassID"];
                    if(clsApplicationDataAccessLayer.FindApplication(ApplicationID,ref PersonID, ref ApplicationDate,
                        ref ApplicationTypeID, ref ApplicationStatus, ref LastStatusDate,
                        ref PaidFees, ref CreatedUserID))
                    {
                        IsFound = true;
                    }
                }

                Reader.Close ();
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

        static public int GetActiveTest(int ldlApplicationID)
        {
            int ActiveTest =-1 ;
            SqlConnection Connection = new SqlConnection (clsDataAccessLayerSettings.ConnectionString);
            string Query = @"Select ActiveTest = 
                            case 
                            	When BiggestTestID is null then 1
                            	when BiggestTestID between 1 and 2 then BiggestTestID+1
                                When BiggestTestID =3 then 0
                            	else -1
                            End
                            from
                            (
                            	Select max(TestTypeID) as BiggestTestID  From Tests
                            	inner join TestAppointments 
                            	on Tests.TestAppointmentID = TestAppointments.TestAppointmentID
                            	Where TestResult =1 and LocalDrivingLicenseApplicationID =@ldlApplicationID
                            )R1;";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@ldlApplicationID" , ldlApplicationID);
            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();
                if(Result != null && int.TryParse(Result.ToString() , out int Temp))
                {
                    ActiveTest = Temp;
                }
            }
            catch
            {
        
            }
            finally { Connection.Close(); }
            return ActiveTest;
        }

        static public int GetNumberOfPassedTests(int ldlApplicationID)
        {
            int PassedTests = -1;
            SqlConnection Connection = new SqlConnection(clsDataAccessLayerSettings.ConnectionString);
            string Query = @"Select Count(tests.TestID) from Tests
            inner join TestAppointments
            on Tests.TestAppointmentID = TestAppointments.TestAppointmentID
            where TestAppointments.LocalDrivingLicenseApplicationID = @ldlApplicationID;";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@ldlApplicationID", ldlApplicationID);
            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();
                if(Result!=null && int.TryParse (Result.ToString() , out int Temp))
                {
                    PassedTests= Temp;
                }
            }
            catch { }
            finally { Connection.Close(); }
            return PassedTests;
        }


    }
}
