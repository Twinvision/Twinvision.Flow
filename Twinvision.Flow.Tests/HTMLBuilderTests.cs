using Microsoft.VisualBasic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;

namespace Twinvision.Flow.Tests
{

    [TestClass()]
    public class HTMLBuilderTests
    {
        private string ConvertResourceStringToCurrentEnvironment(string source)
        {
            return source.Replace("\r\n", Environment.NewLine);
        }

        [TestMethod()]
        [TestCategory("Basics")]
        public void Empty()
        {
            var builder = new HTMLBuilder();
            Assert.AreEqual("", builder.ToString());
        }

        [TestMethod()]
        [TestCategory("Settings")]
        public void TabSize8()
        {
            var builder = new HTMLBuilder();
            builder.Document().Body(content: "Size 8");
            builder.Settings.TabSize = 8;
            Assert.AreEqual(ConvertResourceStringToCurrentEnvironment(Test.Resources.AssertEqualTabSize8), builder.ToString());
        }

        [TestMethod()]
        [TestCategory("Settings")]
        public void SkipComments()
        {
            var builder = new HTMLBuilder();
            builder.Settings.WriteComments = false;
            builder.BeginComponent().Div("component").Child().A("http://www.google.com").EndComponent();
            Assert.AreEqual(ConvertResourceStringToCurrentEnvironment(Test.Resources.AssertSkipComments), builder.ToString());
        }

        [TestMethod()]
        [TestCategory("Basics")]
        public void HelloWorld()
        {
            var builder = new HTMLBuilder();
            builder.Document().Body(content: "Hello world!");
            Assert.AreEqual(ConvertResourceStringToCurrentEnvironment(Test.Resources.AssertEqualHelloWorld), builder.ToString());
        }

        [TestMethod()]
        [TestCategory("Settings")]
        public void PreserveCase()
        {
            var builder = new HTMLBuilder();
            builder.AddElement("DiV", new[] { new HTMLAttribute("CLASS", "Test") }, "Content");
            builder.Settings.EnforceProperCase = false;
            Assert.AreEqual(ConvertResourceStringToCurrentEnvironment(Test.Resources.AssertEqualPreserveCase), builder.ToString());
        }

        [TestMethod()]
        [TestCategory("Settings")]
        public void DoNotPreserveCase()
        {
            var builder = new HTMLBuilder();
            builder.AddElement("DiV", new[] { new HTMLAttribute("CLASS", "Test") }, "Content");
            Assert.AreEqual(ConvertResourceStringToCurrentEnvironment(Test.Resources.AssertDoNotPreserveCase), builder.ToString());
        }


        [TestMethod()]
        [TestCategory("Basics")]
        public void DeeplyNested()
        {
            var builder = new HTMLBuilder();

            for (int i = 1; i <= 25; i++)
            {
                builder.Child().Div(className: "class" + i.ToString(), id: "element" + i.ToString());
            }

            Assert.AreEqual(ConvertResourceStringToCurrentEnvironment(Test.Resources.AssertEqualDeeplyNested), builder.ToString());
        }

        [TestMethod()]
        [TestCategory("Comments")]
        public void SingleLineComment()
        {
            var builder = new HTMLBuilder();
            builder.Document("en").Body().Comment("A comment for a div element").Div(content: "HTML comments test");
            Assert.AreEqual(ConvertResourceStringToCurrentEnvironment(Test.Resources.AssertEqualSingleLineComment), builder.ToString());
        }

        [TestMethod()]
        [TestCategory("Comments")]
        public void MultiLineComment()
        {
            var builder = new HTMLBuilder();
            builder.Document("en").Body().Comment("A comment for" + Environment.NewLine + "a div element").Div(content: "HTML comments test");
            Assert.AreEqual(ConvertResourceStringToCurrentEnvironment(Test.Resources.AssertEqualMultiLineComment), builder.ToString());
        }

        [TestMethod()]
        [TestCategory("Standards")]
        public void InvalidTagForHTML5()
        {
            var builder = new HTMLBuilder(HTMLDocumentType.HTML5);
            bool exceptionThrown = false;
            try
            {
                builder.AddElement("mytag", "Test");
            }
            catch (Exception ex)
            {
                exceptionThrown = true;
                Trace.WriteLine(ex.Message);
            }
            Assert.IsTrue(exceptionThrown);
        }

        [TestMethod()]
        [TestCategory("Standards")]
        public void ValidTagForHTML5()
        {
            var builder = new HTMLBuilder();
            bool exceptionThrown = false;
            try
            {
                builder.AddElement("article", "Test");
            }
            catch (Exception ex)
            {
                exceptionThrown = true;
                Trace.WriteLine(ex.Message);
            }
            Assert.IsFalse(exceptionThrown);
        }

