using DataLayer;
using EntityLayer;
using System.Collections.Generic;

namespace BusinessLayer
{
    public class UserLogin
    {
        private static string _loginedUser;
        public static string LoginedUser
        {
            get
            {
                if (_loginedUser == null) _loginedUser = "";
                return _loginedUser;
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
                throw new System.ArgumentException("Wrong password.", "password");
            }
            _loginedUser = userID;
        }
        public static List<UserEntity> ListUsers()
        {
            return UsersProvider.ListUsers();
        }

        public static bool ModifyUser(string oldUserID, string oldPassword, string newUserId, string password, string confirm)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new System.ArgumentException("Wrong password.", "password");
            }
            else if (string.IsNullOrWhiteSpace(confirm) || password != confirm)
            {
                throw new System.ArgumentException("Wrong confirm password.", "confirm");
            }
            else if (string.IsNullOrWhiteSpace(oldUserID) || !UserLogin.IsValidUserID(oldUserID))
            {
                throw new System.ArgumentException("Wrong old username", "oldUserID");
            }
            else if (string.IsNullOrWhiteSpace(oldPassword) || !UserLogin.IsValidPassword(oldUserID, oldPassword))
            {
                throw new System.ArgumentException("Wrong old password.", "oldPassword");
            }
            else if (string.IsNullOrWhiteSpace(newUserId) || (oldUserID != newUserId && UserLogin.IsValidUserID(newUserId)))
            {
                throw new System.ArgumentException("Wrong new username.", "newUserId");
            }
            else if (UsersProvider.Modify(oldUserID, newUserId, password))
            {
                if (_loginedUser == oldUserID) _loginedUser = newUserId;
                return true;
            }
            else
                return false;
        }
    }
}
