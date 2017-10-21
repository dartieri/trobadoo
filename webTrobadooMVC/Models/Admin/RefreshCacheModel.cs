using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using trobadoo.com.web.Base;
using trobadoo.com.web.Models.Carousel;
using trobadoo.com.web.Entities.Enumerations;

namespace trobadoo.com.web.Models.Concept
{
    public class RefreshCacheModel:BaseModel
    {
        private List<string> _caches;

        public RefreshCacheModel()
            : base(SectionName.Admin, "refreshCache")
        {
            _caches = new List<String>();
        }

        public List<String> Caches
        {
            get
            {
                return _caches;
            }
        }

        public override void AddCss() { }
        public override void AddJs() { }
        public override void AddDocumentReady() { }
    }
}