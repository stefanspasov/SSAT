using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestEnvironment.Entities {
    public class UnitTestInfo {
        public List<string> Tests { get; set; }
        public string WorkingDirectory { get; set; }
        public string AssemblyName { get; set; }
    }
}
