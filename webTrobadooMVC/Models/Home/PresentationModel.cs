using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using trobadoo.com.web.Base;
using trobadoo.com.web.Models.Carousel;
using trobadoo.com.web.Entities.Enumerations;

namespace trobadoo.com.web.Models.Home
{
    public class PresentationModel : SimpleModel
    {
        public PresentationModel( string languageCode)
            : base("home",languageCode)
        {
        }
    }
}