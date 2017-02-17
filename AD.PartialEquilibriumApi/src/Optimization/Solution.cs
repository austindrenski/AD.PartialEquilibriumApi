using System;
using System.Collections.Generic;
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
        /// The function result.
        /// </summary>
        public double Value { get; }

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
            get { return Vector[index]; }
        }

        /// <summary>
        /// Creates a solution given an argument vector and value.
        /// </summary>
        /// <param name="value">The value of this solution.</param>
        /// <param name="vector">A vector of arguments to be passed to the function.</param>
        public Solution(double value, IReadOnlyList<double> vector)
        {
            Vector = new double[vector.Count];
            for (int i = 0; i < vector.Count; i++)
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
        /// True if both <see cref="Solution"/> objects have the same value.
        /// </summary>
        public static bool operator ==(Solution left, Solution right)
        {
            return Math.Abs(left.Value - right.Value) < Tolerance;
        }

        /// <summary>
        /// True if both <see cref="Solution"/> objects do not have the same value.
        /// </summary>
        public static bool operator !=(Solution left, Solution right)
        {
            return Math.Abs(left.Value - right.Value) > Tolerance;
        }

        /// <summary>
        /// True if the left value is less than the right value.
        /// </summary>
        public static bool operator <(Solution left, Solution right)
        {
            return left.Value - right.Value < Tolerance;
        }

        /// <summary>
        /// True if the left value is greater than the right value.
        /// </summary>
        public static bool operator >(Solution left, Solution right)
        {
            return left.Value - right.Value > Tolerance;
        }

        /// <summary>
        /// True if the left value is less than or equal to the right value.
        /// </summary>
        public static bool operator <=(Solution left, Solution right)
        {
            return left.Value - right.Value <= Tolerance;
        }

        /// <summary>
        /// True if the left value is greater than or equal to the right value.
        /// </summary>
        public static bool operator >=(Solution left, Solution right)
        {
            return left.Value - right.Value >= Tolerance;
        }

        /// <summary>
        /// Compares <see cref="Solution"/> objects based on the values.
        /// </summary>
        public int CompareTo(Solution other)
        {
            return Value.CompareTo(other.Value);
        }

        /// <summary>
        /// Compares <see cref="Solution"/> objects based on the values.
        /// </summary>
        int IComparable<Solution>.CompareTo(Solution other)
        {
            return CompareTo(other);
        }

        /// <summary>
        /// True if two <see cref="Solution"/> objects have the same Vector and Value.
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
                if (Math.Abs(Vector[i] - other.Vector[i]) > Tolerance)
                {
                    vectorsEqual = false;
                }
            }
            return vectorsEqual && Value.Equals(other.Value);
        }

        /// <summary>
        /// True if two <see cref="Solution"/> objects have the same Vector and Value.
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj is Solution && Equals((Solution) obj);
        }

        /// <summary>
        /// True if two <see cref="Solution"/> objects have the same Vector and Value.
        /// </summary>
        bool IEquatable<Solution>.Equals(Solution other)
        {
            return Equals(other);
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
