using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using trobadoo.com.web.Base;
using trobadoo.com.web.Models.Carousel;
using trobadoo.com.web.Entities.Enumerations;

namespace trobadoo.com.web.Models.Products
{
    public class ProductCategory
    {
        private String _code;
        private String _description;
        private int _numProducts;
        private List<ProductCategory> _subCategories;
        private string p;
        private string p_2;

        public ProductCategory()
        {
        }

        public ProductCategory(string code, string description, int numProducts)
        {
            this._code = code;
            this._description = description;
            this._numProducts = numProducts;
        }

        public void addSubCategory(ProductCategory subCategory)
        {
            if (_subCategories == null)
            {
                _subCategories = new List<ProductCategory>();
            }
            _subCategories.Add(subCategory);
        }

        public string Code
        {
            get
            {
                return _code;
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }
        }

        public int NumProducts
        {
            get
            {
                return _numProducts;
            }
        }

        public bool hasSubCategories
        {
            get
            {
                return _subCategories != null && _subCategories.Count > 0;
            }
        }

        public List<ProductCategory> SubCategories
        {
            get
            {
                return _subCategories;
            }
        }
    }
}