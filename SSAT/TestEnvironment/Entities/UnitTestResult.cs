using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TestEnvironment.Entities {
    [Serializable]
    [XmlRoot(Namespace = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010", 
             ElementName = "TestRun", DataType = "string", IsNullable = true)]
    public class UnitTestResult {
        [XmlArray("Results")]
        [XmlArrayItem("UnitTestResult", typeof(Result))]
        public Result[] Results { get; set; }
        public class Result {
            [XmlAttribute("testName")]
            public string TestName { get; set; }
            [XmlAttribute("computerName")]
            public string ComputerName { get; set; }
            [XmlAttribute("duration")]
            public string Duration { get; set; }
            [XmlAttribute("startTime")]
            public string StartTime { get; set; }
            [XmlAttribute("endTime")]
            public string EndTime { get; set; }
            [XmlAttribute("outcome")]
            public string Outcome { get; set; }
        }
    }
}
