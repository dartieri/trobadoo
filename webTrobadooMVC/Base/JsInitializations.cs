using System.Collections.Generic;
using System.Text;

namespace trobadoo.com.web.Base
{
    public class JsInitializations
    {
        private readonly List<JsVar> _variables;
        private readonly List<string> _documentReadyActions;
        private readonly List<string> _importedCode;

        public JsInitializations()
        {
            _variables = new List<JsVar>();
            _documentReadyActions = new List<string>();
            _importedCode = new List<string>();
        }

        public void AddVar(string name, string value, bool needQuotes = false, bool globalVar = false)
        {
            _variables.Add(new JsVar(name, value, needQuotes, globalVar));
        }

        public void AddDocumentReadyAction(string action)
        {
            _documentReadyActions.Add(action);
        }

        public void AddImportedCode(string code)
        {
            _importedCode.Add(code);
        }

        public string Print()
        {
            var sb = new StringBuilder();

            foreach (var code in _importedCode)
            {
                sb.AppendLine(code);
            }

            foreach (var jsvar in _variables)
            {
                sb.AppendLine(jsvar.Print());
            }

            if (_documentReadyActions.Count > 0)
            {
                sb.AppendLine("$( document ).ready(function() {");
                foreach (var action in _documentReadyActions)
                {
                    sb.AppendLine(action);
                }
                sb.AppendLine("});");
            }

            return sb.ToString();
        }
    }
}