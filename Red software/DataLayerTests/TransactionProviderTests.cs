using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using EntityLayer;

namespace DataLayer.Tests
{
    [TestClass()]
    public class TransactionProviderTests
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
        public void ListHeadTest()
        {
            //string
            //PartnerEntity partner = new PartnerEntity() { }

            //TransactionProvider.ListHead(p => true);
        }

        [TestMethod()]
        public void ListBodyTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod()]
        public void ListInventoryTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod()]
        public void ListInventoryDetailsTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod()]
        public void ListPartnerTransactionsTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod()]
        public void IsExistHeadTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod()]
        public void IsExistBodyTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod()]
        public void AddOrModifyTransactionTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod()]
        public void RemoveTransactionTest()
        {
            throw new NotImplementedException();
        }
    }
}