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
        private static CmdExecutor cmdE;
        private static ManualExecutor manE;
        private static SikuliExecutor sikE;
        private static SimExecutor simE;
        private static WriteExecutor wrE;
        private static UnitTestExecutor utE;


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
            switch (executorType)
            {
                case TestTechnology.Cmd:
                    if (cmdE == null)
                    {
                        cmdE = new CmdExecutor();
                    }
                    return cmdE;
                case TestTechnology.Human:
                    manE = new ManualExecutor();// Cannot be singleton
                    return manE;
                case TestTechnology.Sikuli:
                    if (sikE == null)
                    {
                        sikE = new SikuliExecutor();
                    }
                    return sikE;
                case TestTechnology.Sim:
                   if (simE == null)
                    {
                        simE = new SimExecutor();
                    }
                   return simE;
                case TestTechnology.Writer:
                    if (wrE == null)
                    {
                        wrE = new WriteExecutor();
                    }
                    return wrE;
                case TestTechnology.UnitTest:
                    if (utE == null)
                    {
                        utE = new UnitTestExecutor();
                    }
                    return utE;
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
