using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        /// <summary>
        /// Sets the MarketEquilibrium attribute. 
        /// </summary>
        /// <returns>A reference to the existing <see cref="XElement"/>. This is returned for use with fluent syntax calls.</returns>
        public static XElement CalculateRootMarketEquilibrium(this XElement element)
        {
            foreach (XElement item in element.Descendants().Reverse())
            {
                item.CalculateMarketEquilibrium();
            }
            double sumOfSquares =
                element.Elements()
                       .Select(x => (double)x.Attribute("MarketEquilibrium"))
                       .Select(x => x * x)
                       .Sum();

            element.SetAttributeValue("MarketEquilibrium", sumOfSquares);
            return element;
        }

        /// <summary>
        /// Sets the MarketEquilibrium attribute. 
        /// </summary>
        /// <returns>A reference to the existing <see cref="XElement"/>. This is returned for use with fluent syntax calls.</returns>
        public static XElement CalculateMarketEquilibrium(this XElement element)
        {
            double initialPrice = (double) element.Attribute("InitialPrice");
            double shockedPrice = (double) element.Attribute("ShockedPrice");
            double priceIndex = (double) element.Attribute("PriceIndex");
            double elasticityOfDemand = (double) element.Attribute("ElasticityOfDemand");
            double elasticityOfSupply = (double) element.Attribute("ElasticityOfSupply");
            double elasticityOfSubstitution = (double) element.Attribute("ElasticityOfSubstitution");

            double marketEquilibrium =
                Math.Pow(
                    shockedPrice,
                    elasticityOfSupply)
                -
                Math.Pow(
                    priceIndex,
                    elasticityOfSubstitution + elasticityOfDemand) / Math.Pow(initialPrice, elasticityOfSubstitution);

            element.SetAttributeValue("MarketEquilibrium", marketEquilibrium);
            return element;
        }
    }
}
