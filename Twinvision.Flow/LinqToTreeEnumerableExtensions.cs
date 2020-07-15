using System;
using System.Collections.Generic;
using System.Linq;

namespace Twinvision.Flow
{
    public static class LinqToTreeEnumerableExtensions
    {
        /// <summary>
        /// Applies the given function to each of the items in the supplied
        /// IEnumerable.
        /// </summary>
        private static IEnumerable<HTMLElementNode> DrillDown(this IEnumerable<HTMLElementNode> items, Func<HTMLElementNode, IEnumerable<HTMLElementNode>> function)
        {
            foreach (HTMLElementNode item in items)
            {
                foreach (HTMLElementNode itemChild in function(item))
                {
                    yield return itemChild;
                }
            }
        }

        /// <summary>
        /// Returns a collection of descendant elements.
        /// </summary>
        public static IEnumerable<HTMLElementNode> Descendants(this IEnumerable<HTMLElementNode> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }
            return items.DrillDown(i => i.Descendants());
        }

        /// <summary>
        /// Returns a collection containing this element and all descendant elements.
        /// </summary>
        public static IEnumerable<HTMLElementNode> DescendantsAndSelf(this IEnumerable<HTMLElementNode> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }
            return items.DrillDown(i => i.DescendantsAndSelf());
        }

        /// <summary>
        /// Returns a collection of ancestor elements.
        /// </summary>
        public static IEnumerable<HTMLElementNode> Ancestors(this IEnumerable<HTMLElementNode> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }
            return items.DrillDown(i => i.Ancestors());
        }

        /// <summary>
        /// Returns a collection containing this element and all ancestor elements.
        /// </summary>
        public static IEnumerable<HTMLElementNode> AncestorsAndSelf(this IEnumerable<HTMLElementNode> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }
            return items.DrillDown(i => i.AncestorsAndSelf());
        }

        /// <summary>
        /// Returns a collection of child elements.
        /// </summary>
        public static IEnumerable<HTMLElementNode> Elements(this IEnumerable<HTMLElementNode> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }
            return items.DrillDown(i => i.Elements());
        }

        /// <summary>
        /// Returns a collection containing this element and all child elements.
        /// </summary>
        public static IEnumerable<HTMLElementNode> ElementsAndSelf(this IEnumerable<HTMLElementNode> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }
            return items.DrillDown(i => i.ElementsAndSelf());
        }
    }

    /// <summary>
    /// Defines extension methods for querying an ILinqTree
    /// </summary>
    public static class LinqToTreeExtensions
    {
        /// <summary>
        /// Returns a collection of descendant elements.
        /// </summary>
        public static IEnumerable<HTMLElementNode> Descendants(this HTMLElementNode adapter)
        {
            if (adapter == null)
            {
                throw new ArgumentNullException(nameof(adapter));
            }
            foreach (HTMLElementNode child in adapter.Children)
            {
                yield return child;

                foreach (HTMLElementNode grandChild in child.Descendants())
                {
                    yield return grandChild;
                }
            }
        }

        /// <summary>
        /// Returns a collection of ancestor elements.
        /// </summary>
        public static IEnumerable<HTMLElementNode> Ancestors(this HTMLElementNode adapter)
        {
            if (adapter == null)
            {
                throw new ArgumentNullException(nameof(adapter));
            }
            var parent = adapter.Parent;
            while (parent != null && parent != parent.Parent)
            {
                yield return parent;
                parent = parent.Parent;
            }
        }

        /// <summary>
        /// Returns a collection of child elements.
        /// </summary>
        public static IEnumerable<HTMLElementNode> Elements(this HTMLElementNode adapter)
        {
            if (adapter == null)
            {
                throw new ArgumentNullException(nameof(adapter));
            }
            foreach (HTMLElementNode child in adapter.Children)
            {
                yield return child;
            }
        }



        /// <summary>
        /// Returns a collection containing this element and all child elements.
        /// </summary>
        public static IEnumerable<HTMLElementNode> ElementsAndSelf(this HTMLElementNode adapter)
        {
            yield return adapter;
            foreach (HTMLElementNode child in adapter.Elements())
            {
                yield return child;
            }
        }

        /// <summary>
        /// Returns a collection of ancestor elements.
        /// </summary>
        public static IEnumerable<HTMLElementNode> AncestorsAndSelf(this HTMLElementNode adapter)
        {
            yield return adapter;
            foreach (HTMLElementNode child in adapter.Ancestors())
            {
                yield return child;
            }
        }

        /// <summary>
        /// Returns a collection containing this element and all descendant elements.
        /// </summary>
        public static IEnumerable<HTMLElementNode> DescendantsAndSelf(this HTMLElementNode adapter)
        {
            yield return adapter;
            foreach (HTMLElementNode child in adapter.Descendants())
            {
                yield return child;
            }
        }



        /// <summary>
        /// Returns a collection of descendant elements.
        /// </summary>
        public static IEnumerable<HTMLElementNode> Descendants<T>(this HTMLElementNode adapter)
        {
            return adapter.Descendants().Where(i => i is T);
        }
    }
}
