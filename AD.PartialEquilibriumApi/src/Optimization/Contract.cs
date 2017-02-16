using JetBrains.Annotations;

namespace AD.PartialEquilibriumApi
{
    /// <summary>
    /// Extension methods to calculate the contracted solution for a <see cref="Simplex"/>.
    /// </summary>
    [PublicAPI]
    public static class ContractExtensions
    {
        /// <summary>
        /// Calculates the contracted solution. For a simplex with three vertices, the contracted solution
        /// is found by moving the worst vertice toward the centroid, but does not through it.
        /// </summary>
        /// <param name="centroid">The centroid calculated on the simplex.</param>
        /// <param name="simplex">The source <see cref="Simplex"/>.</param>
        /// <returns>The current worst solution moved in the direction of the centroid.</returns>
        [Pure]
        public static Solution Contract(this Simplex simplex, Solution centroid)
        {
            int dimensions = simplex.Dimensions;

            double[] contracted = new double[dimensions];

            for (int i = 0; i < dimensions; i++)
            {
                contracted[i] = 0.5 * (centroid[i] + simplex[dimensions][i]);
            }

            contracted = simplex.EnforceBounds(contracted);

            return new Solution(simplex.ObjectiveFunction(contracted), contracted);
        }
    }
}
