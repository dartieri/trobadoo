using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Renci.SshNet;

namespace com.trobadoo.utils.Helpers
{
    public class FtpHelper
    {
        private string ftpUrl;
        private string ftpUser;
        private string ftpPassword;

        public FtpHelper(string url, string user, string password)
        {
            this.ftpUrl = url;
            this.ftpUser = user;
            this.ftpPassword = password;
        }

        public void fileUpload(string ftpUploadPath, string fileName)
        {
            using (WebClient client = new WebClient())
            {
                client.Credentials = new NetworkCredential(ftpUser, ftpPassword);
                client.UploadFile(ftpUrl + ftpUploadPath + fileName, "STOR", fileName);
            }
        }
    }
}
