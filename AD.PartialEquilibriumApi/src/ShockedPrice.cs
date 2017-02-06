using System.Linq;
using System.Xml.Linq;
using JetBrains.Annotations;

namespace AD.PartialEquilibriumApi
{
    /// <summary>
    /// Extension methods to appy tariff shocks to initial prices.
    /// </summary>
    [PublicAPI]
    public static class ShockPriceExtensions
    {
        private static readonly XName XShockedPrice = "ShockedPrice";

        /// <summary>
        /// Gets the ShockedPrice attribute.
        /// </summary>
        /// <param name="element">The source element.</param>
        /// <returns>The shocked price.</returns>
        public static double ShockedPrice(this XElement element)
        {
            return (double) element.Attribute(XShockedPrice);
        }

        /// <summary>
        /// Sets the ShockedPrice attribute = initialPrice * (1 + tariff)
        /// </summary>
        /// <param name="element">The element to shock.</param>
        /// <returns>A reference to the existing <see cref="XElement"/>. This is returned for use with fluent syntax calls.</returns>
        public static XElement ShockPrice(this XElement element)
        {
            double currentPrice = element.CurrentPrice();
            double tariff = element.Shock();
            double shockedPrice = currentPrice * (1 + tariff);
            element.SetAttributeValue(XShockedPrice, shockedPrice);
            return element;
        }

        /// <summary>
        /// Sets each ShockedPrice attribute in reverse document order = initialPrice * (1 + tariff)
        /// </summary>
        /// <param name="element">The root element.</param>
        /// <returns>A reference to the existing <see cref="XElement"/>. This is returned for use with fluent syntax calls.</returns>
        public static XElement ShockAllPrices(this XElement element)
        {
            foreach (XElement item in element.DescendantsAndSelf().Reverse())
            {
                item.ShockPrice();
            }
            return element;
        }
    }
}
