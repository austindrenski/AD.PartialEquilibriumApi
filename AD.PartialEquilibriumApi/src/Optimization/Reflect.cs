using JetBrains.Annotations;

namespace AD.PartialEquilibriumApi.Optimization
{
    /// <summary>
    /// Extension methods to calculate the reflected solution for a <see cref="Simplex"/>.
    /// </summary>
    [PublicAPI]
    public static class ReflectExtension
    {
        /// <summary>
        /// Calculates the reflected solution. For a simplex with three vertices, the reflected solution 
        /// is found by mirroring the worst vertex through the plane of the remaining vertices.
        /// </summary>
        /// <param name="simplex">The source <see cref="Simplex"/>.</param>
        /// <param name="centroid">The centroid calculated for this simplex.</param>
        /// <returns>The reflected solution.</returns>
        public static Solution Reflect(this Simplex simplex, Solution centroid)
        {
            double[] reflected = new double[simplex.Dimensions];

            for (int i = 0; i < simplex.Dimensions; i++)
            {
                reflected[i] = (1 + Simplex.Reflection) * centroid[i] - Simplex.Reflection * simplex[simplex.NumberOfSolutions - 1][i];

                reflected = reflected.EnforceStrictBounds(simplex);
            }

            return new Solution(simplex.ObjectiveFunction(reflected), reflected);
        }
    }
}
