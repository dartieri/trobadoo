using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using trobadoo.com.web.Base;
using trobadoo.com.web.Models.Carousel;
using trobadoo.com.web.Entities.Enumerations;
using trobadoo.com.web.Models.Products;

namespace trobadoo.com.web.Models.Products
{
    public static class CacheManager<T>
    {
        private static List<Cache<T>> _caches = new List<Cache<T>>();

        public static void RegisterCache(Cache<T> cache)
        {
            _caches.Add(cache);
        }

        public static List<Cache<T>> Caches
        {
            get
            {
                return _caches;
            }
        }

    }
}