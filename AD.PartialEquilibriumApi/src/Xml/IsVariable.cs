using System.Linq;
using System.Xml.Linq;

namespace AD.PartialEquilibriumApi
{
    /// <summary>
    /// Extension methods to mark <see cref="XElement"/> objects as variables.
    /// </summary>
    public static class IsVariableExtensions
    {
        private static readonly XName XIsVariable = "IsVariable";

        /// <summary>
        /// Returns true if this element is marked as a variable.
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
        /// Marks the markets in the model as variables if the market name is contained in the value array.
        /// </summary>
        /// <param name="model">The model to search.</param>
        /// <param name="value">The names of the markets to be marked as variables.</param>
        public static void SetIsVariable(this XElement model, XName[] value)
        {
            foreach (XElement market in model.DescendantsAndSelf())
            {
                market.SetAttributeValue(XIsVariable, value.Contains(market.Name));
            }
        }
    }
}
