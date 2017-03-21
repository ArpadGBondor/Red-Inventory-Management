using Microsoft.VisualStudio.TestTools.UnitTesting;
using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Tests
{
    [TestClass()]
    public class ManageProductsTests
    {

        [TestInitialize]
        public void TestInitialize()
        {
            string file = AppDomain.CurrentDomain.BaseDirectory + "\\Database.mdf";
            DatabaseConnection.ChangeDatabaseFile(file);
            Assert.IsTrue(DatabaseConnection.TestConnection());
            Assert.AreEqual(DatabaseConnection.File, file);
        }

        [TestMethod()]
        public void ListProductCategoriesTest1()
        {
            var list = ManageProducts.ListProductCategories();
        }
    }
}