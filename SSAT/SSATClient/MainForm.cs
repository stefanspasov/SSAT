using Newtonsoft.Json;
using SATFUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestEnvironment.Entities;
using TestEnvironment.Executors;

namespace SSATClient {
    public partial class MainForm : Form {
        private static Thread _writerThread;
        private static Thread _executorThread;
        private static bool _isAlive = true;
        private static TcpListener _writeScriptListener; 
        private static TcpListener _runScriptListener;

        public MainForm() {
            InitializeComponent();
            Shown += MainForm_Shown;
            FormClosing += MainForm_FormClosing;
            Process.HandlerRoutine hr = new Process.HandlerRoutine(OnConsoleClosed);
            GC.KeepAlive(hr);
            Process.SetConsoleCtrlHandler(hr, true);
        }

        void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
            OnExit();
        }

        private static bool OnConsoleClosed(Process.CtrlTypes ctrlType) {
            OnExit();
            return true;
        }

        private static void OnExit() {
            _isAlive = false;
            for (int i = 0; i < Constants.TestTechnologies.Count; i++) {
                try {
                    var executor = ExecutorFactory.Instance.Resolve(Constants.TestTechnologies[i]);
                    if (executor != null) {
                        executor.ShutDown();
                    }
                } catch { /*TODO Trace down*/ }
            }
            if (_writerThread.IsAlive) {
                _writeScriptListener.Stop();
                _writerThread.Abort();
            }
            if (_executorThread.IsAlive) {
                _runScriptListener.Stop();
                _executorThread.Abort();
            }
        }

        void MainForm_Shown(object sender, EventArgs e) {
            for (int i = 0; i < Constants.TestTechnologies.Count; i++) {
                var executor = ExecutorFactory.Instance.Resolve(Constants.TestTechnologies[i]);
                if (executor != null) {
                    executor.StartUp();
                }
            }
            _writerThread = new Thread(new ThreadStart(() => WriteScriptThreadHandler(Constants.WRITE_SCRIPT_PORT)));
            _writerThread.Start();
            _executorThread = new Thread(new ThreadStart(() => StartFacade()));
            _executorThread.Start();
            Visible = false;
        }

        public static void WriteScriptThreadHandler(int listeningPort) {
            _writeScriptListener = new TcpListener(listeningPort);
            TcpClient clientSocket = default(TcpClient);
            TcpClient clientSocketResult = default(TcpClient);
            _writeScriptListener.Start();

            while (_isAlive) {
                clientSocket = _writeScriptListener.AcceptTcpClient();
                String scriptName = Connectivity.GetData(clientSocket);
                Console.WriteLine("Receiving file: " + scriptName);
                Connectivity.SendData(clientSocket, Constants.FILENAME_RECEIVED_MSG);
                Connectivity.GetFile(clientSocket, scriptName);
                clientSocket.Close();
                clientSocketResult = _writeScriptListener.AcceptTcpClient();
                Process.DecompressFile(Path.Combine(Constants.ZippedScriptFolder, scriptName), Constants.UnzippedScriptFolderClient);
                Connectivity.SendData(clientSocketResult, Constants.FILE_RECEIVED_MSG);
                Console.WriteLine("File received: " + scriptName);
                clientSocketResult.Close();
            }
        }

        public static void StartFacade() {
            _runScriptListener = new TcpListener(Constants.RUN_SCRIPT_PORT);
            _runScriptListener.Start();
            while (_isAlive) {
                var client = _runScriptListener.AcceptTcpClient();
                string response;
                try {
                    var operationStr = Connectivity.GetData(client);
                    Console.WriteLine("Executing operation: " + operationStr);
                    var operation = JsonConvert.DeserializeObject<Operation>(operationStr);
                    IExecutor executor = ExecutorFactory.Instance.Resolve(operation.Executor);
                    response = executor.Execute(operation.Directive);
                    Console.WriteLine("Operation executed: " + operationStr);
                } catch (Exception ex) {
                    response = "The operation has been failed with the following exception:\r\n"
                               + ex.Message + "\r\nStack Trace:\r\n" + ex.StackTrace;
                }
                Connectivity.SendData(client, response);
                client.Close();
            }
        }
    }
}
