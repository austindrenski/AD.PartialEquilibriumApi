using JetBrains.Annotations;

namespace AD.PartialEquilibriumApi.Optimization
{
    /// <summary>
    /// Extension methods to calculate the reflected solution for a <see cref="Simplex"/>.
    /// </summary>
    [PublicAPI]
    public static class ReflectedExtension
    {
        /// <summary>
        /// Calculates the reflected solution. For a simplex with three vertices, the reflected solution 
        /// is found by mirroring the worst vertex through the plane of the remaining vertices.
        /// </summary>
        /// <param name="simplex">The source <see cref="Simplex"/>.</param>
        /// <returns>The reflected solution.</returns>
        public static Solution Reflected(this Simplex simplex, Solution centroid)
        {
            double[] reflected = new double[simplex.Dimensions];

            for (int i = 0; i < simplex.Dimensions; i++)
            {
                reflected[i] = (1 + Simplex.Reflection) * centroid.Vector[i] - Simplex.Reflection * simplex.Solutions[simplex.NumberOfSolutions - 1].Vector[i];
            }

            return new Solution(simplex.ObjectiveFunction(reflected), reflected);
        }
    }
}
