using System;
using System.IO;
using JetBrains.Annotations;

namespace AD.PartialEquilibriumApi
{
    /// <summary>
    /// This class implements a Nelder-Mead style simplex algorithm to minimize an objective function.
    /// </summary>
    [PublicAPI]
    public class Simplex
    {
        /// <summary>
        /// The dimensions of the simplex. Equal to the length of the argument vectors.
        /// </summary>
        public int Dimensions { get; }

        /// <summary>
        /// The objective function.
        /// </summary>
        public Func<double[], double> ObjectiveFunction { get; }

        /// <summary>
        /// The number of iterations to attempt.
        /// </summary>
        public int Iterations { get; }

        /// <summary>
        /// The number of solutions (vertices) of the simplex. Initially set equal to Simplex.Dimensions + 1.
        /// Increasing the number of solutions may help when search space is complex.
        /// </summary>
        public int NumberOfSolutions { get; }

        /// <summary>
        /// Equal to NumberOfSolutions - 1.
        /// </summary>
        public int LastIndex
        {
            get
            {
                return NumberOfSolutions - 1;
            }
        }

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
        public Solution[] Solutions { get; set; }

        /// <summary>
        /// Indexed access to the vector of <see cref="Solution"/> objects..
        /// </summary>
        /// <param name="index">The solution index.</param>
        public Solution this[int index]
        {
            get
            {
                return Solutions[index];
            }
        }

        /// <summary>
        /// The alpha constant.
        /// </summary>
        public const double Reflection = 1.0;

        /// <summary>
        /// The beta constant.
        /// </summary>
        public const double Contraction = 0.5;

        /// <summary>
        /// The gamma constant.
        /// </summary>
        public const double Expansion = 2.0;

        /// <summary>
        /// The numerical precision used for floating poing comparisons. Initially set equal to 1e-15.
        /// Increasing the numerical precision may help when the search space is complex, or corner solutions may exist.
        /// </summary>
        public double Precision { get; set; } = 1e-15;

        /// <summary>
        /// Random number generator.
        /// </summary>
        public static readonly Random RandomGenerator = new Random(0);

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
        /// <param name="textWriter">Set this property to the standard output for progress reporting.</param>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public Simplex(Func<double[], double> objectiveFunction, double lowerBound, double upperBound, int dimensions, int iterations, TextWriter textWriter = null)
        {
            if (lowerBound > upperBound)
            {
                throw new ArgumentOutOfRangeException("The lower bound must be less than or equal to the upper bound.");
            }
            if (iterations < 1)
            {
                throw new ArgumentOutOfRangeException("The iteration count must be greater than zero.");
            }
            TextWriter = textWriter ?? new StringWriter();
            Dimensions = dimensions;
            Iterations = iterations;
            NumberOfSolutions = dimensions + 1;
            ObjectiveFunction = objectiveFunction;
            LowerBound = lowerBound;
            UpperBound = upperBound;
            Solutions = new Solution[NumberOfSolutions];

            for (int i = 0; i < Solutions.Length; i++)
            {
                Solutions[i] = this.Random();
            }

            Array.Sort(Solutions);
        }

        /// <summary>
        /// Creates a simplex with the given parameters.
        /// </summary>
        /// <param name="objectiveFunction">The function to minimize.</param>
        /// <param name="lowerBound">The lower bound of the search space. Must be less than or equal to the upper bound.</param>
        /// <param name="upperBound">The upper bound of the search space. Must be greater than or equal to the lower bound.</param>
        /// <param name="dimensions">The length of the argument vector.</param>
        /// <param name="textWriter">Set this property to the standard output for progress reporting. If null, a <see cref="StringWriter"/> is initialized.</param>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public Simplex(Func<double[], double> objectiveFunction, double lowerBound, double upperBound, int dimensions, TextWriter textWriter = null)
            : this(objectiveFunction, lowerBound, upperBound, dimensions, dimensions < 5 ? 1000 : dimensions * 200, textWriter)
        {
        }
    }
}