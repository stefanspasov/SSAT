using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestEnvironment.Entities
{
    public class TestCase
    {
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
    }
}
