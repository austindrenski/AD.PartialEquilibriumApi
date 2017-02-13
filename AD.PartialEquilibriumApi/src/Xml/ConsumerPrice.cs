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
            // Set prices where markets are basic and variables of the model.
            int index = 0;
            foreach (XElement market in model.DescendantsAndSelf().Where(x => /*!x.HasElements & */x.IsVariable() && !x.IsExogenous()))
            {
                // If the market is basic, set the i
                if (!market.HasElements)
                {
                    market.SetAttributeValue(XConsumerPrice, values[index++]);
                }
            }

            // Set prices where markets are not basic and variables of the model.
            //index = 0;
            //foreach (XElement market in model.DescendantsAndSelf().Where(x => x.HasElements & x.IsVariable() && !x.IsExogenous()))
            //{
            //    market.SetAttributeValue(XConsumerPrice, values[index++]);
            //}

            // Set prices where the markets are basic but not endogenous to the model.
            foreach (XElement market in model.DescendantsAndSelf().Where(x => !x.HasElements && !x.IsVariable() && !x.IsExogenous()).Reverse())
            {
                double consumerPrice = market.InitialPrice();

                market.SetAttributeValue(XConsumerPrice, consumerPrice);
            }

            // Set prices where the markets are not basic and not endogenous to the model.
            foreach (XElement market in model.DescendantsAndSelf().Where(x => x.HasElements && !x.IsVariable() && !x.IsExogenous()).Reverse())
            {
                double consumerPriceIndexComponents =
                    market.Elements()
                          .Sum(x => x.InitialMarketShare() * Math.Pow(x.ConsumerPrice(), 1 - x.ElasticityOfSubstitution()));

                double consumerPrice =
                    Math.Pow(consumerPriceIndexComponents, 1 / (1 - market.ElasticityOfSubstitution()));

                market.SetAttributeValue(XConsumerPrice, consumerPrice);
            }

            return model;
        }

        /// <summary>
        /// Sets the price if the market is marked exogenous.
        /// </summary>
        /// <param name="model">The model to search.</param>
        /// <param name="values">The prices to be set.</param>
        public static XElement SetExogenousPrices(this XElement model, params double[] values)
        {
            if (values == null)
            {
                return model;
            }

            int index = 0;
            foreach (XElement market in model.DescendantsAndSelf().Where(x => x.IsExogenous()))
            {
                market.SetAttributeValue(XConsumerPrice, values[index++]);
            }

            return model;
        }
    }
}