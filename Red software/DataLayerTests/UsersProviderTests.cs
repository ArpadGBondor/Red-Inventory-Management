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
            Database.InitializeTable(typeof(UserEntity));
        }

        [TestMethod()]
        public void IsEmptyUserDatabaseTest()
        {
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
    }
}