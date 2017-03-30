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

        [TestMethod()]
        public void AddParameterTest()
        {
            string[] StringParameters = { "", " ", "a", "1234567890", null };
            foreach (var name in StringParameters)
            {
                ProductCategoryProvider.Add(
                    new ProductCategoryEntity()
                    {
                        Category = name
                    });
                ProductCategoryProvider.Add(
                    new ProductCategoryEntity()
                    {
                        Id = 1,
                    });
                ProductCategoryProvider.Add(null);
            }
        }

        [TestMethod()]
        public void ModifyParameterTest()
        {
            string[] StringParameters = { "", " ", "a", "1234567890", null };
            foreach (var name in StringParameters)
            {
                ProductCategoryProvider.Modify(
                    new ProductCategoryEntity()
                    {
                        Category = name
                    });
                ProductCategoryProvider.Modify(
                    new ProductCategoryEntity()
                    {
                        Id = 1,
                    });
                ProductCategoryProvider.Modify(null);
            }
        }

        [TestMethod()]
        public void RemoveParameterTest()
        {
            string[] StringParameters = { "", " ", "a", "1234567890", null };
            foreach (var name in StringParameters)
            {
                ProductCategoryProvider.Remove(
                    new ProductCategoryEntity()
                    {
                        Category = name
                    });
                ProductCategoryProvider.Remove(
                    new ProductCategoryEntity()
                    {
                        Id = 1,
                    });
                ProductCategoryProvider.Remove(null);
            }

        }

        [TestMethod()]
        public void AddRemoveTest()
        {
            // Empty table
            Database.Remove<ProductCategoryEntity>(p => true);
            Assert.IsFalse(Database.IsExist<ProductCategoryEntity>(p => true));

            // Add
            string[] StringParameters = {"a", "1234567890" };
            foreach (var name in StringParameters)
            {
                Assert.IsFalse(Database.IsExist<ProductCategoryEntity>(p => p.Category == name));
                Assert.IsTrue(ProductCategoryProvider.Add(
                    new ProductCategoryEntity()
                    {
                        Category = name
                    }));
                Assert.IsTrue(Database.IsExist<ProductCategoryEntity>(p => p.Category == name));
            }

            // Remove
            foreach (var parameter in StringParameters)
            {
                Assert.IsTrue(Database.IsExist<ProductCategoryEntity>(p => p.Category == parameter));
                var list = ProductCategoryProvider.List(p => p.Category == parameter);
                foreach (var category in list)
                    Assert.IsTrue(ProductCategoryProvider.Remove(category));
                Assert.IsFalse(Database.IsExist<ProductCategoryEntity>(p => p.Category == parameter));
                foreach (var category in list)
                    Assert.IsFalse(ProductCategoryProvider.Remove(category));
            }

        }

        [TestMethod()]
        public void ModifyTest()
        {
            // Empty table
            Database.Remove<ProductCategoryEntity>(p => true);
            Assert.IsFalse(Database.IsExist<ProductCategoryEntity>(p => true));

            string CategoryName1 = "Category 1";
            string CategoryName2 = "Category 2";

            // Add category 1
            Assert.IsTrue(ProductCategoryProvider.Add(
                new ProductCategoryEntity()
                {
                    Category = CategoryName1
                }));

            // Category 1 exist
            Assert.IsTrue(Database.IsExist<ProductCategoryEntity>(p => p.Category == CategoryName1));
            // Category 2 does not exist
            Assert.IsFalse(Database.IsExist<ProductCategoryEntity>(p => p.Category == CategoryName2));

            // Prepare modified category with the same ID
            var list = ProductCategoryProvider.List(p => p.Category == CategoryName1);
            var category =
                new ProductCategoryEntity()
                {
                    Id = list.First().Id,
                    Category = CategoryName2
                };

            // Modify
            Assert.IsTrue(ProductCategoryProvider.Modify(category));

            // Category 1 does not exist
            Assert.IsFalse(Database.IsExist<ProductCategoryEntity>(p => p.Category == CategoryName1));
            // Category 2 exist
            Assert.IsTrue(Database.IsExist<ProductCategoryEntity>(p => p.Category == CategoryName2));
        }


    }
}