        [TestMethod()]
        [TestCategory("Standards")]
        public void InvalidTagForHTML401Strict()
        {
            var builder = new HTMLBuilder(HTMLDocumentType.HTML4_01_Strict);
            bool exceptionThrown = false;
            try
            {
                builder.AddElement("article", "Test");
            }
            catch (Exception ex)
            {
                exceptionThrown = true;
                Trace.WriteLine(ex.Message);
            }
            Assert.IsTrue(exceptionThrown);
        }

        [TestMethod()]
        [TestCategory("Standards")]
        public void InvalidCharactersInTagName()
        {
            var builder = new HTMLBuilder();
            bool exceptionThrown = false;
            try
            {
                builder.Settings.EnforceDocType = false;
                builder.AddElement("m3t@", "Test");
            }
            catch (Exception ex)
            {
                exceptionThrown = true;
                Trace.WriteLine(ex.Message);
            }
            Assert.IsTrue(exceptionThrown);
        }

        [TestMethod()]
        [TestCategory("Settings")]
        public void EnforceProperNesting()
        {
            var builder = new HTMLBuilder();
            bool exceptionThrown = false;
            try
            {
                builder.AddElement("p", "").Child().AddElement("li");
            }
            catch (Exception ex)
            {
                exceptionThrown = true;
                Trace.WriteLine(ex.Message);
            }

            Assert.IsTrue(exceptionThrown);
        }

        [TestMethod()]
        [TestCategory("Performance")]
        public void LargeHTMLDocument()
        {
            var builder = new HTMLBuilder();
            builder.Settings.EnforceProperNesting = false;
            builder.Document().Body().P().Child().AddElement("div");
            for (int i = 1; i <= 1000; i++)
            {
                builder.AddElement("p", "List element " + i.ToString()).Child();
            }
            Assert.AreEqual(builder.ToString().Split('\n').Length, 3007);
        }

        [TestMethod()]
        [TestCategory("Standards")]
        public void NoSelfClosing()
        {
            var builder = new HTMLBuilder();
            builder.Settings.EnforceProperNesting = false;
            builder.Empty();
            builder.AddElement("script", new[] { new HTMLAttribute("src", "test.js") }, "");
            Assert.AreEqual(ConvertResourceStringToCurrentEnvironment(Test.Resources.AssertNoSelfClosing), builder.ToString());
        }

        [TestMethod()]
        [TestCategory("Standards")]
        public void SelfClosing()
        {
            var builder = new HTMLBuilder(HTMLDocumentType.HTML5);
            builder.Settings.EnforceProperNesting = true;
            builder.Empty();
            builder.AddElement("br", "");
            Assert.AreEqual(ConvertResourceStringToCurrentEnvironment(Test.Resources.AssertSelfClosing), builder.ToString());
        }

        [TestMethod()]
        [TestCategory("Standards")]
        public void CustomRootElement()
        {
            var builder = new HTMLBuilder(HTMLDocumentType.HTML5, new HTMLEmpty());
            builder.Settings.EnforceProperNesting = true;
            builder.AddElement("br", "");
            Assert.AreEqual(ConvertResourceStringToCurrentEnvironment(Test.Resources.AssertSelfClosing), builder.ToString());
        }

        [TestMethod()]
        [TestCategory("Basics")]
        public void CreateAComponent()
        {
            var builder = new HTMLBuilder();

            builder.Document().Body()
                   .H(1, "Component example")
                   .BeginComponent("My Component")
                        .Div("component")
                            .Child()
                                .AddElement("span", "Extra component content")
                            .Parent()
                   .EndComponent();
            Assert.AreEqual(ConvertResourceStringToCurrentEnvironment(Test.Resources.AssertCreateComponent), builder.ToString());
        }

        [TestMethod]
        [TestCategory("Basics")]
        public void CreateTableFromList()
        {
            var builder = new HTMLBuilder();
            builder.Table(TestRecordData.ListRecords, "Test", "Test data", new HTMLAttribute[] { new HTMLAttribute("style", "width:100%") });
            Debug.WriteLine(builder.ToString());
        }

        [TestMethod]
        [TestCategory("Basics")]
        public void CreateTableFromDataTable()
        {
            var builder = new HTMLBuilder();
            builder.Table(TestRecordData.TableRecords(), "Test", "Test data", new HTMLAttribute[] { new HTMLAttribute("style", "width:100%") });
            Debug.WriteLine(builder.ToString());
        }

        [TestMethod]
        [TestCategory("Basics")]
        public void AddChildrenInLambda()
        {
            var builder = new HTMLBuilder();
            builder.AddElement("div", new HTMLAttribute[] { new HTMLAttribute("class", "parent") });
            builder.Child(() =>
            {
                builder.AddElement("div", "Child Level 1");
                builder.AddAttribute("class", "child");
                builder.Child(() =>
                {
                    builder.H(2, "Child Level 2");
                });
            });
            Debug.WriteLine(builder.ToString());
        }
    }
}
