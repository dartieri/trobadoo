using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using trobadoo.com.web.Base;
using trobadoo.com.web.Models.Carousel;
using trobadoo.com.web.Entities.Enumerations;

namespace trobadoo.com.web.Models.Home
{
    public class HomeModel : BaseModel
    {
        public PresentationModel PresentationModel;

        public HomeModel(string languageCode)
            : base(SectionName.Home, "home", languageCode, true)
        {
            BaseCarouselModel = new HomeCarouselModel();

            PresentationModel = new PresentationModel(languageCode);
        }

        public override void AddJs()
        {
            JsManager.addJS("/Content/js/home.js");
        }
        public override void AddCss() { }
        public override void AddDocumentReady() { }

    }
}