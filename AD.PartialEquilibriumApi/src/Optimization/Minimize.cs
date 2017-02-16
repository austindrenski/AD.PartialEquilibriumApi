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
            int iterations = simplex.Iterations;
            int numberOfSolutions = simplex.Solutions.Length;
            int lastVertex = numberOfSolutions - 1;

            for (int i = 0; i < iterations; i++)
            {
                //simplex.TextWriter.WriteLineAsync($"> i = {$"{i}".PadLeft(simplex.Iterations.ToString().Length)}: {simplex.Solutions[0]}");

                Array.Sort(simplex.Solutions);

                Solution centroid = simplex.Centroid();
                Solution reflected = simplex.Reflect(centroid);

                if (reflected < simplex[0])
                {
                    Solution expanded = simplex.Expand(centroid, reflected);
                    simplex[lastVertex] = expanded < simplex[0] ? expanded : reflected;
                    continue;
                }

                if (simplex.Solutions.Count(x => reflected < x) == 1)
                {
                    simplex[lastVertex] = reflected;

                    Solution contracted = simplex.Contract(centroid);

                    if (contracted < simplex[lastVertex])
                    {
                        simplex[lastVertex] = contracted;
                    }
                    else
                    {
                        for (int j = 1; j < numberOfSolutions; j++)
                        {
                            simplex[j] = simplex.Shrink(simplex[j]);
                        }
                    }
                    continue;
                }
                simplex[lastVertex] = reflected;
            }

            return simplex[0];
        }
    }
}
