Imports System.Text
Imports System.Text.RegularExpressions
Imports Newtonsoft.Json

#Region "Interfaces"

''' <summary>
''' Interface used for all HTML elements. The internal tree uses this interface for its nodes
''' </summary>
''' <remarks></remarks>
Public Interface IHTMLElement
    ReadOnly Property Open As String
    ReadOnly Property Open(enforceProperCase As Boolean) As String
    ReadOnly Property Close As String
    ReadOnly Property Empty As String
    ReadOnly Property Empty(enforceProperCase As Boolean) As String
    Property Attributes As List(Of HTMLAttribute)
    Property Content As String
    ReadOnly Property IsMultiLine As Boolean
    Property ContentPosition As ContentPosition
    Property Tag As String
    Property Tag(enforceProperCase As Boolean) As String
    Function ToString() As String
    Function ToString(enforceProperCase As Boolean) As String
End Interface

#End Region

#Region "Enums"

Public Enum ContentPosition As Integer
    BeforeElements = 0
    AfterAlements = 1
End Enum

Public Enum HTMLDocumentType As Integer
    XHTML_1_1 = 0
    HTML4_01_Frameset = 1
    HTML4_01_Strict = 2
    HTML4_01_Transitional = 3
    HTML5 = 4
    Undefined = 255
End Enum

Public Enum FormMethod As Integer
    [Get] = 1
    Post = 2
End Enum

Public Enum FormEncodingType As Integer
    UrlEncoded = 1
    FormData = 2
    Plain = 3
End Enum

Public Enum Target As Integer
    Blank = 1
    Self = 2
    Parent = 3
    Top = 4
End Enum

#End Region

#Region "Helper classes"

Public Class FlowJsonSerializer
    Inherits JsonSerializer
    Public Sub New()
        Me.ContractResolver = New Serialization.CamelCasePropertyNamesContractResolver()
        Me.Formatting = Formatting.Indented
        Me.ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    End Sub
End Class

''' <summary>
''' This class represents a tree of IHTMLElement
''' </summary>
''' <remarks>Used together with the Interface IHTMLElement to construct an HTML document tree</remarks>
Public Class HTMLElementNode
    Public Property Element As IHTMLElement
    Public Property Parent As HTMLElementNode
    Private _childNodes As List(Of HTMLElementNode)

    Public Sub New(parent As HTMLElementNode, ByVal nodeData As IHTMLElement)
        Element = nodeData
        If parent Is Nothing Then
            Me.Parent = Me
        Else
            Me.Parent = parent
        End If
        _childNodes = New List(Of HTMLElementNode)
    End Sub

    Public ReadOnly Property Children() As HTMLElementNode()
        Get
            Return _childNodes.ToArray
        End Get
    End Property

    Public ReadOnly Property Item(ByVal index As Long) As HTMLElementNode
        Get
            Return _childNodes(index)
        End Get
    End Property

    Public ReadOnly Property Item As HTMLElementNode
        Get
            Return Me
        End Get
    End Property

    Protected Friend Function AddChild(ByVal nodeData As IHTMLElement) As HTMLElementNode
        Dim newNode As HTMLElementNode = New HTMLElementNode(Me, nodeData)
        _childNodes.Add(newNode)
        Return newNode
    End Function

    Protected Friend Function InsertChild(index As Integer, ByVal nodeData As IHTMLElement) As HTMLElementNode
        Dim newNode As HTMLElementNode = New HTMLElementNode(Me, nodeData)
        _childNodes.Insert(index, newNode)
        Return newNode
    End Function

    Protected Friend Sub RemoveChild(ByVal nodeData As HTMLElementNode)
        _childNodes.Remove(nodeData)
    End Sub

    Overrides Function ToString() As String
        Return Element.ToString()
    End Function

    Overloads Function ToString(enforceProperCase As Boolean) As String
        Return Element.ToString(enforceProperCase)
    End Function

End Class

#End Region

#Region "HTML format related classes"

Public Class HTMLTag
    Public Property Name As String
    Public Property DocType As Boolean()
End Class

