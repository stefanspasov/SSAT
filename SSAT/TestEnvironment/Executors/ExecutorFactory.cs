using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestEnvironment.Entities;

namespace TestEnvironment.Executors
{
    public class ExecutorFactory
    {
        private ExecutorFactory() { }
        private static ExecutorFactory _instance;
        public static ExecutorFactory Instance()
        {
            if(_instance == null)
            {
                _instance = new ExecutorFactory();
            }
            return _instance;
        }
        public IExecutor CreateExecutor(TestTechnology executorType)
        {
           // Executor value = (Executor) Enum.Parse(typeof(Executor), executorType);
            switch (executorType)
            {
                case TestTechnology.Cmd:
                    return new CmdExecutor();
                case TestTechnology.Human:
                    return new ManualExecutor();
                case TestTechnology.Sikuli:
                    return new SikuliExecutor();
                case TestTechnology.Sim:
                    return new SimExecutor();
                case TestTechnology.Writer:
                    return new WriteExecutor();
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
