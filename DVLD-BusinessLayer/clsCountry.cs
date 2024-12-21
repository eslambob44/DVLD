using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_BusinessLayer
{
    static public class clsCountry
    {
        static public DataTable ListCountries()
        {
            return clsCountryDataAccessLayer.ListCountries();
        }

        static public int FindCountry(string CountryName)
        {
            return clsCountryDataAccessLayer.FindCountry(CountryName);
        }

        static public string FindCountry(int CountryId) 
        {
            return clsCountryDataAccessLayer.FindCountry(CountryId);
        }

    }
}
