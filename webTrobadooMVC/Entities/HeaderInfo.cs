using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace trobadoo.com.web.Entities
{
    public class HeaderInfo
    {
        private string _title;
        private string _description;
        private string _keywords;

        public HeaderInfo(string title, string description, string keywords)
        {
            _title = title;
            _description = description;
            _keywords = keywords;
        }

        public string Title
        {
            get
            {
                return _title;
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }
        }

        public string Keywords
        {
            get
            {
                return _keywords;
            }
        }
    }
}