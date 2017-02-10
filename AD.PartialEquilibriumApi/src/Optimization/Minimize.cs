using System;
using System.Linq;
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
            if (simplex.Solutions.Length < simplex.Dimensions)
            {
                throw new ArgumentOutOfRangeException("The number of solutions in the simplex must be greater than or equal to the dimensions + 1");
            }

            int lastIndex = simplex.Solutions.Length - 1;

            for (int i = 0; i < simplex.Iterations; i++)
            {
                simplex.TextWriter.WriteLineAsync($"> i = {$"{i}".PadLeft(simplex.Iterations.ToString().Length)}: {simplex.Solutions[0]}");

                Solution centroid = simplex.Centroid(lastIndex);
                Solution reflected = simplex.Reflect(centroid, lastIndex);

                if (reflected < simplex.Solutions[0])
                {
                    Solution expanded = simplex.Expand(centroid, reflected);
                    simplex.Swap(expanded < simplex.Solutions[0] ? expanded : reflected, lastIndex);
                    Array.Sort(simplex.Solutions);
                    continue;
                }

                if (simplex.Solutions.Count(x => reflected < x) == 1)
                {
                    if (reflected <= simplex.Solutions[lastIndex])
                    {
                        simplex.Swap(reflected, lastIndex);
                        Array.Sort(simplex.Solutions);
                    }

                    Solution contracted = simplex.Contract(centroid);

                    if (contracted > simplex.Solutions[lastIndex])
                    {
                        simplex.Shrink();
                        Array.Sort(simplex.Solutions);
                    }
                    else
                    {
                        simplex.Swap(contracted, lastIndex);
                        Array.Sort(simplex.Solutions);
                    }
                    continue;
                }

                simplex.Swap(reflected, lastIndex);
                Array.Sort(simplex.Solutions);
            }

            return simplex.Solutions[0];
        }
    }
}
