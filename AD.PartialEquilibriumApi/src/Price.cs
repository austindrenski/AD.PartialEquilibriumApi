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
        private static readonly XName InitialPrice = "InitialPrice";

        /// <summary>
        /// Returns the value of the InitialPrice attribute.
        /// </summary>
        /// <param name="market">An <see cref="XElement"/> describing a market.</param>
        /// <returns>The value set by the user to the "InitialPrice" attribute.</returns>
        public static double GetInitialPrice([NotNull] this XElement market)
        {
            return double.Parse(market.Attribute(InitialPrice)?.Value ?? "0");
        }

        /// <summary>
        /// Sets the value of the InitialPrice attribute.
        /// </summary>
        /// <param name="market">An <see cref="XElement"/> describing a market.</param>
        /// <param name="value">The value to which the InitialPrice attribute is set.</param>
        public static void SetInitialPrice([NotNull] this XElement market, double value)
        {
            market.SetAttributeValue(InitialPrice, value);
        }

        /// <summary>
        /// Returns the calculated price of the node by aggregating the initial prices of itself or any sub-markets.
        /// </summary>
        /// <param name="market">An <see cref="XElement"/> describing a market.</param>
        /// <returns>The price of this node</returns>
        public static double CalculatePrice([NotNull] this XElement market)
        {
            return market.HasElements ? market.Elements()
                                              .Select(x => x.CalculatePrice())
                                              .Aggregate((current, x) => current + x)

                                      : market.GetInitialPrice();
        }
    }
}
