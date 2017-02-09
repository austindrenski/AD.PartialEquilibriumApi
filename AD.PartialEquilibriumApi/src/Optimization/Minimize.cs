using System;
using JetBrains.Annotations;

namespace AD.PartialEquilibriumApi
{
    /// <summary>
    /// Extension methods for minimization using a <see cref="Simplex"/> object.
    /// </summary>
    [PublicAPI]
    public static class MinimizeExtensions
    {
        /// <summary>
        /// Solves for the minimum value solution.
        /// </summary>
        /// <returns>The solution that produces the minimum value.</returns>
        public static Solution Minimize(this Simplex simplex)
        {
            if (simplex.Solutions.Length < simplex.Dimensions + 1)
            {
                throw new ArgumentOutOfRangeException("The number of solutions in the simplex must be greater than or equal to the dimensions + 1");
            }

            for (int i = 0; i < simplex.Iterations; i++)
            {
                if (i % 10 == 0)
                {
                    simplex.TextWriter.WriteLineAsync($"> i = {$"{i}".PadLeft(simplex.Iterations.ToString().Length)}: {simplex.Solutions[0]}");
                }

                Solution centroid = simplex.Centroid();
                Solution reflected = simplex.Reflect(centroid);

                if (reflected < simplex.Solutions[0])
                {
                    Solution expanded = simplex.Expand(centroid, reflected);
                    simplex.Swap(expanded < simplex.Solutions[0] ? expanded : reflected, simplex.Dimensions);
                    Array.Sort(simplex.Solutions);
                    continue;
                }

                if (reflected < simplex)
                {
                    if (reflected <= simplex.Solutions[simplex.Dimensions])
                    {
                        simplex.Swap(reflected, simplex.Dimensions);
                        Array.Sort(simplex.Solutions);
                    }

                    Solution contracted = simplex.Contract(centroid);

                    if (simplex.Solutions[simplex.Dimensions] < contracted)
                    {
                        simplex.Shrink();
                        Array.Sort(simplex.Solutions);
                    }
                    else
                    {
                        simplex.Swap(contracted, simplex.Dimensions);
                        Array.Sort(simplex.Solutions);
                    }
                    continue;
                }
                simplex.Swap(reflected, simplex.Dimensions);
                Array.Sort(simplex.Solutions);
            }

            return simplex.Solutions[0];
        }
    }
}
