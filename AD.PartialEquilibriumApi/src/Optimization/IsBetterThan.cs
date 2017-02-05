using JetBrains.Annotations;

namespace AD.PartialEquilibriumApi.Optimization
{
    /// <summary>
    /// Extension methods to determine if the new solution is an improvement over current solutions in the <see cref="Simplex"/>.
    /// </summary>
    public static class IsBetterThanExtensions
    {
        /// <summary>
        /// Returns the number of solutions over which the solution is an improvement.
        /// </summary>
        /// <param name="solution">The solution to test.</param>
        /// <param name="simplex">The <see cref="Simplex"/> against which to test the solution.</param>
        /// <returns>The number of solutions in the simplex over which the solution is an improvement.</returns>
        public static int IsBetterThan(this Solution solution, Simplex simplex)
        {
            int betterThan = 0;
            for (int i = 0; i < simplex.NumberOfSolutions; i++)
            {
                if (solution <= simplex.Solutions[i])
                {
                    betterThan++;
                }
            }
            return betterThan;
        }
    }
}
