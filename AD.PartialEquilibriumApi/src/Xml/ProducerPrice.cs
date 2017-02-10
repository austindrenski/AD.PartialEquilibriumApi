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
        private static readonly XName XProducerPrice = "ProducerPrice";

        /// <summary>
        /// Gets the ProducerPrice attribute.
        /// </summary>
        /// <param name="element">The source element.</param>
        /// <returns>The ConsumerPrice modified by the shock.</returns>
        public static double ProducerPrice(this XElement element)
        {
            if (element.Attribute(XProducerPrice) == null)
            {
                element.SetAttributeValue(XProducerPrice, element.InitialPrice());
            }
            return (double) element.Attribute(XProducerPrice);
        }

        /// <summary>
        /// Sets each ProducerPrice attribute on descendant <see cref="XElement"/> objects in reverse document order.
        /// Result = ConsumerPrice / (1 + Shock)
        /// </summary>
        /// <param name="element">The element to shock.</param>
        /// <returns>A reference to the existing <see cref="XElement"/>. This is returned for use with fluent syntax calls.</returns>
        public static XElement ShockProducerPrices(this XElement element)
        {
            foreach (XElement item in element.DescendantsAndSelf().Reverse())
            {
                double consumerPrice = item.ConsumerPrice();
                double shock = item.Shock();
                double shockedPrice = consumerPrice / (1 + shock);
                item.SetAttributeValue(XProducerPrice, shockedPrice);
            }

            return element;
        }
    }
}
