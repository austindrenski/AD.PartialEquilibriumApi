using JetBrains.Annotations;

namespace AD.PartialEquilibriumApi.PSO
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    public static class VelocityExtensions
    {
        /// <summary>
        /// The weight applied to the previous velocity.
        /// </summary>
        private const double Inertia = 1e-10;

        /// <summary>
        /// The weight applied to the errror from the particle minimum.
        /// </summary>
        private const double LocalWeight = 1e+00;

        /// <summary>
        /// The weight applied to the error from the swarm minimum.
        /// </summary>
        private const double GlobalWeight = 1e+00;

        /// <summary>
        /// v(t+1) = [w * v(t)] + [c1 * r1 * (l(t) - x(t))] + [c2 * r2 * (g(t) - x(t))]
        /// </summary>
        /// <param name="swarm"></param>
        /// <param name="particle"></param>
        /// <returns></returns>
        [Pure]
        public static double[] Velocity(this Swarm swarm, Particle particle)
        {
            int dimensions = swarm.Dimensions;

            double[] velocity = new double[dimensions];

            for (int i = 0; i < dimensions; i++)
            {
                velocity[i] =
                    Inertia      * particle.Velocity[i]
                    +
                    LocalWeight  * swarm.RandomGenerator.NextDouble() * (particle.Best[i]    - particle.Current[i])
                    +
                    GlobalWeight * swarm.RandomGenerator.NextDouble() * (swarm.GlobalBest[i] - particle.Current[i]);
            }

            return velocity;
        }
    }
}
