using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer;
using System.Data.Linq;

namespace DataLayer
{
    public class UsersProvider
    {
        static UsersProvider()
        {
            Database.InitializeTable(typeof(UserEntity));
        }

        public static bool IsEmptyUserDatabase()
        {
            bool lExist = false;

            MyDataContext db = new MyDataContext(Database.get_connectionString);

            lExist = (0 < db.Users.Count());

            return !lExist;
        }

        public static bool IsValidUserID(string userID)
        {
            bool lExist = false;

            MyDataContext db = new MyDataContext(Database.get_connectionString);
            var q = from u in db.Users
                    where u.Username == userID
                    select u;
            lExist = (0 < q.Count());

            return lExist;
        }

        public static bool IsValidPassword(string userID,string pw)
        {
            bool lExist = false;

            MyDataContext db = new MyDataContext(Database.get_connectionString);
            var q = from u in db.Users
                    where (u.Username == userID)
                    select u;
            foreach(var user in q)
            {
                lExist = lExist || EncriptionProvider.Confirm(pw, user.Password, EncriptionProvider.Supported_HA.SHA256);
            }

            return lExist;
        }

        public static bool NewUser(string userID,string password)
        {
            UserEntity User = new UserEntity( userID, EncriptionProvider.ComputeHash(password, EncriptionProvider.Supported_HA.SHA256, null));
            return Database.Add<UserEntity>(User);
        }

        public static bool DeleteUser(string userID)
        {
            return Database.Remove<UserEntity>(p => p.Username == userID);
        }

        public static bool Modify(string oldUserID, string newUserId, string password)
        {
            UserEntity User = new UserEntity(newUserId, EncriptionProvider.ComputeHash(password, EncriptionProvider.Supported_HA.SHA256, null));
            if (oldUserID != newUserId) 
            {
                return (DeleteUser(oldUserID) && NewUser(newUserId, password));
            }
            return Database.Modify<UserEntity>(User, p => p.Username == oldUserID);
        }

        public static List<UserEntity> ListUsers()
        {
            return Database.ListTable<UserEntity>(p => true);
        }
    }
}
