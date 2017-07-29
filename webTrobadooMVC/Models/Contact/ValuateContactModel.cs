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
    public class ValuateContactModel : BaseModel
    {
        private readonly string _languageCode;
        private readonly string _photoToken;

        public ValuateContactModel(string languageCode)
            : base(SectionName.Contacto, "appraisalForm", languageCode, true)
        {
            _languageCode = languageCode;
            var ahora = DateTime.Now;
            _photoToken = ahora.Year.ToString() + ahora.Month + ahora.Day + ahora.Hour + ahora.Minute + ahora.Second + ahora.Millisecond;
        }

        public string PhotoToken
        {
            get
            {
                return _photoToken;
            }
        }

        public string DisplayYears()
        {
            var str = new System.Text.StringBuilder();
            str.AppendLine("<option value=\"-1\">Año de compra (aprox.)</option>");
            for (var i = DateTime.Now.Year; i >= DateTime.Now.AddYears(-100).Year; i--)
            {
                str.AppendLine(string.Format("<option>{0}</option>", i));
            }
            str.AppendLine("<option value=\"1000\">Más antiguo</option>");
            return str.ToString();
        }

        public override void AddCss()
        {
            CssManager.addCss("/Content/js/jquery.fileUpload/css/jquery.fileupload-ui.css");
            CssManager.addCss("/Content/js/jquery.fileUpload/css/jquery.fileupload.css");
            //CssManager.addCss("http://blueimp.github.io/Gallery/css/blueimp-gallery.min.css");
        }

        public override void AddJs()
        {
            JsManager.addJS("/Content/js/jquery.fileUpload/js/vendor/jquery.ui.widget.js");
            JsManager.addJS("/Content/js/jquery.fileUpload/js/tmpl.js");
            JsManager.addJS("http://blueimp.github.io/JavaScript-Load-Image/js/load-image.all.min.js");
            JsManager.addJS("http://blueimp.github.io/JavaScript-Canvas-to-Blob/js/canvas-to-blob.min.js");
            JsManager.addJS("http://netdna.bootstrapcdn.com/bootstrap/3.2.0/js/bootstrap.min.js");
            JsManager.addJS("http://blueimp.github.io/Gallery/js/jquery.blueimp-gallery.min.js");
            JsManager.addJS("/Content/js/jquery.fileUpload/js/jquery.iframe-transport.js");
            JsManager.addJS("/Content/js/jquery.fileUpload/js/jquery.fileupload.js");
            JsManager.addJS("/Content/js/jquery.fileUpload/js/jquery.fileupload-process.js");
            JsManager.addJS("/Content/js/jquery.fileUpload/js/jquery.fileupload-image.js");
            JsManager.addJS("/Content/js/jquery.fileUpload/js/jquery.fileupload-audio.js");
            JsManager.addJS("/Content/js/jquery.fileUpload/js/jquery.fileupload-video.js");
            JsManager.addJS("/Content/js/jquery.fileUpload/js/jquery.fileupload-validate.js");
            JsManager.addJS("/Content/js/jquery.fileUpload/js/jquery.fileupload-ui.js");
            //JsManager.addJS("/Content/js/jquery.fileUpload/js/main.js");
            JsManager.addJS("/Content/js/jquery.validate.min.js");
            JsManager.addJS("/Content/js/additional-methods.min.js");
            JsManager.addJS("/Content/js/valuate.js");

        }

        public override void AddDocumentReady()
        {
            var strOptions = new StringBuilder();
            strOptions.AppendFormat("photoToken:'{0}',", _photoToken);
            strOptions.AppendFormat("thankyouLit:'{0}',", GetTranslation("22"));
            strOptions.AppendFormat("errorLit:'{0}'", GetTranslation("23"));

            var str = new StringBuilder();
            for (var i = 1; i <= 17; i++)
            {
                str.AppendFormat("trobadoo.com.valuate.options.validationLiterals[{0}]='{1}';", i, TranslationManager.GetTranslation("formValidation", _languageCode, i.ToString()));
            }
            JsInits.AddDocumentReadyAction(string.Format("trobadoo.com.valuate.init({{{0}}});", strOptions));
            JsInits.AddDocumentReadyAction(str.ToString());
            JsInits.AddDocumentReadyAction("trobadoo.com.valuate.loadValidationRules();");
        }
    }
}