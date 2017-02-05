using JetBrains.Annotations;

namespace AD.PartialEquilibriumApi.Optimization
{
    /// <summary>
    /// Extension methods to calculate the expanded solution for a <see cref="Simplex"/>.
    /// </summary>
    [PublicAPI]
    public static class ExpandedExtensions
    {
        /// <summary>
        /// Calculates the expanded solution.
        /// </summary>
        /// <param name="simplex">The source <see cref="Simplex"/>.</param>
        /// <returns>The expanded solution.</returns>
        public static Solution Expanded(this Simplex simplex, Solution centroid, Solution reflected)
        {
            double[] expanded = new double[simplex.Dimensions];

            for (int i = 0; i < simplex.Dimensions; i++)
            {
                expanded[i] = (1 - Simplex.Expansion) * centroid.Vector[i] + Simplex.Expansion * reflected.Vector[i];
            }

            return new Solution(simplex.ObjectiveFunction(expanded), expanded);
        }
    }
}
