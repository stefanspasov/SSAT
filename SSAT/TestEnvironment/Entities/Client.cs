using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TestEnvironment.Entities
{
    [Serializable]
    public class Client
    {
        private IPAddress _ipAddress;
        [XmlIgnore]
        public IPAddress IpAddress
        {
            get { return _ipAddress; }
            set { _ipAddress = value; }
        }
        private string _name;
        [XmlAttribute]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public Client() { }
    }
}
