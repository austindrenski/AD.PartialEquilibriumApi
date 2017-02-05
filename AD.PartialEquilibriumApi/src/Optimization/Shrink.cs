using System;
using JetBrains.Annotations;

namespace AD.PartialEquilibriumApi.Optimization
{
    /// <summary>
    /// Extension methods to shrink the solutions of a <see cref="Simplex"/>.
    /// </summary>
    [PublicAPI]
    public static class ShrinkExtensions
    {
        /// <summary>
        /// Moves each vertex halfway from its current position to the position of the best vertex.
        /// </summary>
        /// <param name="simplex">The source <see cref="Simplex"/>.</param>
        public static void Shrink(this Simplex simplex)
        {
            for (int i = 1; i < simplex.Dimensions; i++)
            {
                for (int j = 0; j < simplex.Dimensions; j++)
                {
                    simplex.Solutions[i].Vector[j] = 0.5 * (simplex.Solutions[i].Vector[j] + simplex.Solutions[0].Vector[j]);
                    simplex.Solutions[i].Value = simplex.ObjectiveFunction(simplex.Solutions[i].Vector);
                }
            }

            Array.Sort(simplex.Solutions);
        }
    }
}