''' <summary>
''' This class represents an HTML attribute. 
''' These are the key value pairs you can find inside an opening tag of an HTML element
''' </summary>
''' <remarks></remarks>
Public Class HTMLAttribute
    Private _name As String
    Private _value As String

    Public Property Name As String
        Get
            Return _name
        End Get
        Set(value As String)
            _name = value
        End Set
    End Property

    Public Property Value As String
        Get
            Return _value
        End Get
        Set(value As String)
            _value = value
        End Set
    End Property

    Sub New(name As String, value As String)
        Me.Name = name
        Me.Value = value
    End Sub

    Sub New(name As String)
        Me.Name = name
        Me.Value = Nothing
    End Sub

    Public Overrides Function ToString() As String
        If Value Is Nothing Then
            Return Name
        Else
            Return Name + "=""" + Value + """"
        End If
    End Function

    Public Overloads Function ToString(enforceProperCase) As String
        If enforceProperCase Then
            If Value Is Nothing Then
                Return Name.ToLowerInvariant
            Else
                Return Name.ToLowerInvariant + "=""" + Value + """"
            End If
        Else
            Return ToString()
        End If
    End Function
End Class

''' <summary>
''' This class represents an HTML comment. It supports single and multi line comments
''' </summary>
''' <remarks></remarks>
Public Class HTMLComment
    Implements IHTMLElement

    Private _content As String = ""
    Private _isMultiLine As Boolean

    Public Property Content As String Implements IHTMLElement.Content
        Get
            Return _content
        End Get
        Set(value As String)
            _content = value
            _isMultiLine = _content.Contains(vbCrLf)
        End Set
    End Property

    Public ReadOnly Property Open As String Implements IHTMLElement.Open
        Get
            If IsMultiLine Then
                Return "<!--"
            Else
                Return "<!-- "
            End If
        End Get
    End Property

    Public ReadOnly Property Open(enforceProperCase As Boolean) As String Implements IHTMLElement.Open
        Get
            Return Me.Open
        End Get
    End Property

    Public ReadOnly Property Close As String Implements IHTMLElement.Close
        Get
            If IsMultiLine Then
                Return "-->"
            Else
                Return " -->"
            End If
        End Get
    End Property

    Public ReadOnly Property IsMultiLine As Boolean Implements IHTMLElement.IsMultiLine
        Get
            Return _isMultiLine
        End Get
    End Property

    Sub New(content As String)
        Me.Content = content
    End Sub

    Public Overrides Function ToString() As String Implements IHTMLElement.ToString
        If IsMultiLine Then
            Return Open + vbCrLf + Content + vbCrLf + Close
        Else
            Return Open + " " + Content + " " + Close
        End If
    End Function

    Public Overloads Function ToString(enforceProperCase As Boolean) As String Implements IHTMLElement.ToString
        Return ToString()
    End Function

    Public Property Attributes As List(Of HTMLAttribute) Implements IHTMLElement.Attributes

    Public ReadOnly Property Empty As String Implements IHTMLElement.Empty
        Get
            Return ""
        End Get
    End Property

    Public ReadOnly Property Empty(enforceProperCase As Boolean) As String Implements IHTMLElement.Empty
        Get
            Return ""
        End Get
    End Property

    Public Property Tag As String Implements IHTMLElement.Tag
        Get
            Return ""
        End Get
        Set(value As String)
            Throw New NotImplementedException
        End Set
    End Property

    Public Property Tag(enforceProperCase As Boolean) As String Implements IHTMLElement.Tag
        Get
            Return ""
        End Get
        Set(value As String)
            Throw New NotImplementedException
        End Set
    End Property

    Public Property ContentPosition As ContentPosition = Flow.ContentPosition.BeforeElements Implements IHTMLElement.ContentPosition
End Class

''' <summary>
''' This class represents an empty element node without any content. this can be used to create an empty root element to provide multiple root elements in the output
''' </summary>
''' <remarks></remarks>
Public Class HTMLEmpty
    Implements IHTMLElement

    Public Property Content As String Implements IHTMLElement.Content
        Get
            Return ""
        End Get
        Set(value As String)
            ' Not needed
        End Set
    End Property

    Public ReadOnly Property Open As String Implements IHTMLElement.Open
        Get
            Return ""
        End Get
    End Property

    Public ReadOnly Property Open(enforceProperCase As Boolean) As String Implements IHTMLElement.Open
        Get
            Return ""
        End Get
    End Property

    Public ReadOnly Property Close As String Implements IHTMLElement.Close
        Get
            Return ""
        End Get
    End Property

    Public ReadOnly Property IsMultiLine As Boolean Implements IHTMLElement.IsMultiLine
        Get
            Return False
        End Get
    End Property

    Public Overrides Function ToString() As String Implements IHTMLElement.ToString
        If IsMultiLine Then
            Return Open + vbCrLf + Content + vbCrLf + Close
        Else
            Return Open + " " + Content + " " + Close
        End If
    End Function

    Public Overloads Function ToString(enforceProperCase As Boolean) As String Implements IHTMLElement.ToString
        Return ToString()
    End Function

    Public Property Attributes As List(Of HTMLAttribute) Implements IHTMLElement.Attributes

    Public ReadOnly Property Empty As String Implements IHTMLElement.Empty
        Get
            Return ""
        End Get
    End Property

    Public ReadOnly Property Empty(enforceProperCase As Boolean) As String Implements IHTMLElement.Empty
        Get
            Return ""
        End Get
    End Property

    Public Property Tag As String Implements IHTMLElement.Tag
        Get
            Return ""
        End Get
        Set(value As String)
            Throw New NotImplementedException
        End Set
    End Property

    Public Property Tag(enforceProperCase As Boolean) As String Implements IHTMLElement.Tag
        Get
            Return ""
        End Get
        Set(value As String)
            Throw New NotImplementedException
        End Set
    End Property

    Public Property ContentPosition As ContentPosition = Flow.ContentPosition.BeforeElements Implements IHTMLElement.ContentPosition
End Class

''' <summary>
''' This class represents the html element. It will automatically prefix it with an (html 5) doctype element
''' </summary>
''' <remarks>This element can only be added to the root. Otherwise an exception will be thrown.</remarks>
Public Class HTMLDocument
    Inherits HTMLElement

    Private Property DocumentType As HTMLDocumentType = Flow.HTMLDocumentType.HTML5

    Sub New()
        MyBase.New("html")
    End Sub

    Sub New(documentType As HTMLDocumentType)
        MyClass.New()
        Me.DocumentType = documentType
    End Sub

    Public Overrides ReadOnly Property Open As String
        Get
            Select Case DocumentType
                Case Flow.HTMLDocumentType.HTML4_01_Frameset
                    Return "<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.01 Frameset//EN"" ""http://www.w3.org/TR/html4/frameset.dtd"">" + vbCrLf + MyBase.Open
                Case Flow.HTMLDocumentType.HTML4_01_Strict
                    Return "<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.01//EN"" ""http://www.w3.org/TR/html4/strict.dtd"">" + vbCrLf + MyBase.Open
                Case Flow.HTMLDocumentType.HTML4_01_Transitional
                    Return "<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.01 Transitional//EN"" ""http://www.w3.org/TR/html4/loose.dtd"">" + vbCrLf + MyBase.Open
                Case Flow.HTMLDocumentType.HTML5
                    Return "<!DOCTYPE html>" + vbCrLf + MyBase.Open
                Case Flow.HTMLDocumentType.XHTML_1_1
                    Return "<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.1//EN"" ""http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd"">" + vbCrLf + MyBase.Open
                Case Else
                    Return MyBase.Open
            End Select
        End Get
    End Property

    Public Overrides ReadOnly Property Open(enforceProperCase As Boolean) As String
        Get
            Return Me.Open
        End Get
    End Property
End Class

''' <summary>
''' This class represents an html element. Most elements inside a typical HTML document will be of this type.
''' </summary>
''' <remarks></remarks>
Public Class HTMLElement
    Implements IHTMLElement

    Private _tag As String = ""
    Private _content As String = ""
    Private _isMultiLine As Boolean

    Public Overridable Property Content As String Implements IHTMLElement.Content
        Get
            Return _content
        End Get
        Set(value As String)
            _content = value
            _isMultiLine = _content.Contains(vbCrLf)
        End Set
    End Property

    Public Property Tag As String Implements IHTMLElement.Tag
        Get
            Return _tag
        End Get
        Set(value As String)
            If Regex.IsMatch(value, "^[A-Za-z0-9]*$") Then
                _tag = value
            Else
                Throw New Exception(String.Format("Tag <{0}> contains invalid characters", value))
            End If
        End Set
    End Property

    Public Property Tag(enforceProperCase As Boolean) As String Implements IHTMLElement.Tag
        Get
            If enforceProperCase Then
                Return _tag.ToLowerInvariant
            Else
                Return _tag
            End If
        End Get
        Set(value As String)
            Me.Tag = value
        End Set
    End Property

    Public Property Attributes As New List(Of HTMLAttribute) Implements IHTMLElement.Attributes

    Sub New(tag As String)
        Me.Tag = tag
    End Sub

    Sub New(tag As String, content As String)
        Me.Tag = tag
        Me.Content = content
    End Sub

    Sub New(tag As String, attributes As Generic.IEnumerable(Of HTMLAttribute))
        Me.Tag = tag
        If attributes IsNot Nothing Then
            Me.Attributes.AddRange(attributes)
        End If
    End Sub

    Sub New(tag As String, attributes As Generic.IEnumerable(Of HTMLAttribute), content As String)
        Me.Tag = tag
        Me.Content = content
        If attributes IsNot Nothing Then
            Me.Attributes.AddRange(attributes)
        End If
    End Sub

    Sub New(tag As String, content As String, attributes As Generic.IEnumerable(Of HTMLAttribute))
        Me.Tag = tag
        Me.Content = content
        If attributes IsNot Nothing Then
            Me.Attributes.AddRange(attributes)
        End If
    End Sub

    Sub New(tag As String, content As String, attributes As Generic.IEnumerable(Of HTMLAttribute), contentPosition As ContentPosition)
        Me.Tag = tag
        Me.Content = content
        If attributes IsNot Nothing Then
            Me.Attributes.AddRange(attributes)
        End If
        Me.ContentPosition = contentPosition
    End Sub

    Public Overridable ReadOnly Property Open As String Implements IHTMLElement.Open
        Get
            If Attributes.Count = 0 Then
                Return "<" + Tag + ">"
            Else
                Dim result As String = ""
                result = "<" + Tag
                For Each attribute In Attributes
                    result += " " + attribute.ToString
                Next
                result += ">"
                Return result
            End If
        End Get
    End Property

    Public Overridable ReadOnly Property Open(enforceProperCase As Boolean) As String Implements IHTMLElement.Open
        Get
            If Attributes.Count = 0 Then
                Return "<" + Tag + ">"
            Else
                Dim result As String = ""
                result = "<" + Tag
                For Each attribute In Attributes
                    result += " " + attribute.ToString(enforceProperCase)
                Next
                result += ">"
                Return result
            End If
        End Get
    End Property

    Public Overridable ReadOnly Property Close As String Implements IHTMLElement.Close
        Get
            Return "</" + Tag + ">"
        End Get
    End Property

    Public ReadOnly Property Empty As String Implements IHTMLElement.Empty
        Get
            If Attributes.Count = 0 Then
                Return "<" + Tag + "/>"
            Else
                Dim result As String = "<" + Tag
                For Each attribute In Attributes
                    result += " " + attribute.ToString
                Next
                result += " />"
                Return result
            End If
        End Get
    End Property

    Public ReadOnly Property Empty(enforceProperCase As Boolean) As String Implements IHTMLElement.Empty
        Get
            If Attributes.Count = 0 Then
                Return "<" + Tag + "/>"
            Else
                Dim result As String = "<" + Tag
                For Each attribute In Attributes
                    result += " " + attribute.ToString(enforceProperCase)
                Next
                result += " />"
                Return result
            End If
        End Get
    End Property

    Public Overrides Function ToString() As String Implements IHTMLElement.ToString
        If String.IsNullOrWhiteSpace(Content) AndAlso HTMLTags.SelfClosing.Contains(Tag.ToLowerInvariant) Then
            Return Empty
        Else
            Return Open + Content + Close
        End If
    End Function

    Public Overloads Function ToString(enforceProperCase As Boolean) As String Implements IHTMLElement.ToString
        If String.IsNullOrWhiteSpace(Content) AndAlso HTMLTags.SelfClosing.Contains(Tag.ToLowerInvariant) Then
            Return Empty(enforceProperCase)
        Else
            Return Open(enforceProperCase) + Content + Close
        End If
    End Function


    Public ReadOnly Property IsMultiLine As Boolean Implements IHTMLElement.IsMultiLine
        Get
            Return _isMultiLine
        End Get
    End Property

    Public Property ContentPosition As ContentPosition = Flow.ContentPosition.BeforeElements Implements IHTMLElement.ContentPosition
End Class

#End Region

#Region "HTML builder"

''' <summary>
''' This class is instantiated as a shared DefaultSettings property on the HTMLBuilder class.
''' Defaults are provided but you are free to change them at any time.
''' </summary>
''' <remarks>
''' NOTE: These settings will be applied across all instances of the HTMLBuilder class, but
'''       you can change it by providing your own settings on or after HTMLBuilder object creation
''' 
''' You do not have to supply these settings before adding elements to the HTMLBuilder. 
''' All settings will be applied on the fly when using any of the output functions (i.e. ToString or Write).
''' </remarks>
Public Class HTMLBuilderSettings
    Property TabSize As Integer = 4
    Property EnforceProperCase As Boolean = True
    Property EnforceDocType As Boolean = True
    Property EnforceProperNesting As Boolean = True
    Property IndentHeaderAndBodyTags As Boolean = True
    Property WriteComments As Boolean = True
