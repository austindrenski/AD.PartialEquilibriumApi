using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;

namespace AD.PartialEquilibriumApi
{
    /// <summary>
    /// This class implements a Nelder-Mead style simplex algorithm to minimize an objective function.
    /// </summary>
    [PublicAPI]
    public class Simplex : IEnumerable<Solution>
    {
        /// <summary>
        /// The dimensions of the simplex. Equal to the length of the argument vectors.
        /// </summary>
        public int Dimensions { get; }

        /// <summary>
        /// The number of iterations to attempt.
        /// </summary>
        public int Iterations { get; }

        /// <summary>
        /// The number of solutions in the <see cref="Simplex"/>.
        /// </summary>
        public int SolutionCount { get; }

        /// <summary>
        /// The lower bound of the search space.
        /// </summary>
        public double LowerBound { get; }

        /// <summary>
        /// The upper bound of the search space.
        /// </summary>
        public double UpperBound { get; }

        /// <summary>
        /// The solutions that currently define the <see cref="Simplex"/>.
        /// </summary>
        private readonly Solution[] _solutions;

        /// <summary>
        /// Indexed access to the vector of <see cref="Solution"/> objects.
        /// </summary>
        /// <param name="index">The solution index.</param>
        public Solution this[int index]
        {
            get { return _solutions[index]; }
            set { _solutions[index] = value; }
        }

        /// <summary>
        /// The objective function.
        /// </summary>
        public Func<double[], double> ObjectiveFunction { get; }

        /// <summary>
        /// Random number generator.
        /// </summary>
        public Random RandomGenerator { get; }

        /// <summary>
        /// Set this property to the standard output for progress reporting.
        /// </summary>
        public TextWriter TextWriter { get; set; }

        /// <summary>
        /// Creates a simplex with the given parameters.
        /// </summary>
        /// <param name="objectiveFunction">The function to minimize.</param>
        /// <param name="lowerBound">The lower bound of the search space. Must be less than or equal to the upper bound.</param>
        /// <param name="upperBound">The upper bound of the search space. Must be greater than or equal to the lower bound.</param>
        /// <param name="dimensions">The length of the argument vector.</param>
        /// <param name="iterations">The number of iterations to attempt. Must be greater than zero.</param>
        /// <param name="seed">A seed value for the internal random number generator.</param>
        /// <param name="textWriter">Set this property to the standard output for progress reporting.</param>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public Simplex(Func<double[], double> objectiveFunction, double lowerBound, double upperBound, int dimensions, int iterations, int? seed = null, TextWriter textWriter = null)
        {
            if (dimensions < 1)
            {
                throw new ArgumentOutOfRangeException("The dimensions must be greater than or equal to one.");
            }
            if (lowerBound > upperBound)
            {
                throw new ArgumentOutOfRangeException("The lower bound must be less than or equal to the upper bound.");
            }
            if (iterations < 1)
            {
                throw new ArgumentOutOfRangeException("The iteration count must be greater than zero.");
            }
            TextWriter = textWriter ?? new StringWriter();
            RandomGenerator = seed == null ? new Random() : new Random((int) seed);
            Dimensions = dimensions;
            Iterations = iterations;
            ObjectiveFunction = objectiveFunction;
            LowerBound = lowerBound;
            UpperBound = upperBound;
            _solutions = new Solution[dimensions + 1];
            SolutionCount = _solutions.Length;
            for (int i = 0; i < _solutions.Length; i++)
            {
                _solutions[i] = this.Random();
            }
            Array.Sort(_solutions);
        }

        /// <summary>
        /// Creates a simplex with the given parameters.
        /// </summary>
        /// <param name="objectiveFunction">The function to minimize.</param>
        /// <param name="lowerBound">The lower bound of the search space. Must be less than or equal to the upper bound.</param>
        /// <param name="upperBound">The upper bound of the search space. Must be greater than or equal to the lower bound.</param>
        /// <param name="dimensions">The length of the argument vector.</param>
        /// <param name="seed">A seed value for the internal random number generator.</param>
        /// <param name="textWriter">Set this property to the standard output for progress reporting. If null, a <see cref="StringWriter"/> is initialized.</param>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public Simplex(Func<double[], double> objectiveFunction, double lowerBound, double upperBound, int dimensions, int? seed = null, TextWriter textWriter = null)
            : this(objectiveFunction, lowerBound, upperBound, dimensions, dimensions * 1000, seed, textWriter)
        {
        }

        /// <summary>
        /// Sorts the <see cref="Solution"/> objects in the <see cref="Simplex"/> using the <see cref="IComparable{T}"/> generic interface implementation.
        /// </summary>
        public void Sort()
        {
            Array.Sort(_solutions);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<Solution> GetEnumerator()
        {
            return ((IEnumerable<Solution>)_solutions).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _solutions.GetEnumerator();
        }
    }
}