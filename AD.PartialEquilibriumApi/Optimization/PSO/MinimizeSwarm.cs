using JetBrains.Annotations;

namespace AD.PartialEquilibriumApi.Optimization.PSO
{
    /// <summary>
    /// Extension methods for minimization using a <see cref="Swarm"/> object.
    /// </summary>
    [PublicAPI]
    public static class MinimizeSwarmExtensions
    {
        private const double Tolerance = 1e-15;

        /// <summary>
        /// This method executes the optimization algorithm. It is called privately by PsoPe.Optimize().
        /// </summary>
        /// <param name="swarm">The swarm used to find minimum values.</param>
        /// <returns>Returns a double[] of the best position found by the swarm.</returns>
        public static Solution Minimize(this Swarm swarm)
        {
            int dimensions = swarm.Dimensions;
            int iterations = swarm.Iterations;
            int particles = swarm.Particles;
            double[] currentToGlobalTransformation = new double[dimensions];

            // Main processing loop
            for (int h = 0; h < iterations; h++)
            {
                swarm.TextWriter.WriteLineAsync($"> i = {$"{h}".PadLeft(swarm.Iterations.ToString().Length)}: {swarm.GlobalBest}");

                for (int i = 0; i < particles; i++)
                {
                    swarm[i].SetSurvival(swarm);

                    double[] velocity = swarm.Velocity(swarm[i]);
                    swarm[i].SetVelocity(velocity);

                    Solution current = swarm.Position(swarm[i]);
                    swarm[i].SetCurrent(current);
                    
                    if (swarm.GlobalBest.Value - swarm[i].Best.Value > Tolerance)
                    {
                        swarm.GlobalBest = new Solution(swarm[i].Best);
                    }
                    swarm[i] = swarm.Survival(swarm[i]);
                }

                // Line search from bestPosition to bestGlobalPosition
                for (int i = 0; i < particles; i++)
                {
                    for (int j = 0; j < dimensions; j++)
                    {
                        currentToGlobalTransformation[j] = swarm.GlobalBest[j] - swarm[i].Current[j];
                    }
                    for (int j = 0; j < dimensions; j++)
                    {
                        double[] virtualPositionN = new double[dimensions];
                        for (int k = 0; k < dimensions; k++)
                        {
                            virtualPositionN[k] = currentToGlobalTransformation[k] * (j + 1) / (dimensions + 1);
                            virtualPositionN[k] += swarm[i].Current[k];
                        }

                        double virtualCost = swarm.ObjectiveFunction(virtualPositionN);
                        if (swarm.VirtualBest.Value - virtualCost < Tolerance)
                        {
                            continue;
                        }
                        swarm.VirtualBest = new Solution(virtualCost, virtualPositionN);
                    }
                }
                if (swarm.GlobalBest.Value - swarm.VirtualBest.Value < Tolerance)
                {
                    continue;
                }
                swarm.GlobalBest = new Solution(swarm.VirtualBest);
            }
            return swarm.GlobalBest;
        }
    }
}