using System.Xml.Linq;
using JetBrains.Annotations;

namespace AD.PartialEquilibriumApi
{
    /// <summary>
    /// Extension methods to access the Shock attribute.
    /// </summary>
    [PublicAPI]
    public static class ShockExtensions
    {
        private static readonly XName XShock = "Shock";

        /// <summary>
        /// Returns the value of the Shock attribute.
        /// </summary>
        /// <param name="market">An <see cref="XElement"/> describing a market.</param>
        /// <returns>The value set by the user to the Shock attribute.</returns>
        public static double Shock([NotNull] this XElement market)
        {
            if (market.Attribute(XShock) == null)
            {
                return 0;
            }
            return (double)market.Attribute(XShock);
        }

        /// <summary>
        /// Sets the value of the Shock attribute.
        /// </summary>
        /// <param name="market">An <see cref="XElement"/> describing a market.</param>
        /// <param name="value">The value to which the Shock attribute is set.</param>
        public static void Shock([NotNull] this XElement market, double value)
        {
            market.SetAttributeValue(XShock, value);
        }
    }
}
