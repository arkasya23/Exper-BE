using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExperBE.Models.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExperBE.Tests
{
    [TestClass]
    public class TestsConfiguration
    {
        [AssemblyInitialize]
        public void TestsGlobalInitialize()
        {
            ExperConfiguration.ConnectionString = "";
            ExperConfiguration.JwtSecret = "TestSecret";
            ExperConfiguration.SendEmails = false;
        }
    }
}
