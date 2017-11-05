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

namespace trobadoo.com.web.Models.Products
{
    public static class Products
    {
        private static Cache<Product> _cache;
        private static DateTimeOffset _expiration = DateTimeOffset.MinValue;
        private static string PROCEDURE_NAME= "GET_PRODUCTS";

        private static void load()
        {
            Debug.Write("Loading products");
            _cache = new Cache<Product>("products", "Products", getProducts(), _expiration);
            CacheManager<Product>.RegisterCache(_cache);
        }

        private static List<Product> getProducts()
        {
            var products = new List<Product>();
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
            return products;
        }

        /*private static List<Product> loadFromDatabase(SqlParametersList parameters)
        {
            var dbHelper = new DatabaseHelper();
            List<Product> products = new List<Product>();
            DataTable dt = dbHelper.call(PROCEDURE_NAME, parameters, false);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach(DataRow row in dt.Rows)
                {
                    Product product = new Product();
                    product.DepositCreationDate = DateTime.Parse(row["depositCreationDate"].ToString());
                    product.FamilyCode = row["familyCode"].ToString();
                    product.CreationDate = DateTime.Parse(row["creationDate"].ToString());
                    product.Code = row["code"].ToString();
                    product.Description = row["description"].ToString();
                    product.InitialPrice = Decimal.Parse(row["initialPrice"].ToString());
                    product.SellPrice = Decimal.Parse(row["sellPrice"].ToString());
                    product.Stock = Int32.Parse(row["stock"].ToString());
                    product.Observations = row["observations"].ToString();
                    products.Add(product);
                }
            }
            return products;
        }*/

        internal static List<Product> get()
        {
            if (_cache == null)
            {
                load();
            }
            return _cache.get();
        }
    }
}