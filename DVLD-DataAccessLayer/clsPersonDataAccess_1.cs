using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;


namespace DVLD_DataAccessLayer
{
    static public class clsPersonDataAccess
    {

        static public DataTable ListPeople(string Filter , string Value)
        {
            DataTable dtPeople = new DataTable();
            SqlConnection Connection = new SqlConnection(clsDataAccessLayerSettings.ConnectionString);
            string Query = @"Select PersonID , NationalNo , FirstName , SecondName , ThirdName ,LastName , Gendor = 
                            case
                            	when Gendor = 0 then 'Male'
                            	else 'Female'
                            end
                            , DateOfBirth , CountryName as Nationality , Phone , Email
                            From People
                            inner join Countries
                            on Countries.CountryID = People.NationalityCountryID";
            if (!string.IsNullOrEmpty(Filter)) Query += " Where " + Filter + " like '%'+Value+'%'";
            SqlCommand Command = new SqlCommand(Query, Connection);
            if(!string.IsNullOrEmpty(Filter)) Command.Parameters.AddWithValue("@Value" , Value);
            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.HasRows)
                {
                    dtPeople.Load(Reader);
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
            return dtPeople;
        }

        static public DataTable ListPeople()
        {
            return ListPeople("", "");
        }

        static public bool FindPerson( int PersonID , out string NationalityNumberID
                                            , out string FirstName , out string SecondName , out string ThirdName , out string LastName
                                            , out DateTime DateOfBirth , out byte Gendor , out string Address , out string Phone
                                            , out string Email , out int NationalityCountryID , out string ImagePath)
        {
            bool IsFound = false;
            SqlConnection Connection = new SqlConnection(clsDataAccessLayerSettings.ConnectionString);
            string Query = @"Select * From People
                             Where PersonID = @PersonID";
            
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@PersonID" , PersonID );
            NationalityNumberID = "";
            FirstName = "";
            SecondName = "";
            ThirdName = "";
            LastName = "";
            DateOfBirth = DateTime.MinValue;
            Gendor = 0;
            Address = "";
            Phone = "";
            Email = "";
            NationalityCountryID = -1;
            ImagePath = "";
            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.Read())
                {
                    IsFound = true;
                    NationalityNumberID = (string)Reader["NationalNo"];
                    FirstName = (string)Reader["FirstName"];
                    SecondName = (string)Reader["SecondName"];
                    ThirdName = (string)Reader["ThirdName"];
                    LastName = (string)Reader["LastName"];
                    DateOfBirth = (DateTime)Reader["DateOfBirth"];
                    Gendor = (byte)Reader["Gendor"];
                    Address = (string)Reader["Address"];
                    Phone = (string)Reader["Phone"];
                    Email = (Reader["Email"] != DBNull.Value)?(string)Reader["Email"]:null;
                    NationalityCountryID = (int)Reader["NationalityCountryID"];
                    ImagePath = (Reader["ImagePath"] != DBNull.Value)?(string)Reader["ImagePath"]:null;
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

        

        static public int AddNewPerson( string NationalNumber
            , string FirstName, string SecondName, string ThirdName, string LastName
            , DateTime DateOfBirth, byte Gendor, string Address, string Phone
            , string Email, int NationalityCountryID , string ImagePath)
        {

            int PersonID = -1;
            SqlConnection Connection = new SqlConnection(clsDataAccessLayerSettings.ConnectionString);
            string Query = @"Insert Into People(NationalNo , FirstName , SecondName , ThirdName , LastName
                            , DateOfBirth , Gendor , Address , Phone , Email 
                            , NationalityCountryID , ImagePath)
                            Values (@NationalNumber , @FirstName , @SecondName , @ThirdName , @LastName 
                            , @DateOfBirth , @Gendor , @Address , @Phone , @Email , @NationalityCountryID 
                            , @ImagePath)

                            Select SCOPE_IDENTITY(); ";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@NationalNumber", NationalNumber);
            Command.Parameters.AddWithValue("@FirstName", FirstName);
            Command.Parameters.AddWithValue("@SecondName", SecondName);
            Command.Parameters.AddWithValue("@ThirdName", ThirdName);
            Command.Parameters.AddWithValue("@LastName", LastName);
            Command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            Command.Parameters.AddWithValue("@Gendor", Gendor);
            Command.Parameters.AddWithValue("@Address", Address);
            Command.Parameters.AddWithValue("@Phone", Phone);
            if (!string.IsNullOrEmpty(Email)) Command.Parameters.AddWithValue("@Email", Email);
            else Command.Parameters.AddWithValue("@Email", DBNull.Value);
            Command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);
            if(!string.IsNullOrEmpty(ImagePath)) Command.Parameters.AddWithValue("@ImagePath", ImagePath);
            else Command.Parameters.AddWithValue("@ImagePath", DBNull.Value);

            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();
                if (Result != null && int.TryParse(Result.ToString() , out int TempID))
                {
                    PersonID = TempID;
                }
            }
            catch
            {

            }
            finally
            {
                Connection.Close();
            }

            return PersonID;
        }

        static public bool DeletePerson(int PersonID)
        {
            bool IsDeleted = false;
            SqlConnection Connection = new SqlConnection(clsDataAccessLayerSettings.ConnectionString);
            string Query = "Delete From People Where PersonID = @PersonID";
            SqlCommand Command = new SqlCommand(Query, Connection); 
            Command.Parameters.AddWithValue("@PersonID" , PersonID);
            try
            {
                Connection.Open();
                object Result = Command.ExecuteNonQuery();
                if(Result != null && int.TryParse(Result.ToString(), out int RowsAffected))
                {
                    IsDeleted = RowsAffected > 0;
                }
            }
            catch
            {

            }
            finally
            {
                Connection.Close();
            }

            return IsDeleted;
        }

        static public bool UpdatePerson(int PersonID,string NationalNumber
            , string FirstName, string SecondName, string ThirdName, string LastName
            , DateTime DateOfBirth, byte Gendor, string Address, string Phone
            , string Email, int NationalityCountryID, string ImagePath)
        {
            bool IsUpdated = false;
            SqlConnection Connection = new SqlConnection(clsDataAccessLayerSettings.ConnectionString);
            string Query = @"Update People
                            set NationalNo = @NationalNumber,
                                FirstName = @FirstName,
                                SecondName = @SecondName,
                                ThirdName = @ThirdName,
                                LastName = @LastName,
                                DateOfBirth = @DateOfBirth,
                                Gendor = @Gendor,
                                Address = @Address,
                                Phone = @Phone,
                                Email = @Email,
                                NationalityCountryID = @NationalityCountryID,
                                ImagePath = @ImagePath
                                Where PersonID = @PersonID"; ;
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@PersonID" , PersonID );
            Command.Parameters.AddWithValue("@NationalNumber", NationalNumber);
            Command.Parameters.AddWithValue("@FirstName", FirstName);
            Command.Parameters.AddWithValue("@SecondName", SecondName);
            Command.Parameters.AddWithValue("@ThirdName", ThirdName);
            Command.Parameters.AddWithValue("@LastName", LastName);
            Command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            Command.Parameters.AddWithValue("@Gendor", Gendor);
            Command.Parameters.AddWithValue("@Address", Address);
            Command.Parameters.AddWithValue("@Phone", Phone);
            if (!string.IsNullOrEmpty(Email)) Command.Parameters.AddWithValue("@Email", Email);
            else Command.Parameters.AddWithValue("@Email", DBNull.Value);
            Command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);
            if (!string.IsNullOrEmpty(ImagePath)) Command.Parameters.AddWithValue("@ImagePath", ImagePath);
            else Command.Parameters.AddWithValue("@ImagePath", DBNull.Value);

            try
            {
                Connection.Open();
                object Result = Command.ExecuteNonQuery();
                if (Result != null && int.TryParse(Result.ToString(), out int RowsAffected))
                {
                    IsUpdated = RowsAffected> 0;
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

        static public bool IsPersonExists(int PersonID)
        {
            bool IsFound = false;
            SqlConnection Connection =  new SqlConnection(clsDataAccessLayerSettings.ConnectionString);
            string Query = @"Select IsFound = 
                           Case
                                when (Select PersonID From People Where PersonID = @PersonID) is not null then 1
                                else 0
                            End";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@PersonID", PersonID);
            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();
                if (Result != null && (int.TryParse(Result.ToString(), out int TempIsFound)))
                {
                    IsFound = TempIsFound == 1;
                }
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

        static public bool IsPersonExists(string NationalNumber)
        {
            bool IsFound = false;
            SqlConnection Connection = new SqlConnection(clsDataAccessLayerSettings.ConnectionString);
            string Query = @"Select IsFound = 
                           Case
                                when (Select PersonID From People Where NationalNo = @NationalNumber) is not null then 1
                                else 0
                            End";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@NationalNumber", NationalNumber);
            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();
                if (Result != null && (int.TryParse(Result.ToString(), out int TempIsFound)))
                {
                    IsFound = TempIsFound == 1;
                }
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
    }

}
