using System;
using System.Collections.Generic;

namespace Twinvision.Flow
{
    /// <summary>
    /// This class represents an empty element node without any content. this can be used to create an empty root element to provide multiple root elements in the output
    /// </summary>
    /// <remarks></remarks>
    public class HTMLEmpty : IHTMLElement
    {
        public string Content
        {
            get => "";
            set
            {
            }
        }

        public string Open(bool enforceProperCase = true)
        {
            return "";
        }

        public string Close(bool enforceProperCase = true)
        {
            return "";
        }

        public bool IsMultiLine => false;

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

        public void Tag(string value, bool enforceProperCase = true)
        {
            throw new NotImplementedException();
        }

        public ContentPosition ContentPosition { get; set; } = ContentPosition.BeforeElements;
    }
}
