using SATFUtilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestEnvironment.Executors
{
    public class SimExecutor : IExecutor
    {
        bool started = false;
        System.Diagnostics.Process SimProcess = null;
        StreamWriter streamWriter = null;
        public string Execute(string source)
        {
            if (source == "<start>")
            {
                if (started == true) return "sim has been started already";
                ProcessStartInfo pi = new ProcessStartInfo();
                pi.RedirectStandardInput = true;
                pi.UseShellExecute = false;
                pi.WorkingDirectory = Constants.ADEXP_SIM_PATH;
                pi.FileName = Constants.ADEXP_SIM_PATH + "/ADEXPSim.exe";
                SimProcess = System.Diagnostics.Process.Start(pi);
                streamWriter = SimProcess.StandardInput;
                streamWriter.WriteLine("call adexp_setup.txt");
                System.Threading.Thread.Sleep(10000);
                streamWriter.WriteLine("stop_wait");
                System.Threading.Thread.Sleep(2000);
                //SimProcess.WaitForExit();
                started = true;
                return "sim started";
            }
            if (source == "<stop>" && started == true)
            {
                streamWriter.WriteLine("quit");
                streamWriter.WriteLine();
                System.Threading.Thread.Sleep(500);
                streamWriter.Close();
                started = false;
                return "sim closed";
            }
            if( started == true)
            {
                streamWriter.WriteLine(source.Split('^')[1]);
                System.Threading.Thread.Sleep(Int32.Parse(source.Split('^')[0]));
                streamWriter.WriteLine("stop_wait");
                System.Threading.Thread.Sleep(2000);
                return "done";
            }
            return "sim not started | failed";
        }
    }
}
