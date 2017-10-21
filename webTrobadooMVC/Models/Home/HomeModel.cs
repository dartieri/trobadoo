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
        public override void AddDocumentReady() {
            /*var strOptions = new StringBuilder();
            strOptions.AppendFormat("thankyouLit:'{0}',", GetTranslation("15"));
            strOptions.AppendFormat("errorLit:'{0}'", GetTranslation("16"));

            var str = new StringBuilder();
            for (var i = 1; i <= 17; i++)
            {
                str.AppendFormat("trobadoo.com.contact.options.validationLiterals[{0}]='{1}';", i, TranslationManager.GetTranslation("formValidation", _languageCode, i.ToString()));
            }*/
            JsInits.AddDocumentReadyAction("com.trobadoo.Home.init();");
            //JsInits.AddDocumentReadyAction(str.ToString());
            //JsInits.AddDocumentReadyAction("trobadoo.com.contact.loadValidationRules();");
        }

    }
}