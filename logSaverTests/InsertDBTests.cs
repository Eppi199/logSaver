using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using logSaver;
using System.Collections.Generic;

namespace logSaverTests
{
    [TestClass]
    public class InsertDBTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            List<logList> testList = new List<logList>();
            testList.Add(
    new logList
    {
        Date = "2017-05-23 12:19:13.8206",
        Status = "Info",
        Event = "Again info",
        MessageV1 = "Additional info",
        MessageV2 = "Message--MessageMessageMessage--Message--MessageMessage"
    });

            bool result = Process.InsertDB(testList);

            Assert.AreEqual(result, true);
        }
    }
}
