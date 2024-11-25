using EntityLayer;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer
{
    public class UsersProvider
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Checks if the User database is empty.
        /// </summary>
        /// <returns></returns>
        public static bool IsEmptyUserDatabase()
        {
            bool exists = false;
            using (var db = new InventoryContext(DatabaseConnection.ConnectionString))
            {
                exists = (0 < db.Users.Count());
            }
            return !exists;
        }

        /// <summary>
        /// Checks if the UserID is in the database.
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static bool IsValidUserID(string userID)
        {
            bool exists = false;

            using (var db = new InventoryContext(DatabaseConnection.ConnectionString))
            {
                var q = from u in db.Users
                        where u.Username == userID
                        select u;
                exists = (0 < q.Count());
            }
            return exists;
        }

        /// <summary>
        /// Checks if the username is in the database and the password parameter is matching the encripted password.
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="pw">The encription of the password is handled by the UsersProvider.</param>
        /// <returns></returns>
        public static bool IsValidPassword(string userID, string pw)
        {
            bool exists = false;

            using (var db = new InventoryContext(DatabaseConnection.ConnectionString))
            {
                var q = from u in db.Users
                        where (u.Username == userID)
                        select u;
                foreach (var user in q)
                {
                    exists = exists || EncriptionProvider.Confirm(pw, user.Password, EncriptionProvider.Supported_HA.SHA256);
                }
            }
            return exists;
        }

        /// <summary>
        /// Adds a new user to the database
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="password">The encription of the password is handled by the UsersProvider.</param>
        /// <returns></returns>
        public static bool NewUser(string userID, string password)
        {
            UserEntity User = new UserEntity(userID, EncriptionProvider.ComputeHash(password, EncriptionProvider.Supported_HA.SHA256, null));
            return DatabaseConnection.Add<UserEntity>(User);
        }

        /// <summary>
        /// Deletes a user from the database.
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static bool DeleteUser(string userID)
        {
            return DatabaseConnection.Remove<UserEntity>(p => p.Username == userID);
        }

        /// <summary>
        /// Modifies the username and/or the password of a user.
        /// </summary>
        /// <param name="oldUserID"></param>
        /// <param name="newUserId"></param>
        /// <param name="password">The encription of the password is handled by the UsersProvider.</param>
        /// <returns></returns>
        public static bool Modify(string oldUserID, string newUserId, string password)
        {
            UserEntity User = new UserEntity(newUserId, EncriptionProvider.ComputeHash(password, EncriptionProvider.Supported_HA.SHA256, null));
            if (oldUserID != newUserId)
            {
                return (DeleteUser(oldUserID) && NewUser(newUserId, password));
            }
            return DatabaseConnection.Modify<UserEntity>(User, p => p.Username == oldUserID);
        }

        /// <summary>
        /// Lists every record from the Users datatable, but hides the encripted passwords with "******".
        /// </summary>
        /// <returns></returns>
        public static List<UserEntity> ListUsers()
        {
            var list = DatabaseConnection.ListTable<UserEntity>(p => true);
            foreach (var u in list)
                u.Password = "******";
            return list;
        }
    }
}
