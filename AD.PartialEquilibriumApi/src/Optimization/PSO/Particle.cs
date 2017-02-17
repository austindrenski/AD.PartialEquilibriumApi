using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace AD.PartialEquilibriumApi.PSO
{
    /// <summary>
    /// This class initializes a single Particle object.
    /// </summary>
    [PublicAPI]
    public struct Particle : IEquatable<Particle>, IComparable<Particle>
    {
        private const double Tolerance = 1e-15;

        /// <summary>
        /// The Particle's value at the current vector.
        /// </summary>
        public double Value { get; private set; }

        /// <summary>
        /// The Particle's current vector
        /// </summary>
        public double[] Vector { get; }

        /// <summary>
        /// The Particle's value at the current best vector.
        /// </summary>
        public double BestValue { get; private set; }
        
        /// <summary>
        /// The Particle's best vector at present.
        /// </summary>
        public double[] BestVector { get; }

        /// <summary>
        /// The Particle's current adjustment vector.
        /// </summary>
        public double[] Velocity { get; }

        /// <summary>
        /// Indexed access to the current vector.
        /// </summary>
        /// <param name="index">The vector element index.</param>
        public double this[int index]
        {
            get { return Vector[index]; }
        }

        /// <summary>
        /// A <see cref="Particle"/> object for use in Particle Swarm Optimization.
        /// </summary>
        /// <param name="vector">The current vector.</param>
        /// <param name="value">The value of the current vector.</param>
        /// <param name="velocity">The current adjustment vector.</param>
        internal Particle(double value, IReadOnlyList<double> vector, IReadOnlyList<double> velocity)
        {
            if (vector.Count != velocity.Count)
            {
                throw new ArgumentOutOfRangeException("Position and velocity vectors must be equal length.");
            }
            int length = vector.Count;
            Value = value;
            BestValue = value;
            Vector = new double[length];
            BestVector = new double[length];
            Velocity = new double[length];
            for (int i = 0; i < length; i++)
            {
                Vector[i] = vector[i];
                BestVector[i] = vector[i];
                Velocity[i] = velocity[i];
            }
        }

        /// <summary>
        /// Updates the <see cref="Particle"/> with the new value and vector information.
        /// </summary>
        /// <param name="value">The function value at the current vector.</param>
        /// <param name="vector">The current vector.</param>
        public void Update(double value, double[] vector)
        {
            Value = value;
            for (int i = 0; i < vector.Length; i++)
            {
                Vector[i] = vector[i];
            }
            if (BestValue < value)
            {
                return;
            }
            BestValue = value;
            for (int i = 0; i < vector.Length; i++)
            {
                BestVector[i] = vector[i];
            }
        }

        /// <summary>
        /// Returns a string representation of the <see cref="Particle"/>.
        /// </summary>
        public override string ToString()
        {
            return $"[ {string.Join(", ", BestVector.Select(x => $"{x:0.0000e00}"))} ] = {BestValue:0.0000e00}";
        }

        /// <summary>
        /// True if both <see cref="Particle"/> objects have the same value.
        /// </summary>
        public static bool operator ==(Particle left, Particle right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// True if both <see cref="Particle"/> objects do not have the same value.
        /// </summary>
        public static bool operator !=(Particle left, Particle right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// True if the left value is less than the right value.
        /// </summary>
        public static bool operator <(Particle left, Particle right)
        {
            return left.Value - right.Value < Tolerance;
        }

        /// <summary>
        /// True if the left value is greater than the right value.
        /// </summary>
        public static bool operator >(Particle left, Particle right)
        {
            return left.Value - right.Value > Tolerance;
        }

        /// <summary>
        /// True if the left value is less than or equal to the right value.
        /// </summary>
        public static bool operator <=(Particle left, Particle right)
        {
            return left.Value - right.Value <= Tolerance;
        }

        /// <summary>
        /// True if the left value is greater than or equal to the right value.
        /// </summary>
        public static bool operator >=(Particle left, Particle right)
        {
            return left.Value - right.Value >= Tolerance;
        }

        /// <summary>
        /// Compares <see cref="Particle"/> objects based on the best value.
        /// </summary>
        public int CompareTo(Particle other)
        {
            return BestValue.CompareTo(other.BestValue);
        }

        /// <summary>
        /// Compares <see cref="Particle"/> objects based on the best value.
        /// </summary>
        int IComparable<Particle>.CompareTo(Particle other)
        {
            return CompareTo(other);
        }

        /// <summary>
        /// True if two <see cref="Particle"/> objects have the same current vector and value.
        /// </summary>
        public bool Equals(Particle other)
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
        /// True if two <see cref="Particle"/> objects have the same current vector and value.
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj is Particle && Equals((Particle)obj);
        }

        /// <summary>
        /// True if two <see cref="Particle"/> objects have the same current vector and value.
        /// </summary>
        bool IEquatable<Particle>.Equals(Particle other)
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
                return (Value.GetHashCode() * 397) ^ (Vector?.GetHashCode() ?? 0);
            }
        }
    }
}
