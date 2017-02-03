using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using AD.IO;

namespace AD.PartialEquilibriumApi.Example
{
    public static class Program
    {
        public static void Main()
        {
            XElement[] versions = new XElement[]
            {
                ExampleFromXElement(),
                ExampleFromMarkup()
            };

            foreach (XElement usaBeef in versions)
            {
                Console.WriteLine($"usa initial price:        {usaBeef.Element("usa")?.InitialPrice()}");
                Console.WriteLine($"can initial price:        {usaBeef.Element("can")?.InitialPrice()}");
                Console.WriteLine($"mex initial price:        {usaBeef.Element("mex")?.InitialPrice()}");
                Console.WriteLine($"aus initial price:        {usaBeef.Element("aus")?.InitialPrice()}");

                Console.WriteLine($"usaBeef calculate price index: {usaBeef.CalculatePriceIndex()}");

                Console.WriteLine();
                Console.WriteLine();
            }

            Console.ReadLine();
        }

        private static XElement ExampleFromXElement()
        {
            string name = Path.ChangeExtension(Path.GetTempFileName(), ".csv");
            using (StreamWriter writer = new StreamWriter(name))
            {
                writer.WriteLine("ElasticityOfSubstitution,InitialPrice,MarketShare,Tariff");
                writer.WriteLine("4,0.9764852913975930,0.0164876157540142,0.0435080979193930");
                writer.WriteLine("4,1.0000000000000000,0.1826886798599640,0.0000000000000000");
                writer.WriteLine("4,1.0000000000000000,0.0747428059044746,0.0000000000000000");
                writer.WriteLine("4,1.0000000000000000,0.7260808984815470,0.0000000000000000");
            }
            DelimitedFilePath dataFile = DelimitedFilePath.Create(name, ',');
            IDictionary<string, double[]> data = dataFile.ReadData();


            // Define the product market.
            XElement usaBeef = new XElement("usaBeef", new XAttribute("ElasticityOfSubstitution", 4));

            // Define the supplier markets and the initial prices of purchase from those markets.
            XElement usa =
                new XElement("usa",
                    new XAttribute("ElasticityOfSubstitution", 4),
                    new XAttribute("InitialPrice", 0.9764852913975930),
                    new XAttribute("MarketShare",  0.0164876157540142),
                    new XAttribute("Tariff",       0.0435080979193930));

            XElement can =
                new XElement("can",
                    new XAttribute("ElasticityOfSubstitution", 4),
                    new XAttribute("InitialPrice", 1.0000000000000000),
                    new XAttribute("MarketShare",  0.1826886798599640),
                    new XAttribute("Tariff",       0.0000000000000000));

            XElement mex =
                new XElement("mex",
                    new XAttribute("ElasticityOfSubstitution", 4),
                    new XAttribute("InitialPrice", 1.0000000000000000),
                    new XAttribute("MarketShare",  0.0747428059044746),
                    new XAttribute("Tariff",       0.0000000000000000));

            XElement aus =
                new XElement("aus",
                    new XAttribute("ElasticityOfSubstitution", 4),
                    new XAttribute("InitialPrice", 1.0000000000000000),
                    new XAttribute("MarketShare",  0.7260808984815470),
                    new XAttribute("Tariff",       0.0000000000000000));

            // Add the supplier markets to the product market.
            // This has the effect of splitting the product market into product supplied by the supplier markets.
            usaBeef.Add(usa, can, mex, aus);

            return usaBeef;
        }

        private static XElement ExampleFromMarkup()
        {
            return XElement.Parse(
                @"<usaBeef ElasticityOfSubstitution=""4"">
                    <usa ElasticityOfSubstitution=""4"" InitialPrice=""0.9764852913975930"" MarketShare=""0.0164876157540142"" Tariff=""0.0435080979193930"" />
                    <can ElasticityOfSubstitution=""4"" InitialPrice=""1.0000000000000000"" MarketShare=""0.1826886798599640"" Tariff=""0.0000000000000000"" />
                    <mex ElasticityOfSubstitution=""4"" InitialPrice=""1.0000000000000000"" MarketShare=""0.0747428059044746"" Tariff=""0.0000000000000000"" />
                    <aus ElasticityOfSubstitution=""4"" InitialPrice=""1.0000000000000000"" MarketShare=""0.7260808984815470"" Tariff=""0.0000000000000000"" />
                </usaBeef>");
        }
    }
}
