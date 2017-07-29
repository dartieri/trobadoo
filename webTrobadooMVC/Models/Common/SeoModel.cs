using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using trobadoo.com.web.Base;

namespace trobadoo.com.web.Models.Common
{
    public class SeoModel : SimpleModel
    {
        public SeoModel(string languageCode)
            : base("seo", languageCode)
        {

        }
    }
}