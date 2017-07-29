using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using trobadoo.com.web.Helpers;

namespace trobadoo.com.web.Base
{
    public class SimpleModel
    {
        protected string _pageName;
        protected string _languageCode;

        public SimpleModel(string pageName,string languageCode = "es")
        {
            _pageName = pageName;
            _languageCode = languageCode;
        }

        public string GetTranslation(string key)
        {
            return TranslationManager.GetTranslation(_pageName, _languageCode, key);
        }

        public string LanguageCode
        {
            get
            {
                return _languageCode;
            }
        }

    }
}