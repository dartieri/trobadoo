using System;
using System.Web.Mvc;
using trobadoo.com.web.Base;

namespace trobadoo.com.web.filters
{
    public class AjaxActionFilter : ActionFilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var controller = filterContext.Controller as AjaxBaseController;
            if (controller == null) return;
            
            var result = new JsonResult()
            {
                Data = controller.AjaxResponse,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                ContentType = "application/json; charset=utf-8"
            };
            filterContext.Result  = result;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //var controller = filterContext.Controller as AjaxBaseController;
            //if (controller == null) return;
            //controller.ProcessData(filterContext.Controller.ValueProvider);
        }
    }
}