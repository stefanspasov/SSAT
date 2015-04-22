using Newtonsoft.Json;
using SATFUtilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestEnvironment.Entities;

namespace TestEnvironment.Executors {
    public class UnitTestExecutor : IExecutor {
        public string Execute(string source) {
            RunUnitTest(Constants.VS_DEV_CMD_PATH, JsonConvert.DeserializeObject<UnitTestInfo>(source));
            return string.Empty;
        }

        public static void RunUnitTest(string vsDevPath, UnitTestInfo utInfo) {
            var procStartInfo = new ProcessStartInfo("cmd", "/c %comspec% /k \"" + vsDevPath + "\"");
            procStartInfo.RedirectStandardInput = true;
            procStartInfo.RedirectStandardOutput = true;
            procStartInfo.RedirectStandardError = true;
            procStartInfo.UseShellExecute = false;
            procStartInfo.CreateNoWindow = false;
            procStartInfo.WorkingDirectory = utInfo.WorkingDirectory;

            // Build MSTest command from UnitTestInfo
            var cmd = string.Format("MSTest /testcontainer:{0}.dll /resultsfile:{0}_Result_{1}.trx",
                    utInfo.AssemblyName, DateTime.Now.ToString("yyyy-MM-dd_H-mm-ss"));
            if (utInfo.Tests != null && utInfo.Tests.Any()) {
                cmd += string.Join(" ", utInfo.Tests.Select(t => " /test:" + t));
            }
            var proc = System.Diagnostics.Process.Start(procStartInfo);
            proc.StandardInput.WriteLine(cmd);
            proc.StandardInput.WriteLine();
            proc.Close();
        }
    }
}
