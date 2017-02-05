﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace AD.PartialEquilibriumApi.Optimization
{
    /// <summary>
    /// This class holds a collection of <see cref="Particle"/> objects.
    /// </summary>
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
        public int MaximumIterations { get; set; }

        /// <summary>
        /// The objective function to minimize.
        /// </summary>
        public Func<double[], double> ObjectiveFunction { get; }

        /// <summary>
        /// Generates random numbers.
        /// </summary>
        private Random _randomNumberGenerator;

        /// <summary>
        /// Initializes a swarm with the specified number of particles.
        /// </summary>
        /// <param name="count">The number of particles to create.</param>
        /// <param name="variableCount">The length of the variable vector in objectiveFunction</param>
        /// <param name="objectiveFunction"></param>
        public Swarm(int seed, int count, int variableCount, double lowerBound, double upperBound, Func<double[], double> objectiveFunction)
        {
            _randomNumberGenerator = new Random(seed);
            Particles = new Particle[count];
            ObjectiveFunction = objectiveFunction;
            for (int i = 0; i < count; i++)
            {
                double[] randomPosition = new double[variableCount];
                double[] randomVelocity = new double[variableCount];
                for (int j = 0; j < variableCount; j++)
                {
                    randomPosition[j] = (upperBound - lowerBound) * _randomNumberGenerator.NextDouble() + lowerBound;
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
            Particles = OptimizationFactory.ParticleSwarmOptimization(_randomNumberGenerator, this, ObjectiveFunction, objectiveVariableCount, NumberOfParticles, MaximumIterations, lowerBound, upperBound);
        }
    }
}