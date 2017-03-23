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
    public class ProductCategoryProviderTests
    {

        [TestInitialize]
        public void TestInitialize()
        {
            Database.InitializeConnection(AppDomain.CurrentDomain.BaseDirectory + "\\Database.mdf");
            Database.InitializeTable<ProductCategoryEntity>();
        }
        [TestMethod()]
        public void Get_IDTest()
        {
            string[] good_param = { "category 1", "a", "Category 2" };
            string[] bad_param = { null, "", " " };
            foreach (var p in good_param)
                Assert.IsTrue(0 < ProductCategoryProvider.Get_ID(p));
            foreach (var p in bad_param)
                Assert.IsTrue(0 == ProductCategoryProvider.Get_ID(p));
        }


    }
}