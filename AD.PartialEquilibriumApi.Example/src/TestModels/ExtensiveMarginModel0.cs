using System.IO;
using System.Xml.Linq;
using AD.IO;
using JetBrains.Annotations;

#pragma warning disable 1591

namespace AD.PartialEquilibriumApi.Example.TestModels
{
    public class ExtensiveMarginModel0 : IModel
    {
        [UsedImplicitly]
        public XmlFilePath Model()
        {
            string xml = Path.ChangeExtension(Path.GetTempFileName(), ".xml");
            using (StreamWriter writer = new StreamWriter(xml))
            {
                writer.WriteLine(
                    @"<A0>
                          <B0/>
                          <B1/>
                          <B2/>
                      </A0>");
            }
            return new XmlFilePath(xml);
        }

        [UsedImplicitly]
        public DelimitedFilePath Data()
        {
            string csv = Path.ChangeExtension(Path.GetTempFileName(), ".csv");
            using (StreamWriter writer = new StreamWriter(csv))
            {
                writer.WriteLine("ElasticityOfSubstitution,ElasticityOfSupply,ElasticityOfDemand,InitialPrice,InitialMarketShare,Shock");
                writer.WriteLine("4, 5, -1, 1.0, 1.000, 0.00");
                writer.WriteLine("4, 5, -1, 1.0, 0.495, 0.00");
                writer.WriteLine("4, 5, -1, 1.0, 0.495, 0.00");
                writer.WriteLine("4, 5, -1, 1.0, 0.010, 0.90");

            }
            return new DelimitedFilePath(csv, ',');
        }

        [UsedImplicitly]
        public XName[] Variables()
        {
            return new XName[]
            {
                //"A0",
                    "B0",
                    "B1",
                    "B2"
            };
        }
    }
}