using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_BusinessLayer
{
    public class clsUser
    {
        static public int GetUserID(string UserName , string Password)
        {
            return clsUsersDataAccessLayer.GetUserID(UserName , Password);
        }
        static public bool IsUserActive(int UserId) 
        {
            return clsUsersDataAccessLayer.IsUserActive(UserId);
        }
    }
}
