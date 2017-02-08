using System.Linq;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AD.PartialEquilibriumApi.Tests
{
    [TestClass]
    public class ConsumerPriceIndexTests
    {
        [TestMethod]
        public void ConsumerPriceIndexTest_Model0()
        {
            // Arrange
            XElement model = Definitions.CreateModel0();
            model.ShockAllProducerPrices();
            double[] expected = new double[]
            {
                1.0
            };

            // Act
            model.CalculateConsumerPriceIndex();
            double[] consumerPriceIndexes = model.DescendantsAndSelf().Where(x => x.HasElements).Select(x => x.ConsumerPriceIndex()).ToArray();

            // Assert
            Assert.IsTrue(expected.SequenceEqual(consumerPriceIndexes));
        }

        [TestMethod]
        public void ConsumerPriceIndexTest_Model1()
        {
            // Arrange
            XElement model = Definitions.CreateModel1();
            model.ShockAllProducerPrices();
            double[] expected = new double[]
            {
                1.0,
                1.0
            };

            // Act
            model.CalculateConsumerPriceIndex();
            double[] consumerPriceIndexes = model.DescendantsAndSelf().Where(x => x.HasElements).Select(x => x.ConsumerPriceIndex()).ToArray();

            // Assert
            Assert.IsTrue(expected.SequenceEqual(consumerPriceIndexes));
        }
    }
}
