using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using trobadoo.com.web.Base;
using System.IO;
using trobadoo.com.web.Entities;
using System.Configuration;
using trobadoo.com.web.Helpers;
using System.Reflection;
using System.Globalization;

namespace trobadoo.com.web.Controllers.Ajax
{
    public class AjaxSendMailController : AjaxBaseController
    {
        private string _languageCode;
        private string _name;
        private string _surnames;
        private string _mail;
        private string _confMail;
        private string _phoneNumber;
        private string _cellNumber;
        private string _address;
        private string _postCode;
        private string _cityName;
        private string _fileUrl;
        private string _message;
        private string _description;
        private string _buyYear;
        private string _buyPrice;
        private string _productDetails;
        private string _photoId;
        private string _photosNames;

        public override void ProcessData(IValueProvider valueProvider)
        {
            InitLog();
            var respuesta = string.Empty;

            //log.Info("Envio Mail: Entramos en la pagina de envio de mail");

            GetMailParameters(Request);
            var action = Request["action"];
            bool result;
            if (!string.IsNullOrEmpty(action))
            {
                switch (action)
                {
                    case "sendContactMail":
                        result = SendContactMail(Request);
                        if (result)
                        {
                            AjaxResponse.Html = "Gracias. En breve nos pondremos en contacto con usted.";
                            AjaxResponse.Result = "Ok";
                        }
                        else
                        {
                            AjaxResponse.ErrorMessage = "No se ha podido enviar el mail. Revise los logs.";
                            AjaxResponse.Result = "Ko";
                        }
                        break;
                    case "sendValuateMail":
                        var photoToken = Request["photoToken"];
                        var path = Path.Combine(Server.MapPath("/uploadImages"),photoToken);
                        var photos = MailHelper.GetUploadedFiles(path);
                        result = SendValuateMail(Request, photos);
                        if (result)
                        {
                            AjaxResponse.Html = "Gracias. En breve nos pondremos en contacto con usted.";
                            AjaxResponse.Result = "Ok";
                        }
                        else
                        {
                            AjaxResponse.ErrorMessage = "No se ha podido enviar el mail. Revise los logs.";
                            AjaxResponse.Result = "Ko";
                        }
                        break;
                }
            }

        }

        private void GetMailParameters(HttpRequestBase request)
        {
            _languageCode = request["languageCode"];
            _name = request["inputContactName"];
            _surnames = request["inputContactSurnames"];
            _mail = request["inputContactMail"];
            _confMail = request["inputContactConfMail"];
            _phoneNumber = request["inputContactPhone"];
            _cellNumber = request["inputContactCell"];
            _address = request["inputContactAddress"];
            _postCode = request["inputContactPostCode"];
            _cityName = request["inputContactCityName"];
            _message = request["inputContactMessage"];
            _description = request["inputContactDesc"];
            _buyYear = request["inputContactBuyYear"];
            _buyPrice = request["inputContactBuyPrice"];
            _productDetails = request["inputContactMessage"];
        }

        private bool SendContactMail(HttpRequestBase request)
        {
            var mailTemplate = MailHelper.GetEmbeddedResourceText("trobadoo.com.web.Templates.mailTemplate.htm", Assembly.GetExecutingAssembly());
            //var mailContent = new System.Text.StringBuilder();
            var subject = "Formulario Contacto - Trobadoo.com";

            CommonReplacements(ref mailTemplate);

            if (!string.IsNullOrEmpty(_buyYear))
            {
                mailTemplate = mailTemplate.Replace("##BUYYEAR##", _buyYear);
            }
            if (!string.IsNullOrEmpty(_buyPrice))
            {
                mailTemplate = mailTemplate.Replace("##BUYPRICE##", _buyPrice);
            }
            if (!string.IsNullOrEmpty(_message))
            {
                mailTemplate = mailTemplate.Replace("##MESSAGE##", _message);
            }
            try
            {
                //var mailSender = new Toolfactory.Mail.MailSender(ConfigurationManager.AppSettings["mailServer"], ConfigurationManager.AppSettings["mailNoReplyUser"], ConfigurationManager.AppSettings["mailNoReplyPassword"]);
                //mailSender.Send(ConfigurationManager.AppSettings["mailNoreply"], ConfigurationManager.AppSettings["mailInfo"] + ";david.artieri@gmail.com", subject, mailTemplate.ToString(), string.Empty, string.Empty, string.Empty, true);
                return MailHelper.SendMail(ConfigurationManager.AppSettings["mailInfo"] + ";david.artieri@gmail.com", subject, mailTemplate, string.Empty);
            }
            catch (Exception ex)
            {
                //log.Error("ErrorMailValoracion: No se ha podido enviar el mail de valoracion: " + ex.Message + " - {" + mailTemplate + "}",ex);
                return false;
            }
        }

