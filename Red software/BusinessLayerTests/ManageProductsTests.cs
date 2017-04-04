using Microsoft.VisualStudio.TestTools.UnitTesting;
using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BusinessLayer.Tests
{
    [TestClass()]
    public class ManageProductsTests
    {

        [TestInitialize]
        public void TestInitialize()
        {
            string dir = AppDomain.CurrentDomain.BaseDirectory + "\\";
            string dbname = "BusinessLayer_Tests";
            if (File.Exists(dir + dbname + ".mdf"))
                DatabaseConnection.ChangeDatabase(dir, dbname);
            else
                DatabaseConnection.CreateDatabase(dir, dbname);
            Assert.IsTrue(DatabaseConnection.TestConnection());
            Assert.AreEqual(DatabaseConnection.Directory, dir);
            Assert.AreEqual(DatabaseConnection.DbName, dbname);
        }

        [TestMethod()]
        public void ListProductCategoriesTest1()
        {
            var list = ManageProducts.ListProductCategories();
        }
    }
}