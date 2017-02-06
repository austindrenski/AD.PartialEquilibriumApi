using JetBrains.Annotations;

namespace AD.PartialEquilibriumApi.Optimization
{
    /// <summary>
    /// Extension method to calculate the centroid solution for a <see cref="Simplex"/>.
    /// </summary>
    [PublicAPI]
    public static class CentroidExtension
    {
        /// <summary>
        /// Calculates the centroid solution. For a simplex with three vertices, the centroid solution is found where 
        /// the line from the worst vertex to its mirror passes through the line formed by the remaining vertices.
        /// </summary>
        /// <param name="simplex">The source <see cref="Simplex"/>.</param>
        /// <returns>The centroid solution.</returns>
        public static Solution Centroid(this Simplex simplex)
        {
            double[] centroid = new double[simplex.Dimensions];

            for (int i = 0; i < simplex.Dimensions; i++)
            {
                for (int j = 0; j < simplex.LastIndex; j++)
                {
                    centroid[i] += simplex[j][i];
                }
                centroid[i] /= simplex.LastIndex;

                centroid = centroid.EnforceStrictBounds(simplex);
            }
            return new Solution(simplex.ObjectiveFunction(centroid), centroid);
        }
    }
}
