using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestEnvironment.Executors
{
    public interface IExecutor
    {
        string Execute(string source);
    }
}
