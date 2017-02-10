using System;
using System.Linq;
using System.Xml.Linq;
using JetBrains.Annotations;

namespace AD.PartialEquilibriumApi
{
    /// <summary>
    /// Factory class providing methods that aggregate model values into a single objective value.
    /// </summary>
    [PublicAPI]
    public static class ObjectiveFunctionFactory
    {
        /// <summary>
        /// Calculates an objective value as:
        /// model.DescendantsAndSelf().Sum(x => Math.Abs(x.MarketEquilibrium()) * x.AncestorsAndSelf().Count());
        /// </summary>
        /// <param name="model">An <see cref="XElement"/> describing a model.</param>
        /// <returns>A double value that when minimized, optimizes the model.</returns>
        public static double Default(XElement model)
        {
            return model.DescendantsAndSelf()
                        .Sum(x => Math.Abs(x.MarketEquilibrium()) * x.AncestorsAndSelf().Count());
        }
    }
}
