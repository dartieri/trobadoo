using System;
using System.Net.Mail;
using System.Configuration;
using System.Collections.Generic;

namespace trobadoo.com.web.Helpers
{
    public class MailHelper
    {
        public static bool SendMail(string fromAddress, string toAddress, string subject, string body, bool isHtml, List<string> attachs)
        {
            try
            {
                var mail = new MailMessage();
                var SmtpServer = new SmtpClient(ConfigurationManager.AppSettings["mailServer"]);
                foreach (var address in fromAddress.Split(new[] { ';' }))
                {
                    mail.From = new MailAddress(address);
                } 
                foreach(var address in toAddress.Split(new []{';'})){
                mail.To.Add(address);
                }
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = isHtml;

                if (attachs != null && attachs.Count > 0)
                {
                    System.Net.Mail.Attachment attachment;
                    foreach (var file in attachs)
                    {
                        attachment = new System.Net.Mail.Attachment(file);
                        mail.Attachments.Add(attachment);
                    }
                }

                //SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["mailUser"], ConfigurationManager.AppSettings["mailPasword"]);
                //SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
            return true;
        }
    }
}