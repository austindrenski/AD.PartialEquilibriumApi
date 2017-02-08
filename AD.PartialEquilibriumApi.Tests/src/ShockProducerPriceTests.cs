using System.Linq;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AD.PartialEquilibriumApi.Tests
{
    [TestClass]
    public class ShockProducerPriceTests
    {
        [TestMethod]
        public void ShockProducerPriceTest_Model0()
        {
            // Arrange
            XElement model = Definitions.CreateModel0();
            double[] expected = new double[] { 1.0, 1 / 1.05 };

            // Act
            model.ShockProducerPrices();
            double[] producerPrices = model.Descendants().Select(x => x.ProducerPrice()).ToArray();

            // Assert
            Assert.IsTrue(expected.SequenceEqual(producerPrices));
        }

        [TestMethod]
        public void ShockProducerPriceTest_Model1()
        {
            // Arrange
            XElement model = Definitions.CreateModel1();
            double[] expected = new double[] { 1.0, 1.0, 1 / 1.05, 1 / 1.05 };

            // Act
            model.ShockProducerPrices();
            double[] producerPrices = model.Descendants().Select(x => x.ProducerPrice()).ToArray();

            // Assert
            Assert.IsTrue(expected.SequenceEqual(producerPrices));
        }
    }
}
