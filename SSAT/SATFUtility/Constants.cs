using System;
using System.IO;

namespace SATFUtilities
{
    public static class Constants
    {
        public const int RUN_SCRIPT_PORT = 8888;
        public const int WRITE_SCRIPT_PORT = 8890;
        public const int RESULT_SCRIPT_PORT = 8887;
        public const int CALL_SCRIPT_PORT = 8889;
        public const int MANUAL_ANSWER_PORT = 8886;
        public const string FILENAME_RECEIVED_MSG = "FILENAME_RECEIVED";
        public const string FILE_RECEIVED_MSG = "FILE_RECEIVED";
        private const string UNZIPPED_SCRIPTS_FOLDER_CLIENT = "Scripts";
        private const string UNZIPPED_SCRIPTS_FOLDER_ORCH = "Scripts";
        private const string ZIPPED_SCRIPTS_FOLDER = "ScriptsZips";
        public const string PYTHON_SERVER_NAME = "caller.sikuli";
        public const string SETUP_FOLDER = "Setup";
        public static string PROJECT_DIRECTORY = Directory.GetCurrentDirectory();
        public const string SIKULI_IDE_FULL_PATH = "C:\\sikuli\\runIDE.cmd";
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
