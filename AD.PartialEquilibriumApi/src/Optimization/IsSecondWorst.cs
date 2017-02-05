using JetBrains.Annotations;

namespace AD.PartialEquilibriumApi.Optimization
{
    /// <summary>
    /// Extension methods to determine if the reflected solution is the second worst solution in a <see cref="Simplex"/>.
    /// </summary>
    [PublicAPI]
    public static class IsSecondWorstExtensions
    {
        /// <summary>
        /// True if the reflected solution is worse than all but the worst vertex.
        /// </summary>
        /// <param name="simplex">The source <see cref="Simplex"/>.</param>
        /// <param name="reflected">The reflected solution.</param>
        /// <returns>True if the reflected solution is the second worst vertex.</returns>
        public static bool IsSecondWorst(this Simplex simplex, Solution reflected)
        {
            for (int i = 0; i < simplex.NumberOfSolutions - 1; i++)
            {
                if (reflected.Value <= simplex.Solutions[i].Value)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
