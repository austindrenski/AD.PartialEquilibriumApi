using System.Xml.Linq;
using JetBrains.Annotations;

namespace AD.PartialEquilibriumApi
{
    /// <summary>
    /// Extension methods to access the MarketShare attribute.
    /// </summary>
    [PublicAPI]
    public static class MarketShareExtensions
    {
        private static readonly XName XInitialMarketShare = "InitialMarketShare";

        /// <summary>
        /// Returns the value of the InitialMarketShare attribute.
        /// </summary>
        /// <param name="market">An <see cref="XElement"/> describing a market.</param>
        /// <returns>The value set by the user to the InitialMarketShare attribute.</returns>
        public static double InitialMarketShare([NotNull] this XElement market)
        {
            return (double)market.Attribute(XInitialMarketShare);
        }

        /// <summary>
        /// Sets the value of the InitialMarketShare attribute.
        /// </summary>
        /// <param name="market">An <see cref="XElement"/> describing a market.</param>
        /// <param name="value">The value to which the InitialMarketShare attribute is set.</param>
        public static void InitialMarketShare([NotNull] this XElement market, double value)
        {
            market.SetAttributeValue(XInitialMarketShare, value);
        }
    }
}
