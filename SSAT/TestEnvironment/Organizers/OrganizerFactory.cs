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

namespace TestEnvironment.Organizers {
    class OrganizerFactory {
        private OrganizerFactory() { _container = new UnityContainer().LoadConfiguration(); }
        private IUnityContainer _container;
        private static OrganizerFactory _instance;
        public static OrganizerFactory Instance {
            get {
                if (_instance == null) {
                    _instance = new OrganizerFactory();
                }
                return _instance;
            }
        }
        public IOrganizer Resolve(string organizer) {
            var name = Constants.TestTechnologies[organizer] + "Organizer";
            return _container.IsRegistered<IOrganizer>(name) ? _container.Resolve<IOrganizer>(name) : null;
        }
    }
}
