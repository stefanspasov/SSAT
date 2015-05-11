using SATFUtilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestEnvironment.Executors {
    public class SimExecutor : IExecutor {
        bool _started = false;
        System.Diagnostics.Process _simProcess = null;
        StreamWriter _streamWriter = null;
        public string Execute(string source) {
            if (source == "<stop>") {
                ShutDown();
                return "sim stopped";
            }
            if (source == "<start>" && _started == true) return "sim has been started already";
            if (!_started) {
                StartUp();
                if (source == "<start>") return "sim started";
            }
            if (source.Contains('^')) {
                _streamWriter.WriteLine(source.Split('^')[1]);
                System.Threading.Thread.Sleep(Int32.Parse(source.Split('^')[0]));
            } else {
                _streamWriter.WriteLine(source);
                System.Threading.Thread.Sleep(1000);
            }
            return "Done with the following command: " + source;
        }

        public void StartUp() {
            if (!_started) {
                ProcessStartInfo pi = new ProcessStartInfo();
                pi.RedirectStandardInput = true;
                pi.UseShellExecute = false;
                pi.WorkingDirectory = Constants.ADEXP_SIM_PATH;
                pi.FileName = Constants.ADEXP_SIM_PATH + "/ADEXPSim.exe";
                _simProcess = System.Diagnostics.Process.Start(pi);
                _streamWriter = _simProcess.StandardInput;
                _streamWriter.WriteLine("call adexp_setup.txt");
                System.Threading.Thread.Sleep(7000);
                _started = true;
            }
        }

        public void ShutDown() {
            if (_started) {
                _streamWriter.WriteLine("quit");
                _streamWriter.WriteLine();
                System.Threading.Thread.Sleep(500);
                _streamWriter.Close();
                _simProcess.CloseMainWindow();
                _started = false;
            }
        }
    }
}
