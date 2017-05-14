using System;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using AD.IO;
using AD.PartialEquilibriumApi.Optimization;
using AD.PartialEquilibriumApi.Xml;

namespace AD.PartialEquilibriumApi.Example
{
    public static class Example2
    {
        public static void Example()
        {

                Market a =
                    new Market
                    {
                        ElasticityOfDemand = -1.0,
                        ElasticityOfSubstitution = 4.0,
                        ElasticityOfSupply = 5.0,
                        InitialMarketShare = 1.0,
                        MarketShare = 1.0,
                        InitialPrice = 1.0,
                        Shock = 0.0,
                    };

                Market b =
                    new Market
                    {
                        ElasticityOfDemand = -1.0,
                        ElasticityOfSubstitution = 4.0,
                        ElasticityOfSupply = 5.0,
                        InitialMarketShare = 0.5,
                        MarketShare = 0.5,
                        InitialPrice = 1.0,
                        Shock = 0.05,
                        DownstreamMarkets = new Market[] { a }
                    };

                Market c =
                    new Market
                    {
                        ElasticityOfDemand = -1.0,
                        ElasticityOfSubstitution = 4.0,
                        ElasticityOfSupply = 5.0,
                        InitialMarketShare = 0.5,
                        MarketShare = 0.5,
                        InitialPrice = 1.0,
                        Shock = 0.05,
                        DownstreamMarkets = new Market[] { a }
                    };

                a.UpstreamMarkets = new Market[] { b, c };

            // Create the objective function.
            double ObjectiveFunction(double[] x)
            {
                a.SetConsumerPrice(x);
                a.CalculateMarketShares();
                a.CalculateMarketEquilibrium();
                return a.SumOfSquaresMinimizer;
            }

            // Set up the simplex solver.
            Simplex simplex =
                new Simplex(
                    objectiveFunction: ObjectiveFunction,
                    lowerBound: 0,
                    upperBound: 10,
                    dimensions: 2,
                    iterations: 50000,
                    seed: 0,
                    textWriter: Console.Out
                );

            // Find the minimum solution.
            Solution solution = simplex.Minimize();

            a.SetConsumerPrice(solution.Vector);
            a.CalculateMarketShares();
            a.CalculateMarketEquilibrium();

            // Print the results
            PrintResults(a, solution);
        }

        private static void PrintResults(Market market, Solution solution)
        {
            Console.WriteLine("-----------------------------------------------------------------------------------------");
            Console.WriteLine();

            market.UpstreamMarkets.ToList().ForEach(x => Console.WriteLine(x.MarketEquilibrium));
            Console.WriteLine(market.MarketEquilibrium);

            Console.WriteLine();
            Console.WriteLine($"Final solution: {solution}.");
            Console.WriteLine();
            Console.WriteLine("-----------------------------------------------------------------------------------------");
            Console.ReadLine();
        }
    }
}