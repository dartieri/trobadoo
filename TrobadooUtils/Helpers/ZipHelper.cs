using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using Ionic.Zip;

namespace com.trobadoo.utils.Helpers
{
    public class ZipHelper
    {
        private string dirPath;

        public ZipHelper(string dirPath)
        {
            this.dirPath = dirPath;
        }

        public void compressDirectory()
        {
            DirectoryInfo di = new DirectoryInfo(dirPath);

            // Compress the directory's files.
            foreach (FileInfo fi in di.GetFiles())
            {
                Compress(fi);

            }
        }

        public void decompressDirectory(string dirPath)
        {
            DirectoryInfo di = new DirectoryInfo(dirPath);

            // Decompress all *.gz files in the directory.
            foreach (FileInfo fi in di.GetFiles("*.gz"))
            {
                Decompress(fi);

            }
        }

        private void Compress(FileInfo fi)
        {
            // Get the stream of the source file.
            using (FileStream inFile = fi.OpenRead())
            {
                // Prevent compressing hidden and 
                // already compressed files.
                if ((File.GetAttributes(fi.FullName)
                    & FileAttributes.Hidden)
                    != FileAttributes.Hidden & fi.Extension != ".gz")
                {
                    // Create the compressed file.
                    using (FileStream outFile =
                                File.Create(fi.FullName + ".gz"))
                    {
                        using (GZipStream Compress =
                            new GZipStream(outFile,
                            CompressionMode.Compress))
                        {
                            // Copy the source file into 
                            // the compression stream.
                            inFile.CopyTo(Compress);

                            Console.WriteLine("Compressed {0} from {1} to {2} bytes.",
                                fi.Name, fi.Length.ToString(), outFile.Length.ToString());
                        }
                    }
                }
            }
        }

        private void Decompress(FileInfo fi)
        {
            // Get the stream of the source file.
            using (FileStream inFile = fi.OpenRead())
            {
                // Get original file extension, for example
                // "doc" from report.doc.gz.
                string curFile = fi.FullName;
                string origName = curFile.Remove(curFile.Length -
                        fi.Extension.Length);

                //Create the decompressed file.
                using (FileStream outFile = File.Create(origName))
                {
                    using (GZipStream Decompress = new GZipStream(inFile,
                            CompressionMode.Decompress))
                    {
                        // Copy the decompression stream 
                        // into the output file.
                        Decompress.CopyTo(outFile);

                        Console.WriteLine("Decompressed: {0}", fi.Name);

                    }
                }
            }
        }

        public void compressDirectory(string zipFileName)
        {
            using (Ionic.Zip.ZipFile zip = new ZipFile())
            {
                zip.AddDirectory(dirPath);
                zip.Save(zipFileName);
            }
        }
    }
}
