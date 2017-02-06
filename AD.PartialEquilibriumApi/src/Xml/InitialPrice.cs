using System.Xml.Linq;
using JetBrains.Annotations;

namespace AD.PartialEquilibriumApi
{
    /// <summary>
    /// Extension methods to access the InitialPrice attribute.
    /// </summary>
    [PublicAPI]
    public static class InitialPriceExtensions
    {
        private static readonly XName XInitialPrice = "InitialPrice";

        /// <summary>
        /// Returns the value of the InitialPrice attribute.
        /// </summary>
        /// <param name="market">An <see cref="XElement"/> describing a market.</param>
        /// <returns>The value set by the user to the "InitialPrice" attribute.</returns>
        public static double InitialPrice([NotNull] this XElement market)
        {
            return (double)market.Attribute(XInitialPrice);
        }

        /// <summary>
        /// Sets the value of the InitialPrice attribute.
        /// </summary>
        /// <param name="market">An <see cref="XElement"/> describing a market.</param>
        /// <param name="value">The value to which the InitialPrice attribute is set.</param>
        public static void InitialPrice([NotNull] this XElement market, double value)
        {
            market.SetAttributeValue(XInitialPrice, value);
            market.ConsumerPrice(value);
        }
    }
}
