using JetBrains.Annotations;

namespace AD.PartialEquilibriumApi.Optimization
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
        [Pure]
        public static Solution Random(this Simplex simplex)
        {
            int dimensions = simplex.Dimensions;

            double[] random = new double[dimensions];

            for (int i = 0; i < dimensions; i++)
            {
                random[i] = (simplex.UpperBound - simplex.LowerBound) * simplex.RandomGenerator.NextDouble() + simplex.LowerBound;
            }

            return new Solution(simplex.ObjectiveFunction(random), random);
        }
    }
}
