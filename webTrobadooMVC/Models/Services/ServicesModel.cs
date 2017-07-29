using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using trobadoo.com.web.Base;
using trobadoo.com.web.Models.Carousel;
using trobadoo.com.web.Entities.Enumerations;

namespace trobadoo.com.web.Models.Services
{
    public class ServicesModel:BaseModel
    {
        public ServicesModel(string languageCode)
            : base(SectionName.Servicios,"services",languageCode, true)
        {
            BaseCarouselModel = new HomeCarouselModel();
        }

        public override void AddCss() { }
        public override void AddJs() { }
        public override void AddDocumentReady() { }
    }
}