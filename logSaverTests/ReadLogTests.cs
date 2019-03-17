using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using logSaver;
using System.Collections.Generic;

namespace logSaverTests
{
    [TestClass]
    public class ReadLogTests
    {
        [TestMethod]
        public void ReadLogTest_String_List()
        {
            List<logList> expectedList = new List<logList>();
            expectedList.Add(
                new logList
                {
                    Date = "2017-05-23 12:19:13.8206",
                    Status = "Info",
                    Event = "Again info",
                    MessageV1 = "Additional info",
                    MessageV2 = "Message--MessageMessageMessage--Message--MessageMessage"
                });

            Process proc = new Process();
            List<logList> testList = proc.ReadLog(@"C:\Users\Александр\Desktop\TestTest.log");

            Assert.AreEqual(expectedList[0].Date, testList[0].Date);
            Assert.AreEqual(expectedList[0].Event, testList[0].Event);
            Assert.AreEqual(expectedList[0].Status, testList[0].Status);
            Assert.AreEqual(expectedList[0].MessageV1, testList[0].MessageV1);
            Assert.AreEqual(expectedList[0].MessageV2, testList[0].MessageV2);
        }
    }
}
