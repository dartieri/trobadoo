using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using trobadoo.com.web.Models.Concept;
using trobadoo.com.web.Base;

namespace trobadoo.com.web.Controllers
{
    public class ConceptController : BaseController
    {
        public ConceptModel ConceptModel;

        public ActionResult Index()
        {
            InitController("Concept", string.Empty);

            ConceptModel = new ConceptModel(languageCode);
            ConceptModel.HeaderInfo = new Entities.HeaderInfo("Trobadoo Segunda Mano", "Articulos de segunda mano, sofas, mesas, sillas, decoración,", "");
            return View("Concept",ConceptModel);
        }

    }
}
