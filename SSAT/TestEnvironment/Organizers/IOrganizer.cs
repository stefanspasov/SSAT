using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TestEnvironment.Organizers
{
    interface IOrganizer
    {
        void Setup(IPAddress ipAddress);
    }
}
