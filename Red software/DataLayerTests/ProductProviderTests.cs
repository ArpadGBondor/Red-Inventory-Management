using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using EntityLayer;
using System.IO;

namespace DataLayer.Tests
{
    [TestClass()]
    public class ProductProviderTests
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
        public void ListTest()
        {
            // Empty table
            Database.Remove<ProductEntity>(p => true);
            Database.Remove<ProductCategoryEntity>(p => true);

            string[] parameters = { "Product 1", "Product 2", "Product 3" };
            foreach (var p in parameters)
            {
                ProductListEntity record = new ProductListEntity();
                record.Name = p;
                record.Category = p;
                Assert.IsTrue(ProductProvider.Add(record));
            }
            foreach (var p in parameters)
            {
                ProductListEntity record = new ProductListEntity();
                record.Name = "#2 " + p;
                Assert.IsTrue(ProductProvider.Add(record));
            }
            var list = ProductProvider.List(p => true);
            Assert.IsTrue(list.Count == 6);
        }

        [TestMethod()]
        public void AddParameterTest()
        {
            string[] StringParameters = { "", " ", "a", "1234567890", null };
            foreach (var name in StringParameters)
            {
                ProductProvider.Add(
                    new ProductListEntity()
                    {
                        Name = name,
                        Category = name
                    });
                ProductProvider.Add(
                    new ProductListEntity()
                    {
                        Name = name,
                        Id = 1,
                    });
                ProductProvider.Add(null);
            }
        }

        [TestMethod()]
        public void ModifyParameterTest()
        {
            string[] StringParameters = { "", " ", "a", "1234567890", null };
            foreach (var name in StringParameters)
            {
                ProductProvider.Modify(
                    new ProductListEntity()
                    {
                        Name = name,
                        Category = name
                    });
                ProductProvider.Modify(
                    new ProductListEntity()
                    {
                        Name = name,
                        Id = 1,
                    });
                ProductProvider.Modify(null);
            }
        }

        [TestMethod()]
        public void RemoveParameterTest()
        {
            string[] StringParameters = { "", " ", "a", "1234567890", null };
            foreach (var name in StringParameters)
            {
                ProductProvider.Remove(
                    new ProductListEntity()
                    {
                        Name = name,
                        Category = name
                    });
                ProductProvider.Remove(
                    new ProductListEntity()
                    {
                        Name = name,
                        Id = 1,
                    });
                ProductProvider.Remove(null);
            }
        }

        [TestMethod()]
        public void AddRemoveTest()
        {
            // Empty table
            Database.Remove<ProductEntity>(p => true);
            Database.Remove<ProductCategoryEntity>(p => true);

            // Add
            string[] parameters = { "Product 1", "Product 2", "Product 3" };
            foreach (var p in parameters)
            {
                Assert.IsFalse(ProductProvider.IsExist(e => e.Name == p));
                Assert.IsFalse(Database.IsExist<ProductCategoryEntity>(e => e.Category == p));

                ProductListEntity record = new ProductListEntity();
                record.Name = p;
                record.Category = p;

                Assert.IsTrue(ProductProvider.Add(record));

                Assert.IsTrue(ProductProvider.IsExist(e => e.Name == p));
                Assert.IsTrue(Database.IsExist<ProductCategoryEntity>(e => e.Category == p));
            }

            // Remove
            foreach (var p in parameters)
            {
                Assert.IsTrue(ProductProvider.IsExist(e => e.Name == p));

                var list = ProductProvider.List(e => e.Name == p);
                foreach (var product in list)
                    Assert.IsTrue(ProductProvider.Remove(product));
                Assert.IsFalse(ProductProvider.IsExist(e => e.Name == p));
                foreach (var product in list)
                    Assert.IsFalse(ProductProvider.Remove(product));
                Assert.IsFalse(ProductProvider.IsExist(e => e.Name == p));
            }
        }

        [TestMethod()]
        public void ModifyTest()
        {
            // Empty table
            Database.Remove<ProductEntity>(p => true);
            Database.Remove<ProductCategoryEntity>(p => true);

            string ProductName1 = "Product 1";
            string ProductName2 = "Product 2";

            // Add product 1
            Assert.IsTrue(ProductProvider.Add(
                new ProductListEntity()
                {
                    Category = ProductName1,
                    Code = ProductName1,
                    Name = ProductName1,
                    Cost_Price = 1,
                    Sell_Price = 1
                }));

            // Product 1 exist
            Assert.IsTrue(ProductProvider.IsExist(p => p.Code == ProductName1));
            Assert.IsTrue(ProductProvider.IsExist(p => p.Name == ProductName1));
            Assert.IsTrue(ProductProvider.IsExist(p => p.Cost_Price == 1));
            Assert.IsTrue(ProductProvider.IsExist(p => p.Sell_Price == 1));

            // Product 2 does not exist
            Assert.IsFalse(ProductProvider.IsExist(p => p.Code == ProductName2));
            Assert.IsFalse(ProductProvider.IsExist(p => p.Name == ProductName2));
            Assert.IsFalse(ProductProvider.IsExist(p => p.Cost_Price == 2));
            Assert.IsFalse(ProductProvider.IsExist(p => p.Sell_Price == 2));

            // Prepare modified product with the same ID
            var list = ProductProvider.List(p => p.Name == ProductName1);
            var product =
                new ProductListEntity()
                {
                    Id = list.First().Id,
                    Category = ProductName2,
                    Code = ProductName2,
                    Name = ProductName2,
                    Cost_Price = 2,
                    Sell_Price = 2
                };

            // Modify
            Assert.IsTrue(ProductProvider.Modify(product));

            // Product 1 does not exist
            Assert.IsFalse(ProductProvider.IsExist(p => p.Code == ProductName1));
            Assert.IsFalse(ProductProvider.IsExist(p => p.Name == ProductName1));
            Assert.IsFalse(ProductProvider.IsExist(p => p.Cost_Price == 1));
            Assert.IsFalse(ProductProvider.IsExist(p => p.Sell_Price == 1));

            // Product 2 exist
            Assert.IsTrue(ProductProvider.IsExist(p => p.Code == ProductName2));
            Assert.IsTrue(ProductProvider.IsExist(p => p.Name == ProductName2));
            Assert.IsTrue(ProductProvider.IsExist(p => p.Cost_Price == 2));
            Assert.IsTrue(ProductProvider.IsExist(p => p.Sell_Price == 2));

            // Both category exist
            Assert.IsTrue(Database.IsExist<ProductCategoryEntity>(p => p.Category == ProductName1));
            Assert.IsTrue(Database.IsExist<ProductCategoryEntity>(p => p.Category == ProductName2));
        }
    }
}