using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer;

namespace DataLayer.Tests
{
    [TestClass()]
    public class UsersProviderTests
    {
        [TestInitialize]
        public void TestInitialize()
        {
            Database.InitializeConnection(AppDomain.CurrentDomain.BaseDirectory + "\\Database.mdf");
            Database.InitializeTable<UserEntity>();
        }

        [TestMethod()]
        public void IsEmptyUserDatabaseTest()
        {
            var records = UsersProvider.ListUsers();
            foreach (var rec in records)
                Assert.IsTrue(UsersProvider.DeleteUser(rec.Username));

            Assert.IsTrue(UsersProvider.IsEmptyUserDatabase());
            string[] good_parameters = { "", "a", "1234567890" };
            foreach (var p in good_parameters)
            {
                Assert.IsTrue(UsersProvider.NewUser(p, p));
                Assert.IsFalse(UsersProvider.IsEmptyUserDatabase());
                Assert.IsTrue(UsersProvider.DeleteUser(p));
            }
            Assert.IsTrue(UsersProvider.IsEmptyUserDatabase());
        }

        [TestMethod()]
        public void NewDeleteUserTest()
        {
            string[] good_parameters = { "", "a", "1234567890" };
            string[] bad_parameters = { "", " ", "a", "1234567890", null };
            foreach (var p in good_parameters)
                Assert.IsTrue(UsersProvider.NewUser(p, p));
            foreach (var p in good_parameters)
                Assert.IsFalse(UsersProvider.NewUser(p, p));
            foreach (var p in good_parameters)
                Assert.IsTrue(UsersProvider.DeleteUser(p));
        }

        [TestMethod()]
        public void IsValidUserIDTest()
        {
            string[] parameters = { "", "a", "1234567890" };
            foreach (var p in parameters)
            {
                Assert.IsFalse(UsersProvider.IsValidUserID(p));
                UsersProvider.NewUser(p, p);
                Assert.IsTrue(UsersProvider.IsValidUserID(p));
                UsersProvider.DeleteUser(p);
                Assert.IsFalse(UsersProvider.IsValidUserID(p));
            }

        }

        [TestMethod()]
        public void IsValidPasswordTest()
        {
            string[] parameters = { "", "a", "1234567890" };
            foreach (var p in parameters)
            {
                UsersProvider.NewUser(p, p);
                foreach (var pw in parameters)
                {
                    if (p == pw)
                    {
                        Assert.IsTrue(UsersProvider.IsValidPassword(p, pw));
                    }
                    else
                    {
                        Assert.IsFalse(UsersProvider.IsValidPassword(p, pw));
                    }
                }
            }
        }

        [TestMethod()]
        public void ListUsersTest()
        {
            UsersProvider.ListUsers();

        }

        [TestMethod()]
        public void ModifyTest()
        {
            // Empty table
            var records = UsersProvider.ListUsers();
            foreach (var rec in records)
                Assert.IsTrue(UsersProvider.DeleteUser(rec.Username));

            string user1 = "User 1";
            string user2 = "User 2";
            string password1 = "Password 1";
            string password2 = "Password 2";

            Assert.IsFalse(UsersProvider.IsValidUserID(user1));

            // Ad user
            Assert.IsTrue(UsersProvider.NewUser(user1, password1));

            Assert.IsTrue(UsersProvider.IsValidUserID(user1));
            Assert.IsTrue(UsersProvider.IsValidPassword(user1, password1));

            // Modify username
            Assert.IsTrue(UsersProvider.Modify(user1, user2, password1));

            Assert.IsFalse(UsersProvider.IsValidUserID(user1));
            Assert.IsFalse(UsersProvider.IsValidPassword(user1, password1));

            Assert.IsTrue(UsersProvider.IsValidUserID(user2));
            Assert.IsTrue(UsersProvider.IsValidPassword(user2, password1));

            // Modify password
            Assert.IsTrue(UsersProvider.Modify(user2, user2, password2));

            Assert.IsTrue(UsersProvider.IsValidUserID(user2));
            Assert.IsFalse(UsersProvider.IsValidPassword(user2, password1));
            Assert.IsTrue(UsersProvider.IsValidPassword(user2, password2));

        }
    }
}