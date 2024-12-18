using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
namespace DVLD_DataAccessLayer
{
    static public class clsCountryDataAccessLayer
    {
        static public DataTable ListCountries()
        {
            DataTable dtCountries = new DataTable();
            SqlConnection Connection = new SqlConnection(clsDataAccessLayerSettings.ConnectionString);
            string Query = "Select * From Countries";
            SqlCommand Command = new SqlCommand(Query, Connection);
            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if(Reader.HasRows)
                {
                    dtCountries.Load(Reader);

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
            return dtCountries;
        }

        static public string FindCountry(int CountryID)
        {
            string CountryName = null;
            SqlConnection Connection = new SqlConnection(clsDataAccessLayerSettings.ConnectionString);
            string Query = @"Select CountryName From Countries Where CountryID = @CountryID";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@CountryID", CountryID);
            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();
                if(Result != null) CountryName = Result.ToString();
            }
            catch
            {

            }
            finally
            {
                Connection.Close();
            }
            return CountryName;
        }

        static public int FindCountry(string CountryName)
        {
            int CountryID = -1;
            SqlConnection Connection = new SqlConnection(clsDataAccessLayerSettings.ConnectionString);
            string Query = @"Select CountryID From Countries Where CountryName = @CountryName";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@CountryName", CountryName);
            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();
                if (Result != null && int.TryParse(Result.ToString() , out int TempID))
                {
                    CountryID = TempID;
                }
            }
            catch
            {

            }
            finally
            {
                Connection.Close();
            }
            return CountryID;
        }
    }
}
