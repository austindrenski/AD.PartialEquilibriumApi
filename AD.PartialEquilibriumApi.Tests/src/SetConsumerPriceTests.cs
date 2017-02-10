using System.Linq;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AD.PartialEquilibriumApi.Tests
{
    [TestClass]
    public class SetConsumerPriceTests
    {
        [TestMethod]
        public void SetConsumerPriceTest_Model0()
        {
            // Arrange
            XElement model = Definitions.CreateModel0();
            XName[] variables =
                new XName[]
                {
                    "Supplier1",
                    "Supplier2",
                };
            double[] values =
                new double[]
                {
                    2.5,
                    3.0
                };
            model.SetIsVariable(variables);

            // Act
            model.SetConsumerPrices(values);

            // Assert
            Assert.IsTrue((double)model.Descendants("Supplier1").Single().Attribute("ConsumerPrice") == 2.5);
            Assert.IsTrue((double)model.Descendants("Supplier2").Single().Attribute("ConsumerPrice") == 3.0);
        }

        [TestMethod]
        public void SetConsumerPriceTest_Model1()
        {
            // Arrange
            XElement model = Definitions.CreateModel1();
            XName[] variables =
                new XName[]
                {
                    "Supplier1",
                    "Supplier2",
                };
            double[] values =
                new double[]
                {
                    2.5,
                    3.0
                };
            model.SetIsVariable(variables);

            // Act
            model.SetConsumerPrices(values);

            // Assert
            Assert.IsTrue((double)model.Descendants("Supplier1").Single().Attribute("ConsumerPrice") == 2.5);
            Assert.IsTrue((double)model.Descendants("Supplier2").Single().Attribute("ConsumerPrice") == 3.0);
        }
    }
}
