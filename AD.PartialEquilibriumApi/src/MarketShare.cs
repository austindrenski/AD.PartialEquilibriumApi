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
        private static readonly XName XMarketShare = "MarketShare";

        /// <summary>
        /// Returns the value of the MarketShare attribute.
        /// </summary>
        /// <param name="market">An <see cref="XElement"/> describing a market.</param>
        /// <returns>The value set by the user to the MarketShare attribute.</returns>
        public static double MarketShare([NotNull] this XElement market)
        {
            return (double)market.Attribute(XMarketShare);
        }

        /// <summary>
        /// Sets the value of the MarketShare attribute.
        /// </summary>
        /// <param name="market">An <see cref="XElement"/> describing a market.</param>
        /// <param name="value">The value to which the MarketShare attribute is set.</param>
        public static void MarketShare([NotNull] this XElement market, double value)
        {
            market.SetAttributeValue(XMarketShare, value);
        }
    }
}
