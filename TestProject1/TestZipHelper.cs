using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using com.trobadoo.utils.Helpers;
using System.IO;
using System.Diagnostics;

namespace TrobadooUtilsTest
{
    [TestClass]
    public class TestZipHelper
    {
        [TestMethod]
        [Ignore]
        public void TestZipFolder()
        {
            // Directory
            string directoryPath = "D:\\\\git\\\\trobadoo\\\\webTrobadooMVC\\\\Content\\images\\slider";
            string zipFileName = "D:\\temp\\fotos.zip";
            ZipHelper zipHelper = new ZipHelper(directoryPath);
            zipHelper.compressDirectory(zipFileName);

            bool existsFile = File.Exists(zipFileName);

            // Assert
            const bool expected = true;
            Assert.AreEqual(expected, existsFile);
        }

        [TestMethod]
        public void TestGetFilesModifiedSinceNoResults()
        {
            Debug.WriteLine("Send to debug output.");
            // Directory
            string directoryPath = "D:\\\\git\\\\trobadoo\\\\webTrobadooMVC\\\\Content\\images\\slider";
            DateTime dateFrom = DateTime.Now.AddDays(-10);
            FileHelper fileHelper = new FileHelper(directoryPath);
            var files = fileHelper.getFilesNamesModifiedSince(dateFrom,fileHelper.ValidExtensions);

            Debug.WriteLine("Num Files:" + files.Count());
            Debug.WriteLine("Files:" + files);

            // Assert
            Assert.AreEqual(0, files.Count());

        }

        [TestMethod]
        public void TestGetFilesModifiedSince()
        {
            // Directory
            string directoryPath = "D:\\\\git\\\\trobadoo\\\\webTrobadooMVC\\\\Content\\images\\slider";
            DateTime dateFrom = DateTime.Now.AddDays(-300);
            FileHelper fileHelper = new FileHelper(directoryPath);
            var files = fileHelper.getFilesNamesModifiedSince(dateFrom, fileHelper.ValidExtensions);

            // Assert
            Assert.AreNotEqual(0, files.Count());

        }

        [TestMethod]
        public void TestGetFilesLastModificationDate()
        {
#if DEBUG
            Console.WriteLine("the message");
#endif
            // Directory
            string directoryPath = "D:\\\\git\\\\trobadoo\\\\webTrobadooMVC\\\\Content\\images\\slider";
            FileHelper fileHelper = new FileHelper(directoryPath);
            fileHelper.getFilesLastModificationDate();
        }
    }
}
