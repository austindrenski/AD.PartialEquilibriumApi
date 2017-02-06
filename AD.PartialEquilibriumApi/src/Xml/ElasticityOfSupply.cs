using System.Xml.Linq;
using JetBrains.Annotations;

namespace AD.PartialEquilibriumApi
{
    /// <summary>
    /// Extension methods to get the elasticity of supply attribute.
    /// </summary>
    [PublicAPI]
    public static class ElasticityOfSupplyExtensions
    {
        private static readonly XName XElasticityOfSupply = "ElasticityOfSupply";

        /// <summary>
        /// Returns the ElasticityOfSupply attribute.
        /// </summary>
        /// <param name="element">The source element.</param>
        /// <returns>The value of the "ElasticityOfSupply" attribute.</returns>
        public static double ElasticityOfSupply([NotNull] this XElement element)
        {
            return (double)element.Attribute(XElasticityOfSupply);
        }

        /// <summary>
        /// Sets the value of the ElasticityOfSupply attribute.
        /// </summary>
        /// <param name="element">The source element.</param>
        /// <param name="value">The value to which the ElasticityOfSupply attribute is set.</param>
        public static void ElasticityOfSupply([NotNull] this XElement element, double value)
        {
            element.SetAttributeValue(XElasticityOfSupply, value);
        }
    }
}
