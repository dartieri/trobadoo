using System;
using System.Text;

namespace trobadoo.com.web.Base
{
    public class JsVar
    {
        public string Name;
        public string Value;
        public bool NeedQuotes;
        public bool GlobalVar;

        public JsVar(string name, string value, bool needQuotes = false, bool globalVar = false)
        {
            Name = name;
            Value = value;
            NeedQuotes = needQuotes;
            GlobalVar = globalVar;

        }

        public string Print()
        {
            var sb = new StringBuilder();
            if (GlobalVar)
            {
                sb.Append("var ");
            }
            sb.Append(Name);
            if (Value != null)
            {
                sb.Append("=");
                if (NeedQuotes)
                {
                    sb.AppendFormat("'{0}'", Value.Replace("'", "\\'"));
                }
                else
                {
                    sb.Append(Value);
                }
            }
            sb.Append(";");
            return sb.ToString();
        }
    }
}