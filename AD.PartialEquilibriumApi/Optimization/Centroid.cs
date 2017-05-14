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
        /// Calculates the centroid solution. For a simplex with three vertices, the centroid solution is found along
        /// the line from the worst vertex to its mirror across the plane of the remaining vertices.
        /// </summary>
        /// <param name="simplex">The source <see cref="Simplex"/>.</param>
        /// <returns>The centroid solution.</returns>
        [Pure]
        public static Solution Centroid(this Simplex simplex)
        {
            int dimensions = simplex.Dimensions;

            double[] centroid = new double[dimensions];

            for (int i = 0; i < dimensions; i++)
            {
                for (int j = 0; j < dimensions; j++)
                {
                    centroid[i] += simplex[j][i];
                }
                centroid[i] /= dimensions;
            }

            centroid = simplex.EnforceBounds(centroid);

            return new Solution(simplex.ObjectiveFunction(centroid), centroid);
        }
    }
}
