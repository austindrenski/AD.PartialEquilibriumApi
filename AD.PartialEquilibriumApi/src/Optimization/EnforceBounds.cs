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
        /// Enforces bounds as: lower &lt;= x &lt;= upper. 
        /// </summary>
        /// <param name="vector">The vector to enforce.</param>
        /// <param name="simplex">The <see cref="Simplex"/> for which to enforce boundary constraints.</param>
        /// <returns></returns>
        [Pure]
        public static double[] EnforceBounds(this Simplex simplex, double[] vector)
        {
            int dimensions = simplex.Dimensions;

            double[] result = new double[dimensions];

            for (int i = 0; i < dimensions; i++)
            {
                if (vector[i] < simplex.LowerBound)
                {
                    result[i] = 
                        vector[i] < simplex.LowerBound 
                        ? simplex.LowerBound + simplex.Precision + 1e-01 * (simplex.UpperBound - simplex.LowerBound) 
                        : vector[i];
                }
                else
                {
                    result[i] = 
                        vector[i] > simplex.UpperBound 
                        ? simplex.UpperBound - simplex.Precision - 1e-01 * (simplex.UpperBound - simplex.LowerBound) 
                        : vector[i];
                }
            }

            return result;
        }
    }
}
