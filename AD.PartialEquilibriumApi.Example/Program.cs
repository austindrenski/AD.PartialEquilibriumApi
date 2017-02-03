using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                ExampleFromXElement(dataFile),
                ExampleCompleteFromMarkup()
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
                           .DefineAttributeData(dataFile);
        }

        private static XElement ExampleFromXElement(DelimitedFilePath dataFile)
        {
            IDictionary<string, double[]> data = dataFile.ReadData();

            // Define the product market.
            XElement usaBeef = new XElement("usaBeef",
                    new XAttribute("ElasticityOfSubstitution", data["ElasticityOfSubstitution"][0]),
                    new XAttribute("InitialPrice", data["InitialPrice"][0]),
                    new XAttribute("MarketShare", data["MarketShare"][0]),
                    new XAttribute("Tariff", data["Tariff"][0]));

            // Define the supplier markets and the initial prices of purchase from those markets.
            XElement usa =
                new XElement("usa",
                    new XAttribute("ElasticityOfSubstitution", data["ElasticityOfSubstitution"][1]),
                    new XAttribute("InitialPrice", data["InitialPrice"][1]),
                    new XAttribute("MarketShare", data["MarketShare"][1]),
                    new XAttribute("Tariff", data["Tariff"][1]));

            XElement can =
                new XElement("can",
                    new XAttribute("ElasticityOfSubstitution", data["ElasticityOfSubstitution"][2]),
                    new XAttribute("InitialPrice", data["InitialPrice"][2]),
                    new XAttribute("MarketShare", data["MarketShare"][2]),
                    new XAttribute("Tariff", data["Tariff"][2]));

            XElement mex =
                new XElement("mex",
                    new XAttribute("ElasticityOfSubstitution", data["ElasticityOfSubstitution"][3]),
                    new XAttribute("InitialPrice", data["InitialPrice"][3]),
                    new XAttribute("MarketShare", data["MarketShare"][3]),
                    new XAttribute("Tariff", data["Tariff"][3]));

            XElement aus =
                new XElement("aus",
                    new XAttribute("ElasticityOfSubstitution", data["ElasticityOfSubstitution"][4]),
                    new XAttribute("InitialPrice", data["InitialPrice"][4]),
                    new XAttribute("MarketShare", data["MarketShare"][4]),
                    new XAttribute("Tariff", data["Tariff"][4]));

            // Add the supplier markets to the product market.
            // This has the effect of splitting the product market into product supplied by the supplier markets.
            usaBeef.Add(usa, can, mex, aus);

            return usaBeef;
        }

        private static XElement ExampleCompleteFromMarkup()
        {
            return XElement.Parse(
                @"<usaBeef ElasticityOfSubstitution=""4"" InitialPrice=""1.0"" MarketShare=""0.0"" Tariff=""0.0"" >
                    <usa ElasticityOfSubstitution=""4"" InitialPrice=""0.9764852913975930"" MarketShare=""0.0164876157540142"" Tariff=""0.0435080979193930"" />
                    <can ElasticityOfSubstitution=""4"" InitialPrice=""1.0000000000000000"" MarketShare=""0.1826886798599640"" Tariff=""0.0000000000000000"" />
                    <mex ElasticityOfSubstitution=""4"" InitialPrice=""1.0000000000000000"" MarketShare=""0.0747428059044746"" Tariff=""0.0000000000000000"" />
                    <aus ElasticityOfSubstitution=""4"" InitialPrice=""1.0000000000000000"" MarketShare=""0.7260808984815470"" Tariff=""0.0000000000000000"" />
                </usaBeef>");
        }
    }
}