        private bool SendValuateMail(HttpRequestBase request, List<Photo> photos)
        {
            var mailTemplate = MailHelper.GetEmbeddedResourceText("trobadoo.com.web.Templates.valuateMailTemplate.htm", Assembly.GetExecutingAssembly());
            //var mailContent = new System.Text.StringBuilder();
            var subject = "Formulario Valoracion Gratuita - Trobadoo.com";

            CommonReplacements(ref mailTemplate);

            if (!string.IsNullOrEmpty(_description))
            {
                mailTemplate = mailTemplate.Replace("##DESCRIPTION##", _description);
            }
            if (!string.IsNullOrEmpty(_buyYear))
            {
                mailTemplate = mailTemplate.Replace("##BUYYEAR##", _buyYear);
            }
            else
            {
                mailTemplate = mailTemplate.Replace("##BUYYEAR##", "N/A");
            }
            if (!string.IsNullOrEmpty(_buyPrice))
            {
                mailTemplate = mailTemplate.Replace("##BUYPRICE##", _buyPrice);
            }
            else
            {
                mailTemplate = mailTemplate.Replace("##BUYPRICE##", "N/A");
            }
            if (!string.IsNullOrEmpty(_productDetails))
            {
                mailTemplate = mailTemplate.Replace("##DETAILS##", _productDetails);
            }
            var attachs = string.Empty;
            if (photos != null && photos.Count > 0)
            {
                foreach (var photo in photos)
                {
                    attachs += photo.physicalPath + ";";
                }
                attachs.TrimEnd(';');
            }
            try
            {
                return MailHelper.SendMail(ConfigurationManager.AppSettings["mailInfo"] + ";david.artieri@gmail.com", subject, mailTemplate, attachs);
                //var mailSender = new Toolfactory.Mail.MailSender(ConfigurationManager.AppSettings["mailServer"], ConfigurationManager.AppSettings["mailNoReplyUser"], ConfigurationManager.AppSettings["mailNoReplyPassword"]);
                //mailSender.Send(ConfigurationManager.AppSettings["mailNoreply"], ConfigurationManager.AppSettings["mailInfo"] + ";david.artieri@gmail.com", subject, mailTemplate.ToString(), string.Empty, string.Empty, attachs, true);
            }
            catch (Exception ex)
            {
                //log.Error("ErrorMailValoracion: No se ha podido enviar el mail de valoracion: " + ex.Message + " - {" + mailTemplate + "}", ex);
                return false;
            }
        }

        private void CommonReplacements(ref string mailTemplate)
        {
            if (!string.IsNullOrEmpty(_name))
            {
                mailTemplate = mailTemplate.Replace("##NAME##", _name);
            }
            if (!string.IsNullOrEmpty(_surnames))
            {
                mailTemplate = mailTemplate.Replace("##SURNAMES##", _surnames);
            }
            if (!string.IsNullOrEmpty(_mail))
            {
                mailTemplate = mailTemplate.Replace("##EMAIL##", _mail);
            }
            if (!string.IsNullOrEmpty(_phoneNumber))
            {
                mailTemplate = mailTemplate.Replace("##PHONE##", _phoneNumber);
            }
            else
            {
                mailTemplate = mailTemplate.Replace("##PHONE##", "N/A");
            }
            if (!string.IsNullOrEmpty(_cellNumber))
            {
                mailTemplate = mailTemplate.Replace("##CELLPHONE##", _cellNumber);
            }
            else
            {
                mailTemplate = mailTemplate.Replace("##CELLPHONE##", "N/A");
            }
            if (!string.IsNullOrEmpty(_address))
            {
                mailTemplate = mailTemplate.Replace("##ADDRESS##", _address);
            }
            else
            {
                mailTemplate = mailTemplate.Replace("##ADDRESS##", "N/A");
            }
            if (!string.IsNullOrEmpty(_postCode))
            {
                mailTemplate = mailTemplate.Replace("##POSTCODE##", _postCode);
            }
            else
            {
                mailTemplate = mailTemplate.Replace("##POSTCODE##", "N/A");
            }
            if (!string.IsNullOrEmpty(_cityName))
            {
                mailTemplate = mailTemplate.Replace("##CITY##", _cityName);
            }
        }
    }
}
