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
        /// The argument vector.
        /// </summary>
        public double[] Vector { get; set; }

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
        /// <returns></returns>
        public int CompareTo(Solution other)
        {
            return Value.CompareTo(other.Value);
        }

        public static bool operator <(Solution left, Solution right)
        {
            return left.Value < right.Value;
        }

        public static bool operator >(Solution left, Solution right)
        {
            return left.Value > right.Value;
        }

        public static bool operator <=(Solution left, Solution right)
        {
            return left.Value <= right.Value;
        }

        public static bool operator >=(Solution left, Solution right)
        {
            return left.Value >= right.Value;
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
