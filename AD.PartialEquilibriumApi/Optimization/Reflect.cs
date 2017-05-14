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
        [Pure]
        public static Solution Reflect(this Simplex simplex, Solution centroid)
        {
            int dimensions = simplex.Dimensions;

            double[] reflected = new double[dimensions];

            for (int i = 0; i < dimensions; i++)
            {
                reflected[i] = 2.0 * centroid[i] - simplex[dimensions][i];
            }

            reflected = simplex.EnforceBounds(reflected);

            return new Solution(simplex.ObjectiveFunction(reflected), reflected);
        }
    }
}
