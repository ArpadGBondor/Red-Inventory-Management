using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using EntityLayer;

namespace DataLayer.Tests
{
    [TestClass()]
    public class ProductProviderTests
    {
        [TestInitialize]
        public void TestInitialize()
        {
            Database.InitializeConnection(AppDomain.CurrentDomain.BaseDirectory + "\\Database.mdf");
            Database.InitializeTable(typeof(ProductCategoryEntity));
            Database.InitializeTable(typeof(ProductEntity));
        }

        [TestMethod()]
        public void ListTest()
        {

            string[] parameters = { "Product 1", "Product 2", "Product 3" };
            foreach (var p in parameters)
            {
                ProductListEntity record = new ProductListEntity();
                record.Name = p;
                record.Category = p;
                ProductProvider.Add(record);
            }
            foreach (var p in parameters)
            {
                ProductListEntity record = new ProductListEntity();
                record.Name = "#2 " + p;
                ProductProvider.Add(record);
            }
            var list = ProductProvider.List(p => true);

        }

        [TestMethod()]
        public void AddTest()
        {
            // remove every record
            Database.Remove<ProductEntity>(p => true);
            Database.Remove<ProductCategoryEntity>(p => true);

            string[] parameters = { "Product 1", "Product 2", "Product 3"};
            foreach(var p in parameters)
            {
                ProductListEntity record = new ProductListEntity();
                record.Name = p;
                record.Category = p;

                ProductProvider.Add(record);

                Assert.IsTrue(Database.IsExist<ProductEntity>(e=>e.Name == p));
                Assert.IsTrue(Database.IsExist<ProductCategoryEntity>(e => e.Category == p));
            }

        }
    }
}