using Microsoft.VisualBasic;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Microsoft.VisualBasic.CompilerServices;

namespace Twinvision.Flow
{


    /// <summary>

    /// Interface used for all HTML elements. The internal tree uses this interface for its nodes

    /// </summary>

    /// <remarks></remarks>
    public interface IHTMLElement
    {
        string Open(bool enforceProperCase = false);
        string Close { get; }
        string Empty(bool enforceProperCase = false);
        List<HTMLAttribute> Attributes { get; set; }
        string Content { get; set; }
        bool IsMultiLine { get; }
        ContentPosition ContentPosition { get; set; }
        string Tag(bool enforceProperCase = false);
        string ToString();
        string ToString(bool enforceProperCase);
    }



    public enum ContentPosition : int
    {
        BeforeElements = 0,
        AfterAlements = 1
    }

    public enum HTMLDocumentType : int
    {
        XHTML_1_1 = 0,
        HTML4_01_Frameset = 1,
        HTML4_01_Strict = 2,
        HTML4_01_Transitional = 3,
        HTML5 = 4,
        Undefined = 255
    }

    public enum FormMethod : int
    {
        Get = 1,
        Post = 2
    }

    public enum FormEncodingType : int
    {
        UrlEncoded = 1,
        FormData = 2,
        Plain = 3
    }

    public enum Target : int
    {
        Blank = 1,
        Self = 2,
        Parent = 3,
        Top = 4
    }



    public class FlowJsonSerializer : JsonSerializer
    {
        public FlowJsonSerializer()
        {
            ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
            Formatting = Formatting.Indented;
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        }
    }

    /// <summary>

    /// This class represents a tree of IHTMLElement

    /// </summary>

    /// <remarks>Used together with the Interface IHTMLElement to construct an HTML document tree</remarks>
    public class HTMLElementNode
    {
        public IHTMLElement Element { get; set; }
        public HTMLElementNode Parent { get; set; }
        private List<HTMLElementNode> _childNodes;

        public HTMLElementNode(HTMLElementNode parent, IHTMLElement nodeData)
        {
            Element = nodeData;
            if (parent == null)
                Parent = this;
            else
                Parent = parent;
            _childNodes = new List<HTMLElementNode>();
        }

        public HTMLElementNode[] Children => _childNodes.ToArray();

        public HTMLElementNode this[long index]
        {
            get
            {
                return _childNodes[Conversions.ToInteger(index)];
            }
        }

        protected internal HTMLElementNode AddChild(IHTMLElement nodeData)
        {
            var newNode = new HTMLElementNode(this, nodeData);
            _childNodes.Add(newNode);
            return newNode;
        }

        protected internal HTMLElementNode InsertChild(int index, IHTMLElement nodeData)
        {
            var newNode = new HTMLElementNode(this, nodeData);
            _childNodes.Insert(index, newNode);
            return newNode;
        }

        protected internal void RemoveChild(HTMLElementNode nodeData)
        {
            _childNodes.Remove(nodeData);
        }

        public override string ToString()
        {
            return Element.ToString();
        }

        public string ToString(bool enforceProperCase)
        {
            return Element.ToString(enforceProperCase);
        }
    }



    public class HTMLTag
    {
        public string Name { get; set; }
        public bool[] DocType { get; set; }
    }

    /// <summary>

    /// This class represents an HTML attribute.

    /// These are the key value pairs you can find inside an opening tag of an HTML element

    /// </summary>

    /// <remarks></remarks>
    public class HTMLAttribute
    {
        private string _name;
        private string _value;

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }

        HTMLAttribute(string name, string value)
        {
            Name = name;
            Value = value;
        }

        HTMLAttribute(string name)
        {
            Name = name;
            Value = null;
        }

        public override string ToString()
        {
            if (Value == null)
                return Name;
            else
                return Name + "=\"" + Value + "\"";
        }

