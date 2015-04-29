using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestEnvironment.Entities {
    [Serializable]
    public class TestSuite {
        private List<TestCase> _testCases;

        public List<TestCase> TestCases {
            get { if (_testCases == null) _testCases = new List<TestCase>(); return _testCases; }
        }
    }
}
