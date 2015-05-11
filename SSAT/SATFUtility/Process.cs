using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.IO;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Configuration;

namespace SATFUtilities
{
    public static class Process
    {

        public static System.Diagnostics.Process StartSikuliServer(string serverFullPath)
        {
            //try
            //{
            //    Console.WriteLine("Starting Sikuli server...");
                string command = Constants.SIKULI_IDE_FULL_PATH + " -r \"" + serverFullPath + "\"";
                return StartProcessAsAdmin(command);
            //    Console.WriteLine("Sikuli server started.");
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("Fail to start Sikuli server. Unexpected behaviors might occur.\r\n" + ex.Message + "\n\nStack Trace:\n" + ex.StackTrace);
            //}
        }

        public static System.Diagnostics.Process StartProcessAsAdmin(String command)
        {
            System.Diagnostics.ProcessStartInfo procStartInfo = new System.Diagnostics.ProcessStartInfo("cmd", "/c " + command);
            procStartInfo.UseShellExecute = true;
            procStartInfo.CreateNoWindow = true;
            procStartInfo.Verb = "runas";
            // Reason for commenting out the following code:
            // Using administrator's credentials doesn't work on Windows 7
            // Change the solution to "runas" instead.
            // 
            //procStartInfo.RedirectStandardOutput = true;
            //procStartInfo.UserName = ConfigurationManager.AppSettings["username"];
            //procStartInfo.Password = SATFUtilities.Security.MakeSecureString(ConfigurationManager.AppSettings["password"]);
            System.Diagnostics.Process proc = new System.Diagnostics.Process();

            proc.StartInfo = procStartInfo;
            proc.Start();
            return proc;
        }

        public static void CompressFolder(string originFolderPathFull, string destinationFolderPathFull,  bool includeBaseFolder)
        {
            string zippedFilePath = Path.Combine(destinationFolderPathFull, Path.GetFileName(originFolderPathFull) + ".zip");
            if (File.Exists(zippedFilePath))
            {
                File.Delete(zippedFilePath);
            }
            if (!Directory.Exists(destinationFolderPathFull))
            {
                Directory.CreateDirectory(destinationFolderPathFull);
            }
            ZipFile.CreateFromDirectory(originFolderPathFull, zippedFilePath, CompressionLevel.Fastest, includeBaseFolder);
        }

        public static void DecompressFile(string zipFileFullName, string destinationFolderFull)
        {
            if (!Directory.Exists(Path.GetPathRoot(destinationFolderFull)))
            {
                Directory.CreateDirectory(Path.GetPathRoot(destinationFolderFull));
            }
            using (ZipArchive zipFile = ZipFile.OpenRead(zipFileFullName))
            {
                foreach (var entry in zipFile.Entries)
                {
                    if (Path.GetFileName(entry.FullName) == "Thumbs.db") { continue; }
                    var dir = Path.GetDirectoryName(Path.Combine(destinationFolderFull, entry.FullName));
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                    entry.ExtractToFile(Path.Combine(destinationFolderFull, entry.FullName), true);
                }
            }
        }

        [DllImport("Kernel32")]
        public static extern bool SetConsoleCtrlHandler(HandlerRoutine Handler, bool Add);

        //delegate type to be used of the handler routine
        public delegate bool HandlerRoutine(CtrlTypes CtrlType);

        // control messages
        public enum CtrlTypes
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT,
            CTRL_CLOSE_EVENT,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT
        }
    }
}
