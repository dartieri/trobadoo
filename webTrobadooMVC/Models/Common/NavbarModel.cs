using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using trobadoo.com.web.Base;
using trobadoo.com.web.Entities;
using trobadoo.com.web.Entities.Enumerations;

namespace trobadoo.com.web.Models.Common
{
    public class NavbarModel : SimpleModel
    {
        private List<MenuSection> _sections = new List<MenuSection>();

        public NavbarModel(SectionName activeSection, string languageCode)
            : base("navbar", languageCode)
        {

        }

        private void MountSections(SectionName activeSection = SectionName.Home)
        {
            _sections.Add(new MenuSection(SectionName.Concepto, GetTranslation("1"), GetTranslation("2"), string.Empty, activeSection == SectionName.Concepto, false));
            _sections.Add(new MenuSection(SectionName.Servicios, GetTranslation("3"), GetTranslation("4"), string.Empty, activeSection == SectionName.Servicios, false));
            _sections.Add(new MenuSection(SectionName.Contacto, GetTranslation("5"), GetTranslation("6"), string.Empty, activeSection == SectionName.Contacto, false));
            _sections.Add(new MenuSection(SectionName.Valoration, GetTranslation("7"), GetTranslation("8"), "btnValoracion", activeSection == SectionName.Valoration, false));

        }

        public void AddSection(MenuSection menuSection)
        {
            _sections.Add(menuSection);
        }

        public List<MenuSection> Sections
        {
            get
            {
                if (_sections.Count == 0)
                {
                    MountSections();
                }
                return _sections;
            }
        }
    }
}