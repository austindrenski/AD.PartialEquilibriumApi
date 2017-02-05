using System;
using System.Linq;
using System.Xml.Linq;
using JetBrains.Annotations;

namespace AD.PartialEquilibriumApi
{
    /// <summary>
    /// Extension methods to access the CurrentPrice attribute.
    /// </summary>
    [PublicAPI]
    public static class CurrentPriceExtensions
    {
        private static readonly XName XCurrentPrice = "CurrentPrice";

        /// <summary>
        /// Returns the value of the CurrentPrice attribute.
        /// </summary>
        /// <param name="market">An <see cref="XElement"/> describing a market.</param>
        /// <returns>The value set by the user to the "CurrentPrice" attribute.</returns>
        public static double CurrentPrice([NotNull] this XElement market)
        {
            return (double)market.Attribute(XCurrentPrice);
        }

        /// <summary>
        /// Sets the value of the CurrentPrice attribute.
        /// </summary>
        /// <param name="market">An <see cref="XElement"/> describing a market.</param>
        /// <param name="value">The value to which the CurrentPrice attribute is set.</param>
        public static void CurrentPrice([NotNull] this XElement market, double value)
        {
            market.SetAttributeValue(XCurrentPrice, value);
        }

        /// <summary>
        /// Sets the value of the CurrentPrice attribute.
        /// </summary>
        /// <param name="market">An <see cref="XElement"/> describing a market.</param>
        /// <param name="value">The value to which the CurrentPrice attribute is set.</param>
        public static void CurrentPrice([NotNull] this XElement market, Func<XElement, double> value)
        {
            market.SetAttributeValue(XCurrentPrice, value(market));
        }

        /// <summary>
        /// Sets the values of the CurrentPrice attributes in provided an array of values in document-order.
        /// </summary>
        /// <param name="market">An <see cref="XElement"/> describing a market.</param>
        /// <param name="values">The values to which the CurrentPrice attributes are set.</param>
        public static void SetCurrentPrices([NotNull] this XElement market, double[] values)
        {
            XName[] names = market.DescendantsAndSelf().Select(x => x.Name).ToArray();
            for (int i = 0; i < names.Length; i++)
            {
                market.DescendantsAndSelf(names[i])
                      .Single()
                      .CurrentPrice(values[i]);
            }
        }

        /// <summary>
        /// Sets the values of the CurrentPrice attributes in provided an array of values in document-order.
        /// </summary>
        /// <param name="market">An <see cref="XElement"/> describing a market.</param>
        /// <param name="values">The values to which the CurrentPrice attributes are set.</param>
        /// <param name="variable">True if the price at this index is variable.</param>
        public static void SetCurrentPrices([NotNull] this XElement market, double[] values, bool[] variable)
        {
            XName[] names = market.DescendantsAndSelf().Where((x, i) => variable[i]).Select(x => x.Name).ToArray();
            for (int i = 0; i < names.Length; i++)
            {
                int index = i;
                market.DescendantsAndSelf(names[i])
                      .Single()
                      .CurrentPrice(x => values[index]);
            }
        }
    }
}
