using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Twinvision.Flow
{

    public enum ContentPosition : int
    {
        BeforeElements = 0,
        AfterAlements = 1
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "Makes it more readable")]
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

        public HTMLBuilderSettings Settings { get; set; } = new HTMLBuilderSettings();

        public HTMLDocumentType DocumentType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1065:Do not raise exceptions in unexpected locations", Justification = "Changing this property to a function would break existing code")]
        public HTMLElementNode DOM
        {
            get
            {
                if (tree == null)
                {
                    throw new Exception("Add an element first before accessing the DOM.");
                }

                return tree;
            }
        }

        public HTMLBuilder()
        {
            Settings = new HTMLBuilderSettings();
            DocumentType = HTMLDocumentType.HTML5;
        }

        public HTMLBuilder(HTMLBuilderSettings settings)
        {
            Settings = settings;
            DocumentType = HTMLDocumentType.HTML5;
        }

        public HTMLBuilder(HTMLDocumentType documentType)
        {
            DocumentType = documentType;
        }

        public HTMLBuilder(HTMLDocumentType documentType, HTMLBuilderSettings settings)
        {
            DocumentType = documentType;
            Settings = settings;
        }

        private void WriteTree(StringBuilder sb, HTMLElementNode root, int nestedLevel = 0)
        {
            if (root != null)
            {
                string content = "";
                var elementType = root.Element.GetType();

                if (elementType == typeof(HTMLEmpty) || !Settings.WriteComments & elementType == typeof(HTMLComment))
                {
                    foreach (HTMLElementNode child in root.Children)
                    {
                        WriteTree(sb, child, nestedLevel);
                    }
                }
                else if (root.Children.Count == 0 && string.IsNullOrEmpty(root.Element.Content) && HTMLTags.SelfClosing.Contains(root.Element.Tag().ToLowerInvariant()))
                {
                    sb.AppendLine(new string(' ', nestedLevel * Settings.TabSize) + root.Element.Empty(Settings.EnforceProperCase));
                }
                else
                {
                    sb.Append(new string(' ', nestedLevel * Settings.TabSize) + root.Element.Open(Settings.EnforceProperCase));
                    if (root.Children.Count > 0 | root.Element.IsMultiLine)
                    {
                        sb.AppendLine();
                    }

                    if (!string.IsNullOrEmpty(root.Element.Content))
                    {
                        if (root.Children.Count > 0 | root.Element.IsMultiLine)
                        {
                            using (var sr = new System.IO.StringReader(root.Element.Content))
                            {
                                while (sr.Peek() != -1)
                                {
                                    content += new string(' ', (nestedLevel + 1) * Settings.TabSize) + sr.ReadLine() + Environment.NewLine;
                                }
                            }
                        }
                        else
                        {
                            content += root.Element.Content;
                        }

                        if (root.Element.ContentPosition == (int)ContentPosition.BeforeElements)
                        {
                            sb.Append(content);
                        }
                    }
                    foreach (HTMLElementNode child in root.Children)
                    {
                        if (Settings.IndentHeaderAndBodyTags || !Settings.IndentHeaderAndBodyTags && !((root.Element.Tag(true) ?? "") == "html"))
                        {
                            nestedLevel += 1;
                        }

                        WriteTree(sb, child, nestedLevel);
                        if (Settings.IndentHeaderAndBodyTags || !Settings.IndentHeaderAndBodyTags && !((root.Element.Tag(true) ?? "") == "html"))
                        {
                            nestedLevel -= 1;
                        }
                    }
                    if ((int)root.Element.ContentPosition == (int)ContentPosition.AfterAlements && !string.IsNullOrEmpty(content))
                    {
                        sb.Append(content);
                    }

                    if (root.Children.Count == 0 && !root.Element.IsMultiLine)
                    {
                        sb.AppendLine(root.Element.Close(Settings.EnforceProperCase));
                    }
                    else
                    {
                        sb.AppendLine(new string(' ', nestedLevel * Settings.TabSize) + root.Element.Close(Settings.EnforceProperCase));
                    }
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
            {
                throw new Exception("HTML comments cannot have child elements");
            }

            AddChildNode = true;
            return this;
        }

        public HTMLBuilder Child(Action action)
        {
            Child();
            action.Invoke();
            Parent();
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
                {
                    return false;
                }
            }
            set => _addChildNode = value;
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
                {
                    return true;
                }
            }
            set => _addNextNode = value;
        }

        public HTMLBuilder AddElement(IHTMLElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }
            return AddElement(-1, element.Tag(), element.Attributes.ToArray(), element.Content, AddChildNode, ContentPosition.BeforeElements);
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
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }
            return AddElement(index, element.Tag(), element.Attributes.ToArray(), element.Content, AddChildNode, ContentPosition.BeforeElements);
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
            if (string.IsNullOrWhiteSpace(tag))
            {
                throw new ArgumentNullException(nameof(tag));
            }

            string tagLowerCase = tag.ToLowerInvariant();
            HTMLElementNode node;

            // Do not add this node if this property is false
            if (!AddNextNode)
            {
                return this;
            }

            // Check doc type
            if (Settings.EnforceDocType && !((int)DocumentType == (int)HTMLDocumentType.Undefined))
            {
                if (HTMLTags.Support.ContainsKey(tagLowerCase))
                {
                    if (!HTMLTags.Support[tagLowerCase][(int)DocumentType])
                    {
                        throw new Exception($"Tag <{tag}> not supported for document type {DocumentType.ToString()}");
                    }
                }
                else
                {
                    throw new Exception($"<{tag}> is not a valid tag for document type {DocumentType.ToString()}");
                }
            }

            // Check nesting if enabled
            if (tree != null && Settings.EnforceProperNesting)
            {
                if (HTMLTags.Nesting.ContainsKey(tagLowerCase))
                {
                    string foundNestingItem;
                    if (asChild)
                    {
                        node = lastNode;
                    }
                    else
                    {
                        node = lastNode.Parent;
                    }

                    foundNestingItem = HTMLTags.Nesting[tagLowerCase].FirstOrDefault(where => where.StartsWith(node.Element.Tag(), StringComparison.InvariantCultureIgnoreCase));
                    if (foundNestingItem != null)
                    {
                        var nestingItemCount = foundNestingItem.Split(':');
                        if ((nestingItemCount[1] ?? "") == "1" && node.Descendants().Any(where => (where.Element.Tag().ToLowerInvariant() ?? "") == (tagLowerCase ?? "")))
                        {
                            throw new Exception($"Tag <{tag}> cannot be nested multiple times inside tag <{node.Element.Tag()}>");
                        }
                    }
                    else
                    {
                        throw new Exception($"Tag <{tag}> cannot be nested inside tag <{node.Element.Tag()}>");
                    }
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
                {
                    lastNode = lastNode.AddChild(element);
                }
                else
                {
                    lastNode = lastNode.InsertChild(index, element);
                }
            }
            else if (index < 0)
            {
                lastNode = lastNode.Parent.AddChild(element);
            }
            else
            {
                lastNode = lastNode.Parent.InsertChild(index, element);
            }

            return this;
        }

        private void AddElementsFromElementEnumerable(IEnumerable<HTMLElementNode> elements)
        {
            foreach (var element in elements)
            {
                AddElement(element.Element.Tag(), element.Element.Attributes, element.Element.Content);
                if (element.Children.Count > 0)
                {
                    AddChildNode = true;
                    AddElementsFromElementEnumerable(element.Children);
                }
            }
        }

        public HTMLBuilder AddElementsFrom(HTMLBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
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
            if (elements == null)
            {
                throw new ArgumentNullException(nameof(elements));
            }
            AddElementsFromElementEnumerable(elements);
            return this;
        }

        public HTMLBuilder SetActiveElement(HTMLElementNode element)
        {
            HTMLElementNode node;
            node = DOM.DescendantsAndSelf().FirstOrDefault(where => where == element);
            if (node != null)
            {
                lastNode = node;
            }
            else
            {
                throw new Exception("Element not found");
            }

            return this;
        }

        public HTMLBuilder DeleteElement(HTMLElementNode element)
        {
            HTMLElementNode node;
            node = DOM.DescendantsAndSelf().FirstOrDefault(where => where == element);

            if (node != null)
            {
                if (node.Parent == node)
                {
                    throw new Exception("Cannot remove root element");
                }
                else
                {
                    lastNode = node.Parent;
                    lastNode.RemoveChild(node);
                }
            }
            else
            {
                throw new Exception("Element not found");
            }

            return this;
        }

        public HTMLBuilder AddAttribute(string name)
        {
            if (!AddNextNode)
            {
                return this;
            }

            lastNode.Element.Attributes.Add(new HTMLAttribute(name));
            return this;
        }

        public HTMLBuilder AddAttribute(string name, string value)
        {
            if (!AddNextNode)
            {
                return this;
            }

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
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }
            AddNextNode = expression.Invoke();
            return this;
        }

        public HTMLBuilder Div(string className = "", string id = "", string content = "", HTMLAttribute[] additionalAttributes = null)
        {
            var attributes = new List<HTMLAttribute>();
            if (!string.IsNullOrWhiteSpace(className))
            {
                attributes.Add(new HTMLAttribute("class", className));
            }

            if (!string.IsNullOrWhiteSpace(id))
            {
                attributes.Add(new HTMLAttribute("id", id));
            }

            if (additionalAttributes != null)
            {
                attributes.AddRange(additionalAttributes);
            }

            return AddElement("div", attributes.ToArray(), content);
        }

        public HTMLBuilder Body(string className = "", string content = "", HTMLAttribute[] additionalAttributes = null)
        {
            var attributes = new List<HTMLAttribute>();

            if (!string.IsNullOrWhiteSpace(className))
            {
                attributes.Add(new HTMLAttribute("class", className));
            }

            if (additionalAttributes != null)
            {
                attributes.AddRange(additionalAttributes);
            }

            AddElement("body", attributes.ToArray(), content);
            AddChildNode = true;
            return this;
        }

        public HTMLBuilder A(string href = "", string content = "", string className = "", HTMLAttribute[] additionalAttributes = null)
        {
            var attributes = new List<HTMLAttribute>();
            if (!string.IsNullOrWhiteSpace(className))
            {
                attributes.Add(new HTMLAttribute("class", className));
            }

            if (!string.IsNullOrWhiteSpace(href))
            {
                attributes.Add(new HTMLAttribute("href", href));
            }

            if (additionalAttributes != null)
            {
                attributes.AddRange(additionalAttributes);
            }

            AddElement("a", attributes.ToArray(), content);
            return this;
        }

        public HTMLBuilder P(string content = "", string className = "", HTMLAttribute[] additionalAttributes = null)
        {
            var attributes = new List<HTMLAttribute>();
            if (!string.IsNullOrWhiteSpace(className))
            {
                attributes.Add(new HTMLAttribute("class", className));
            }

            if (additionalAttributes != null)
            {
                attributes.AddRange(additionalAttributes);
            }

            AddElement("p", attributes.ToArray(), content);
            return this;
        }

        public HTMLBuilder BR(HTMLAttribute[] additionalAttributes = null)
        {
            var attributes = new List<HTMLAttribute>();
            if (additionalAttributes != null)
            {
                attributes.AddRange(additionalAttributes);
            }

            AddElement("br", attributes.ToArray(), "");
            return this;
        }

        public HTMLBuilder H(int level, string content = "", string className = "", HTMLAttribute[] additionalAttributes = null)
        {
            var attributes = new List<HTMLAttribute>();
            if (!string.IsNullOrWhiteSpace(className))
            {
                attributes.Add(new HTMLAttribute("class", className));
            }

            if (additionalAttributes != null)
            {
                attributes.AddRange(additionalAttributes);
            }

            AddElement($"h{level}", attributes.ToArray(), content);
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
                {
                    AddElement(-1, "title", null, title, true, ContentPosition.BeforeElements);
                }

                if (!string.IsNullOrWhiteSpace(description))
                {
                    AddElement(-1, "meta", new HTMLAttribute[] { new HTMLAttribute("name", "description"), new HTMLAttribute("content", description) }, "", false, ContentPosition.BeforeElements);
                }

                if (!string.IsNullOrWhiteSpace(keywords))
                {
                    AddElement(-1, "meta", new HTMLAttribute[] { new HTMLAttribute("name", "keywords"), new HTMLAttribute("content", keywords) }, "", false, ContentPosition.BeforeElements);
                }
            }
            finally
            {
                if (keepNode != null)
                {
                    lastNode = keepNode;
                }
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
                    {
                        additionalAttributes = Array.Empty<HTMLAttribute>();
                    }
                }
                AddElement("meta", new[] { new HTMLAttribute("name", name), new HTMLAttribute("content", content) }.Concat(additionalAttributes).ToArray(), "");
            }
            finally
            {
                if (keepNode != null)
                {
                    lastNode = keepNode;
                }
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
                    {
                        additionalAttributes = Array.Empty<HTMLAttribute>();
                    }

                    AddElement("link", new[] { new HTMLAttribute("rel", rel), new HTMLAttribute("href", href) }.Concat(additionalAttributes).ToArray(), "");
                }
            }
            finally
            {
                if (keepNode != null)
                {
                    lastNode = keepNode;
                }
            }
            return this;
        }

        public HTMLBuilder Document(string language = "")
        {
            if (!AddNextNode)
            {
                return this;
            }

            var element = new HTMLDocument(DocumentType);
            if (!string.IsNullOrWhiteSpace(language))
            {
                element.Attributes.Add(new HTMLAttribute("lang", language));
            }

            if (tree == null)
            {
                tree = new HTMLElementNode(null, element);
                lastNode = tree;
                AddChildNode = true;
            }
            else
            {
                throw new Exception("The <html> tag must be the first element in an HTML document");
            }

            return this;
        }

        public HTMLBuilder Comment(string content)
        {
            if (!AddNextNode)
            {
                return this;
            }

            var element = new HTMLComment(content);
            if (tree == null)
            {
                tree = new HTMLElementNode(null, element);
                lastNode = tree;
                AddChildNode = true;
            }
            else if (AddChildNode)
            {
                lastNode = lastNode.AddChild(element);
            }
            else
            {
                lastNode = lastNode.Parent.AddChild(element);
            }

            return this;
        }

        public HTMLBuilder Form(string name, string action, FormMethod method = FormMethod.Post, FormEncodingType encodingType = FormEncodingType.UrlEncoded, bool autoComplete = true, bool novalidate = false, HTMLAttribute[] additionalAttributes = null)
        {
            var attributes = new List<HTMLAttribute>();
            if (!string.IsNullOrWhiteSpace(name))
            {
                attributes.Add(new HTMLAttribute("name", name));
            }

            if (!string.IsNullOrWhiteSpace(action))
            {
                attributes.Add(new HTMLAttribute("action", action));
            }

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
            {
                attributes.Add(new HTMLAttribute("autocomplete", "off"));
            }

            if (novalidate)
            {
                attributes.Add(new HTMLAttribute("novalidate"));
            }

            if (additionalAttributes != null)
            {
                attributes.AddRange(additionalAttributes);
            }

            AddElement("form", attributes.ToArray());
            return this;
        }

        public HTMLBuilder Table<T>(IEnumerable<T> data, string name, string caption, HTMLAttribute[] attributes = null)
        {
            var addAttributes = new List<HTMLAttribute>();
            var type = typeof(T);
            var properties = type.GetProperties();
            if (!string.IsNullOrWhiteSpace(name))
            {
                addAttributes.Add(new HTMLAttribute("name", name));
            }
            if (attributes != null)
            {
                addAttributes.AddRange(attributes);
            }
            AddElement("table", addAttributes).Child();
            if (!string.IsNullOrWhiteSpace(caption))
            {
                AddElement("caption", caption);
            }
            AddElement("tr").Child();
            // Table headers
            foreach (var p in properties)
            {
                AddElement("th", p.Name);
            }
            Parent();
            // Table rows
            foreach (T row in data)
            {
                AddElement("tr").Child();
                foreach (var p in properties)
                {
                    AddElement("td", p.GetValue(row).ToString());
                }
                Parent();
            }
            Parent().Parent();
            return this;
        }

        public HTMLBuilder Table(System.Data.DataTable data, string name, string caption, HTMLAttribute[] attributes = null)
        {
            var addAttributes = new List<HTMLAttribute>();
            if (!string.IsNullOrWhiteSpace(name))
            {
                addAttributes.Add(new HTMLAttribute("name", name));
            }
            if (attributes != null)
            {
                addAttributes.AddRange(attributes);
            }
            AddElement("table", addAttributes).Child();
            if (!string.IsNullOrWhiteSpace(caption))
            {
                AddElement("caption", caption);
            }
            AddElement("tr").Child();
            // Table headers
            foreach (System.Data.DataColumn column in data.Columns)
            {
                AddElement("th", column.ColumnName);
            }
            Parent();
            // Table rows
            foreach (System.Data.DataRow row in data.Rows)
            {
                AddElement("tr").Child();
                foreach (System.Data.DataColumn column in data.Columns)
                {
                    AddElement("td", row[column.ColumnName].ToString());
                }
                Parent();
            }
            Parent().Parent();
            return this;
        }

        public HTMLBuilder Empty()
        {
            if (!AddNextNode)
            {
                return this;
            }

            var element = new HTMLEmpty();
            if (tree == null)
            {
                tree = new HTMLElementNode(null, element);
                lastNode = tree;
                AddChildNode = true;
            }
            else if (AddChildNode)
            {
                lastNode = lastNode.AddChild(element);
            }
            else
            {
                lastNode = lastNode.Parent.AddChild(element);
            }

            return this;
        }



        public HTMLBuilder BeginComponent(string componentName)
        {
            if (componentStack == null)
            {
                componentStack = new Stack<Tuple<string, HTMLElementNode>>();
            }

            if (tree == null)
            {
                Empty();
            }

            if (string.IsNullOrWhiteSpace(componentName))
            {
                componentName = "Component";
            }

            Comment($"Begin {componentName}");
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
                Comment($"End {componentName}");
                return this;
            }
            else
            {
                throw new Exception("There is no component started with BeginComponent left to end");
            }
        }

        public HTMLBuilder Component(string name, Action action)
        {
            BeginComponent(name);
            action.Invoke();
            EndComponent();
            return this;
        }

        public HTMLBuilder Component(Action action)
        {
            return Component("", action);
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
            if (sb == null)
            {
                throw new ArgumentNullException(nameof(sb));
            }
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
