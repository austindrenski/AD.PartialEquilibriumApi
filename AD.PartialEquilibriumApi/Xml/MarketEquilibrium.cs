﻿using System;
using System.Linq;
using System.Xml.Linq;
using JetBrains.Annotations;

namespace AD.PartialEquilibriumApi.Xml
{
    /// <summary>
    /// Extension methods to calculate the market equilibrium condition.
    /// </summary>
    [PublicAPI]
    public static class CalculateMarketEquilibriumExtensions
    {
        private static readonly XName XMarketEquilibrium = "MarketEquilibrium";

        /// <summary>
        /// Returns the value of the MarketEquilibrium attribute.
        /// </summary>
        /// <param name="market">An <see cref="XElement"/> describing a market.</param>
        /// <returns>The value of the MarketEquilibrium attribute.</returns>
        public static double MarketEquilibrium([NotNull] this XElement market)
        {
            if (market.Attribute(XMarketEquilibrium) == null)
            {
                return 1;
            }
            return (double)market.Attribute(XMarketEquilibrium);
        }

        /// <summary>
        /// Sets the MarketEquilibrium attribute on descendant <see cref="XElement"/> objects in reverse document order.
        /// Result = (producerPrice ^ elasticityOfSupply) - [(consumerConsumerPriceIndex ^ (elasticityOfSubstitution + elasticityOfDemand)) / (consumerPrice ^ elasticityOfSubstitution)]
        /// </summary>
        /// <returns>A reference to the existing <see cref="XElement"/>. This is returned for use with fluent syntax calls.</returns>
        public static XElement CalculateMarketEquilibrium([NotNull] this XElement model)
        {
            foreach (XElement market in model.DescendantsAndSelf().Reverse())
            {
                //if (market.HasElements)
                //{
                //    market.SetAttributeValue(XMarketEquilibrium, market.Elements().Sum(x => x.MarketEquilibrium()));
                //    continue;
                //}

                double priceIndexComponents =
                    market.Parent?
                          .Elements()
                          .Sum(x => x.MarketShare() * Math.Pow(x.ConsumerPrice(), 1 - x.ElasticityOfSubstitution()))
                    ?? 
                    market.MarketShare() * Math.Pow(market.ConsumerPrice(), 1 - market.ElasticityOfSubstitution());

                double priceIndex =
                    Math.Pow(priceIndexComponents, 1 / (1 - market.ElasticityOfSubstitution()));

                double consumerPrice = market.ConsumerPrice();
                double producerPrice = market.ProducerPrice();
                double elasticityOfDemand = market.ElasticityOfDemand();
                double elasticityOfSubstitution = market.ElasticityOfSubstitution();
                double elasticityOfSupply = market.ElasticityOfSupply();

                double marketEquilibrium =
                    Math.Pow(producerPrice, elasticityOfSupply)
                    -
                    Math.Pow(priceIndex, elasticityOfSubstitution + elasticityOfDemand)
                    /
                    Math.Pow(consumerPrice, elasticityOfSubstitution);

                market.SetAttributeValue(XMarketEquilibrium, marketEquilibrium);
            }

            return model;
        }
    }
}