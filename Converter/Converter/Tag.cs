using System.Collections.Generic;

namespace Converter
{
    internal class Tag
    {
        private readonly List<Tag> _childs;
        private readonly Dictionary<string, string> _params;
        private readonly bool _hasPair;
        private string _innerText;
        private readonly string _name;

        public Tag(string name)
        {
            _childs = new List<Tag>();
            _params = new Dictionary<string, string>();
            this._hasPair = true;
            this._name = name;
        }

        public Tag(string name, bool hasPair)
        {
            _childs = new List<Tag>();
            _params = new Dictionary<string, string>();
            this._hasPair = hasPair;
            this._name = name;
        }

        public void AddParam(string key, string value)
        {
            _params.Add(key, value);
        }

        public void AddChild(Tag child)
        {
            _childs.Add(child);
        }

        public void AddInnerText(string innerText)
        {
            this._innerText = innerText;
        }

        public string ConvertToHtml()
        {
            var result = $"<{_name}";

            foreach (var key in _params.Keys)
            {
                result += $" {key}=\"{_params[key]}\" ";
            }

            if (!_hasPair)
            {
                result += "/>";
                return result;
            }


            result += ">";

            foreach (var item in _childs)
            {
                result += item.ConvertToHtml();
            }

            result += $"{_innerText}</{_name}>";

            return result;
        }
    }
}
