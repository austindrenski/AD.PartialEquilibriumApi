using JetBrains.Annotations;

namespace AD.PartialEquilibriumApi
{
    /// <summary>
    /// Extension methods to shrink the solutions of a <see cref="Simplex"/>.
    /// </summary>
    [PublicAPI]
    public static class ShrinkExtensions
    {
        /// <summary>
        /// Moves each vertex from its current position to the position of the best vertex.
        /// </summary>
        /// <param name="simplex">The source <see cref="Simplex"/>.</param>
        public static void Shrink(this Simplex simplex)
        {
            int dimensions = simplex.Dimensions;

            int vertices = simplex.Solutions.Length;
            
            for (int i = 1; i < vertices; i++)
            {
                for (int j = 0; j < dimensions; j++)
                {
                    simplex.Solutions[i][j] = simplex[i][j] + simplex[0][j];
                }
                for (int j = 0; j < dimensions; j++)
                {
                    simplex.Solutions[i][j] *= 0.5;

                }
                simplex.Solutions[i].Value = simplex.ObjectiveFunction(simplex[i].Vector);
            }
        }
    }
}
