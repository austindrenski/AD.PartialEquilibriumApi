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
        /// <param name="market">The source element.</param>
        /// <returns>The ConsumerPrice modified by the shock.</returns>
        public static double ProducerPrice(this XElement market)
        {
            if (market.Attribute(XProducerPrice) == null)
            {
                market.SetAttributeValue(XProducerPrice, market.InitialPrice());
            }
            return (double)market.Attribute(XProducerPrice);
        }

        /// <summary>
        /// Sets each ProducerPrice attribute on descendant <see cref="XElement"/> objects in reverse document order.
        /// Result = ConsumerPrice / (1 + Shock)
        /// </summary>
        /// <param name="model">The model to shock.</param>
        /// <returns>A reference to the existing <see cref="XElement"/>. This is returned for use with fluent syntax calls.</returns>
        public static XElement ShockProducerPrices(this XElement model)
        {
            foreach (XElement market in model.DescendantsAndSelf().Reverse())
            {
                double consumerPrice = market.ConsumerPrice();
                double shock = market.Shock();
                double shockedPrice = consumerPrice / (1 + shock);
                market.SetAttributeValue(XProducerPrice, shockedPrice);
            }
            return model;
        }
    }
}
