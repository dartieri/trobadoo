using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using trobadoo.com.web.Base;
using trobadoo.com.web.Entities;
using trobadoo.com.web.Entities.Enumerations;

namespace trobadoo.com.web.Models.Common
{
    public class HeaderModel : SimpleModel
    {
        public HeaderModel(SectionName activeSection, string languageCode)
            : base("header", languageCode)
        {

        }

        public string LanguageIconClass(string language)
        {
            return language == _languageCode ? "hidden" : string.Empty;
        }
    }
}