        public new string ToString(bool enforceProperCase)
        {
            if (enforceProperCase)
            {
                if (Value == null)
                    return Name.ToLowerInvariant();
                else
                    return Name.ToLowerInvariant() + "=\"" + Value + "\"";
            }
            else
                return ToString();
        }
    }

    /// <summary>

    /// This class represents an HTML comment. It supports single and multi line comments

    /// </summary>

    /// <remarks></remarks>
    public class HTMLComment : IHTMLElement
    {
        private string _content = "";
        private bool _isMultiLine;

        public string Content
        {
            get
            {
                return _content;
            }
            set
            {
                _content = value;
                _isMultiLine = _content.Contains(Constants.vbCrLf);
            }
        }

        public string Open(bool enforceProperCase = true)
        {
            if (IsMultiLine)
                return "<!--";
            else
                return "<!-- ";
        }

        public string Close
        {
            get
            {
                if (IsMultiLine)
                    return "-->";
                else
                    return " -->";
            }
        }

        public bool IsMultiLine
        {
            get
            {
                return _isMultiLine;
            }
        }

        public HTMLComment(string content)
        {
            Content = content;
        }

        public string ToString(bool enforceProperCase = true)
        {
            if (IsMultiLine)
                return Open(enforceProperCase) + Constants.vbCrLf + Content + Constants.vbCrLf + Close;
            else
                return Open(enforceProperCase) + " " + Content + " " + Close;
        }

        public List<HTMLAttribute> Attributes { get; set; }

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

    /// <summary>

    /// This class represents an empty element node without any content. this can be used to create an empty root element to provide multiple root elements in the output

    /// </summary>

    /// <remarks></remarks>
    public class HTMLEmpty : IHTMLElement
    {
        public string Content
        {
            get
            {
                return "";
            }
            set
            {
            }
        }

        public string Open
        {
            get
            {
                return "";
            }
        }

        public string get_Open(bool enforceProperCase)
        {
            return "";
        }

        public string Close
        {
            get
            {
                return "";
            }
        }

        public bool IsMultiLine
        {
            get
            {
                return false;
            }
        }

        public override string ToString()
        {
            if (IsMultiLine)
                return Open + Constants.vbCrLf + Content + Constants.vbCrLf + Close;
            else
                return Open + " " + Content + " " + Close;
        }

        public new string ToString(bool enforceProperCase)
        {
            return ToString();
        }

        public List<HTMLAttribute> Attributes { get; set; }

        public string Empty
        {
            get
            {
                return "";
            }
        }

        public string get_Empty(bool enforceProperCase)
        {
            return "";
        }

        public string Tag
        {
            get
            {
                return "";
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string get_Tag(bool enforceProperCase)
        {
            return "";
        }

        public void set_Tag(bool enforceProperCase, string value)
        {
            throw new NotImplementedException();
        }

        public ContentPosition ContentPosition { get; set; } = ContentPosition.BeforeElements;
    }

    /// <summary>

    /// This class represents the html element. It will automatically prefix it with an (html 5) doctype element

    /// </summary>

    /// <remarks>This element can only be added to the root. Otherwise an exception will be thrown.</remarks>
    public class HTMLDocument : HTMLElement
    {
        private HTMLDocumentType DocumentType { get; set; } = HTMLDocumentType.HTML5;

        public HTMLDocument() : base("html")
        {
        }

        HTMLDocument(HTMLDocumentType documentType) : this()
        {
            DocumentType = documentType;
        }

        public override string Open
        {
            get
            {
                switch (DocumentType)
                {
                    case HTMLDocumentType.HTML4_01_Frameset:
                        {
                            return "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Frameset//EN\" \"http://www.w3.org/TR/html4/frameset.dtd\">" + Constants.vbCrLf + base.Open;
                        }

                    case HTMLDocumentType.HTML4_01_Strict:
                        {
                            return "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01//EN\" \"http://www.w3.org/TR/html4/strict.dtd\">" + Constants.vbCrLf + base.Open;
                        }

                    case HTMLDocumentType.HTML4_01_Transitional:
                        {
                            return "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\" \"http://www.w3.org/TR/html4/loose.dtd\">" + Constants.vbCrLf + base.Open;
                        }

                    case HTMLDocumentType.HTML5:
                        {
                            return "<!DOCTYPE html>" + Constants.vbCrLf + base.Open;
                        }

                    case HTMLDocumentType.XHTML_1_1:
                        {
                            return "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.1//EN\" \"http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd\">" + Constants.vbCrLf + base.Open;
                        }

                    default:
                        {
                            return base.Open;
                        }
                }
            }
        }

        public override string get_Open(bool enforceProperCase)
        {
            return Open;
        }
    }

    /// <summary>

    /// This class represents an html element. Most elements inside a typical HTML document will be of this type.

    /// </summary>

    /// <remarks></remarks>
    public class HTMLElement : IHTMLElement
    {
        private string _tag = "";
        private string _content = "";
        private bool _isMultiLine;

        public virtual string Content
        {
            get
            {
                return _content;
            }
            set
            {
                _content = value;
                _isMultiLine = _content.Contains(Constants.vbCrLf);
            }
        }

        public string Tag
        {
            get
            {
                return _tag;
            }
            set
            {
                if (Regex.IsMatch(value, "^[A-Za-z0-9]*$"))
                    _tag = value;
                else
                    throw new Exception(string.Format("Tag <{0}> contains invalid characters", value));
            }
        }

        public string get_Tag(bool enforceProperCase)
        {
            if (enforceProperCase)
                return _tag.ToLowerInvariant();
            else
                return _tag;
        }

        public void set_Tag(bool enforceProperCase, string value)
        {
            Tag = value;
        }

        public List<HTMLAttribute> Attributes { get; set; } = new List<HTMLAttribute>();

        public HTMLElement(string tag)
        {
            Tag = tag;
        }

        public HTMLElement(string tag, string content)
        {
            Tag = tag;
            Content = content;
        }

        public HTMLElement(string tag, IEnumerable<HTMLAttribute> attributes)
        {
            Tag = tag;
            if (attributes != null)
                Attributes.AddRange(attributes);
        }

        public HTMLElement(string tag, IEnumerable<HTMLAttribute> attributes, string content)
        {
            Tag = tag;
            Content = content;
            if (attributes != null)
                Attributes.AddRange(attributes);
        }

        public HTMLElement(string tag, string content, IEnumerable<HTMLAttribute> attributes)
        {
            Tag = tag;
            Content = content;
            if (attributes != null)
                Attributes.AddRange(attributes);
        }

        HTMLElement(string tag, string content, IEnumerable<HTMLAttribute> attributes, ContentPosition contentPosition)
        {
            Tag = tag;
            Content = content;
            if (attributes != null)
                Attributes.AddRange(attributes);
            ContentPosition = contentPosition;
        }

        public virtual string Open
        {
            get
            {
                if (Attributes.Count == 0)
                    return "<" + Tag + ">";
                else
                {
                    string result = "";
                    result = "<" + Tag;
                    foreach (var attribute in Attributes)
                        result += " " + attribute.ToString();
                    result += ">";
                    return result;
                }
            }
        }

        public virtual string get_Open(bool enforceProperCase)
        {
            if (Attributes.Count == 0)
                return "<" + Tag + ">";
            else
            {
                string result = "";
                result = "<" + Tag;
                foreach (var attribute in Attributes)
                    result += " " + attribute.ToString(enforceProperCase);
                result += ">";
                return result;
            }
        }

        public virtual string Close
        {
            get
            {
                return "</" + Tag + ">";
            }
        }

        public string Empty
        {
            get
            {
                if (Attributes.Count == 0)
                    return "<" + Tag + "/>";
                else
                {
                    string result = "<" + Tag;
                    foreach (var attribute in Attributes)
                        result += " " + attribute.ToString();
                    result += " />";
                    return result;
                }
            }
        }

        public string get_Empty(bool enforceProperCase)
        {
            if (Attributes.Count == 0)
                return "<" + Tag + "/>";
            else
            {
                string result = "<" + Tag;
                foreach (var attribute in Attributes)
                    result += " " + attribute.ToString(enforceProperCase);
                result += " />";
                return result;
            }
        }

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Content) && HTMLTags.SelfClosing.Contains(Tag.ToLowerInvariant()))
                return Empty;
            else
                return Open + Content + Close;
        }

        public new string ToString(bool enforceProperCase)
        {
            if (string.IsNullOrWhiteSpace(Content) && HTMLTags.SelfClosing.Contains(Tag.ToLowerInvariant()))
                return get_Empty(enforceProperCase);
            else
                return get_Open(enforceProperCase) + Content + Close;
        }


        public bool IsMultiLine
        {
            get
            {
                return _isMultiLine;
            }
        }

        public ContentPosition ContentPosition { get; set; } = ContentPosition.BeforeElements;
    }



    /// <summary>

    /// This class is instantiated as a shared DefaultSettings property on the HTMLBuilder class.

    /// Defaults are provided but you are free to change them at any time.

    /// </summary>

    /// <remarks>

    /// NOTE: These settings will be applied across all instances of the HTMLBuilder class, but

    /// you can change it by providing your own settings on or after HTMLBuilder object creation

    /// 

    /// You do not have to supply these settings before adding elements to the HTMLBuilder.

    /// All settings will be applied on the fly when using any of the output functions (i.e. ToString or Write).

    /// </remarks>
    public class HTMLBuilderSettings
    {
        public int TabSize { get; set; } = 4;
        public bool EnforceProperCase { get; set; } = true;
        public bool EnforceDocType { get; set; } = true;
        public bool EnforceProperNesting { get; set; } = true;
        public bool IndentHeaderAndBodyTags { get; set; } = true;
        public bool WriteComments { get; set; } = true;
    }

    /// <summary>

    /// This class alows you to add elements using many helper functions (i.e. Document Body, Header and Div)

    /// and create some hopefully well-formed HTML output

    /// </summary>

    /// <remarks></remarks>
    public class HTMLBuilder
    {
        private HTMLElementNode tree;
        private HTMLElementNode lastNode;
        private Stack<Tuple<string, HTMLElementNode>> componentStack;

        private bool _addChildNode = false;
        private bool _addNextNode = true;

        public HTMLBuilderSettings Settings = new HTMLBuilderSettings();
        public static HTMLBuilderSettings DefaultSettings { get; set; } = new HTMLBuilderSettings();

        public HTMLDocumentType DocumentType { get; set; }

        public HTMLElementNode DOM
        {
            get
            {
                if (tree == null)
                    throw new Exception("Add an element first before accessing the DOM.");
                return tree;
            }
        }



        HTMLBuilder()
        {
            Settings = DefaultSettings;
            DocumentType = HTMLDocumentType.HTML5;
        }

        HTMLBuilder(HTMLBuilderSettings settings)
        {
            Settings = settings;
            DocumentType = HTMLDocumentType.HTML5;
        }

        HTMLBuilder(HTMLDocumentType documentType)
        {
            DocumentType = documentType;
        }

        HTMLBuilder(HTMLDocumentType documentType, HTMLBuilderSettings settings)
        {
            DocumentType = documentType;
            Settings = settings;
        }



        private void WriteTree(StringBuilder sb, HTMLElementNode root, int nestedLevel = 0)
        {
            if (!(root == null))
            {
                string content = "";
                var elementType = root.Element.GetType();

                if (elementType == typeof(HTMLEmpty) || !Settings.WriteComments & elementType == typeof(HTMLComment))
                {
                    foreach (HTMLElementNode child in root.Children)
                        WriteTree(sb, child, nestedLevel);
                }
                else if (root.Children.Length == 0 && string.IsNullOrEmpty(root.Element.Content) && HTMLTags.SelfClosing.Contains(root.Element.Tag.ToLowerInvariant()))
                    sb.AppendLine(Strings.Space(nestedLevel * Settings.TabSize) + root.Element.get_Empty(Settings.EnforceProperCase));
                else
                {
                    sb.Append(Strings.Space(nestedLevel * Settings.TabSize) + root.Element.get_Open(Settings.EnforceProperCase));
                    if (root.Children.Length > 0 | root.Element.IsMultiLine)
                        sb.AppendLine();
                    if (!string.IsNullOrEmpty(root.Element.Content))
                    {
                        if (root.Children.Length > 0 | root.Element.IsMultiLine)
                        {
                            using (var sr = new System.IO.StringReader(root.Element.Content))
                            {
                                while (sr.Peek() != -1)
                                    content += Strings.Space((nestedLevel + 1) * Settings.TabSize) + sr.ReadLine() + Environment.NewLine;
                            }
                        }
                        else
                            content += root.Element.Content;
                        if (root.Element.ContentPosition == (int)ContentPosition.BeforeElements)
                            sb.Append(content);
                    }
                    foreach (HTMLElementNode child in root.Children)
                    {
                        if (Settings.IndentHeaderAndBodyTags || !Settings.IndentHeaderAndBodyTags && !((root.Element.get_Tag(true) ?? "") == "html"))
                            nestedLevel += 1;
                        WriteTree(sb, child, nestedLevel);
                        if (Settings.IndentHeaderAndBodyTags || !Settings.IndentHeaderAndBodyTags && !((root.Element.get_Tag(true) ?? "") == "html"))
                            nestedLevel -= 1;
                    }
                    if ((int)root.Element.ContentPosition == (int)ContentPosition.AfterAlements && !string.IsNullOrEmpty(content))
                        sb.Append(content);
                    if (root.Children.Length == 0 && !root.Element.IsMultiLine)
                        sb.AppendLine(root.Element.Close);
                    else
                        sb.AppendLine(Strings.Space(nestedLevel * Settings.TabSize) + root.Element.Close);
                }
            }
        }



        public HTMLBuilder Parent()
        {
            lastNode = lastNode.Parent;
            return this;
        }

        public HTMLBuilder Child()
        {
            if (lastNode != null && lastNode.Element is HTMLComment)
                throw new Exception("HTML comments cannot have child elements");
            AddChildNode = true;
            return this;
        }

        private bool AddChildNode
        {
            get
            {
                if (_addChildNode)
                {
                    _addChildNode = false;
                    return true;
                }
                else
                    return false;
            }
            set
            {
                _addChildNode = value;
            }
        }

        private bool AddNextNode
        {
            get
            {
                if (!_addNextNode)
                {
                    _addNextNode = true;
                    return false;
                }
                else
                    return true;
            }
            set
            {
                _addNextNode = value;
            }
        }



        public HTMLBuilder AddElement(IHTMLElement element)
        {
            return AddElement(-1, element.Tag, element.Attributes.ToArray(), element.Content, AddChildNode, ContentPosition.BeforeElements);
        }

        public HTMLBuilder AddElement(string tag)
        {
            return AddElement(tag, null, "");
        }

        public HTMLBuilder AddElement(string tag, string content)
        {
            return AddElement(tag, null, content);
        }

        public HTMLBuilder AddElement(string tag, IEnumerable<HTMLAttribute> attributes, string content, ContentPosition contentPosition)
        {
            return AddElement(-1, tag, attributes, content, AddChildNode, contentPosition);
        }

        public HTMLBuilder AddElement(string tag, IEnumerable<HTMLAttribute> attributes)
        {
            return AddElement(-1, tag, attributes, "", AddChildNode, ContentPosition.BeforeElements);
        }

        public HTMLBuilder AddElement(string tag, IEnumerable<HTMLAttribute> attributes, string content)
        {
            return AddElement(-1, tag, attributes, content, AddChildNode, ContentPosition.BeforeElements);
        }

        public HTMLBuilder InsertElement(int index, IHTMLElement element)
        {
            return AddElement(index, element.Tag, element.Attributes.ToArray(), element.Content, AddChildNode, ContentPosition.BeforeElements);
        }

        public HTMLBuilder InsertElement(int index, string tag)
        {
            return AddElement(index, tag, null, "", AddChildNode, ContentPosition.BeforeElements);
        }

        public HTMLBuilder InsertElement(int index, string tag, string content)
        {
            return AddElement(index, tag, null, content, AddChildNode, ContentPosition.BeforeElements);
        }

        public HTMLBuilder InsertElement(int index, string tag, IEnumerable<HTMLAttribute> attributes, string content)
        {
            return AddElement(index, tag, attributes, content, AddChildNode, ContentPosition.BeforeElements);
        }

        public HTMLBuilder InsertElement(int index, string tag, IEnumerable<HTMLAttribute> attributes, string content, ContentPosition contentPosition)
        {
            return AddElement(index, tag, attributes, content, AddChildNode, contentPosition);
        }

        private HTMLBuilder AddElement(int index, string tag, IEnumerable<HTMLAttribute> attributes, string content, bool asChild, ContentPosition contentPosition)
        {
            string tagLowerCase = tag.ToLowerInvariant();
            HTMLElementNode node;

            // Do not add this node if this property is false
            if (!AddNextNode)
                return this;

            // Check doc type
            if (Settings.EnforceDocType && !((int)DocumentType == (int)HTMLDocumentType.Undefined))
            {
                if (HTMLTags.Support.ContainsKey(tagLowerCase))
                {
                    if (!HTMLTags.Support[tagLowerCase][(int)DocumentType])
                        throw new Exception(string.Format("Tag <{0}> not supported for document type {1}", tag, DocumentType.ToString()));
                }
                else
                    throw new Exception(string.Format("<{0}> is not a valid tag for document type {1}", tag, DocumentType.ToString()));
            }

            // Check nesting if enabled
            if (tree != null && Settings.EnforceProperNesting)
            {
                if (HTMLTags.Nesting.ContainsKey(tagLowerCase))
                {
                    string foundNestingItem;
                    if (asChild)
                        node = lastNode;
                    else
                        node = lastNode.Parent;
                    foundNestingItem = HTMLTags.Nesting[tagLowerCase].FirstOrDefault(where => where.StartsWith(node.Element.Tag.ToLower()));
                    if (foundNestingItem != null)
                    {
                        var nestingItemCount = foundNestingItem.Split(':');
                        if ((nestingItemCount[1] ?? "") == "1" && node.Descendants().Count(where => (where.Element.Tag.ToLower() ?? "") == (tagLowerCase ?? "")) > 0)
                            throw new Exception(string.Format("Tag <{0}> cannot be nested multiple times inside tag <{1}>", tag, node.Element.Tag));
                    }
                    else
                        throw new Exception(string.Format("Tag <{0}> cannot be nested inside tag <{1}>", tag, node.Element.Tag));
                }
            }

            var element = new HTMLElement(tag, content, attributes, contentPosition);
            if (tree == null)
            {
                tree = new HTMLElementNode(null, element);
                lastNode = tree;
                AddChildNode = true;
            }
            else if (asChild)
            {
                if (index < 0)
                    lastNode = lastNode.AddChild(element);
                else
                    lastNode = lastNode.InsertChild(index, element);
            }
            else if (index < 0)
                lastNode = lastNode.Parent.AddChild(element);
            else
                lastNode = lastNode.Parent.InsertChild(index, element);
            return this;
        }

        private void AddElementsFromElementEnumerable(IEnumerable<HTMLElementNode> elements)
        {
            foreach (var element in elements)
            {
                AddElement(element.Element.Tag, element.Element.Attributes, element.Element.Content);
                if (element.Children.Length > 0)
                {
                    AddChildNode = true;
                    AddElementsFromElementEnumerable(element.Children);
                }
            }
        }

        public HTMLBuilder AddElementsFrom(HTMLBuilder builder)
        {
            if (builder.tree != null)
            {
                var storeLastNode = lastNode;
                AddElementsFromElementEnumerable(new[] { builder.tree });
                lastNode = storeLastNode;
            }
            return this;
        }

        public HTMLBuilder AddElementsFrom(HTMLElementNode[] elements)
        {
            AddElementsFromElementEnumerable(elements);
            return this;
        }

        public HTMLBuilder SetActiveElement(HTMLElementNode element)
        {
            HTMLElementNode node;
            node = DOM.DescendantsAndSelf().FirstOrDefault(where => where == element);
            if (node != null)
                lastNode = node;
            else
                throw new Exception("Element not found");
            return this;
        }

        public HTMLBuilder DeleteElement(HTMLElementNode element)
        {
            HTMLElementNode node;
            node = DOM.DescendantsAndSelf().FirstOrDefault(where => where == element);

            if (node != null)
            {
                if (node.Parent == node)
                    throw new Exception("Cannot remove root element");
                else
                {
                    lastNode = node.Parent;
                    lastNode.RemoveChild(node);
                }
            }
            else
                throw new Exception("Element not found");
            return this;
        }

        public HTMLBuilder AddAttribute(string name)
        {
            if (!AddNextNode)
                return this;
            lastNode.Element.Attributes.Add(new HTMLAttribute(name));
            return this;
        }

        public HTMLBuilder AddAttribute(string name, string value)
        {
            if (!AddNextNode)
                return this;
            lastNode.Element.Attributes.Add(new HTMLAttribute(name, value));
            return this;
        }

        /// <summary>
        /// A call to this function does not impact Child() and Parent() functions, it affects only
        /// the next element added in the builder
        /// </summary>
        /// <param name="expression"></param>
        /// <returns>HTMLBuilder</returns>
        /// <remarks></remarks>
        public HTMLBuilder OnlyWhen(Func<bool> expression)
        {
            AddNextNode = expression.Invoke();
            return this;
        }



        public HTMLBuilder Div(string className = "", string id = "", string content = "", HTMLAttribute[] additionalAttributes = null)
        {
            var attributes = new List<HTMLAttribute>();
            if (!string.IsNullOrWhiteSpace(className))
                attributes.Add(new HTMLAttribute("class", className));
            if (!string.IsNullOrWhiteSpace(id))
                attributes.Add(new HTMLAttribute("id", id));
            if (additionalAttributes != null)
                attributes.AddRange(additionalAttributes);
            return AddElement("div", attributes.ToArray(), content);
        }

        public HTMLBuilder Body(string className = "", string content = "", HTMLAttribute[] additionalAttributes = null)
        {
            var attributes = new List<HTMLAttribute>();

            if (!string.IsNullOrWhiteSpace(className))
                attributes.Add(new HTMLAttribute("class", className));
            if (additionalAttributes != null)
                attributes.AddRange(additionalAttributes);
            AddElement("body", attributes.ToArray(), content);
            AddChildNode = true;
            return this;
        }

        public HTMLBuilder A(string href = "", string content = "", string className = "", HTMLAttribute[] additionalAttributes = null)
        {
            var attributes = new List<HTMLAttribute>();
            if (!string.IsNullOrWhiteSpace(className))
                attributes.Add(new HTMLAttribute("class", className));
            if (!string.IsNullOrWhiteSpace(href))
                attributes.Add(new HTMLAttribute("href", href));
            if (additionalAttributes != null)
                attributes.AddRange(additionalAttributes);
            AddElement("a", attributes.ToArray(), content);
            return this;
        }

        public HTMLBuilder P(string content = "", string className = "", HTMLAttribute[] additionalAttributes = null)
        {
            var attributes = new List<HTMLAttribute>();
            if (!string.IsNullOrWhiteSpace(className))
                attributes.Add(new HTMLAttribute("class", className));
            if (additionalAttributes != null)
                attributes.AddRange(additionalAttributes);
            AddElement("p", attributes.ToArray(), content);
            return this;
        }

        public HTMLBuilder BR(HTMLAttribute[] additionalAttributes = null)
        {
            var attributes = new List<HTMLAttribute>();
            if (additionalAttributes != null)
                attributes.AddRange(additionalAttributes);
            AddElement("br", attributes.ToArray(), "");
            return this;
        }

        public HTMLBuilder H(int level, string content = "", string className = "", HTMLAttribute[] additionalAttributes = null)
        {
            var attributes = new List<HTMLAttribute>();
            if (!string.IsNullOrWhiteSpace(className))
                attributes.Add(new HTMLAttribute("class", className));
            if (additionalAttributes != null)
                attributes.AddRange(additionalAttributes);
            AddElement("h" + level.ToString(), attributes.ToArray(), content);
            return this;
        }

        public HTMLBuilder Header(HTMLAttribute[] additionalAttributes = null)
        {
            return Header("", "", "", additionalAttributes);
        }

        public HTMLBuilder Header(string title, HTMLAttribute[] additionalAttributes = null)
        {
            return Header(title, "", "", additionalAttributes);
        }

        public HTMLBuilder Header(string title, string description, HTMLAttribute[] additionalAttributes = null)
        {
            return Header(title, description, "", additionalAttributes);
        }

        public HTMLBuilder Header(string title, string description, string keywords, HTMLAttribute[] additionalAttributes = null)
        {
            var keepNode = lastNode;

            try
            {
                InsertElement(0, "head", additionalAttributes, "");
                if (!string.IsNullOrWhiteSpace(title))
                    AddElement(-1, "title", null, title, true, ContentPosition.BeforeElements);
                if (!string.IsNullOrWhiteSpace(description))
                    AddElement(-1, "meta", new HTMLAttribute[] { new HTMLAttribute("name", "description"), new HTMLAttribute("content", description) }, "", false, ContentPosition.BeforeElements);
                if (!string.IsNullOrWhiteSpace(keywords))
                    AddElement(-1, "meta", new HTMLAttribute[] { new HTMLAttribute("name", "keywords"), new HTMLAttribute("content", keywords) }, "", false, ContentPosition.BeforeElements);
            }
            finally
            {
                if (keepNode != null)
                    lastNode = keepNode;
            }
            return this;
        }

        public HTMLBuilder Meta(string name, string content, HTMLAttribute[] additionalAttributes = null)
        {
            var keepNode = lastNode;
            try
            {
                if (!string.IsNullOrWhiteSpace(name))
                {
                    if (additionalAttributes == null)
                        additionalAttributes = new[] { };
                }
                AddElement("meta", new[] { new HTMLAttribute("name", name), new HTMLAttribute("content", content) }.Concat(additionalAttributes).ToArray(), "");
            }
            finally
            {
                if (keepNode != null)
                    lastNode = keepNode;
            }
            return this;
        }

        public HTMLBuilder Link(string rel, string href, HTMLAttribute[] additionalAttributes = null)
        {
            var keepNode = lastNode;
            try
            {
                if (!string.IsNullOrWhiteSpace(rel))
                {
                    if (additionalAttributes == null)
                        additionalAttributes = new[] { };
                    AddElement("link", new[] { new HTMLAttribute("rel", rel), new HTMLAttribute("href", href) }.Concat(additionalAttributes).ToArray(), "");
                }
            }
            finally
            {
                if (keepNode != null)
                    lastNode = keepNode;
            }
            return this;
        }

        public HTMLBuilder Document(string language = "")
        {
            if (!AddNextNode)
                return this;

            var element = new HTMLDocument(DocumentType);
            if (!string.IsNullOrWhiteSpace(language))
                element.Attributes.Add(new HTMLAttribute("lang", language));
            if (tree == null)
            {
                tree = new HTMLElementNode(null, element);
                lastNode = tree;
                AddChildNode = true;
            }
            else
                throw new Exception("The <html> tag must be the first element in an HTML document");
            return this;
        }

        public HTMLBuilder Comment(string content)
        {
            if (!AddNextNode)
                return this;

            var element = new HTMLComment(content);
            if (tree == null)
            {
                tree = new HTMLElementNode(null, element);
                lastNode = tree;
                AddChildNode = true;
            }
            else if (AddChildNode)
                lastNode = lastNode.AddChild(element);
            else
                lastNode = lastNode.Parent.AddChild(element);
            return this;
        }

        public HTMLBuilder Form(string name, string action, FormMethod method = FormMethod.Post, FormEncodingType encodingType = FormEncodingType.UrlEncoded, bool autoComplete = true, bool novalidate = false, HTMLAttribute[] additionalAttributes = null)
        {
            var attributes = new List<HTMLAttribute>();
            if (!string.IsNullOrWhiteSpace(name))
                attributes.Add(new HTMLAttribute("name", name));
            if (!string.IsNullOrWhiteSpace(action))
                attributes.Add(new HTMLAttribute("action", action));
            switch (method)
            {
                case FormMethod.Get:
                    {
                        attributes.Add(new HTMLAttribute("method", "get"));
                        break;
                    }

                case FormMethod.Post:
                    {
                        attributes.Add(new HTMLAttribute("method", "post"));
                        break;
                    }
            }
            switch (encodingType)
            {
                case FormEncodingType.UrlEncoded:
                    {
                        attributes.Add(new HTMLAttribute("enctype", "application/x-www-form-urlencoded"));
                        break;
                    }

                case FormEncodingType.FormData:
                    {
                        attributes.Add(new HTMLAttribute("enctype", "multipart/form-data"));
                        break;
                    }

                case FormEncodingType.Plain:
                    {
                        attributes.Add(new HTMLAttribute("enctype", "text/plain	"));
                        break;
                    }
            }
            if (!autoComplete)
                attributes.Add(new HTMLAttribute("autocomplete", "off"));
            if (novalidate)
                attributes.Add(new HTMLAttribute("novalidate"));
            if (additionalAttributes != null)
                attributes.AddRange(additionalAttributes);
            AddElement("form", attributes.ToArray());
            return this;
        }

        public HTMLBuilder Empty()
        {
            if (!AddNextNode)
                return this;

            var element = new HTMLEmpty();
            if (tree == null)
            {
                tree = new HTMLElementNode(null, element);
                lastNode = tree;
                AddChildNode = true;
            }
            else if (AddChildNode)
                lastNode = lastNode.AddChild(element);
            else
                lastNode = lastNode.Parent.AddChild(element);
            return this;
        }



        public HTMLBuilder BeginComponent(string componentName)
        {
            if (componentStack == null)
                componentStack = new Stack<Tuple<string, HTMLElementNode>>();
            if (tree == null)
                Empty();

            if (string.IsNullOrWhiteSpace(componentName))
                componentName = "Component";
            Comment(string.Format("Begin {0}", componentName));
            componentStack.Push(new Tuple<string, HTMLElementNode>(componentName, lastNode));
            return this;
        }

        public HTMLBuilder BeginComponent()
        {
            return BeginComponent("");
        }

        public HTMLBuilder EndComponent()
        {
            if (componentStack.Count > 0)
            {
                var component = componentStack.Pop();
                string componentName = component.Item1;
                lastNode = component.Item2;
                AddChildNode = false;
                Comment(string.Format("End {0}", componentName));
                return this;
            }
            else
                throw new Exception("There is no component started with BeginComponent left to end");
        }



        public override string ToString()
        {
            var sb = new StringBuilder();
            WriteTree(sb, tree);
            return sb.ToString();
        }

        public string ToHTMLEncodedString()
        {
            return System.Net.WebUtility.HtmlEncode(ToString());
        }

        public string ToJSON()
        {
            using (var sw = new System.IO.StringWriter())
            {
                var js = new FlowJsonSerializer();
                js.Serialize(sw, tree);
                return sw.ToString();
            }
        }

        public void Write(StringBuilder sb)
        {
            WriteTree(sb, tree);
        }

        public void Write(string fileName, bool append, Encoding encoding)
        {
            var sb = new StringBuilder();
            WriteTree(sb, tree);
            using (var sw = new System.IO.StreamWriter(fileName, append, encoding))
            {
                sw.Write(sb.ToString());
            }
        }

        public void Write(string fileName, bool append = false)
        {
            Write(fileName, append, new UTF8Encoding());
        }
    }
}
