using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccessLayer
{
    static public class clsDriverDataAccessLayer
    {
        static public int AddDriver(int PersonID , int CreatedUserID , DateTime CreatedDate)
        {
            int DriverID = -1;
            SqlConnection Connection = new SqlConnection(clsDataAccessLayerSettings.ConnectionString);
            string Query = @"Insert Into Drivers VALUES
                            (@PersonID , @CreatedUserID , @CreatedDate);
                            SELECT SCOPE_IDENTITY();";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@PersonID", PersonID);
            Command.Parameters.AddWithValue("@CreatedUserID", CreatedUserID);
            Command.Parameters.AddWithValue("@CreatedDate", CreatedDate);
            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();
                if(Result != null && int.TryParse(Result.ToString() , out int Temp))
                {
                    DriverID = Temp;
                }
            }
            catch { }
            finally { Connection.Close(); }
            return DriverID;
        }

        static public bool FindDriver(int DriverID, ref int PersonID, ref int CreatedUserID, ref DateTime CreatedDate)
        {
            bool IsFound = false;
            SqlConnection Connection = new SqlConnection(clsDataAccessLayerSettings.ConnectionString);
            string Query = @"Select * From Drivers Where DriverID = @DriverID";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@DriverID", DriverID);
            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if(Reader.Read())
                {
                    IsFound= true;
                    PersonID = (int)Reader["PersonID"];
                    CreatedUserID = (int)Reader["CreatedByUserID"];
                    CreatedDate = (DateTime)Reader["CreatedDate"];
                }
                Reader.Close();
            }
            catch { }
            finally { Connection.Close(); }
            return IsFound;
        }

        static public bool IsPersonADriver(int PersonID) 
        {
            bool IsDriver = false;
            SqlConnection Connection = new SqlConnection(clsDataAccessLayerSettings.ConnectionString);
            string Query = @"Select IsDriver = 
                            case
                                when (Select DriverID From Drivers Where PersonID = @PersonID ) is not null then 1
                                else 0
                            end";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@PersonID", PersonID);
            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();
                if(Result != null)
                    IsDriver = Result.ToString() =="1";
                
            }
            catch { }
            finally { Connection.Close(); }
            return IsDriver;
        }

        static public int GetDriverID(int PersonID)
        {
            int DriverID = -1;
            SqlConnection Connection = new SqlConnection(clsDataAccessLayerSettings.ConnectionString);
            string Query = @"Select DriverID From Drivers Where PersonID = @PersonID";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@PersonID", PersonID);
            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();
                if (Result != null&& int.TryParse(Result.ToString() , out int Temp))
                    DriverID = Temp;

            }
            catch { }
            finally { Connection.Close(); }
            return DriverID;
        }

        static public DataTable ListDrivers()
        {
            SqlConnection Connection = new SqlConnection(clsDataAccessLayerSettings.ConnectionString);
            DataTable dtDrivers = new DataTable();
            string Query = @"Select DriverID as [Driver ID] , Drivers.PersonID as [Person ID] ,NationalNo as [National Number]
                            , [Full Name] = (FirstName + ' ' + SecondName + ' ' + ThirdName + ' ' + LastName) 
                            , CreatedDate as Date , 
                            [Active Licenses] = 
                            (
                                Select Count(LicenseID) From Licenses 
                                Where DriverID = Drivers.DriverID and IsActive = 1
                            )
                            from Drivers
                            inner join People on 
                            Drivers.PersonID = People.PersonID";
            SqlCommand Command = new SqlCommand(Query, Connection);
            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if(Reader.HasRows)
                {
                    dtDrivers.Load(Reader);
                }
                Reader.Close();
            }
            catch { }
            finally { Connection.Close(); }
            return dtDrivers;
        }

        static public DataTable ListLocalLicenses(int DriverID)
        {
            DataTable dtLicenses = new DataTable();
            SqlConnection Connection = new SqlConnection(clsDataAccessLayerSettings.ConnectionString);
            string Query = @"Select LicenseID as [Lic.ID] , ApplicationID as [App.ID] , [Class Name] = 
                            (
                            	Select ClassName From LicenseClasses Where LicenseClasses.LicenseClassID = Licenses.LicenseClass
                            ) 
                            ,IssueDate as [Issue Date] , ExpirationDate as [Expiration Date] , IsActive as [Is Active]
                            From Licenses
                            
                            Where DriverID = @DriverID
                            Order By IsActive DESC";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@DriverID", DriverID);
            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.HasRows)
                    dtLicenses.Load(Reader);
            }
            catch { }
            finally { Connection.Close(); }
            return dtLicenses;
        }

    }
}
