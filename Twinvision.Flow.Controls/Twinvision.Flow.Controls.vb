Public Interface IDependency
    Property Path As String
    Property Dependencies As IEnumerable(Of IDependency)
End Interface

Public MustInherit Class Dependency
    Implements IDependency

    Sub New(path As String)
        Me.Path = path
    End Sub

    Sub New(path As String, dependencies As IEnumerable(Of Dependency))
        Me.Path = path
        Me.Dependencies = dependencies
    End Sub

    Public Property Path As String Implements IDependency.Path
    Property Dependencies As IEnumerable(Of IDependency) Implements IDependency.Dependencies
End Class

Public Class CssDependency
    Inherits Dependency

    Sub New(path As String)
        MyBase.New(path)
    End Sub

    Sub New(path As String, dependencies As IEnumerable(Of Dependency))
        MyBase.New(path, dependencies)
    End Sub
End Class

Public Class JsDependency
    Inherits Dependency

    Sub New(path As String)
        MyBase.New(path)
    End Sub

    Sub New(path As String, dependencies As IEnumerable(Of Dependency))
        MyBase.New(path, dependencies)
    End Sub
End Class

Public Interface IControl
    Property Name As String
    Property Id As String
    Property JsDependencies As IEnumerable(Of JsDependency)
    Property CssDependencies As IEnumerable(Of CssDependency)
End Interface

Public MustInherit Class Control
    Inherits Twinvision.Flow.HTMLElement
    Implements IControl

    Sub New(tag As String, content As String)
        MyBase.New(tag, content)
        CssDependencies = New List(Of CssDependency)
        JsDependencies = New List(Of JsDependency)
    End Sub

    Public Property Id As String Implements IControl.Id
    Public Property Name As String Implements IControl.Name

    Public Property CssDependencies As IEnumerable(Of CssDependency) Implements IControl.CssDependencies
    Public Property JsDependencies As IEnumerable(Of JsDependency) Implements IControl.JsDependencies
End Class

Public Class Select2
    Inherits Control

    Sub New(id As String)
        MyBase.New("select", "")
        JsDependencies = {New JsDependency("select2", {New JsDependency("jquery")})}
        CssDependencies = {New CssDependency("select2")}
    End Sub

End Class