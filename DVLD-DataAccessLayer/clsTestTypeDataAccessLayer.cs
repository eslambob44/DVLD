using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccessLayer
{
    static public class clsTestTypeDataAccessLayer
    {
        static public DataTable ListTestTypes()
        {
            DataTable dtTestTypes = new DataTable();
            SqlConnection Connection = new SqlConnection(clsDataAccessLayerSettings.ConnectionString);
            string Query = "Select * From TestTypes";
            SqlCommand Command = new SqlCommand(Query, Connection);
            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if(Reader.HasRows)
                {
                    dtTestTypes.Load(Reader);
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
            return dtTestTypes;
        }

        static public bool Find(int TestTypeID , ref string TestTypeTitle , ref string TestTypeDescription , ref float TestFees)
        {
            bool IsFound = false;
            SqlConnection Connection = new SqlConnection(clsDataAccessLayerSettings.ConnectionString);
            string Query = @"Select * From TestTypes
                            Where TestTypeID = @TestTypeID";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.Read())
                {
                    IsFound = true;
                    TestTypeTitle = (string)Reader["TestTypeTitle"];
                    TestTypeDescription = (string)Reader["TestTypeDescription"];
                    TestFees = (float)(decimal)Reader["TestTypeFees"];
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

        static public bool Update(int TestTypeID , string TestTypeTitle , string TestTypeDescription , float TestFees) 
        { 
            bool IsUpdated = false;
            SqlConnection Connection = new SqlConnection( clsDataAccessLayerSettings.ConnectionString);
            string Query = @"Update TestTypes
                            set TestTypeTitle = @TestTypeTitle ,
                                TestTypeDescription = @TestTypeDescription , 
                                TestTypeFees = @TestFees
                                Where TestTypeID = @TestTypeID";
            SqlCommand Command = new SqlCommand(Query, Connection); 
            Command.Parameters.AddWithValue("@TestTypeID" , TestTypeID);
            Command.Parameters.AddWithValue("@TestTypeTitle", TestTypeTitle);
            Command.Parameters.AddWithValue("@TestTypeDescription", TestTypeDescription);
            Command.Parameters.AddWithValue("@TestFees", TestFees);
            try
            {
                Connection.Open();
                int RowsAffected = Command.ExecuteNonQuery();
                IsUpdated = RowsAffected > 0;
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

        static public float GetFees(int TestTypeID)
        {
            float TestFees = -1;
            SqlConnection Connection = new SqlConnection(clsDataAccessLayerSettings.ConnectionString);
            string Query = @"Select TestTypeFees From TestTypes
                            Where TestTypeID = @TestTypeID";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();
                if(Result != null && float.TryParse(Result.ToString() , out float Temp))
                {
                    TestFees = Temp;
                }
            }
            catch { }
            finally { Connection.Close(); }
            return TestFees;
        }
    }
}
