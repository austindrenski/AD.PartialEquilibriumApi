using System.IO;
using System.Xml.Linq;
using AD.IO;
using AD.PartialEquilibriumApi.Xml;

namespace AD.PartialEquilibriumApi.Tests
{
    internal static class Definitions
    {
        public static XElement CreateModel0()
        {
            return XElement.Load(CreateTempXmlFile0())
                           .DefineAttributeData(CreateTempCsvFile0());
        }

        public static XElement CreateModel1()
        {
            return XElement.Load(CreateTempXmlFile1())
                           .DefineAttributeData(CreateTempCsvFile1());
        }

        private static XmlFilePath CreateTempXmlFile0()
        {
            string xml = Path.ChangeExtension(Path.GetTempFileName(), ".xml");
            using (StreamWriter writer = new StreamWriter(xml))
            {
                writer.WriteLine(
                    @"<Retail>
                        <Supplier1 />
                        <Supplier2 />
                      </Retail>");
            }
            return new XmlFilePath(xml);
        }

        private static XmlFilePath CreateTempXmlFile1()
        {
            string xml = Path.ChangeExtension(Path.GetTempFileName(), ".xml");
            using (StreamWriter writer = new StreamWriter(xml))
            {
                writer.WriteLine(
                    @"<Retail>
                        <Supplier1 />
                        <Supplier2>
                            <Input1 />
                            <Input2 />
                        </Supplier2>
                      </Retail>");
            }
            return new XmlFilePath(xml);
        }

        private static DelimitedFilePath CreateTempCsvFile0()
        {
            string csv = Path.ChangeExtension(Path.GetTempFileName(), ".csv");
            using (StreamWriter writer = new StreamWriter(csv))
            {
                writer.WriteLine("ElasticityOfSubstitution,ElasticityOfSupply,ElasticityOfDemand,InitialPrice,InitialMarketShare,Shock");
                writer.WriteLine("4,5,-1,1.0,1.00,0.00");
                writer.WriteLine("4,5,-1,1.0,0.50,0.00");
                writer.WriteLine("4,5,-1,1.0,0.50,0.05");

            }
            return new DelimitedFilePath(csv, ',');
        }

        private static DelimitedFilePath CreateTempCsvFile1()
        {
            string csv = Path.ChangeExtension(Path.GetTempFileName(), ".csv");
            using (StreamWriter writer = new StreamWriter(csv))
            {
                writer.WriteLine("ElasticityOfSubstitution,ElasticityOfSupply,ElasticityOfDemand,InitialPrice,InitialMarketShare,Shock");
                writer.WriteLine("4,5,-1,1.0,1.00,0.00");
                writer.WriteLine("4,5,-1,1.0,0.50,0.00");
                writer.WriteLine("4,5,-1,1.0,0.50,0.00");
                writer.WriteLine("4,5,-1,1.0,0.50,0.05");
                writer.WriteLine("4,5,-1,1.0,0.50,0.05");

            }
            return new DelimitedFilePath(csv, ',');
        }
    }
}
