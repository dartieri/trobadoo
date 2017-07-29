using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using trobadoo.com.web.Base;
using trobadoo.com.web.Models.Carousel;
using trobadoo.com.web.Entities.Enumerations;

namespace trobadoo.com.web.Models.Concept
{
    public class ConceptModel:BaseModel
    {
        public ConceptModel(string languageCode)
            : base(SectionName.Concepto, "concept",languageCode, true)
        {
            BaseCarouselModel = new HomeCarouselModel();
        }

        public override void AddCss() { }
        public override void AddJs() { }
        public override void AddDocumentReady() { }
    }
}