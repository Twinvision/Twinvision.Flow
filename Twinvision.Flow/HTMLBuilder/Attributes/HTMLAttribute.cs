namespace Twinvision.Flow
{
    /// <summary>
    /// This class represents an HTML attribute.
    /// These are the key value pairs you can find inside an opening tag of an HTML element
    /// </summary>
    /// <remarks></remarks>
    public class HTMLAttribute : IAttribute
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public HTMLAttribute(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public HTMLAttribute(string name)
        {
            Name = name;
            Value = null;
        }

        public override string ToString()
        {
            if (Value == null)
            {
                return Name;
            }
            else
            {
                return Name + "=\"" + Value + "\"";
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "Lower case is the proper casing for html attributes")]
        public string ToString(bool enforceProperCase)
        {
            if (enforceProperCase)
            {
                if (Value == null)
                {
                    return Name.ToLowerInvariant();
                }
                else
                {
                    return Name.ToLowerInvariant() + "=\"" + Value + "\"";
                }
            }
            else
            {
                return ToString();
            }
        }
    }
}
