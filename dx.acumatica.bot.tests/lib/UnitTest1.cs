using System;
using dx.acumatica.bot.lib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace dx.acumatica.bot.tests.lib
{
    [TestClass]
    public class OpportunitiesTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var opportunities = new Opportunities();
            var result = opportunities.Get().Result;
            Assert.IsNotNull(result);
        }
    }
}
