using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SATFUtilities;
using System.IO;

namespace TestEnvironment.Organizers
{
    public class SikuliOrganizer : IOrganizer
    {
        public void Setup(IPAddress ipAddress)
        {
            Process.CompressFolder(Path.Combine(Constants.SetupFolder, "Sikuli"), Constants.ZippedScriptFolder, false); 
            Connectivity.SendFileWithMetadata(ipAddress, Path.Combine(Constants.ZippedScriptFolder, "Sikuli.zip"));
        }
    }
}
