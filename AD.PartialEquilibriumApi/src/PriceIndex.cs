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
    public static class CalculatePriceIndexExtensions
    {
        private static readonly XName XPriceIndex = "PriceIndex";

        /// <summary>
        /// Returns the value of the PriceIndex attribute.
        /// </summary>
        /// <param name="market">An <see cref="XElement"/> describing a market.</param>
        /// <returns>The value set by the user to the "PriceIndex" attribute.</returns>
        public static double PriceIndex([NotNull] this XElement market)
        {
            return (double)market.Attribute(XPriceIndex);
        }

        /// <summary>
        /// Sets the PriceIndex attribute by aggregating the price indices of itself or any sub-markets.
        /// Result = [Σ marketShare[i] * (price[i] ^ (1 - elasticityOfSubstitution[i])] ^ [1 / (1 - elasticityOfSubstitution)]
        /// </summary>
        /// <param name="market">An <see cref="XElement"/> describing a market.</param>
        /// <returns>A reference to the existing <see cref="XElement"/>. This is returned for use with fluent syntax calls.</returns>
        public static XElement CalculatePriceIndex([NotNull] this XElement market)
        {
            double price;
            double priceIndex;
            double elasticityOfSubstitution = market.ElasticityOfSubstitution();

            switch (market.HasElements)
            {
                default:
                {
                    throw new ArgumentException();
                }
                case true:
                {
                    price = market.Elements()
                                  .Select(x => x.CalculatePriceIndex())
                                  .Select(x => x.PriceIndex())
                                  .Sum();

                    priceIndex = Math.Pow(price, 1 / (1 - elasticityOfSubstitution));
                    break;
                }
                case false:
                {
                    double share = market.MarketShare();
                    price = market.CurrentPrice();

                    priceIndex = share * Math.Pow(price, 1 - elasticityOfSubstitution);
                    break;
                }
            }

            if (market.Parent == null)
            {
                foreach (XElement item in market.DescendantsAndSelf())
                {
                    item.SetAttributeValue(XPriceIndex, priceIndex);
                }
            }

            market.SetAttributeValue(XPriceIndex, priceIndex);
            return market;
        }
    }
}
