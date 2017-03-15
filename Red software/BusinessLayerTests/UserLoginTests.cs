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
            DatabaseConnection.ChangeDatabaseFile(AppDomain.CurrentDomain.BaseDirectory + "\\Database.mdf");
            Assert.IsTrue(DatabaseConnection.TestConnection());
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
            string[] parameters = {"", " ", "a", "1234567890", "god", "qwerty", null };
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
                        if(string.IsNullOrWhiteSpace(p1) || string.IsNullOrWhiteSpace(p1))
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
    }
}