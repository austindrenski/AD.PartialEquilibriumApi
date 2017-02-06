using JetBrains.Annotations;

namespace AD.PartialEquilibriumApi
{
    /// <summary>
    /// Extension methods to enforce boundary constraints on a vector given a <see cref="Simplex"/>.
    /// </summary>
    [PublicAPI]
    public static class EnforceBoundsExtensions
    {
        /// <summary>
        /// Enforces bounds as: lower &lt; x &lt; upper
        /// </summary>
        /// <param name="vector">The vector to enforce.</param>
        /// <param name="simplex">The <see cref="Simplex"/> for which to enforce boundary constraints.</param>
        /// <returns></returns>
        public static double[] EnforceStrictBounds(this double[] vector, Simplex simplex)
        {
            for (int i = 0; i < simplex.Dimensions; i++)
            {
                if (simplex.LowerBound < vector[i] 
                                      && vector[i] < simplex.UpperBound)
                {
                    continue;
                }
                vector[i] = (simplex.UpperBound - simplex.LowerBound) * Simplex.Random.NextDouble() + simplex.LowerBound + double.Epsilon;
            }
            return vector;
        }

        /// <summary>
        /// Enforces bounds as: lower &lt; x &lt;= upper
        /// </summary>
        /// <param name="vector">The vector to enforce.</param>
        /// <param name="simplex">The <see cref="Simplex"/> for which to enforce boundary constraints.</param>
        /// <returns></returns>
        public static double[] EnforceStrictLowerBounds(this double[] vector, Simplex simplex)
        {
            for (int i = 0; i < simplex.Dimensions; i++)
            {
                if (simplex.LowerBound < vector[i] 
                                      && vector[i] <= simplex.UpperBound)
                {
                    continue;
                }
                vector[i] = (simplex.UpperBound - simplex.LowerBound) * Simplex.Random.NextDouble() + simplex.LowerBound + double.Epsilon;
            }
            return vector;
        }

        /// <summary>
        /// Enforces bounds as: lower &lt;= x &lt; upper
        /// </summary>
        /// <param name="vector">The vector to enforce.</param>
        /// <param name="simplex">The <see cref="Simplex"/> for which to enforce boundary constraints.</param>
        /// <returns></returns>
        public static double[] EnforceStrictUpperBounds(this double[] vector, Simplex simplex)
        {
            for (int i = 0; i < simplex.Dimensions; i++)
            {
                if (simplex.LowerBound <= vector[i] 
                                       && vector[i] < simplex.UpperBound)
                {
                    continue;
                }
                vector[i] = (simplex.UpperBound - simplex.LowerBound) * Simplex.Random.NextDouble() + simplex.LowerBound;
            }
            return vector;
        }
    }
}
