using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WeatherClientTool_Framework;

namespace CustomUnitTestProject
{
    [TestClass]
    public class UnitTestData
    {
        [TestMethod]
        public void TestKolkata()
        {         
            Assert.AreEqual(true,Weather_Checker.GetWeatherDetails("Kolkata"));
        }
        [TestMethod]
        public void TestMumbai()
        {
            Assert.AreEqual(true, Weather_Checker.GetWeatherDetails("Mumbai"));
        }
        [TestMethod]
        public void TestKanpur()
        {
            //Kanpur is not present in the city list csv file with latitude and longitude data
            Assert.AreEqual(false, Weather_Checker.GetWeatherDetails("Kanpur"));
        }
        [TestMethod]
        public void TestInvalidCity()
        {
            Assert.AreEqual(false, Weather_Checker.GetWeatherDetails("kkkkk"));
        }
    }
}
