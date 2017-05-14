using System;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using AD.IO;
using AD.PartialEquilibriumApi.Optimization;
using AD.PartialEquilibriumApi.Xml;

namespace AD.PartialEquilibriumApi.Example
{
    public static class Example
    {
        public static void Example1()
        {
            TestModels.IModel modelFactory = TestModels.ModelFactory.Model2B();
            
            XmlFilePath structureFile = modelFactory.Model();
            DelimitedFilePath dataFile = modelFactory.Data();

            // Read in the model and the data.
            XElement model =
                XElement.Load(structureFile)
                        .DefineAttributeData(dataFile);

            // Create the objective function.
            double ObjectiveFunction(double[] x)
            {
                XElement localModel = new XElement(model);
                localModel.SetConsumerPrices(x)
                          .ShockProducerPrices()
                          .CalculateMarketShares()
                          .CalculateMarketEquilibrium();

                return ObjectiveFunctionFactory.SumOfSquares(localModel);
            }

            // Set up the simplex solver.
            Simplex simplex =
                new Simplex(
                    objectiveFunction: ObjectiveFunction,
                    lowerBound: 0,
                    upperBound: 10,
                    //dimensions: model.DescendantsAndSelf().Count(),
                    dimensions: model.DescendantsAndSelf().Count(x => !x.HasElements),
                    iterations: 50000,
                    seed: 0,
                    textWriter: Console.Out
                );

            // Find the minimum solution.
            Solution solution = simplex.Minimize();

            // Apply the final solution
            model.SetConsumerPrices(solution.Vector)
                 .ShockProducerPrices()
                 .CalculateMarketShares()
                 .CalculateMarketEquilibrium();

            // Print the results
            PrintResults(model, solution);

            //// Set up the swarm solver.
            //PSO.Swarm swarm =
            //    new PSO.Swarm(
            //        objectiveFunction: x => objectiveFunction(x),
            //        lowerBound: 0,
            //        upperBound: 5,
            //        dimensions: model.DescendantsAndSelf().Count(x => !x.HasElements),
            //        iterations: 25000,
            //        particles: model.DescendantsAndSelf().Count(x => !x.HasElements) * 2,
            //        seed: 0,
            //        textWriter: Console.Out
            //    );

            //// Find the minimum solution.
            //Solution solution = PSO.MinimizeSwarmExtensions.Minimize(swarm);

            //// Apply the final solution
            //model.SetConsumerPrices(solution.Vector)
            //     .ShockProducerPrices()
            //     .CalculateMarketShares()
            //     .CalculateMarketEquilibrium();

            //// Print the results
            //PrintResults(model, solution);
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