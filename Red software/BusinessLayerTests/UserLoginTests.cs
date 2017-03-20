using Microsoft.VisualStudio.TestTools.UnitTesting;
using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using EntityLayer;

namespace BusinessLayer.Tests
{
    [TestClass()]
    public class UserLoginTests
    {
        [TestInitialize]
        public void TestInitialize()
        {
            string file = AppDomain.CurrentDomain.BaseDirectory + "\\Database.mdf";
            DatabaseConnection.ChangeDatabaseFile(file);
            Assert.IsTrue(DatabaseConnection.TestConnection());
            Assert.AreEqual(DatabaseConnection.File,file);
        }

        [TestMethod()]
        public void IsEmptyUserDatabaseTest()
        {
            Assert.IsTrue(UserLogin.IsEmptyUserDatabase());
            string[] good_parameters = { "a", "1234567890", "god", "qwerty" };
            foreach (var p in good_parameters)
            {
                Assert.IsTrue(UserLogin.AddUser(p, p));
                Assert.IsFalse(UserLogin.IsEmptyUserDatabase());
                Assert.IsTrue(UserLogin.RemoveUser(p));
            }
            Assert.IsTrue(UserLogin.IsEmptyUserDatabase());
        }

        [TestMethod()]
        public void AddRemoveUserTest()
        {
            string[] good_parameters = { "a", "1234567890", "god", "qwerty" };
            string[] bad_parameters = { "", " ", "a", "1234567890", null };
            foreach (var p in good_parameters)
                Assert.IsTrue(UserLogin.AddUser(p, p));
            foreach (var p in good_parameters)
                try
                {
                    Assert.IsFalse(UserLogin.AddUser(p, p));
                }
                catch
                {
                    Assert.IsTrue(string.IsNullOrWhiteSpace(p));
                }
            foreach (var p in good_parameters)
                Assert.IsTrue(UserLogin.RemoveUser(p));
        }

        [TestMethod()]
        public void IsValidUserIDTest()
        {
            string[] parameters = { "a", "1234567890", "god", "qwerty" };
            foreach (var p in parameters)
            {
                Assert.IsFalse(UserLogin.IsValidUserID(p));
                Assert.IsTrue(UserLogin.AddUser(p, p));
                Assert.IsTrue(UserLogin.IsValidUserID(p));
                Assert.IsTrue(UserLogin.RemoveUser(p));
                Assert.IsFalse(UserLogin.IsValidUserID(p));
            }
        }

        [TestMethod()]
        public void IsValidPasswordTest()
        {
            string[] parameters = { "a", "1234567890", "god", "qwerty" };
            foreach (var p in parameters)
            {
                Assert.IsTrue(UserLogin.AddUser(p, p));
                foreach (var pw in parameters)
                {
                    if (p == pw)
                    {
                        Assert.IsTrue(UserLogin.IsValidPassword(p, pw));
                    }
                    else
                    {
                        Assert.IsFalse(UserLogin.IsValidPassword(p, pw));
                    }
                }
            }
            foreach (var p in parameters)
                Assert.IsTrue(UserLogin.RemoveUser(p));
        }

        [TestMethod()]
        public void LoginTest()
        {
            Assert.AreEqual(UserLogin.LoginedUser, "");
            string[] parameters = { "", " ", "a", "1234567890", "god", "qwerty", null };
            foreach (var p in parameters)
            {
                try
                {
                    Assert.IsFalse(UserLogin.IsValidUserID(p));
                    Assert.IsTrue(UserLogin.AddUser(p, p));
                }
                catch { }
            }
            foreach (var p1 in parameters)
                foreach (var p2 in parameters)
                {
                    try
                    {
                        UserLogin.Login(p1, p2);
                        Assert.AreEqual(UserLogin.LoginedUser, p1);
                    }
                    catch
                    {
                        if (string.IsNullOrWhiteSpace(p1) || string.IsNullOrWhiteSpace(p1))
                        {
                            //Assert.IsTrue(true);
                        }
                        else
                        {
                            Assert.AreNotEqual(p1, p2);
                        }
                    }
                }
            foreach (var p in parameters)
                if (!string.IsNullOrWhiteSpace(p))
                    Assert.IsTrue(UserLogin.RemoveUser(p));
        }

        [TestMethod()]
        public void ListUsersTest()
        {
            UserLogin.ListUsers();
        }

        [TestMethod()]
        public void ModifyUserTest()
        {
            // Empty table
            if (!UserLogin.IsEmptyUserDatabase())
            {
                var records = UserLogin.ListUsers();
                foreach (var rec in records)
                    Assert.IsTrue(UserLogin.RemoveUser(rec.Username));
            }


            string user1 = "User 1";
            string user2 = "User 2";
            string password1 = "Password 1";
            string password2 = "Password 2";

            // Add user
            Assert.IsTrue(UserLogin.AddUser(user1, password1));

            Assert.IsTrue(UserLogin.IsValidUserID(user1));
            Assert.IsTrue(UserLogin.IsValidPassword(user1,password1));

            // Modify password
            UserLogin.ModifyUser(user1,password1,user1,password2,password2);

            Assert.IsTrue(UserLogin.IsValidUserID(user1));
            Assert.IsFalse(UserLogin.IsValidPassword(user1, password1));
            Assert.IsTrue(UserLogin.IsValidPassword(user1, password2));

            // Modify username
            UserLogin.ModifyUser(user1, password2, user2, password2, password2);

            Assert.IsFalse(UserLogin.IsValidUserID(user1));
            Assert.IsTrue(UserLogin.IsValidUserID(user2));
            Assert.IsTrue(UserLogin.IsValidPassword(user2, password2));

        }
    }
}