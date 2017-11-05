using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace com.trobadoo.utils.Helpers
{
    public class FileHelper
    {
        private string dirPath;
        private List<string> validExtensions = new List<string> { ".jpg", ".png", ".xml" };

        public FileHelper(string dirPath)
        {
            this.dirPath = dirPath;
        }

        private IEnumerable<FileInfo> getFilesModifiedSince(DateTime fromDate, List<string> extensions)
        {
            var directory = new DirectoryInfo(dirPath);
            DateTime toDate = DateTime.Now;
            var files = directory.GetFiles()
              .Where(file => extensions.Contains(file.Extension) && (file.LastWriteTime >= fromDate && file.LastWriteTime <= toDate)).ToList();
            return files;
        }

        private IEnumerable<FileInfo> getFilesCreatedSince(DateTime fromDate, List<string> extensions)
        {
            var directory = new DirectoryInfo(dirPath);
            DateTime toDate = DateTime.Now;
            var files = directory.GetFiles()
              .Where(file => extensions.Contains(file.Extension) && (file.CreationTime >= fromDate && file.CreationTime <= toDate)).ToList();
            return files;
        }

        public IEnumerable<string> getFilesNamesModifiedSince(DateTime fromDate, List<string> extensions)
        {
            List<string> filesNames = new List<string>();
            var files = getFilesModifiedSince(fromDate, extensions);
            foreach (FileInfo file in files)
            {
                filesNames.Add(file.Name);
            }
            return filesNames;
        }

        public IEnumerable<string> getFilesNamesCreatedSince(DateTime fromDate, List<string> extensions)
        {
            List<string> filesNames = new List<string>();
            var files = getFilesCreatedSince(fromDate, extensions);
            foreach (FileInfo file in files)
            {
                filesNames.Add(file.Name);
            }
            return filesNames;
        }
        public void getFilesLastModificationDate()
        {
            var directory = new DirectoryInfo(dirPath);
            var files = directory.GetFiles();
            foreach (FileInfo file in files)
            {
                Console.WriteLine("File " + file.Name + ": " + file.LastWriteTime);
            }
        }

        public void getFilesCreationDate()
        {
            var directory = new DirectoryInfo(dirPath);
            var files = directory.GetFiles();
            foreach (FileInfo file in files)
            {
                Console.WriteLine("File " + file.Name + ": " + file.LastWriteTime);
            }
        }

        public List<string> ValidExtensions
        {
            get
            {
                return validExtensions;
            }
        }

        public string deleteFile(string filePath)
        {
            try
            {
                FileInfo file = new FileInfo(filePath);
                file.Delete();
            }
            catch (IOException e)
            {
                return string.Format("Error: {0}. Stack: {1}", e.Message, e.StackTrace);
            }
            return null;
        }
    }
}
