Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

<TestClass()> Public Class HTMLBuilderTests

    <TestMethod(), TestCategory("Basics")> Public Sub Empty()
        Dim builder As New HTMLBuilder
        Assert.AreEqual("", builder.ToString)
    End Sub

    <TestMethod(), TestCategory("Settings")> Public Sub TabSize8()
        Dim builder As New HTMLBuilder
        builder.Document.Body(content:="Size 8")
        builder.Settings.TabSize = 8
        Assert.AreEqual(My.Resources.AssertEqualTabSize8, builder.ToString)
    End Sub

    <TestMethod(), TestCategory("Settings")> Public Sub SkipComments()
        Dim builder As New HTMLBuilder
        builder.Settings.WriteComments = False
        builder.BeginComponent.Div("component").Child.A("http://www.google.com").EndComponent()
        Assert.AreEqual(My.Resources.AssertSkipComments, builder.ToString)
    End Sub

    <TestMethod(), TestCategory("Basics")> Public Sub HelloWorld()
        Dim builder As New HTMLBuilder
        builder.Document.Body(content:="Hello world!")
        Assert.AreEqual(My.Resources.AssertEqualHelloWorld, builder.ToString)
    End Sub

    <TestMethod(), TestCategory("Settings")> Public Sub PreserveCase()
        Dim builder As New HTMLBuilder
        builder.AddElement("DiV", {New HTMLAttribute("CLASS", "Test")}, "Content")
        builder.Settings.EnforceProperCase = False
        Assert.AreEqual(My.Resources.AssertEqualPreserveCase, builder.ToString)
    End Sub

    <TestMethod(), TestCategory("Basics")> Public Sub DeeplyNested()
        Dim builder As New HTMLBuilder

        For i = 1 To 25
            builder.Child.Div(className:="class" + i.ToString, id:="element" + i.ToString)
        Next
        Assert.AreEqual(My.Resources.AssertEqualDeeplyNested, builder.ToString)
    End Sub

    <TestMethod(), TestCategory("Comments")> Public Sub SingleLineComment()
        Dim builder As New HTMLBuilder
        builder.Document("en").Body.Comment("A comment for a div element").Div(content:="HTML comments test")
        Assert.AreEqual(My.Resources.AssertEqualSingleLineComment, builder.ToString)
    End Sub

    <TestMethod(), TestCategory("Comments")> Public Sub MultiLineComment()
        Dim builder As New HTMLBuilder
        builder.Document("en").Body.Comment("A comment for" + vbCrLf + "a div element").Div(content:="HTML comments test")
        Assert.AreEqual(My.Resources.AssertEqualMultiLineComment, builder.ToString)
    End Sub

    <TestMethod(), TestCategory("Standards")> Public Sub InvalidTagForHTML5()
        Dim builder As New HTMLBuilder, exceptionThrown As Boolean = False
        Try
            builder.AddElement("mytag", "Test")
        Catch ex As Exception
            exceptionThrown = True
            Trace.WriteLine(ex.Message)
        End Try
        Assert.IsTrue(exceptionThrown)
    End Sub

    <TestMethod(), TestCategory("Standards")> Public Sub ValidTagForHTML5()
        Dim builder As New HTMLBuilder, exceptionThrown As Boolean = False
        Try
            builder.AddElement("article", "Test")
        Catch ex As Exception
            exceptionThrown = True
            Trace.WriteLine(ex.Message)
        End Try
        Assert.IsFalse(exceptionThrown)
    End Sub

    <TestMethod(), TestCategory("Standards")> Public Sub InvalidTagForHTML401Strict()
        Dim builder As New HTMLBuilder(HTMLDocumentType.HTML4_01_Strict), exceptionThrown As Boolean = False
        Try
            builder.AddElement("article", "Test")
        Catch ex As Exception
            exceptionThrown = True
            Trace.WriteLine(ex.Message)
        End Try
        Assert.IsTrue(exceptionThrown)
    End Sub

    <TestMethod(), TestCategory("Standards")> Public Sub InvalidCharactersInTagName()
        Dim builder As New HTMLBuilder, exceptionThrown As Boolean = False
        Try
            builder.Settings.EnforceDocType = False
            builder.AddElement("m3t@", "Test")
        Catch ex As Exception
            exceptionThrown = True
            Trace.WriteLine(ex.Message)
        End Try
        Assert.IsTrue(exceptionThrown)
    End Sub

    <TestMethod(), TestCategory("Settings")> Public Sub EnforceProperNesting()
        Dim builder As New HTMLBuilder, exceptionThrown As Boolean = False
        Try
            builder.AddElement("p", "").Child.AddElement("li")
        Catch ex As Exception
            exceptionThrown = True
            Trace.WriteLine(ex.Message)
        End Try

        Assert.IsTrue(exceptionThrown)
    End Sub

    <TestMethod(), TestCategory("Performance")> Public Sub LargeHTMLDocument()
        Dim builder As New HTMLBuilder
        builder.Settings.EnforceProperNesting = False
        builder.Document.Body.P.Child.AddElement("div")
        For i As Integer = 1 To 1000
            builder.AddElement("p", "List element " + i.ToString).Child()
        Next
        Dim s As String = builder.ToString()
    End Sub

    <TestMethod(), TestCategory("Standards")> Public Sub SelfClosing()
        Dim builder As New HTMLBuilder
        builder.Settings.EnforceProperNesting = False
        builder.Empty()
        builder.AddElement("script", {New HTMLAttribute("src", "test.js")}, "")
        Dim s As String = builder.ToString()
    End Sub

End Class