using System;
using JetBrains.Annotations;

namespace AD.PartialEquilibriumApi.Optimization
{
    /// <summary>
    /// Represents the solution to an objective function.
    /// </summary>
    [PublicAPI]
    public struct Solution : IComparable<Solution>
    {
        /// <summary>
        /// Returns the argument vector. An indexer is provided for set operations.
        /// </summary>
        public double[] Vector { get; }

        /// <summary>
        /// Indexed access to the solution vector.
        /// </summary>
        /// <param name="index">The vector element index.</param>
        public double this[int index]
        {
            get
            {
                return Vector[index];
            }
            set
            {
                Vector[index] = value;
            }
        }

        /// <summary>
        /// The function result.
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// Creates a solution given an argument vector and value.
        /// </summary>
        /// <param name="value">The value of this solution.</param>
        /// <param name="vector">A vector of arguments to be passed to the function.</param>
        public Solution(double value, double[] vector)
        {
            Vector = new double[vector.Length];
            for (int i = 0; i < vector.Length; i++)
            {
                Vector[i] = vector[i];
            }
            Value = value;
        }

        /// <summary>
        /// Compares <see cref="Solution"/> objects based on the values.
        /// </summary>
        /// <param name="other">The solution to which the comparison is made.</param>
        public int CompareTo(Solution other)
        {
            return Value.CompareTo(other.Value);
        }

        /// <summary>
        /// True if the value of the left solution is less than the value of the right solution.
        /// </summary>
        public static bool operator <(Solution left, Solution right)
        {
            return left.Value < right.Value;
        }

        /// <summary>
        /// True if the value of the left solution is greater than the value of the right solution.
        /// </summary>
        public static bool operator >(Solution left, Solution right)
        {
            return left.Value > right.Value;
        }

        /// <summary>
        /// True if the value of the left solution is less than or equal to the value of the right solution.
        /// </summary>
        public static bool operator <=(Solution left, Solution right)
        {
            return left.Value <= right.Value;
        }

        /// <summary>
        /// True if the value of the left solution is greater than or equal to the value of the right solution.
        /// </summary>
        public static bool operator >=(Solution left, Solution right)
        {
            return left.Value >= right.Value;
        }

        /// <summary>
        /// True if the value of the solution is less than any of the solutions in the <see cref="Simplex"/>.
        /// </summary>
        public static bool operator <(Solution solution, Simplex simplex)
        {
            int betterThan = 0;
            for (int i = 0; i < simplex.NumberOfSolutions; i++)
            {
                if (solution < simplex.Solutions[i])
                {
                    betterThan++;
                }
            }
            return betterThan > 0;
        }

        /// <summary>
        /// True if the value of the solution is greater than any of the solutions in the <see cref="Simplex"/>.
        /// </summary>
        public static bool operator >(Solution solution, Simplex simplex)
        {
            int betterThan = 0;
            for (int i = 0; i < simplex.NumberOfSolutions; i++)
            {
                if (solution > simplex.Solutions[i])
                {
                    betterThan++;
                }
            }
            return betterThan > 0;
        }
        
        /// <summary>
        /// True if the value of the solution is less than or equal to any of the solutions in the <see cref="Simplex"/>.
        /// </summary>
        public static bool operator <=(Solution solution, Simplex simplex)
        {
            int betterThan = 0;
            for (int i = 0; i < simplex.NumberOfSolutions; i++)
            {
                if (solution <= simplex.Solutions[i])
                {
                    betterThan++;
                }
            }
            return betterThan > 0;
        }

        /// <summary>
        /// True if the value of the solution is greater than or equal to any of the solutions in the <see cref="Simplex"/>.
        /// </summary>
        public static bool operator >=(Solution solution, Simplex simplex)
        {
            int betterThan = 0;
            for (int i = 0; i < simplex.NumberOfSolutions; i++)
            {
                if (solution >= simplex.Solutions[i])
                {
                    betterThan++;
                }
            }
            return betterThan > 0;
        }

        /// <summary>
        /// Returns a string representation of the <see cref="Solution"/>.
        /// </summary>
        public override string ToString()
        {
            return $"[ {string.Join(", ", Vector)} ] = {Value}";
        }
    }
}
