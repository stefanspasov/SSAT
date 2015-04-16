using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TestEnvironment.Entities
{
   public class Client
    {
        private IPAddress _ipAddress;

        public IPAddress IpAddress
        {
            get { return _ipAddress; }
            set { _ipAddress = value; }
        }
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public Client(string ipAddress, string name)
        {
            this._ipAddress = IPAddress.Parse(ipAddress);
            this._name = name;
        }

        public Client(IPAddress ipAddress, string name)
        {
            this._ipAddress = ipAddress;
            this._name = name;
        }
    }
}
