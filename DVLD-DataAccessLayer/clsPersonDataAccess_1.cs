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

            using (SqlConnection Connection = new SqlConnection(clsDataAccessLayerSettings.ConnectionString)) 
            {
                try
                {
                    
                    using (SqlCommand Command = new SqlCommand("SP_GetAllPeople", Connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;
                        Command.Parameters.AddWithValue("@Filter" , Filter );
                        Command.Parameters.AddWithValue("@Value", Value);

                        Connection.Open();
                        using (SqlDataReader Reader = Command.ExecuteReader())
                        {
                            if (Reader.HasRows)
                            {
                                dtPeople.Load(Reader);
                            }
                        }
                    }
                }
                catch
                {

                }
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
            using (SqlConnection Connection = new SqlConnection(clsDataAccessLayerSettings.ConnectionString))
            {

                

                using (SqlCommand Command = new SqlCommand("SP_GetPersonByID", Connection))
                {
                    Command.CommandType = CommandType.StoredProcedure;
                    
                    Command.Parameters.AddWithValue("@PersonID", PersonID);
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
                        using (SqlDataReader Reader = Command.ExecuteReader())
                        {
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
                                Email = (Reader["Email"] != DBNull.Value) ? (string)Reader["Email"] : null;
                                NationalityCountryID = (int)Reader["NationalityCountryID"];
                                ImagePath = (Reader["ImagePath"] != DBNull.Value) ? (string)Reader["ImagePath"] : null;
                            }
                        }
                    }
                    catch
                    {

                    }
                }
            }
            
            return IsFound;
        }

        static public bool FindPerson(string NationalityNumberID, out int PersonID
                                            , out string FirstName, out string SecondName, out string ThirdName, out string LastName
                                            , out DateTime DateOfBirth, out byte Gendor, out string Address, out string Phone
                                            , out string Email, out int NationalityCountryID, out string ImagePath)
        {
            bool IsFound = false;
            SqlConnection Connection = new SqlConnection(clsDataAccessLayerSettings.ConnectionString);

            SqlCommand Command = new SqlCommand("SP_GetPersonByNationalNumber", Connection);
            Command.CommandType = CommandType.StoredProcedure;
            Command.Parameters.AddWithValue("@NationalNo", NationalityNumberID);
            PersonID = -1;
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
                    PersonID = (int)Reader["PersonID"];
                    FirstName = (string)Reader["FirstName"];
                    SecondName = (string)Reader["SecondName"];
                    ThirdName = (string)Reader["ThirdName"];
                    LastName = (string)Reader["LastName"];
                    DateOfBirth = (DateTime)Reader["DateOfBirth"];
                    Gendor = (byte)Reader["Gendor"];
                    Address = (string)Reader["Address"];
                    Phone = (string)Reader["Phone"];
                    Email = (Reader["Email"] != DBNull.Value) ? (string)Reader["Email"] : null;
                    NationalityCountryID = (int)Reader["NationalityCountryID"];
                    ImagePath = (Reader["ImagePath"] != DBNull.Value) ? (string)Reader["ImagePath"] : null;
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
            
            SqlCommand Command = new SqlCommand("sp_AddNewPerson", Connection);
            Command.CommandType = CommandType.StoredProcedure;
            Command.Parameters.AddWithValue("@NationalNo", NationalNumber);
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

            SqlParameter OutputParam = new SqlParameter("@PersonID", SqlDbType.Int);
            OutputParam.Direction = ParameterDirection.Output;
            Command.Parameters.Add(OutputParam);
            try
            {
                Connection.Open();
                Command.ExecuteNonQuery();
                if (Command.Parameters["@PersonID"].Value != DBNull.Value)
                {
                    PersonID = (int)Command.Parameters["@PersonID"].Value;
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
            SqlCommand Command = new SqlCommand("SP_DeletePerson", Connection); 
            Command.CommandType = CommandType.StoredProcedure;
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

            SqlCommand Command = new SqlCommand("SP_UpdatePerson", Connection);
            Command.CommandType = CommandType.StoredProcedure;
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
