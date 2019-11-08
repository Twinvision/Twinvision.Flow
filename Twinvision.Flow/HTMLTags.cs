using System.Collections.Generic;

namespace Twinvision.Flow
{
    internal static class HTMLTags
    {
        public static SortedDictionary<string, bool[]> Support = new SortedDictionary<string, bool[]>()
        {
            {
                "a",
                new bool[] { true, true, true, true, true }
            },
            {
                "abbr",
                new bool[] { true, true, true, true, true }
            },
            {
                "acronym",
                new bool[] { true, true, true, true, false }
            },
            {
                "address",
                new bool[] { true, true, true, true, true }
            },
            {
                "applet",
                new bool[] { false, true, false, true, false }
            },
            {
                "area",
                new bool[] { false, true, true, true, true }
            },
            {
                "article",
                new bool[] { false, false, false, false, true }
            },
            {
                "aside",
                new bool[] { false, false, false, false, true }
            },
            {
                "audio",
                new bool[] { false, false, false, false, true }
            },
            {
                "b",
                new bool[] { true, true, true, true, true }
            },
            {
                "base",
                new bool[] { true, true, true, true, true }
            },
            {
                "basefont",
                new bool[] { false, true, false, true, false }
            },
            {
                "bdi",
                new bool[] { false, false, false, false, true }
            },
            {
                "bdo",
                new bool[] { false, true, true, true, true }
            },
            {
                "big",
                new bool[] { true, true, true, true, false }
            },
            {
                "blockquote",
                new bool[] { true, true, true, true, true }
            },
            {
                "body",
                new bool[] { true, true, true, true, true }
            },
            {
                "br",
                new bool[] { true, true, true, true, true }
            },
            {
                "button",
                new bool[] { true, true, true, true, true }
            },
            {
                "canvas",
                new bool[] { false, false, false, false, true }
            },
            {
                "caption",
                new bool[] { true, true, true, true, true }
            },
            {
                "center",
                new bool[] { false, true, false, true, false }
            },
            {
                "cite",
                new bool[] { true, true, true, true, true }
            },
            {
                "code",
                new bool[] { true, true, true, true, true }
            },
            {
                "col",
                new bool[] { false, true, true, true, true }
            },
            {
                "colgroup",
                new bool[] { false, true, true, true, true }
            },
            {
                "command",
                new bool[] { false, false, false, false, true }
            },
            {
                "datalist",
                new bool[] { false, false, false, false, true }
            },
            {
                "dd",
                new bool[] { true, true, true, true, true }
            },
            {
                "del",
                new bool[] { false, true, true, true, true }
            },
            {
                "details",
                new bool[] { false, false, false, false, true }
            },
            {
                "dfn",
                new bool[] { true, true, true, true, true }
            },
            {
                "dir",
                new bool[] { false, true, false, true, false }
            },
            {
                "div",
                new bool[] { true, true, true, true, true }
            },
            {
                "dl",
                new bool[] { true, true, true, true, true }
            },
            {
                "dt",
                new bool[] { true, true, true, true, true }
            },
            {
                "em",
                new bool[] { true, true, true, true, true }
            },
            {
                "embed",
                new bool[] { false, false, false, false, true }
            },
            {
                "fieldset",
                new bool[] { true, true, true, true, true }
            },
            {
                "figcaption",
                new bool[] { false, false, false, false, true }
            },
            {
                "figure",
                new bool[] { false, false, false, false, true }
            },
            {
                "font",
                new bool[] { false, true, false, true, false }
            },
            {
                "footer",
                new bool[] { false, false, false, false, true }
            },
            {
                "form",
                new bool[] { true, true, true, true, true }
            },
            {
                "frame",
                new bool[] { false, true, false, false, false }
            },
            {
                "frameset",
                new bool[] { false, true, false, false, false }
            },
            {
                "h1",
                new bool[] { true, true, true, true, true }
            },
            {
                "h2",
                new bool[] { true, true, true, true, true }
            },
            {
                "h3",
                new bool[] { true, true, true, true, true }
            },
            {
                "h4",
                new bool[] { true, true, true, true, true }
            },
            {
                "h5",
                new bool[] { true, true, true, true, true }
            },
            {
                "h6",
                new bool[] { true, true, true, true, true }
            },
            {
                "head",
                new bool[] { true, true, true, true, true }
            },
            {
                "header",
                new bool[] { false, false, false, false, true }
            },
            {
                "hgroup",
                new bool[] { false, false, false, false, true }
            },
            {
                "hr",
                new bool[] { true, true, true, true, true }
            },
            {
                "html",
                new bool[] { true, true, true, true, true }
            },
            {
                "i",
                new bool[] { true, true, true, true, true }
            },
            {
                "iframe",
                new bool[] { false, true, false, true, true }
            },
            {
                "img",
                new bool[] { true, true, true, true, true }
            },
            {
                "input",
                new bool[] { true, true, true, true, true }
            },
            {
                "ins",
                new bool[] { false, true, true, true, true }
            },
            {
                "kbd",
                new bool[] { true, true, true, true, true }
            },
            {
                "keygen",
                new bool[] { false, false, false, false, true }
            },
            {
                "label",
                new bool[] { true, true, true, true, true }
            },
            {
                "legend",
                new bool[] { true, true, true, true, true }
            },
            {
                "li",
                new bool[] { true, true, true, true, true }
            },
            {
                "link",
                new bool[] { true, true, true, true, true }
            },
            {
                "map",
                new bool[] { false, true, true, true, true }
            },
            {
                "mark",
                new bool[] { false, false, false, false, true }
            },
            {
                "menu",
                new bool[] { false, true, false, true, true }
            },
            {
                "meta",
                new bool[] { true, true, true, true, true }
            },
            {
                "meter",
                new bool[] { false, false, false, false, true }
            },
            {
                "nav",
                new bool[] { false, false, false, false, true }
            },
            {
                "noframes",
                new bool[] { false, true, false, true, false }
            },
            {
                "noscript",
                new bool[] { true, true, true, true, true }
            },
            {
                "object",
                new bool[] { true, true, true, true, true }
            },
            {
                "ol",
                new bool[] { true, true, true, true, true }
            },
            {
                "optgroup",
                new bool[] { true, true, true, true, true }
            },
            {
                "option",
                new bool[] { true, true, true, true, true }
            },
            {
                "output",
                new bool[] { false, false, false, false, true }
            },
            {
                "p",
                new bool[] { true, true, true, true, true }
            },
            {
                "param",
                new bool[] { true, true, true, true, true }
            },
            {
                "pre",
                new bool[] { true, true, true, true, true }
            },
            {
                "progress",
                new bool[] { false, false, false, false, true }
            },
            {
                "q",
                new bool[] { true, true, true, true, true }
            },
            {
                "rp",
                new bool[] { false, false, false, false, true }
            },
            {
                "rt",
                new bool[] { false, false, false, false, true }
            },
            {
                "ruby",
                new bool[] { false, false, false, false, true }
            },
            {
                "s",
                new bool[] { false, true, false, true, true }
            },
            {
                "samp",
                new bool[] { true, true, true, true, true }
            },
            {
                "script",
                new bool[] { true, true, true, true, true }
            },
            {
                "section",
                new bool[] { false, false, false, false, true }
            },
            {
                "select",
                new bool[] { true, true, true, true, true }
            },
            {
                "small",
                new bool[] { true, true, true, true, true }
            },
            {
                "source",
                new bool[] { false, false, false, false, true }
            },
            {
                "span",
                new bool[] { true, true, true, true, true }
            },
            {
                "strike",
                new bool[] { false, true, false, true, false }
            },
            {
                "strong",
                new bool[] { true, true, true, true, true }
            },
            {
                "style",
                new bool[] { true, true, true, true, true }
            },
            {
                "sub",
                new bool[] { true, true, true, true, true }
            },
            {
                "summary",
                new bool[] { false, false, false, false, true }
            },
            {
                "sup",
                new bool[] { true, true, true, true, true }
            },
            {
                "table",
                new bool[] { true, true, true, true, true }
            },
            {
                "tbody",
                new bool[] { false, true, true, true, true }
            },
            {
                "td",
                new bool[] { true, true, true, true, true }
            },
            {
                "textarea",
                new bool[] { true, true, true, true, true }
            },
            {
                "tfoot",
                new bool[] { false, true, true, true, true }
            },
            {
                "th",
                new bool[] { true, true, true, true, true }
            },
            {
                "thead",
                new bool[] { false, true, true, true, true }
            },
            {
                "time",
                new bool[] { false, false, false, false, true }
            },
            {
                "title",
                new bool[] { true, true, true, true, true }
            },
            {
                "tr",
                new bool[] { true, true, true, true, true }
            },
            {
                "track",
                new bool[] { false, false, false, false, true }
            },
            {
                "tt",
                new bool[] { true, true, true, true, false }
            },
            {
                "u",
                new bool[] { false, true, false, true, false }
            },
            {
                "ul",
                new bool[] { true, true, true, true, true }
            },
            {
                "var",
                new bool[] { true, true, true, true, true }
            },
            {
                "video",
                new bool[] { false, false, false, false, true }
            },
            {
                "wbr",
                new bool[] { false, false, false, false, true }
            }
        };

