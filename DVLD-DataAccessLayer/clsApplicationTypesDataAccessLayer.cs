using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccessLayer
{
    static public class clsApplicationTypesDataAccessLayer
    {
        static public DataTable ListApplicationTypes()
        {
            DataTable dtAppTypes = new DataTable();
            SqlConnection Connection = new SqlConnection(clsDataAccessLayerSettings.ConnectionString);
            string Query = "Select * From ApplicationTypes";
            SqlCommand Command = new SqlCommand(Query, Connection);
            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.HasRows)
                {
                    dtAppTypes.Load(Reader);
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
            return dtAppTypes;

        }

        static public bool UpdateApplicationType(int AppTypeID , string AppTypeTitle , float Fees)
        {
            bool IsUpdated = false;
            SqlConnection Connection = new SqlConnection(clsDataAccessLayerSettings.ConnectionString);
            string Query = @"Update ApplicationTypes
                             set ApplicationTypeTitle = @AppTypeTitle ,
                                 ApplicationFees = @Fees
                                 Where ApplicationTypeID = @AppTypeID";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@AppTypeTitle", AppTypeTitle);
            Command.Parameters.AddWithValue("@Fees", Fees);
            Command.Parameters.AddWithValue("@AppTypeID", AppTypeID);
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
    }
}
