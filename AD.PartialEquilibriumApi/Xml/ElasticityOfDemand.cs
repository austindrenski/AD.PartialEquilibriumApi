using System.Xml.Linq;
using JetBrains.Annotations;

namespace AD.PartialEquilibriumApi.Xml
{
    /// <summary>
    /// Extension methods to get the elasticity of demand attribute.
    /// </summary>
    [PublicAPI]
    public static class ElasticityOfDemandExtensions
    {
        private static readonly XName XElasticityOfDemand = "ElasticityOfDemand";

        /// <summary>
        /// Returns the ElasticityOfDemand attribute.
        /// </summary>
        /// <param name="element">The source element.</param>
        /// <returns>The value of the "ElasticityOfDemand" attribute.</returns>
        public static double ElasticityOfDemand([NotNull] this XElement element)
        {
            return (double)element.Attribute(XElasticityOfDemand);
        }

        /// <summary>
        /// Sets the value of the ElasticityOfDemand attribute.
        /// </summary>
        /// <param name="element">The source element.</param>
        /// <param name="value">The value to which the ElasticityOfDemand attribute is set.</param>
        public static void ElasticityOfDemand([NotNull] this XElement element, double value)
        {
            element.SetAttributeValue(XElasticityOfDemand, value);
        }
    }
}
