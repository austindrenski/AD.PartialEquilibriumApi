using System;
using System.Collections.Generic;
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
        /// The Particle's value at the current position.
        /// </summary>
        public double Cost { get; internal set; }

        /// <summary>
        /// The Particle's value at the current best position.
        /// </summary>
        public double BestCost { get; internal set; }
        
        /// <summary>
        /// The Particle's current position
        /// </summary>
        public double[] Position { get; internal set; }

        /// <summary>
        /// The Particle's best position at present.
        /// </summary>
        public double[] BestPosition { get; internal set; }

        /// <summary>
        /// The Particle's current adjustment vector.
        /// </summary>
        public double[] Velocity { get; internal set; }

        /// <summary>
        /// A <see cref="Particle"/> object for use in Particle Swarm Optimization.
        /// </summary>
        /// <param name="position">The current position.</param>
        /// <param name="cost">The value of the current position.</param>
        /// <param name="velocity">The current adjustment vector.</param>
        internal Particle(double cost, IReadOnlyList<double> position, IReadOnlyList<double> velocity)
        {
            if (position.Count != velocity.Count)
            {
                throw new ArgumentOutOfRangeException("Position and velocity vectors must be equal length.");
            }
            int length = position.Count;
            Cost = cost;
            BestCost = cost;
            Position = new double[length];
            BestPosition = new double[length];
            Velocity = new double[length];
            for (int i = 0; i < length; i++)
            {
                Position[i] = position[i];
                BestPosition[i] = position[i];
                Velocity[i] = velocity[i];
            }
        }

        public bool Equals(Particle other)
        {
            throw new NotImplementedException();
        }

        public int CompareTo(Particle other)
        {
            throw new NotImplementedException();
        }
    }
}
