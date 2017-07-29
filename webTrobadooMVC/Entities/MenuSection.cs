using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using trobadoo.com.web.Entities.Enumerations;

namespace trobadoo.com.web.Entities
{
    public class MenuSection
    {
        private string _id;
        private string _title;
        private string _url;
        private string _cssClass;
        private string _iconClass;
        private string _linkClass;
        private bool _showIcon;

        public MenuSection(SectionName sectionName,string title, string url, string linkClass, bool isActive = false, bool showIcon = false)
        {
            _id = sectionName.ToString();
            _title = title;
            _url = url;
            _linkClass = linkClass;
            _cssClass = isActive ? "active":string.Empty;
            _showIcon = showIcon;

            SetIconClass();
        }

        private void SetIconClass()
        {
            switch (_id.ToLower())
            {
                case "concepto":
                    _iconClass = "fa-lightbulb-o";
                    break;
                case "servicios":
                    _iconClass = "fa-wrench";
                    break;
                case "contacto":
                    _iconClass = "fa-bullhorn";
                    break;
                case "valoracion":
                    _iconClass = "fa-calculator";
                    break;
            }
        }

        public string Id
        {
            get
            {
                return _id;
            }
        }

        public string Title
        {
            get
            {
                return _title;
            }
        }
        
        public string Url
        {
            get
            {
                return _url;
            }
        }

        public string CssClass
        {
            get
            {
                return _cssClass;
            }
        }

        public string IconClass
        {
            get
            {
                return _iconClass;
            }
        }

        public string LinkClass
        {
            get
            {
                return _linkClass;
            }
        }

        public bool ShowIcon
        {
            get
            {
                return _showIcon;
            }
        }
    }
}