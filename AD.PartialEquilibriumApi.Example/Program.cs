using System;
using System.IO;
using System.Xml.Linq;
using AD.IO;

namespace AD.PartialEquilibriumApi.Example
{
    public static class Program
    {
        public static void Main()
        {
            string xml = Path.ChangeExtension(Path.GetTempFileName(), ".xml");
            using (StreamWriter writer = new StreamWriter(xml))
            {
                writer.WriteLine(
                    @"<usaBeef >
                        <usa />
                        <can />
                        <mex />
                        <aus />
                    </usaBeef>");
            }
            XmlFilePath structureFile = new XmlFilePath(xml);
            
            string csv = Path.ChangeExtension(Path.GetTempFileName(), ".csv");
            using (StreamWriter writer = new StreamWriter(csv))
            {
                writer.WriteLine("ElasticityOfSubstitution,InitialPrice,MarketShare,Tariff");
                writer.WriteLine("4,1.0000000000000000,0.0000000000000000,0.0000000000000000");
                writer.WriteLine("4,0.9764852913975930,0.0164876157540142,0.0435080979193930");
                writer.WriteLine("4,1.0000000000000000,0.1826886798599640,0.0000000000000000");
                writer.WriteLine("4,1.0000000000000000,0.0747428059044746,0.0000000000000000");
                writer.WriteLine("4,1.0000000000000000,0.7260808984815470,0.0000000000000000");
            }
            DelimitedFilePath dataFile = new DelimitedFilePath(csv, ',');

            XElement[] versions = new XElement[]
            {
                ExampleFromFiles(structureFile, dataFile),
                ExampleFromXElement(dataFile)
            };

            foreach (XElement usaBeef in versions)
            {
                Console.WriteLine();

                Console.WriteLine("Elasticity of substitution:");
                Console.WriteLine($"    usa: {usaBeef.Element("usa")?.ElasticityOfSubstitution()}");
                Console.WriteLine($"    can: {usaBeef.Element("can")?.ElasticityOfSubstitution()}");
                Console.WriteLine($"    mex: {usaBeef.Element("mex")?.ElasticityOfSubstitution()}");
                Console.WriteLine($"    aus: {usaBeef.Element("aus")?.ElasticityOfSubstitution()}");

                Console.WriteLine();

                Console.WriteLine("Initial price:");
                Console.WriteLine($"    usa: {usaBeef.Element("usa")?.InitialPrice()}");
                Console.WriteLine($"    can: {usaBeef.Element("can")?.InitialPrice()}");
                Console.WriteLine($"    mex: {usaBeef.Element("mex")?.InitialPrice()}");
                Console.WriteLine($"    aus: {usaBeef.Element("aus")?.InitialPrice()}");

                Console.WriteLine();

                Console.WriteLine("Market share:");
                Console.WriteLine($"    usa: {usaBeef.Element("usa")?.MarketShare()}");
                Console.WriteLine($"    can: {usaBeef.Element("can")?.MarketShare()}");
                Console.WriteLine($"    mex: {usaBeef.Element("mex")?.MarketShare()}");
                Console.WriteLine($"    aus: {usaBeef.Element("aus")?.MarketShare()}");

                Console.WriteLine();

                Console.WriteLine("Tariff:");
                Console.WriteLine($"    usa: {usaBeef.Element("usa")?.Tariff()}");
                Console.WriteLine($"    can: {usaBeef.Element("can")?.Tariff()}");
                Console.WriteLine($"    mex: {usaBeef.Element("mex")?.Tariff()}");
                Console.WriteLine($"    aus: {usaBeef.Element("aus")?.Tariff()}");

                Console.WriteLine();

                Console.WriteLine($"usaBeef calculate price index: {usaBeef.CalculatePriceIndex()}");

                Console.WriteLine();

                Console.WriteLine("-------------------------");
            }

            Console.ReadLine();
        }

        private static XElement ExampleFromFiles(XmlFilePath structureFile, DelimitedFilePath dataFile)
        {
            return XElement.Load(structureFile)
                           .DefineAttributeData(dataFile)
                           .ShockAllPrices();
        }

        private static XElement ExampleFromXElement(DelimitedFilePath dataFile)
        {
            // Define the product market.
            XElement usaBeef = new XElement("usaBeef").DefineAttributeData(dataFile, 0);

            // Define the supplier markets and the initial prices of purchase from those markets.
            XElement usa = new XElement("usa").DefineAttributeData(dataFile, 1);

            XElement can = new XElement("can").DefineAttributeData(dataFile, 2);

            XElement mex = new XElement("mex").DefineAttributeData(dataFile, 3);

            XElement aus = new XElement("aus").DefineAttributeData(dataFile, 4);

            // Add the supplier markets to the product market.
            // This has the effect of splitting the product market into product supplied by the supplier markets.
            usaBeef.Add(usa, can, mex, aus);

            // Apply the price shocks
            usaBeef.ShockAllPrices();

            return usaBeef;
        }
    }
}
