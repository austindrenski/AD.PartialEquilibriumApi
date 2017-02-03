using System.Xml.Linq;
using JetBrains.Annotations;

namespace AD.PartialEquilibriumApi
{
    /// <summary>
    /// Extension methods to access the Tariff attribute.
    /// </summary>
    [PublicAPI]
    public static class ElasticityOfSubstitutionExtensions
    {
        private static readonly XName ElasticityOfSubstitutionXName = "ElasticityOfSubstitution";

        /// <summary>
        /// Returns the value of the ElasticityOfSubstitution attribute.
        /// </summary>
        /// <param name="market">An <see cref="XElement"/> describing a market.</param>
        /// <returns>The value set by the user to the ElasticityOfSubstitution attribute.</returns>
        public static double ElasticityOfSubstitution([NotNull] this XElement market)
        {
            return (double)market.Attribute(ElasticityOfSubstitutionXName);
        }

        /// <summary>
        /// Sets the value of the ElasticityOfSubstitution attribute.
        /// </summary>
        /// <param name="market">An <see cref="XElement"/> describing a market.</param>
        /// <param name="value">The value to which the ElasticityOfSubstitution attribute is set.</param>
        public static void ElasticityOfSubstitution([NotNull] this XElement market, double value)
        {
            market.SetAttributeValue(ElasticityOfSubstitutionXName, value);
        }
    }
}
