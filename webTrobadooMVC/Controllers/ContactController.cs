using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using trobadoo.com.web.Models.Contact;
using trobadoo.com.web.Base;

namespace trobadoo.com.web.Controllers
{
    public class ContactController : BaseController
    {
        public ContactModel ContactModel;
        public FormContactModel FormContactModel;
        public ValuateContactModel ValuateContactModel;

        public ActionResult Index()
        {
            InitController("Contact", string.Empty);
            ContactModel = new ContactModel(languageCode);
            ContactModel.HeaderInfo = new Entities.HeaderInfo("Trobadoo Segunda Mano", "Articulos de segunda mano, sofas, mesas, sillas, decoración,", "");
            return View("Contact", ContactModel);
        }

        public ActionResult Form()
        {
            InitController("Form", string.Empty);
            FormContactModel = new FormContactModel(languageCode);
            FormContactModel.HeaderInfo = new Entities.HeaderInfo("Trobadoo Segunda Mano", "Articulos de segunda mano, sofas, mesas, sillas, decoración,", "");
            return View("Form", FormContactModel);
        }
        public ActionResult Valuate()
        {
            InitController("Valuate", string.Empty);
            ValuateContactModel = new ValuateContactModel(languageCode);
            ValuateContactModel.HeaderInfo = new Entities.HeaderInfo("Trobadoo Segunda Mano", "Articulos de segunda mano, sofas, mesas, sillas, decoración,", "");
            return View("Valuate", ValuateContactModel);
        }
    }
}
