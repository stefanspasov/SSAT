using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestEnvironment.Entities;

namespace TestEnvironment.Organizers
{
    class OrganizerFactory
    {
        private OrganizerFactory() { }
        private static OrganizerFactory _instance;
        public static OrganizerFactory Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new OrganizerFactory();
                }
                return _instance;
            }
        }

        public IOrganizer CreateOrganizer(TestTechnology organizer)
        {
            switch (organizer)
            {
                case TestTechnology.Sikuli:
                    return new SikuliOrganizer();
                case TestTechnology.Cmd:
                    
                case TestTechnology.Sim:
                    
                case TestTechnology.Writer:

                case TestTechnology.Human:
                    return null;
                   
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
