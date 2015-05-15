using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TestEnvironment.Entities {
    public enum TestStatus { NotRun, Pending, Running, Passed, Failed }
    [Serializable]
    public class TestCase {
        string _id;
        [XmlAttribute]
        public string Id {
            get { return _id; }
            set { _id = value; }
        }
        TestStatus _status;
        [XmlIgnore]
        public TestStatus Status {
            get { return _status; }
            set { _status = value; }
        }
        List<TestAction> _testActions;
        public List<TestAction> TestActions
        {
            get {
                if (_testActions == null) _testActions = new List<TestAction>();
                return _testActions;
            }
        }
        string _description;
        [XmlAttribute]
        public string Description {
            get { return _description; }
            set { _description = value; }
        }
        string _name;
        [XmlAttribute]
        public string Name {
            get { return _name; }
            set { _name = value; }
        }
        string _response;
        [XmlIgnore]
        public string Response {
            get { return _response; }
            set { _response = value; }
        }
    }
}
