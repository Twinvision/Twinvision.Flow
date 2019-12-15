using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test;

namespace Twinvision.Flow.Tests
{
    [TestClass]
    public class HTMLParserTests
    {
        [TestMethod]
        public void TestParser()
        {
            var parser = new HTMLBuilder();
            var s = parser.Parse(Resources.AssertCreateComponent).ToString();

        }
    }
}
