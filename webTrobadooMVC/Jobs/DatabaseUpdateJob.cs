using System;
using Quartz;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using trobadoo.com.web.Helpers;
using System.Configuration;
using com.trobadoo.utils.Helpers;
using System.Text;
using System.IO;
using com.trobadoo.utils.Helpers.db;
using trobadoo.com.utils.Helpers.db;
using System.Data.SqlClient;
using System.Data;

namespace trobadoo.com.web.Jobs
{
    public class DatabaseUpdateJob : IJob
    {
        private static string UPDATE_PRODUCTS = "UPSERT_PRODUCT";

        public void Execute(IJobExecutionContext context)
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.AppendFormat("DatabaseUpdate job : {0} {1}, and proceeding to log{2}",
                         DateTime.Now.ToShortDateString(), DateTime.Now.ToLongTimeString(), "\n");

            //Leyendo el xml de productos
            strBuilder.AppendLine("Buscando xml de productos");
            string xmlPath = ConfigurationManager.AppSettings["xmlPath"];
            if (xmlPath == null)
            {
                throw new Exception("Must define the xmlPath parameter");
            }
            FileHelper fileHelper = new FileHelper(xmlPath);

            DateTime lastExecutionTime = DateTime.Now.AddDays(-1);
            if (context.PreviousFireTimeUtc.HasValue)
            {
                lastExecutionTime = context.PreviousFireTimeUtc.Value.DateTime;
            }
            strBuilder.AppendLine("Buscando fichero modificado desde la ultima ejecucion del job: " + lastExecutionTime);
            IEnumerable<string> files = fileHelper.getFilesNamesModifiedSince(lastExecutionTime, new List<string> { ".xml" });

            strBuilder.AppendLine("Ficheros XML encontrados: " + files.Count());

            if (files.Count() > 0)
            {
                //Actualizamos la base de datos a partir del xml de productos
                foreach (string file in files)
                {
                    strBuilder.AppendLine("Fichero: " + file);
                    //Database update
                    bool result = updateDatabase(file);

                    //Si se ha actualizado la base de datos, eliminamos el fichero
                    if (result)
                    {
                        string error = fileHelper.deleteFile(file);
                        if (error != null)
                        {
                            strBuilder.AppendLine(error);
                        }
                    }
                }
            }
            LogHelper.toFile(@"D:\web\logs", strBuilder.ToString());
            Common.Logging.LogManager.Adapter.GetLogger("DatabaseUpdateJob").Info(strBuilder.ToString());

            Console.WriteLine(strBuilder.ToString());
        }

        private bool updateDatabase(string file)
        {
            //TODO Implementar
            DatabaseHelper dbHelper = new DatabaseHelper();
            dbHelper.call(UPDATE_PRODUCTS, getParams(), true, true);
            return false;
        }

        private SqlParametersList getParams()
        {
            SqlParametersList sqlParameterList = new SqlParametersList();
            sqlParameterList.@add("@products", getDataTable(), SqlDbType.Structured);
            throw new NotImplementedException();
        }

        private DataTable getDataTable()
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
            dataTable.Rows.Add("1", "Test 1", DateTime.Now, DateTime.Now.AddDays(-1), DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-20), 1, "FAM1", "Familia 1", 100.00, 120.00, DateTime.Now, null);
            dataTable.Rows.Add("2", "Product 2", DateTime.Now, DateTime.Now.AddDays(-2), DateTime.Now.AddDays(-20), DateTime.Now.AddDays(-40), 10, "FAM2", "Familia 2", 50.00, 70.00, DateTime.Now.AddDays(-5), DateTime.Now.AddDays(-3));
            return dataTable;
        }

    }
}
