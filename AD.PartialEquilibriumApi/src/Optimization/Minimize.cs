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
            int lastVertex = simplex.Solutions.Length - 1;

            for (int i = 0; i < simplex.Iterations; i++)
            {
                //simplex.TextWriter.WriteLineAsync($"> i = {$"{i}".PadLeft(simplex.Iterations.ToString().Length)}: {simplex.Solutions[0]}");

                Array.Sort(simplex.Solutions);

                Solution centroid = simplex.Centroid();
                Solution reflected = simplex.Reflect(centroid);

                if (reflected < simplex.Solutions[0])
                {
                    Solution expanded = simplex.Expand(centroid, reflected);
                    simplex[lastVertex] = expanded < simplex.Solutions[0] ? expanded : reflected;
                    continue;
                }

                if (simplex.Solutions.Count(x => reflected < x) == 1)
                {
                    if (reflected <= simplex.Solutions[lastVertex])
                    {
                        simplex[lastVertex] = reflected;
                    }

                    Solution contracted = simplex.Contract(centroid);

                    if (contracted < simplex.Solutions[lastVertex])
                    {
                        simplex[lastVertex] = contracted;
                    }
                    else
                    {
                        simplex.Shrink();
                    }
                    continue;
                }
                simplex[lastVertex] = reflected;
            }

            return simplex.Solutions[0];
        }
    }
}
