using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestEnvironment.Entities
{
    public enum TestStatus { NotRun, Pending, Running, Passed, Failed }
    public class TestCase
    {
        string _id;
        public string Id 
        {
            get { return _id; }
            set { _id = value; }
        }
        TestStatus _status;
        public TestStatus Status {
            get { return _status; }
            set { _status = value; }
        }
        Queue<Step> _steps;
        public Queue<Step> Steps
        {
            get
            {
                if (_steps == null) _steps = new Queue<Step>();
                return _steps;
            }
        }        
        string _description;
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        string _response;
        public string Response {
            get { return _response; }
            set { _response = value; }
        }
    }
}
