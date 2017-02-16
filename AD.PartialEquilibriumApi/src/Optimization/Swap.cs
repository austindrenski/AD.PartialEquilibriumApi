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
        /// <param name="index">The index of the current solution to be replaced.</param>
        public static void Swap(this Simplex simplex, Solution solution, int index)
        {
            for (int i = 0; i < simplex.Dimensions; i++)
            {
                simplex.Solutions[index][i] = solution[i];
            }

            simplex.Solutions[index].Value = simplex.ObjectiveFunction(simplex.Solutions[index].Vector);
        }
    }
}
