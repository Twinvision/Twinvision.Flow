Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Public Module LinqToTreeEnumerableExtensions

    ''' <summary>
    ''' Applies the given function to each of the items in the supplied
    ''' IEnumerable.
    ''' </summary>
    <System.Runtime.CompilerServices.Extension> _
    Private Iterator Function DrillDown(items As IEnumerable(Of HTMLElementNode), [function] As Func(Of HTMLElementNode, IEnumerable(Of HTMLElementNode))) As IEnumerable(Of HTMLElementNode)
        For Each item As HTMLElementNode In items
            For Each itemChild As HTMLElementNode In [function](item)
                Yield itemChild
            Next
        Next
    End Function

    ''' <summary>
    ''' Returns a collection of descendant elements.
    ''' </summary>
    <System.Runtime.CompilerServices.Extension> _
    Public Function Descendants(items As IEnumerable(Of HTMLElementNode)) As IEnumerable(Of HTMLElementNode)

        Return items.DrillDown(Function(i) i.Descendants())
    End Function

    ''' <summary>
    ''' Returns a collection containing this element and all descendant elements.
    ''' </summary>
    <System.Runtime.CompilerServices.Extension> _
    Public Function DescendantsAndSelf(items As IEnumerable(Of HTMLElementNode)) As IEnumerable(Of HTMLElementNode)
        Return items.DrillDown(Function(i) i.DescendantsAndSelf())
    End Function

    ''' <summary>
    ''' Returns a collection of ancestor elements.
    ''' </summary>
    <System.Runtime.CompilerServices.Extension> _
    Public Function Ancestors(items As IEnumerable(Of HTMLElementNode)) As IEnumerable(Of HTMLElementNode)
        Return items.DrillDown(Function(i) i.Ancestors())
    End Function

    ''' <summary>
    ''' Returns a collection containing this element and all ancestor elements.
    ''' </summary>
    <System.Runtime.CompilerServices.Extension> _
    Public Function AncestorsAndSelf(items As IEnumerable(Of HTMLElementNode)) As IEnumerable(Of HTMLElementNode)
        Return items.DrillDown(Function(i) i.AncestorsAndSelf())
    End Function

    ''' <summary>
    ''' Returns a collection of child elements.
    ''' </summary>
    <System.Runtime.CompilerServices.Extension> _
    Public Function Elements(items As IEnumerable(Of HTMLElementNode)) As IEnumerable(Of HTMLElementNode)
        Return items.DrillDown(Function(i) i.Elements())
    End Function

    ''' <summary>
    ''' Returns a collection containing this element and all child elements.
    ''' </summary>
    <System.Runtime.CompilerServices.Extension> _
    Public Function ElementsAndSelf(items As IEnumerable(Of HTMLElementNode)) As IEnumerable(Of HTMLElementNode)
        Return items.DrillDown(Function(i) i.ElementsAndSelf())
    End Function

End Module

''' <summary>
''' Defines extension methods for querying an ILinqTree
''' </summary>
Public Module LinqToTreeExtensions

#Region "primary Linq methods"

    ''' <summary>
    ''' Returns a collection of descendant elements.
    ''' </summary>
    <System.Runtime.CompilerServices.Extension> _
    Public Iterator Function Descendants(adapter As HTMLElementNode) As IEnumerable(Of HTMLElementNode)
        For Each child As HTMLElementNode In adapter.Children()
            Yield child

            For Each grandChild As HTMLElementNode In child.Descendants()
                Yield grandChild
            Next
        Next
    End Function

    ''' <summary>
    ''' Returns a collection of ancestor elements.
    ''' </summary>
    <System.Runtime.CompilerServices.Extension> _
    Public Iterator Function Ancestors(adapter As HTMLElementNode) As IEnumerable(Of HTMLElementNode)
        Dim parent = adapter.Parent
        While parent IsNot Nothing AndAlso parent IsNot parent.Parent
            Yield parent
            parent = parent.Parent
        End While
    End Function

    ''' <summary>
    ''' Returns a collection of child elements.
    ''' </summary>
    <System.Runtime.CompilerServices.Extension> _
    Public Iterator Function Elements(adapter As HTMLElementNode) As IEnumerable(Of HTMLElementNode)
        For Each child As HTMLElementNode In adapter.Children()
            Yield child
        Next
    End Function

#End Region

#Region "'AndSelf' implementations"

    ''' <summary>
    ''' Returns a collection containing this element and all child elements.
    ''' </summary>
    <System.Runtime.CompilerServices.Extension> _
    Public Iterator Function ElementsAndSelf(adapter As HTMLElementNode) As IEnumerable(Of HTMLElementNode)
        Yield adapter
        For Each child As HTMLElementNode In adapter.Elements()
            Yield child
        Next
    End Function

    ''' <summary>
    ''' Returns a collection of ancestor elements.
    ''' </summary>
    <System.Runtime.CompilerServices.Extension> _
    Public Iterator Function AncestorsAndSelf(adapter As HTMLElementNode) As IEnumerable(Of HTMLElementNode)
        Yield adapter
        For Each child As HTMLElementNode In adapter.Ancestors()
            Yield child
        Next
    End Function

    ''' <summary>
    ''' Returns a collection containing this element and all descendant elements.
    ''' </summary>
    <System.Runtime.CompilerServices.Extension> _
    Public Iterator Function DescendantsAndSelf(adapter As HTMLElementNode) As IEnumerable(Of HTMLElementNode)
        Yield adapter
        For Each child As HTMLElementNode In adapter.Descendants()
            Yield child
        Next
    End Function

#End Region

#Region "Method which take type parameters"

    ''' <summary>
    ''' Returns a collection of descendant elements.
    ''' </summary>
    <System.Runtime.CompilerServices.Extension> _
    Public Function Descendants(Of T)(adapter As HTMLElementNode) As IEnumerable(Of HTMLElementNode)
        Return adapter.Descendants().Where(Function(i) TypeOf i.Item Is T)
    End Function

#End Region

End Module
