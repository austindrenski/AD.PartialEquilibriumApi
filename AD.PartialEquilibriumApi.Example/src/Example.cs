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
            //// Create boolean vector indicating which nodes (in document-order) are variable.
            //XName[] variables =
            //    new XName[]
            //    {
            //        "Supplier1",
            //        "Supplier2",
            //    };

            XmlFilePath structureFile = CreateTempXmlFile2();
            DelimitedFilePath dataFile = CreateTempCsvFile2();
            // Create boolean vector indicating which nodes (in document-order) are variable.
            XName[] variables =
                new XName[]
                {
                    "Supplier1",
                    //"Supplier2",
                        "Input1",
                        "Input2"
                };

            // Read in the model and the data.
            XElement model = CreateModelFromFile(structureFile, dataFile);

            // Apply the price shocks.
            model.ShockAllProducerPrices();

            // Calculate the price indices.
            //model.CalculateConsumerPriceIndex();

            // Calculate the market equilibrium starting on the root.
            model.CalculateRootMarketEquilibrium();

            // Create the objective function.
            Func<double[], double> objectiveFunction =
                x =>
                {
                    // Update consumer prices to the argument vector.
                    // Result: x[i] if variables[i] is true
                    model.SetConsumerPrices(x, variables);
                    // Shock the current prices:
                    // Result: currentPrice * (1 + shock)
                    model.ShockAllProducerPrices();
                    // Calculate a price index for the sector:
                    // Result: [Σ marketShare[i] * (price[i] ^ (1 - elasticityOfSubstitution[i])] ^ [1 / (1 - elasticityOfSubstitution)]
                    //model.CalculateConsumerPriceIndex();
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
                    numberOfSolutions: 3 * variables.Length,
                    iterations: 10000 * variables.Length,
                    textWriter: Console.Out);

            // Find the minimum solution.
            Solution solution = simplex.Minimize();

            // Update the XML tree one more time with the optimal result.
            double[] result = solution.Vector;
            model.SetConsumerPrices(result, variables);
            model.ShockAllProducerPrices();
            //model.CalculateConsumerPriceIndex();
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