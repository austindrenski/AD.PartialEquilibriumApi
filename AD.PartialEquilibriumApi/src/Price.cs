using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using JetBrains.Annotations;

namespace AD.PartialEquilibriumApi
{
    /// <summary>
    /// Extension methods to calculate prices for markets described by XML trees.
    /// </summary>
    [PublicAPI]
    public static class PriceExtensions
    {
        /// <summary>
        /// Returns the calculated price of the node by aggregating the initial prices of itself or any sub-markets.
        /// Result = [Σ marketShare[i] * (price[i] ^ (1 - elasticityOfSubstitution[i])] ^ [1 / (1 - elasticityOfSubstitution)]
        /// </summary>
        /// <param name="market">An <see cref="XElement"/> describing a market.</param>
        /// <returns>The price of this node</returns>
        public static double CalculatePriceIndex([NotNull] this XElement market)
        {
            if (!market.HasElements)
            {
                double initialPrice = market.InitialPrice();
                double si = market.MarketShare();
                double s = market.ElasticityOfSubstitution();
                return si * Math.Pow(initialPrice, 1 - s);
            }

            IEnumerable<double> prices = 
                market.Elements()
                      .Select(x => x.CalculatePriceIndex());

            double elasticityOfSubstitution = market.ElasticityOfSubstitution();

            return Math.Pow(prices.Sum(), 1 / (1 - elasticityOfSubstitution));
        }
    }
}
