using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using AD.IO;
using JetBrains.Annotations;

namespace AD.PartialEquilibriumApi.Example
{
    public static class Example
    {
        public static void Example1()
        {
            //XmlFilePath structureFile = CreateTempXmlFile();
            //DelimitedFilePath dataFile = CreateTempCsvFile();
            //XName[] variables =
            //    new XName[]
            //    {
            //        //"Retail",
            //            "Supplier1",
            //            "Supplier2",
            //    };

            XmlFilePath structureFile = CreateTempXmlFile2();
            DelimitedFilePath dataFile = CreateTempCsvFile2();
            XName[] variables =
                new XName[]
                {
                    //"Retail",
                        "Supplier1",
                        //"Supplier2",
                            "Input1",
                            "Input2"
                };

            // Read in the model and the data.
            XElement model = CreateModelFromFile(structureFile, dataFile);

            // Apply the price shocks.
            model.ShockProducerPrices();

            // Calculate the market equilibrium starting on the root.
            model.CalculateRootMarketEquilibrium();

            // Create the objective function.
            Func<double[], double> objectiveFunction =
                x =>
                {
                    // Update consumer prices to the argument vector or calculate a price index.
                    model.SetConsumerPrices(x, variables);
                    // Shock the current prices:
                    // Result: currentPrice * (1 + shock)
                    model.ShockProducerPrices();
                    // Caclulate the market equilibrium. Zero means equilibrium.
                    // [shockedPrice ^ elasticityOfSupply] - [(priceIndex ^ (elasticityOfSubstitution + elasticityOfDemand)) / (initialPrice ^ elasticityOfSubstitution)]
                    model.CalculateRootMarketEquilibrium();
                    // Return the sector's equilibrium value to the caller.
                    return model.Descendants().Select(y => y.MarketEquilibrium()).Sum(y => y * y);
                };

            // Set up the simplex solver.
            Simplex simplex =
                new Simplex(
                    objectiveFunction: x => objectiveFunction(x),
                    lowerBound: 0,
                    upperBound: 100,
                    dimensions: variables.Length,
                    iterations: 10000,
                    textWriter: Console.Out);

            // Find the minimum solution.
            Solution solution = simplex.Minimize();

            // Update the XML tree one more time with the optimal result.
            double[] result = solution.Vector;
            model.SetConsumerPrices(result, variables);
            model.ShockProducerPrices();
            model.CalculateRootMarketEquilibrium();

            // Calculate new market shares
            //model.CalculateAllFinalMarketShares();

            // Print the results.
            Console.WriteLine("-----------------------------------------------------------------------------------------");
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Encoding = Encoding.UTF8,
                Indent = true,
                NewLineOnAttributes = true,
                IndentChars = "    "
            };
            using (XmlWriter writer = XmlWriter.Create(Console.Out, settings))
            {
                model.WriteTo(writer);
            }
            Console.WriteLine();
            Console.WriteLine("-----------------------------------------------------------------------------------------");
            Console.ReadLine();
        }

        [UsedImplicitly]
        public static XElement CreateModelFromFile(XmlFilePath structureFile, DelimitedFilePath dataFile)
        {
            return XElement.Load(structureFile)
                           .DefineAttributeData(dataFile);
        }

        [UsedImplicitly]
        public static XmlFilePath CreateTempXmlFile()
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

        [UsedImplicitly]
        public static XmlFilePath CreateTempXmlFile2()
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

        [UsedImplicitly]
        public static DelimitedFilePath CreateTempCsvFile()
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

        [UsedImplicitly]
        public static DelimitedFilePath CreateTempCsvFile2()
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