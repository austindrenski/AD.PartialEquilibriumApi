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
        public static double[] EnforceBounds(this double[] vector, Simplex simplex)
        {
            for (int i = 0; i < simplex.Dimensions; i++)
            {
                vector[i] = vector[i] < simplex.LowerBound ? simplex.LowerBound + simplex.Precision + 1e-01 : vector[i];
                vector[i] = vector[i] > simplex.UpperBound ? simplex.UpperBound - simplex.Precision - 1e-01 : vector[i];
            }
            return vector;
        }
    }
}
