using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace trobadoo.com.web.Models.Carousel
{
    public class HomeCarouselModel : BaseCarouselModel
    {
        
        public HomeCarouselModel(int numPhotos = 6) : base(numPhotos) {
        }
    }
}