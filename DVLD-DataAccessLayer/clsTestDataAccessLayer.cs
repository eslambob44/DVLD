using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccessLayer
{
    static public class clsTestDataAccessLayer
    {
        static public int AddTest(int TestAppointmentID , bool TestResult , string Notes , int CreatedUserID)
        {
            int TestID = -1;
            SqlConnection Connection = new SqlConnection(clsDataAccessLayerSettings.ConnectionString);
            string Query = @"Insert into Tests values
                            (@TestAppointmentID , @TestResult , @Notes , @CreatedUserID);
                            SELECT SCOPE_IDENTIRY();";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
            Command.Parameters.AddWithValue("@TestResult", TestResult);
            Command.Parameters.AddWithValue("@Notes", Notes);
            Command.Parameters.AddWithValue("@CreatedUserID", CreatedUserID);
            try
            {
                Connection.Open();
                object Result= Command.ExecuteScalar();
                if(Result != null && int.TryParse(Result.ToString() , out int Temp))
                {
                    TestID = Temp;
                }
            }
            catch { }
            finally { Connection.Close(); }
            return TestID;
        }

        static public bool Find(int TestAppointmentID,ref int TestID , ref bool TestResult,
            ref string Notes, ref int CreatedUserID )
        {
            bool IsFound = false;
            SqlConnection Connection = new SqlConnection( clsDataAccessLayerSettings.ConnectionString);
            string Query = @"Select * From Tests Where TestAppointmentID = @TestAppointmentID";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if(Reader.Read())
                {
                    IsFound = true;
                    TestID = (int)Reader["TestID"];
                    TestResult = (bool)Reader["TestResult"];
                    Notes = (string)Reader["Notes"];
                    CreatedUserID = (int)Reader["CreatedByUserID"];
                }
            }
            catch { }
            finally { Connection.Close(); }
            return IsFound;
        }
    }
}
