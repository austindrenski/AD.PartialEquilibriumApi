using JetBrains.Annotations;

namespace AD.PartialEquilibriumApi.Optimization.PSO
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    public static class RandomVelocityExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="swarm"></param>
        /// <param name="particle"></param>
        /// <returns></returns>
        [Pure]
        public static double[] RandomVelocity(this Swarm swarm, Particle particle)
        {
            int dimensions = swarm.Dimensions;

            double[] velocity = new double[dimensions];

            for (int i = 0; i < dimensions; i++)
            {
                velocity[i] = 5e-02 * particle.Velocity[i];
            }

            return velocity;
        }
    }
}
