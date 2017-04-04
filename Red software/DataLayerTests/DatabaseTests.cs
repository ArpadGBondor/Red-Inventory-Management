using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer;
using System.Windows;
using System.IO;

namespace DataLayer.Tests
{
    [TestClass()]
    public class DatabaseTests
    {
        [TestInitialize]
        public void TestInitialize()
        {
            string dir = AppDomain.CurrentDomain.BaseDirectory + "\\";
            string dbname = "DataLayer_Tests";
            if (File.Exists(dir + dbname + ".mdf"))
                Database.InitializeConnection(dir, dbname);
            else
                Database.CreateDatabase(dir, dbname);
            Assert.IsTrue(Database.Test());
            Assert.AreEqual(Database.Directory, dir);
            Assert.AreEqual(Database.DbName, dbname);
        }

        [TestMethod()]
        public void InitializeConnectionParameterTest()
        {
            string[] parameters = { "", " ", "a", "1234567890", null };
            foreach (var p1 in parameters)
                foreach (var p2 in parameters)
                {
                    Database.InitializeConnection(p1,p2);
                }
        }

        //[TestMethod()]
        //public void ConnectionTest()
        //{
        //    Assert.IsTrue(Database.OpenConnection());
        //    Assert.IsTrue(Database.CloseConnection());
        //}

        [TestMethod()]
        public void TestTest()
        {
            Assert.IsTrue(Database.Test());
        }

        [TestMethod()]
        public void PropertyTest()
        {
            //var x1 = Database.get_connection;
            var x2 = Database.ConnectionString;
        }


        [TestMethod()]
        public void TableCreationTest()
        {
            Database.InitializeTable<UserEntity>();
            Database.InitializeTable<UserEntity>();

            Database.InitializeTable<ProductCategoryEntity>();
            Database.InitializeTable<ProductEntity>();
            Database.InitializeTable<ProductCategoryEntity>();
            Database.InitializeTable<UserEntity>();

            Assert.IsTrue(Database.TableExists<UserEntity>());

        }

        [TestMethod()]
        public void AddRemoveTest()
        {
            Database.InitializeTable<UserEntity>();
            string[] usernames = { "username 1", "username 2", "username 3" };
            List<UserEntity> users = new List<UserEntity>();
            foreach (var name in usernames)
                users.Add(new UserEntity(name, "password"));

            foreach (var user in users)
            {
                Assert.IsFalse(Database.IsExist<UserEntity>(p => p.Username == user.Username));
                Assert.IsTrue(Database.Add<UserEntity>(user));
                Assert.IsTrue(Database.IsExist<UserEntity>(p => p.Username == user.Username));
            }

            foreach (var user in users)
            {
                Assert.IsTrue(Database.IsExist<UserEntity>(p => p.Username == user.Username));
                Assert.IsTrue(Database.Remove<UserEntity>(p => p.Username == user.Username));
                Assert.IsFalse(Database.IsExist<UserEntity>(p => p.Username == user.Username));
            }

        }

        [TestMethod()]
        public void ModifyTest()
        {
            Database.InitializeTable<UserEntity>();

            UserEntity user1 = new UserEntity("username", "pw1");
            UserEntity user2 = new UserEntity("username", "pw2");


            Assert.IsFalse(Database.IsExist<UserEntity>(p => p.Password == user1.Password));
            Assert.IsFalse(Database.IsExist<UserEntity>(p => p.Password == user2.Password));

            Assert.IsTrue(Database.Add<UserEntity>(user1));

            Assert.IsTrue(Database.IsExist<UserEntity>(p => p.Password == user1.Password));

            Assert.IsTrue(Database.Modify<UserEntity>(user2, p => p.Password == user1.Password));

            Assert.IsFalse(Database.IsExist<UserEntity>(p => p.Password == user1.Password));
            Assert.IsTrue(Database.IsExist<UserEntity>(p => p.Password == user2.Password));
        }
    }
}