using System.Linq;
using System.Xml.Linq;
using JetBrains.Annotations;

namespace AD.PartialEquilibriumApi
{
    /// <summary>
    /// Extension methods to mark <see cref="XElement"/> objects as variables.
    /// </summary>
    [PublicAPI]
    public static class IsVariableExtensions
    {
        private static readonly XName XIsVariable = "IsVariable";

        /// <summary>
        /// Returns true if this element is marked as variable.
        /// </summary>
        /// <param name="element">The element to check.</param>
        public static bool IsVariable(this XElement element)
        {
            if (element.Attribute(XIsVariable) == null)
            {
                return false;
            }
            return (bool) element.Attribute(XIsVariable);
        }

        /// <summary>
        /// Marks the markets in the model as variable if the market name is contained in the names array.
        /// </summary>
        /// <param name="model">The model to search.</param>
        /// <param name="names">The names of the markets to be marked as variable.</param>
        public static XElement SetIsVariable(this XElement model, params XName[] names)
        {
            foreach (XElement market in model.DescendantsAndSelf().Where(x => names.Contains(x.Name)))
            {
                market.SetAttributeValue(XIsVariable, true);
            }
            return model;
        }
    }
}
