using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using trobadoo.com.web.Base;
using trobadoo.com.web.Models.Carousel;
using trobadoo.com.web.Entities.Enumerations;

namespace trobadoo.com.web.Models.Products
{
    public class Product
    {

        private DateTime _depositCreationDate;
        private String _familyCode;
        private String _familyDescription;
        private String _code;
        private String _description;
        private DateTime _creationDate;
        private Decimal _initialPrice;
        private Decimal _sellPrice;
        private int _stock;
        private String _observations;

        public Product()
        {
        }

        public Product(DateTime depositCreationDate, String familyCode, String familyDescription, String code, String description, DateTime creationDate, Decimal initialPrice, Decimal sellPrice, int stock, String observations)
        {
            this._depositCreationDate = depositCreationDate;
            this._familyCode = familyCode;
            this._familyDescription = familyDescription;
            this._code = code;
            this._description = description;
            this._creationDate = creationDate;
            this._initialPrice = initialPrice;
            this._sellPrice = sellPrice;
            this._stock = stock;
            this._observations = observations;
        }

        public DateTime DepositCreationDate
        {
            get { return _depositCreationDate; }
            set { _depositCreationDate = value; }
        }

        public string FamilyCode
        {
            get { return _familyCode; }
            set { _familyCode = value; }
        }

        public string FamilyDescription
        {
            get { return _familyDescription; }
            set { _familyDescription = value; }
        }

        public string Code
        {
            get { return _code; }
            set { _code = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public DateTime CreationDate
        {
            get { return _creationDate; }
            set { _creationDate = value; }
        }

        public Decimal InitialPrice
        {
            get { return _initialPrice; }
            set { _initialPrice = value; }
        }

        public Decimal SellPrice
        {
            get { return _sellPrice; }
            set { _sellPrice = value; }
        }

        public int Stock
        {
            get { return _stock; }
            set { _stock = value; }
        }

        public String Observations
        {
            get { return _observations; }
            set { _observations = value; }
        }
    }
}