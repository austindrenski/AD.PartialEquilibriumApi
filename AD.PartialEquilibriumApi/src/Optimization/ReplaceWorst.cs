using System;
using JetBrains.Annotations;

namespace AD.PartialEquilibriumApi.Optimization
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    public static class ReplaceWorstExtensions
    {
        /// <summary>
        /// Replaces the worst solution with the specified solution.
        /// </summary>
        /// <param name="simplex">The source <see cref="Simplex"/>.</param>
        /// <param name="solution">The replacement solution.</param>
        public static void ReplaceWorst(this Simplex simplex, Solution solution)
        {
            for (int i = 0; i < simplex.Dimensions; i++)
            {
                simplex.Solutions[simplex.NumberOfSolutions - 1].Vector[i] = solution.Vector[i];
            }

            simplex.Solutions[simplex.NumberOfSolutions - 1].Value = solution.Value;
            Array.Sort(simplex.Solutions);
        }
    }
}
