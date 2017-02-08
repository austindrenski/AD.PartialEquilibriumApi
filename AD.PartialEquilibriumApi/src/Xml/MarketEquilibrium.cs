using System;
using System.Linq;
using System.Xml.Linq;
using JetBrains.Annotations;

namespace AD.PartialEquilibriumApi
{
    /// <summary>
    /// Extension methods to calculate the market equilibrium condition.
    /// </summary>
    [PublicAPI]
    public static class CalculateMarketEquilibriumExtensions
    {
        private static readonly XName XMarketEquilibrium = "MarketEquilibrium";

        /// <summary>
        /// Returns the value of the MarketEquilibrium attribute.
        /// </summary>
        /// <param name="element">An <see cref="XElement"/> describing a market.</param>
        /// <returns>The value of the MarketEquilibrium attribute.</returns>
        public static double MarketEquilibrium([NotNull] this XElement element)
        {
            return (double)element.Attribute(XMarketEquilibrium);
        }

        /// <summary>
        /// Sets the MarketEquilibrium attribute on descendant <see cref="XElement"/> objects in reverse document order.
        /// Result = (producerPrice ^ elasticityOfSupply) - [(consumerConsumerPriceIndex ^ (elasticityOfSubstitution + elasticityOfDemand)) / (consumerPrice ^ elasticityOfSubstitution)]
        /// </summary>
        /// <returns>A reference to the existing <see cref="XElement"/>. This is returned for use with fluent syntax calls.</returns>
        public static XElement CalculateRootMarketEquilibrium([NotNull] this XElement element)
        {
            foreach (XElement item in element.DescendantsAndSelf().Reverse())
            {
                if (item.Parent == null)
                {
                    continue;
                }

                double consumerConsumerPriceIndex = item.Parent.ConsumerPrice();
                double consumerPrice = item.ConsumerPrice();
                double elasticityOfDemand = item.ElasticityOfDemand();
                double elasticityOfSubstitution = item.ElasticityOfSubstitution();
                double elasticityOfSupply = item.ElasticityOfSupply();
                double producerPrice = item.ProducerPrice();

                double marketEquilibrium =
                    Math.Pow(producerPrice, elasticityOfSupply)
                    -
                    Math.Pow(consumerConsumerPriceIndex, elasticityOfSubstitution + elasticityOfDemand)
                    /
                    Math.Pow(consumerPrice, elasticityOfSubstitution);

                item.SetAttributeValue(XMarketEquilibrium, marketEquilibrium);
            }

            return element;
        }
    }
}
