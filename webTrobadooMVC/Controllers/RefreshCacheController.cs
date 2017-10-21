using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using trobadoo.com.web.Base;
using trobadoo.com.web.Models.Concept;

namespace trobadoo.com.web.Controllers
{
    public class RefreshCacheController : BaseController
    {
        public ActionResult Index()
        {
            RefreshCacheModel refreshCacheModel = new RefreshCacheModel();
            return View("RefreshCache", refreshCacheModel);
        }

    }
}
