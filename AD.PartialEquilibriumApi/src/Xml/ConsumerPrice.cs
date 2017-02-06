using System;
using System.Linq;
using System.Xml.Linq;
using JetBrains.Annotations;

namespace AD.PartialEquilibriumApi
{
    /// <summary>
    /// Extension methods to access the ConsumerPrice attribute.
    /// </summary>
    [PublicAPI]
    public static class ConsumerPriceExtensions
    {
        private static readonly XName XConsumerPrice = "ConsumerPrice";

        /// <summary>
        /// Returns the value of the ConsumerPrice attribute.
        /// </summary>
        /// <param name="market">An <see cref="XElement"/> describing a market.</param>
        /// <returns>The value set by the user to the "ConsumerPrice" attribute.</returns>
        public static double ConsumerPrice([NotNull] this XElement market)
        {
            return (double)market.Attribute(XConsumerPrice);
        }

        /// <summary>
        /// Sets the value of the ConsumerPrice attribute.
        /// </summary>
        /// <param name="market">An <see cref="XElement"/> describing a market.</param>
        /// <param name="value">The value to which the ConsumerPrice attribute is set.</param>
        public static void ConsumerPrice([NotNull] this XElement market, double value)
        {
            market.SetAttributeValue(XConsumerPrice, value);
        }

        /// <summary>
        /// Sets the value of the ConsumerPrice attribute.
        /// </summary>
        /// <param name="market">An <see cref="XElement"/> describing a market.</param>
        /// <param name="value">The value to which the ConsumerPrice attribute is set.</param>
        public static void ConsumerPrice([NotNull] this XElement market, Func<XElement, double> value)
        {
            market.SetAttributeValue(XConsumerPrice, value(market));
        }

        /// <summary>
        /// Sets the values of the ConsumerPrice attributes in provided an array of values in document-order.
        /// </summary>
        /// <param name="market">An <see cref="XElement"/> describing a market.</param>
        /// <param name="values">The values to which the ConsumerPrice attributes are set.</param>
        public static void SetConsumerPrices([NotNull] this XElement market, double[] values)
        {
            XName[] names = market.DescendantsAndSelf().Select(x => x.Name).ToArray();
            for (int i = 0; i < names.Length; i++)
            {
                market.DescendantsAndSelf(names[i])
                      .Single()
                      .ConsumerPrice(values[i]);
            }
        }

        /// <summary>
        /// Sets the values of the ConsumerPrice attributes in provided an array of values in document-order.
        /// </summary>
        /// <param name="market">An <see cref="XElement"/> describing a market.</param>
        /// <param name="values">The values to which the ConsumerPrice attributes are set.</param>
        /// <param name="variable">True if the price at this index is variable.</param>
        public static void SetConsumerPrices([NotNull] this XElement market, double[] values, bool[] variable)
        {
            XName[] names = market.DescendantsAndSelf().Where((x, i) => variable[i]).Select(x => x.Name).ToArray();
            for (int i = 0; i < names.Length; i++)
            {
                int index = i;
                market.DescendantsAndSelf(names[i])
                      .Single()
                      .ConsumerPrice(x => values[index]);
            }
        }
    }
}
