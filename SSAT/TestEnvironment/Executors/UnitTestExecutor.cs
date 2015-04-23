using Newtonsoft.Json;
using SATFUtilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestEnvironment.Entities;

namespace TestEnvironment.Executors {
    public class UnitTestExecutor : IExecutor {
        public string Execute(string source) {
            var utInfo = JsonConvert.DeserializeObject<UnitTestInfo>(source);
            var resultFileName = RunUnitTest(Constants.VS_DEV_CMD_PATH, utInfo);
            return File.ReadAllText(Path.Combine(utInfo.WorkingDirectory, resultFileName));
        }

        public static string RunUnitTest(string vsDevPath, UnitTestInfo utInfo) {
            var procStartInfo = new ProcessStartInfo("cmd", "/c %comspec% /k \"" + vsDevPath + "\"");
            procStartInfo.RedirectStandardInput = true;
            procStartInfo.RedirectStandardOutput = true;
            procStartInfo.RedirectStandardError = true;
            procStartInfo.UseShellExecute = false;
            procStartInfo.CreateNoWindow = false;
            procStartInfo.WorkingDirectory = utInfo.WorkingDirectory;

            string resultFileName = string.Format("{0}_Result_{1}.trx",
                    utInfo.AssemblyName, DateTime.Now.ToString("yyyy-MM-dd_H-mm-ss"));

            // Build MSTest command from UnitTestInfo
            var cmd = string.Format("MSTest /testcontainer:{0}.dll /resultsfile:{1}",
                    utInfo.AssemblyName, resultFileName);
            if (utInfo.Tests != null && utInfo.Tests.Any()) {
                cmd += string.Join(" ", utInfo.Tests.Select(t => " /test:" + t));
            }
            var proc = System.Diagnostics.Process.Start(procStartInfo);
            proc.StandardInput.WriteLine(cmd);
            proc.StandardInput.WriteLine();
            var x = proc.StandardOutput.ReadLine();
            proc.Close();
            return resultFileName;
        }
    }
}
