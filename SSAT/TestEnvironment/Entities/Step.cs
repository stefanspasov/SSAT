using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestEnvironment.Entities
{
    public class Step
    {
        private Queue<TestAction> _actions;

        public Queue<TestAction> Actions
        {
            get { if (_actions == null) _actions = new Queue<TestAction>();  return _actions; }
        }

        bool _passed;
        String _response;

        public String Response
        {
            get { return _response; }
            set { _response = value; }
        }

        public bool Passed
        {
            get { return _passed; }
            set { _passed = value; }
        }
    }
}
