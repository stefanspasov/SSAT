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
            get {
                if (_steps == null) _steps = new Queue<Step>();
                return _steps; 
            }
        }
    }
}
