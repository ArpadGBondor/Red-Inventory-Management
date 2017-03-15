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
            Database.InitializeConnection(AppDomain.CurrentDomain.BaseDirectory + "\\Database.mdf");
        }

        [TestMethod()]
        public void InitializeConnectionTest()
        {
            string[] parameters = { "", " ", "a", "1234567890", null};
            foreach (var p in parameters)
            {
                Database.InitializeConnection(p);
            }
        }

        [TestMethod()]
        public void ConnectionTest()
        {
            Assert.IsTrue(Database.OpenConnection());
            Assert.IsTrue(Database.CloseConnection());
        }

        [TestMethod()]
        public void TestTest()
        {
            Assert.IsTrue(Database.Test());
        }

        [TestMethod()]
        public void PropertyTest()
        {
            var x1 = Database.get_connection;
            var x2 = Database.get_connectionString;
        }


        [TestMethod()]
        public void TableCreationTest()
        {
            Database.InitializeTable(typeof(UserEntity));
            Assert.IsTrue(Database.OpenConnection());
            Assert.IsTrue(Database.TableExists(typeof(UserEntity)));
            Assert.IsTrue(Database.CloseConnection());    
        }


    }
}