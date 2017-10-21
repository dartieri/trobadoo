using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using trobadoo.com.web.Base;
using trobadoo.com.web.Models.Carousel;
using trobadoo.com.web.Entities.Enumerations;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Data;
using trobadoo.com.web.Helpers;
using trobadoo.com.web.Helpers.db;

namespace trobadoo.com.web.Models.Products
{
    public static class ProductCategories
    {
        private static Cache<ProductCategory> _cache;
        private static DateTimeOffset _expiration = DateTimeOffset.MinValue;
        private static string PROCEDURE_NAME = "WEB_GET_PRODUCT_CATEGORIES";

        private static void load()
        {
            Debug.Write("Loading product categories");
            _cache = new Cache<ProductCategory>("productCategories", "ProductCategories", getProductCategories(), _expiration);
            CacheManager<ProductCategory>.RegisterCache(_cache);
        }

        private static List<ProductCategory> getProductCategories()
        {
            //var productCategories = new List<ProductCategory>();
            var productCategories = loadFromDatabase();
            /*Random random = new Random(100);
            for (int i = 1; i < 10; i++)
            {
                ProductCategory productCategory = new ProductCategory(i.ToString(), "Category" + i, random.Next());
                if (i % 3 == 0)
                {
                    productCategory.addSubCategory(new ProductCategory((i + 100).ToString(), "SubCategory" + i + 100, random.Next()));
                }
                productCategories.Add(productCategory);
            }*/
            return productCategories;
        }

        private static List<ProductCategory> loadFromDatabase()
        {
            var dbHelper = new DatabaseHelper();
            List<ProductCategory> productCategories = new List<ProductCategory>();
            DataTable dt = dbHelper.call(PROCEDURE_NAME, null, false);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    ProductCategory productCategory = new ProductCategory(row["code"].ToString(), row["description"].ToString(), Int32.Parse(row["numProducts"].ToString()));
                    productCategories.Add(productCategory);
                }
            }
            return productCategories;
        }

        internal static List<ProductCategory> get()
        {
            if (_cache == null)
            {
                load();
            }
            return _cache.get();
        }
    }
}