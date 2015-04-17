using System.Collections.Generic;
using TestEnvironment.Entities;

namespace Orchestrator
{
    interface ITestAccess
    {
        IList<TestCase> LoadTestCases();
    }
}
