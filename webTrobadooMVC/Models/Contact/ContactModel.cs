using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using trobadoo.com.web.Base;
using trobadoo.com.web.Models.Carousel;
using trobadoo.com.web.Entities.Enumerations;

namespace trobadoo.com.web.Models.Contact
{
    public class ContactModel : BaseModel
    {
        public ContactModel(string languageCode)
            : base(SectionName.Contacto, "contact", languageCode, true)
        {
        }

        public override void AddJs()
        {
            JsManager.addJS("https://maps.google.com/maps/api/js?sensor=false");
            JsManager.addJS("/Content/js/map.js");
        }

        public override void AddCss() { }

        public override void AddDocumentReady() { }
    }
}