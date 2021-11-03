using System.Collections.Generic;

namespace Twinvision.Flow
{
    /// <summary>
    /// Interface used for all HTML elements. The internal tree uses this interface for its nodes
    /// </summary>
    /// <remarks></remarks>
    public interface IHTMLElement
    {
        string Open(bool enforceProperCase = true);
        string Close(bool enforceProperCase = true);
        string Empty(bool enforceProperCase = true);
        List<IAttribute> Attributes { get; }
        string Content { get; set; }
        bool IsMultiLine { get; }
        ContentPosition ContentPosition { get; set; }
        string Tag(bool enforceProperCase = true);
        string ToString();
        string ToString(bool enforceProperCase = true);
    }
}
