using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace com.trobadoo.utils.Helpers
{
    public class LogHelper
    {
        public static void toFile(string logPath, string message)
        {
            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);
            }
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(logPath + @"\console.log", true))
            {
                file.WriteLine(message);
            }
        }
    }
}
