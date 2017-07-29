using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI;
using System.Web.UI.WebControls;
using trobadoo.com.web.Entities;
using trobadoo.com.web.filters;
using trobadoo.com.web.Helpers;

namespace trobadoo.com.web.Base
{
    public class AjaxBaseController : BaseController
    {
        public AjaxBaseResponse AjaxResponse = new AjaxBaseResponse();

        [AjaxActionFilter]
        public void Index()
        {
ProcessData(ValueProvider);
        }

        #region Private Methods
        
        #endregion

        #region public Methods

        #endregion

        public virtual void ProcessData(IValueProvider valueProvider) { }

        protected string RenderPartialString(string viewName, object model)
        {
            var r = new ViewRendererHelper();
            return r.RenderPartialViewToString(viewName, model);
        }
    }
}
