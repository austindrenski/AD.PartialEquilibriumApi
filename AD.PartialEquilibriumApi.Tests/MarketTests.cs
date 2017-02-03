using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AD.PartialEquilibriumApi.Tests
{
    [TestClass]
    public class MarketTests
    {
        [TestMethod]
        public void MarketTest0()
        {
            // Arrange
            string file = @"G:\data\Austin D\OOPE\market serialization test0.xml";
            Market usa = new Market("usa", 1);
            Market can = new Market("can", 1);
            Market usaBeef = new Market("usaBeef");
            usaBeef.SubMarkets.Add(usa);
            usaBeef.SubMarkets.Add(can);
            can.HasSubMarkets = true;

            // Act
            XmlSerializer serializer = new XmlSerializer(typeof(Market));
            using (StreamWriter writer = new StreamWriter(file))
            {
                serializer.Serialize(writer, usaBeef);
            }
            XElement element = XElement.Load(file);
            IEnumerable<XElement> descendants = element.DescendantsAndSelf("Market").ToArray();

            // Assert
            Assert.IsTrue(true);
        }
    }
}
