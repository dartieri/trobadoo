using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using trobadoo.com.web.Helpers;
using trobadoo.com.web.Jobs;

namespace webTrobadooMVC
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            TranslationManager.LoadTranslations();

            JobsManager.initJobs();
        }

        private bool ExecutionMode()
        {
            return System.Configuration.ConfigurationManager.AppSettings["mode"] == "Dev";
        }

        protected void Application_Error()
        {
            var strError = Server.GetLastError().ToString();
            var lastException = Server.GetLastError();


            if (!ExecutionMode())
            {
                Server.ClearError();    //En desarrlllo dejamos que se muestre la pantalla de error
            }

            if (strError.IndexOf("ISAPIWorkerRequestInProcForIIS6.FlushCore") != -1)
            {
                //No tratamos el error de FW 2.0: The remote host closed the connection
                return;
            }

            var codigo = strError;
            var mensajeError = new System.Text.StringBuilder();
            var sesion = string.Empty;
            sesion = Request["sesion"];
            mensajeError.AppendLine("sesion=" + sesion);
            mensajeError.AppendLine(Request.Path);
            mensajeError.AppendLine(DateTime.Now.ToString());
            if (strError.Length > 600)
            {
                mensajeError.AppendLine(strError.Substring(0, 597) + " ...");
            }
            else
            {
                mensajeError.AppendLine(strError);
            }

            //Enviamos el mail de error
            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["mailErrores"]))
            {
                SendMail(System.Configuration.ConfigurationManager.AppSettings["mailNoreply"],
                    System.Configuration.ConfigurationManager.AppSettings["mailErrores"],
                    codigo,
                    GetHtmlMessage(strError, codigo, sesion).ToString());
            }
        }
        private void SendMail(string mailFromDefault, string strMails, string strCodigo, string mensaje)
        {
            //ILog log= log = LogManager.GetLogger(this.GetType());
            if (!ExecutionMode())
            {
                try
                {
                    if (strCodigo != null && strCodigo == "TROB00")
                    {
                        MailHelper.SendMail(strMails, "Web Trobadoo", "Trobadoo.com - Mensaje de la aplicacion", strCodigo + " - " + DateTime.Now);
                    }
                    else
                    {
                        MailHelper.SendMail(strMails, "Web Trobadoo", "Trobadoo.com - Error de la aplicacion", strCodigo + " - " + Request.ServerVariables["HTTP_HOST"] + " - " + Request.ServerVariables["LOCAL_ADDR"] + " - " + Request.Path + " - " + DateTime.Now);
                    }
                }
                catch (Exception ex)
                {
                    MailHelper.SendMail("david.artieri@gmail.com", "Web Trobadoo", "Trobadoo.com - Error de la aplicacion", strCodigo + " - " + ex.Message + " - " + DateTime.Now);
                }
            }
        }


        private System.Text.StringBuilder GetHtmlMessage(string strError, string codigo, string sesion)
        {
            var mensaje = new System.Text.StringBuilder();
            mensaje.Append("<html><body>");
            mensaje.Append("<table width='650' border=1 cellpadding=0 cellspacing=0 bordercolor='#dddddd'>");
            mensaje.Append(" <tr>");
            mensaje.Append(" <td><h1>Ha ocurrido un error en la aplicacion ");
            mensaje.Append(", en la url " + Request.Path + "</h1>");
            mensaje.Append(strError + "</td>");
            mensaje.Append(" </tr>");
            mensaje.Append(" <tr>");
            mensaje.Append(" <td><b>Código de error:</b>+nbsp;" + codigo + "</td>");
            mensaje.Append(" </tr>");
            mensaje.Append(" <tr>");
            mensaje.Append(" <td><b>Número de sesión:</b>+nbsp;" + sesion + "</td>");
            mensaje.Append(" </tr>");
            mensaje.Append(" <tr>");
            mensaje.Append(" <td><b>Fecha y hora:</b>+nbsp;" + DateTime.Now + "</td>");
            mensaje.Append(" </tr>");
            mensaje.Append(" <tr>");
            mensaje.Append(" <td><b>Thread ID:</b>+nbsp;" + System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() + "</td>");
            mensaje.Append(" </tr>");


            mensaje.Append(" <tr>");
            mensaje.Append(" <td>&nbsp;<b>PARÁMETROS REQUEST:</b></td>");
            mensaje.Append(" </tr>");
            mensaje.Append(" <tr>");
            mensaje.Append(" <td><table>");
            foreach (string item in Request.QueryString)
            {
                mensaje.Append(" <tr>");
                mensaje.Append(" <td>G- " + item + ":</td>");
                mensaje.Append(" <td>" + Request[item] + "</td>");
                mensaje.Append(" </tr>");
            }
            foreach (string item in Request.Form)
            {
                mensaje.Append(" <tr>");
                mensaje.Append(" <td>P- " + item + ":</td>");
                mensaje.Append(" <td>" + Request[item] + "</td>");
                mensaje.Append(" </tr>");
            }
            mensaje.Append(" </table></td>");
            mensaje.Append(" </tr>");
            mensaje.Append(" <tr>");
            mensaje.Append(" <td>&nbsp;<b>PARÁMETROS SERVER_VARIABLES:</b></td>");
            mensaje.Append(" </tr>");
            mensaje.Append(" <tr>");
            mensaje.Append(" <td><table>");
            foreach (string item in Request.ServerVariables)
            {
                mensaje.Append(" <tr>");
                mensaje.Append(" <td>" + item + ":</td>");
                mensaje.Append(" <td>" + Request.ServerVariables[item] + "</td>");
                mensaje.Append(" </tr>");
            }
            mensaje.Append(" </table></td>");
            mensaje.Append(" </tr>");
            mensaje.Append("</table>");
            mensaje.Append("</body></html>");

            return mensaje;
        }
    }
}