using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AD.PartialEquilibriumApi.Tests
{
    [TestClass]
    public class SetConsumerPriceTests
    {
        [TestMethod]
        public void SetConsumerPriceTest_Model0()
        {
            // Arrange
            XElement model = Definitions.CreateModel0();

            // Act


            // Assert
        }

        [TestMethod]
        public void SetConsumerPriceTest_Model1()
        {
            // Arrange
            XElement model = Definitions.CreateModel1();


            // Act


            // Assert
        }
    }
}
