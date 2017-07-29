using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using trobadoo.com.web.Entities;

namespace trobadoo.com.web.Helpers
{
    public static class TranslationManager
    {
        private static Dictionary<string, TranslationsDictionary> _translations;

        static TranslationManager()
        {
            if (_translations == null)
            {
                _translations = new Dictionary<string, TranslationsDictionary>();
            }

        }

        public static string GetTranslation(string pageName,string languageCode, string key)
        {
            return _translations[pageName].GetTranslation(languageCode, key);
        }

        public static void LoadTranslations(){
            var xmlTranslationsFactory = new XmlTranslationsFactory();
            _translations= xmlTranslationsFactory.GetTranslations();
        }

        public static void Reload()
        {
            _translations.Clear();
            LoadTranslations();
        }
    }
}