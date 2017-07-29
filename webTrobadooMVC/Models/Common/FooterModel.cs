using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using trobadoo.com.web.Base;

namespace trobadoo.com.web.Models.Common
{
    public class FooterModel:SimpleModel
    {
        public FooterModel(string languageCode)
            : base("footer",languageCode)
        {
            
        }
    }
}