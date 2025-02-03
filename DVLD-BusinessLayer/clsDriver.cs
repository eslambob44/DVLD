using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_BusinessLayer
{
    public class clsDriver
    {
        private int _DriverID;
        public int DriverID { get { return _DriverID; } }
        private enum enMode { AddNew , ReadOnly}
        private enMode _Mode;
        private int _PersonID;
        public int PersonID { 
            get { return _PersonID; }
            set
            {
                if(_Mode == enMode.AddNew) _PersonID = value;
            }
        }

        private int _CreatedUserID;
        public int CreatedUserID
        {
            get { return _CreatedUserID; }
            set
            {
                if(_Mode == enMode.AddNew) _CreatedUserID = value;
            }
        }

        private DateTime _CreatedDate;
        public DateTime CreatedDate
        {
            get { return _CreatedDate; }
        }

        private clsDriver()
        {
            _DriverID = -1;
            _Mode = enMode.AddNew;
            _PersonID = -1;
            _CreatedUserID = -1;
            _CreatedDate = DateTime.Now;
        }

        private clsDriver(int driverID, int personID, int createdUserID, DateTime createdDate)
        {
            _DriverID = driverID;
            _Mode = enMode.ReadOnly;
            _PersonID = personID;
            _CreatedUserID = createdUserID;
            _CreatedDate = createdDate;
        }

        static public clsDriver Find(int DriverID)
        {
            int PersonID = -1;
            int CreatedUserID = -1;
            DateTime CreatedDate = DateTime.Now;
            if (clsDriverDataAccessLayer.FindDriver(DriverID, ref PersonID, ref CreatedUserID, ref CreatedDate))
            {
                return new clsDriver(DriverID, PersonID, CreatedUserID, CreatedDate);
            }
            else return null;
        }

        static public clsDriver FindByPersonID(int PersonID)
        {
            int DriverID = clsDriverDataAccessLayer.GetDriverID(PersonID);
            if (DriverID != -1)
            {
                return Find(DriverID);
            }
            else return null;
        }

        static public clsDriver GetAddNewObject()
        {
            return new clsDriver();
        }

        private bool _AddNew()
        {
            int DriverID = clsDriverDataAccessLayer.AddDriver(PersonID,CreatedUserID,CreatedDate);
            if(DriverID != -1)
            {
                _DriverID = DriverID;
                return true;
            }
            else return false;
        }

        public bool Save()
        {
            if (_Mode == enMode.ReadOnly) return false;

            if(_Mode == enMode.AddNew)
            {
                if(IsPersonADriver(_PersonID))
                {
                    return false;
                }
                else
                {
                    if (_AddNew())
                    {
                        _Mode = enMode.ReadOnly;
                        return true;
                    }
                    else return false;
                }
            }
            return false;
        }

        static public bool IsPersonADriver(int PersonID)
        {
            return clsDriverDataAccessLayer.IsPersonADriver(PersonID);
        }
    }
}
