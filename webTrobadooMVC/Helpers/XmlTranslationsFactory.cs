using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using trobadoo.com.web.Entities;

namespace trobadoo.com.web.Helpers
{
    public class XmlTranslationsFactory
    {
        private XmlDocument _xmlDoc;

        public XmlTranslationsFactory()
        {
            var xmlString = ResourceManager.LoadResource("trobadoo.com.web.xml.translations.xml");
            _xmlDoc = new XmlDocument();
            _xmlDoc.LoadXml(xmlString);
        }

        public Dictionary<string, TranslationsDictionary> GetTranslations()
        {
            if (_xmlDoc != null)
            {
                var translations = new Dictionary<string, TranslationsDictionary>();
                var xPath = "//pages/page";
                var pageNodes = _xmlDoc.SelectNodes(xPath);
                foreach (XmlNode pageNode in pageNodes)
                {
                    if (pageNode != null && pageNode.FirstChild != null && pageNode.Attributes.Count > 0 && pageNode.Attributes["id"] != null && !string.IsNullOrEmpty(pageNode.Attributes["id"].Value))
                    {
                        var pageName = pageNode.Attributes["id"].Value;
                        var xPath2 = string.Format("language");
                        var languageNodes = pageNode.SelectNodes(xPath2);

                        translations.Add(pageName, new TranslationsDictionary());

                        foreach (XmlNode languageNode in languageNodes)
                        {
                            if (languageNode != null && languageNode.FirstChild != null && languageNode.Attributes.Count > 0 && languageNode.Attributes["id"] != null && !string.IsNullOrEmpty(languageNode.Attributes["id"].Value))
                            {
                                var languageCode = languageNode.Attributes["id"].Value;
                                if (!translations[pageName].ExistTranslations(languageCode))
                                {
                                    translations[pageName].AddLanguage(languageCode);
                                }
                                var xPath3 = "literal";
                                var literalNodes = languageNode.SelectNodes(xPath3);

                                foreach (XmlNode literalNode in literalNodes)
                                {
                                    if (literalNode != null && literalNode.FirstChild != null && !string.IsNullOrEmpty(literalNode.FirstChild.Value) && literalNode.Attributes.Count > 0 && literalNode.Attributes["id"] != null && !string.IsNullOrEmpty(literalNode.Attributes["id"].Value))
                                    {
                                        translations[pageName].AddTranslation(languageCode, literalNode.Attributes["id"].Value, literalNode.FirstChild.Value);
                                    }
                                }
                            }
                        }
                    }
                }
                return translations;
            }
            return null;
        }
    }
}