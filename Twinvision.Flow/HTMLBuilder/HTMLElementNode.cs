using System.Collections.Generic;

namespace Twinvision.Flow
{
    /// <summary>
    /// This class represents a tree of IHTMLElement
    /// </summary>
    /// <remarks>Used together with the Interface IHTMLElement to construct an HTML document tree</remarks>
    public class HTMLElementNode
    {
        public IHTMLElement Element { get; set; }
        public HTMLElementNode Parent { get; set; }

        public HTMLElementNode(HTMLElementNode parent, IHTMLElement nodeData)
        {
            Element = nodeData;
            if (parent == null)
            {
                Parent = this;
            }
            else
            {
                Parent = parent;
            }

            Children = new List<HTMLElementNode>();
        }

        public List<HTMLElementNode> Children { get; }

        public HTMLElementNode this[int index] => Children[index];

        protected internal HTMLElementNode AddChild(IHTMLElement nodeData)
        {
            var newNode = new HTMLElementNode(this, nodeData);
            Children.Add(newNode);
            return newNode;
        }

        protected internal HTMLElementNode InsertChild(int index, IHTMLElement nodeData)
        {
            var newNode = new HTMLElementNode(this, nodeData);
            Children.Insert(index, newNode);
            return newNode;
        }

        protected internal void RemoveChild(HTMLElementNode nodeData)
        {
            Children.Remove(nodeData);
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
}
