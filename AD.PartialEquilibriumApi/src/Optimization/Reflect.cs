using JetBrains.Annotations;

namespace AD.PartialEquilibriumApi
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
        /// <param name="index">The index of the point to be reflected.</param>
        /// <returns>The reflected solution.</returns>
        public static Solution Reflect(this Simplex simplex, Solution centroid, int index)
        {
            double[] reflected = new double[simplex.Dimensions];

            for (int i = 0; i < simplex.Dimensions; i++)
            {
                reflected[i] = 2.0 * centroid[i] - simplex[index][i];
            }

            reflected = reflected.EnforceBounds(simplex);

            return new Solution(simplex.ObjectiveFunction(reflected), reflected);
        }
    }
}
