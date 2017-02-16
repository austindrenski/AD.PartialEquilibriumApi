using JetBrains.Annotations;

namespace AD.PartialEquilibriumApi
{
    /// <summary>
    /// Extension methods to shrink the solutions of a <see cref="Simplex"/>.
    /// </summary>
    [PublicAPI]
    public static class ShrinkExtensions
    {
        /// <summary>
        /// Shrinks the solution from its current position to the position of the best vertex by half.
        /// </summary>
        /// <param name="simplex">The source <see cref="Simplex"/>.</param>
        /// <param name="solution">The solution to shrink toward the best solution in the <see cref="Simplex"/>.</param>
        [Pure]
        public static Solution Shrink(this Simplex simplex, Solution solution)
        {
            int dimensions = simplex.Dimensions;

            double[] result = new double[dimensions];

            for (int i = 0; i < dimensions; i++)
            {
                result[i] = 0.5 * (solution[i] + simplex[0][i]);
            }

            return new Solution(simplex.ObjectiveFunction(result), result);
        }
    }
}
