using System;
using System.Configuration;
using System.IO;

namespace SATFUtilities
{
    public static class Constants
    {
        public static readonly int RUN_SCRIPT_PORT = Convert.ToInt32(ConfigurationManager.AppSettings["run-script-port"]);
        public static readonly int WRITE_SCRIPT_PORT = Convert.ToInt32(ConfigurationManager.AppSettings["write-script-port"]);
        public static readonly int RESULT_SCRIPT_PORT = Convert.ToInt32(ConfigurationManager.AppSettings["result-script-port"]);
        public static readonly int CALL_SCRIPT_PORT = Convert.ToInt32(ConfigurationManager.AppSettings["call-script-port"]);
        public static readonly int MANUAL_ANSWER_PORT = Convert.ToInt32(ConfigurationManager.AppSettings["manual-script-port"]);
        public const string FILENAME_RECEIVED_MSG = "FILENAME_RECEIVED";
        public const string FILE_RECEIVED_MSG = "FILE_RECEIVED";
        private const string UNZIPPED_SCRIPTS_FOLDER_CLIENT = "Scripts";
        private const string UNZIPPED_SCRIPTS_FOLDER_ORCH = "Scripts";
        private const string ZIPPED_SCRIPTS_FOLDER = "ScriptsZips";
        public const string PYTHON_SERVER_NAME = "sikuli-server.sikuli";
        public const string SETUP_FOLDER = "Setup";
        public static readonly string PROJECT_DIRECTORY = Directory.GetCurrentDirectory();
        public static readonly string SIKULI_IDE_FULL_PATH = ConfigurationManager.AppSettings["sikuli-ide-path"];
        public static readonly string VS_DEV_CMD_PATH = ConfigurationManager.AppSettings["dev-cmd-path"];
        public static readonly string ADEXP_SIM_PATH = ConfigurationManager.AppSettings["adexp-sim-path"];
        public static string UnzippedScriptFolderClient
        {
            get
            {
                return Path.Combine(PROJECT_DIRECTORY, Constants.UNZIPPED_SCRIPTS_FOLDER_CLIENT);
            }
        }
        public static string UnzippedScriptFolderOrchestrator
        {
            get
            {
                return Path.Combine(PROJECT_DIRECTORY, Constants.UNZIPPED_SCRIPTS_FOLDER_ORCH);
            }
        }
        public static string ZippedScriptFolder
        {
            get
            {
                return Path.Combine(PROJECT_DIRECTORY, Constants.ZIPPED_SCRIPTS_FOLDER);
            }
        }
        public static string SetupFolder
        {
            get
            {
                return Path.Combine(PROJECT_DIRECTORY, Constants.SETUP_FOLDER);
            }
        }
    }
}
