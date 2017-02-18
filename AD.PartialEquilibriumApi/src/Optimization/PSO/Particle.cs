using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace AD.PartialEquilibriumApi.PSO
{
    /// <summary>
    /// This class initializes a single Particle object.
    /// </summary>
    [PublicAPI]
    public struct Particle
    {
        private const double Tolerance = 1e-15;

        /// <summary>
        /// The current solution of this <see cref="Particle"/>.
        /// </summary>
        public Solution Current { get; private set; }

        /// <summary>
        /// The best solution found by this <see cref="Particle"/>.
        /// </summary>
        public Solution Best { get; private set; }

        /// <summary>
        /// The current adjustment vector.
        /// </summary>
        public double[] Velocity { get; }

        /// <summary>
        /// The likelihood that the particle will survive the current iteration.
        /// </summary>
        public double Survival { get; set; }

        /// <summary>
        /// A <see cref="Particle"/> object for use in Particle Swarm Optimization.
        /// </summary>
        /// <param name="vector">The current vector.</param>
        /// <param name="value">The value of the current vector.</param>
        /// <param name="velocity">The current adjustment vector.</param>
        public Particle(double value, IReadOnlyList<double> vector, IReadOnlyList<double> velocity)
        {
            if (vector.Count != velocity.Count)
            {
                throw new ArgumentOutOfRangeException("Position and velocity vectors must be equal length.");
            }
            Current = new Solution(value, vector);
            Best = new Solution(value, vector);
            Velocity = new double[vector.Count];
            for (int i = 0; i < vector.Count; i++)
            {
                Velocity[i] = velocity[i];
            }
            Survival = 1.0;
        }

        /// <summary>
        /// A <see cref="Particle"/> object for use in Particle Swarm Optimization.
        /// </summary>
        /// <param name="solution"></param>
        /// <param name="velocity">The current adjustment vector.</param>
        public Particle(Solution solution, IReadOnlyList<double> velocity)
        {
            if (solution.Vector.Length != velocity.Count)
            {
                throw new ArgumentOutOfRangeException("Position and velocity vectors must be equal length.");
            }
            Current = new Solution(solution);
            Best = new Solution(solution);
            Velocity = new double[velocity.Count];
            for (int i = 0; i < velocity.Count; i++)
            {
                Velocity[i] = velocity[i];
            }
            Survival = 1.0;
        }

        /// <summary>
        /// Updates the <see cref="Particle"/> with the new current solution.
        /// </summary>
        /// <param name="solution">The new current solution.</param>
        public void SetCurrent(Solution solution)
        {
            Current = new Solution(solution);
            if (solution.Value < Best.Value)
            {
                Best = new Solution(solution);
            }
        }

        /// <summary>
        /// Updates the <see cref="Particle"/> with the new velocity.
        /// </summary>
        /// <param name="velocity">The velocity at the current vector.</param>
        public void SetVelocity(double[] velocity)
        {
            for (int i = 0; i < velocity.Length; i++)
            {
                Velocity[i] = velocity[i];
            }
        }

        /// <summary>
        /// Updates the <see cref="Particle"/> with the new velocity.
        /// </summary>
        /// <param name="swarm">The <see cref="Swarm"/> to which the <see cref="Particle"/> belongs.</param>
        public void SetSurvival(Swarm swarm)
        {
            Survival = swarm.RandomGenerator.NextDouble();
        }

        /// <summary>
        /// Returns a string representation of the <see cref="Particle"/>.
        /// </summary>
        public override string ToString()
        {
            return Best.ToString();
        }
    }
}
