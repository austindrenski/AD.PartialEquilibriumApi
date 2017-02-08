using System;
using System.Linq;
using System.Xml.Linq;
using JetBrains.Annotations;

namespace AD.PartialEquilibriumApi
{
    /// <summary>
    /// Extension methods to calculate the final market share.
    /// </summary>
    [PublicAPI]
    public static class FinalMarketShareExtensions
    {
        private static readonly XName XFinalMarketShare = "FinalMarketShare";

        /// <summary>
        /// Returns the value of the FinalMarketShare attribute.
        /// </summary>
        /// <param name="market">An <see cref="XElement"/> describing a market.</param>
        /// <returns>The value set by the user to the FinalMarketShare attribute.</returns>
        public static double FinalMarketShare([NotNull] this XElement market)
        {
            return (double)market.Attribute(XFinalMarketShare);
        }

        /// <summary>
        /// Sets the value of the FinalMarketShare attribute.
        /// </summary>
        /// <param name="market">An <see cref="XElement"/> describing a market.</param>
        /// <param name="value">The value to which the FinalMarketShare attribute is set.</param>
        public static void FinalMarketShare([NotNull] this XElement market, double value)
        {
            market.SetAttributeValue(XFinalMarketShare, value);
        }

        /// <summary>
        /// Calculates the market share based on current information.
        /// </summary>
        /// <param name="market">An <see cref="XElement"/> describing a market.</param>
        public static void CalculateFinalMarketShare([NotNull] this XElement market)
        {
            double marketShare = market.InitialMarketShare();

            double expenditure = Math.Pow(market.ConsumerPrice(), 1 - market.ElasticityOfSubstitution());

            double totalExpenditure = market.Parent?
                                            .Elements()
                                            .Select(x => x.ConsumerPriceIndex() * x.InitialMarketShare())
                                            .Sum() ?? 1;

            double substitutionAdjustedTotalExpenditure = Math.Pow(totalExpenditure, 1 - market.ElasticityOfSubstitution());

            market.FinalMarketShare(marketShare * expenditure / substitutionAdjustedTotalExpenditure);
        }

        /// <summary>
        /// Calculates the market share based on current information.
        /// </summary>
        /// <param name="market">An <see cref="XElement"/> describing a market.</param>
        public static void CalculateAllFinalMarketShares([NotNull] this XElement market)
        {
            foreach (XElement item in market.DescendantsAndSelf().Reverse())
            {
                item.CalculateFinalMarketShare();
            }
        }
    }
}
