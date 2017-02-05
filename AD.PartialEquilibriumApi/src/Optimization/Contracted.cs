using JetBrains.Annotations;

namespace AD.PartialEquilibriumApi.Optimization
{
    /// <summary>
    /// Extension methods to calculate the contracted solution for a <see cref="Simplex"/>.
    /// </summary>
    [PublicAPI]
    public static class ContractedExtensions
    {
        /// <summary>
        /// Calculates the contracted solution. For a simplex with three vertices, the contracted solution
        /// is found by moving the worst vertice toward the centroid, but does not through it.
        /// </summary>
        /// <param name="simplex">The source <see cref="Simplex"/>.</param>
        /// <returns></returns>
        public static Solution Contracted(this Simplex simplex, Solution centroid)
        {
            double[] contracted = new double[simplex.Dimensions];

            for (int i = 0; i < simplex.Dimensions; i++)
            {
                contracted[i] = (1 - Simplex.Reflection) * centroid.Vector[i] + Simplex.Reflection * simplex.Solutions[simplex.NumberOfSolutions - 1].Vector[i];
            }

            return new Solution(simplex.ObjectiveFunction(contracted), contracted);
        }
    }
}
