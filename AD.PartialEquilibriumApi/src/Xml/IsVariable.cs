using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AD.PartialEquilibriumApi
{
    public static class IsVariableExtensions
    {
        private static readonly XName XIsVariable = "IsVariable";

        public static bool IsVariable(this XElement element)
        {
            if (element.Attribute(XIsVariable) == null)
            {
                return false;
            }
            return (bool) element.Attribute(XIsVariable);
        }

        public static void SetIsVariable(this XElement model, XName[] value)
        {
            foreach (XElement market in model.DescendantsAndSelf())
            {
                market.SetAttributeValue(XIsVariable, value.Contains(market.Name));
            }
        }
    }
}
