using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using trobadoo.com.web.Base;
using trobadoo.com.web.Models.Carousel;
using trobadoo.com.web.Entities.Enumerations;
using System.Runtime.Caching;

namespace trobadoo.com.web.Models.Products
{
    public class Cache<T>
    {
        private MemoryCache _memoryCache = MemoryCache.Default;
        private String _code;
        private String _description;
        private DateTimeOffset _expiration;
        private  List<T> _elements;

        public Cache(string code, string description, List<T> elements)
        {
            this._code = code;
            this._description = description;
            this._elements = elements;
            load();
        }

        public Cache(string code, string description,List<T> elements, DateTimeOffset expiration)
        {
            this._code = code;
            this._description = description;
            this._elements = elements;
            this._expiration = expiration;
            load();
        }

        public void load()
        {
            if (!_memoryCache.Contains(_code))
            {
                if (_expiration == DateTimeOffset.MinValue)
                {
                    _expiration = DateTimeOffset.UtcNow.AddDays(1);
                }
                _memoryCache.Add(_code, _elements, _expiration);
            }
        }

        public void reload()
        {
            _memoryCache.Remove(_code);
        }

        public List<T> get()
        {
            return _elements;
        }

        public string Code
        {
            get
            {
                return _code;
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }
        }
    }
}