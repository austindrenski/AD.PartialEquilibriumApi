using System;
using JetBrains.Annotations;

namespace AD.PartialEquilibriumApi.Optimization
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    public static class OptimizationFactory
    {
        /// <summary>
        /// The weight applied to the previous velocity.
        /// </summary>
        private const double Inertia = 1e-10;

        /// <summary>
        /// The weight applied to the errror from the particle minimum.
        /// </summary>
        private const double LocalWeight = 1e00;

        /// <summary>
        /// The weight applied to the error from the swarm minimum.
        /// </summary>
        private const double GlobalWeight = 1e00;

        /// <summary>
        /// Specifies the algorithm's numerical precision. 
        /// E.g. F(*) - F(k) > Tolerance
        /// </summary>
        private const double Tolerance = 1e-15;

        /// <summary>
        /// Generates random numbers.
        /// </summary>
        private static Random _randomNumberGenerator = new Random();

        /// <summary>
        /// This method executes the optimization algorithm. It is called privately by PsoPe.Optimize().
        /// </summary>
        /// <param name="seed">The seed value should be passed by a Random.NextDouble() to ensure that tight loops do not generate equivalent sequences.</param>
        /// <param name="swarm">The swarm used to find minimum values.</param>
        /// <param name="objectiveFunction">Contains the user-supplied objective function(s) and methods for calculations (normal) or evaluations (optimizations).</param>
        /// <param name="objectiveVariableCount">Count of independent variables.</param>
        /// <param name="numberOfParticles">Count of particles used in the swarm.</param>
        /// <param name="maxIterations">The count of iteratiosn without improvement performed by the algorithm prior to exiting.</param>
        /// <returns>Returns a double[] of the best position found by the swarm.</returns>
        public static Particle[] ParticleSwarmOptimization(Random random, Swarm swarm, Func<double[], double> objectiveFunction, int objectiveVariableCount, int numberOfParticles, int maxIterations, double lowerBound, double upperBound)
        {
            double[] bestGlobalPosition = new double[objectiveVariableCount];
            double bestGlobalCost = double.MaxValue;
            int iterationsSinceImprovement = 0;
            double previousBestCost = double.MaxValue;
            double[] currentToGlobalTransformation = new double[objectiveVariableCount];
            double[] virtualBestPosition = new double[objectiveVariableCount];
            double virtualBestCost = double.MaxValue;

            //
            // Main processing loop
            //
            while (iterationsSinceImprovement < maxIterations)
            {
                iterationsSinceImprovement++;
                if (previousBestCost - bestGlobalCost > Tolerance)
                {
                    iterationsSinceImprovement = 0;
                    previousBestCost = bestGlobalCost;
                }
                for (int i = 0; i < swarm.NumberOfParticles; i++)
                {
                    double death = random.NextDouble();

                    // new velocity, v(t+1) = [w * v(t)] + [c1 * r1 * (l(t) - x(t))] + [c2 * r2 * (g(t) - x(t))]
                    for (int j = 0; j < objectiveVariableCount; j++)
                    {
                        double r1 = random.NextDouble();
                        double r2 = random.NextDouble();
                        swarm.Particles[i].Velocity[j] = 
                            Inertia * swarm.Particles[i].Velocity[j] 
                            + 
                            LocalWeight * r1 * (swarm.Particles[i].BestPosition[j] - swarm.Particles[i].Position[j]) 
                            + 
                            GlobalWeight * r2 * (bestGlobalPosition[j] - swarm.Particles[i].Position[j]);
                    }

                    // new position, x(t+1) = x(t) + v(t+1)
                    for (int j = 0; j < objectiveVariableCount; j++)
                    {
                        swarm.Particles[i].Position[j] += swarm.Particles[i].Velocity[j];
                        if (swarm.Particles[i].Position[j] < lowerBound)
                        {
                            swarm.Particles[i].Position[j] = lowerBound;
                            death = 0;
                        }
                        if (swarm.Particles[i].Position[j] > upperBound)
                        {
                            swarm.Particles[i].Position[j] = upperBound;
                            death = 0;
                        }
                    }

                    // new cost, F(x(t+1))
                    swarm.Particles[i].Cost = objectiveFunction(swarm.Particles[i].Position);
                    if (swarm.Particles[i].BestCost - swarm.Particles[i].Cost > Tolerance)
                    {
                        swarm.Particles[i].BestPosition = swarm.Particles[i].Position;
                        swarm.Particles[i].BestCost = swarm.Particles[i].Cost;
                    }
                    if (bestGlobalCost - swarm.Particles[i].BestCost > Tolerance)
                    {
                        bestGlobalPosition = swarm.Particles[i].BestPosition;
                        bestGlobalCost = swarm.Particles[i].BestCost;
                    }
                    if (death < 0.05)
                    {
                        for (int j = 0; j < objectiveVariableCount; j++)
                        {
                            swarm.Particles[i].Position[j] = (upperBound - lowerBound) * random.NextDouble() + lowerBound;
                            swarm.Particles[i].Velocity[j] *= 0.05;
                        }
                        swarm.Particles[i].BestPosition = swarm.Particles[i].Position;
                        swarm.Particles[i].Cost = objectiveFunction(swarm.Particles[i].Position);
                        swarm.Particles[i].BestCost = swarm.Particles[i].Cost;
                    }
                }

                //
                // Line search from bestPosition to bestGlobalPosition
                //
                for (int i = 0; i < swarm.Particles.Length; i++)
                {
                    for (int j = 0; j < objectiveVariableCount; j++)
                    {
                        currentToGlobalTransformation[j] = bestGlobalPosition[j] - swarm.Particles[i].Position[j];
                    }
                    for (int j = 0; j < objectiveVariableCount; j++)
                    {
                        double[] virtualPositionN = new double[objectiveVariableCount];
                        for (int k = 0; k < objectiveVariableCount; k++)
                        {
                            virtualPositionN[k] = currentToGlobalTransformation[k] * (j + 1) / (objectiveVariableCount + 1);
                            virtualPositionN[k] += swarm.Particles[i].Position[k];
                        }
                        double virtualCost = objectiveFunction(virtualPositionN);
                        if (virtualBestCost - virtualCost > Tolerance)
                        {
                            virtualBestPosition = virtualPositionN;
                            virtualBestCost = virtualCost;
                        }
                    }
                }
                if (bestGlobalCost - virtualBestCost > Tolerance)
                {
                    bestGlobalPosition = virtualBestPosition;
                    bestGlobalCost = virtualBestCost;
                }
            }
            return swarm.Particles;
        }
    }
}
