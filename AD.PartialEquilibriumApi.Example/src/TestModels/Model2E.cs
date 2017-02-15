using System.IO;
using System.Xml.Linq;
using AD.IO;
using JetBrains.Annotations;

#pragma warning disable 1591

namespace AD.PartialEquilibriumApi.Example.TestModels
{
    public class Model2E : IModel
    {
        [UsedImplicitly]
        public XmlFilePath Model()
        {
            string xml = Path.ChangeExtension(Path.GetTempFileName(), ".xml");
            using (StreamWriter writer = new StreamWriter(xml))
            {
                writer.WriteLine(
                    @"<A0>
                          <B0>
                              <C0>
                                  <D0/>
                                  <D1/>
                              </C0>
                              <C1>
                                  <D3/>
                                  <D4/>
                              </C1>   
                          </B0>
                          <B1>
                              <C2>
                                  <D4/>
                                  <D5/>
                              </C2>   
                              <C3>
                                  <D6/>
                                  <D7/>
                              </C3>   
                          </B1>
                      </A0>");
            }
            return new XmlFilePath(xml);
        }

        [UsedImplicitly]
        public DelimitedFilePath Data()
        {
            string csv = Path.ChangeExtension(Path.GetTempFileName(), ".csv");
            using (StreamWriter writer = new StreamWriter(csv))
            {
                writer.WriteLine("ElasticityOfSubstitution,ElasticityOfSupply,ElasticityOfDemand,InitialPrice,InitialMarketShare,Shock");
                writer.WriteLine("4,5,-1,1.0,1.00,0.00");// A0
                writer.WriteLine("4,5,-1,1.0,0.50,0.00");// ..B0 
                writer.WriteLine("4,5,-1,1.0,0.50,0.00");// ....C0
                writer.WriteLine("4,5,-1,1.0,0.50,0.00");// ......D0
                writer.WriteLine("4,5,-1,1.0,0.50,0.00");// ......D1
                writer.WriteLine("4,5,-1,1.0,0.50,0.00");// ....C1
                writer.WriteLine("4,5,-1,1.0,0.50,0.00");// ......D2
                writer.WriteLine("4,5,-1,1.0,0.50,0.00");// ......D3
                writer.WriteLine("4,5,-1,1.0,0.50,0.00");// ..B1
                writer.WriteLine("4,5,-1,1.0,0.50,0.00");// ....C2
                writer.WriteLine("4,5,-1,1.0,0.50,0.05");// ......D4
                writer.WriteLine("4,5,-1,1.0,0.50,0.05");// ......D5
                writer.WriteLine("4,5,-1,1.0,0.50,0.00");// ....C3
                writer.WriteLine("4,5,-1,1.0,0.50,0.05");// ......D6
                writer.WriteLine("4,5,-1,1.0,0.50,0.05");// ......D7
            }
            return new DelimitedFilePath(csv, ',');
        }

        [UsedImplicitly]
        public XName[] Variables()
        {
            return new XName[]
            {
                "A0",
                    "B0",
                        "C0",
                            "D0",
                            "D1",
                        "C1",
                            "D2",
                            "D3",
                    "B1",
                        "C2",
                            "D4",
                            "D5",
                        "C3",
                            "D6",
                            "D7"
            };
        }
    }
}