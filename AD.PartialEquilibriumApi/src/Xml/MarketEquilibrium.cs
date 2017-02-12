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
        public static XElement CalculateMarketEquilibrium([NotNull] this XElement element)
        {
            foreach (XElement market in element.DescendantsAndSelf().Reverse())
            {
                if (market.HasElements)
                {
                    market.SetAttributeValue(XMarketEquilibrium, market.Elements().Sum(x => x.MarketEquilibrium()));
                    continue;
                }
                if (market.Parent == null)
                {
                    throw new ArgumentNullException("This error shouldn't be thrown. Something has gone wrong with the model state.");
                }
                double consumerConsumerPriceIndex = market.Parent.ConsumerPrice();
                double consumerPrice = market.ConsumerPrice();
                double elasticityOfDemand = market.ElasticityOfDemand();
                double elasticityOfSubstitution = market.ElasticityOfSubstitution();
                double elasticityOfSupply = market.ElasticityOfSupply();
                double producerPrice = market.ProducerPrice();
                
                double marketEquilibrium =
                    Math.Pow(producerPrice, elasticityOfSupply)
                    -
                    Math.Pow(consumerConsumerPriceIndex, elasticityOfSubstitution + elasticityOfDemand)
                    /
                    Math.Pow(consumerPrice, elasticityOfSubstitution);

                market.SetAttributeValue(XMarketEquilibrium, marketEquilibrium);
            }

            return element;
        }
    }
}
