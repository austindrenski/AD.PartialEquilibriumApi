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
        /// Calculates the market share on this and descendant <see cref="XElement"/> objects in reverse document order.
        /// </summary>
        /// <param name="model">An <see cref="XElement"/> describing the model.</param>
        /// <returns>A reference to the existing <see cref="XElement"/>. This is returned for use with fluent syntax calls.</returns>
        public static XElement CalculateFinalMarketShares([NotNull] this XElement model)
        {
            foreach (XElement market in model.DescendantsAndSelf().Reverse())
            {
                if (market.Parent == null)
                {
                    market.SetAttributeValue(XFinalMarketShare, market.Elements().Sum(x => x.FinalMarketShare()));
                    continue;
                }

                double totalExpenditure =
                    market.Parent
                          .Elements()
                          .Sum(x => x.ConsumerPrice() * x.InitialMarketShare());

                double expenditure =
                    Math.Pow(market.ConsumerPrice(), 1 - market.ElasticityOfSubstitution());

                double initialMarketShare = market.InitialMarketShare();

                double substitutionAdjustedTotalExpenditure = 
                    Math.Pow(totalExpenditure, 1 - market.ElasticityOfSubstitution());

                double finalMarketShare = initialMarketShare * expenditure / substitutionAdjustedTotalExpenditure;

                market.SetAttributeValue(XFinalMarketShare, finalMarketShare);
            }

            return model;
        }
    }
}