using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace trobadoo.com.web.Entities
{
    public class Photo
    {
        public string name;
        public string physicalPath;
        public long size;
        public string url;
        public string thumbnailUrl;
        public string deleteUrl;
        public string deleteType = "DELETE";
        public string error;
    }
}