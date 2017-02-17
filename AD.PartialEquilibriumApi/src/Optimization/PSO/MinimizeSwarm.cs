using System.Linq;
using JetBrains.Annotations;

namespace AD.PartialEquilibriumApi.PSO
{
    /// <summary>
    /// Extension methods for minimization using a <see cref="Swarm"/> object.
    /// </summary>
    [PublicAPI]
    public static class MinimizeSwarmExtensions
    {
        /// <summary>
        /// Specifies the algorithm's numerical precision. 
        /// E.g. F(*) - F(k) > Tolerance
        /// </summary>
        private const double Tolerance = 1e-15;

        /// <summary>
        /// The weight applied to the previous velocity.
        /// </summary>
        private const double Inertia = 1e-01;

        /// <summary>
        /// The weight applied to the errror from the particle minimum.
        /// </summary>
        private const double LocalWeight = 1e+00;

        /// <summary>
        /// The weight applied to the error from the swarm minimum.
        /// </summary>
        private const double GlobalWeight = 1e+00;

        /// <summary>
        /// This method executes the optimization algorithm. It is called privately by PsoPe.Optimize().
        /// </summary>
        /// <param name="swarm">The swarm used to find minimum values.</param>
        /// <returns>Returns a double[] of the best position found by the swarm.</returns>
        public static Particle Minimize(this Swarm swarm)
        {
            int dimensions = swarm.Dimensions;
            int iterations = swarm.Iterations;
            int particles = swarm.Particles;
            double lowerBound = swarm.LowerBound;
            double upperBound = swarm.UpperBound;

            double bestGlobalCost = double.MaxValue;
            double previousBestCost = double.MaxValue;
            double virtualBestCost = double.MaxValue;
            double[] bestGlobalPosition = new double[dimensions];
            double[] currentToGlobalTransformation = new double[dimensions];
            double[] virtualBestPosition = new double[dimensions];

            //
            // Main processing loop
            //
            for (int h = 0; h < iterations; h++)
            {
                swarm.TextWriter.WriteLineAsync($"> i = {$"{h}".PadLeft(swarm.Iterations.ToString().Length)}: {swarm.Min()}");

                if (previousBestCost - bestGlobalCost > Tolerance)
                {
                    previousBestCost = bestGlobalCost;
                }
                for (int i = 0; i < particles; i++)
                {
                    double death = swarm.RandomGenerator.NextDouble();

                    // new velocity, v(t+1) = [w * v(t)] + [c1 * r1 * (l(t) - x(t))] + [c2 * r2 * (g(t) - x(t))]
                    for (int j = 0; j < dimensions; j++)
                    {
                        double r1 = swarm.RandomGenerator.NextDouble();
                        double r2 = swarm.RandomGenerator.NextDouble();
                        swarm[i].Velocity[j] =
                            Inertia * swarm[i].Velocity[j]
                            +
                            LocalWeight * r1 * (swarm[i].BestVector[j] - swarm[i][j])
                            +
                            GlobalWeight * r2 * (bestGlobalPosition[j] - swarm[i][j]);
                    }

                    // new position, x(t+1) = x(t) + v(t+1)
                    for (int j = 0; j < dimensions; j++)
                    {
                        swarm[i].Vector[j] += swarm[i].Velocity[j];
                        if (swarm[i][j] < lowerBound)
                        {
                            swarm[i].Vector[j] = lowerBound;
                            death = 0;
                        }
                        if (swarm[i][j] > upperBound)
                        {
                            swarm[i].Vector[j] = upperBound;
                            death = 0;
                        }
                    }

                    // update new cost, F(x(t+1))
                    swarm[i].Update(swarm.ObjectiveFunction(swarm[i].Vector), swarm[i].Vector);
                    if (bestGlobalCost - swarm[i].BestValue > Tolerance)
                    {
                        bestGlobalPosition = swarm[i].BestVector;
                        bestGlobalCost = swarm[i].BestValue;
                    }
                    if (death > 0.05)
                    {
                        continue;
                    }
                    for (int j = 0; j < dimensions; j++)
                    {
                        swarm[i].Vector[j] = (upperBound - lowerBound) * swarm.RandomGenerator.NextDouble() + lowerBound;
                        swarm[i].Velocity[j] *= 0.05;
                    }
                    swarm[i].Update(swarm.ObjectiveFunction(swarm[i].Vector), swarm[i].Vector);
                }

                //
                // Line search from bestPosition to bestGlobalPosition
                //
                for (int i = 0; i < particles; i++)
                {
                    for (int j = 0; j < dimensions; j++)
                    {
                        currentToGlobalTransformation[j] = bestGlobalPosition[j] - swarm[i][j];
                    }
                    for (int j = 0; j < dimensions; j++)
                    {
                        double[] virtualPositionN = new double[dimensions];
                        for (int k = 0; k < dimensions; k++)
                        {
                            virtualPositionN[k] = currentToGlobalTransformation[k] * (j + 1) / (dimensions + 1);
                            virtualPositionN[k] += swarm[i][k];
                        }
                        double virtualCost = swarm.ObjectiveFunction(virtualPositionN);
                        if (virtualBestCost - virtualCost < Tolerance)
                        {
                            continue;
                        }
                        virtualBestPosition = virtualPositionN;
                        virtualBestCost = virtualCost;
                    }
                }
                if (bestGlobalCost - virtualBestCost < Tolerance)
                {
                    continue;
                }
                bestGlobalPosition = virtualBestPosition;
                bestGlobalCost = virtualBestCost;
            }
            return swarm.Min();
        }
    }
}