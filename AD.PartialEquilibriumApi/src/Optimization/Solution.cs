using System;
using System.Linq;
using JetBrains.Annotations;

namespace AD.PartialEquilibriumApi
{
    /// <summary>
    /// Represents the solution to an objective function.
    /// </summary>
    [PublicAPI]
    public struct Solution : IEquatable<Solution>, IComparable<Solution>
    {
        private const double Tolerance = 1e-15;

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
        /// Returns a string representation of the <see cref="Solution"/>.
        /// </summary>
        public override string ToString()
        {
            return $"[ {string.Join(", ", Vector.Select(x => $"{x:0.0000e00}"))} ] = {Value:0.0000e00}";
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
            return left.Value - right.Value < Tolerance;
        }

        /// <summary>
        /// True if the value of the left solution is greater than the value of the right solution.
        /// </summary>
        public static bool operator >(Solution left, Solution right)
        {
            return left.Value - right.Value > Tolerance;
        }

        /// <summary>
        /// True if the value of the left solution is less than or equal to the value of the right solution.
        /// </summary>
        public static bool operator <=(Solution left, Solution right)
        {
            return left.Value - right.Value <= Tolerance;
        }

        /// <summary>
        /// True if the value of the left solution is greater than or equal to the value of the right solution.
        /// </summary>
        public static bool operator >=(Solution left, Solution right)
        {
            return left.Value - right.Value >= Tolerance;
        }

        /// <summary>
        /// True if two solutions have the same Vector and Value.
        /// </summary>
        public bool Equals(Solution other)
        {
            if (Vector.Length != other.Vector.Length)
            {
                return false;
            }
            bool vectorsEqual = true;
            for (int i = 0; i < Vector.Length; i++)
            {
                if (Math.Abs(Vector[i] - other.Vector[i]) > double.Epsilon)
                {
                    vectorsEqual = false;
                }
            }
            return vectorsEqual && Value.Equals(other.Value);
        }

        /// <summary>
        /// True if two solutions have the same Vector and Value.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            return obj is Solution && Equals((Solution)obj);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return ((Vector?.GetHashCode() ?? 0) * 397) ^ Value.GetHashCode();
            }
        }
    }
}
