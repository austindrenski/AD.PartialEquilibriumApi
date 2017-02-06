using System;
using System.Linq;
using System.Xml.Linq;
using JetBrains.Annotations;

namespace AD.PartialEquilibriumApi
{
    /// <summary>
    /// Extension methods to calculate prices for markets described by XML trees.
    /// </summary>
    [PublicAPI]
    public static class CalculateConsumerPriceIndexExtensions
    {
        private static readonly XName XConsumerPriceIndex = "ConsumerPriceIndex";

        /// <summary>
        /// Returns the value of the ConsumerPriceIndex attribute.
        /// </summary>
        /// <param name="market">An <see cref="XElement"/> describing a market.</param>
        /// <returns>The value set by the user to the "ConsumerPriceIndex" attribute.</returns>
        public static double ConsumerPriceIndex([NotNull] this XElement market)
        {
            return (double)market.Attribute(XConsumerPriceIndex);
        }

        /// <summary>
        /// Sets the ConsumerPriceIndex attribute by aggregating the price indices of itself or any sub-markets.
        /// Result = [Σ marketShare[i] * (consumerPrice[i] ^ (1 - elasticityOfSubstitution[i])] ^ [1 / (1 - elasticityOfSubstitution)]
        /// </summary>
        /// <param name="market">An <see cref="XElement"/> describing a market.</param>
        /// <returns>A reference to the existing <see cref="XElement"/>. This is returned for use with fluent syntax calls.</returns>
        public static XElement CalculateConsumerPriceIndex([NotNull] this XElement market)
        {
            double consumerPrice;
            double consumerPriceIndex;
            double elasticityOfSubstitution = market.ElasticityOfSubstitution();

            switch (market.HasElements)
            {
                default:
                {
                    throw new ArgumentException();
                }
                case true:
                {
                    consumerPrice = market.Elements()
                                          .Select(x => x.CalculateConsumerPriceIndex())
                                          .Select(x => x.ConsumerPriceIndex())
                                          .Sum();

                    consumerPriceIndex = Math.Pow(consumerPrice, 1 / (1 - elasticityOfSubstitution));
                    break;
                }
                case false:
                {
                    double share = market.InitialMarketShare();
                    consumerPrice = market.ConsumerPrice();

                    consumerPriceIndex = share * Math.Pow(consumerPrice, 1 - elasticityOfSubstitution);
                    break;
                }
            }

            if (market.Parent == null)
            {
                foreach (XElement item in market.DescendantsAndSelf())
                {
                    item.SetAttributeValue(XConsumerPriceIndex, consumerPriceIndex);
                }
            }

            market.SetAttributeValue(XConsumerPriceIndex, consumerPriceIndex);
            return market;
        }
    }
}
