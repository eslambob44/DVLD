using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_BusinessLayer
{
    public class clsTestType
    {

        public enum enTestType { Vision = 1, Written, Street };

        private enTestType _ID;
        public enTestType ID { get { return _ID; } }
        public string Title { get;set; }
        public string Description {  get; set; }
        public float Fees { get; set; } 

        private clsTestType(enTestType ID , string Title , string Description , float Fees)
        {
            _ID = ID;
            this.Title = Title;
            this.Description = Description;
            this.Fees = Fees;
        }
        static public clsTestType Find(enTestType TestTypeID)
        {
            
            string Title= string.Empty;
            string Description = string.Empty;
            float Fees=-1;
            if(clsTestTypeDataAccessLayer.Find((int)TestTypeID , ref Title , ref Description , ref Fees))
            {
                return new clsTestType(TestTypeID , Title , Description , Fees);
            }
            else
            {
                return null;
            }
        }

        private bool _Update()
        {
            return clsTestTypeDataAccessLayer.Update((int)_ID, Title, Description, Fees);
        }

        public bool Save()
        {
            return _Update();
        }

        public static DataTable ListTestTypes()
        {
            return clsTestTypeDataAccessLayer.ListTestTypes();
        }

        static public float GetTestFees(int TestTypeID)
        {
            return clsTestTypeDataAccessLayer.GetFees(TestTypeID);
        }

        
    }
}