End Class

''' <summary>
''' This class alows you to add elements using many helper functions (i.e. Document Body, Header and Div) 
''' and create some hopefully well-formed HTML output
''' </summary>
''' <remarks></remarks>
Public Class HTMLBuilder

#Region "Properties"

    Private tree As HTMLElementNode
    Private lastNode As HTMLElementNode
    Private componentStack As Stack(Of Tuple(Of String, HTMLElementNode))

    Private _addChildNode As Boolean = False
    Private _addNextNode As Boolean = True

    Public Settings As New HTMLBuilderSettings
    Public Shared Property DefaultSettings As New HTMLBuilderSettings

    Public Property DocumentType As HTMLDocumentType

    Public ReadOnly Property DOM As HTMLElementNode
        Get
            If tree Is Nothing Then
                Throw New Exception("Add an element first before accessing the DOM.")
            End If
            Return tree
        End Get
    End Property

#End Region

#Region "Protected helper functions"

    Sub New()
        Me.Settings = DefaultSettings
        Me.DocumentType = HTMLDocumentType.HTML5
    End Sub

    Sub New(settings As HTMLBuilderSettings)
        Me.Settings = settings
        Me.DocumentType = HTMLDocumentType.HTML5
    End Sub

    Sub New(documentType As HTMLDocumentType)
        Me.DocumentType = documentType
    End Sub

    Sub New(documentType As HTMLDocumentType, settings As HTMLBuilderSettings)
        Me.DocumentType = documentType
        Me.Settings = settings
    End Sub

#End Region

#Region "Main builder Function"

    Private Sub WriteTree(sb As StringBuilder, root As HTMLElementNode, Optional nestedLevel As Integer = 0)
        If Not root Is Nothing Then
            Dim content As String = "", elementType As Type = root.Element.GetType

            If elementType Is GetType(HTMLEmpty) OrElse (Not Settings.WriteComments And elementType Is GetType(HTMLComment)) Then
                For Each child As HTMLElementNode In root.Children
                    WriteTree(sb, child, nestedLevel)
                Next
            Else
                If root.Children.Length = 0 AndAlso String.IsNullOrEmpty(root.Element.Content) AndAlso HTMLTags.SelfClosing.Contains(root.Element.Tag.ToLowerInvariant) Then
                    sb.AppendLine(Space(nestedLevel * Settings.TabSize) + root.Element.Empty(Settings.EnforceProperCase))
                Else
                    sb.Append(Space(nestedLevel * Settings.TabSize) + root.Element.Open(Settings.EnforceProperCase))
                    If root.Children.Length > 0 Or root.Element.IsMultiLine Then
                        sb.AppendLine()
                    End If
                    If root.Element.Content <> "" Then
                        If root.Children.Length > 0 Or root.Element.IsMultiLine Then
                            Using sr As New IO.StringReader(root.Element.Content)
                                While sr.Peek <> -1
                                    content += Space((nestedLevel + 1) * Settings.TabSize) + sr.ReadLine + Environment.NewLine
                                End While
                            End Using
                        Else
                            content += root.Element.Content
                        End If
                        If root.Element.ContentPosition = ContentPosition.BeforeElements Then
                            sb.Append(content)
                        End If
                    End If
                    For Each child As HTMLElementNode In root.Children
                        If Settings.IndentHeaderAndBodyTags OrElse (Not Settings.IndentHeaderAndBodyTags AndAlso Not root.Element.Tag(True) = "html") Then
                            nestedLevel += 1
                        End If
                        WriteTree(sb, child, nestedLevel)
                        If Settings.IndentHeaderAndBodyTags OrElse (Not Settings.IndentHeaderAndBodyTags AndAlso Not root.Element.Tag(True) = "html") Then
                            nestedLevel -= 1
                        End If
                    Next
                    If root.Element.ContentPosition = ContentPosition.AfterAlements AndAlso content <> "" Then
                        sb.Append(content)
                    End If
                    If root.Children.Length = 0 AndAlso Not root.Element.IsMultiLine Then
                        sb.AppendLine(root.Element.Close)
                    Else
                        sb.AppendLine(Space(nestedLevel * Settings.TabSize) + root.Element.Close)
                    End If
                End If
            End If
        End If
    End Sub

#End Region

#Region "Navigation and modification functions"

    Public Function Parent() As HTMLBuilder
        lastNode = lastNode.Parent
        Return Me
    End Function

    Public Function Child() As HTMLBuilder
        If lastNode IsNot Nothing AndAlso TypeOf lastNode.Element Is HTMLComment Then
            Throw New Exception("HTML comments cannot have child elements")
        End If
        AddChildNode = True
        Return Me
    End Function

    Private Property AddChildNode As Boolean
        Get
            If _addChildNode Then
                _addChildNode = False
                Return True
            Else
                Return False
            End If
        End Get
        Set(value As Boolean)
            _addChildNode = value
        End Set
    End Property

    Private Property AddNextNode As Boolean
        Get
            If Not _addNextNode Then
                _addNextNode = True
                Return False
            Else
                Return True
            End If
        End Get
        Set(value As Boolean)
            _addNextNode = value
        End Set
    End Property

#End Region

#Region "Generic element functions"

    Public Function AddElement(element As IHTMLElement) As HTMLBuilder
        Return AddElement(-1, element.Tag, element.Attributes.ToArray, element.Content, AddChildNode, ContentPosition.BeforeElements)
    End Function

    Public Function AddElement(tag As String) As HTMLBuilder
        Return AddElement(tag, Nothing, "")
    End Function

    Public Function AddElement(tag As String, content As String) As HTMLBuilder
        Return AddElement(tag, Nothing, content)
    End Function

    Public Function AddElement(tag As String, attributes As Generic.IEnumerable(Of HTMLAttribute), content As String, contentPosition As ContentPosition) As HTMLBuilder
        Return AddElement(-1, tag, attributes, content, AddChildNode, contentPosition)
    End Function

    Public Function AddElement(tag As String, attributes As Generic.IEnumerable(Of HTMLAttribute)) As HTMLBuilder
        Return AddElement(-1, tag, attributes, "", AddChildNode, ContentPosition.BeforeElements)
    End Function

    Public Function AddElement(tag As String, attributes As Generic.IEnumerable(Of HTMLAttribute), content As String) As HTMLBuilder
        Return AddElement(-1, tag, attributes, content, AddChildNode, ContentPosition.BeforeElements)
    End Function

    Public Function InsertElement(index As Integer, element As IHTMLElement) As HTMLBuilder
        Return AddElement(index, element.Tag, element.Attributes.ToArray, element.Content, AddChildNode, ContentPosition.BeforeElements)
    End Function

    Public Function InsertElement(index As Integer, tag As String) As HTMLBuilder
        Return AddElement(index, tag, Nothing, "", AddChildNode, ContentPosition.BeforeElements)
    End Function

    Public Function InsertElement(index As Integer, tag As String, content As String) As HTMLBuilder
        Return AddElement(index, tag, Nothing, content, AddChildNode, ContentPosition.BeforeElements)
    End Function

    Public Function InsertElement(index As Integer, tag As String, attributes As Generic.IEnumerable(Of HTMLAttribute), content As String) As HTMLBuilder
        Return AddElement(index, tag, attributes, content, AddChildNode, ContentPosition.BeforeElements)
    End Function

    Public Function InsertElement(index As Integer, tag As String, attributes As Generic.IEnumerable(Of HTMLAttribute), content As String, contentPosition As ContentPosition) As HTMLBuilder
        Return AddElement(index, tag, attributes, content, AddChildNode, contentPosition)
    End Function

    Private Function AddElement(index As Integer, tag As String, attributes As Generic.IEnumerable(Of HTMLAttribute), content As String, asChild As Boolean, contentPosition As ContentPosition) As HTMLBuilder
        Dim tagLowerCase As String = tag.ToLowerInvariant, node As HTMLElementNode

        ' Do not add this node if this property is false
        If Not AddNextNode Then Return Me

        ' Check doc type
        If Settings.EnforceDocType AndAlso Not DocumentType = HTMLDocumentType.Undefined Then
            If HTMLTags.Support.ContainsKey(tagLowerCase) Then
                If Not HTMLTags.Support.Item(tagLowerCase)(DocumentType) Then
                    Throw New Exception(String.Format("Tag <{0}> not supported for document type {1}", tag, DocumentType.ToString))
                End If
            Else
                Throw New Exception(String.Format("<{0}> is not a valid tag for document type {1}", tag, DocumentType.ToString))
            End If
        End If

        ' Check nesting if enabled
        If tree IsNot Nothing AndAlso Settings.EnforceProperNesting Then
            If HTMLTags.Nesting.ContainsKey(tagLowerCase) Then
                Dim foundNestingItem As String
                If asChild Then
                    node = lastNode
                Else
                    node = lastNode.Parent
                End If
                foundNestingItem = HTMLTags.Nesting.Item(tagLowerCase).FirstOrDefault(Function(where) where.StartsWith(node.Element.Tag.ToLower))
                If foundNestingItem IsNot Nothing Then
                    Dim nestingItemCount As String() = foundNestingItem.Split(":")
                    If nestingItemCount(1) = "1" AndAlso node.Descendants.Count(Function(where) where.Element.Tag.ToLower = tagLowerCase) > 0 Then
                        Throw New Exception(String.Format("Tag <{0}> cannot be nested multiple times inside tag <{1}>", tag, node.Element.Tag))
                    End If
                Else
                    Throw New Exception(String.Format("Tag <{0}> cannot be nested inside tag <{1}>", tag, node.Element.Tag))
                End If
            End If
        End If

        Dim element As New HTMLElement(tag, content, attributes, contentPosition)
        If tree Is Nothing Then
            tree = New HTMLElementNode(Nothing, element)
            lastNode = tree
            AddChildNode = True
        Else
            If asChild Then
                If index < 0 Then
                    lastNode = lastNode.AddChild(element)
                Else
                    lastNode = lastNode.InsertChild(index, element)
                End If
            Else
                If index < 0 Then
                    lastNode = lastNode.Parent.AddChild(element)
                Else
                    lastNode = lastNode.Parent.InsertChild(index, element)
                End If
            End If
        End If
        Return Me
    End Function

    Private Sub AddElementsFromElementEnumerable(elements As Generic.IEnumerable(Of HTMLElementNode))
        For Each element In elements
            AddElement(element.Element.Tag, element.Element.Attributes, element.Element.Content)
            If element.Children.Length > 0 Then
                AddChildNode = True
                AddElementsFromElementEnumerable(element.Children)
            End If
        Next
    End Sub

    Public Function AddElementsFrom(builder As HTMLBuilder) As HTMLBuilder
        If builder.tree IsNot Nothing Then
            Dim storeLastNode As HTMLElementNode = lastNode
            AddElementsFromElementEnumerable({builder.tree})
            lastNode = storeLastNode
        End If
        Return Me
    End Function

    Public Function AddElementsFrom(elements As HTMLElementNode()) As HTMLBuilder
        AddElementsFromElementEnumerable(elements)
        Return Me
    End Function

    Public Function SetActiveElement(element As HTMLElementNode) As HTMLBuilder
        Dim node As HTMLElementNode
        node = Me.DOM.DescendantsAndSelf.FirstOrDefault(Function(where) where Is element)
        If node IsNot Nothing Then
            lastNode = node
        Else
            Throw New Exception("Element not found")
        End If
        Return Me
    End Function

    Public Function DeleteElement(element As HTMLElementNode) As HTMLBuilder
        Dim node As HTMLElementNode
        node = Me.DOM.DescendantsAndSelf.FirstOrDefault(Function(where) where Is element)

        If node IsNot Nothing Then
            If node.Parent Is node Then
                Throw New Exception("Cannot remove root element")
            Else
                lastNode = node.Parent
                lastNode.RemoveChild(node)
            End If
        Else
            Throw New Exception("Element not found")
        End If
        Return Me
    End Function

    Public Function AddAttribute(name As String) As HTMLBuilder
        If Not AddNextNode Then Return Me
        lastNode.Element.Attributes.Add(New HTMLAttribute(name))
        Return Me
    End Function

    Public Function AddAttribute(name As String, value As String) As HTMLBuilder
        If Not AddNextNode Then Return Me
        lastNode.Element.Attributes.Add(New HTMLAttribute(name, value))
        Return Me
    End Function

    ''' <summary>
    ''' A call to this function does not impact Child() and Parent() functions, it affects only
    ''' the next element added in the builder
    ''' </summary>
    ''' <param name="expression"></param>
    ''' <returns>HTMLBuilder</returns>
    ''' <remarks></remarks>
    Public Function OnlyWhen(expression As System.Func(Of Boolean)) As HTMLBuilder
        AddNextNode = expression.Invoke()
        Return Me
    End Function

