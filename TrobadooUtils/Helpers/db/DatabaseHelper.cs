using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Reflection;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using System.Configuration;
using System.Diagnostics;
using System.Text;
using System.Data;
using System.Xml.Linq;
using trobadoo.com.utils.Helpers.db;

namespace com.trobadoo.utils.Helpers.db
{
    public class DatabaseHelper
    {
        private string strConWriteWeb;
        private string strConReadTrobadoo;
        private string strConReadWeb;

        #region "Constructor"
        public DatabaseHelper()
        {
            //TROBADOO
            string serverReadTrobadoo, userReadTrobadoo, bdPasswordReadTrobadoo, databaseReadTrobadoo;
            string serverReadWeb, userReadWeb, bdPasswordReadWeb, databaseReadWeb;
            string serverWriteWeb, userWriteWeb, bdPasswordWriteWeb, databaseWriteWeb;
            //LECTURA
            if (ConfigurationManager.AppSettings["bdServerReadTrobadoo"] == null)
            {
                throw new Exception("Parameter bdServerReadTrobadoo not defined");
            }
            serverReadTrobadoo = System.Configuration.ConfigurationManager.AppSettings["bdServerReadTrobadoo"];
            if (System.Configuration.ConfigurationManager.AppSettings["bdUserReadTrobadoo"] == null)
            {
                throw new Exception("Parameter bdUserReadTrobadoo not defined");
            }
            userReadTrobadoo = System.Configuration.ConfigurationManager.AppSettings["bdUserReadTrobadoo"];
            if (System.Configuration.ConfigurationManager.AppSettings["bdPasswordReadTrobadoo"] == null)
            {
                throw new Exception("Parameter bdPasswordReadTrobadoo not defined");
            }
            bdPasswordReadTrobadoo = System.Configuration.ConfigurationManager.AppSettings["bdPasswordReadTrobadoo"];
            if (System.Configuration.ConfigurationManager.AppSettings["bdDatabaseReadTrobadoo"] == null)
            {
                throw new Exception("Parameter bdDatabaseReadTrobadoo not defined");
            }
            databaseReadTrobadoo = System.Configuration.ConfigurationManager.AppSettings["bdDatabaseReadTrobadoo"];

            if (ConfigurationManager.AppSettings["bdServerReadWeb"] == null)
            {
                throw new Exception("Parameter bdServerReadWeb not defined");
            }
            serverReadWeb = System.Configuration.ConfigurationManager.AppSettings["bdServerReadWeb"];
            if (System.Configuration.ConfigurationManager.AppSettings["bdUserReadWeb"] == null)
            {
                throw new Exception("Parameter bdUserReadWeb not defined");
            }
            userReadWeb = System.Configuration.ConfigurationManager.AppSettings["bdUserReadWeb"];
            if (System.Configuration.ConfigurationManager.AppSettings["bdPasswordReadWeb"] == null)
            {
                throw new Exception("Parameter bdPasswordReadWeb not defined");
            }
            bdPasswordReadWeb = System.Configuration.ConfigurationManager.AppSettings["bdPasswordReadWeb"];
            if (System.Configuration.ConfigurationManager.AppSettings["bdDatabaseReadWeb"] == null)
            {
                throw new Exception("Parameter bdDatabaseReadWeb not defined");
            }
            databaseReadWeb = System.Configuration.ConfigurationManager.AppSettings["bdDatabaseReadWeb"];
            
            //ESCRITURA
            if (System.Configuration.ConfigurationManager.AppSettings["bdServerWriteWeb"] == null)
            {
                throw new Exception("Parameter bdServerWriteWeb not defined");
            }
            serverWriteWeb = System.Configuration.ConfigurationManager.AppSettings["bdServerWriteWeb"];
            if (System.Configuration.ConfigurationManager.AppSettings["bdUserWriteWeb"] == null)
            {
                throw new Exception("Parameter bdUserWriteWeb");
            }
            userWriteWeb = System.Configuration.ConfigurationManager.AppSettings["bdUserWriteWeb"];
            if (System.Configuration.ConfigurationManager.AppSettings["bdPasswordWriteWeb"] == null)
            {
                throw new Exception("Parameter bdPasswordWriteWeb not defined");
            }
            bdPasswordWriteWeb = System.Configuration.ConfigurationManager.AppSettings["bdPasswordWriteWeb"];
            if (System.Configuration.ConfigurationManager.AppSettings["bdDatabaseWriteWeb"] == null)
            {
                throw new Exception("Parameter bdDatabaseWriteWeb not defined");
            }
            databaseWriteWeb = System.Configuration.ConfigurationManager.AppSettings["bdDatabaseWrite"];

            inicializa(serverReadTrobadoo, databaseReadTrobadoo, userReadTrobadoo, bdPasswordReadTrobadoo, serverReadWeb, databaseReadWeb, userReadWeb, bdPasswordReadWeb, serverWriteWeb, databaseWriteWeb, userWriteWeb, bdPasswordWriteWeb);
        }

        private void inicializa(string serverReadTrobadoo, string bdReadTrobadoo, string userReadTrobadoo, string passwordReadTrobadoo, string serverReadWeb, string bdReadWeb, string userReadWeb, string passwordReadWeb, string serverWriteWeb, string bdWriteWeb, string userWriteWeb, string passwordWriteWeb)
        {
            // conexion a la BBDD de TROBADOO
            strConReadTrobadoo = ConnectionHelper.getStringConnection(serverReadTrobadoo, userReadTrobadoo, passwordReadTrobadoo, bdReadTrobadoo);
            strConReadWeb = ConnectionHelper.getStringConnection(serverReadWeb, userReadWeb, passwordReadWeb, bdReadWeb);
            strConWriteWeb = ConnectionHelper.getStringConnection(serverWriteWeb, userWriteWeb, passwordWriteWeb, bdWriteWeb);
        }

        #endregion

        #region "Métodos de acceso a BBDD"
        public DataTable call(string procedureName, SqlParametersList procedureParams, bool webDatabase, bool write)
        {
            return DB.SingleInstance.execProcedureData(webDatabase ? (write ? strConWriteWeb : strConReadWeb) : strConReadTrobadoo, procedureName, procedureParams, CommandType.StoredProcedure);
        }

        public void executeQueryNoResult(string query)
        {
            DB.SingleInstance.execProcedureData(strConWriteWeb, query, null, CommandType.Text);
        }

        public object executeQueryResult(string query)
        {
            return DB.SingleInstance.execProcedureEscalar(strConWriteWeb, query, null, CommandType.Text);
        }

        public DataTable getDataTable(string query, bool webDatabase)
        {
            return DB.SingleInstance.execProcedureData(webDatabase ? strConReadWeb : strConReadTrobadoo, query, null, CommandType.Text);
        }

        public XDocument getXml(string procedureName, SqlParametersList parameters, bool webDatabase)
        {
            return DB.SingleInstance.execProcedureXml(webDatabase ? strConReadWeb : strConReadTrobadoo, procedureName, parameters);
        }

        #endregion

        #region "Utils"
        public static string bdString(string valor)
        {
            if (valor == "")
            {
                return "''";
            }
            else
            {
                return "'" + valor.Replace("'", "''") + "'";
            }
        }

        public static string bdNullString(string valor)
        {
            if (String.IsNullOrEmpty(valor))
            {
                return "'NULL'";
            }
            else
            {
                return "'" + valor.Replace("'", "''") + "'";
            }
        }
        #endregion

    }
}