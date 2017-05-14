using JetBrains.Annotations;

namespace AD.PartialEquilibriumApi.Optimization.PSO
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    public static class RandomSolutionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="swarm"></param>
        /// <param name="particle"></param>
        /// <returns></returns>
        [Pure]
        public static Solution RandomSolution(this Swarm swarm, Particle particle)
        {
            int dimensions = swarm.Dimensions;

            double[] position = new double[dimensions];

            for (int i = 0; i < dimensions; i++)
            {
                position[i] = (swarm.UpperBound - swarm.LowerBound) * swarm.RandomGenerator.NextDouble() + swarm.LowerBound;
            }

            return new Solution(swarm.ObjectiveFunction(position), position);
        }
    }
}
