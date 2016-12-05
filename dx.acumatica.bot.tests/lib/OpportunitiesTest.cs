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

    [TestClass]
    public class UtilitiesTest
    {
        [TestMethod]
        public void ConvertStringToDateShouldReturnCorrecDateForMonthAndYearString()
        {
            var yearAndMonthString = "January 2018";
            var result = Utilities.ConvertStringToDate(yearAndMonthString, true);
            Assert.AreEqual(DateTime.Parse("1/1/2018"), result);
        }
    }
}
