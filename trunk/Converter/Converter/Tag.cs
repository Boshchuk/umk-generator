using System;
using System.Collections.Generic;
using System.Text;

namespace Converter
{
    class Tag
    {
        private List<Tag> Childs;
        private Dictionary<string, string> Params;
        private bool hasPair;
        private string Text, Name;

        public Tag(string Name)
        {
            Childs = new List<Tag>();
            Params = new Dictionary<string, string>();
            this.hasPair = true;
            this.Name = Name;
        }

        public Tag(string Name, bool hasPair)
        {
            Childs = new List<Tag>();
            Params = new Dictionary<string, string>();
            this.hasPair = hasPair;
            this.Name = Name;
        }

        public void AddParam(string key, string value)
        {
            Params.Add(key, value);
        }

        public void AddChild(Tag child)
        {
            Childs.Add(child);
        }

        public void AddText(string text)
        {
            this.Text = text;
        }

        public string ConvertToHTML()
        {
            string result = "";

            result += "<" + Name;
            foreach (string key in Params.Keys)
            {
                result += " " + key + "=" + @"""" + Params[key] + @"""" + " ";
            }

            if (hasPair)
            {
                result += ">";
            }
            else
            {
                result += "/>";
                return result;
            }

            foreach (Tag item in Childs)
            {
                result += item.ConvertToHTML();
            }

            result += Text + "</" + Name + ">";            

            return result;
        }
    }
}
