using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using trobadoo.com.web.Entities;
using System.Web.Mvc;
using trobadoo.com.web.Models.Common;
using trobadoo.com.web.Helpers;

namespace trobadoo.com.web.Base
{
    public class BaseController : Controller
    {
        
        protected string PageName;
        protected string CdnDomain;
        protected string languageCode="es";
        //protected ILog log;

        public void InitController(string pageName, string cdnDomain)
        {
            PageName = pageName;
            CdnDomain = cdnDomain;

            if (!string.IsNullOrEmpty(Request["lang"]))
            {
                languageCode = Request["lang"];
                CookieManager.WriteLanguageCookie(Request["lang"], Response);
            }
            else
            {
                var lang = CookieManager.ReadLanguageCookie(Request);
                if(!string.IsNullOrEmpty(lang)){
                    languageCode = lang;
                }
            }
        }

        public void InitLog()
        {
            //log4net.Config.XmlConfigurator.Configure();
            //log = LogManager.GetLogger(this.GetType()); //aquí procedemos a inicializar el objeto log.
        }
    }
}