using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;

namespace AD.PartialEquilibriumApi.Optimization.PSO
{
    /// <summary>
    /// This class holds a collection of <see cref="Particle"/> objects.
    /// </summary>
    [PublicAPI]
    public class Swarm : IEnumerable<Particle>
    {
        /// <summary>
        /// The collection of <see cref="Particle"/> objects.
        /// </summary>
        private Particle[] _particles;

        /// <summary>
        /// Indexed access to the vector of <see cref="Particle"/> objects.
        /// </summary>
        /// <param name="index">The particle index.</param>
        public Particle this[int index]
        {
            get { return _particles[index]; }
            set { _particles[index] = value; }
        }

        /// <summary>
        /// The best solution found by any <see cref="Particle"/> in the <see cref="Swarm"/>.
        /// </summary>
        public Solution GlobalBest { get; set; }

        /// <summary>
        /// The best solution along the path from any position of any <see cref="Particle"/> to the best solution found by the <see cref="Swarm"/>.
        /// </summary>
        public Solution VirtualBest { get; set; }

        /// <summary>
        /// The count of variables in the objective function.
        /// </summary>
        public int Dimensions { get; }
    
        /// <summary>
        /// The count of iterations to perform without improvement prior to exiting.
        /// </summary>
        public int Iterations { get; }

        /// <summary>
        /// The number of particles in the swarm.
        /// </summary>
        public int Particles { get; }

        /// <summary>
        /// The lower bound of the search space.
        /// </summary>
        public double LowerBound { get; }

        /// <summary>
        /// The upper bound of the search space.
        /// </summary>
        public double UpperBound { get; }

        /// <summary>
        /// Generates random numbers.
        /// </summary>
        public Random RandomGenerator { get; }

        /// <summary>
        /// Set this property to the standard output for progress reporting.
        /// </summary>
        public TextWriter TextWriter { get; }

        /// <summary>
        /// The objective function to minimize.
        /// </summary>
        public Func<double[], double> ObjectiveFunction { get; }

        /// <summary>
        /// Returns an <see cref="IEnumerable"/>  copy of the internal <see cref="Particle"/> collection.
        /// </summary>
        public IEnumerable<Particle> ParticleCollection
        {
            get { return _particles; }
        }

        /// <summary>
        /// Creates a <see cref="Swarm"/> with the given parameters.
        /// </summary>
        /// <param name="objectiveFunction">The function to minimize.</param>
        /// <param name="lowerBound">The lower bound of the search space. Must be less than or equal to the upper bound.</param>
        /// <param name="upperBound">The upper bound of the search space. Must be greater than or equal to the lower bound.</param>
        /// <param name="dimensions">The length of the argument vector.</param>
        /// <param name="iterations">The number of iterations to attempt. Must be greater than zero.</param>
        /// <param name="particles">The number of particles to create.</param>
        /// <param name="seed">A seed value for the internal random number generator.</param>
        /// <param name="textWriter">Set this property to the standard output for progress reporting.</param>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public Swarm(Func<double[], double> objectiveFunction, double lowerBound, double upperBound, int dimensions, int iterations, int particles, int? seed = null, TextWriter textWriter = null)
        {
            if (dimensions < 1)
            {
                throw new ArgumentOutOfRangeException("The dimensions must be greater than or equal to one.");
            }
            if (lowerBound > upperBound)
            {
                throw new ArgumentOutOfRangeException("The lower bound must be less than or equal to the upper bound.");
            }
            if (particles < 1)
            {
                throw new ArgumentOutOfRangeException("The particle count must be greater than zero.");
            }
            TextWriter = textWriter ?? new StringWriter();
            RandomGenerator = seed == null ? new Random() : new Random((int)seed);
            GlobalBest = new Solution(double.MaxValue, new double[dimensions]);
            VirtualBest = new Solution(double.MaxValue, new double[dimensions]);
            Dimensions = dimensions;
            Iterations = iterations;
            Particles = particles;
            LowerBound = lowerBound;
            UpperBound = upperBound;
            _particles = new Particle[particles];
            ObjectiveFunction = objectiveFunction;
            for (int i = 0; i < particles; i++)
            {
                double[] randomVector = new double[dimensions];
                double[] randomVelocity = new double[dimensions];
                for (int j = 0; j < dimensions; j++)
                {
                    randomVector[j] = (upperBound - lowerBound) * RandomGenerator.NextDouble() + lowerBound;
                    randomVelocity[j] = 1e-05 * randomVector[j];
                }
                double value = ObjectiveFunction(randomVector);
                _particles[i] = new Particle(value, randomVector, randomVelocity);
                if (GlobalBest.Value < _particles[i].Best.Value)
                {
                    continue;
                }
                GlobalBest = new Solution(_particles[i].Best);
                VirtualBest = new Solution(_particles[i].Best);
            }
        }

        /// <summary>
        /// Creates a <see cref="Swarm"/> with the given parameters.
        /// </summary>
        /// <param name="objectiveFunction">The function to minimize.</param>
        /// <param name="lowerBound">The lower bound of the search space. Must be less than or equal to the upper bound.</param>
        /// <param name="upperBound">The upper bound of the search space. Must be greater than or equal to the lower bound.</param>
        /// <param name="dimensions">The length of the argument vector.</param>
        /// <param name="seed">A seed value for the internal random number generator.</param>
        /// <param name="textWriter">Set this property to the standard output for progress reporting.</param>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public Swarm(Func<double[], double> objectiveFunction, double lowerBound, double upperBound, int dimensions, int? seed = null, TextWriter textWriter = null)
            : this(objectiveFunction, lowerBound, upperBound, dimensions, dimensions * 1000, dimensions * 100, seed, textWriter)
        {
        }

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="Particle"/> collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the <see cref="Particle"/> collection.</returns>
        public IEnumerator<Particle> GetEnumerator()
        {
            return ParticleCollection.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="Particle"/> collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the <see cref="Particle"/> collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}