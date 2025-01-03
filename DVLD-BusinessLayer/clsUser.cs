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
        static public int FindUser(string UserName , string Password)
        {
            return clsUsersDataAccessLayer.FindUser(UserName , Password);
        }
        static public bool IsUserActive(int UserId) 
        {
            return clsUsersDataAccessLayer.IsUserActive(UserId);
        }
    }
}
