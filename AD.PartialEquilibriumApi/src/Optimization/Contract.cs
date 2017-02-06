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
        public static Solution Contract(this Simplex simplex, Solution centroid)
        {
            double[] contracted = new double[simplex.Dimensions];

            for (int i = 0; i < simplex.Dimensions; i++)
            {
                contracted[i] = (1 - Simplex.Contraction) * centroid[i] + Simplex.Contraction * simplex[simplex.LastIndex][i];
            }

            contracted = contracted.EnforceBounds(simplex);

            return new Solution(simplex.ObjectiveFunction(contracted), contracted);
        }
    }
}
