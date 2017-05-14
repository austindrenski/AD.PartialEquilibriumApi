using System.Xml.Linq;
using JetBrains.Annotations;

namespace AD.PartialEquilibriumApi.Xml
{
    /// <summary>
    /// Extension methods to access the UpstreamCostShare attribute.
    /// </summary>
    [PublicAPI]
    public static class UpstreamCostShareExtensions
    {
        private static readonly XName XUpstreamCostShare = "UpstreamCostShare";

        /// <summary>
        /// Returns the value of the UpstreamCostShare attribute.
        /// </summary>
        /// <param name="market">An <see cref="XElement"/> describing a market.</param>
        /// <returns>The value set by the user to the UpstreamCostShare attribute.</returns>
        public static double UpstreamCostShare([NotNull] this XElement market)
        {
            if (market.Attribute(XUpstreamCostShare) == null)
            {
                return 0;
            }
            return (double)market.Attribute(XUpstreamCostShare);
        }

        /// <summary>
        /// Sets the value of the UpstreamCostShare attribute.
        /// </summary>
        /// <param name="market">An <see cref="XElement"/> describing a market.</param>
        /// <param name="value">The value to which the UpstreamCostShare attribute is set.</param>
        public static void UpstreamCostShare([NotNull] this XElement market, double value)
        {
            market.SetAttributeValue(XUpstreamCostShare, value);
        }
    }
}
