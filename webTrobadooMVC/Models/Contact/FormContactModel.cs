using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using trobadoo.com.web.Base;
using trobadoo.com.web.Models.Carousel;
using trobadoo.com.web.Entities.Enumerations;
using System.Text;
using trobadoo.com.web.Helpers;

namespace trobadoo.com.web.Models.Contact
{
    public class FormContactModel:BaseModel
    {
        private readonly string _languageCode;

        public FormContactModel(string languageCode)
            : base(SectionName.Contacto, "contactForm", languageCode, true)
        {
            _languageCode = languageCode;
        }

        public override void AddCss() { }
        public override void AddJs() {
            JsManager.addJS("/Content/js/jquery.validate.min.js");
            JsManager.addJS("/Content/js/additional-methods.min.js");
            JsManager.addJS("/Content/js/contact.js");
        }

        public override void AddDocumentReady()
        {
            var strOptions = new StringBuilder();
            strOptions.AppendFormat("thankyouLit:'{0}',", GetTranslation("15"));
            strOptions.AppendFormat("errorLit:'{0}'", GetTranslation("16"));

            var str = new StringBuilder();
            for (var i = 1; i <= 17; i++)
            {
                str.AppendFormat("trobadoo.com.contact.options.validationLiterals[{0}]='{1}';", i, TranslationManager.GetTranslation("formValidation", _languageCode, i.ToString()));
            }
            JsInits.AddDocumentReadyAction(string.Format("trobadoo.com.contact.init({{{0}}});",strOptions));
            JsInits.AddDocumentReadyAction(str.ToString());
            JsInits.AddDocumentReadyAction("trobadoo.com.contact.loadValidationRules();");
        }
    }
}