using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace trobadoo.com.web.Entities
{
    public class TranslationsDictionary
    {
        private Dictionary<string, Dictionary<string, string>> _translations;

        public TranslationsDictionary()
        {
            _translations = new Dictionary<string, Dictionary<string, string>>();
        }

        public void AddLanguage(string languageCode)
        {
            _translations.Add(languageCode, new Dictionary<string,string>());
        }

        public void AddTranslation(string languageCode, string key, string translation)
        {
            if (_translations[languageCode].All(t => t.Key != languageCode))
            {
                _translations[languageCode].Add(key, translation);
            }
            else
            {
                _translations[languageCode][key]=translation;
            }
        }

        public string GetTranslation(string languageCode,string key)
        {
            var translation = string.Format("[{0}-{1}]", languageCode, key);
            if (ExistTranslations(languageCode))
            {
                var translations = GetTranslations(languageCode);
                translation = ExistTranslation(translations,key) ? translations[key] : translation;
            }
            return translation;
        }

        public Dictionary<string, string> GetTranslations(string languageCode)
        {
            if (ExistTranslations(languageCode))
            {
                return _translations[languageCode];
            } 
            return null;
        }

        public bool ExistTranslations(string languageCode)
        {
            return _translations.ContainsKey(languageCode);
        }

        private bool ExistTranslation(Dictionary<string, string> translations,string key)
        {
            return translations.ContainsKey(key);
        }
    }
}