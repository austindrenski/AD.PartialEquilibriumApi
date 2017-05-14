using JetBrains.Annotations;

namespace AD.PartialEquilibriumApi.Optimization.PSO
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    public static class PositionExtensions
    {
        /// <summary>
        /// x(t+1) = x(t) + v(t+1)
        /// </summary>
        /// <param name="swarm"></param>
        /// <param name="particle"></param>
        /// <returns></returns>
        [Pure]
        public static Solution Position(this Swarm swarm, Particle particle)
        {
            int dimensions = swarm.Dimensions;

            double[] position = new double[dimensions];

            for (int i = 0; i < dimensions; i++)
            {
                position[i] = particle.Current[i] + particle.Velocity[i];

                if (position[i] < swarm.LowerBound)
                {
                    position[i] = swarm.LowerBound + 1e-01 * (swarm.UpperBound - swarm.LowerBound);
                    particle.Survival = 0.0;
                }
                if (position[i] > swarm.UpperBound)
                {
                    position[i] = swarm.UpperBound - 1e-01 * (swarm.UpperBound - swarm.LowerBound);
                    particle.Survival = 0.0;
                }
            }
            return new Solution(swarm.ObjectiveFunction(position), position);
        }
    }
}
