using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TestEnvironment.Entities
{
    [Serializable]
    public class Step
    {
        private List<TestAction> _actions;

        public List<TestAction> Actions
        {
            get { if (_actions == null) _actions = new List<TestAction>(); return _actions; }
        }

        String _response;
        [XmlIgnore]
        public String Response
        {
            get { return _response; }
            set { _response = value; }
        }
        [XmlIgnore]
        public bool Passed
        {
            get { return Actions.All(a => a.Status == TestStatus.Passed); }
        }
    }
}
