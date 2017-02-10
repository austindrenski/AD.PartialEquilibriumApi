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
            // This method performs the same work as the LINQ form commented below.
            // Need to benchmark this later. Leaving the for-loop version for now. 
            // Possible that assignments will be faster and lighter on the GC.
            //
            //simplex.Solutions[index] = new Solution(simplex.ObjectiveFunction(solution.Vector), solution.Vector);
            //
            for (int i = 0; i < simplex.Dimensions; i++)
            {
                simplex.Solutions[index][i] = solution[i];
            }

            simplex.Solutions[index].Value = simplex.ObjectiveFunction(simplex.Solutions[index].Vector);
        }
    }
}
