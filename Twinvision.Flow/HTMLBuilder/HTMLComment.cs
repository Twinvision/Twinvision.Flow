using System.Collections.Generic;

namespace Twinvision.Flow
{
    /// <summary>
    /// This class represents an HTML comment. It supports single and multi line comments
    /// </summary>
    /// <remarks></remarks>
    public class HTMLComment : IHTMLElement
    {
        private string _content = "";

        public string Content
        {
            get => _content;
            set
            {
                _content = value ?? "";
                IsMultiLine = _content.Contains(System.Environment.NewLine);
            }
        }

        public string Open(bool enforceProperCase = true)
        {
            if (IsMultiLine)
            {
                return "<!--";
            }
            else
            {
                return "<!-- ";
            }
        }

        public string Close(bool enforceProperCase = true)
        {
            if (IsMultiLine)
            {
                return "-->";
            }
            else
            {
                return " -->";
            }
        }

        public bool IsMultiLine { get; private set; }

        public HTMLComment(string content)
        {
            Content = content;
        }

        public string ToString(bool enforceProperCase = true)
        {
            if (IsMultiLine)
            {
                return Open(enforceProperCase) + System.Environment.NewLine + Content + System.Environment.NewLine + Close(enforceProperCase);
            }
            else
            {
                return Open(enforceProperCase) + " " + Content + " " + Close(enforceProperCase);
            }
        }

        public List<HTMLAttribute> Attributes { get; }

        public string Empty(bool enforceProperCase = true)
        {
            return "";
        }

        public string Tag(bool enforceProperCase = true)
        {
            return "";
        }

        public ContentPosition ContentPosition { get; set; } = ContentPosition.BeforeElements;
    }
}
