using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;
using SATFUtilities;

namespace TestEnvironment.Executors {
    public class SikuliExecutor : IExecutor {
        public string Execute(string source) {
            TcpClient clientSocket = new TcpClient();
            clientSocket.Connect(IPAddress.Loopback, Constants.CALL_SCRIPT_PORT);
            Connectivity.SendData(
                clientSocket,
                Path.Combine(Constants.UnzippedScriptFolderClient, source, (source.EndsWith(".sikuli") ? source.Substring(0, source.Length - ".sikuli".Length) : source) + ".py*"));
            clientSocket.Close();
            if (source != "<stop>") {
                return Connectivity.WaitForAnswer(Constants.RESULT_SCRIPT_PORT);
            }
            return "stopped";
        }

        public void StartUp() {
            Process.StartSikuliServer(Path.Combine(Constants.UnzippedScriptFolderClient, Constants.PYTHON_SERVER_NAME));
        }

        public void ShutDown() {
            Execute("<stop>");
        }
    }
}