        // Check for nesting. Also checks if an element is allowed just once or multiple times; tag:1 or tag:*
        public static SortedDictionary<string, string[]> Nesting = new SortedDictionary<string, string[]>()
        {
            {
                "li",
                new string [] {
                    "ul:*",
                    "ol:*",
                    "dir:*"
                }
            },
            {
                "body",
              new string []   {
                    "html:1"
                }
            },
            {
                "head",
          new string []       {
                    "html:1"
                }
            },
            {
                "title",
           new string []      {
                    "head:1"
                }
            },
            {
                "link",
         new string []        {
                    "head:*"
                }
            },
            {
                "meta",
      new string []           {
                    "head:*"
                }
            },
            {
                "tr",
        new string []         {
                    "table:*"
                }
            },
            {
                "td",
     new string []            {
                    "tr:*"
                }
            },
            {
                "th",
    new string []             {
                    "tr:*"
                }
            },
            {
                "caption",
      new string []           {
                    "table:1"
                }
            },
            {
                "tfoot",
     new string []            {
                    "table:1"
                }
            },
            {
                "tbody",
 new string []                {
                    "table:1"
                }
            },
            {
                "thead",
  new string []               {
                    "table:1"
                }
            },
            {
                "colgroup",
    new string []             {
                    "table:*"
                }
            },
            {
                "col",
  new string []               {
                    "colgroup:*"
                }
            },
            {
                "dt",
     new string []            {
                    "dl:*"
                }
            },
            {
                "dd",
     new string []            {
                    "dt:*"
                }
            },
            {
                "source",
    new string []             {
                    "audio:*",
                    "video:*"
                }
            },
            {
                "track",
 new string []                {
                    "audio:*",
                    "video:*"
                }
            },
            {
                "frame",
    new string []             {
                    "frameset:*"
                }
            },
            {
                "noframes",
      new string []           {
                    "frameset:1"
                }
            },
            {
                "option",
       new string []          {
                    "select:*",
                    "datalist:*",
                    "optgroup:*"
                }
            },
            {
                "optgroup",
     new string []            {
                    "select:*"
                }
            },
            {
                "summary",
    new string []             {
                    "details:1"
                }
            },
            {
                "legend",
   new string []              {
                    "fieldset:1"
                }
            },
            {
                "figcaption",
    new string []             {
                    "figure:1"
                }
            },
            {
                "area",
   new string []              {
                    "map:*"
                }
            }
        };

        public static List<string> SelfClosing = new List<string>()
        {
            "area",
            "base",
            "br",
            "col",
            "command",
            "embed",
            "hr",
            "img",
            "input",
            "keygen",
            "link",
            "meta",
            "param",
            "source",
            "track",
            "wbr"
        };
    }
}
