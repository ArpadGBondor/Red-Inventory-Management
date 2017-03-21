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


        [TestMethod()]
        public void ListProductCategoriesTest1()
        {
            var list = ManageProducts.ListProductCategories();
        }
    }
}