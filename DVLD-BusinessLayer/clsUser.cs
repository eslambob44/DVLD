using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_BusinessLayer
{
    public class clsUser
    {
        enum enMode { Update , AddNew};

        private enMode _Mode;
        private int _PersonID;
        public int PersonID { get { return _PersonID; } }
        private int _UserID;
        public int UserID { get { return _UserID; } }
        public string UserName { get;set; }
        public bool IsActive { get; set; }
        private string _Password;
        

        public clsUser(int UserID , int PersonID , string UserName , bool IsActive) 
        {
            this._UserID = UserID;
            this._PersonID = PersonID;
            this.UserName = UserName;
            this.IsActive = IsActive;
            _Mode = enMode.Update;  
        }

        public clsUser(int PersonID, string Password)
        {
            this.IsActive = false;
            this._UserID = -1;
            this._PersonID = PersonID;
            this.UserName = null;
            this._Password = Password;
            _Mode = enMode.AddNew;
        }

        static public clsUser Find(int UserID)
        {
            int PersonID = -1;
            string UserName = null;
            bool IsActive = false;
            bool IsFound = clsUsersDataAccessLayer.FindUser(UserID, ref PersonID, ref UserName, ref IsActive);
            if (IsFound)
            {
                return new clsUser(UserID, PersonID, UserName, IsActive);
            }
            else return null;
        }

        static public clsUser GetAddNewUser(int PersonID ,string Password)
        {
            if (IsPersonAUser(PersonID) || !clsPerson.IsPersonExists(PersonID)) return null;
            return new clsUser(PersonID, Password);
           
        }

        static public bool DeleteUser(int UserID)
        {
            return clsUsersDataAccessLayer.DeleteUser(UserID);
        }

        private bool _UpdateUser()
        {
            return clsUsersDataAccessLayer.UpdateUser(_UserID , UserName, IsActive);
        }
        private int _AddNewUser()
        {
            return clsUsersDataAccessLayer.AddNewUser(_PersonID, UserName, _Password, IsActive);
        }

        public bool Save()
        {
            switch(_Mode)
            {
                case enMode.Update:
                    return _UpdateUser();
                case enMode.AddNew:
                    int UserID = _AddNewUser();
                    if(UserID != -1)
                    {
                        _Mode = enMode.Update;
                        return true;
                    }
                    return false;
            }
            return false;
        }

        static public int GetUserID(string UserName , string Password)
        {
            return clsUsersDataAccessLayer.GetUserID(UserName , Password);
        }
        static public bool IsUserActive(int UserId) 
        {
            return clsUsersDataAccessLayer.IsUserActive(UserId);
        }

        public bool IsCorrectPassword(string Password)
        {
            return clsUsersDataAccessLayer.IsCorrectPassword(_UserID , Password);
        }

        public bool ChangePassword(string OldPassword , string NewPassword)
        {
            return clsUsersDataAccessLayer.ChangePassword(_UserID , OldPassword , NewPassword);
        }

        static public DataTable ListUsers()
        {
            return clsUsersDataAccessLayer.ListUsers();
        }

        static public bool IsPersonAUser(int PersonID)
        {
            return clsUsersDataAccessLayer.GetUserID(PersonID) != -1;
        }

        
    }
}
