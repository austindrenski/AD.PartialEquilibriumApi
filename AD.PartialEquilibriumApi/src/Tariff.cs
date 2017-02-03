using System.Xml.Linq;
using JetBrains.Annotations;

namespace AD.PartialEquilibriumApi
{
    /// <summary>
    /// Extension methods to access the Tariff attribute.
    /// </summary>
    [PublicAPI]
    public static class TariffExtensions
    {
        private static readonly XName TariffXName = "Tariff";

        /// <summary>
        /// Returns the value of the Tariff attribute.
        /// </summary>
        /// <param name="market">An <see cref="XElement"/> describing a market.</param>
        /// <returns>The value set by the user to the Tariff attribute.</returns>
        public static double Tariff([NotNull] this XElement market)
        {
            return (double)market.Attribute(TariffXName);
        }

        /// <summary>
        /// Sets the value of the Tariff attribute.
        /// </summary>
        /// <param name="market">An <see cref="XElement"/> describing a market.</param>
        /// <param name="value">The value to which the Tariff attribute is set.</param>
        public static void Tariff([NotNull] this XElement market, double value)
        {
            market.SetAttributeValue(TariffXName, value);
        }
    }
}
