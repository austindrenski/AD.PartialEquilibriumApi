using System.Collections.Generic;
using System.Xml.Serialization;

// ReSharper disable All

namespace AD.PartialEquilibriumApi
{
    /// <summary>
    /// Describes a market.
    /// </summary>
    [XmlRoot("Market", Namespace = "https://austindrenski.github.io/AD.PartialEquilibriumApi/")]
    [XmlType("Market")]
    public class Market
    {
        /// <summary>
        /// The name of this market.
        /// </summary>
        [XmlAttribute]
        public string Name { get; set; }

        /// <summary>
        /// The initial price provided by the user.
        /// </summary>
        [XmlAttribute]
        public double InitialPrice { get; set; }

        /// <summary>
        /// The price index of sub-market prices or the initial price.
        /// </summary>
        [XmlAttribute]
        public double CalculatedPrice { get; set; }

        /// <summary>
        /// True if this market is composed of sub-markets.
        /// </summary>
        [XmlAttribute]
        public bool HasSubMarkets
        {
            get
            {
                return SubMarkets.Count > 0;
            }
            set { }
        }

        /// <summary>
        /// A list of markets that compose this market.
        /// </summary>
        [XmlElement(nameof(Market))]
        public List<Market> SubMarkets { get; set; }

        /// <summary>
        /// Parameterless constructor for XML serialization.
        /// </summary>
        private Market() { }
        
        /// <summary>
        /// Creates a market with the specified name.
        /// </summary>
        /// <param name="name">The name of this market.</param>
        public Market(string name)
        {
            Name = name;
            SubMarkets = new List<Market>();
        }

        /// <summary>
        /// Creates a market with the specified name and initial price.
        /// </summary>
        /// <param name="name">The name of this market.</param>
        /// <param name="initialPrice">The initial price of this market.</param>
        public Market(string name, double initialPrice)
        {
            Name = name;
            InitialPrice = initialPrice;
            SubMarkets = new List<Market>();
        }
    }
}
