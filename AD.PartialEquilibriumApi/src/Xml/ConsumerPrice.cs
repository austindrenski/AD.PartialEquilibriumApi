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
            if (market.Attribute(XConsumerPrice) == null)
            {
                market.SetAttributeValue(XConsumerPrice, market.InitialPrice());
            }
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
        /// <param name="variable">True if the price at this index is variable.</param>
        public static void SetConsumerPrices([NotNull] this XElement market, double[] values, XName[] variable)
        {
            foreach (XElement item in market.DescendantsAndSelf().Reverse())
            {
                if (variable.Contains(item.Name))
                {
                    item.ConsumerPrice(values[Array.IndexOf(variable, item.Name)]);
                }
                else
                {
                    double elasticityOfSubstitution = item.ElasticityOfSubstitution();

                    double consumerPriceIndexComponents = 
                        item.Elements()
                            .Select(x => x.InitialMarketShare() * Math.Pow(x.ConsumerPrice(), 1 - elasticityOfSubstitution))
                            .Sum();

                    double consumerPriceIndex = Math.Pow(consumerPriceIndexComponents, 1 / (1 - elasticityOfSubstitution));

                    item.ConsumerPrice(consumerPriceIndex);
                }
            }
        }
    }
}
