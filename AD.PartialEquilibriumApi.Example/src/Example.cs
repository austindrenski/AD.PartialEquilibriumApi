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
            //Simplex rosenbrock = new Simplex(x => 2 * (100 * Math.Pow(x[1] - Math.Pow(x[1], 2), 2) + Math.Pow(x[0] - 1, 2)), -5, 5, 2, 10000, Console.Out);
            //rosenbrock.Minimize();

            //XmlFilePath structureFile = CreateTempXmlFile0();
            //DelimitedFilePath dataFile = CreateTempCsvFile0();
            //XName[] variables =
            //    new XName[]
            //    {
            //        //"Retail",
            //            "Supplier1",
            //            "Supplier2",
            //    };

            //XmlFilePath structureFile = CreateTempXmlFile1();
            //DelimitedFilePath dataFile = CreateTempCsvFile1();
            //XName[] variables =
            //    new XName[]
            //    {
            //        //"Retail",
            //            "Supplier1",
            //            //"Supplier2",
            //                "Input1",
            //                "Input2"
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
                            //"Input2",
                                "Factor1",
                                "Factor2"
                };

            // Read in the model and the data.
            XElement model = CreateModelFromFile(structureFile, dataFile);

            // Create chart depicting the model
            //XElement html0 = ChartFactory.CreateOrganizationalChart(model);
            //using (StreamWriter writer = new StreamWriter(@"g:\data\austin d\pe modeling\chart.html"))
            //{
            //    writer.WriteLine(html0.ToString());
            //}

            // Mark variables
            model.SetIsVariable(variables);

            // Apply the price shocks.
            model.ShockProducerPrices();

            // Calculate the market equilibrium starting on the root.
            model.CalculateMarketEquilibrium();

            // Create the objective function.
            Func<double[], double> objectiveFunction =
                x =>
                {
                    model.SetConsumerPrices(x);
                    model.ShockProducerPrices();
                    model.CalculateMarketEquilibrium();
                    return ObjectiveFunctionFactory.Default(model);
                };

            // Set up the simplex solver.
            Simplex simplex =
                new Simplex(
                    objectiveFunction: x => objectiveFunction(x),
                    lowerBound: 0,
                    upperBound: 10,
                    dimensions: variables.Length,
                    iterations: 1000,
                    seed: 0,
                    textWriter: Console.Out);

            // Find the minimum solution.
            Solution solution = simplex.Minimize();

            // Update the XML tree one more time with the optimal result.
            double[] result = solution.Vector;
            model.SetConsumerPrices(result);
            model.ShockProducerPrices();
            model.CalculateMarketEquilibrium();

            // Calculate new market shares
            model.CalculateFinalMarketShares();

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
        public static XmlFilePath CreateTempXmlFile0()
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
        public static XmlFilePath CreateTempXmlFile1()
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
                            <Input2>
                                <Factor1 />
                                <Factor2 />
                            </Input2>    
                        </Supplier2>
                      </Retail>");
            }
            return new XmlFilePath(xml);
        }

        [UsedImplicitly]
        public static DelimitedFilePath CreateTempCsvFile0()
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
        public static DelimitedFilePath CreateTempCsvFile1()
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
                writer.WriteLine("4,5,-1,1.0,0.50,0.00");
                writer.WriteLine("4,5,-1,1.0,0.50,0.00");
                writer.WriteLine("4,5,-1,1.0,0.50,0.05");
                writer.WriteLine("4,5,-1,1.0,0.50,0.05");
            }
            return new DelimitedFilePath(csv, ',');
        }
    }
}