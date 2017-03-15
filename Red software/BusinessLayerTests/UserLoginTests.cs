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
            string[] good_parameters = { "", "a", "1234567890" };
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
            string[] good_parameters = { "", "a", "1234567890" };
            string[] bad_parameters = { "", " ", "a", "1234567890", null };
            foreach (var p in good_parameters)
                Assert.IsTrue(UserLogin.AddUser(p, p));
            foreach (var p in good_parameters)
                Assert.IsFalse(UserLogin.AddUser(p, p));
            foreach (var p in good_parameters)
                Assert.IsTrue(UserLogin.RemoveUser(p));
        }

        [TestMethod()]
        public void IsValidUserIDTest()
        {
            string[] parameters = { "", "a", "1234567890" };
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
            string[] parameters = { "", "a", "1234567890" };
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

        }
        [TestMethod()]
        public void PropertyTest()
        {
            Assert.AreEqual(UserLogin.UserID,"");
            UserLogin.UserID = "XXX";
            Assert.AreEqual(UserLogin.UserID, "XXX");
        }

    }
}