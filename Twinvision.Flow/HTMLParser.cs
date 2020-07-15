using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twinvision.Flow
{
    public enum ParseState : int
    {
        Valid,
        ElementClosedWithNoTagName,
        ElementWhiteSpaceInTagName,
        ElementInvalidCharaterInTagName,
        ElementNotOpened,
        ElementNotClosed,
        AttributeNotClosed,
        NonSelfClosingElementClosed
    }
    public enum ParseMode : int
    {
        WhiteSpace,
        DocumentHeader,
        Element,
        Attribute,
        Comment
    }

    public class ParseException : Exception
    {
        public ParseException(string message, int position, ParseMode mode, ParseState state) : base(message)
        {           
            Position = position;
            Mode = mode;
            State = state;
        }

        public int Position { get; set; }
        public ParseState State { get; set; }
        public ParseMode Mode { get; set; }
    }

    public class ParseWarning
    {
        public ParseWarning(string message, int position, ParseMode mode, ParseState state)
        {
            Message = message;
            Position = position;
            Mode = mode;
            State = state;
        }

        public string Message { get; private set; }
        public int Position { get; private set; }
        public ParseMode Mode { get; private set; }
        public ParseState State { get; private set; }
    }

    class ParseElement
    {
        string Tag { get; set; }
        string Content { get; set; }
    }

    class ParseAttribute
    {
        string Name { get; set; }
        string Value { get; set; }
    }

    public static class HTMLParser
    {

        private enum ParseCharacterType : int
        {
            WhiteSpace,
            ElementOpen,
            ElementName,
            ElementValue,
            ElementClosed,
            AttributeOpen,
            AttributeName,
            AttributeValue,
            AttributeClosed,
            CommentOpen,
            CommentClosed
        }

        private static string Peek(string html, int position, int length = 1)
        {
            if (position < html.Length)
            {
                if (position + length < html.Length)
                {
                    return html.Substring(position, length);
                }
                else
                {
                    return html.Substring(position);
                }
            }
            else
            {
                return "";
            }
        }

        private static string PeekUpperInvariant(string html, int position, int length = 1)
        {
            return Peek(html, position, length).ToUpperInvariant();
        }

        public static HTMLBuilder Parse(this HTMLBuilder builder, string html)
        {
            int position = 0;
            int length = html.Length;
            char c;
            ParseCharacterType ct = ParseCharacterType.WhiteSpace;
            ParseMode pm = ParseMode.WhiteSpace;
            ParseState ps = ParseState.Valid;

            IHTMLElement element = null;

            do
            {
                c = html[position];
                switch (pm)
                {
                    case ParseMode.WhiteSpace:
                        {
                            switch (c)
                            {
                                case '<':
                                    ct = ParseCharacterType.ElementOpen;
                                    pm = ParseMode.Element;
                                    element = new HTMLElement("");
                                    break;
                                case '>':
                                case '"':
                                case '\'':
                                    ps = ParseState.ElementNotOpened;
                                    break;
                                default:
                                    if (!Char.IsWhiteSpace(c))
                                    {
                                        ps = ParseState.ElementNotOpened;
                                    }
                                    else
                                    {
                                        ct = ParseCharacterType.WhiteSpace;
                                    }
                                    break;
                            }
                        }
                        break;
                    case ParseMode.Element:
                        if (ps == ParseState.Valid)
                        {
                            switch (c)
                            {
                                case '>':
                                    ct = ParseCharacterType.ElementClosed;
                                    pm = ParseMode.WhiteSpace;
                                    if (element.Tag() == "")
                                    {
                                        ps = ParseState.ElementClosedWithNoTagName;
                                    }
                                    break;
                                case '!':
                                    if (PeekUpperInvariant(html, position + 1, 7) == "DOCTYPE")
                                    {
                                        pm = ParseMode.DocumentHeader;
                                        position = +7;
                                        element = new HTMLDocument(HTMLDocumentType.HTML5);
                                    }
                                    else if (Peek(html, position, 2) == "--")
                                    {
                                        pm = ParseMode.Comment;
                                        element = new HTMLComment("");
                                    }
                                    break;
                                case '"':
                                case '\'':
                                case '/':
                                case '\\':
                                    ps = ParseState.ElementInvalidCharaterInTagName;
                                    break;
                                default:
                                    if (!Char.IsWhiteSpace(c))
                                    {
                                        ps = ParseState.Valid;
                                    }
                                    else
                                    {
                                        ps = ParseState.ElementWhiteSpaceInTagName;
                                        ct = ParseCharacterType.WhiteSpace;
                                    }
                                    break;
                            }
                        }
                        else
                        {
                            switch (ps)
                            {
                                case ParseState.ElementClosedWithNoTagName:
                                    pm = ParseMode.WhiteSpace;
                                    break;
                                case ParseState.ElementInvalidCharaterInTagName:
                                    pm = ParseMode.WhiteSpace;
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;
                    case ParseMode.DocumentHeader:
                        if (ps == ParseState.Valid)
                        {
                            switch (c)
                            {
                                case '>':
                                    ct = ParseCharacterType.ElementClosed;
                                    pm = ParseMode.WhiteSpace;
                                    if (element.Tag() == "")
                                    {
                                        ps = ParseState.ElementClosedWithNoTagName;
                                    }
                                    break;
                                case 'h':
                                    if (PeekUpperInvariant(html, position, 4) == "html")
                                    {
                                        pm = ParseMode.DocumentHeader;
                                        position = 7;
                                        element = new HTMLDocument(HTMLDocumentType.HTML5);
                                    }
                                    else if (Peek(html, position, 2) == "--")
                                    {
                                        pm = ParseMode.Comment;
                                        element = new HTMLComment("");
                                    }
                                    break;
                                case '"':
                                case '\'':
                                case '/':
                                case '\\':
                                    ps = ParseState.ElementInvalidCharaterInTagName;
                                    break;
                                default:
                                    if (!Char.IsWhiteSpace(c))
                                    {
                                        ps = ParseState.Valid;
                                    }
                                    break;
                            }
                        }
                        else
                        {
                            switch (ps)
                            {
                                case ParseState.ElementClosedWithNoTagName:
                                    pm = ParseMode.WhiteSpace;
                                    break;
                                case ParseState.ElementInvalidCharaterInTagName:
                                    pm = ParseMode.WhiteSpace;
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;
                    default:
                        break;
                }
                position++;
            } while (position < length);
            return builder;
        }
    }
}
