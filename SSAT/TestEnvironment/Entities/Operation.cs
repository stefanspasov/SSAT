using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TestEnvironment.Entities
{
    public class Operation
    {
        string _directive;

        public string Directive
        {
            get { return _directive; }
            set { _directive = value; }
        }
        TestTechnology _executor;

        public TestTechnology Executor
        {
            get { return _executor; }
            set { _executor = value; }
        }

        public Operation(string directive, TestTechnology executor)
        {
            _directive = directive;
            _executor = executor;
        }
    }
}
