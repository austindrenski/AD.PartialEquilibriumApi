namespace AD.PartialEquilibriumApi.Optimization
{
    /// <summary>
    /// This class initializes a single Particle object.
    /// </summary>
    public struct Particle
    {
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
        internal Particle(double cost, double[] position, double[] velocity)
        {
            BestCost = cost;
            BestPosition = position;
            Cost = cost;
            Position = position;
            Velocity = velocity;
        }
    }
}
