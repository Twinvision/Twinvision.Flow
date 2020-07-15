using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Twinvision.Flow
{
    /// <summary>
    /// This class represents an html element. Most elements inside a typical HTML document will be of this type.
    /// </summary>
    /// <remarks></remarks>
    public class HTMLElement : IHTMLElement
    {
        private string _tag = "";
        private string _content = "";

        public virtual string Content
        {
            get => _content;
            set
            {
                _content = value ?? "";
                IsMultiLine = _content.Contains(System.Environment.NewLine);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "Tags are in lowercase when properly cased")]
        public string Tag(bool enforceProperCase = true)
        {
            if (enforceProperCase)
            {
                return _tag.ToLowerInvariant();
            }
            else
            {
                return _tag;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "Tags are in lowercase when properly cased")]
        public void Tag(string value, bool enforceProperCase = true)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            if (Regex.IsMatch(value, "^[A-Za-z0-9]*$"))
            {
                if (enforceProperCase)
                {
                    _tag = value.ToLowerInvariant();
                }
                else
                {
                    _tag = value;
                }
            }
            else
            {
                throw new Exception($"Tag <{value}> contains invalid characters");
            }
        }

        public List<HTMLAttribute> Attributes { get; } = new List<HTMLAttribute>();

        public HTMLElement(string tag)
        {
            Tag(tag, false);
        }

        public HTMLElement(string tag, string content)
        {
            Tag(tag, false);
            Content = content;
        }

        public HTMLElement(string tag, IEnumerable<HTMLAttribute> attributes)
        {
            Tag(tag, false);
            if (attributes != null)
            {
                Attributes.AddRange(attributes);
            }
        }

        public HTMLElement(string tag, IEnumerable<HTMLAttribute> attributes, string content)
        {
            Tag(tag, false);
            Content = content;
            if (attributes != null)
            {
                Attributes.AddRange(attributes);
            }
        }

        public HTMLElement(string tag, string content, IEnumerable<HTMLAttribute> attributes)
        {
            Tag(tag, false);
            Content = content;
            if (attributes != null)
            {
                Attributes.AddRange(attributes);
            }
        }

        public HTMLElement(string tag, string content, IEnumerable<HTMLAttribute> attributes, ContentPosition contentPosition)
        {
            Tag(tag, false);
            Content = content;
            if (attributes != null)
            {
                Attributes.AddRange(attributes);
            }

            ContentPosition = contentPosition;
        }

        public virtual string Open(bool enforceProperCase)
        {
            if (Attributes.Count == 0)
            {
                return "<" + Tag(enforceProperCase) + ">";
            }
            else
            {
                string result = "";
                result = "<" + Tag(enforceProperCase);
                foreach (var attribute in Attributes)
                {
                    result += " " + attribute.ToString(enforceProperCase);
                }

                result += ">";
                return result;
            }
        }

        public virtual string Close(bool enforceProperCase = true)
        {
            return "</" + Tag(enforceProperCase) + ">";
        }

        public string Empty(bool enforceProperCase = true)
        {
            if (Attributes.Count == 0)
            {
                return "<" + Tag(enforceProperCase) + "/>";
            }
            else
            {
                string result = "<" + Tag(enforceProperCase);
                foreach (var attribute in Attributes)
                {
                    result += " " + attribute.ToString(enforceProperCase);
                }

                result += " />";
                return result;
            }
        }

        public string ToString(bool enforceProperCase = true)
        {
            if (string.IsNullOrWhiteSpace(Content) && HTMLTags.SelfClosing.Contains(Tag(enforceProperCase).ToLowerInvariant()))
            {
                return Empty(enforceProperCase);
            }
            else
            {
                return Open(enforceProperCase) + Content + Close(enforceProperCase);
            }
        }


        public bool IsMultiLine { get; private set; }

        public ContentPosition ContentPosition { get; set; } = ContentPosition.BeforeElements;
    }
}