#End Region

#Region "Targeted element functions"

    Public Function Div(Optional className As String = "", Optional id As String = "", Optional content As String = "", Optional additionalAttributes As HTMLAttribute() = Nothing) As HTMLBuilder
        Dim attributes As New List(Of HTMLAttribute)
        If Not String.IsNullOrWhiteSpace(className) Then
            attributes.Add(New HTMLAttribute("class", className))
        End If
        If Not String.IsNullOrWhiteSpace(id) Then
            attributes.Add(New HTMLAttribute("id", id))
        End If
        If additionalAttributes IsNot Nothing Then
            attributes.AddRange(additionalAttributes)
        End If
        Return AddElement("div", attributes.ToArray, content)
    End Function

    Public Function Body(Optional className As String = "", Optional content As String = "", Optional additionalAttributes As HTMLAttribute() = Nothing) As HTMLBuilder
        Dim attributes As New List(Of HTMLAttribute)

        If Not String.IsNullOrWhiteSpace(className) Then
            attributes.Add(New HTMLAttribute("class", className))
        End If
        If additionalAttributes IsNot Nothing Then
            attributes.AddRange(additionalAttributes)
        End If
        AddElement("body", attributes.ToArray, content)
        AddChildNode = True
        Return Me
    End Function

    Public Function A(Optional href As String = "", Optional content As String = "", Optional className As String = "", Optional additionalAttributes As HTMLAttribute() = Nothing) As HTMLBuilder
        Dim attributes As New List(Of HTMLAttribute)
        If Not String.IsNullOrWhiteSpace(className) Then
            attributes.Add(New HTMLAttribute("class", className))
        End If
        If Not String.IsNullOrWhiteSpace(href) Then
            attributes.Add(New HTMLAttribute("href", href))
        End If
        If additionalAttributes IsNot Nothing Then
            attributes.AddRange(additionalAttributes)
        End If
        AddElement("a", attributes.ToArray, content)
        Return Me
    End Function

    Public Function P(Optional content As String = "", Optional className As String = "", Optional additionalAttributes As HTMLAttribute() = Nothing) As HTMLBuilder
        Dim attributes As New List(Of HTMLAttribute)
        If Not String.IsNullOrWhiteSpace(className) Then
            attributes.Add(New HTMLAttribute("class", className))
        End If
        If additionalAttributes IsNot Nothing Then
            attributes.AddRange(additionalAttributes)
        End If
        AddElement("p", attributes.ToArray, content)
        Return Me
    End Function

    Public Function BR(Optional additionalAttributes As HTMLAttribute() = Nothing) As HTMLBuilder
        Dim attributes As New List(Of HTMLAttribute)
        If additionalAttributes IsNot Nothing Then
            attributes.AddRange(additionalAttributes)
        End If
        AddElement("br", attributes.ToArray, "")
        Return Me
    End Function

    Public Function H(level As Integer, Optional content As String = "", Optional className As String = "", Optional additionalAttributes As HTMLAttribute() = Nothing) As HTMLBuilder
        Dim attributes As New List(Of HTMLAttribute)
        If Not String.IsNullOrWhiteSpace(className) Then
            attributes.Add(New HTMLAttribute("class", className))
        End If
        If additionalAttributes IsNot Nothing Then
            attributes.AddRange(additionalAttributes)
        End If
        AddElement("h" + level.ToString, attributes.ToArray, content)
        Return Me
    End Function

    Public Function Header(Optional additionalAttributes As HTMLAttribute() = Nothing) As HTMLBuilder
        Return Header("", "", "", additionalAttributes)
    End Function

    Public Function Header(title As String, Optional additionalAttributes As HTMLAttribute() = Nothing) As HTMLBuilder
        Return Header(title, "", "", additionalAttributes)
    End Function

    Public Function Header(title As String, description As String, Optional additionalAttributes As HTMLAttribute() = Nothing) As HTMLBuilder
        Return Header(title, description, "", additionalAttributes)
    End Function

    Public Function Header(title As String, description As String, keywords As String, Optional additionalAttributes As HTMLAttribute() = Nothing) As HTMLBuilder
        Dim keepNode As HTMLElementNode = lastNode

        Try
            InsertElement(0, "head", additionalAttributes, "")
            If Not String.IsNullOrWhiteSpace(title) Then
                AddElement(-1, "title", Nothing, title, True, ContentPosition.BeforeElements)
            End If
            If Not String.IsNullOrWhiteSpace(description) Then
                AddElement(-1, "meta", New HTMLAttribute() {New HTMLAttribute("name", "description"), New HTMLAttribute("content", description)}, "", False, ContentPosition.BeforeElements)
            End If
            If Not String.IsNullOrWhiteSpace(keywords) Then
                AddElement(-1, "meta", New HTMLAttribute() {New HTMLAttribute("name", "keywords"), New HTMLAttribute("content", keywords)}, "", False, ContentPosition.BeforeElements)
            End If
        Finally
            If keepNode IsNot Nothing Then
                lastNode = keepNode
            End If
        End Try
        Return Me
    End Function

    Public Function Meta(name As String, content As String, Optional additionalAttributes As HTMLAttribute() = Nothing) As HTMLBuilder
        Dim keepNode As HTMLElementNode = lastNode
        Try
            If Not String.IsNullOrWhiteSpace(name) Then
                If additionalAttributes Is Nothing Then
                    additionalAttributes = {}
                End If
            End If
            AddElement("meta", {New HTMLAttribute("name", name), New HTMLAttribute("content", content)}.Concat(additionalAttributes).ToArray, "")
        Finally
            If keepNode IsNot Nothing Then
                lastNode = keepNode
            End If
        End Try
        Return Me
    End Function

    Public Function Link(rel As String, href As String, Optional additionalAttributes As HTMLAttribute() = Nothing) As HTMLBuilder
        Dim keepNode As HTMLElementNode = lastNode
        Try
            If Not String.IsNullOrWhiteSpace(rel) Then
                If additionalAttributes Is Nothing Then
                    additionalAttributes = {}
                End If
                AddElement("link", {New HTMLAttribute("rel", rel), New HTMLAttribute("href", href)}.Concat(additionalAttributes).ToArray, "")
            End If
        Finally
            If keepNode IsNot Nothing Then
                lastNode = keepNode
            End If
        End Try
        Return Me
    End Function

    Public Function Document(Optional language As String = "") As HTMLBuilder
        If Not AddNextNode Then Return Me

        Dim element As New HTMLDocument(Me.DocumentType)
        If Not String.IsNullOrWhiteSpace(language) Then
            element.Attributes.Add(New HTMLAttribute("lang", language))
        End If
        If tree Is Nothing Then
            tree = New HTMLElementNode(Nothing, element)
            lastNode = tree
            AddChildNode = True
        Else
            Throw New Exception("The <html> tag must be the first element in an HTML document")
        End If
        Return Me
    End Function

    Public Function Comment(content As String) As HTMLBuilder
        If Not AddNextNode Then Return Me

        Dim element As New HTMLComment(content)
        If tree Is Nothing Then
            tree = New HTMLElementNode(Nothing, element)
            lastNode = tree
            AddChildNode = True
        Else
            If AddChildNode Then
                lastNode = lastNode.AddChild(element)
            Else
                lastNode = lastNode.Parent.AddChild(element)
            End If
        End If
        Return Me
    End Function

    Public Function Form(name As String, action As String, Optional method As FormMethod = FormMethod.Post, Optional encodingType As FormEncodingType = FormEncodingType.UrlEncoded, Optional autoComplete As Boolean = True, Optional novalidate As Boolean = False, Optional additionalAttributes As HTMLAttribute() = Nothing) As HTMLBuilder
        Dim attributes As New List(Of HTMLAttribute)
        If Not String.IsNullOrWhiteSpace(name) Then
            attributes.Add(New HTMLAttribute("name", name))
        End If
        If Not String.IsNullOrWhiteSpace(action) Then
            attributes.Add(New HTMLAttribute("action", action))
        End If
        Select Case method
            Case FormMethod.Get
                attributes.Add(New HTMLAttribute("method", "get"))
            Case FormMethod.Post
                attributes.Add(New HTMLAttribute("method", "post"))
        End Select
        Select Case encodingType
            Case FormEncodingType.UrlEncoded
                attributes.Add(New HTMLAttribute("enctype", "application/x-www-form-urlencoded"))
            Case FormEncodingType.FormData
                attributes.Add(New HTMLAttribute("enctype", "multipart/form-data"))
            Case FormEncodingType.Plain
                attributes.Add(New HTMLAttribute("enctype", "text/plain	"))
        End Select
        If Not autoComplete Then
            attributes.Add(New HTMLAttribute("autocomplete", "off"))
        End If
        If novalidate Then
            attributes.Add(New HTMLAttribute("novalidate"))
        End If
        If additionalAttributes IsNot Nothing Then
            attributes.AddRange(additionalAttributes)
        End If
        AddElement("form", attributes.ToArray)
        Return Me
    End Function

    Public Function Empty() As HTMLBuilder
        If Not AddNextNode Then Return Me

        Dim element As New HTMLEmpty()
        If tree Is Nothing Then
            tree = New HTMLElementNode(Nothing, element)
            lastNode = tree
            AddChildNode = True
        Else
            If AddChildNode Then
                lastNode = lastNode.AddChild(element)
            Else
                lastNode = lastNode.Parent.AddChild(element)
            End If
        End If
        Return Me
    End Function

