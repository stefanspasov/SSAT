using SATFUtilities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml.Serialization;
using TestEnvironment.Entities;

namespace Orchestrator {
    public class TestAccess : ITestAccess {
        public IList<TestCase> LoadTestCases(string fileName) {
            var serializer = new XmlSerializer(typeof(TestSuite));
            TestSuite ts;
            // TODO: locate the test-suite file using OpenFileDialog
            using (var reader = new StreamReader(fileName))
            {
                ts = (TestSuite)serializer.Deserialize(reader);
                reader.Close();
            }
            // Get clients' locations based on their names
            var clientCollection = Constants.ClientCollection;
            if (clientCollection != null) {
                foreach (var client in ts.TestCases.SelectMany(t => t.TestActions).Select(a => a.TargetClient)) {
                    client.IpAddress = IPAddress.Parse(clientCollection[client.Name]);
                }
            }

            return ts.TestCases;
        }

        public void SaveTestCases(IList<TestCase> testCases, string fileName) {
            var serializer = new XmlSerializer(typeof(TestSuite));
            var tss = new TestSuite();
            tss.TestCases.AddRange(testCases);
            using (var writer = new StreamWriter(fileName))
            {
                serializer.Serialize(writer, tss);
                writer.Close();
            }
        }
    }
}
