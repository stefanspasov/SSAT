using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using SATFUtilities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestEnvironment.Entities;

namespace TestEnvironment.Executors {
    public class ExecutorFactory {
        private ExecutorFactory() { _container = new UnityContainer().LoadConfiguration(); }
        private IUnityContainer _container;
        private static ExecutorFactory _instance;
        public static ExecutorFactory Instance {
            get {
                if (_instance == null) {
                    _instance = new ExecutorFactory();
                }
                return _instance;
            }
        }
        public IExecutor Resolve(string executorType) {
            var name = Constants.TestTechnologies[executorType] + "Executor";
            return _container.IsRegistered<IExecutor>(name) ? _container.Resolve<IExecutor>(name) : null;
        }
    }
}
