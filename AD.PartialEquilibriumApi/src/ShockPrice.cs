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
        /// <summary>
        /// Sets the ShockedPrice attribute = initialPrice * (1 + tariff)
        /// </summary>
        /// <param name="element">The element to shock.</param>
        /// <returns>A reference to the existing <see cref="XElement"/>. This is returned for use with fluent syntax calls.</returns>
        public static XElement ShockPrice(this XElement element)
        {
            double initialPrice = element.InitialPrice();
            double tariff = element.Tariff();
            double shockedPrice = initialPrice * (1 + tariff);
            element.SetAttributeValue("ShockedPrice", shockedPrice);
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
