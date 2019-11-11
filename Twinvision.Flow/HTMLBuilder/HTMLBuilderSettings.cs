namespace Twinvision.Flow
{
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
}
