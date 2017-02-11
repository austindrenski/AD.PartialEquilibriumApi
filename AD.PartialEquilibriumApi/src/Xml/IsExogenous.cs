using System.Linq;
using System.Xml.Linq;
using JetBrains.Annotations;

namespace AD.PartialEquilibriumApi
{
    /// <summary>
    /// Extension methods to mark <see cref="XElement"/> objects as variables.
    /// </summary>
    [PublicAPI]
    public static class IsExogenousExtensions
    {
        private static readonly XName XIsExogenous = "IsExogenous";

        /// <summary>
        /// Returns true if this element is marked as exogenous.
        /// </summary>
        /// <param name="element">The element to check.</param>
        public static bool IsExogenous(this XElement element)
        {
            if (element.Attribute(XIsExogenous) == null)
            {
                return false;
            }
            return (bool)element.Attribute(XIsExogenous);
        }

        /// <summary>
        /// Marks the markets in the model as exogenous if the market name is contained in the names array.
        /// </summary>
        /// <param name="model">The model to search.</param>
        /// <param name="names">The names of the markets to be marked as exogenous.</param>
        public static XElement SetIsExogenous(this XElement model, params XName[] names)
        {
            if (names == null)
            {
                return model;
            }
            foreach (XElement market in model.DescendantsAndSelf().Where(x => names.Contains(x.Name)))
            {
                market.SetAttributeValue(XIsExogenous, true);
            }
            return model;
        }
    }
}
