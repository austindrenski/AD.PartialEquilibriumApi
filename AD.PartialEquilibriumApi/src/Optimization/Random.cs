using JetBrains.Annotations;

namespace AD.PartialEquilibriumApi
{
    /// <summary>
    /// Extensions methods to generate random solutions.
    /// </summary>
    [PublicAPI]
    public static class RandomExtensions
    {
        /// <summary>
        /// Calculates a random solution. 
        /// </summary>
        /// <param name="simplex">The source <see cref="Simplex"/>.</param>
        /// <returns>The random solution.</returns>
        public static Solution Random(this Simplex simplex)
        {
            double[] random = new double[simplex.Dimensions];

            for (int i = 0; i < simplex.Dimensions; i++)
            {
                random[i] = (simplex.UpperBound - simplex.LowerBound) * simplex.RandomGenerator.NextDouble() + simplex.LowerBound;
            }

            return new Solution(simplex.ObjectiveFunction(random), random);
        }
    }
}
