using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using AD.IO;
using AD.PartialEquilibriumApi.Optimization;

namespace AD.PartialEquilibriumApi.Example
{
    public static class Example
    {
        public static void Example1()
        {
            XmlFilePath structureFile = CreateTempXmlFile();
            DelimitedFilePath dataFile = CreateTempCsvFile();

            // Read in the model and the data.
            XElement model = XElement.Load(structureFile)
                                     .DefineAttributeData(dataFile);

            // Set the current prices.
            model.SetCurrentPrices(model.DescendantsAndSelf()
                                        .Select(x => x.InitialPrice())
                                        .ToArray());

            // Apply the price shocks.
            model.ShockAllPrices();

            // Calculate the price indices.
            model.CalculatePriceIndex();

            // Calculate the market equilibrium starting on the root.
            model.CalculateRootMarketEquilibrium();

            // Create boolean vector indicating which nodes (in document-order) are variable.
            bool[] variables =
                new bool[]
                {
                    false,
                    true,
                    true,
                    true,
                    true
                };

            // Create the objective function.
            Func<double[], double> objectiveFunction =
                x =>
                {
                    // Update current prices to the argument vector.
                    // Result: x[i] if variables[i] is true
                    model.SetCurrentPrices(x, variables);
                    // Shock the current prices:
                    // Result: currentPrice * (1 + shock)
                    model.ShockAllPrices();
                    // Calculate a price index for the sector:
                    // Result: [Σ marketShare[i] * (price[i] ^ (1 - elasticityOfSubstitution[i])] ^ [1 / (1 - elasticityOfSubstitution)]
                    model.CalculatePriceIndex();
                    // Caclulate the market equilibrium. Zero means equilibrium.
                    // [shockedPrice ^ elasticityOfSupply] - [(priceIndex ^ (elasticityOfSubstitution + elasticityOfDemand)) / (initialPrice ^ elasticityOfSubstitution)]
                    model.CalculateRootMarketEquilibrium();
                    // Return the sector's equilibrium value to the caller.
                    return model.MarketEquilibrium();
                };

            // Set up the simplex solver.
            Simplex simplex =
                new Simplex(
                    numberOfSolutions: 5,
                    dimensions: 5,
                    lowerBound: 0,
                    upperBound: 100,
                    iterations: 1000,
                    objectiveFunction: x => objectiveFunction(x));

            // Find the minimum solution.
            Solution solution = simplex.Minimize();

            // Update the XML tree one more time with the optimal result.
            double[] result = solution.Vector;
            model.SetCurrentPrices(result, variables);
            model.ShockAllPrices();
            model.CalculatePriceIndex();
            model.CalculateRootMarketEquilibrium();

            // Print the results.
            Console.WriteLine("-----------------------------------------------------------------------------------------");
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Encoding = Encoding.UTF8,
                Indent = true,
                NewLineOnAttributes = true
            };
            using (XmlWriter writer = XmlWriter.Create(Console.Out, settings))
            {
                model.WriteTo(writer);
            }
            Console.WriteLine();
            Console.WriteLine("-----------------------------------------------------------------------------------------");
            Console.ReadLine();
        }

        private static XmlFilePath CreateTempXmlFile()
        {
            string xml = Path.ChangeExtension(Path.GetTempFileName(), ".xml");
            using (StreamWriter writer = new StreamWriter(xml))
            {
                writer.WriteLine(
                    @"<Retail>
                        <Supplier1 />
                        <Supplier2 />
                        <Supplier3 />
                        <Supplier4 />
                      </Retail>");
            }
            return new XmlFilePath(xml);
        }

        private static DelimitedFilePath CreateTempCsvFile()
        {
            string csv = Path.ChangeExtension(Path.GetTempFileName(), ".csv");
            using (StreamWriter writer = new StreamWriter(csv))
            {
                writer.WriteLine("ElasticityOfSubstitution,ElasticityOfSupply,ElasticityOfDemand,InitialPrice,CurrentPrice,MarketShare,Shock");
                writer.WriteLine("4,5,-1,1.0,1.0,1.00,0.00");
                writer.WriteLine("4,5,-1,1.0,1.0,0.25,0.00");
                writer.WriteLine("4,5,-1,1.0,1.0,0.25,0.00");
                writer.WriteLine("4,5,-1,1.0,1.0,0.25,0.05");
                writer.WriteLine("4,5,-1,1.0,1.0,0.25,0.00");
            }
            return new DelimitedFilePath(csv, ',');
        }
    }
}