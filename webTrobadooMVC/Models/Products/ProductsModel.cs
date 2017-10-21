using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using trobadoo.com.web.Base;
using trobadoo.com.web.Models.Carousel;
using trobadoo.com.web.Entities.Enumerations;

namespace trobadoo.com.web.Models.Products
{
    public class ProductsModel:BaseModel
    {
        public ProductsModel(string languageCode)
            : base(SectionName.Productos, "Products",languageCode, true)
        {
            //BaseCarouselModel = new HomeCarouselModel();
        }

        public override void AddJs()
        {
            JsManager.addJS("/Content/js/products.js");
        }

        public override void AddCss() { }
        public override void AddDocumentReady() {
            JsInits.AddDocumentReadyAction("com.trobadoo.Products.init();");
        }
    }
}