﻿using DataLayer;
using EntityLayer;
using System.Collections.Generic;

namespace BusinessLayer
{
    public class ManageProducts
    {
        public static List<ProductListEntity> ListProducts(int CategoryId = 0)
        {
            if (CategoryId == 0)
                return ProductProvider.List(p => true);
            else
                return ProductProvider.List(p => p.CategoryId == CategoryId);
        }

        public static bool NewProduct(ProductListEntity product)
        {
            return ProductProvider.Add(product);
        }
        public static bool DeleteProduct(ProductListEntity product)
        {
            return ProductProvider.Remove(product);
        }
        public static bool ModifyProduct(ProductListEntity product)
        {
            return ProductProvider.Modify(product);
        }

        public static List<ProductCategoryEntity> ListProductCategories()
        {
            return ProductCategoryProvider.List(p => true);
        }

        public static bool NewProductCategory(ProductCategoryEntity category)
        {
            return ProductCategoryProvider.Add(category);
        }
        public static bool DeleteProductCategory(ProductCategoryEntity category)
        {
            return ProductCategoryProvider.Remove(category);
        }
        public static bool ModifyProductCategory(ProductCategoryEntity category)
        {
            return ProductCategoryProvider.Modify(category);
        }
    }
}
