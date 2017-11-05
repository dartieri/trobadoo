using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Diagnostics;
using com.trobadoo.utils.Helpers.db;
using com.trobadoo.utils.Helpers;
using System.IO;
using trobadoo.com.web.Helpers;
using System.Net;

namespace TrobadooWebUpdater
{
    class Program
    {
        private static StringBuilder strBuilder = new StringBuilder();
        private static string GET_PRODUCTS = "WEB_GET_PRODUCTS_XML";
        private static string xmlPath;
        private static string ftpUrl;
        private static string ftpUser;
        private static string ftpPassword;
        private static string ftpProductsPath;
        private static string ftpImagesPath;
        private static string toAddress;
        private static string fromAddress;
        private static string imagesPath;

        static void Main(string[] args)
        {
            //test();
            execute();
        }

        static void test()
        {
#if DEBUG
            Console.WriteLine("Test");
#endif
            // Directory
            string directoryPath = "D:\\\\git\\\\trobadoo\\\\webTrobadooMVC\\\\Content\\images\\slider";
            FileHelper fileHelper = new FileHelper(directoryPath);
            DateTime fromDate = DateTime.Now.AddDays(-1);
            IEnumerable<string> files = fileHelper.getFilesNamesCreatedSince(fromDate,fileHelper.ValidExtensions);
            foreach (string file in files)
            {
                Console.WriteLine("File:" + file);
            }
            Console.ReadLine();
        }

        static void execute()
        {
            //Inicializa los parametros del proceso
            appendMessage("Inicializamos los parametros del proceso");
            initParameters();

            //Genera el fichero xml de productos en local
            appendMessage("Generamos el fichero xml de productos en local");
            DateTime now = DateTime.Now;
            if (!Directory.Exists(xmlPath))
            {
                Directory.CreateDirectory(xmlPath);
            } 
            string fileName = xmlPath + "products_" + now.ToString("yyyy-MM-dd_HHmm") + ".xml";
            DatabaseHelper dbHelper = new DatabaseHelper();
            XDocument xml = dbHelper.getXml(GET_PRODUCTS, null, false);
            if (xml != null)
            {
                //Guarda el fichero
                appendMessage("Guardamos el fichero " + fileName);
                xml.Save(fileName);

 #if !DEBUG
                //Sube el fichero xml al FTP de Trobadoo
                appendMessage("Subimos el fichero xml al FTP de Trobadoo");
                FtpHelper ftpHelper = new FtpHelper(ftpUrl, ftpUser, ftpPassword);
                try
                {
                    //Subimos el xml al FTP
                    ftpHelper.fileUpload(ftpProductsPath, fileName);
                    appendMessage("Fichero subido al FTP");

                    //Borra el fichero local
                    appendMessage("Borramos el fichero local");
                    File.Delete(fileName);

                    //Sacamos la lista de las fotos
                    FileHelper fileHelper = new FileHelper(imagesPath);
                    DateTime fromDate = DateTime.Now.AddDays(-1);
                    IEnumerable<string> files = fileHelper.getFilesNamesCreatedSince(fromDate, fileHelper.ValidExtensions);

                    if (files.Count() > 0)
                    {
                        //Subimos los ficheros al FTP
                        foreach (string file in files)
                        {
                            var remoteZipFilePath = ftpImagesPath + "/" + file;

                            //Subimos el zip de las fotos al FTP
                            ftpHelper.fileUpload(ftpImagesPath, file);

                            appendMessage("Fichero " + file + " subido al FTP");
                        }
                    }
                    else
                    {
                        appendMessage("No hay fotos nuevas");
                    }
                }
                catch (WebException e)
                {
                    string errorMessage = "<p>No se ha podido subir el fichero " + fileName + " al FTP.</p>";
                    errorMessage += "<p>Error: " + e.Message + "</p>";
                    errorMessage += "<pre>Stack: " + e.StackTrace + "</pre>";
                    appendMessage(errorMessage);
                    MailHelper.SendMail(fromAddress, toAddress, "Error subiendo fichero de productos al FTP", errorMessage, true, null);
                }
#endif
                LogHelper.toFile(@"logs", strBuilder.ToString());
            }
        }

        private static void initParameters()
        {
            xmlPath = ConfigurationManager.AppSettings["xmlPath"];
            if (xmlPath == null)
            {
                throw new Exception("Must define the xmlPath parameter");
            }
            ftpUrl = ConfigurationManager.AppSettings["ftpUrl"];
            if (String.IsNullOrEmpty(ftpUrl))
            {
                throw new Exception("Must define the ftpUrl parameter");
            }
            ftpUser = ConfigurationManager.AppSettings["ftpUser"];
            if (String.IsNullOrEmpty(ftpUser))
            {
                throw new Exception("Must define the ftpUser parameter");
            }
            ftpPassword = ConfigurationManager.AppSettings["ftpPassword"];
            if (String.IsNullOrEmpty(ftpPassword))
            {
                throw new Exception("Must define the ftpPassword parameter");
            }
            ftpProductsPath = ConfigurationManager.AppSettings["ftpProductsPath"];
            if (String.IsNullOrEmpty(ftpProductsPath))
            {
                throw new Exception("Must define the ftpProductsPath parameter");
            }
            ftpImagesPath = ConfigurationManager.AppSettings["ftpImagesPath"];
            if (String.IsNullOrEmpty(ftpImagesPath))
            {
                throw new Exception("Must define the ftpImagesPath parameter");
            }
            fromAddress = ConfigurationManager.AppSettings["mailNoreply"];
            if (String.IsNullOrEmpty(fromAddress))
            {
                throw new Exception("Must define the mailNoreply parameter");
            }
            toAddress = ConfigurationManager.AppSettings["to"];
            if (String.IsNullOrEmpty(toAddress))
            {
                throw new Exception("Must define the to parameter");
            }
            imagesPath = ConfigurationManager.AppSettings["imagesPath"];
            if (String.IsNullOrEmpty(imagesPath))
            {
                throw new Exception("Must define the imagesPath parameter");
            }
        }

        private static void appendMessage(string message)
        {
            strBuilder.AppendLine(DateTime.Now + " - " + message);
        }
    }
}
