using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using trobadoo.com.web.Models.Services;
using trobadoo.com.web.Base;

namespace trobadoo.com.web.Controllers
{
    public class ServicesController : BaseController
    {
        public ServicesModel ServicesModel;

        public ActionResult Index()
        {
            InitController("Services", string.Empty);
            
            ServicesModel = new ServicesModel(languageCode);
            ServicesModel.HeaderInfo = new Entities.HeaderInfo("Trobadoo Segunda Mano", "Articulos de segunda mano, sofas, mesas, sillas, decoración,", "");
            return View("Services",ServicesModel);
        }

    }
}
