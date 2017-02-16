using JetBrains.Annotations;

namespace AD.PartialEquilibriumApi
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
        public static Solution Centroid(this Simplex simplex)
        {
            int dimensions = simplex.Dimensions;

            double[] centroid = new double[dimensions];

            for (int i = 0; i < dimensions; i++)
            {
                for (int j = 0; j < dimensions; j++)
                {
                    centroid[j] += simplex[i][j];
                }
            }

            for (int i = 0; i < dimensions; i++)
            {
                centroid[i] /= dimensions;
            }

            centroid = centroid.EnforceBounds(simplex);

            return new Solution(simplex.ObjectiveFunction(centroid), centroid);
        }
    }
}
