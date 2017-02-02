using AD.IO;
using JetBrains.Annotations;

namespace AD.PartialEquilibriumApi
{
    /// <summary>
    /// An abstract base class for partial equilibrium models.
    /// </summary>
    [PublicAPI]
    public abstract class Model
    {
        /// <summary>
        /// The number of markets in the model.
        /// </summary>
        public int NumberOfMarkets { get; set; }

        /// <summary>
        /// The number of variables that change during the solution process.
        /// </summary>
        public int NumberOfVariables { get; set; }

        /// <summary>
        /// The user-supplied data for the model.
        /// </summary>
        public double[][] Data { get; set; }

        /// <summary>
        /// Initializes the model.
        /// </summary>
        protected Model()
        {
            NumberOfMarkets = 0;
            NumberOfVariables = 0;
            Data = new double[0][];
        }

        /// <summary>
        /// Initializes the model.
        /// </summary>
        /// <param name="delimitedFilePath"></param>
        protected Model(DelimitedFilePath delimitedFilePath)
        {
            Data = null;
        }
    }
}
