using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using EntityLayer;

namespace BusinessLayer
{
    public class UserLogin
    {
        private static string loginedUser;
        public static string LoginedUser
        {
            get
            {
                if (loginedUser == null) loginedUser = "";
                return loginedUser;
            }
        } 
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
            if (string.IsNullOrWhiteSpace(userID))
            {
                throw new System.ArgumentException("Wrong username.", "userID");
            }
            else if (string.IsNullOrWhiteSpace(password))
            {
                throw new System.ArgumentException("Wrong password.", "userID");
            }
            return UsersProvider.NewUser(userID, password);
        }
        public static bool RemoveUser(string userID)
        {
            return UsersProvider.DeleteUser(userID);
        }
        public static void Login(string userID, string password)
        {
            if (string.IsNullOrWhiteSpace(userID) || !UserLogin.IsValidUserID(userID))
            {
                throw new System.ArgumentException("Wrong username.", "userID");
            }
            else if (string.IsNullOrWhiteSpace(password) || !UserLogin.IsValidPassword(userID, password))
            {
                throw new System.ArgumentException("Wrong password.", "userID");
            }
            loginedUser = userID;
        }
        public static List<UserEntity> ListUsers()
        {
            var list = UsersProvider.ListUsers();
            foreach (var u in list)
                u.Password = "******";
            return list;
        }
    }
}
