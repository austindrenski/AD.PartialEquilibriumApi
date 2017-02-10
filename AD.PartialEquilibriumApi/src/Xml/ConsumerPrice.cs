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
        /// Sets the values of the ConsumerPrice attributes in provided an array of values in document-order.
        /// </summary>
        /// <param name="model">An <see cref="XElement"/> describing a model.</param>
        /// <param name="values">The values to which the ConsumerPrice attributes are set.</param>
        public static XElement SetConsumerPrices([NotNull] this XElement model, double[] values)
        {
            XElement[] variableMarkets = 
                model.DescendantsAndSelf()
                      .Where(x => x.IsVariable())
                      .ToArray();

            for (int i = 0; i < values.Length; i++)
            {
                variableMarkets[i].SetAttributeValue(XConsumerPrice, values[i]);
            }

            foreach (XElement item in model.DescendantsAndSelf().Where(x => x.HasElements).Reverse())
            {
                double consumerPriceIndexComponents =
                        item.Elements()
                            .Sum(x => x.InitialMarketShare() * Math.Pow(x.ConsumerPrice(), 1 - x.ElasticityOfSubstitution()));

                double consumerPrice = 
                    Math.Pow(consumerPriceIndexComponents, 1 / (1 - item.ElasticityOfSubstitution()));

                item.SetAttributeValue(XConsumerPrice, consumerPrice);
            }
            return model;
        }
    }
}