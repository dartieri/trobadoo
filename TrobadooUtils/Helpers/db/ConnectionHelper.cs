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

namespace com.trobadoo.utils.Helpers.db
{
    public class ConnectionHelper
    {
        public static string getStringConnection(string server, string user, string password, string bbdd)
        {
            return getStringConnection(server, user, password, bbdd, 30);
        }

        public static string getStringConnection(string server, string user, string password, string bbdd, int timeout)
        {

            return getStringConnection(server, user, password, bbdd, timeout, true, 500);
        }

        public static string getStringConnection(string server, string user, string password, string bbdd, int timeout, bool pooling, int maxPoolSize)
        {

            return String.Format("server={0};Trusted_Connection=false;database={1};user ID={2};password={3};Pooling={4};Max Pool Size={5}; TimeOut={6}", server, bbdd, user, password, pooling.ToString().ToLower(), maxPoolSize, timeout);
        }
    }

}