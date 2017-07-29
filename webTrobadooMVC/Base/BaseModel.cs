using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using trobadoo.com.web.Models.Common;
using trobadoo.com.web.Entities;
using trobadoo.com.web.Models.Carousel;
using trobadoo.com.web.Entities.Enumerations;
using trobadoo.com.web.Helpers;

namespace trobadoo.com.web.Base
{
    public abstract class BaseModel
    {
        private string _pageName;
        private string _languageCode;
        protected SEOLibrary.cGestorJS JsManager;
        protected SEOLibrary.cGestorCSS CssManager;
        protected JsInitializations JsInits;

        private bool _showTopContainer;
        private string cdnDomain;
        public HeaderInfo HeaderInfo;
        public HeaderModel HeaderModel;
        public FooterModel FooterModel;
        public SeoModel SeoModel; 
        public BaseCarouselModel BaseCarouselModel;

        public BaseModel(SectionName activeSection, string pageName, string languageCode = "es", bool showTopContainer = false)
        {
            _pageName = pageName;
            _languageCode = languageCode;
            _showTopContainer = showTopContainer;
            HeaderModel = new HeaderModel(activeSection, _languageCode);
            FooterModel = new FooterModel(_languageCode);
            SeoModel = new SeoModel(_languageCode);

            JsManager = new SEOLibrary.cGestorJS(cdnDomain);
            CssManager = new SEOLibrary.cGestorCSS(pageName, cdnDomain);
            JsInits = new JsInitializations();

            CssManager.addCss("/Content/css/font-awesome.min.css");
            CssManager.addCss("/Content/css/bootstrap.min.css");
            CssManager.addCss("/Content/css/helpers.css");
            CssManager.addCss("/Content/css/trobadoo.css");
            CssManager.addCss("/Content/css/notify.css"); 
            CssManager.addCss("/Content/css/owl.carousel.css");
            CssManager.addCss("/Content/css/owl.theme.css");
            CssManager.addCss("/Content/css/owl.transitions.css");

            JsManager.addJS("/Content/js/jquery-1.11.1.min.js");
            JsManager.addJS("/Content/js/jquery.lazyload.js");
            JsManager.addJS("/Content/js/bootstrap.min.js");
            JsManager.addJS("/Content/js/jquery.diseno.js");
            JsManager.addJS("/Content/js/notify.min.js");
            //JsManager.addJS("/Content/js/noty/jquery.noty.packaged.min.js");
            JsManager.addJS("/Content/js/owl.carousel.min.js");
            JsManager.addJS("/Content/js/trobadoo.js");
        }

        public string GetTranslation(string key)
        {
            return TranslationManager.GetTranslation(_pageName, _languageCode,key);
        }

        public string PrintCssIncludes()
        {
            AddCss();
            return CssManager.getIncludeCSS();
        }

        public string PrintJsIncludes()
        {
            AddJs();
            return JsManager.getIncludeJS();
        }

        public string PrintJsInits()
        {
            AddDocumentReady();
            return JsInits.Print();
        }

        public abstract void AddJs();
        public abstract void AddCss();
        public abstract void AddDocumentReady();
        public string LanguageCode
        {
            get
            {
                return _languageCode;
            }
        }

        public bool ShowTopContainer
        {
            get
            {
                return _showTopContainer;
            }
        }
    }
}