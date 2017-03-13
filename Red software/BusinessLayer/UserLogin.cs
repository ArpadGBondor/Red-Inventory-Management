using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;

namespace BusinessLayer
{
    public class UserLogin
    {
        static UserLogin()
        {
            userID = "";
        }
        private static string userID;
        public static string UserID { get { return userID; } set { userID = value;} } 
        public static bool IsEmptyUserDatabase()
        {
            return UsersProvider.IsEmptyUserDatabase();
        }
        public static bool IsValidUserID(string userID)
        {
            return UsersProvider.IsValidUserID(userID);
        }
        public static bool IsValidPassword(string userID, string password)
        {
            return UsersProvider.IsValidPassword(userID, password);
        }
        public static bool AddUser(string userID, string password)
        {
            return UsersProvider.NewUser(userID, password);
        }
    }
}
