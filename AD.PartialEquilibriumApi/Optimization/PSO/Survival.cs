﻿using JetBrains.Annotations;

namespace AD.PartialEquilibriumApi.Optimization.PSO
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    public static class SurvivalExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="swarm"></param>
        /// <param name="particle"></param>
        /// <returns></returns>
        [Pure]
        public static Particle Survival(this Swarm swarm, Particle particle)
        { 
            if (particle.Survival < 1e-01)
            {
                return particle;
            }

            double[] randomVelocity = swarm.RandomVelocity(particle);

            Solution randomSolution = swarm.RandomSolution(particle);

            return new Particle(randomSolution, randomVelocity);
        }
    }
}