using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.IO;

namespace trobadoo.com.web.Helpers
{
    public class ResourceManager
    {
        public static string LoadResource(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                var result = reader.ReadToEnd();
                return result;
            }
        }
    }
}