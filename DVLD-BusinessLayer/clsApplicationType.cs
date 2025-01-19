using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_BusinessLayer
{
    public class clsApplicationType
    {
        private int _AppTypeID;
        public int ID { get { return _AppTypeID; } }
        public string Title { get; set; }
        public float Fees { get; set; }

        private clsApplicationType(int ID , string Title , float Fees)
        {
            this._AppTypeID = ID;
            this.Title = Title;
            this.Fees = Fees;
        }

        static public clsApplicationType Find(int ApplicationTypeID)
        {
            string Title = "";
            float Fees = 0;
            if(clsApplicationTypesDataAccessLayer.Find(ApplicationTypeID, ref Title,ref Fees) == true) 
            {
                return new clsApplicationType(ApplicationTypeID, Title , Fees);
            }
            else
            {
                return null;
            }
        }

        bool _Update()
        {
            return clsApplicationTypesDataAccessLayer.UpdateApplicationType(_AppTypeID, Title, Fees);
        }

        public bool Save()
        {
            return _Update();
        }

        static public DataTable ListApplicationTypes()
        {
            return clsApplicationTypesDataAccessLayer.ListApplicationTypes();
        }
    }
}
