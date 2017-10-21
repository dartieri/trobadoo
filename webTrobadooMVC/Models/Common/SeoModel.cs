using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;
using trobadoo.com.web.Base;
using trobadoo.com.web.Entities;
using trobadoo.com.web.Entities.Enumerations;
using trobadoo.com.web.Models.Products;

namespace trobadoo.com.web.Models.Common
{
    public class SeoModel : SimpleModel
    {
        private List<MenuSection> _seoSections = new List<MenuSection>();
        private List<ProductCategory> _productCategories;
        public SeoModel(string languageCode)
            : base("seo", languageCode)
        {
            var productCategories = ProductCategories.get();
            if (productCategories != null && productCategories.Count > 0)
            {
                _productCategories = new List<ProductCategory>();
                foreach (ProductCategory item in productCategories.Where(pc => !pc.Code.Contains(',')).ToList ())
                {
                    int numProducts = 0;
                    List<ProductCategory> subCategories = productCategories.Where(c=> c.Code.StartsWith(item.Code + ",")).ToList();
                    foreach(ProductCategory pc in subCategories)
                    {
                        numProducts += pc.NumProducts;
                    }
                    _productCategories.Add (new ProductCategory(item.Code, item.Description , numProducts));
                }
            }
        }

        private void MountSections(SectionName activeSection = SectionName.Home)
        {
            foreach (ProductCategory productCategory in _productCategories)
            {
                _seoSections.Add(new MenuSection(productCategory.Description + " (" + productCategory.NumProducts + ")", "/Category/" + productCategory.Code, string.Empty, _productCategories.IndexOf(productCategory) < _productCategories.Count - 1));
            }

        }

        public void AddSection(MenuSection menuSection)
        {
            _seoSections.Add(menuSection);
        }

        public List<MenuSection> Sections
        {
            get
            {
                if (_seoSections.Count == 0)
                {
                    MountSections();
                }
                return _seoSections;
            }
        }
    }
}