using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using AD.Collections;
using AD.IO;
using AD.Xml;
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
        /// The headers found in the data file.
        /// </summary>
        public IEnumerable<string> DataHeaders { get; }

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
        /// <param name="delimitedFilePath">The file from which data is read.</param>
        protected Model(DelimitedFilePath delimitedFilePath)
        {
            DataHeaders = 
                File.ReadLines(delimitedFilePath)
                    .FirstOrDefault()
                    .SplitDelimitedLine(delimitedFilePath.Delimiter);

            string[] lines = 
                File.ReadLines(delimitedFilePath)
                    .Skip(1)
                    .ToArray();

            int count = lines.Length;

            Data = new double[count][];

            for (int i = 0; i < count; i++)
            {
                Data[i] = lines[i].SplitDelimitedLine(delimitedFilePath.Delimiter)
                                  .Select(double.Parse)
                                  .ToArray();
            }
        }
    }
}
