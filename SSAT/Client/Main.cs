using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using SATFUtilities;
using System.Net;
using Newtonsoft.Json;
using TestEnvironment.Executors;
using TestEnvironment;
using TestEnvironment.Entities;
using System.IO;
using System.Runtime.InteropServices;


namespace Client
{
    class MainProcess
    {

        //DONE ON EXIT
        private static bool ConsoleCtrlCheck(Process.CtrlTypes ctrlType) {
            for (int i = 0; i < Constants.TestTechnologies.Count; i++) {
                try {
                    var executor = ExecutorFactory.Instance.Resolve(Constants.TestTechnologies[i]);
                    if (executor != null) {
                        executor.ShutDown();
                    }
                } catch { /*TODO Trace down*/ }
            }
            return true;
        }

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

            Process.HandlerRoutine hr = new Process.HandlerRoutine(ConsoleCtrlCheck);
            GC.KeepAlive(hr);
            Process.SetConsoleCtrlHandler(hr, true);
            for (int i = 0; i < Constants.TestTechnologies.Count; i++) {
                var executor = ExecutorFactory.Instance.Resolve(Constants.TestTechnologies[i]);
                if (executor != null) {
                    executor.StartUp();
                }
            }
            Thread writeScriptThread = new Thread(new ThreadStart(() => WriteScriptThreadHandler(Constants.WRITE_SCRIPT_PORT)));
            writeScriptThread.Start();
            StartFacade();
        }

        private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            AppException.Handle(e.ExceptionObject as Exception);
        }

        public static void WriteScriptThreadHandler(int listeningPort)
        {
            TcpListener listener = new TcpListener(listeningPort);
            TcpClient clientSocket = default(TcpClient);
            TcpClient clientSocketResult = default(TcpClient);
            listener.Start();

            while (true)
            {
                clientSocket = listener.AcceptTcpClient();
                String scriptName = Connectivity.GetData(clientSocket);
                Console.WriteLine("Receiving file: " + scriptName);
                Connectivity.SendData(clientSocket, Constants.FILENAME_RECEIVED_MSG);
                Connectivity.GetFile(clientSocket, scriptName);
                clientSocket.Close();
                clientSocketResult = listener.AcceptTcpClient();
                Process.DecompressFile(Path.Combine(Constants.ZippedScriptFolder, scriptName),  Constants.UnzippedScriptFolderClient);
                Connectivity.SendData(clientSocketResult, Constants.FILE_RECEIVED_MSG);
                Console.WriteLine("File received: " + scriptName);
                clientSocketResult.Close();
            }
        }

        public static void StartFacade() {
            TcpListener listener = new TcpListener(Constants.RUN_SCRIPT_PORT);
            listener.Start();
            while (true) {
                var client = listener.AcceptTcpClient();
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
