using JetBrains.Annotations;

namespace AD.PartialEquilibriumApi
{
    /// <summary>
    /// Extension methods to swap solutions in a <see cref="Simplex"/>.
    /// </summary>
    [PublicAPI]
    public static class SwapExtensions
    {
        /// <summary>
        /// Replaces the worst solution with the specified solution.
        /// </summary>
        /// <param name="simplex">The source <see cref="Simplex"/>.</param>
        /// <param name="solution">The replacement solution.</param>
        public static void Swap(this Simplex simplex, Solution solution)
        {
            int dimensions = simplex.Dimensions;

            for (int i = 0; i < dimensions; i++)
            {
                simplex.Solutions[dimensions][i] = solution[i];
            }

            simplex.Solutions[dimensions].Value = simplex.ObjectiveFunction(simplex.Solutions[dimensions].Vector);
        }
    }
}
