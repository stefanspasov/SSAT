using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml.Serialization;
using TestEnvironment.Entities;

namespace Orchestrator
{
    public class TestAccess : ITestAccess
    {
        public IList<TestCase> LoadTestCases()
        {
            var serializer = new XmlSerializer(typeof(TestSuite));
            // TODO: locate the test-suite file using OpenFileDialog
            var ts = (TestSuite) serializer.Deserialize(new StreamReader("test-suite.xml"));

            // Get clients' locations based on their names
            var clientCollection = ConfigurationManager.GetSection("clientCollection") as NameValueCollection;
            if (clientCollection != null) {
                foreach (var client in ts.TestCases.SelectMany(t => t.Steps).SelectMany(s => s.Actions).Select(a => a.TargetClient)) {
                    client.IpAddress = IPAddress.Parse(clientCollection[client.Name]);
                }
            }
            
            return ts.TestCases;
        }
    }
}
