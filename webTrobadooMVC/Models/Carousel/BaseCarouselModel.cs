using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace trobadoo.com.web.Models.Carousel
{
    public class BaseCarouselModel
    {
        protected const int TOTALPHOTOS = 29;
        private const string BASEPATH = "/Content/images/banners/home";
        private Random random = new Random();
        private List<string> _photosUrl = new List<string>();

        public BaseCarouselModel(int numPhotos)
        {
            var photosIndex = GenerateRandom(numPhotos);
            foreach (var index in photosIndex) {
                _photosUrl.Add(GetImageUrl(index));
            }
        }

        public List<string> PhotosUrl
        {
            get
            {
                return _photosUrl;
            }
        }

        private string GetImageUrl(int index)
        {
                 return string.Format("{0}/{1}.jpg",BASEPATH,index);
        }

        private List<int> GenerateRandom(int countPhotos)
        {
            // generate count random values.
            HashSet<int> candidates = new HashSet<int>();
            while (candidates.Count < countPhotos)
            {
                // May strike a duplicate.
                candidates.Add(random.Next(1,TOTALPHOTOS));
            }

            // load them in to a list.
            List<int> result = new List<int>();
            result.AddRange(candidates);

            // shuffle the results:
            int i = result.Count;
            while (i > 1)
            {
                i--;
                int k = random.Next(i + 1);
                int value = result[k];
                result[k] = result[i];
                result[i] = value;
            }
            return result;
        }

        /*
        protected List<int> Seeds()
        {
            Random rand = new Random();
            List<int> result = new List<int>();
            HashSet<int> check = new HashSet<int>();
            for (Int32 i = 0; i < 300; i++)
            {
                int curValue = rand.Next(1, TOTALPHOTOS);
                while (check.Contains(curValue))
                {
                    curValue = rand.Next(1, TOTALPHOTOS);
                }
                result.Add(curValue);
                check.Add(curValue);
            }
            return result;
        }
         */
    }
}