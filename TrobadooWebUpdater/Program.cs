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
using trobadoo.com.utils.Helpers.db;
using System.Data;

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
            //execute();
            testDb();
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
            IEnumerable<string> files = fileHelper.getFilesNamesCreatedSince(fromDate, fileHelper.ValidExtensions);
            foreach (string file in files)
            {
                Console.WriteLine("File:" + file);
            }
            Console.ReadLine();
        }

        static void testDb()
        {
            initParameters();

#if DEBUG
            Console.WriteLine("Test Db");
#endif
            DateTime lastExecutionTime = DateTime.Now.AddDays(-10);
            FileHelper fileHelper = new FileHelper(xmlPath);
            IEnumerable<string> files = fileHelper.getFilesNamesModifiedSince(lastExecutionTime, new List<string> { ".xml" });

            strBuilder.AppendLine("Ficheros XML encontrados: " + files.Count());

            if (files.Count() > 0)
            {
                foreach (string file in files)
                {
                    Console.WriteLine("File:" + file);

                    readXml(xmlPath + file);
                    /*
                    bool result = updateDatabase(file);

                    Console.WriteLine("------------------------");
                    Console.WriteLine("Result:" + result);

                    Console.WriteLine("------------------------");
                    string familyCode = null;
                    DataTable dt = getProducts(familyCode);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            Console.WriteLine("Row--> Code:" + row["pro_code"] + " - " + row["pro_description"] + "\n");
                        }
                    }
                    Console.WriteLine("Result:" + result);
                    */
                }
                Console.WriteLine("Type any key to exit");
                Console.ReadLine();
            }
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


        private static bool updateDatabase(string file)
        {
            //TODO Implementar
            DatabaseHelper dbHelper = new DatabaseHelper();
            dbHelper.call("UPSERT_PRODUCT", getParams(), true, true);
            return true;
        }

        private static DataTable getProducts(string familyCode)
        {
            //TODO Implementar
            DatabaseHelper dbHelper = new DatabaseHelper();
            return dbHelper.call("GET_PRODUCTS", getFamilyCodeParam(familyCode), true, true);
        }
        private static SqlParametersList getParams()
        {
            SqlParametersList sqlParameterList = new SqlParametersList();
            sqlParameterList.@add("@products", getDataTable(), SqlDbType.Structured);
            return sqlParameterList;
        }

        private static SqlParametersList getFamilyCodeParam(string familyCode)
        {
            SqlParametersList sqlParameterList = new SqlParametersList();
            sqlParameterList.@add("@familyCode", familyCode, SqlDbType.Text);
            return sqlParameterList;
        }

        private static DataTable getDataTable()
        {
            DataTable dataTable = new DataTable("productItem");
            //we create column names as per the type in DB

            dataTable.Columns.Add("pro_code", typeof(string));
            dataTable.Columns.Add("pro_description", typeof(string));
            dataTable.Columns.Add("pro_depositCreationDate", typeof(DateTime));
            dataTable.Columns.Add("pro_creationDate", typeof(DateTime));
            dataTable.Columns.Add("pro_lastModificationDate", typeof(DateTime));
            dataTable.Columns.Add("pro_formerModificationDate", typeof(DateTime));
            dataTable.Columns.Add("pro_stock", typeof(Int32));
            dataTable.Columns.Add("fam_code", typeof(string));
            dataTable.Columns.Add("fam_description", typeof(string));
            dataTable.Columns.Add("pro_initialPrice", typeof(decimal));
            dataTable.Columns.Add("pro_sellPrice", typeof(decimal));
            dataTable.Columns.Add("dat_cre", typeof(DateTime));
            dataTable.Columns.Add("dat_mod", typeof(DateTime));

            //and fill in some values 
            for (var i = 1; i < 5; i++)
            {
                dataTable.Rows.Add(i, "Test " + i, DateTime.Now, DateTime.Now.AddDays(-i), DateTime.Now.AddDays(-10 * i), DateTime.Now.AddDays(-10 * i + 10), 1, "FAM" + i % 3, "Familia " + i % 3, 100.00 * i, 100 * i + 25, null, null);
            }
            return dataTable;
        }

        private static void readXml(string filePath)
        {
            //Load xml
            Console.WriteLine(filePath);

            XDocument xdoc = XDocument.Load(filePath);

            //Run query
            var products = from product in xdoc.Descendants("Product")
                           select new
                           {
                               CreationDate = product.Attribute("creationDate") != null ? product.Attribute("creationDate").Value : null,
                               LastModificationDate = product.Attribute("lastModificationDate") != null ? product.Attribute("lastModificationDate").Value : null,
                               FormerModificationDate = product.Attribute("formerModificationDate") != null ? product.Attribute("formerModificationDate").Value : null,
                               Code = product.Descendants("Code").FirstOrDefault().Value,
                               Description = product.Descendants("Description").FirstOrDefault().Value,
                               Stock = product.Descendants("Description").FirstOrDefault().Value,
                               FamilyCode = product.Descendants("Family").FirstOrDefault().Descendants("Code").FirstOrDefault().Value,
                               FamilyDescription = product.Descendants("Family").FirstOrDefault().Descendants("Description").FirstOrDefault().Value,
                               InitialPrice = product.Descendants("Price").FirstOrDefault().Descendants("Initial").FirstOrDefault().Value,
                               SellPrice = product.Descendants("Price").FirstOrDefault().Descendants("Sell").FirstOrDefault().Value,

                           };

            //Loop through results

            Console.WriteLine("Results found: " + products.ToList().Count);

            foreach (var product in products)
            {
                StringBuilder result = new StringBuilder();
                result.AppendLine("CreationDate: " + product.CreationDate);
                result.AppendLine("LastModificationDate: " + product.LastModificationDate);
                result.AppendLine("FormerModificationDate: " + product.FormerModificationDate);
                result.AppendLine("Code:" + product.Code.First());
                result.AppendLine("Description: " + product.Description);
                result.AppendLine("Stock: " + product.Stock);
                result.AppendLine("FamilyCode: " + product.FamilyCode);
                result.AppendLine("FamilyDescription: " + product.FamilyDescription);
                result.AppendLine("InitialPrice: " + product.InitialPrice);
                result.AppendLine("SellPrice: " + product.SellPrice);
                result.AppendLine("------------------------------ ");
                Console.WriteLine(result);
            }
        }
    }
}
