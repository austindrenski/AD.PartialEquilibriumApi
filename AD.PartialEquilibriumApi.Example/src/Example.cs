using System;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using AD.IO;
using AD.PartialEquilibriumApi.PSO;

namespace AD.PartialEquilibriumApi.Example
{
    public static class Example
    {
        public static void Example1()
        {
            TestModels.IModel modelFactory = TestModels.ModelFactory.Model2D();
            
            XmlFilePath structureFile = modelFactory.Model();
            DelimitedFilePath dataFile = modelFactory.Data();

            // Read in the model and the data.
            XElement model =
                XElement.Load(structureFile)
                        .DefineAttributeData(dataFile);

            // Create the objective function.
            Func<double[], double> objectiveFunction =
                x =>
                {
                    XElement localModel = new XElement(model);
                    localModel.SetConsumerPrices(x)
                              .ShockProducerPrices()
                              .CalculateMarketShares()
                              .CalculateMarketEquilibrium();

                    return ObjectiveFunctionFactory.SumOfSquares(localModel);
                };

            //// Set up the simplex solver.
            //Simplex simplex =
            //    new Simplex(
            //        objectiveFunction: x => objectiveFunction(x),
            //        lowerBound: 0,
            //        upperBound: 10,
            //        dimensions: model.DescendantsAndSelf().Count(),
            //        iterations: 25000,
            //        seed: 0,
            //        textWriter: Console.Out
            //    );

            //// Find the minimum solution.
            //Solution solution = simplex.Minimize();

            //// Apply the final solution
            //model.SetConsumerPrices(solution.Vector)
            //     .ShockProducerPrices()
            //     .CalculateMarketShares()
            //     .CalculateMarketEquilibrium();

            //// Print the results
            //PrintResults(model, solution);
            
            // Set up the swarm solver.
            Swarm swarm =
                new Swarm(
                    objectiveFunction: x => objectiveFunction(x),
                    lowerBound: 0,
                    upperBound: 10,
                    dimensions: model.DescendantsAndSelf().Count(),
                    iterations: 25000,
                    particles: model.DescendantsAndSelf().Count() + 1,
                    seed: 0,
                    textWriter: Console.Out
                );

            // Find the minimum solution.
            Particle particle = swarm.Minimize();

            // Apply the final solution
            model.SetConsumerPrices(particle.Vector)
                 .ShockProducerPrices()
                 .CalculateMarketShares()
                 .CalculateMarketEquilibrium();

            // Print the results
            PrintResults(model, particle);
        }

        private static void PrintResults(XNode model, object solution)
        {
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
            Console.WriteLine();
            Console.WriteLine($"Final solution: {solution}.");
            Console.WriteLine();
            Console.WriteLine("-----------------------------------------------------------------------------------------");
            Console.ReadLine();
        }
    }
}