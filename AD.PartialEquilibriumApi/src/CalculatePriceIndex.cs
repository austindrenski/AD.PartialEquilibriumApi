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
                                  .Select(x => (double)x.Attribute("PriceIndex"))
                                  .Sum();

                    priceIndex = Math.Pow(price, 1 / (1 - elasticityOfSubstitution));
                    break;
                }
                case false:
                {
                    double share = market.MarketShare();
                    price = market.InitialPrice();

                    priceIndex = share * Math.Pow(price, 1 - elasticityOfSubstitution);
                    break;
                }
            }

            if (market.Parent == null)
            {
                foreach (XElement item in market.DescendantsAndSelf())
                {
                    item.SetAttributeValue("PriceIndex", priceIndex);
                }
            }

            market.SetAttributeValue("PriceIndex", priceIndex);
            return market;
        }
    }
}
