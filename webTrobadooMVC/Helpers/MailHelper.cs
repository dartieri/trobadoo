using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using trobadoo.com.web.Entities;
using System.Reflection;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using System.Configuration;
using System.Diagnostics;

namespace trobadoo.com.web.Helpers
{
    public class MailHelper
    {
        private static string _server = ConfigurationManager.AppSettings["mailServer"];
        private static int _port = int.Parse(ConfigurationManager.AppSettings["mailServerPort"]);
        private static string _user = ConfigurationManager.AppSettings["mailNoReplyUser"];
        private static string _password = ConfigurationManager.AppSettings["mailNoReplyPassword"];
        private static string _mailFrom = ConfigurationManager.AppSettings["mailNoreply"];

        /// <summary>
        /// Load embedded resource
        /// Use Example
        /// var text = GetEmbeddedResourceText("Namespace.ResourceFileName.txt",
        ///                Assembly.GetExecutingAssembly());
        /// </summary>
        /// <param name="resourceName"></param>
        /// <param name="resourceAssembly"></param>
        /// <returns></returns>
        public static string GetEmbeddedResourceText(string resourceName, Assembly resourceAssembly)
        {
            using (Stream stream = resourceAssembly.GetManifestResourceStream(resourceName))
            {
                int streamLength = (int)stream.Length;
                byte[] data = new byte[streamLength];
                stream.Read(data, 0, streamLength);

                // lets remove the UTF8 file header if there is one:
                if ((data[0] == 0xEF) && (data[1] == 0xBB) && (data[2] == 0xBF))
                {
                    byte[] scrubbedData = new byte[data.Length - 3];
                    Array.Copy(data, 3, scrubbedData, 0, scrubbedData.Length);
                    data = scrubbedData;
                }

                return System.Text.Encoding.UTF8.GetString(data);
            }
        }

        public static List<Photo> GetUploadedFiles(string serverPath)
        {
            List<Photo> photosList = null;
            if (Directory.Exists(serverPath))
            {
                photosList = new List<Photo>();
                foreach (var file in Directory.GetFiles(serverPath))
                {
                    photosList.Add(new Photo
                    {
                        physicalPath = file
                    });
                }
            }
            return photosList;
        }

        public static bool SendMail(string recipients, string subject, string body, string attachmentFilenames)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient();
                NetworkCredential basicCredential = new NetworkCredential(_user, _password);
                MailMessage message = new MailMessage();
                MailAddress fromAddress = new MailAddress(_mailFrom);

                // setup up the host, increase the timeout to 5 minutes
                smtpClient.Host = _server;
                smtpClient.Port = _port;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = basicCredential;
                //smtpClient.EnableSsl = false; // FALSO
                smtpClient.Timeout = (60 * 5 * 1000);

                message.From = fromAddress;
                message.Subject = subject;
                message.IsBodyHtml = true;
                message.Body = body;
                foreach (var recipient in recipients.TrimEnd(';').Split(new[] { ';' }))
                {
                    message.To.Add(recipient);
                }

                if (attachmentFilenames != null && !string.IsNullOrEmpty(attachmentFilenames))
                {
                    foreach (var attachmentFilename in attachmentFilenames.TrimEnd(';').Split(new[] { ';' }))
                    {
                        Attachment attachment = new Attachment(attachmentFilename, MediaTypeNames.Application.Octet);
                        ContentDisposition disposition = attachment.ContentDisposition;
                        disposition.CreationDate = File.GetCreationTime(attachmentFilename);
                        disposition.ModificationDate = File.GetLastWriteTime(attachmentFilename);
                        disposition.ReadDate = File.GetLastAccessTime(attachmentFilename);
                        disposition.FileName = Path.GetFileName(attachmentFilename);
                        disposition.Size = new FileInfo(attachmentFilename).Length;
                        disposition.DispositionType = DispositionTypeNames.Attachment;
                        message.Attachments.Add(attachment);
                    }
                }
                smtpClient.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                //log.Error(String.Format("Error while sending email: " + ex.Message), ex);
                return false;
            }
        }
    }
}