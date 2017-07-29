using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using trobadoo.com.web.Models.Home;
using trobadoo.com.web.Base;

namespace trobadoo.com.web.Controllers
{
    public class HomeController : BaseController
    {
        public HomeModel HomeModel;

        public ActionResult Index()
        {
            InitController("Home", string.Empty);

            HomeModel = new HomeModel(languageCode);
            HomeModel.HeaderInfo = new Entities.HeaderInfo("Trobadoo Segunda Mano", "Articulos de segunda mano, sofas, mesas, sillas, decoración,", "");
            return View("Home", HomeModel);
        }

        
    }
}
