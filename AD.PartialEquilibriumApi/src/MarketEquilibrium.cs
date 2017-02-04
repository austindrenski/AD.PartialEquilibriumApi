using System;
using System.Linq;
using System.Xml.Linq;
using JetBrains.Annotations;

namespace AD.PartialEquilibriumApi
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
        /// <param name="element">An <see cref="XElement"/> describing a market.</param>
        /// <returns>The value of the MarketEquilibrium attribute.</returns>
        public static double MarketEquilibrium([NotNull] this XElement element)
        {
            return (double)element.Attribute(XMarketEquilibrium);
        }

        /// <summary>
        /// Sets the MarketEquilibrium attribute. 
        /// </summary>
        /// <returns>A reference to the existing <see cref="XElement"/>. This is returned for use with fluent syntax calls.</returns>
        public static XElement CalculateRootMarketEquilibrium([NotNull] this XElement element)
        {
            foreach (XElement item in element.Descendants().Reverse())
            {
                item.CalculateMarketEquilibrium();
            }
            double sumOfSquares =
                element.Elements()
                       .Select(x => x.MarketEquilibrium())
                       .Select(x => x * x)
                       .Sum(); 

            element.SetAttributeValue(XMarketEquilibrium, sumOfSquares);
            return element;
        }

        /// <summary>
        /// Sets the MarketEquilibrium attribute. 
        /// Result = (shockedPrice ^ elasticityOfSupply) - [(priceIndex ^ (elasticityOfSubstitution + elasticityOfDemand)) / (initialPrice ^ elasticityOfSubstitution)]
        /// </summary>
        /// <returns>A reference to the existing <see cref="XElement"/>. This is returned for use with fluent syntax calls.</returns>
        public static XElement CalculateMarketEquilibrium([NotNull] this XElement element)
        {
            double elasticityOfDemand = element.ElasticityOfDemand();
            double elasticityOfSubstitution = element.ElasticityOfSubstitution();
            double elasticityOfSupply = element.ElasticityOfSupply();
            double initialPrice = element.InitialPrice();
            double priceIndex = element.PriceIndex();
            double shockedPrice = element.ShockedPrice();

            double marketEquilibrium =
                Math.Pow(shockedPrice, elasticityOfSupply)
                -
                Math.Pow(priceIndex, elasticityOfSubstitution + elasticityOfDemand) 
                / 
                Math.Pow(initialPrice, elasticityOfSubstitution);

            element.SetAttributeValue(XMarketEquilibrium, marketEquilibrium);
            return element;
        }
    }
}
