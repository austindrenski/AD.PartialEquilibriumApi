using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace AD.PartialEquilibriumApi.PSO
{
    /// <summary>
    /// This class holds a collection of <see cref="Particle"/> objects.
    /// </summary>
    [PublicAPI]
    public class Swarm
    {
        /// <summary>
        /// The minimum value found by each <see cref="Particle"/>.
        /// </summary>
        public IEnumerable<double> ResultValues
        {
            get
            {
                return Particles.Select(x => x.BestCost);
            }
        }

        /// <summary>
        /// The minimum value found by the <see cref="Swarm"/>.
        /// </summary>
        public double BestCost
        {
            get
            {
                return Particles.Select(x => x.BestCost).Min();
            }
        }

        /// <summary>
        /// The minimum positions found by each <see cref="Particle"/>.
        /// </summary>
        public IEnumerable<IReadOnlyList<double>> ResultPositions
        {
            get
            {
                return Particles.Select(x => x.BestPosition);
            }
        }

        /// <summary>
        /// The minimum location found by the <see cref="Swarm"/>.
        /// </summary>
        public IReadOnlyList<double> BestPosition
        {
            get
            {
                return Particles.Aggregate((current, x) => x.BestCost < current.BestCost ? x : current)
                                .BestPosition;
            }
        }

        /// <summary>
        /// The collection of <see cref="Particle"/> objects.
        /// </summary>
        public Particle[] Particles { get; internal set; }

        /// <summary>
        /// Indexed access to the vector of <see cref="Particle"/> objects.
        /// </summary>
        /// <param name="index">The particle index.</param>
        public Particle this[int index]
        {
            get
            {
                return Particles[index];
            }
            set
            {
                Particles[index] = value;
            }
        }

        /// <summary>
        /// The number of particles in the swarm.
        /// </summary>
        public int NumberOfParticles
        {
            get
            {
                return Particles.Length;
            }
        }

        /// <summary>
        /// The count of iterations to perform without improvement prior to exiting.
        /// </summary>
        public int MaximumIterations { get; }

        /// <summary>
        /// The objective function to minimize.
        /// </summary>
        public Func<double[], double> ObjectiveFunction { get; }

        /// <summary>
        /// Generates random numbers.
        /// </summary>
        public Random RandomGenerator { get; }

        /// <summary>
        /// Initializes a swarm with the specified number of particles.
        /// </summary>
        /// <param name="seed"></param>
        /// <param name="count">The number of particles to create.</param>
        /// <param name="variableCount">The length of the variable vector in objectiveFunction</param>
        /// <param name="upperBound"></param>
        /// <param name="objectiveFunction"></param>
        /// <param name="lowerBound"></param>
        public Swarm(int seed, int count, int variableCount, double lowerBound, double upperBound, Func<double[], double> objectiveFunction)
        {
            RandomGenerator = new Random(seed);
            Particles = new Particle[count];
            ObjectiveFunction = objectiveFunction;
            for (int i = 0; i < count; i++)
            {
                double[] randomPosition = new double[variableCount];
                double[] randomVelocity = new double[variableCount];
                for (int j = 0; j < variableCount; j++)
                {
                    randomPosition[j] = (upperBound - lowerBound) * RandomGenerator.NextDouble() + lowerBound;
                    randomVelocity[j] = 1e-5 * randomPosition[j];
                }
                double cost = ObjectiveFunction(randomPosition);
                Particles[i] = new Particle(cost, randomPosition, randomVelocity);
            }
        }

        /// <summary>
        /// Searches for a minimum value.
        /// </summary>
        public void Optimize(int objectiveVariableCount, double lowerBound, double upperBound)
        {
            Particles = OptimizationFactory.ParticleSwarmOptimization(RandomGenerator, this, ObjectiveFunction, objectiveVariableCount, NumberOfParticles, MaximumIterations, lowerBound, upperBound);
        }
    }
}