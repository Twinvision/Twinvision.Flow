namespace Twinvision.Flow
{
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

        public HTMLDocument(HTMLDocumentType documentType) : this()
        {
            DocumentType = documentType;
        }

        public override string Open(bool enforceProperCase = true)
        {
            switch (DocumentType)
            {
                case HTMLDocumentType.HTML4_01_Frameset:
                    {
                        return "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Frameset//EN\" \"http://www.w3.org/TR/html4/frameset.dtd\">" + System.Environment.NewLine + base.Open(enforceProperCase);
                    }

                case HTMLDocumentType.HTML4_01_Strict:
                    {
                        return "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01//EN\" \"http://www.w3.org/TR/html4/strict.dtd\">" + System.Environment.NewLine + base.Open(true);
                    }

                case HTMLDocumentType.HTML4_01_Transitional:
                    {
                        return "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\" \"http://www.w3.org/TR/html4/loose.dtd\">" + System.Environment.NewLine + base.Open(enforceProperCase);
                    }

                case HTMLDocumentType.HTML5:
                    {
                        return "<!DOCTYPE html>" + System.Environment.NewLine + base.Open(enforceProperCase);
                    }

                case HTMLDocumentType.XHTML_1_1:
                    {
                        return "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.1//EN\" \"http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd\">" + System.Environment.NewLine + base.Open(enforceProperCase);
                    }

                default:
                    {
                        return base.Open(enforceProperCase);
                    }
            }
        }
    }
}
