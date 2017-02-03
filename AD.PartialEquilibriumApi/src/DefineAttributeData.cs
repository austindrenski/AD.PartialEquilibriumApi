using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using AD.IO;
using JetBrains.Annotations;

namespace AD.PartialEquilibriumApi
{
    /// <summary>
    /// Extension methods to set attribute values on the root and descendants using a data file.
    /// </summary>
    [PublicAPI]
    public static class DefineAttributeDataExtensions
    {
        /// <summary>
        /// Defines attributes on the root and descendants using a data file.
        /// </summary>
        /// <param name="root">The root element.</param>
        /// <param name="dataFile">A <see cref="DelimitedFilePath"/> with headers and double values.</param>
        /// <returns>A reference to the existing <see cref="XElement"/>. This is returned for use with fluent syntax calls.</returns>
        public static XElement DefineAttributeData(this XElement root, DelimitedFilePath dataFile)
        {
            IDictionary<string, double[]> data = dataFile.ReadData();

            XElement[] elements = root.DescendantsAndSelf().ToArray();

            foreach (string header in data.Keys)
            {
                for (int i = 0; i < elements.Length; i++)
                {
                    elements[i].SetAttributeValue(header, data[header][i]);
                }
            }

            return root;
        }
    }
}
