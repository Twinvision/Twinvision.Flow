using System;

namespace Twinvision.Flow.Demos
{
    static class HTMLBuilderDemos
    {
        public static void Main()
        {
            var builder = new HTMLBuilder();
            // builder.Settings.EnforceProperNesting = True
            // builder.Document.Header("Test").Body()
            // For Each element In {"Test", "Test 2"}
            // builder.BeginComponent("Test" + vbCrLf + "test 2").P.Child.Div("component")
            // If True Then
            // builder.AddAttribute("selected")
            // End If
            // builder.Child.A("http://www.google.com", element).BeginComponent("Link 2 " + element).A("http://www.microsoft.com", "Link 2" + element).EndComponent.EndComponent()
            // Next
            // builder.P.Child.A("http://www.twinvision.nl", "Link").Parent()
            // builder.Child().AddElement("textarea", "bla").Child().AddElement("p", "bla").Parent()
            // builder.OnlyWhen(Function() DateTime.Now.Hour < 18).AddElement("p", "Goedemiddag!").OnlyWhen(Function() DateTime.Now.Hour >= 18).AddElement("p", "Goedeavond!").P("Deze altijd")
            // Console.WriteLine(builder.ToString)
            // System.Windows.Forms.Clipboard.SetData(System.Windows.Forms.DataFormats.Text, builder.ToString)
            // Console.ReadLine()
            builder.BeginComponent("Test");
            builder.Div("test2").Child();
            builder.EndComponent();
            builder.Form("Test", "/Form/Test", FormMethod.Post);
            Console.WriteLine(builder.ToString());
            Console.ReadLine();
        }
    }
}
