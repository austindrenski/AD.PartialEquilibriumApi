using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using System.Xml.Linq;
using AD.IO;

namespace AD.PartialEquilibriumApi
{
    public class Sector
    {
        public IImmutableList<Market> Markets { get; }

        public Sector(IEnumerable<Market> markets)
        {
            Markets = markets.ToImmutableArray();
        }
    }
}
