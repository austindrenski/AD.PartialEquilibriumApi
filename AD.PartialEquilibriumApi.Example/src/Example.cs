using System;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using AD.IO;

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

            // Set up the simplex solver.
            Simplex simplex =
                new Simplex(
                    objectiveFunction: x => objectiveFunction(x),
                    lowerBound: 0,
                    upperBound: 10,
                    dimensions: model.Descendants().Count(x => !x.HasElements),
                    iterations: 5000,
                    seed: 0,
                    textWriter: Console.Out
                );

            // Find the minimum solution.
            Solution solution = simplex.Minimize();

            // Find the minimum solution in parallel.
            //Solution solution = simplex.Minimize(10);


            // Apply the final solution
            model.SetConsumerPrices(solution.Vector)
                 .ShockProducerPrices()
                 .CalculateMarketShares()
                 .CalculateMarketEquilibrium();

            //// Repeat calculations for "adjustment periods"
            //for (int i = 0; i < 10; i++)
            //{
            //    model.SetConsumerPrices()
            //         .ShockProducerPrices()
            //         .CalculateMarketShares()
            //         .CalculateMarketEquilibrium();
            //}

            // Print the results
            PrintResults(model, solution);
        }

        private static void PrintResults(XNode model, Solution solution)
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
            Console.WriteLine($"Final solution (standard): {solution}.");
            Console.WriteLine();
            Console.WriteLine("-----------------------------------------------------------------------------------------");
            Console.ReadLine();
        }
    }
}