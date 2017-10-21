using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using trobadoo.com.web.Base;
using trobadoo.com.web.Models.Products;

namespace trobadoo.com.web.Controllers
{
    public class ProductsController : BaseController
    {
        public ProductsModel ProductsModel;

        public ActionResult Index()
        {
            InitController("Products", string.Empty);

            ProductsModel = new ProductsModel(languageCode);
            ProductsModel.HeaderInfo = new Entities.HeaderInfo("Trobadoo Segunda Mano", "Articulos de segunda mano, sofas, mesas, sillas, decoración,", "");
            return View("Products",ProductsModel);
        }

    }
}
