using System;
using System.Linq;
using System.Xml.Linq;
using JetBrains.Annotations;

namespace AD.PartialEquilibriumApi.Xml
{
    /// <summary>
    /// Extension methods to calculate the final market share.
    /// </summary>
    [PublicAPI]
    public static class FinalMarketShareExtensions
    {
        private static readonly XName XMarketShare = "MarketShare";

        /// <summary>
        /// Returns the value of the MarketShare attribute.
        /// </summary>
        /// <param name="market">An <see cref="XElement"/> describing a market.</param>
        /// <returns>The value set by the user to the MarketShare attribute.</returns>
        public static double MarketShare([NotNull] this XElement market)
        {
            if (market.Attribute(XMarketShare) == null)
            {
                market.SetAttributeValue(XMarketShare, market.InitialMarketShare());
            }
            return (double)market.Attribute(XMarketShare);
        }

        /// <summary>
        /// Calculates market shares on this and descendant <see cref="XElement"/> objects in reverse document order.
        /// Result = [γ_i * price_i^(1-σ_i)] / Σ_j [γ_j * price_j^(1-σ_j)].
        /// </summary>
        /// <param name="model">An <see cref="XElement"/> describing the model.</param>
        /// <returns>A reference to the existing <see cref="XElement"/>. This is returned for use with fluent syntax calls.</returns>
        public static XElement CalculateMarketShares([NotNull] this XElement model)
        {
            foreach (XElement market in model.DescendantsAndSelf().Reverse())
            {
                double expenditure = 
                    market.MarketShare() * Math.Pow(market.ConsumerPrice(), 1 - market.ElasticityOfSubstitution());

                double totalExpenditure = 
                    market.Parent?
                          .Elements()
                          .Sum(x => x.MarketShare() * Math.Pow(x.ConsumerPrice(), 1 - x.ElasticityOfSubstitution())) 
                    ??
                    market.MarketShare() * Math.Pow(market.ConsumerPrice(), 1 - market.ElasticityOfSubstitution());

                double marketShare = 
                    expenditure / totalExpenditure;

                market.SetAttributeValue(XMarketShare, marketShare);
            }

            return model;
        }
    }
}