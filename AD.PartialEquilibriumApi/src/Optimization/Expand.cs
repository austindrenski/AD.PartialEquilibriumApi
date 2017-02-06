using JetBrains.Annotations;

namespace AD.PartialEquilibriumApi
{
    /// <summary>
    /// Extension methods to calculate the expanded solution for a <see cref="Simplex"/>.
    /// </summary>
    [PublicAPI]
    public static class ExpandExtensions
    {
        /// <summary>
        /// Calculates the expanded solution.
        /// </summary>
        /// <param name="simplex">The source <see cref="Simplex"/>.</param>
        /// <param name="centroid">The centroid solution calculated on the <see cref="Simplex"/>.</param>
        /// <param name="reflected">The reflected solution calculated on the <see cref="Simplex"/>.</param>
        /// <returns>The expanded solution.</returns>
        public static Solution Expand(this Simplex simplex, Solution centroid, Solution reflected)
        {
            double[] expanded = new double[simplex.Dimensions];

            for (int i = 0; i < simplex.Dimensions; i++)
            {
                expanded[i] = (1 - Simplex.Expansion) * centroid[i] + Simplex.Expansion * reflected[i];

                expanded = expanded.EnforceBounds(simplex);
            }

            return new Solution(simplex.ObjectiveFunction(expanded), expanded);
        }
    }
}
