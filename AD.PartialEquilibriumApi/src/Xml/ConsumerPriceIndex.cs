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
            double elasticityOfSubstitution = market.ElasticityOfSubstitution();

            if (market.HasElements)
            {
                double consumerPrice = market.Elements()
                                             .Select(x => x.InitialMarketShare() * Math.Pow(x.ConsumerPrice(), 1 - elasticityOfSubstitution))
                                             .Sum();

                double consumerPriceIndex = Math.Pow(consumerPrice, 1 / (1 - elasticityOfSubstitution));

                foreach (XElement item in market.Elements())
                {
                    item.SetAttributeValue(XConsumerPriceIndex, consumerPriceIndex);
                }
            }

            //switch (market.HasElements)
            //{
            //    default:
            //    {
            //        throw new ArgumentException();
            //    }
            //    case true:
            //    {
            //        consumerPrice = market.Elements()
            //                              .Select(x => x.InitialMarketShare() * Math.Pow(x.ConsumerPrice(), 1 - elasticityOfSubstitution))
            //                              .Sum();

            //        consumerPriceIndex = Math.Pow(consumerPrice, 1 / (1 - elasticityOfSubstitution));
            //        break;
            //    }
            //    case false:
            //    {
            //        consumerPriceIndex = 1.0;
            //        break;
            //    }
            //}



            if (market.Parent == null || !market.HasElements)
            {
                market.SetAttributeValue(XConsumerPriceIndex, 1.0);
            }
            
            return market;
        }

        /// <summary>
        /// Sets the ConsumerPriceIndex attribute by aggregating the price indices of itself or any sub-markets.
        /// Result = [Σ marketShare[i] * (consumerPrice[i] ^ (1 - elasticityOfSubstitution[i])] ^ [1 / (1 - elasticityOfSubstitution)]
        /// </summary>
        /// <param name="market">An <see cref="XElement"/> describing a market.</param>
        /// <returns>A reference to the existing <see cref="XElement"/>. This is returned for use with fluent syntax calls.</returns>
        public static XElement CalculateRootConsumerPriceIndex([NotNull] this XElement market)
        {
            foreach (XElement item in market.DescendantsAndSelf().Reverse())
            {
                item.CalculateConsumerPriceIndex();
            }
            return market;
        }
    }
}
