using System;
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
            double[] values =
                new double[]
                {
                    2.5,
                    3.0
                };

            // Act
            model.SetConsumerPrices(values);

            // Assert
            Assert.IsTrue(Math.Abs((double)model.Descendants("Supplier1").Single().Attribute("ConsumerPrice") - 2.5) < 1e-15);
            Assert.IsTrue(Math.Abs((double)model.Descendants("Supplier2").Single().Attribute("ConsumerPrice") - 3.0) < 1e-15);
        }

        [TestMethod]
        public void SetConsumerPriceTest_Model1()
        {
            // Arrange
            XElement model = Definitions.CreateModel1();
            double[] values =
                new double[]
                {
                    2.5,
                    3.0
                };

            // Act
            model.SetConsumerPrices(values);

            // Assert
            Assert.IsTrue(Math.Abs((double)model.Descendants("Supplier1").Single().Attribute("ConsumerPrice") - 2.5) < 1e-15);
            Assert.IsTrue(Math.Abs((double)model.Descendants("Supplier2").Single().Attribute("ConsumerPrice") - 3.0) < 1e-15);
        }
    }
}
