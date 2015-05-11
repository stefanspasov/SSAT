using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SATFUtilities;
using System.IO;
using TestEnvironment.Entities;
using System.Net.Sockets;
using Newtonsoft.Json;

namespace TestEnvironment.Organizers {
    public class SimOrganizer : IOrganizer {
        public void Setup(IPAddress ipAddress) {
            //TcpClient cs = new TcpClient();
            //cs.Connect(ipAddress, Constants.RUN_SCRIPT_PORT);
            //Connectivity.SendData(cs, JsonConvert.SerializeObject(new Operation("<start>", "Sim")));
            //cs.Close();
        }
        public void TearDown(IPAddress ipAddress) {
            //TcpClient cs = new TcpClient();
            //cs.Connect(ipAddress, Constants.RUN_SCRIPT_PORT);
            //Connectivity.SendData(cs, JsonConvert.SerializeObject(new Operation("<stop>", "Sim")));
            //cs.Close();
        }
    }
}