#End Region

#Region "Component functions"

    Public Function BeginComponent(componentName As String) As HTMLBuilder
        If componentStack Is Nothing Then
            componentStack = New Stack(Of Tuple(Of String, HTMLElementNode))
        End If
        If tree Is Nothing Then Empty()

        If String.IsNullOrWhiteSpace(componentName) Then
            componentName = "Component"
        End If
        Me.Comment(String.Format("Begin {0}", componentName))
        componentStack.Push(New Tuple(Of String, HTMLElementNode)(componentName, lastNode))
        Return Me
    End Function

    Public Function BeginComponent() As HTMLBuilder
        Return BeginComponent("")
    End Function

    Public Function EndComponent() As HTMLBuilder
        If componentStack.Count > 0 Then
            Dim component = componentStack.Pop
            Dim componentName As String = component.Item1
            lastNode = component.Item2
            AddChildNode = False
            Me.Comment(String.Format("End {0}", componentName))
            Return Me
        Else
            Throw New Exception("There is no component started with BeginComponent left to end")
        End If
    End Function

#End Region

#Region "Output functions"

    Public Overrides Function ToString() As String
        Dim sb As New StringBuilder
        WriteTree(sb, tree)
        Return sb.ToString
    End Function

    Public Function ToHTMLEncodedString() As String
        Return System.Net.WebUtility.HtmlEncode(ToString())
    End Function

    Public Function ToJSON() As String
        Using sw As New IO.StringWriter
            Dim js As New FlowJsonSerializer
            js.Serialize(sw, tree)
            Return sw.ToString
        End Using
    End Function

    Public Sub Write(sb As StringBuilder)
        WriteTree(sb, tree)
    End Sub

    Public Sub Write(fileName As String, append As Boolean, encoding As Text.Encoding)
        Dim sb As New StringBuilder
        WriteTree(sb, tree)
        Using sw As New IO.StreamWriter(fileName, append, encoding)
            sw.Write(sb.ToString)
        End Using
    End Sub

    Public Sub Write(fileName As String, Optional append As Boolean = False)
        Write(fileName, append, New UTF8Encoding)
    End Sub

#End Region

End Class

#End Region