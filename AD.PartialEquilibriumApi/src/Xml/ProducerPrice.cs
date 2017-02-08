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
        /// Sets the ProducerPrice attribute = ConsumerPrice / (1 + Shock)
        /// </summary>
        /// <param name="element">The element to shock.</param>
        /// <returns>A reference to the existing <see cref="XElement"/>. This is returned for use with fluent syntax calls.</returns>
        public static XElement ShockProducerPrice(this XElement element)
        {
            if (element.Parent == null)
            {
                return element;
            }

            double consumerPrice = element.ConsumerPrice();
            double shock = element.Shock();
            double shockedPrice = consumerPrice / (1 + shock);
            element.SetAttributeValue(XProducerPrice, shockedPrice);
            return element;
        }

        /// <summary>
        /// Sets each ProducerPrice attribute in reverse document order = ConsumerPrice / (1 + Shock)
        /// </summary>
        /// <param name="element">The root element.</param>
        /// <returns>A reference to the existing <see cref="XElement"/>. This is returned for use with fluent syntax calls.</returns>
        public static XElement ShockAllProducerPrices(this XElement element)
        {
            foreach (XElement item in element.DescendantsAndSelf().Reverse())
            {
                item.ShockProducerPrice();
            }
            return element;
        }
    }
}
