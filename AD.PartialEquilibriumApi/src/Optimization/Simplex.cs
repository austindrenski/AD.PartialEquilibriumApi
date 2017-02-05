using System;
using JetBrains.Annotations;

namespace AD.PartialEquilibriumApi.Optimization
{
    /// <summary>
    /// This class represents an implementation of the Nelder-Mead Simplex algorithm. 
    /// Based on an algorithm demonstrated by Dr. James McCaffrey, June 2013.
    /// https://msdn.microsoft.com/en-us/magazine/dn201752.aspx
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
        /// The maximum number of iterations to attempt.
        /// </summary>
        public int MaximumIterations { get; }

        /// <summary>
        /// The number of solutions to employ. Equal to the number of vertices.
        /// </summary>
        public int NumberOfSolutions { get; }

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
        public Solution[] Solutions { get; }

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
        /// Creates a simplex with the given parameters.
        /// </summary>
        /// <param name="numberOfSolutions">The number of <see cref="Solution"/> objects to use in the <see cref="Simplex"/>.</param>
        /// <param name="dimensions">The length of the argument vector.</param>
        /// <param name="lowerBound">The lower bound of the search space.</param>
        /// <param name="upperBound">The upper bound of the search space.</param>
        /// <param name="maximumIterations">The maximum number of iterations to attempt.</param>
        /// <param name="objectiveFunction">The function to minimize.</param>
        public Simplex(int numberOfSolutions, int dimensions, double lowerBound, double upperBound, int maximumIterations, Func<double[], double> objectiveFunction)
        {
            Dimensions = dimensions;
            MaximumIterations = maximumIterations;
            NumberOfSolutions = numberOfSolutions;
            ObjectiveFunction = objectiveFunction;
            LowerBound = lowerBound;
            UpperBound = upperBound;
            Solutions = new Solution[numberOfSolutions];

            Random random = new Random(0);
            for (int i = 0; i < Solutions.Length; i++)
            {
                double[] vector = new double[dimensions];
                for (int j = 0; j < dimensions; j++)
                {
                    vector[j] = (upperBound - lowerBound) * random.NextDouble() + lowerBound;
                }
                Solutions[i] = new Solution(objectiveFunction(vector), vector);
            }
            Array.Sort(Solutions);
        }
        
        /// <summary>
        /// Solves for the minimum value solution.
        /// </summary>
        /// <returns>The solution that produces the minimum value.</returns>
        public Solution Solve()
        {
            for (int i = 0; i < MaximumIterations; i++)
            {
                if (i % 10 == 0)
                {
                    Console.WriteLine($"At t = {i} current best solution = {Solutions[0]}");
                }

                Solution centroid = this.Centroid();
                Solution reflected = this.Reflected(centroid);

                if (reflected < Solutions[0])
                {
                    Solution expanded = this.Expanded(centroid, reflected);
                    this.ReplaceWorst(expanded < Solutions[0] ? expanded : reflected);
                    continue;
                }

                if (this.IsSecondWorst(reflected))
                {
                    if (reflected <= Solutions[NumberOfSolutions - 1])
                    {
                        this.ReplaceWorst(reflected);
                    }

                    Solution contracted = this.Contracted(centroid);

                    if (contracted > Solutions[NumberOfSolutions - 1])
                    {
                        this.Shrink();
                    }
                    else
                    {
                        this.ReplaceWorst(contracted);
                    }
                    continue;
                }

                this.ReplaceWorst(reflected);
            }

            return Solutions[0];
        }
    }
}
